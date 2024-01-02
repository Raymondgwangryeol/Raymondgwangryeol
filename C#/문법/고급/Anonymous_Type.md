## Anonymous Type(익명 타입)
_C# 3.0부터 제공_
</br>
</br>

클래스를 미리 정의하지 않고 사용할 수 있게 하는 타입.
</br>
new{...}와 같은 형식을 사용. 안에 속성 =  값 할당을 한다. 각 속성=값은 ,로 구분한다.
</br>
</br>


'''cs
    // 익명 타입
    
    var t =new { Name="홍길동", Age=20 };
    string s = t.Name;
  
'''


</br>
공식적으로 클래스를 정의할 필요 럾이 임시로 만들어 사용할 때 유용함.
</br>
특히, LINQ를 사용할 때 많이 사용됨.
</br>
</br>

예제) LINQ의 Where()를 이용해 특정 조건의 data를 찾고, Select()로 일부 컬럼들로만 구성된 새 익명 타입을 만들어 리턴하기
</br>



''' private void RunTest()
{
  var v new[] {
    new { Name="Lee", Age=33, Phone="02-111-1111" },
    new { Name="Kim", Age=25, Phone="02-222-2222" },
    new { Name="Park", Age=37, Phone="02-333-3333" },
  }

 // LINQ Select를 이용해 Name과 Age만 갖는 새 익명타입 객체들을 리턴. 
  var list = v.Where(p => p.Age >= 30).Select(p => new {p.Name, p.Age});
  foreach (var a in list)
  {
    Debug.WriteLine(a.Name + a.Age);
  }
}

'''
