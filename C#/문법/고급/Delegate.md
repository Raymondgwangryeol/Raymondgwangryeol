# Delecate
</br>
공부 자료 : 예제로 배우는 C# 프로그래밍    
</br></br>

### 기초 개념
  Method를 다른 Method의 인자로 전달하기 위해 만들어 졌다.</br>
  함수 인자로 숫자 또는 객체를 전달하듯이, 메서드 또한 인수로서 전달 가능하다. (복수개의 Method도 전달 가능!)</br>
  Method 리턴도 가능.</br>
  </br>
  #### +) Class를 인자로 넘기면? </br>
  - Class 객체는 데이터(필드)와 행위(메서드)로 이뤄져 있음.
  - 즉, delecate를 통해 클래스를 인자로 보낸다는 건, 클래스에 포함된 메서드를 인자로 넘긴다는 의미
  </br><img src="https://www.csharpstudy.com/CSharp/Image/delegate-concept.png"></img></br>
  </br></br>
  delegate 선언식은 C# 컴파일러에 의해 System.MulticastDelegate 클래스로부터 파생된 MyDelegate 클래스를 생성함.</br>
  </br>
  
  ### ■ Delegate 정의
  - 입력 파라미터들과 리턴 타입이 중요함.</br>
  => 델리게이트 메서드와 Prototype이 일치한 메서드 사용 가능.</br>
  - delegate는 결국 클래스이므로, 클래스 객체 생성과 같은 방식을 사용</br>
  - run = new RunDelegate(RunThat); 을 줄여서 run=RunThat;이라고 써도 됨.
     </br>
     
#### [생성]
  </br>

   ```cs
  
   // int StringToInt(string s) { ... }

   MyDelegate m = new MyDelegate(StringToInt);
   Run(m);
   ```
    
</br>
    
   #### [호출] 
   </br>

 ```cs
      
        i = m.Invoke("123");
        // .Invoke();나 .Begininvoke(); 메서드 사용
     
        i = m("123"); //.Invoke() 생략하고 이렇게 해도 됨.
    
  ```
    
 </br>
    
   #### [ 예제 ]

</br>

    
  ```cs
      
      class Program
  {
      static void Main(string[] args)
      {
          new Program().Test();
      }
  
      // 델리게이트 정의
      delegate int MyDelegate(string s);
  
      void Test()
      {
          //델리게이트 객체 생성
          MyDelegate m = new MyDelegate(StringToInt);
  
          //델리게이트 객체를 메서드로 전달
          Run(m);
      }
  
      // 델리게이트 대상이 되는 어떤 메서드
      int StringToInt(string s)
      {
          return int.Parse(s);
      }
  
      // 델리게이트를 전달 받는 메서드
      void Run(MyDelegate m)
      {
          // 델리게이트로부터 메서드 실행
          int i = m("123");
  
          Console.WriteLine(i);
      }
  }
  
  ```
    
### ■ Delegate 파라미터
- delegate를 인수로 받은 메서드는 delegate를 내부함수 호출하듯 쓸 수 있음.
- delegate 클래서가 지원하는 속성과 메서드 또한 사용 가능.
</br>
 
 #### [ 예제 ]

