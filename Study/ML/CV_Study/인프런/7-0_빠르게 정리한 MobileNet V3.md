      참고 영상: https://www.youtube.com/watch?v=JPs2Uy9DLO8
      MnasNet 자료: https://ech97.tistory.com/entry/MnasNet
# MobileNet V3

## Property
- 효율적인 모델 구성
- Compact 하면서도 높은 Accuracy
- Lower latency
- 연산이 줄어들기 때문에, help preserve battery life through reduced power consumption

## Abstract
- AutoML적인 관점을 차용, Structure를 Searching & Tunning
- 자동화된 탐색 알고리즘인 **Network Search** 부분과 인간이 개입해 redesign해서 최적화하는 **Network Improvements** 부분으로 나눌 수 있음
- **Network Search**
  - Use NAS for Block-wise search(platform viewpoint)
  - Use NetAdapt for Layer-wise search
- **Network Improvements**
  1. Redesining expensive layers
  2. 더 좋은 Non-linearities 사용
  3. MnasNet 구조 차용, squeeze-and excite block
  4. Large/Small
- 배포를 위한 두 가지 모델. (Network Improvements)
  - Large: 연산 속도 상대적으로 느림. 무겁고 정확도 높음
  - Small: 연산 속도 상대적으로 빠름. 가볍고 정확도 낮음
- 이런 모델들은 Object Detection, Semantic Segmentation등 다양한 작업에 적용됨.
- MobileNetV2보다 MobileNet-Large가 ImageNet분류에서 3.2% 더 정확, 지연 시간 20% 감소
- MobileNetV3-Small은 MobileNetV2랑 지연 시간은 비슷, 6.6% 더 정확

## Introduction
목표 달성을 위해 도입, 소개된 것들    
1. 상호 보완 탐색 기술들(Complementary Search Techniques)
2. 모바일 환경에 실용적인 새롭고 효율적인 함수
3. 새롭고 효율적인 Network 디자인 (설계)
4. 새롭고 효율적인 Segmentic decoder LR-ASPP(분할 해독기)

### MnasNet(Automated Mobile Neural Architecture Search)
모바일에서 사용가능한 구조를 자동으로 찾는 것. Accuracy 뿐 아니라 Latency도 같이 고려.    
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/8a8f7ff4-652b-4ec6-9a79-6af65620be70)       
- 편리하고 Reward를 customizing하기 편한 방식의 강화학습을 통해 Multi-object(Accuracy, Latency)Search Problem을 해결.
- CNN Model이 매개변수 θ를 기반으로 강화학습한 Agent의 Action a<sub>1:t</sub>의 Sequence에 의해 결정된 token들들의 list에 mapping
  - Agent: 강화 학습 환경에서 행동을 취하는 주체
    - 다양한 아키텍처를 탐색하고 선택하는 알고리즘 또는 모델
  - Action: agent가 환경에서 취하는 행동
    - 특정 레이어의 커널 크기, 채널 수, stride등을 선택하는 행위
  - Token: 상태나 행동을 표현하는 단위
    - 특정 레이어의 타입, 크기, 파라미터 등 네트워크 아키텍처의 구성요소를 나타냄.
#### Sample-eval-update Loop
- Controller가 θ를 사용, RNN의 Softmax logits를 기반으로 Token의 sequence를 예측하여, Model의 묶음들을 sampling(일부 모델을 선택).
- 각 Sampling된 모델 m에 대해 Training, 정확도 ACC(m)과 지연시간 LAT(m)을 구함.
- 다음 수식으로 Reward R(m) 계산      
  ![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/c23e7f2c-8b42-47d3-b13a-ad38095586c9)    
  - 단순히 Latency가 T(Target latency)보다 작을 때 가장 높은 ACC를 보이는 모델을 고르면, 이것만으로는 Pareto Optimal을 달성할 수 없음.
    ACC가 커지면 Reward가 커지고, w는 음수이기 때문에 LAT가 커지면 Reward가 작아짐.
- 각각의 step의 마지막에서 Reward를 최대화 하는 방향으로 θ를 Proximal Policy Optimization을 사용하여 update
- 최대 step 수 또는 최적의 θ에 도달할 때 까지 loop 반복   
  ![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/35f48350-7427-4a98-9092-36dfc8d906dd)   

