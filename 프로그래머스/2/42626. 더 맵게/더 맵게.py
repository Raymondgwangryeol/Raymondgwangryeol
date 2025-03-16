import heapq

def solution(scoville, K):
    heapq.heapify(scoville)
    cnt=0
    while scoville[0]<K:
        if len(scoville)<2:
            return -1
        a = heapq.heappop(scoville)
        heapq.heapreplace(scoville, a + scoville[0] * 2)
        cnt+=1
    return cnt