## 문제
N개의 수로 이루어진 수열 A[1], A[2], …, A[N]이 있다.    
이 수열에 대해서 버블 소트를 수행할 때, Swap이 총 몇 번 발생하는지 알아내는 프로그램을 작성하시오.

버블 소트는 서로 인접해 있는 두 수를 바꿔가며 정렬하는 방법이다.     
예를 들어 수열이 3 2 1 이었다고 하자. 이 경우에는 인접해 있는 3, 2가 바뀌어야 하므로 2 3 1 이 된다.     
다음으로는 3, 1이 바뀌어야 하므로 2 1 3 이 된다.     
다음에는 2, 1이 바뀌어야 하므로 1 2 3 이 된다. 그러면 더 이상 바꿔야 할 경우가 없으므로 정렬이 완료된다.   

## 입력
첫째 줄에 N(1 ≤ N ≤ 500,000)이 주어진다.     
다음 줄에는 N개의 정수로 A[1], A[2], …, A[N]이 주어진다. 각각의 A[i]는 0 ≤ |A[i]| ≤ 1,000,000,000의 범위에 들어있다.   

## 출력
첫째 줄에 Swap 횟수를 출력한다    

## 풀이
인프런에서 Do it! 알고리즘 코딩테스트 강의를 듣고 푼 문제. 문제 접근 방식이 어려워서 플래티넘 문제라고 한다.   

Merge Sort를 간단하게 정리해 보자면,   
1. 나눌 수 없을 만큼 나눈다(2배씩)
2. 합치면서 정렬한다
3. 1 뭉텅이가 될 때 까지 반복

시간 복잡도는 한 번 정렬하는 데 access하는 data 수 N과, 전체 정렬 하는 횟수인 log(N)을 곱한 Nlog(N)이다.    

특히, Merge Sort에서 2개의 그룹을 병합하는 과정을 이해했는지 물어보는 문제가 자주 출제되고 있다고 한다!   

예를 들어, 자동차 경주를 한다고 치고, 숫자가 작은 숫자일 수록 빠른 자동차라고 하자.
3, 2, 8, 7, 1, 4, 5, 6
이 있고, 이를 4개씩 2그룹으로 나눴다고 했을 때 무한정 달린다면, 1은 몇 개를 역전했는지 물어본다면 이 말은,   
1번이 앞 애들을 몇 번 뛰어넘었는가?로 볼 수 있고,   
앞 뭉탱이에 뛰어넘을 애들이 몇이 있는가로 생각할 수 있다.     
한 놈씩 뛰어 넘을 때 마다, 형태가 버블 정렬과 같으므로, 4번 버블 정렬을 했다고 할 수 있다!   
이를 이용해서 이번 문제를 접근해야 한다.   

겉으로 보기에는 버블 정렬 문제지만... 시간 복잡도가 O(N²)이어서 2천만에서 1억개 사이의 data를 다루면 1초가 넘어가기 때문에 쓸 수 없다..    
그렇다면 O(Nlog(N))인 병합정렬이나 O(log(N))인 힙정렬은 어떤데? ...이런 방식으로 다가가야 문제를 풀 수 있다.   

병합정렬로 문제를 풀면 문제가 되게 간단해 지는데, Swap이 몇 번 발생했는가 = Merge sort에서 뒷 뭉탱이가 앞 뭉탱이를 얼마나 뛰어넘었는가(?)로 풀면 되기 때문이다.    

```python
import sys
count = 0

def merge_sort (submit):

    if(len(submit) <= 1):
        return submit

    mid = len(submit)//2

    left= submit[:mid]
    right = submit[mid:]

    left_sort = merge_sort(left)
    right_sort = merge_sort(right)

    return merge(left_sort, right_sort)

def merge(left, right):
    global count
    sorted_list = []
    i=j=0

    while(i<len(left) and j<len(right)):
        if(left[i]<=right[j]):
            sorted_list.append(left[i])
            i+=1
        else:
           sorted_list.append(right[j])
           count+=len(left)-i
           j+=1

    while len(right) > j:
        sorted_list.append(right[j])
        j+=1
    while len(left) > i:
        sorted_list.append(left[i])
        i+=1

    return sorted_list

n = int(input())
submit = list(map(int, sys.stdin.read().split()))
merge_sort(submit)
print(count)
```
