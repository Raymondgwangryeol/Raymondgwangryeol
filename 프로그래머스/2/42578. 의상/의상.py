from collections import defaultdict

def solution(clothes):
    dict_clothes = defaultdict(list)
    for name, category in clothes:
        dict_clothes[category].append(name)
    mul=1
    for category in dict_clothes:
        l=len(dict_clothes[category])
        mul*=(l+1)
    return mul-1