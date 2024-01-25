# Entity Framework
## ORM과 Entity Framework(EF)
C#언어와 같은 객체 지향형 프로그래밍(OOP) 언어에서 DB를 쉽게 사용하기 위한 ORM(Object Relational Mapping).
객체(Object)와 관계형(Relational) DB의 테이블을 매핑(Mapping)하여 (ADO.NET처럼 별도의 SQL 쿼리를 쓰지 않아도)쉽게 데이터에 접근할 수 있다.

Microsoft가 구현한 ORM으로는 EF, LINQ가 있고, 이 외에도 Dapper같은 다른 .NET용 ORM 오픈소스들이 있다.

EF가 ADO.NET에서만 사용하는 기술은 아니다.
하지만, ASP.NET MVC에서 데이터를 엑세스 하는 기본 프레임워크로 EF를 사용하고 있기 때문에, MVC를 사용하면 자연스럽게 EF도 쓴다.

## Entity Framework 접근 모델
1. Model First
   - Visual Studio의 Visual Model Designer(EDMX) 사용.
   - 기존 DB가 없을 때, 직접 Visual Model Designer에 직접 Entity들을 하나씩 추가하며 데이터 모델을 구성하는 방법.
2. Database First
   - Visual Studio의 Visual Model Designer(EDMX) 사용.
   - 기존 DB로부터 테이블 구조들을 읽어 Visual Model을 구성.
3. Code First
   - Visual Model Designer(EDMX)를 사용하지 않고, 데이터 모델을 C#클래스로 직접 코딩하는 방식
Entity Framework Core는 Code First 모델을 지원한다.

## Code First 모델
이 접근 방식은 DB가 미리 설계되지 않아도 사용할 수 있는 모델이다.
C# 클래스들로 Domain Object를 정의해놓고, 프로그램 실행 시 만약 DB가 없으면 자동으로 DB를 생성해준다.

### 1. C# 클래스로 테이블 구조 정의하기 (이미 설계된 DB구조가 있는 경우 생략)
   아래와 같이 간단한 속성들로 정의되어, 외부 Framework에 의존하지 않는 단순한 Entity 클래스를 POCO(Plain Old CLR Object) 클래스라고 함.
   단순히 데이터를 저장했다가 전달하는 역할을 함.
   
   ```
   
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
     
    namespace GuestBook.Models
    {
        public class GuestDbContext : DbContext //Entity Framework에서 파생된 DbContext 클래스 상속.
        {
            public GuestDbContext() : base() //base(): 처음 GuestDbContext 객체 생성시, Web.config의 정의에 따라 해당 클래스 이름을 따서 local DB파일(*.mdf)로 만듦.
            {
            }
     
            public DbSet<Guest> Guests { get; set; }        
        }
     
        [Table("Guest")] //생성될 Table의 이름을 정하는 속성. 별도의 선언이 없으면, Default로 class이름의 복수형으로 지정됨.(해당 예제에서는 Guests)
        public class Guest
        {
            //클래스의 각 속성들은 Guest 테이블에 1:1로 매핑된다.
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime CreateDate { get; set; }
            public string Message { get; set; }
        }
    }

   ```


### 2. LINQ 사용
   위의 에제와 같이 DbContext와 Table class가 정해지면, LINQ를 써서 쉽게 SELECT, INSERT, UPDATE, DELETE를 할 수 있다.

   ```

   ///AddGuest() 메서드에서 데이터 INSERT하기
   public ActionResult AddGuest()
   {
       string name = Request["name"];
       string msg = Request["msg"];
    
       var db = new GuestDbContext();
    
       Guest g = new Guest();
       g.Name = name;
       g.CreateDate = DateTime.Now;
       g.Message = msg;
    
       db.Guests.Add(g);
       db.SaveChanges();
    
       return RedirectToAction("ShowGuests");
   }
    
   // Guest 리스트
   //최근 10개의 리스트 출력
   public ActionResult ShowGuests()
   {
       var db = new GuestDbContext();
    
       // select top 10 * from guest order by id desc
       List<Guest> guests = db.Guests.OrderByDescending(p => p.Id).Take(10).ToList();
    
       return View(guests);
   }
   ```
