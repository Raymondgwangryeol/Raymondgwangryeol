from collections import Counter

def solution(k, tangerine):
    answer=0
    l = Counter(tangerine).most_common()
    for _, num in l:
        k-=num        
        answer+=1
        if k<=0:
            break
    return answer