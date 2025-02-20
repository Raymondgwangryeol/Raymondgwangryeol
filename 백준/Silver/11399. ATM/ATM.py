sum = 0
total = 0
n = int(input())
list_ = list(map(int, input().split()))

list_.sort()

for i in list_:
    sum+=i
    total+=sum

print(total)