## Partial Type
_C# 2.0부터 추가_
</br>
</br>

같은 클래스, 구조체, 또는 인터페이스를 여기저기 찢어놓는데 사용하는(??) 키워드.
</br>

Code Generator가 만든 코드와 사용자가 만드는 코드를 분리하기 위함.
</br>

**예)** 윈폼 Form1.designer.cs, Form1.cs에 Form1 class가 partial로 들어가 있다.
</br>
</br>

**[예제]**
</br>

```cs
// 1. Partial Class - 3개로 분리한 경우
partial class Class1
{
    public void Run() { }
}

partial class Class1
{
    public void Get() { }
}

partial class Class1
{
    public void Put() { }
}

// 2. Partial Struct
// 비록 아래와 같이 필드를 분리하는 것도 가능하지만, 필드끼리 한군데 모아 두는 것이 권장사항이다
partial struct Struct1
{
    public int ID;
}

partial struct Struct1
{
    public string Name;

    public Struct1(int id, string name)
    {
        this.ID = id;
        this.Name = name;
    }
}

// 3. Partial Interface
partial interface IDoable
{
    string Name { get; set; }
}

partial interface IDoable
{
    void Do();
}

// IDoable 인터페이스를 구현
public class DoClass : IDoable
{
    public string Name { get; set; }

    public void Do()
    {
    }
}
```
</br>
</br>


## Partial Method
_C# 3.0부터 추가_
</br>
</br>

#### 전제 조건
1. 반드시 Private Method여야 함.
2. 리턴값이 void여야 함.
</br>

#### 방법
1. 선언 파일 또는 클래스에 body없이 메서드 선언부만 적기
2. 구현 파일 또는 구역에 실제 메서드 구현하기
</br>

만약 구현부분이 없고 선언부분만 있다면, C# 컴파일러가 컴파일 시 해당 메소드를 걍 날려버림. 생략 그 자체
</br>

즉, 구현 부분이 있냐 없냐에 따라 메서드를 통으로 생략할 수 도 있다는 것.
</br>
</br>

**[예제]**
```cs
// Partial Method (C# 3.0)
public partial class Class2
{
    public void Run()
    {
        DoThis();
    }

    // 조건1: private only
    // 조건2: void return only
    partial void DoThis(); // 메서드 디폴트 한정자가 private임.
}

public partial class Class2
{
    partial void DoThis()
    {
        Log(DateTime.Now);
    }
}
```
