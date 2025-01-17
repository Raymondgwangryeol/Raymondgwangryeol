n = int(input())
cnt=1

while n>=cnt*2:
    cnt=cnt*2

print((n-cnt)*2) if n!=cnt else print(n)