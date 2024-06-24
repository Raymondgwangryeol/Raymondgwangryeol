# **Pytorch Troubleshooting**
## **OOM(Out Of Memory)**
해결하기 어려운 이유
- 왜, 어디서 발생했는지 파악 어려움
- Error Backtracking이 이상한데로 감
- 메모리 이전 상황의 파악이 어려움
  - iteration을 돌면서 OOM이 많이 일어나기 때문. 이런 경우 메모리 상황을 지속적으로 알기 어려움.

제일 쉽고 편한 방법(잘 짜진 코드라는 전제 하에)    
Batch Size 줄이고 → GPU Clean → Run    
잘 짜진 코드라면 대부분 이렇게 해도 해결이 된다.   

그 외 발생할 수 있는 문제들을 Troubleshooting 하는 방법    
### **GPUtil 사용**
- nvidia-smi 처럼 GPU의 상태를 보여주는 모듈
  - nvidia-smi는 Snapshot은 보여줄 수 있지만, GPUtil처럼 iteration이 돌아가면서 모메리에 쌓이는 건 보여주지 못함.
- Colab에서 GPU 확인이 편함
- Iter마다 메모리가 늘어나는지 확인
  - 메모리가 늘어난다면 메모리 어딘가에 데이터가 계속 잘못 쌓이고 있는 것
```python
!pip install GPUtil

import GPUtil
GPUtil.show.Utilization()
```
<br><br>

**❓퀴즈**
### **torch.cuda.empty_cache() 사용**
- <mark>사용되지 않은 GPU상 Cache를 정리.</mark>
  - 예를 들면 CNN에서 메모리 버퍼를 만든 다음 backward가 될 때 마다 메모리에 저장되는데, 한 번 쓰고 다시 쓰이진 않는데 쌓여있게 됨
- 가용 메모리를 확보
- del과 구분 필요
  - del은 관계를 끊는 것(free)
- reset 대신 쓰기 좋은 함수
- **[추천]** loop가 시작하기 전 한 번 써주면, 이전 학습에 사용됐던 메모리이 영향을 덜 받을 수 있음.
```python
import torch
from GPUtil import showUtilization as gpu_usage

print('Initial GPU Usage')
gpu_usage()

tensor_text=[]
for x in range(10):
    tensorList.append(torch.randn(100000000, 10).cuda())

print('GPU Usage after allocating a bunch of Tensors')
gpu_usage()
del tensorList # free()후 Garbage Collectior에 의해 처리가 되어야 가용 용량이 줆
print('GPU Usage after deleting Tensors')
gpu_usage()
print('GPU Usage after emptying the cache')
torch.cuda.empty_cache()
gpu_usage()
```
<br>

## **Troubleshooting 확인 사항들**
1. **Training loop에 tensor로 축적되는 변수 확인**
   - tensor로 처리된 변수는 GPU상 메모리 사용.
     - required_grad=True면 메모리 버퍼 상에도 올라가서 더 많은 메모리를 사용
   - 해당 변수가 loop 안 연산에 있을 때 GPU에 Computational graph를 생성
     - 사용되지 않는 메모리가 해제되지 않고 계속 차지하면 메모리 누수 발생
   - 1d tensor의 경우 python 기본 객체로 변환하여 처리
  ```python
  total_loss=0

  for x in range(10):
      iter_loss=torch.randn((3,4)).mean()
      iter_loss.requires_grad=True.mean()
      total_loss += iter_loss.item() #item()을 붙이거나, float(iter_loss)로 기본객체로 반환
  ```
2. **del 명령어의 적절한 사용**
   - 파이썬의 메모리 배치 특성상 loop이 끝나도 메모리를 차지. 필요가 없어진 변수는 적절한 삭제가 필요
3. **가능 batch 사이즈 실험해보기**
   - 학습시 OOM이 발생했다면 batch size를 1로 해서 실험.
```python
   oom = False

   try:
       run_model(batch_size)
   except RuntimeError: #OOM
       oom=True

   if oom:
       for i in range(batch_size):
           run_model(1)

```

4. **❓퀴즈: torch.no_grad() 사용**
- <mark>Inference 시점에서는 torch.no_grad()구문을 사용</mark>
- backward pass로 인해 쌓이는 메모리에서 자유로움
```python
with torch.no_grad():
   for data, target in test_loader:
        output = network(data)
        test_loss += F.nll_loss(output, target, size_average=False).item()
        pred = output.data.max(1, keepdim=True)[1]
        correct += pred.eq(target.data.view_as(pred)).sum()
```
5. **그 외**
   - colab에서 너무 큰 사이즈는 실행하지 말 것(linear, CNN, LSTM)
   - CNN의 대부분 에러는 크기가 안 맞아서 생김(torchsummary 등으로 사이즈를 맞출 것)
   - tensor의 float precision을 16bit로 줄일 수도 있음
