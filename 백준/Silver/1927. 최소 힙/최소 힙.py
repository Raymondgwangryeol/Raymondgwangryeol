import heapq
import sys
n = int(input())
queue = []

for _ in range(n):
    x = int(sys.stdin.readline().strip())
    if x==0:
        if len(queue)>0:
            print(heapq.heappop(queue))
        else:
            print(0)
    else:
        heapq.heappush(queue,x)