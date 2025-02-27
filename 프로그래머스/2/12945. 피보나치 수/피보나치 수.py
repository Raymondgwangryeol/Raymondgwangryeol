def solution(n):
    nums=[0]*100001
    nums[1]=nums[2]=1
    for i in range(3, n+1):
        nums[i] = nums[i-1]+nums[i-2]
    return nums[n]%1234567