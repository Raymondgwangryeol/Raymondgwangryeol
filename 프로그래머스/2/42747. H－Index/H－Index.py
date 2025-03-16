def solution(citations):
    citations.sort()
    answer=0
    for i in range(len(citations)-1, -1, -1):
        if citations[i] >= len(citations) - i:
            answer=max(len(citations) - i, answer)
    return answer 