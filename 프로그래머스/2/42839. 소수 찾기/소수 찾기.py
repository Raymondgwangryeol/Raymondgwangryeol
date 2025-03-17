from itertools import permutations
def solution(numbers):
    num_set=set()
    for i in range(1, len(numbers)+1):
        num_set.update([int("".join(item)) for item in permutations(numbers, i)])
    answer = 0
    for _, num in enumerate(num_set):
        for j in range(2, int(num**0.5)+1):
            if num%j==0:
                break
        else:
            if num>=2:
                answer+=1
    return answer