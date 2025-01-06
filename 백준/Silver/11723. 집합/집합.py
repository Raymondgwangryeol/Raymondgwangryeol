import sys

def add_set(s,x):
    s.add(x)
    return s

def remove_set(s,x):
    try:
        s.remove(x)
    except:
        pass
    return s

def check(s,x):
    return 1 if x in s else 0

def toggle_set(s,x):
    s.add(x) if check(s,x)==0 else s.remove(x)
    return s

def empty_set(s):
    s.clear()
    return s

def all_set(s):
    s.clear()
    s.update(range(1,21))
    return s

n = int(input())
s=set()
for _ in range(n):
    command = sys.stdin.readline().strip().split()
    keyword = command[0]

    if keyword == 'add':
        s=add_set(s,int(command[1]))
    if keyword == 'remove':
        s=remove_set(s,int(command[1]))
    if keyword == 'check':
        print(check(s,int(command[1])))
    if keyword == 'toggle':
        s=toggle_set(s,int(command[1]))
    if keyword == 'all':
        s=all_set(s)
    if keyword == 'empty':
        s=empty_set(s)