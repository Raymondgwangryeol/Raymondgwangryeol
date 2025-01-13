import heapq
import sys
n = int(input())
plus = []
minus = []

for _ in range(n):
    x = int(sys.stdin.readline().strip())
    if x==0:
        if plus or minus:
            if not plus:
                print(-heapq.heappop(minus))
            elif not minus:
                print(heapq.heappop(plus))
            else:
                if plus[0]<minus[0]:
                    print(heapq.heappop(plus))
                else:
                    print(-heapq.heappop(minus))
        else:
            print(0)
    elif x>0:
        heapq.heappush(plus,x)
    else:
        heapq.heappush(minus,-x)