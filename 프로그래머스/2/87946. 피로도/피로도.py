def solution(k, dungeons):
    from itertools import permutations
    
    max_explored = 0
    
    for order in permutations(dungeons, len(dungeons)):  # 가능한 모든 던전 순서
        cur_k = k
        count = 0
        for min_required, fatigue in order:
            if cur_k >= min_required:  # 최소 필요 피로도 충족하면 탐험 가능
                cur_k -= fatigue
                count += 1
            else:
                break  # 더 이상 탐험할 수 없음
        max_explored = max(max_explored, count)  # 최댓값 갱신
    
    return max_explored
