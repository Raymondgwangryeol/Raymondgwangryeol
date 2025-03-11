from functools import reduce

def gcd (a, b):
    if b == 0:
        return a
    else:
        return gcd(b, a%b)
    
def lcm(a, b):
    return ((a*b)//gcd(a, b))

def solution(arr):
    return reduce(lcm, arr)