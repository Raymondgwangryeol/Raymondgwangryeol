## ADO.NET의 Connection Polling
</br>
</br>
클라이언트(WAS, 웹 어플리케이션 서버. DB 조회나 다양한 로직 처리를 요구하는 동적인 컨텐츠를 제공하기 위해 만들어짐)가 DB서버랑 통신한다고 치자.
</br>
이 둘의 연결을 위해서는 3-way-handshaking 작업이 필요하다. 만약 DB연결 요청이 많은 프로그램이라면, 네트워크 구간에서 병목현상의 원인이 될 수 있다.
</br>
그럼 계속 handshaking 하지 말고, 한 번 Connection 하면 끊지 말고 일정시간동안 Connection Pool에 저장해 놓았다가 가져다 쓸까?
</br>

=> **Connection Polling**
</br></br>

ADO.NET에서는 디폴트로 Connection Polling ON 해놓음. 최소 **1개에서 100개까지** 저장 가능.(디폴트는 1개)
</br></br></br>

#### [생성과정]
클라이언트 프로그램 실행 -> SqlConnection 객체 만들고 Open -> SQL 서버에 로그인 및 연결.
</br>
마지막 과정 때 SQL 서버에 커넥션과 관련된 리소스가 할당되고, Connection 관련 Context가 생성됨.
</br>
(SQL 서버에서는 이 Connection에 SPID, 서버 프로세스 아이디를 붙임.)
</br>

우리가 생성된 SqlConnection을 쓰고 Close를 했을 때, 사실 Connection은 끊기지 않고 Connection Polling에 들어갔던 것이었어요
</br>
Connection string에 **Min Pool Size=20;** 이런식으로 최소 Pool 크기 조정 가능함.
</br></br></br>

#### [만약 Connection Pool이 다 차면?]
Pool이 다 찬 상태에서 다른 Connection이 들어올 경우, Queue에서 일 다 끝내고 Connection 리턴 될 때 까지 기다림. 근데 기다리는 것도 제한시간이 있음.
</br>
제한시간 안에 자리 안 나면 Exception을 throw하게 됨.
</br>
Queue 안에서 놀고 있는 Connection들도 이용 시간 제한 있음. 시간 끝나면 나가야 함. 이렇게 해서 SQL의 부하를 줄일 수 있다~
</br></br>

SQL 서버에 있던 모든 Connection 정보가 날라가 버렸을 때, 클라이언트는 Connection Pooling의 Connection을 가져와 SQL문 실행할 때 그 때 에러를 알게 됨.(있었는데요, 없어졌습니다)
</br>
SqlConnection.ClearAllPools(); => Pool에 있는 Connection들 지우기
</br></br></br>

**[예제]**
</br>


```cs
  public void TestConnectionPool()
  {
      // 최소 20 Connection 지정
      string strConn = "Data Source=(local);Initial Catalog=pubs;User id=test;Password=1;Min Pool Size=20;";    
  
      using (SqlConnection conn = new SqlConnection(strConn))
      {
          conn.Open();
          var cmd = new SqlCommand("select * from authors", conn);
          SqlDataReader rdr = cmd.ExecuteReader();
          rdr.Close();
      }
  
      // ClearAllPools 호출 이전
      // 결과 : 20 커넥션
      ShowPerfCounter();
      Console.WriteLine("Press Enter.");
      Console.ReadLine();
  
      // ClearAllPools 호출 이후
      // 결과 : 0 커넥션
      SqlConnection.ClearAllPools();
      Console.WriteLine("After ClearAllPools");
      ShowPerfCounter();
  }
  
  private void ShowPerfCounter()
  {
      string processName = Assembly.GetExecutingAssembly().GetName().Name;
      int pid = Process.GetProcessesByName(processName)[0].Id;
      string instanceName = string.Format("{0}[{1}]", processName, pid);
  
      // .NET Data Provider for SqlServer 카테고리 안의
      // NumberOfPooledConnections 카운터 측정
      var counter1 = new PerformanceCounter(".NET Data Provider for SqlServer", "NumberOfPooledConnections", instanceName);            
      var v1 = counter1.NextValue();            
      Console.WriteLine(v1);                                
  }
```

</br></br></br>

### pool 생성 기준
같은 코드를 실행했다고 해도, ***프로세스별, 프로세스 내의 App Domain 별, Connection String별**로 Pool이 생성된다.
</br>
예를 들어 Connection String의 경우 DataSource=(local)이랑 DataSource=MyLocalServer랑 같은 뜻이지만, 똑같은 문자열이 아니기 때문에 다른 Pool이 생성됨.
</br>
같은 옵션인데 순서만 다르게 써도 다른 Pool로 인식함.
</br></br>

**#참고#**
</br>
Conneting Sting이 윈도우즈 인증(Integrated Security=SSPI), 같은 Connnection이어도 **유저가 다르면 유저별**로 Pool이 생성됨.
</br>
  => 윈도우즈 인증보다는 SQL 로그인 하는게 성능 측면에서 더 좋을 수 있음.
  </br></br></br>


### Sql Connection Pooling의 다양한 옵션
Connection Pooling과 관련된 옵션은 Connection string에 다 적으면 된다.
</br>
대표적으로,
</br>
Pooling: Pooling 하는지 안 하는지 여부
</br>
Min Pool Size / Max Pool Size
Connection Timeout
등이 있다.

