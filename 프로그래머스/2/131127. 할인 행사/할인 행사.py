from collections import Counter 

def solution(want, number, discount):
    want_d={}
    count = 0
    for item, num in zip(want, number):
        want_d[item]=num
    for i in range(len(discount)-9):
        check=True
        discount_d=Counter(discount[i:i+10])
        for value in want_d:
            if discount_d.get(value, 0)!=want_d[value]:
                check=False
                break
        if check:
            count+=1
    return count