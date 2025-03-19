def solution(name):
    answer = 0

    # 알파벳 변경 횟수 계산
    answer += sum(min(ord(c) - ord('A'), ord('Z') - ord(c) + 1) for c in name)

    # 좌우 이동 거리 계산
    length = len(name)
    min_move = length - 1  # 기본적으로 오른쪽으로 쭉 가는 경우

    for i in range(length):
        next_idx = i + 1
        while next_idx < length and name[next_idx] == 'A':  # 연속된 A 찾기
            next_idx += 1
        # 1) 오른쪽으로 갔다가 되돌아오기
        # 2) 왼쪽으로 갔다가 다시 오른쪽으로 이동
        min_move = min(min_move, i * 2 + (length - next_idx), (length - next_idx) * 2 + i)

    answer += min_move
    return answer
