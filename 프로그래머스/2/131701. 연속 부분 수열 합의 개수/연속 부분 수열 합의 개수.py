def solution(elements):
    n = len(elements)
    answer = set()

    # 길이 1부터 n까지의 부분 수열을 확인
    for length in range(1, n + 1):
        for start in range(n):
            # 원형 수열이므로 슬라이싱을 통해 합을 계산
            subarray = elements[start:start + length]
            if len(subarray) < length:  # 원형이므로 앞부분 추가
                subarray += elements[:length - len(subarray)]
            answer.add(sum(subarray))

    return len(answer)