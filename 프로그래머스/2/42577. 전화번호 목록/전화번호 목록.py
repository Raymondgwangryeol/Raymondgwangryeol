def solution(phone_book):
    phone_dict={item:True for item in phone_book}
    for number in phone_book:
        for i in range(1, len(number)):
            if number[:i] in phone_dict:
                return False 
        
    return True