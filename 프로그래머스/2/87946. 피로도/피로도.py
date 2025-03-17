def solution(k, dungeons):
    answer = 0
    dungeons.sort(key=lambda x:-x[0])
    while dungeons:
        if k<dungeons[-1][0]:
            break   
        else:
            use=0
            for i in range(1,len(dungeons)):
                if dungeons[0][0]-dungeons[0][1] <= k-dungeons[i][1]:
                    _, use = dungeons.pop(i)
                    break
            else:
                _, use = dungeons.pop(0)
            if k>=use:
                k-=use
                answer+=1
    return answer if answer>0 else -1