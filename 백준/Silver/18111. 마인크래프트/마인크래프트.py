import sys
from collections import Counter

h, w, block = map(int, sys.stdin.readline().split())

land = []
for _ in range(h):
    land += list(map(int, sys.stdin.readline().split()))

height_count = Counter(land)

min_height = min(height_count.keys())
max_height = max(height_count.keys())

result = float('inf')
best_height = 0

for target_height in range(min_height, max_height + 1):
    remove_blocks = 0
    add_blocks = 0

    for height, count in height_count.items():
        if height < target_height:
            add_blocks += (target_height - height) * count
        elif height > target_height:
            remove_blocks += (height - target_height) * count

    if remove_blocks + block >= add_blocks:
        time = remove_blocks * 2 + add_blocks
        if time < result or (time == result and target_height > best_height):
            result = time
            best_height = target_height

print(result, best_height)