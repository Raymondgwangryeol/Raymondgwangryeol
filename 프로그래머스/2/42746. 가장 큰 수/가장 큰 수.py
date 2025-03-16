def solution(numbers):
    answer = ''.join(sorted(map(str, numbers), key=lambda x: x * 10, reverse=True))
    return "0" if answer[0] == "0" else answer