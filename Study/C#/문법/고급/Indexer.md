```cs

  // 예제로 배우는 C# 프로그래밍 참고
  
  using System;
  
  /*
   * Indexer
   
      클래스 객체의 데이터를 마치 배열처럼 인덱스를 사용해서 데이터 access할 수 있게 해줌.
  
      //Indexer를 이요해 문자 data를 클래스 객체에 넣음
      MyClass cls = new MyClass();
      cls[0] = "First";
  
      여러 데이터 타입을 쓸 수 있음.
      int나 string 타입을 사용하여 인덱스 값을 주는 것이 일반적임.
  */
  namespace Basic
  {
      // 인덱서 정의 예시
      // int 인덱스를 받아 배열의 데이터를 찾아서 정수를 리턴
      class MyClass
      {
          //get, set 쓸거니까, private 선언 가능
          private const int MAX = 10;
          private string name;
  
          // 내부의 정수 배열 데이터
          private int[] data = new int[MAX];
  
          // 인덱서 정의. int 파라미터 사용
          public int this[int index] // this[]를 써서 아래와 같이 get, set을 정의
          {
              get
              {
                  if (index < 0 || index >= MAX) // 만약 index가 0보다 작거나 10 이상이면
                  {
                      throw new IndexOutOfRangeException(); // 인덱스 범위 넘어감 예외 던져버리고
                  }
                  else //아니면
                  {
                      // 정수배열로부터 값 리턴
                      return data[index];
                  }
              }
              set
              {
                  if (!(index < 0 || index >= MAX))
                  {
                      // 정수배열에 값 저장
                      data[index] = value;
                      Console.WriteLine("The value: " + data[index] + " Index:" + index);
                  }
              }
          }
      }
 
 
      class indexer_01
       {
           //클래스를 배열처럼 사용
           static void Main(string[] args)
           {
               MyClass cls = new MyClass();
   
               // 인덱서 set 사용
               cls[1] = 1024;
   
               // 인덱서 get 사용
               int i = cls[1];
   
               Console.WriteLine(i);
           }
       }
   }

```
