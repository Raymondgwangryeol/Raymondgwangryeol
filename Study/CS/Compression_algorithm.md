        공부 자료:
        - https://dydtjr1128.github.io/image/2019/07/01/Image-compression.html
        - https://woonys.tistory.com/188
        - https://namhoon.kim/2022/08/02/cs_introduction/008/
        - https://mvje.tistory.com/86
        - https://m.blog.naver.com/grtwow9/20197052486
        시각 자료
        - https://woonys.tistory.com/188
        - https://velog.io/@hygoogi/DCT-%EB%B3%80%ED%99%98-%EC%8B%A4%EC%8A%B5-s1k66r4mp8
        - https://sketchofcreed.tistory.com/1
# **1. Image compression**

## **1-1. Intro**
### ✔Lossy vs Lossless
각각의 방법은 용도, 속도 측면에서 각자 장단점이 존재.
- **손실 압축(Lossy compression)**
  - 유튜브, 동영상 스트리밍, 오디오 스트리밍에서 많이 시용
  - 사람이 인지하지 못하는 영역의 주파수인 고주파 정보를 제거해 용량을 줄임.
    - **전체적인 구조**: 픽셀 간 색상 및 밝기 변화가 크지 않은 저주파 성분. 사람이 인지하기 쉬운 영역
    - **디테일한 변화**: 픽셀 간 색상 및 밝기 변화가 큰 고주파 성분. 사람이 인지하기 어려운 영역
    - 주파수 영역대를 더 낮은 쪽으로 잡을 수록 이미지 용량은 감소하지만, 사람이 인지할 수 있는 영역의 정보까지 제거될 수 있어 화질이 저하. 
  - 예를 들어 영상의 경우 사람이 듣지 못하는 소리를 버린다던가, 용량을 줄이기 위해 화질을 포기하는 등 데이터 손실 발생
- **비손실 압축(Lossless compression)**
  - 압축된 상태에서도 디지털 원본과 100% 똑같은 형태를 유지하는 압축 방식
  - 반복 표현되는 정보를 최대한 줄이는 방식.
  - 비슷한 값이 자주 나타닐수록 더 적인 비용으로 압축 가능.
<br>

### ✔Indexed color vs Direct color
- Indexed color
  - 제작자가 Color Map이라는 곳에 제한된 수(256가지)의 색상을 가진 paltte로만 저장할 수 있는 속성
- Direct color
  - 제작자가 직접 선택하지 않은 수천가지의 컬러를 지정할 수 있는 속성
  - Indexed color보다 많은 용량 필요

<br><br>

## **1-2. Image Compression Techniques**
### **✔BMP(BitMap) - Lossless, Indexed and Direct**
- 오래된 이미지 포맷. 압축을 전혀 하지 않음(사이즈 너무 큼, 때문에 많이 사용되지 않음)
- 대신 디코딩 할 게 별로 없어 속도가 빠름
- Indexed, Direct color 둘 다 지원
<br>

### **✔JPG, JPEG(Joint Photographic Experts Group) - Lossy, Direct**
- jpg == jpeg(same imply). 원래 jpeg인데 DOS에서는 확장자를 4자리 이상으로 지정할 수 없어 jpg로 줄인 것
- 손실 압축 방법의 대표적 예
- 데이터 손실 발생, 용량이 작아 웹에서 널리 쓰임
- RGB 색상 공간 사용, Opaque(불투명도) 사용 불가
<br>

### **✔PNG(Portable Network Graphics) - Lossless / Indexed(PNG8), Lossless / Direct(PNG-24)**
- 비손실 압축의 대표 방법
- 고품질 이미지 생성, 파일 크기는 다른 포맷보다 상대적으로 큼. 느린 압축 속도.
- 트루 컬러 지원, RGB, 8bit(1byte)씩 할당. RGB를 배합해 1 pixel을 가짐
- PNG-8은 256색(해상도 낮음, 단순한 이미지에 적합), PNG-24는 16,777,216색 + 알파 값(고화질 이미지나 사진 등)
- 알파 값이 추가되면 반투명 이미지나 그림자 표현 가능
<br>

### **✔SVG(Scalable Vector Graphics) - Lossless, Vector**
- 2차원 벡터 그래픽을 표현하기 위한 XML 기반의 파일 형식
- XML 텍스트 파일로 정의되어 검색화, 목록화, 스크립트화가 가능, 필요하다면 압축도 가능
- 주요 웹 브라우저들이 현재 SVG 지원
- 픽셀 대신 라인과 곡선들로 이루어져 깨짐 현상이 없음(로고 등에 적절)
- 사이즈도 작은 편에 속함
- 모양이 복잡할 수록 벡터 계산이 많이 필요할 수 있음.
<br><br>

## **1-3. Ways of Lossy/Lossless Compression Algorithnm**
### **✔Lossy Compression - JPEG**
**RGB to YCrCb → 다운샘플링 → DCT(이산 코사인 변환) → 양자화 → 지그재그 스캐닝 → 허프만 코딩**  
<br>

