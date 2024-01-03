## C#의 Dynamic Language요소
_C# 4.0부터 추가_
</br>
</br>

#### 정적 언어(Static Language)
   C, C++, JAVA처럼 코드를 짤 때 변수에 무조건 자료형 지정하는 언어. 
   </br>
   자료형을 컴파일 시에 결정함.
   </br>

   [C언어]
   </br>
   int num = 1; // 컴파일 성공
   </br>
   float num2 = 1.03; // 컴파일 성공
   </br>
   bool num3 = 1; // 컴파일 에러
   </br>
   </br>
   
   **장점:** 컴파일 시 타입에 대한 정보를 결정하기 때문에 속도 빠르고, 타입 에러 문제를 초기에 발견할 수 있어서 안정성 높음.
   </br>

#### 동적 언어(Dynamic Language)
   파이썬, 루비, 자바스크립트 같이 자료형 변수에 지정 안 해도 지 혼자 알아서 잘 집어넣는 언어.
   </br>
   컴파일 시 자료형이 정해지는게 아니라, 런타임 시 결정함.
   </br>
   </br>
   
   [Python]
   </br>
   num1 = 10;
   </br>
   name = "Evan Hwang"
   </br>
   </br>
   **장점:** 런타임까지 자료형 결정을 끌고 갈 수 있기 때문에, 많은 선택의 여지가 있다.
   </br>
   **단점:** 배우기는 쉬운데, 실행하다가 중간에 예상 못한 변수 타입이 들어오는 등의 Type Error가 발생할 수 있음.
   </br>
   </br>
   그럼 **C#은 무슨 언어인가요?**
   </br>
       -> **정적 언어**입니다. 근데 **동적 언어의 요소를 곁들인**
</br>
</br>

**추가된 동적 언어 요소**
   </br>
   - dynamic 키워드 추가
   - .NET Framework 4.0에 DLR(Dynamic Language Runtime)추가
       => 다른 동적 언어를 함께 사용하는 게 가능해 졌다.(이게 무슨 말임)
     </br>
     </br>
     </br>


### dynamic 키워드

   컴파일러가 변수의 자료형을 체크하지 않도록 하고, 런타임시까지는 해당 타입을 알 수 없음을 표시한다.
   </br>
   동적 언어인 척 하겠다는 소리.
   </br>
   내부적으로 dynamic 타입은 object 타입을 사용하므로, dynamic 타입의 변수에 아무 자료형이나 때려 박기 가능.
   </br>
   예를 들어 숫자를 할당했다가 갑자기 문자열을 줬다가.... 이런거 가능
   </br>
   </br>

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
   </br>
   </br>

### 익명 타입에 dynamic 사용해보기

   dynamic은 컴파일러에게 하나의 정적 타입으로 인식되기 때문에, 메서드 원형에서도 int나 string처럼 dynamic 파라미터를 지정할 수 있다.
   </br>
   </br>
   **주의!**
   </br>
   1. 익명 타입은 한 번 생성 된 후에는 다시 새로운 속성을 추가할 수 없다.
   2. 익명 타입 자체가 메서드 이벤트 등을 갖지 못한다.
      => 따라서, 이런 익명 타입 멤버를 동적 할당 해서 dynamic 타입에 추가할 수 없다.
   4. Class2가 동일하지 않은 어셈블리에 놓인다면, 에러가 뜬다.
      => dynamic 타입이 예제에서 익명 타입인데, 익명타입은 다른 어셈블리에서 볼 수가 없다.</br>
   </br>
   
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
   </br>
   </br>

### EdpandoObject 클래스 사용 예제
   DLR의 Namespace인 System.Dynamic에서는 중요한 클래스인 ExpandoObject와, DynamicObject가 있음.
   </br>
   DynamicObject: 보다 유연한 Customization을 위한 고급 dynamic 기능 지원
   </br>
   ExpandoObject: Dynamic 타입에 속성, 메서드, 이벤트를 동적으로 쉽게 할당할 수 있도록 도와주는 클래스
   </br>
   </br>

   **[예제]**
   </br>
   
   ```cs
   using System;
   using System.Collections.Generic;
   using System.Text;
   using System.ComponentModel;
   using System.Dynamic;
   
   namespace Basic
   {
       class Event_14
       {
           public void M1()
           {
               // ExpandoObject에서 dynamic 타입 생성
               dynamic person = new ExpandoObject();
   
               // 속성 지정
               // 동적 할당 가능!
               person.Name = "Kim";
               person.Age = 10;
   
               // 메서드 지정
               // 메서드도 할당 가능하다....
               person.Display = (Action)(() => //Action은 리턴값이 없는 함수에 사용되는 Delegate임.
                                               //Action은 파라미터 없음, Action<T>는 파라미터 1개, Action<T1, T2>는 파라미터 2개.... 이런 식
              {
                  Console.WriteLine("{0} {1}", person.Name, person.Age);
              });
   
               person.ChangeAge = (Action<int>)((age) => // 매개변수가 int형 하나인 Action<T> Delegate
               {
                   person.Age = age; // Age 값 갱신
                   if (person.AgeChanged != null) // Age값이 null이 아니면
                       //이미 여기서 AgeChanged가 만들어 져서 아래에서는 확장 메서드로 가나본데??
                   {
                       person.AgeChanged(this, EventArgs.Empty); // 확장메서드.
                     //person.AgeChanged?.Invoke(this, EventArgs.Empty); 
                   }
               });
   
               // 이벤트 초기화
               person.AgeChanged = null; //dynamic 이벤트는 먼저 null 초기화함
   
               // 이벤트핸들러 지정
               person.AgeChanged += new EventHandler(OnAgeChanged);
   
               // 타 메서드에 파라미터로 전달
               M2(person);
   
         
           }
   
   
           private void OnAgeChanged(object sender, EventArgs e)
           {
               Console.WriteLine("Age changed");
           }
   
           // dynamic 파라미터 전달받음
           public void M2(dynamic d)
           {
               // dynamic 타입 메서드 호출 
               d.Display();
               d.ChangeAge(20);
               d.Display();
           }
   
           class program
           {
               public static void Main(string[] args)
               {
                  
                   Event_14 event_ = new Event_14(); // 저거 쓰려면 참조가 필요하다고 해서...
                                                     // class program을 만들고 그 안에 Main을 집어 넣음
                                                     // 아니면 static으로 참조 없이 바로 쓰는 방법도 있는데
                                                     // 일단 Main도 Event_14 안에 있기 때문에...
                                                     // 그리고 static에는 생성자를 포함할 수 없으니까...
                   event_.M1();
                   
               }
           }
       }
       
   }
   
   
   // this는 확장 메서드의 첫 번째 매개 변수에 대한 한정자로도 사용됩니다.
   ```
   
   </br>
   </br>

