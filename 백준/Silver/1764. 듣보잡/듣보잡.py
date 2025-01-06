import sys

hear, see = map(int,input().split())
result=[]
d={}
for i in range(hear):
    d[sys.stdin.readline().strip()]=i
for j in range(see):
    name=sys.stdin.readline().strip()
    try:
        d[name]
    except:
        pass
    else:
        result.append(name)
result.sort()
print(len(result))
print(*result, sep="\n", end="")