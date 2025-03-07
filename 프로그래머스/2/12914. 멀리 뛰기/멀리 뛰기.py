fibo_l=[0]*2001
fibo_l[1]=1
fibo_l[2]=2
for i in range(3, 2001):
    fibo_l[i] = fibo_l[i-1]+fibo_l[i-2]
def solution(n):
    global fibo_l
    return fibo_l[n]%1234567