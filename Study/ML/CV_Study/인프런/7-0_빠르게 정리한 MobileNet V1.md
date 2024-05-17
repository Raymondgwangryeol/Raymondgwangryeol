    참고 영상 & 슬라이드: https://www.youtube.com/watch?v=JuEeRdRG_2E
# MobileNet V1
- 모바일 또는 임베디드 기기와 같은 소형 컴퓨터에 적합하도록 디자인 된 딥러닝 모델   
- 최대한 성능을 보존하면서 컴퓨팅 속도를 빠르게 함을 목표  
- 정확성 보다는 효율성(Efficiency) 관점     
- 즉, 파라미터 수를 많이 줄이면서도 성능을 보존하는 네트워크를 제안      
- 엄청 최고는 아니더라도 소형 컴퓨터에서 충분히 높은 성능, 빠른 컴퓨팅 속도, 낮은 전력 소모, 사이즈가 작은 모델 사용.
- MobileNets의 핵심: **Depthwise Separable Convolution**     
     - Depthwise Convolution(공간적 방향) Pointwise Convolution(채널 방향) 을 조합, 연산량을 줄인다

## Standard Convolution
![IMG_1531](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/de83b2ca-13d3-4264-b7ac-5bbc78affc09)
   
- Dk: 필터 크기
- M: 채널 수
- N: 필터 수
- Df: Output image 크기
## Depthwise Convolution
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/bcd17f60-f0d9-4ed3-b6a3-d910d3f3bd2e)    

- 채널마다 따로 필터를 학습시키는 방법
- 이미지의 공간적인 차원(폭과 높이)을 고려하여 컨볼루션 연산을 수행
- 각 Single 채널에 대해서만 Convolution을 학습
- 때문에 입력, 출력 모두 똑같은 채널 수를 가지게 됨. 즉, 차원 감소(Depth Reduction)가 일어나지 않음
- 각각의 채널에 대해서 연산하기 때문에, 채널 수를 곱할 필요가 없음.
  
	  Dk * Dk * M * Df * Df

## Pointwise Convolution
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/972dfc8e-5b82-40ac-9536-d79d034f938d)      

- 필터의 크기가 1 x 1로 고정된 Convolution layer.
- 필터 크기가 1x1이기 때문에, input에 대한 공간적인 feature은 추출하지 않은 상태로 각 채널에 대한 연산을 진행
- Output의 크기는 변하지 않고, 채널의 크기는 자유롭게 조절 가능.
- 일반적으로 이런 Pointwise Convolution은 차원 축소를 위해 많이 사용되는데 , 덕분에 연산량을 많이 줄여줄 수 있음.
- 채널수 * 필터수 * 이미지 사이즈

       M * N * Df * Df

## Depthwise Separable Convolution
![tempsnip](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/d26f5b0f-4faf-41ac-9681-8dedf3520b81)      
   

	  Dk * Dk * M * Df * Df + M * N * Df * Df
전에는 공간적 방향, 채널 방향을 동시에 고려해야 했다면, 해당 방식은 둘을 분리해서 따로따로 계산하겠다는 아이디어.   

두 방향 모두 고려를 하기 때문에 기존 Convolution과 유사하게 동작하지만, 파라미터의 수와 연산량은 훨씬 적다   

각 채널 별 공간적 Convolution을 한 이후에, 생성된 Feature끼리  Linear Combination(공간적 - Vector, 채널 - Scalar)을 해주는 것으로 정리할 수 있음    

공간적 컨볼루션은 입력 이미지의 공간 정보를 캡처하고 특징을 추출하는 데 중점을 두며, 채널 컨볼루션은 입력 이미지의 채널 간의 관계를 모델링하고 특정 채널의 특성을 강조하는 데 사용    


![IMG_1530](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/e7dc71bb-829a-4088-86a4-1c078d41be49)
   


<br><br>

추가적으로 MoblieNets에서는 컴퓨팅 효율을 높이기 위해 다음의 2가지 제안을 추가로 하고 있음.
1. **Width multiplier**
	- 입력 채널의 수를 α배 만큼 축소
	- 입력 채널이 원래 64개이고 α가 0.5면 축소된 입력 채널 개수는 32개

2. **Resolution multiplier**
	- 해상도를 ρ배 만큼 축소함
	- 입력 영상의 해상도: 244 * 244, ρ=0.571이면 축소된 해상도는 128*128


## 구조
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/17d4885d-8c30-42c5-b097-970f3060a6fa)    

처음부터 DepthWise Convolution을 하는게 아니라, 처음에는 정보 손실을 피하고자 일반적인 Standard Convolution 연산을 수행.
먼저 충분히 feature 선택이 이뤄진 후에 Depthwise Convolution을 진행.     
s2는 stride=2라는 뜻

그 다음 레이어부터 Depthwise Convolution 진행.

총 28 layer 연산 수행

마지막으로 average pooling, fully connected layer를 거치고 softmax로 classification

## 성능 분석
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/feeb0b34-256e-46fa-8c7b-96d4db3b5a76)     

전체 연산량에서 pointwise convolution이 94.86퍼센트를 차지. 모든 feature map에 대해 1x1연산을 전부 수행하기 때문.

일반적인 Convolution의 정확도는 71.7, 모바일 넷은 70.6으로 정확도는 좀 떨어지지만, 연산량은 9배 정도가 감소    
  
#### **Narrow vs Shallow**(layer 수가 깊을수록 좋은지 얕을수록 좋은지)
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/4062f7c7-1e67-44d9-bab0-f765888ef455)    
깊었을때가 얕을 때 보다 3%정도 성능이 더 좋았다

#### **Width Multiplier**
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/f635ed52-7770-4bc1-9350-1126fc386a6a)    

채널 수를 줄일수록 성능은 다소 떨어지지만 파라미터 수 역시 감소해서 보다 적은 연산을 수행

#### **Resolution Multiplier**
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/3c1e6877-5b6d-4819-9421-69ee85231969)   

해상도가 떨어질수록 성능은 줄어들지만 연산량 역시 줄어듦

각각 4개의 α과 ρ값의 실험 결과연산량이 적어질수록 정확성이 linear하게 떨어지는 것을 확인할 수 있음.
<br><br>

![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/f9cfbad4-d44a-4c22-b691-f7f54ab03438)

그럼에도 GoogleLeNet보다 성능도 좋고 연산량도 적었음.    
VGG보다는 정확성은 떨어지지만 연산량은 매우 줄었음    
SqueezeNet과 AlexNet보다 성능은 더 좋으면서 연산량은 획기적으로 줄였음

위치 인식에 MobileNet을 사용했을때, 성능이 좋았다

## 정리
Classification, Object Detection, Localization 등 여러 활용 분야에 MobileNet을 적용할 수 있고, 모델이 가벼워 모바일 또는 임베디드 기기에 적용하기에 적합한 네트워크다
