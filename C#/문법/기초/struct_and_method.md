## Struct(구조체)
  </br>
  C#에서는 struct를 사용하면 Value Type을 만들고, Class를 사용하면 Reference Type을 만든다.
  </br>
  
  **+)** C에서는 여러 자료형을 묶어서 하나의 자료형처럼 쓰는 형태.(public 느낌) 구조체 안에서 함수 선언 안 됨
  </br>
  **+)** C++에서는 구조체랑 Class랑 기능은 같은데,(둘 다 함수 호출 가능) class는 기본 접근 지정자가 private고, struct는 public이다. 하지만 여기서도 struct는 간단한 데이터들의 모음(Plain Old Data, POD)로 사용한다.
  </br>
  </br>
  C#.NET의 기본 데이터 형들(int, double...)은 다 struct로 정의됨. Value Type은 상속될 수 없고, 상대적으로 간단한 데이터 값을 저장하는데 사용됨.
  </br>
  </br>

### Value Type vs Reference Type

  **Value Type**
    </br>
      - 데이터를 복사해서 파라미터를 전달 (stack)
    </br>
    </br>
  **Reference Type**
    </br>
    - Heap상의 객체에 대한 레퍼런스를 전달
    </br>
    </br>
  
  C# struct는 다른 언어와 마찬가지로 Value Type을 정의하기 위해 사용됨.
  </br>
  클래스보다 상대적으로 가벼운 오버헤드(프로그램 실행 도중 동떨어진 위치의 코드를 실행시켜야 할 때, 추가적으로 시간, 메모리, 자원이 사용되는 현상)를 지님.
  </br>
  C# struct는 상속은 안 되지만, 인터페이스 구현은 가능함.
  </br>
  </br>

#### 구조체 인터페이스 사용시 발생 가능한 오류

  ,,,
  
    MyValue v1 = new MyValue(3);
    IAdd itf = v1 as IAdd;
    itf.AddOne(); //1을 더해주는 함수
    Console.WriteLine(v1.Value); //그대로 3이 나옴. 오류
    
  ,,,
  
  
  </br>
  인터페이스를 갖는 구조체에서 (C# as Operator를 사용하여) 인터페이스를 얻게 되면, 값타입(Value Type)인 struct는 박싱(Boxing)을 일으켜 스택에 있는 struct값을 힙(Heap)으로 복제하여 레퍼런스 객체를 생성하고 이를 인터페이스 포인터에 할당한다.
  </br>
  따라서 인터페이스로 AddOne()을 호출하면 힙에 있는 레퍼런스 객체의 값은 증가하지만 스택에 있는 struct값을 그대로 변하지 않는다.
  </br>
  때문에 인터페이스를 갖는 구조체를 설계할 때는 이러한 발생가능한 시나리오에 특히 주의를 기울여야 한다.
  </br>
  </br>


## Method

  클래스 내에서 일련의 코드 블럭을 실행시키는 함수를 말함. 여러개의 인수를 가질 수 있고, 하나의 리턴값(또는 void)를 가짐.
  </br>
  public과 같은 접근 제한자를 지정할 수 있음.
  </br>
  </br>
  C#에서는 메서드에 인수 전달 시, 디폴트로 값을 복사해서 전달하는 Pass by Value 방식을 따름. 만약 전달된 인수를 메서드 내에서 변경한다고 해도 함수 내에서만 적용됨.
  </br>
  인수를 참조로 전달하고 싶으면, ref 키워드를 사용해서 참조 변수를 넘기면 됨. 인수로 넘기기 전에, 넘길 ref 변수가 사전에 초기화 되어닜어야 함.
  </br>
  </br>
  ref와 비슷한 얘로 out 키워드가 있음. out 키워드로 받아와진 인수는 매서드 내에서 값을 반드시 지정해줘야 함. ref와 달리 사전에 변수를 초기화 할 필요는 없음.
  </br>
  
  
,,,

    // ref 정의
    static double GetData(ref int a, ref double b)
    { return ++a * ++b; }
    
    // out 정의
    static bool GetData(int a, int b, out int c, out int d)
    {
        c = a + b;
        d = a - b;
        return true;
    }
    
    static void Main(string[] args)
    {
        // ref 사용. 초기화 필요.
        int x = 1;
        double y = 1.0;
        double ret = GetData(ref x, ref y);
    
        // out 사용. 초기화 불필요.
        int c, d;
        bool bret = GetData(10, 20, out c, out d);
    }
    
,,,
  
  </br>

### 추가: Named 파라미터
  C# 4.0 부터. 파이썬의 키워드 인수랑 똑같음

</br>

### 추가: Optional 파라미터
  4.0에서부터 어떤 메서드의 파라미터가 디폴트 값을 갖고 있다면, 메서드 호출시 이러한 파라미터를 생략하는 것을 허용하였다. 파이썬과 동일.
  ptional 파라미터는 반드시 파라미터들 중 맨 마지막에 놓여져야 한다.