#### **Squeeze and Excitation Block(from SENet) 사용**
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/4dce142a-4039-425f-9292-3e1bcbe3b04b)    

Parameter를 정해놓고 튜닝을 해 나가야 하기 때문에 bottleneck structure에 Squeeze and Excitation Block 사용

## MobileNet V3 - Network Search
### Platform - Aware NAS for Block-wise Search
- MnasNet 모델 적용, 목표값도 비슷하게 설정함
- 대신 reward weight 값을 다르게 설정했음.
  - w = - 0.15 (vs the original w = -0.07)
    - Accuracy가 Latency보다 더 변화의 폭이 컸다.(for small models)
    - 그래서 다른 latencies에 대한 더 큰 accuracy 변화를 상쇄하기 위해 w값을 더 작게 설정했음.
### NetAdapt for Layer-wize Search
- 좀 더 세부적. fine-tunning을 individual layer 단위로 적용.
- **Steps**
  - NASNet이 seed network architecture로 플랫폼에 기반한 latancy와 Accuracy를 찾음.
  - For each step
    - Generate a set of new proposals. 전 단계 보다 설정된 최소한의 latenct로 감소하는 것을 목표로 함.
    - Each proposal을 파인튜닝(for T), 각각 Proposal들의 Accuracy를 구하게 되고, 이를 estimate
    - metric(측정 기준)에 따라 가장 좋은 performence를 보여준 proposal를 찾음.   
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/9544263e-3143-47b8-8b7a-69ecc3c8eff2)   
  - Target latency에 도달할 때 까지 반복   

## MobileNet V3 - Network Improvements
### Redesigning Expensive Layers
- Expensive한 layer들이 Network의 처음과 끝 부분에 있음     
- MobileNet V2의 inverted bottleneck 구조를 변경     
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/4cec895a-4750-4502-99ea-26bd06ec9a54)
- Projection and filtering layer부분 (3X3 Conv, 1X1 Conv 부분)이 더 이상 필요가 없었다
- 대신 Average Pool을 앞쪽에 끌어다 놓았더니 더 효율적이었음.

#### The inital set of filters
원래는 32filter를 3X3 Conv full 연산 해서 사용했는데, 이를 절반으로 줄인 16개의 filter를 사용. ReLU와 Swish 함수를 썼더니 필터를 32개 썼을 때와 동일한 Accuracy가 나옴.   
(원래는 nonlinearities로 ReLU만 사용했음)

#### Swish 함수(Nonlinearities)
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/a8bf0450-dea8-4588-afdf-249d2741b28b)   
Swish 함수는 x에 시그모이드를 곱한 형태를 가지고 있는데, 모바일 디바이스에서 시그모이드 함수를 연산하기에는 expensive하다.      
h-swish(hard)는 sigmoid 함수를 사용하지 않으면서, sigmoid와 거의 비슷한 모양의 그래프를 가짐.    
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/1c0f2d5e-015c-4aba-ab28-63e7bdc4dca5)    

sigmoid 대신 계산하기 쉬운 ReLU를 사용.     
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/2c2d5021-fa25-4b3c-a1d5-daab3f836d43)     
h-swish 함수를 썼을 때 모바일 디바이스에서도 잘 돌아갔고, 정확도도 높았음.     

원래는 다 ReLU를 썼었는데, 그 중 모델의 절반을 h-swish로 대체함.     
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/cb343fe0-9c81-4ca0-aafb-7b286c7a6c57)
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/f68bc214-2576-40b9-a31a-c25d7a7a1d1e)
    

### Large squeeze-and-excite
MNASNet에서는 squeeze-and-excite bottleneck의 size가 Convolutional bottleneck size와 연관이 있었는데, size를 고정된 수인 expension layer 채널의 1/4에 해당하는 값으로 고정함.     
Accuracy 증가, 적당한 파라미터 증가, 별 차이 없는 latency cost 기록

### MobileNet V3 Definitions
- Large/Small
- high/low resoure를 타겟팅
- trade-off가 있는 inference와 accuracy에 대해 벤치마킹을 하기 위함.
