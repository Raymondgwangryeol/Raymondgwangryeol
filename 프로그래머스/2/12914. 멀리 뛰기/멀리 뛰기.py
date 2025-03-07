def solution(num):
    a, b = 1, 2
    for i in range(2,num):
        a, b = b, a+b
    return 1 if num==1 else b%1234567