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
- CNN Model이 매개변수 θ를 기반으로 강화학습한 Agent의 Action a<sub>1:t</sub>의 Sequence에 의해 결정된 token들(???무슨 말이지)들의 list에 mapping
#### Sample-eval-update Loop

   
