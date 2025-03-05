def divisor(yellow):
    divisors = []
    for i in range(1, int(yellow**0.5) + 1):
        if yellow % i == 0:
            divisors.append((yellow // i, i))
    return divisors

def solution(brown, yellow):
    divisors = divisor(yellow)
    for n, m in divisors:
        if 2 * n + 2 * m + 4 == brown:
            return [n + 2, m + 2]