### ExpandoObject의 dynamic 멤버 보기
   ExpandoObject의 속성, 이벤트, 메서드는 IDictionary 해시 테이블에 저장됨.
   </br>
   ExpandoObject가 IDictionary 인터페이스를 구현하고 있기 때문에, 해당 인터페이스로 캐스팅 해서 내부 멤버 데어터에 접근할 수 있음.
   </br>
   </br>
   
   **[예제 - 위의 예제에 IDictionary 확인 코드만 넣음]**
   </br>
   
   ```cs
   using System;
   using System.Collections.Generic;
   using System.Text;
   using System.ComponentModel;
   using System.Dynamic;
   
   namespace Basic
   {
       class Event_14
       {
           public void M1()
           {
               // ExpandoObject에서 dynamic 타입 생성
               dynamic person = new ExpandoObject();
   
               // 속성 지정
               // 동적 할당 가능!
               person.Name = "Kim";
               person.Age = 10;
   
               // 메서드 지정
               // 메서드도 할당 가능하다....
               person.Display = (Action)(() => //Action은 리턴값이 없는 함수에 사용되는 Delegate임.
                                               //Action은 파라미터 없음, Action<T>는 파라미터 1개, Action<T1, T2>는 파라미터 2개.... 이런 식
              {
                  Console.WriteLine("{0} {1}", person.Name, person.Age);
              });
   
               person.ChangeAge = (Action<int>)((age) => // 매개변수가 int형 하나인 Action<T> Delegate
               {
                   person.Age = age; // Age 값 갱신
                   if (person.AgeChanged != null) // Age값이 null이 아니면
                       //이미 여기서 AgeChanged가 만들어 져서 아래에서는 확장 메서드로 가나본데??
                       //아 윗줄에서 만들어지는 거 맞음!!!!
                       
                   {
                       person.AgeChanged(this, EventArgs.Empty); // 확장메서드.
                     //person.AgeChanged?.Invoke(this, EventArgs.Empty); 
                   }
               });
   
   
               // 이벤트 초기화
               person.AgeChanged = null; //dynamic 이벤트는 먼저 null 초기화함
   
   
   
               // 이벤트핸들러 지정
               person.AgeChanged += new EventHandler(OnAgeChanged); //EventHandler 여기서 할당!
   
   
   
               // 타 메서드에 파라미터로 전달
               M2(person);
   
         
           }
   
   
           private void OnAgeChanged(object sender, EventArgs e)
           {
               Console.WriteLine("Age changed");
           }
   
           // dynamic 파라미터 전달받음
           public void M2(dynamic d)
           {
               // dynamic 타입 메서드 호출 
               d.Display();
               d.ChangeAge(20);
               d.Display();
   
               // 추가된 부분
               // ExpandoObject를 IDictionary로 변환
               var dict = (IDictionary<string, object>)d; //그니까 IDictionary로 바꾸면 ExpandoObject 안에 구현되어있는 IDictionary 인터페이스 부분이 추출(?)되는 거네?
   
               // person 의 속성,메서드,이벤트는
               // IDictionary 해시테이블에 저정되어 있는데
               // 아래는 이를 출력함
               foreach (var dr in dict)
               {
                   Console.WriteLine("{0}: {1}", dr.Key, dr.Value);
               }
               //추가된 부분 끝
           }
   
           class program
           {
               public static void Main(string[] args)
               {
                  
                   Event_14 event_ = new Event_14(); // 저거 쓰려면 참조가 필요하다고 해서...
                                                     // class program을 만들고 그 안에 Main을 집어 넣음
                                                     // 아니면 static으로 참조 없이 바로 쓰는 방법도 있는데
                                                     // 일단 Main도 Event_14 안에 있기 때문에...
                                                     // 그리고 static에는 생성자를 포함할 수 없으니까...
                   event_.M1();
                   
               }
           }
       }
       
   }
   
   
   // this는 확장 메서드의 첫 번째 매개 변수에 대한 한정자로도 사용됩니다.
   ```
