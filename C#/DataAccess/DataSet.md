### C# DataSet 클래스
메모리에 적재된(있는) Table들을 가지고 있는 클래스. 서버와 연결을 유지하진 않는다.
</br>
DataSet클래스에 직접 테이블 구조 만들어가지고 데이터 넣고 할 수 있지만,
</br>
일반적으로 DataAdapter(SqlDataAdapter)로 서버에서 통으로 table가져와서 DataSet에 할당한 후 사용함.
</br>
DataSet 객체는 DataGridView에 데이터를 바인딩하여 뿌릴 수 있다.
</br></br></br>

### C# DataTable 클래스
메모리상에 있는 Table을 나타내는 클래스.
</br>
DataTable이 모이면 DataSet임. 보통 DataTable은 DataSet.Tables 컬렉션에 포함되어 사용됨.
</br>
DataSet 안에 있는 table 접근할 때 DataSet.Tables[0] 이런식으로 인덱스를 사용할 수 있음.
</br>
DataSet.Tables["Tag"]처럼 테이블 이름으로 찾기도 가능
</br>
(아.. 차트할 때 그게 이 말이었구나....)
</br></br></br>

### C# DataView 클래스
DataTable을 정렬, 필터링, 펀집, 검색등을 할 때 사용됨.
</br>
table의 일정 부분만 정렬해서 보여준다거나.. 이런
</br>
DataTable은 기본적으로 DefaultView 속성을 가지고 있는데, DataTable에서 기본적으로 제공하는 DataView임. 이걸로 정렬, 편집등을 할 수 있음.
</br></br>


```cs

DataSet ds = new DataSet();

using (SqlConnection conn = new SqlConnection(cn))
{
   conn.Open();
   SqlDataAdapter adpt = new SqlDataAdapter("SELECT * FROM AAA", conn);
   adpt.Fill(ds, "AAA"); //DataSet에 "AAA"넣기(또는 "AAA"처럼 추가) in .NET 6
}

// DataTable.DefaultView를 사용하여
// 필터링 (name컬럼이 L로 시직하는 경우)
DataTable dt = ds.Tables["AAA"];
dt.DefaultView.RowFilter = "name like 'L%'";

dataGridView1.DataSource = dt;


```
</br></br>

### DataSet 병합

DataSet, 또는 DataTable은 다른 애들()이랑 Merge(DataSet, DataTable 혹은 DataRow) 메서드로 합병시키기 가능함.
</br>
Marge()는 테이블의 Primary Key를 기준으로, 병합하려는 각각의 테이블에서 추가/삭제/변경된 record들을 해당 테이블에 병함.
</br></br>

[예시 - 다른 DB의 같은 이름의 Table들을 합치기]
</br>

```cs
  ataSet ds1 = new DataSet();
  DataSet ds2 = new DataSet();
  
  string strConn1 = "Data Source=(local);Initial catalog=pubs;Integrated Security=SSPI;";
  string strConn2 = "Data Source=(local);Initial catalog=pubs2;Integrated Security=SSPI;";
  
  string sql = "SELECT * FROM authors";
  
  // pubs DB에서 authors 테이블 가져오기 (23개 rows).
  SqlDataAdapter adpt1 = new SqlDataAdapter(sql, strConn1);
  adpt1.MissingSchemaAction = MissingSchemaAction.AddWithKey; // DataSet에 Adapter가 Fill 할 때 디폴트로 MissingSchemaAction.Add 사용함. 속성명과 타입을 자동으로 추가
                                                              // AddWithKey하면 Primary Key 정보도 가져와 추가함. 즉, DataSet에 데이타가 Fill 되기 전에 기본 컬럼 스키마와 Primary Key를 미리 정의하게 됨.
                                                              // AddWithKey로 정의한 후 Fill이 실행될 때, 동일한 Primary Key값이 추가되면 Row는 Append되지 않고 Update됨
  adpt1.Fill(ds1); 
  adpt1.Dispose();
  
  // pubs2 DB에서 authors 테이블 가져오기.
  // (24개 rows : 1개 row가 추가되었고, 1 row가 Update됨)
  SqlDataAdapter adpt2 = new SqlDataAdapter(sql, strConn2);
  adpt2.MissingSchemaAction = MissingSchemaAction.AddWithKey;
  adpt2.Fill(ds2); 
  adpt2.Dispose();
  
  // Merge후 authors에는 1개 추가 row와 갱신된 1개 row가 있음.
  Console.WriteLine("Before Merge: {0}", ds1.Tables[0].Rows.Count);
  ds1.Merge(ds2);
  Console.WriteLine("After Merge: {0}", ds1.Tables[0].Rows.Count);
  
  foreach (DataRow r in ds1.Tables[0].Rows)
  {
      Console.WriteLine("{0}: {1} {2}", r[0], r[1], r[2]);
  }
```

