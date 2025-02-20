import sys

n,m = map(int, input().split())
poketmon={}

for i in range(n):
    poketmon[sys.stdin.readline().strip()]=i+1

keys=list(poketmon.keys())

for i in range(m):
    find=input()
    if find.isdigit():
        print(keys[int(find)-1])
    else:
        print(poketmon[find])
