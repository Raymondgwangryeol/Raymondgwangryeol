import sys

hear, see = map(int, input().split())  
d = set(sys.stdin.readline().strip() for _ in range(hear))

result = []
for _ in range(see):
    name = sys.stdin.readline().strip()
    if name in d:
        result.append(name)
        
result.sort()
print(len(result))
print(*result, sep="\n", end="")