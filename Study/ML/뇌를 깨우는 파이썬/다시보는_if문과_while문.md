## if문과 문자열

조건문 -> tab은 4칸 띄어쓰기 권장   
<br>

      if boolean 표현식 :   
      
          코드블록   
      
      else:   
      
          코드블록   

 <br>

맞다 여기 if랑 else랑 elif가 있지   

if문은 2번까지만 중첩하자   

<br> 

if else 문이 그렇게 길지 않다면?   

조건 표현식으로 간단하게 쓸 수 있다.   

 <br>

    if boolean 표현식 else 표현식2   
    
    if 10%2==0 else print("홀수")   

 
<br><br>
 

## 문자열

len() => 문자열의 길이를 나타내는 함수   

str() => 문자열으로 형변환 해주는 함수   

<br> 

문자열 끼리 더할 수 있는데, 이를 스트링 객체에 덧셈 연산자가 오버로딩 되었다고 한다.    

'ab'[0]을 출력하면 'a'가 나온다... 와 이렇게도 되냐?   

=> 인덱싱이라고 한다   

 <br><br>

### 슬라이싱(slicing)

여러 인덱스를 잘라서 추출하고 싶을 때   

<code>s[start_index:end_index]</code>     

=> start_index부터 end_index-1까지!   

맨 첫 번째 인덱스랑 마지막 인덱스는 생략할 수 있음    

 <br><br>

### f 포맷 문자열
```python
s = 'coffee'
n = 5

result1 = f'저는 {s}를 좋아합니다. 하루 {n}잔 마셔요.'
```
 <br>

### 입력받는법   

<code>input()</code>   
<br>
input은 입력되는 모든 것을 문자열로 취급하기 때문에 number는 숫자가 아닌 문자열이라는 것에 주의하자.   

 <br><br>

### 문자 인코딩?   
```python
string = "테스트 문자열"    

string.encode(encoding='UTF-8', errors='strict')   
```
<br>
문자열을 특정 인코딩 타입으로 인코딩 해서 바이너리로 만들어주는 함수   
<br><br>

## 파이썬의 반복문

 

### for문

    for 변수 in 시퀀스:  
        코드 블록  
<br>

시퀀스: 연속된 값을 의미.(시퀀스 자체는 비스칼라)   

파이썬의 for문은 연속된 값(시퀀스)에서 값을 하나씩 꺼내서 코드 블록을 실행하는 구조. boolean 조건문을 사용하지 않는다!   
```python
for _ in (1,2,3):   
  print("Hello")   
```
<br>

파이썬은 변수 이름을 '\_'로 시작해도 된다... 그래서 for문에 변수를 '\_'로 쓴다는 거는 변수에 시퀀스 값을 받아서 직접 쓰지는 않는다는 뜻.   
위 예제처럼 Hello를 3번 출력하기 위해 쓰는 경우가 해당됨.   

 
```python
for i in range(3):
  print("Hello")
```
<br>

<code>range()</code>함수 => 시작 부터 끝-1까지 의 수를 가지고 있는 range 객체를 리턴함.   
<br>
<code>range(start, stop[, step])</code>   

## 완전 열거 알고리즘
정답을 얻을 때 까지, 또는 가능한 값을 모두 소진할 때까지 시도하는 알고리즘.   

비효율적으로 보일 수 있지만, 요즘 컴퓨터 계산 능력이 빠르기 때문에 간단한 코드라면 실용적일 수 있음.   

구현하기도 쉽고, 이해하기도 쉬움.   

 

### 세제곱근 구하기
```python
#정수 세제곱근을 찾습니다
x = int(input('정수를 입력하세요: '))
ans = 0
while ans**3 < abs(x): #정수 절대값보다 크거나 같아질 때 까지 돌린다
# ans**3-abs(x) < 0이렇게 해도 같은 식임.
    ans = ans + 1
if ans**3 != abs(x): #세제곱한 수가 x랑 같지 않으면 x는 세제곱해서 나올 수 있는 수가 아니다
    print(x, '는 완전한 세제곱수가 아닙니다')
else:
    if x < 0: #x가 음수라면
        ans = -ans # ans도 음수로 바꿔준다
    print(x,'의 세제곱근은', ans, '입니다')
``` 

### 소수 확인하기
 
``` python
# 2보다 큰 정수가 소수인지 테스트합니다. 소수가 아니면 가장 작은 제수를 출력합니다.
x = int(input('2보다 큰 정수를 입력하세요: '))
smallest_divisor = None
for guess in range(2, x):
    if x%guess == 0:
        smallest_divisor = guess
        break
if smallest_divisor != None:
    print(x, '의 가장 작은 제수는', smallest_divisor, '입니다')
else:
    print(x, '는 소수입니다')
``` 

