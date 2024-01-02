## Generic
</br>
</br>
클래스 정의 시, 클래스의 데이터 타입과 받은 인자들의 데이터 타입을 명시해야 한다.
</br>
하지만, 만약 쓰고 싶은 클래스 내용은 1개인데, 인자로 다양한 데이터 타입 변수가 들어가야 한다면?
</br>
이런 경우, C#의 Generic 타입을 사용할 수 있다.
</br>
</br>

### Generic Type
int, float 같은 데이터 타입을 특정짓지 않고, 나중에 쓸 때 데이터 타입 자체를 Type Parameter로 받도록(< T >) 클래스, 인터페이스, 메서드 등에 붙여 정의함.
</br>
제네릭 타입 클래스 객체를 만들어서 사용 할 때 <>안에 구체적인 데이터 타입을 정해주는 방식.
</br>
실행시, Generic Type으로부터 지정된 타입의 객체(object)를 구체적으로 생성해서 사용하게 됨.
</br>
타입 파라미터는 여러개를 지정할 수도 있음.
</br>


```cs
        
        
 // 어떤 요소 타입도 받아들 일 수 있는
 // 스택 클래스를 C# 제네릭을 이용하여 정의
 class MyStack<T>
      {
       T[] _elements;
       int pos = 0;
                
       public MyStack()
       {
                _elements = new T[100];
       }
                
       public void Push(T element)
       {
                _elements[++pos] = element;
       }
                
               public T Pop()
               {
                       return _elements[pos--];
               }
       }
                
        // 두 개의 서로 다른 타입을 갖는 스택 객체를 생성
        MyStack<int> numberStack = new MyStack<int>();
        MyStack<string> nameStack = new MyStack<string>();

,,,



</br>
</br>


### .NET Generic 클래스들
.NET 프레임워크에는 상당히 많은 제네릭 클래스들이 포함되어 있음.
</br>
특히, System.Collections.Generic 네임스페이스에 있는 모든 자료구조 관련 클래스들은 제네릭 타입임. (List<T>, Dictionary<T>, LinkedList<T> 등)
</br>


```cs


        List<string> nameList = new List<string>();
        nameList.Add("홍길동");
        nameList.Add("이태백");
        
        Dictionary<string, int> dic = new Dictionary<string, int>();
        dic["길동"] = 100;
        dic["태백"] = 90;


'''


</br>
</br>

### Generic Type 제약조건
**where T: 제약 조건**
</br>
Generic Type을 선택할 때, 해당 인수가 어떤 조건을 만족하는 인수여야하는지 where 뒤에 제약 조건을 붙여서 받아올 데이터 타입을 제한할 수 있음.
</br>

```cs

      // T는 Value 타입(struct)
      class MyClass<T> where T : struct 
      
      // T는 Reference 타입(class)
      class MyClass<T> where T : class
      
      // T는 디폴트 생성자를 가져야 함
      class MyClass<T> where T : new() 
      
      // T는 MyBase의 파생클래스이어야 함
      class MyClass<T> where T : MyBase
      
      // T는 IComparable 인터페이스를 가져야 함
      class MyClass<T> where T : IComparable
      
      // 좀 더 복잡한 제약들
      class EmployeeList<T> where T : Employee,
         IEmployee, IComparable<T>, new()
      {
      }
      
      // 복수 타입 파라미터 제약
      class MyClass<T, U> 
          where T : class
          where U : struct
      {
      }

'''
