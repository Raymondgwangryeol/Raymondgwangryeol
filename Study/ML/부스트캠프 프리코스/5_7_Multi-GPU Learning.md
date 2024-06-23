# **Multi-GPU 학습**
오늘날의 딥러닝은 점점 더 대용량의 데이터를 다루고 있기 때문에, GPU 확보가 중요해지고 있다.

### **개념 정리**
- Single GPU vs Multi GPU
- GPU vs Node
  - Node는 한 대의 컴퓨터(또는 시스템)이라고 생각하면 된다.
- Single Node Single GPU
  - 한 대의 컴퓨터 한 개의 GPU
- Single Node Multi GPU
  - 한 대의 컴퓨터 여러개의 GPU
  - 주로 이 상황을 많이 겪음
- Multi Node Multi GPU
  - 여러대의 컴퓨터 여러개의 GPU
  - 궁극적인 방향

## **Multi-GPU 학습의 방법**
모델 나누기(병렬화), 데이터 나누기
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/87c1a333-ebc2-484d-a6e6-f3422bc06939)
<br>

### **Model parallel**
**❓퀴즈**    
다중 GPU에 학습을 분산하는 2가지 방법 중, 모델을 나누는 방식은 무엇이라 불리는가?    
**🖊 정답:** Model parallel    
- 생각보다 예전부터 쓴 방법.(AlexNet)
- 모델의 병목, 파이프라인의 어려움 등으로 인해 모델 병렬화는 고난이도 과제

### **Model parallel의 어려움**
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/2b7cadb7-6310-4299-9e75-e30e56dd8f4e)
<br>

```python
class ModelParallelResNet50(ResNet):
    def __init__(self, *args, **kwargs):
        super(ModelParallelResNet50, self).__init__(Bottleneck, [3,4,6,3], num_classes=num_classes, *args, **kwargs)

        self.seq1 = nn.Sequential(#첫 번째 모델을 cuda 0에 할당
                    self.conv1, self.bn1, self.relu, self.maxpool, self.layer1, self.layer2).to('cuda:0')
    
        self.seq2 = nn.Sequential(
                    self.layer3, self.layer4, self.avgpool).to('cuda:1') # 두 번째 모델을 cuda 1에 할당
    
        self.fc.to('cuda:0')

   def forward(self, x):  # 두 모델을 연결하기
        x = self.seq2(self.seq1(x).to('cuda:1')) # .to() -> copy해서 보내준다.
        return self.fc(x.view(x.size(0), -1))
# 이렇게만 구현하면 병렬화가 잘 안 된다. (병목현상 발생 가능)
```

### **Data parallel**
- 데이터를 나눠 GPU에 할당 후 결과의 평균을 취하는 방법.
- Minibatch와 유사, 한 번에 여러 GPU에서 수행
  - Minibatch의 병렬화를 시킨다고 이해하면 편하다.
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/b8eb04d7-ace8-4649-a2df-9bb86f06903e)
<br>

PyTorch에서는 두 가지 방식의 Data parallel을 제공.
- **DataParallel**
  - 단순히 데이터를 분배한 후 평균을 취함.
  - GPU 사용 불균형 문제 발생(하나의 GPU에 할 일을 전가 -> GPU 폭발)
  - 해당 GPU에 맞춰서 Batch Size 감소, 중간에 Coordinate하는 GPU가 많은 메모리를 차지하기 때문에 한 GPU가 병목, 데이터 분산 불균형 발생, GIL
      - **❓퀴즈** 데이터 분산 불균형
        - 한 GPU가 병목이 될 때 다른 GPU들도 그 병목 때문에 영향을 받게 되는 현상
      - GIL(Global Interpreter Lock)
        - 스레드끼리 공유하는 프로세스의 자원을 global하게 lock해버리고, 단 하나의 스레드에만 이 자원에 접근하는 것을 허용.
        - 멀티스레드라 하더라도 한 번에 하나의 스레드만 실행
```python
parallel_model = torch.nn.DataParallel(model)
```
<br>

- **DistributeDataParallel**
  - **❓퀴즈** 각 CPU마다 Process 생성하여 개별 GPU에 할당
    - 모으지 않아도 각자 Coordinate 가능. 따로 처리 후 나중에 평균치를 반영하는 방식.
```python
train_sampler=torch.utils.data.distributed.DistributedSampler(train_data) # DataLoader의 Sampler 사용
                                                                          # Sampler: index를 컨트롤ultiprocessign 하는 방법.
                                                                          # shuffle=False여야 함. (직접 인덱스를 컨트롤하기 때문)
                                                                          # DistributedSampler를 사용해야 학습데이터가 각각의 프로세스에 균일하게 배분됨.
shuffle = False
pin_memory = True

trainloader = torch.utils.data.DataLoader(train_data, batch_size=20, shuffle=shuffle, pin_memory=pin_memory, num_workers=3, sampler=train_sampler)

def main():
    n_gpus = torch.cuda.device_count()
    torch.multiprocessing.spawn(main_worker, nprocs=n_gpus, args=(n_gpus))

def main_worker(gpu, n_gpus):
    image_size = 224
    batch_size = 512
    num_worker = 8
    epochs = ...

    batch_size = int(batch_size/n_gpus) # gpu 갯수만큼 나눠줘야 함
    num_worker = int(num_worker/n_gpus)

    torch.distributed.init_process_group(backend='nccl', init_method='tcp://127.0.0.1:2568', world_size=n_gpus, rank=gpu) # 멀티프로세싱 통신 규약 정의
    model=MODEL
    torch.cuda.set_device(gpu)
    model=model.cuda(gpu)
    model=torch.nn.parallel.DistributedDataParallel(model, device_ids=[gpu]) # DistributedDataParallel 정의
```