</br>

    
```cs
      
     class MySort
{
    // 델리게이트 CompareDelegate 선언
    public delegate int CompareDelegate(int i1, int i2);
    //오름차순이든 내림차순이든 일단 비교해서 순서가 반대면 교환하면 됨
    public static void Sort(int[] arr, CompareDelegate comp)
    {
        if (arr.Length < 2) return;
        Console.WriteLine("함수 Prototype: " + comp.Method);

        int ret;
        for (int i = 0; i < arr.Length - 1; i++)
        {
            for (int j = i+1; j < arr.Length; j++)
            {
                ret = comp(arr[i], arr[j]);
                if (ret == -1)
                {
                    // 교환
                    int tmp = arr[j];
                    arr[j] = arr[i];
                    arr[i] = tmp;
                }
            }
        }
        Display(arr);
    }
    static void Display(int[] arr)
    {
        foreach (var i in arr) Console.Write(i + " ");
        Console.WriteLine();
    }
}

class Program
{
    static void Main(string[] args)
    {
        (new Program()).Run();
    }

    void Run()
    {
        int[] a = { 5, 53, 3, 7, 1 };
        
        // 올림차순으로 소트
        MySort.CompareDelegate compDelegate = AscendingCompare;
        MySort.Sort(a, compDelegate);

        // 내림차순으로 소트
        compDelegate = DescendingCompare;
        MySort.Sort(a, compDelegate);            
    }

    // CompareDelegate 델리게이트와 동일한 Prototype
    int AscendingCompare(int i1, int i2)
    {
        if (i1 == i2) return 0;
        return (i2 - i1) > 0 ? 1 : -1;
    }

    // CompareDelegate 델리게이트와 동일한 Prototype
    int DescendingCompare(int i1, int i2)
    {
        if (i1 == i2) return 0;
        return (i1 - i2) > 0 ? 1 : -1;
    }
}
```

</br>

### ■ Delegate 필드, 속성
- delegate도 특정 클래스의 필드나 속성값으로 사용될 수 있음. 일종의 함수 포인터 변수 값이라고 생각하면 됨

</br></br>

### ■ Multicast Delegate
- 연산자 +=로 delegate에 여러 메서드 할당 가능함.
- 추가된 메서드는 .NET MulticastDelegate 클래스에서 리스트(InvocationList)로 관리됨.

</br></br>

### ■ Delegate와 Event
- delegate의 문제점
  1. -=, +=연산이 아닌 할당연산자(=)를 쓸 경우 원래 InwocationList에 있던 게 다 사라지고 새로 덮어써져 할당됨.
  2. 외부에서 직접 호출 너무 가능
- event는 문제점을 보완하고자 다음과 같은 특징을 가짐
  1. 할당 연산자 못 씀, +=, -=만 사용 가능
  2. 해당 클래스 외부에서 직접 호출 못 함

</br>
 
#### [ 예제 ]

</br>

    
```cs
      
     using System.Windows.Forms;
namespace MySystem
{
   class MyArea : Form
   {
      public MyArea()
      {
         // 이 부분은 당분간 무시. (무명메서드 참조)
         // 예제를 테스트하기 위한 용도임.
         this.MouseClick += delegate { MyAreaClicked(); };
      }

      public delegate void ClickEvent(object sender);

      // event 필드
      public event ClickEvent MyClick;

      // 예제를 단순화 하기 위해
      // MyArea가 클릭되면 아래 함수가 호출된다고 가정
      void MyAreaClicked()
      {
         if (MyClick != null)
         {
            MyClick(this);
         }
      }
   }

   class Program
   {
      static MyArea area;

      static void Main(string[] args)
      {
         area = new MyArea();
         
         // 이벤트 가입
         area.MyClick += Area_Click;
         area.MyClick += AfterClick;

         // 이벤트 탈퇴
         area.MyClick -= Area_Click;

         // Error: 이벤트 직접호출 불가
         //area.MyClick(this);

         area.ShowDialog();
      }

      static void Area_Click(object sender)
      {
         area.Text += " MyArea 클릭! ";      
      }

      static void AfterClick(object sender)
      {
         area.Text += " AfterClick 클릭! ";      
      }
   }
}

```

</br></br>

### ■ Delegate와 함수 포인터
 - 함수 포인터: 함수가 어디 있는지만 앎/ 하나의 함수 포인터 가짐/ 자료형 safty 완전히 보장 안 함
 - delegate: 함수가 어디 있는지도 알고, 함수가 어디 담겨(?)있는지도 앎/ Multicast 가능/ 자료형 safty 완전히 보장
