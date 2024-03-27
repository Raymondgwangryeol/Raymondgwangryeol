## 문제
그래프를 DFS로 탐색한 결과와 BFS로 탐색한 결과를 출력하는 프로그램을 작성하시오.   
단, 방문할 수 있는 정점이 여러 개인 경우에는 정점 번호가 작은 것을 먼저 방문하고, 더 이상 방문할 수 있는 점이 없는 경우 종료한다. 정점 번호는 1번부터 N번까지이다.

## 입력
첫째 줄에 정점의 개수 N(1 ≤ N ≤ 1,000), 간선의 개수 M(1 ≤ M ≤ 10,000), 탐색을 시작할 정점의 번호 V가 주어진다. 다음 M개의 줄에는 간선이 연결하는 두 정점의 번호가 주어진다. 어떤 두 정점 사이에 여러 개의 간선이 있을 수 있다. 입력으로 주어지는 간선은 양방향이다.

## 출력
첫째 줄에 DFS를 수행한 결과를, 그 다음 줄에는 BFS를 수행한 결과를 출력한다.    
V부터 방문된 점을 순서대로 출력하면 된다.

## 풀이
풀고 굉장히 많이 반성하게 되었던... 문제.   
아직 개념을 잘 이해하지 못했구나 하는...   
여기서 graph를 NxN의 형태로 만드는데, 이제서야 2차원을 그래프 형식으로 바꿔서 문제를 풀라는 말이 뭔지 깨달았다.   
대체 이걸 어떻게 graph형식으로 만들지 몰라서, 다른 분의 코드를 참고해서 풀었다.   
관련 문제를 더 풀어봐야 할 것 같다..   

내가 참고한 분의 코드는 입력이 1부터 들어오기 때문에, (N+1)x(N+1)형태로 그래프를 만드셨다.   
나중에 코딩할 때 그렇게 하는 게 훨씬 편한 것 같다. 참고해야겠다.   
```python
from collections import deque

n, m, v = map(int, input().split())

queue = deque()
graph = [[0]*n for _ in range (n)] # 이런 생각은 어떻게 하시는 겁니까..
                                   # 그래프를 N*N의 형식으로 만들었다.
check1 = [0]*n  # check는 들어오는 정점 수 만큼만 확인하면 되니까
check2 = [0]*n

for _ in range(m):
    x, y = map(int, input().split())
    graph[x-1][y-1] = graph[y-1][x-1] = 1 # 입력으로 주어지는 간선은 양방향이어서 그것 까지 처리를 해 준다.

def dfs(v):
    check1[v-1] = 1
    print(v, end=" ")
    for i in range(n):
        if(check1[i]== 0 and graph[v-1][i] == 1): # graph[v-1]의 인덱스를 0부터 쭉 확인하는... 
                                                  # 진짜 천재인가
            dfs(i+1)

def bfs(v):
    queue.append(v)
    check2[v-1] = 1
    while queue:
        v=queue.popleft()
        print(v, end=" ")
        for i in range(n):
            if(check2[i]==0 and graph[v-1][i] == 1):
                queue.append(i+1)
                check2[i] = 1

dfs(v)
print()
bfs(v)
```
