## 확장 메서드(Extention Method)
_C# 3.0부터 지원_

</br>
</br>

특수한 종류의 static 메소드로, 마치 다른 클래스(혹은 구조체)의 인스턴스 메서드인 것 처럼 사용되는 메서드.
</br>
즉, 기존의 클래스를 손대지 않고도 메서드를 추가할 수 있다.
</br>
LINQ에 자주 사용됨.
</br>
</br>

#### 확장 메서드 전제조건
1. static class여야 함.
2. 첫 번째 파라미터로 this를 써줘야 함.(확장 메서드가 사용 될 클래스 타입 지정)
   => 실제로 쓸 때에는 this 값을 따로 넘겨주지 않는다.
   </br>
   </br>

아래 예제는 String 클래스의 확장 메서드를 정의하고, 이를 응용하는 예제이다.
</br>
확장메소드는 대문자를 소문자로 바꾸고, 소문자를 대문자로 바꾸는 메소드임
</br>

```cs
using System;
using System.Text;

namespace MySystem
{
   // static class를 정의. static 메소드는 static class 안에서 정의되어야 함.
   public static class ExClass
   {
      // static 확장메서드를 정의. 첫번째 파라미터는
      // 어떤 클래스가 사용할 지만 지정. 
      public static string ToChangeCase(this String str) //지정하면 나중에 인텔리센스에 뜬다
      {
         StringBuilder sb = new StringBuilder(); // stringbuilder=> 변경할 수 있는 문자열을 나타냄. string은 변경 못 함.
         foreach (var ch in str) // 순환 가능한 item의 내부 인덱스 끝까지 돌아주는 for문
         {
            if (ch >= 'A' && ch <= 'Z')
               sb.Append((char)('a' + ch - 'A'));
            else if (ch >= 'a' && ch <= 'z')
               sb.Append((char)('A' + ch - 'a'));
            else
               sb.Append(ch);
         }
         return sb.ToString(); // string을 확장하는거니까 string 형식으로 바꿔줌. stringbuilder랑 string이랑 다른 타입인 듯.
                               // 물론 string끼리 +하면 더해지는거 맞는데, 원래 있던 자리에 더해지는 게 아니고 아예 새로운 영역에 값을 집어넣어서 변수를 참조하게 만듦. 때문에 연산에서 많은 시간과 자원을 사용하게 될 수 있음
                               // 단, equals를 쓰고싶다면 ToString써서 string으로 변환한 다음에 비교해야함. 안 그러면 st1 == st2랑 다를게 없음. equal은 StringBuilder로 오버라이딩이 안 된다고 함.
      }

      // 이 확장메서드는 파라미터 ch가 필요함
      public static bool Found(this String str, char ch)
      {
         int position = str.IndexOf(ch); //문자열 내에서 지정된 문자열이 처음 등장하는 인덱스를 말함. 안 나오면 -1
         return (position >= 0); //true, false를 리턴받아야 되니까 저렇게 씀. 
      }
   }

   class Program
   {
      static void Main(string[] args)
      {
         string s = "This is a Test";
         
         // s객체 즉 String객체가
         // 확장메서드의 첫 파리미터임
         // 실제 ToChangeCase() 메서드는
         // 파라미터를 갖지 않는다.
         string s2 = s.ToChangeCase();

         // String 객체가 사용하는 확장메서드이며
         // z 값을 파라미터로 사용
         bool found = s.Found('z');
      }
   }
}


```
</br>
</br>
</br>

### 확장 메서드 예제(With LINQ)

System.Linq.Enumerable 클래스는 LINQ 쿼리에서 사용되는 많은 **확장 메서드**들을 포함하는 클래스다.(Where() 같은?) 
</br>

```cs
// LINQ에 정의된 Where 확장메서드

public static IEnumerable<TSource> Where<TSource>(
    this IEnumerable<TSource> source, // IEnumerable<T> 인터페이스를 지원하는 모든 type에 사용 가능
                                      // IEnumerable<T> 인터페이스란 List, Stack, Queue와 같은 컬렉션에 반복이 필요한 경우 사용되는 인터페이스
                                      // 즉, 리스트, 스택, 큐 대환영이라는 뜻
    Func<TSource, bool> predicate  // Func라는 Delegate를 받아들이겠다는 뜻. 주로 LINQ 쿼리를 람다식으로 표현해서 넣음.
)
```
</br>
</br>

**예제1.** Where()을 이용해서 리스트 요소중 A로 시작되는 문자열 뽑아내기
```cs
// Where 확장메서드를 List<T>에서 사용
List<string> list = new List<string> {"Apple", "Grape",  "Banana"};
IEnumberable<string> q = new IEnumberable<string> list Where(p => p.StartsWith("A")); // 아니 이 p는 도대체 뭐야...
```
</br>
</br>

**예제2.** Where()을 이요해 3으로 나눠 떨어지는 애들만 리스트에서 뽑아내기
```cs
tatic void Main(string[] args)
{
  List<int> nums = new List<int> { 55, 44, 33, 66, 11 };

  // Where 확장 메서드 정수 리스트에 사용
  IEnumberable v = new IEnumberable (Where(p => p % 3 == 0)); //이게 쿼리라서 가능한 걸 수도...?

  // IEnumerable<int> 결과를 정수리스트로 변환
  List<int> arr = v.ToList<int>(); //LINQ 결과를 리스트 또는 배열로 반환

  // 리스트 출력
  arr.Foreach(n => Console.WriteLine(n));
}
```