</br></br>

### DataSet에 DataTable 합병하기
특정 DataSet에 다른 DataSet에 있던 DataTable들을 합병(진짜 같다 붙인 느낌)하면, 해당 특정 DataSet은 합병된 DataSet에 Table들도 가지게 됨.
</br>
DataSet.Tables[]에 차례로 집어넣는거
</br></br></br>

**[예시]**
</br>

```cs
  DataSet ds1 = new DataSet();
  DataSet ds2 = new DataSet();
  string strConn1 = "Data Source=(local);Initial catalog=pubs;Integrated Security=SSPI;";
  
  // ds1 에는 authors 테이블
  string sql = "SELECT * FROM authors";
  
  SqlDataAdapter adpt1 = new SqlDataAdapter(sql, strConn1);
  adpt1.MissingSchemaAction = MissingSchemaAction.AddWithKey;
  adpt1.Fill(ds1); 
  adpt1.Dispose();
  
  // ds2 에는 titles 테이블과 employee 테이블 2개
  sql = "SELECT * FROM titles; SELECT * FROM employee";
  
  SqlDataAdapter adpt2 = new SqlDataAdapter(sql, strConn1);
  adpt2.MissingSchemaAction = MissingSchemaAction.AddWithKey;
  adpt2.Fill(ds2); 
  adpt2.Dispose(); // ds2에 채웠으니까 없애기
  
  // ds2의 두 테이블을 ds1으로 병합
  ds1.Merge(ds2);
  
  // 병합 후 ds1에는 총 3개의 테이블이 존재
  foreach (DataRow r in ds1.Tables[0].Rows)
  {
      Console.WriteLine(r[0]);
  }
  foreach (DataRow r in ds1.Tables[1].Rows)
  {
      Console.WriteLine(r[0]);
  }
  foreach (DataRow r in ds1.Tables[2].Rows)
  {
      Console.WriteLine(r[0]);
  }
```

</br></br>

### 특정 DataTable 병합
원하는 테이블끼리 병합하고 싶은 경우, DataTable 객체에서 Merge() 호출하여 병합.
</br>
아래의 예제는 DataSet d1에 같은 table이 담긴 authors1, authors2 table을 authors1에 하나로 합치는 코드이다.
</br>
이처럼 한 데이터베이스 안에 동일한 구조의 두 테이블이 있으면, DataSet 하나 만들고 DataSet.Table에 두 개를 집어넣은 다음,
</br>
한쪽으로 몰아주면 된다
</br></br>

**[예제]**

```cs
DataSet ds1 = new DataSet();
string strConn2 = "Data Source=(local);Initial catalog=pubs2;Integrated Security=SSPI;";

// authors 테이블을 authors1 DataTable에 채움
string sql = "SELECT * FROM authors";
SqlDataAdapter adpt = new SqlDataAdapter(sql, strConn2);
adpt.MissingSchemaAction = MissingSchemaAction.AddWithKey;
adpt.Fill(ds1, "authors1"); 

// authors2 테이블을 authors2 DataTable에 채움
sql = "SELECT * FROM authors2";
adpt = new SqlDataAdapter(sql, strConn2);
adpt.MissingSchemaAction = MissingSchemaAction.AddWithKey;
adpt.Fill(ds1, "authors2");

// authors2 DataTable을 authors1 DataTable에 병합
ds1.Tables["authors1"].Merge(ds1.Tables["authors2"]);

foreach (DataRow r in ds1.Tables["authors1"].Rows)
{
    Console.WriteLine("{0}: {1} {2}", r[0], r[1], r[2]);
}
```

