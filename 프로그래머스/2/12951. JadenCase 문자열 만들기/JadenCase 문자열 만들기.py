def solution(s):
    s=s.lower()
    answer=s[0].upper()+"".join([s[i] if s[i-1]!=" " else s[i].upper()  for i in range(1, len(s))])
    return answer