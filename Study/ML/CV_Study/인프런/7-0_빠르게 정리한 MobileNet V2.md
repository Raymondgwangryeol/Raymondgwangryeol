# **MobileNet V2**
    참고 영상 URL: https://youtu.be/mT5Y-Zumbbw?feature=shared
## **Introduction**
- Existing network require high computational resources
- Architectural Search가 계속 연구되고 있지만, 당시 나오던 network 구조가 복잡했다

## **Key Features**
- Depthwise Separable Convolution
- Linear Bottlenecks (new!)
- Inverted Residuals (new!)

## **Linear Bottlenecks**
- Bottleneck structure
    - Feature의 차원을 축소해서 연산하고, 다시 확장시켜주는 구조. 병목의 형상을 닮았다 하여 붙여진 이름.
    - Parameter 연산 관점에서 효율적이면서도 Complexity는 비슷.
- Assumption
  - 사람들은 이미지를 넣었을 때, network의 layer들이 manifold of interest를 형성한다고 생각함.
  - Neural Network의 manifolds of interest가 낮은 차원의 subspaces에 mapping될 수 있다고 오랫동안 추측되어져 왔다
- Property
  - Manifolds of interest가 ReLU 통과 이후 non-zero volume을 가지면, linear transformation이 가능.
      - ReLU는 -1과 x로 나뉘는데, 여기서 살아남은 x들은 선형변환 구조를 가질수 밖에 없다
  - ReLU는 input manifold가 input공간의 low 차원 subspace에 있을 때 입력 mainfold에 대한 완전한 정보를 보존할 수 있다.
      - 채널의 차원이 보장되어야, 정보 유지가 된다.
  - 때문에, 채널 수를 줄일 때 linear bottleneck layer를 ReLU 대신 집어넣는게 더 좋다

## **Inverted Residual Blocks**
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/e3bb2152-bb8b-457a-84e6-34512207a719)   
ResNet의 Residual Block은   

         wide           →                  narrow(bottleneck)             →                wide approch   
       채널 많음                       채널 수를 줄인 후 3X3 Conv                     1X1 Conv로 다시 채널을 늘림   
                                                                                     Residual Connection이 와서
                                                                                      다시 더해져야 하기 때문    
<br><br><br>

MobileNet V2에서는 이와 반대로 expension을 먼저 함   
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/c54a69ed-4264-4b68-b338-f5c4b52574bd)   

             narrow                →                wide                 →              narrow approch   
      1X1 linear bottleneck

채널 수가 많은 곳에서 적은 곳으로 갈 때 그 채널의 정보를 잃지 않고 다 담고 있다고 가정.   
때문에, 정보를 다 담고 있는 narrow에서 residual connection을 만들어도 문제 없다.   
이렇게 하면 메모리도 효율적으로 사용 가능함.    

<br><br><br>

![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/4a982d62-9c0a-40f3-b40a-aabc6fd5a478)   
- 충분한 channel의 수를 확보 하기 위해 처음에 expension 수행
- 마지막 layer는 linear bottleneck layer
  - 채널 수 줄임(Projection)
  - ReLU가 안 붙어 있음
    - Linear feature를 가져오기 위해서 일부러 붙이지 않음.
    - ReLU와 같은 non-linear 함수를 붙이는 게 다른 feature map으로 transform시 정보가 유지되지 않는 경우가 있었음. (rank나 dimension manifolds)
    - 정보 유지를 위해서는 채널의 차원이 보장 되어야 함.
- 중간에 채널을 얼마나 키울 건지 결정하는 t(expension factor)값은 6으로 진행됐고, 논문에서는 5에서 10정도가 적당하다고 언급.
 <br> <br>
 ![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/8e6ad7a6-2b27-4394-b4a6-f73c44eadcc6)
- stride가 1일 때만 residual connection이 있음.

## **Parameter count for bottleneck convolution**
가로: h
세로: w
expansion factor: t
커널 크기: k
input channel 크기: d'
output channel 크기: d"
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/37002a05-a670-4966-a52c-168b9e354b03)     
MobileNet V1보다 연산량이 많이보일 수 있지만, 실제로는 더 적은 채널 수로 학습 할 수 있기 때문에, 연산량이 더 적다.

## **Information flow interpretation**
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/c0ed007c-e09c-4631-baf9-ec94794bd5e7)    
Residual block에서 expressiveness와 capacity를 잘 seperate 시켰다.
- Expressiveness는 expansion layer에서 얼마나 채널 수를 늘려주느냐에 따라 결정됨.
- Capacity는 projection layer에서 얼마나 채널 수를 줄이느냐와 관련이 있음.

## **Trade-off Hyper Parameters**
- Input Resolution: 96에서 224의 값 사용
- Width Multiplier: 0.35에서 1.4의 값 사용

## **Memory Efficient Inference**
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/c0ed007c-e09c-4631-baf9-ec94794bd5e7)
Memory Operation은 값을 외부 메모리에서 가져와야 하는 맨 앞 layer와 맨 뒤 layer에서 일어남.   
나머지 layer들은 내부 메모리에서 처리 가능.   
Memory Operation이 일어나는 두 layer들이 narrow하기 때문에, mobile applications에 중요한 효율적인 메모리 구현이 가능.   
The total amount of memory would be dominated by the size of bottleneck tensors.

## **Bottleneck Residual Block**
Expension을 한 번에 다 하지 말고, 채널을 몇 묶음씩 나눠서 보내면 메모리 사용량을 줄일 수 있다.   
너무 잘게 자르면 cache miss가 발생해서 오히려 성능이 떨어질 수 있음.
2에서 5정도가 좋다고 언급.

## **Model Structure**
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/3d2a24f9-dd7e-49a6-bc6c-c1aa07930127)    
t: expansion factor    
c: output channel    
n: repeated times    
s: stride    

## **Object Detection Performance**
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/86b3dd47-ed5a-42a6-ba88-5cf867710742)    

ImageNet 수행, Open Source TensorFlow Object Detection API로 학습 및 평가.    
이미지의 너비를 크게 했을 때 비교군 중 가장 높은 정확도를 기록했다. 너비가 클 수록 Parameter수와 연산량, 수행 시간도 증가했다.   
이미지 너비를 넓히지 않았을 때는 정확도는 다소 떨어지지만, Parameter 수가 많이 줄었고, 연산량도 300M으로 ShuffleNet(1.5) 다음으로 작은 연산량을 가졌다.    
특히 수행 속도 시간은 75ms로, 측정된 수행시간 중 가장 낮은 시간을 기록했다.     
MobileNet V1과 비교했을 때 모든 면에서 향상된 결과를 보였다.  
