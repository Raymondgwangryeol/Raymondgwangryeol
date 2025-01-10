N=int(input())
M=int(input())
d={i:[] for i in range(1,N+1)}
for _ in range(M):
    x,y = map(int, input().split())
    d[x]+=[y]
    if x!=1:
        d[y]+=[x]
visited=set()
def dfs(x):
    if x in visited:
        return
    visited.add(x)
    for i in d[x]:
        dfs(i)
dfs(1)

print(len(visited)-1)