**1. RGB to YCrCb**
  - YCrCb?
    - Y(Luminace, 밝기), Cr(Chroma red, 적색 색차 = Red-Y, 빨간색이 강한 정도), Cb(Chroma blue, 청색 색차 = Blue-Y, 파란색이 강한 정도)
    - 다운샘플링을 위해서 YCrCb로 반환
<br>

**2. 다운샘플링**
  - 인간의 시각 정보 인식 방법에 근거. 색 관련 채널은 밝기 채널에 비해 중요도가 낮으므로, 해당 정보 용량을 압축(다운 샘플링)
  - 다운 샘플링 비율은 **J:a:b** (가로 : 1번 행의 활성화 요소 수 : 2번 행의 활성화 요소 수. 세로 길이는 2로 고정).
    - 만약 J가 4면 2X4 사이즈의 픽셀 블럭을 옆으로 옮기면서 샘플을 추출.
  ![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/a9bf509c-55f2-40ba-b5e7-4c2fa3611dfe)
<br>

**3. DCT**
- 각 채널을 8X8 블럭(JPEG 기준)으로 분할 후, 각 블럭에 DCT 수행
- DCT는 이미지 정보를 frequency(주파수) 도메인으로 변환, 변화에 초점을 맞춤.
  - INPUT: 입력 화소값
  - OUTPUT: 변환값
  - INPUT, OUTPUT의 픽셀 좌표는 각각((i,j), (x, y))
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/a14640c2-8223-4432-a442-ef7289b974b0)

![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/b6bda361-07ed-4660-a84d-a15908f62ebf)
<br>

**4. 양자화(디지털화)**    
- DCT 계수를 양자화, 높은 고주파 신호와 관련된 계수는 대부분 0이 되게 함.
- DCT 계수 중 가장 처음 얻는 값인 0차(0,0) 계수는 코사인 함수가 아니며, 이를 DC계수라 함.
  - DC계수는 해당 블록 전체의 명도를 결정하는 평균 색상 값을 가짐.
- DCT 수행하면 좌측 상단에 저주파 성분이, 우측 하단에 고주파 성분이 배치되게 됨.
<br>

**5. 지그재그 스캐닝**    
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/eed8c18e-297c-403e-8436-9f31fef0b424)

- 2차원 배열에 담겨있는 주파수 계수를 지그재그 순서로 읽어 1차원 벡터 형태로 변환
- 지그재그로 읽는 이유는 벡터의 뒷부분에 최대한 0을 많이 모으기 위함. (고주파 성분을 0으로 바꿨기 때문에, 이렇게 읽으면 0을 뒤쪽 끝으로 몰 수 있다.)
<br>

**6. 허프만 코딩**
- 엔트로피 부호화 과정, 데이터의 출현 빈도를 파악해 기호에 짧은 접두어 코드를 부여. 빈도수가 높을수록 작은 비트를 부여함.
  - 많이 사용되는 데이터에 부여하는 비트 크기가 작을수록 공간이 절약됨.
  - 예로, aaaabbbccd가 있으면 빈도는 a>b>c>d순 이므로, a=0 , b=01, c=011, d=111 이런 식으로 비트를 부여함. 압축한 결과는 0000010101011011111.
- 진행 과정
  1. 데이터 내 문자들에 대한 빈도 수 구하기
  2. 빈도를 기준으로 내림차순 정리하기
  3. 낮은 빈도를 가진 문자를 기준으로 이진트리 만들기
  ```mermaid
  graph TD;
  A(14)
  B(A:7)
  C(7)
  D(4)
  E(C:3)
  F(B:2)
  G(2)
  H(D:1)
  I(G:1)
  J(10)
  K(F:6)
  L(E:4)
  M(24)
  M --- A
  M --- J
  A --- B
  A --- C
  C --- D
  C --- E
  D --- F
  D --- G
  G --- H
  G --- I
  J --- K
  J --- L
  ```
  
  5. 각 가지에 코드 부여하기
     - 왼쪽 가지에는 0, 오른쪽 가지에는 1 부여
   ```mermaid
  graph TD;
  A(14)
  B(A:7)
  C(7)
  D(4)
  E(C:3)
  F(B:2)
  G(2)
  H(D:1)
  I(G:1)
  J(10)
  K(F:6)
  L(E:4)
  M(24)
  M-- 0 ---A
  M-- 1 ---J
  A-- 0 ---B
  A-- 1 ---C
  C-- 0 ---D
  C-- 1 ---E
  D-- 0 ---F
  D-- 1 ---G
  G-- 0 ---H
  G-- 1 ---I
  J-- 0 ---K
  J-- 1 ---L
  ```
  6. 각 코드를 붙여가면서 허프만 코드를 정의     
<br/>

  |문자|빈도|허프만 코드|비트 수|
  |:---:|:---:|:---:|:---:|
  |A|7|00|2|
  |F|6|01|2|
  |E|4|11|2|
  |C|3|011|3|
  |B|2|0100|4|
  |D|1|01010|5|
  |G|1|01011|5|

<br><br><br>

