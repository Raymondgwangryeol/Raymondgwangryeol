import heapq
import sys

n = int(input())
plus_heap = []
minus_heap = []

for _ in range(n):
    x = int(sys.stdin.readline().strip())
    
    if x == 0:
        if plus_heap or minus_heap:
            print(-heapq.heappop(minus_heap) if not plus_heap or (minus_heap and minus_heap[0] <= plus_heap[0]) else heapq.heappop(plus_heap))
        else:
            print(0)
    else:
        heapq.heappush(plus_heap, x) if x > 0 else heapq.heappush(minus_heap, -x)