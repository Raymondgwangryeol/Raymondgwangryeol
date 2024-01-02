C#의 Dynamic Language요소
C# 4.0부터 추가

정적 언어(Static Language)
C, C++, JAVA처럼 코드를 짤 때 변수에 무조건 자료형 지정하는 언어.
자료형을 컴파일 시에 결정함.

[C언어]
int num = 1; // 컴파일 성공
float num2 = 1.03; // 컴파일 성공
bool num3 = 1; // 컴파일 에러

장점: 컴파일 시 타입에 대한 정보를 결정하기 때문에 속도 빠르고, 타입 에러 문제를 초기에 발견할 수 있어서 안정성 높음.

동적 언어(Dynamic Language)
파이썬, 루비, 자바스크립트 같이 자료형 변수에 지정 안 해도 지 혼자 알아서 잘 집어넣는 언어.
컴파일 시 자료형이 정해지는게 아니라, 런타임 시 결정함.

[Python]
num1 = 10;
name = "Evan Hwang"

장점: 런타임까지 자료형 결정을 끌고 갈 수 있기 때문에, 많은 선택의 여지가 있다.
단점: 배우기는 쉬운데, 실행하다가 중간에 예상 못한 변수 타입이 들어오는 등의 Type Error가 발생할 수 있음.

그럼 C#은 무슨 언어인가요?
정적 언어입니다. 근데 동적 언어의 요소를 곁들인

추가된 동적 언어 요소
- dynamic 키워드 추가
- .NET Framework 4.0에 DLR(Dynamic Language Runtime)추가
    => 다른 동적 언어를 함께 사용하는 게 가능해 졌다.(이게 무슨 말임)


dynamic 키워드

컴파일러가 변수의 자료형을 체크하지 않도록 하고, 런타임시까지는 해당 타입을 알 수 없음을 표시한다.
동적 언어인 척 하겠다는 소리.
내부적으로 dynamic 타입은 object 타입을 사용하므로, dynamic 타입의 변수에 아무 자료형이나 때려 박기 가능.
예를 들어 숫자를 할당했다가 갑자기 문자열을 줬다가.... 이런거 가능

```cs
// 1. dynamic은 중간에 형을 변환할 수 있다.

dynamic v = 1;
// System.Int32 출력
Console.WriteLine(v.GetType()); // type 정보를 가져오는 함수. 실행 시점에서 평가됨.
                                // typeof()는 컴파일 시점에 정적으로 평가됨. 주의!

v = "abc";
// System.String 출력
Console.WriteLine(v.GetType());


// 2. dynamic은 cast가 필요없다

object o = 10;
// 틀린표현
// (에러: Operator '+' cannot be applied to operands of type 'object' and 'int')
o = o + 20;
// 맞는 표현: object 타입은 casting이 필요하다
o = (int)o + 20;

// dynamic 타입은 casting이 필요없다.
dynamic d = 10;
d = d + 20;


```


익명 타입에 dynamic 사용해보기

dynamic은 컴파일러에게 하나의 정적 타입으로 인식되기 때문에, 메서드 원형에서도 int나 string처럼 dynamic 파라미터를 지정할 수 있다.
주의!
1. 익명 타입은 한 번 생성 된 후에는 다시 새로운 속성을 추가할 수 없다.
2. 익명 타입 자체가 메서드 이벤트 등을 갖지 못한다.
   => 따라서, 이런 익명 타입 멤버를 동적 할당 해서 dynamic 타입에 추가할 수 없다.
4. Class2가 동일하지 않은 어셈블리에 놓인다면, 에러가 뜬다.
   => dynamic 타입이 예제에서 익명 타입인데, 익명타입은 다른 어셈블리에서 볼 수가 없다.


```cs
// 동일 어셈블리에서 익명타입에 dynamic 사용한 경우
class Class1
{
    public void Run()
    {
        dynamic t = new { Name = "Kim", Age = 25 }; //Name과 Age 변수를 가지고 있는 익명 타입 객체를 dynamic t에 할당.

        var c = new Class2(); //Class2를 참조하는 포인터를 c가 가리키고 있다고 생각하면 말이 되긴 하는데...
                              //var이어서 가능한 듯?       
        c.Run(t);
    }
}

class Class2
{
    public void Run(dynamic o) //다이나믹 타입 객체를 인자로 전달
    {
        // dynamic 타입의 속성 직접 사용
        Console.WriteLine(o.Name);
        Console.WriteLine(o.Age);
    }
}
```