### **✔Lossless Compression**
**DEFLATE(LZ77 + 하프만 코딩)**
- **LZ77: 사전(Dictionary) 방식 압축 알고리즘**
  - 현재 압축하려는 데이터가 이전에 존재했는지 파악, 부호화 도중에 새로 나타나는 심볼열을 사전에 기록 후 해당 내용을 다음 부호화에 사용.
  - 처음 등장하는 정보는 사전에 기록되어 있지 않아 정보량이 큼
  - 한 번 사전에 입력된 정보는 꺼내 쓰면 되기 때문에 정보량이 작음.
 <br><br><br><br><br>

 # **2. Video Compression - MPEG**
 ## 2-1. Intro
 - MPEG(Moving Picture Experts Group)
   - ISO 및 IEC 산하에서 비디오와 오디오 등 멀티미디어 표준 개발을 담당하는 소규모 그룹
   - 현재 가낭 널리 쓰이는 비디오 압축 규격
## 2-2. Type of MPEG
### ✔MPEG-1
- 최초의 비디오/오디오 표준. CD에 동영상을 담기 위해 사용
- CD-ROM 속도에 맞춰 최대 1.5Mbps의 전송률을 지원, 표준 해상도는 352X240(30fps)
- CD 한 장에 74분의 영상을 담을 수 있으며, 2채널 Stereo를 지원
- MP3 오디오 압축 포맷이 해당 표준에 포함.(MPEG1의 Layer 3이 떨어져 나온 것)
### ✔MPEG-2
- 방송용 DVD, HDTV에 사용됨.
- 4Mbps 이상의 전송률, Full HD 해상도 구현 가능.
- 5.1채널 입체 음향 지원
### ✔MPEG-4
- 현재 가장 많이 쓰이는 압축 기술
- 웹, 모바일 환경에서 사용.
- 유튜브 등 인터넷에 업로드 되는 영상은 거의 이 방식을 사용.
- 24Kbps ~ 2Mbps의 낮은 전송 속도, 높은 압축률(.mp4)
### ✔MPEG-7
- 주로 멀티미디어 정보 검색에 사용되는 압축 표준
- 데이터 검색을 용이하게 하기 위해 만들어짐.
  - 키워드로 동영상을 검색하는 등 다양한 분야에서 사용 가능.
## 2-3. Compression principle of MPEG
- 프레임 단위의 압축과 프레임 간의 연관성을 이용.
- 이전 프레임과 현재 프레임의 차로 객체의 움직임을 추정하고 이를 보상.
- 비디오 시퀀스가 가지는 시간축 상의 중복을 없애기 위한 **Motion Compression** 적용
  - **Motion Compression의 3가지 Frame**
    ![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/2885f22e-7e98-4495-a9f0-5065836dcb7b)

    - **I-Frame(Key Frame)**
      - 자신의 현재 정보만(단일 프레임) 압축.(JPEG 압축 방식과 유사)
      - 원본 소스에 가장 근접, 화질 뛰어남. 용량이 큼.
      - 배경과 Object의 데이터를 가진 유일한 Frame 유형
      - 데이터 스트림의 어느 위치에도 올 수 있기 때문에, I-Frame으로 인해 Random access가 가능
    - **P-Frame**
      - 가장 가까운 이전 프레임(순방향 예측)으로 부터 움직임 예측 후, 나머지 차이 부분을 DCT 변환하여 압축
      - 과거 프레임으로 부터 예측하기 때문에 이전 프레임에 대한 의존도가 높음
    - **B-Frame**
      - 자신의 현재 이미지를 기준으로, 이전 프레임 뿐 아니라 이후 프레임도 참고하여(양방향 예측) 압축
      - 화질 가장 떨어짐, 용량이 작고 압축률 가장 높음
      - 이전, 이후 프레임에 대한 의존도가 높음.
 - **GOP(Group of Pictures)**
   ![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/f8605647-3ccc-496c-95aa-fa3876d99aa8)

   - MPEG의 압축 단위. 현재 I-Frame에서 다음 I-Frame까지의 개별 시퀀스 프레임 개수를 의미함.
   - 역방향 재생 시 Key Frame인 I-Frame을 찾기 힘들 경우, 기준이 없어 복원이 불가능하고, 동영상의 한 지점에 Random Access할 때 빠르게 해당 시점의 프레임을 복원해야함. 이를 위해 GOP 구조가 추가됨.
   - 때문에 GOP내에 반드시 I-Frame이 포함되어야 하며, GOP 앞부분에 시퀀스 헤더를 붙여 Random Access시 해당 헤더위 위치를 확인하고, GOP 내의 I-Frame을 참조하여 영상 복원.
 <br><br><br><br><br>
 # **3. String Compression**
 ### 3-1. RLE(Run-length encoding)
 - 문자열을 문자와 반복 횟수로 표현하는 방법.
   - ex) AAAAAAABBCCCDEEEEFFFFFFG
     - RLE 적용: A7B2C3D1E4F6G1
<br>

### 3-2. 허프만 코딩(Huffman coding)
- 위에 기술한 내용과 동일.
 
