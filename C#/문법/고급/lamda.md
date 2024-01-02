## Lambda Expression(람다식)
_C# 3.0부터 지원_

</br>
무명 메서드와 비슷하게 무명 함수를 표현하는데 사용됨.
</br>

**(입력 파라미터) => (실행문장 블럭);**
</br>

예시) 문자열을 받아 메시지 박스 띄우기
</br>

```cs
str => {MessageBox.Show(str)};
```
</br>

입력 파라미터는 하나도 없는 경우부터 여러개 있는 경우까지 다양함.
</br>
파라미터의 타입을 명시하지 않아도 컴파일러가 알아서 찾아내긴 하지만, 타입이 모호한 경우 직접 파라미터 타입을 명시해줄 수 있음.
</br>

```cs 
//아래는 진짜 예시임. 이거 오류나는 코드임.
//입력 파라미터 없을 때
() => Write("No");

// 입력 파라미터가 1~2개 있을 때
(p) => Write(p);
(s, e) => {Write(s); Write(e);}

//파라미터 타입 명시
(string s, int i) => Write(s,i);
```
</br>

### 람다식 활용

1. LINQ 쿼리
```cs
  List<Person> people = new List<Person>
   {
                
     new Person {Name = "홍길동", Age = 27 };
     new Person {Name = "둘리", Age = 28 };
   }

   //람다식 사용 부분

   var result = people.Where(x => x.Name == "홍길동");
```

2. 대리자(Delegate)
```cs
    public delegate int Calculator(int a, int b);
       
        public class MyCalculator
        {
            public int Add(int a, int b) => a + b;
        }
    static void Main(string[] args)

        {
            var calc = new MyCalculator();
            Calculator add = (a,b) => calc.Add(a, b);

            Console.WriteLine(add(1, 2));
```

3. 이벤트 핸들러
```cs
   Button button1 = new Button();
   button1.Click += (sender, e) => {Console.WriteLine("Clicked");}
```
4. 스레드 처리
</br>
     코드가 간결해지고, 스레드에서 실행 될 코드를 직접 작성하는 것 보다 효율적인 코드 작성 가능.
     </br>
     더 적은 양의 코드로 멀티스레드를 처리할 수 있어, 복잡한 작업을 처리할 때 유용.
     </br>

```cs
  Thread t = new Thread(()=>
    {
      Console.WriteLine("start");
      Console.WriteLine("end");
    }
