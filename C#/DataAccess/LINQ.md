# LINQ for SQL

LINQ(Language Intergrated Query)란 C# 3.0에 나온 기능으로,

간략한 수식을 사용해 데이터 소스 또는 컬렉션으로부터 데이터를 핸들링 하는데 사용됨.

</br>

어떤 데이터를 쓰냐에 따라 다양한 버전이 있음.

 - .NET Object -> LINQ to Objects
 
 - XML -> LINQ to XML
 
 - SQL DB -> LINQ to SQL
 
 등...

</br>
 
 LINQ 사용하려면 System.Linq 네임스페이스를 추가해야 함.
 
</br>
 LINQ 사용을 위한 준비
 
 - 윈폼 프로젝트에서 추가 -> 새 파일
 
 - .DBML파일(일종의 XML) 만들기 (data -> LINQ to SQL Classes)
 
 - Server Explorer(서버 탐색기)에서 SQL 추가하기
 
   ( Data Connections-> Add Connection -> 연결 추가)
 </br>
 

.dbml 저장하면, VS가 DBML XML을 기초로 LINQ에서 사용될 클래스들을 만들어 .designer.cs파일에 저장함.

우와... MSSQL 함수라니...

</br></br></br>

## LINQ to SQL Query
LINQ 쿼리는 C#의 새로운 키워드 from, where, orderby, group-by, join, select 등을 이용하여 데이타소스에 대해 쿼리를 실행함.

내부적으로 LINQ 쿼리는 결국에 SQL문으로 변환되서 실행됨.
</br>

**[순서]**

**from절 -> where절(과 같은 조건문들) -> 가장 나중에 select문** 
</br>

+) 참고 -  SQL 구문 순서

    SELECT 컬럼명
    
    FROM 테이블명
     
    WHERE 테이블 조건 -> 조건식
     
    GROUP BY 컬럼명  -> 그룹 만들어 줌
     
    HAVING 그룹 조건   -> 주어진 조건에 맞는 그룹들을 추출
     
    ORDER BY 컬럼명 -> 데이터 정렬

</br>

  **[예시]**

```cs
  var names = from m in Members //Members에 있는 값들을 m에다가 꺼내놓는다
        where City=="서울"
        select m.Name;
```
</br></br></br>


# LINQ 쿼리 실행
</br>

LINQ에서 각 테이블에 접근하려면 DataContext 객체가 있어야함. .dbml 파일 생성 시, 파일명과 같은 이름의 DataContext 객체가 생김(디폴트임)

DataContext객체가 생성되면, 각각의 테이블은 해당 객체의 속성으로 들어가고, Stored Procedure/Function은 객체의 메서드로 들어감.

예를 들어 DataContext 객체인 dc1가 있고, 거기에 new라는 속성을 가진 테이블이 들어간 상황이라고 하면,

new라는 속성값에 접근할 때,

dc1.new

이렇게 접근함.
</br>

LINQ 쿼리는 객체 생성시 바로 실행되는게 아니라, foreach같이 반복적으로 돌려질 때 실행되고, 데이터를 가져오게(fetch)됨.
</br></br></br>


## LINQ to SQL: orderby
데이터를 순서대로 정렬하는 쿼리.

오름차순(ascending), 또는 내림차순(descending)으로 정렬할 수 있음.
</br>

**[예제]**

```cs
  private void button1_Click(object sender, EventArgs e)
         {
             var orderData = from Scores in dataContext.Scores
                              orderby Scores.Score descending
                              select Scores; // 전체 row를 통째로 고를 때
             dataGridView1.DataSource = orderData;
             dataGridView1.Show();
         }
```

</br></br></br>

## LINQ to SQL: Anonymous Type 생성
두 필드 이상이 필요하거나, 새로운 필드를 조합해서 만들어 내는 경우 select new를 써서 Anonymos Type를 만들어 줘야 함.
</br>

**[예제]**

```cs
 private void button1_Click(object sender, EventArgs e)
         {
             var orderData = from Scores in dataContext.Scores
                             select new
                             {
                                 score = Scores.Score,
                                 Id = Scores.id
                             };
             dataGridView1.DataSource = orderData;
             dataGridView1.Show();
         }
```

</br></br></br>

## LINQ to SQL: Insert
**[방법]**
</br>

1. LINQ 레코드 클래스 객체 생성
2. InsertOnSubmit() 메서드로 LINQ 테이블 클래스 객체에 레코드 클래스 객체 추가
3. SubmitChanges()로 DataContext객체 갱신

</br>
```cs
 MyDBDataContext db = new MyDBDataContext();
 
 // 새 레코드 객체 생성
 //'Member'라는 table이 .dbml에 포함되면, Member라는 레코드 클래스와 Members라는 테이블 클래스가 생성됨.
 // 테이블 클래스 = 레코드들의 집합이라는 의미로, 복수로 써줌. (Member table => 'Members' table class)
 Member mem = new Member(); 
 mem.MemberId = 1;
 mem.Name = "Alex";
 mem.Address = "Seattle, WA";
 
 // 레코드객체를 테이블객체에 추가
 db.Members.InsertOnSubmit(mem);
 
 // 서버에 전송
 db.SubmitChanges(); 
```
