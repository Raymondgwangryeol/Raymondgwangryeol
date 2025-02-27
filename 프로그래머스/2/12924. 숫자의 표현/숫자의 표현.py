def solution(n):
    answer = 0
    left = right = total = 1
    while left<=n and right<=n:
        if total==n:
            answer+=1
        if total<n:
            right+=1
            total+=right
        else: 
            total-=left
            left+=1 
    return answer