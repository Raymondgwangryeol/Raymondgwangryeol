from collections import Counter
def solution(n):
    cnt = Counter(bin(n)[2:])["1"]
    for i in range(n+1, 1000001): 
        if Counter(bin(i))["1"] == cnt:
            return i