```cs

using System;
namespace Advanced
{
    /*
       1. static method
       - 클래스로부터 객체를 생성(new)하지 않고 바로 호출하는 메서드.(클래스 명.메서드 명)
       - static 키워드 사용, 인스턴스 객체로부터 호출될 수 없고, 인스턴스 객체 멤버를 참조해서는 안 됨.
    */
    public class MyClass
    {
        private int val = 1;

        // 인스턴스 메서드
        public int InstRun()
        {
            return val; // val 참조
        }

        // 정적(Static) 메서드
        public static int Run()
        {
            return 1; // val 참조 못 함
        }
    }

    public class Client
    {
        public void Test()
        {
            // 인스턴스 메서드 호출
            MyClass myClass = new MyClass();
            int i = myClass.InstRun();

            // 정적 메서드 호출
            int j = MyClass.Run();
        }
    }

    /*
     2. static 속성, 필드
        -[클래스명.속성명]으로 사용. static 키워드로 정의.
        -인스턴트 생성시 마다 메모리에 새로 적재되는 Non-static 필드
            => static 필드는 클래스가 처음 사용될 때 한 번 초기화 된 후 동일한 메모리를 사용

        // static 필드
        protected static int _id;

        // static 속성
        public static string Name { get; set; }

     3. static class
        -모든 class 멤버가 static인 친구
        -static은 객체 생성이 불가함.
            => 따라서 public 생성자는 못 가지지만 static 생성자는 가질 수 있음.
            => static 생성자는 static 필드들을 초기화 할 때 사용
        -장점: 객체 안 만들어도 됨

    // static 클래스 정의
    public static class MyUtility
    {
       private static int ver;

       // static 생성자
       static MyUtility()
       { 
          ver = 1;
       }

       public static string Convert(int i)
       {
          return i.ToString();
       }

       public static int ConvertBack(string s)
       {
          return int.Parse(s);
       }
    }

    // static 클래스 사용
    static void Main(string[] args)
    {
       string str = MyUtility.Convert(123);
       int i = MyUtility.ConvertBack(str);
    }
     */
}

```