더 효율적인 코드   
```python
# 2보다 큰 정수가 소수인지 테스트합니다. 소수가 아니면 가장 작은 제수를 출력합니다.
x = int(input('2보다 큰 정수를 입력하세요: '))
smallest_divisor = None
if x%2 == 0:
    smallest_divisor = 2
else:
    for guess in range(3, x, 2): #어차피 2로 나누면 나머지 0 되는 짝수는 보나마나 소수가 아님. 홀수로만 나눈다.
        if x%guess == 0:
            smallest_divisor = guess
            break
if smallest_divisor != None:
    print(x, '의 가장 작은 제수는', smallest_divisor, '입니다')
else:
    print(x, '는 소수입니다')
```
## 이분 검색

양수의 제곱근 구하기   

 

case 1:   
```python
x = 25 
epsilon = 0.01 # 얼만큼 이동할 것인지
step = epsilon**2 # 0.0001씩 앞으로 간다
num_guesses = 0
ans = 0.0
while abs(ans**2 - x) >= epsilon and ans <= x: # ans**2(우리는 제곱근을 찾는 거니까) - x가 아직은 0.01보다 크고, ans가 x보다 작을 때
    ans += step # 계속 0.0001씩 더해감
    num_guesses += 1 # step 더한 횟수
print('추측 횟수 =', num_guesses)
if abs(ans**2 - x) >= epsilon: #x와 ans의 차이가 0.01보다 크다면, 근사값이 아닌 것으로 판단
    print(x, '의 제곱근을 찾지 못했습니다')
else:
    print(ans, '(은)는', x, '의 제곱근에 가깝습니다')
```
case 2:   
```python
x = 0.25
epsilon = 0.01
step = epsilon**2
num_guesses = 0
ans = 0.0
while abs(ans**2 - x) >= epsilon and ans <= x: #ans**2-x가 0.2501-0.25 = 0.0001이므로 True지만,  0.2501<=0.25는 성립하지 않으므로 False
    #ans<=x이므로, ans**2-x가 epsilon보다 작다고 해도 x와 비슷해질 때 까지 돌기 때문에 제곱근인 0.5가 아닌 0.25와 비슷한 숫자에서 stop
    ans += step
    num_guesses += 1
print('추측 횟수 =', num_guesses)
if abs(ans**2 - x) >= epsilon:  # epsilon과 값이 같아져서 못 찾음.
    print(x, '의 제곱근을 찾지 못했습니다')
else:
    print(ans, '(은)는', x, '의 제곱근에 가깝습니다')
```
case3:    
```python
x = 0.25
epsilon = 0.01
step = epsilon**2
num_guesses = 0
ans = 0.0
while abs(ans**2 - x) >= epsilon and ans*ans <= x: # ans의 제곱과 x가 비슷해질 때 까지 하는게 더 정확함.
    ans += step
    num_guesses += 1
print('추측 횟수 =', num_guesses)
if abs(ans**2 - x) >= epsilon:
    print(x, '의 제곱근을 찾지 못했습니다')
else:
    print(ans, '(은)는', x, '의 제곱근에 가깝습니다')
```
위의 프로그램의 문제   

- step 크기가 크면 일부 제곱근을 못 찾을 수 있다
- 그렇다고 너무 작으면 실행하는데 오래 걸린다

### 이분 검색(bisection search)

연속적인 값의 범위를 절반씩 줄여 나가며 근을 찾는다.   

이진 검색은 이분 검색과 비슷한데, 비스칼라 데이터셋(튜플, 리스트)에서 비스칼라 데이터를 정렬해놓고 사용한다는 점이 다름   
```python
x = 25
epsilon = 0.01
# 최대 최소는 처음 기준점으로 맞춰놓는다
num_guesses, low = 0, 0 # 최소값 일단 0으로 
high = max(1, x) # x가 0과 1사이면 1이 max가 된다(high=1, low=0)
ans = (high + low)/2 # 반으로 나눠야 하니까
#아니 음수는 안 되지 않아..? i를 어떻게 처리할건데..
while abs(ans**2 - x) >= epsilon: # ans**2-x가 epsilon이랑 같거나 더 작아지기 전까지
    print('low =', low, 'high =', high, 'ans =', ans)
    num_guesses += 1
    if ans**2 < x: # 만약 ans**2가 x보다 작으면
        low = ans # ans는 지금보다는 더 커야하니까 low=ans
    else:
        high = ans
    ans = (high + low)/2 #ans를 high+low의 중간지점으로 지정
print('추측 횟수 =', num_guesses)
print(ans, '(은)는', x, '의 제곱근에 가깝습니다')
```
