from collections import Counter

def solution(clothes):
    dict_clothes = Counter([category for name, category in clothes])
    mul=1
    for category in dict_clothes:
        mul*=(dict_clothes[category]+1)
    return mul-1