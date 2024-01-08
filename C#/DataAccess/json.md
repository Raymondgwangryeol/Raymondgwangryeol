## JSON 이란?
JSON(JavaScript Object Notation)은 두 Device 간의 데이터 전송 포맷중 하나임.
기존의 XML이 네트워크 전송시 좀 무거웠던 것에 대한 대안으로 많이 쓰이고 있음.
Name-Value Pair 포맷 형식
  이름-값 쌍을 대괄호로 묶어서 하나의 JSON 객체를 만듦.
{"Id" : 1, "Name" :  "Kim"}

## JSON 배열
여러 JSON 객체를 배열로 묶은 것. 묶을 때 객체 값들은 ,로 구분 됨.
    { "Id" : 1, "Name" : "Lee" },
    { "Id" : 2, "Name" : "Kim" },
    { "Id" : 3, "Name" : "Park" }

## JavaScriptSerializer 클래스
.NET 3.5부터 사용
.NET Application에서 JSON을 간단하게 사용할 수 있는 .NET에 Built-in된 클래스.
3.5버전 미만인 경우, JSON.NET을 사용함.

.NET 객체를 JSON 포맷 문자열로 만드는 것을 Serializaton이라고 함.
 Serializaton을 위해 JavaScriptSerializer의 Serialize()메소드를 사용.
 JSON 문자열 -> .NET 객체로 복원하는(Deserialization) 메소드는 Deseralize().

 ```cs
  namespace jsonApp
  {    
      // 1. System.Web.Extensions.dll 참조 (.NET 3.5+)
      using System.Web.Script.Serialization;
  
      // JavaScriptSerializer 를 이용한 Json 예제
      class JsonUsingJavaScriptSerializer
      {        
          public void DoTest()
          {
              // 2. JSON string 만들기
              var p = new Person { Id = 1, Name = "Alex" };
              var jss = new JavaScriptSerializer();
              string jsonString = jss.Serialize(p);
              System.Diagnostics.Debug.WriteLine(jsonString);
  
              // 3. JSON string으로부터 Object 가져오기
              Person pObj = jss.Deserialize<Person>(jsonString);
              System.Diagnostics.Debug.WriteLine(pObj.Name);
          }
      }
  
      class Person
      {
          public int Id { get; set; }
          public string Name { get; set; }        
      }
  }

```

### JavaScriptSerializer의 문제점
1. 날짜형 데이터 포맷 처리
  - 다른 많은 Serializer가 사용하는 ISO 8601을 따르지 않고, Unix timestamp에 기초한 MS 고유 날짜 포맷으로 출력함.
  - Microsoft Ajax Library를 사용하면 자동으로 JavaScript Date로 바꿔줌.
  - Microsoft Ajax Library를 사용 안 한다면 JavaScript에서 받는 JSON 날짜 데이터 포맷을 변형해줘야 함.

2. 순환 참조(Circular Reference) 핸들링 불가
   - A가 B를 참조하고, B가 C를 참조하고, C가 A를 참조하고.....
   - 순환 참조에서 JSON 사용하려면, JavaScriptSerializer 대신 JSON.NET 이용
   

## JSON.NET
.NET에서 JSON을 사용할 때 가장 널리 사용되는 오픈소스.
dll을 별도로 설치해야 하지만, JavaScriptSerializer 나 DataContractJsonSerializer보다 훨씬 많은 Feature를 가지고 있음.
특히 LINQ를 사용할 수 있는 기능을 제공하고 있음.

[설치 방법]
1. http://james.newtonking.com/json에서 다운
2. NuGet에서 직접 설치

### JSON.NET 사용
[사용 방법]
1. Newtonsoft.Json.dll 참조 추가
2. uisng Newtonsoft.Json 네임스페이스 사용

JSON.NET Seralization -> JsonConvert.SerializeObject() 사용
JSON.NET Deseralization -> JsonConvert.DeserializeObject<T>() 사용

```cs
  namespace jsonApp
  {    
      // 1. JSON.NET 설치 (NuGet)
      //     PM> install-package Newtonsoft.Json
      //
      // 2. Newtonsoft.Json.dll 참조 (.NET 버젼별로 다름)
      using Newtonsoft.Json;
      
      // JSON.NET 를 이용한 Json 예제
      class JsonUsingJSONNET
      {
          public void DoTest()
          {
              // 2. JSON string 만들기
              var p = new Person { Id = 1, Name = "Alex" };
              string jsonString = JsonConvert.SerializeObject(p);            
              System.Diagnostics.Debug.WriteLine(jsonString);
  
              // 3. JSON string으로부터 Object 가져오기
              Person pObj = JsonConvert.DeserializeObject<Person>(jsonString);
              System.Diagnostics.Debug.WriteLine(pObj.Name);
          }
      }
  }

```


### LINQ to JSON과 dynamic 사용법
JSON to SQL은 LINQ를 사용할 수 있는 LINQ to JSON 기능을 추가했음.
LINQ to JSON은 JSON의 특정 노드 값을 선택적으로 읽거나, 새로운 JSON을 만들 때 유용하게 사용될 수 있음.

[예제 1]
JSON 문자열을 파싱한 후 LINQ 사용.
JObject.Parse()는 JSON 문자열을 파싱해서 JSON 전체를 JObject로 리턴함. JObject 객체의 각 노드들에 접근해서 JToken 객체를 얻을 수 있고, LINQ 쿼리를 써서 노드에서 필요한 데이터를 읽어올 수 있음.
데이터 타입을 미리 C#안에서 지정하지 않았을 때 유용하게 사용 가능한 방법.

```cs
using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Program
{
    static void Main(string[] args)
    {
        string str = @"{ 
            Id: 101,                
            Phone: ['010-123-3456', '02-2222-3333', '010-222-1121']
        }";

        // 예제 1 : LINQ to JSON
        // JSON 문자열을 파싱하여 JObject를 리턴
        JObject jo = JObject.Parse(str);

        // JObject 인덱서를 사용하여 특정 Token을 리턴
        JToken idToken = jo["Id"];
        int id = (int)idToken;
        string phone1 = jo["Phone"][0].ToString();
        Console.WriteLine("{0}:{1}", id, phone1);

        var cell = jo["Phone"].Select(p => p.ToString().StartsWith("010"));
    }
}
```


[예제 2]
JSON 문자열을 JsonConvert.DeserializeObject()로 Deserialize시킨다음, dynamic 객체에 집어넣기

```cs
using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Program
{
    static void Main(string[] args)
    {
        string str = @"{ 
            Id: 101,                
            Phone: ['010-123-3456', '02-2222-3333', '010-222-1121']
        }";

        // 예제 2 : dynamic 
        dynamic jobj = JsonConvert.DeserializeObject(str);
        var xid = jobj.Id.ToString();
        var xphone1 = jobj.Phone[1].ToString();
        Console.WriteLine("{0}:{1}", xid, xphone1);
    }
}

```


## WCF에서 JSON 사용하기
WCF는 옵션에 따라 데이터를 XML 또는 JSON현태로 클라이언트에게 전달함.
만약 WCF 서버 코드에서 ResponseFormat이 JSON으로 설정된 경우에는 따로 JSON코드를 안 넣어도 되는데, 개발자가 직접 JSON을 핸들링 할 경우 DataContractJsonSerialiser 클래스를 씀
DataContractJsonSerialiser는 WCF를 위해 만들어진 클래스지만, System.Runtime.Serialization.dll을 참조하면 윈폼이나 WPF등의 일반 C#프로그램에서도 쓸 수 있음.

[예제 - DataContractJsonSerialiser로 .NET객체 ⇆ JSON 변환]
```cs
using System.IO;
namespace jsonApp
{
    // 1. System.Runtime.Serialization.dll 참조
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json; 

    class JsonUsingDataContractJsonSerializer // WCF
    {
        public void DoTest()
        {
            // 2. JSON string 만들기
            var p = new Person2 { Id = 1, Name = "Alex" };

            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Person2));
            MemoryStream mem = new MemoryStream();

            // Memory에 Person2 객체가 Serialize 되어 저장됨
            // 이 메모리스트림은 네트워크나 디스크에 전송
            js.WriteObject(mem, p);

            // 메모리스트림으로부터 JSON string을 만들 경우는:
            string jsonString = string.Empty;
            mem.Position = 0; // 중요! 메모스트림 0부터 읽겠다
            using (StreamReader sr = new StreamReader(mem))
            {
                jsonString = sr.ReadToEnd();
                System.Diagnostics.Debug.WriteLine(jsonString);
            }

            // 3. JSON string으로부터 Object 가져오기
            using (MemoryStream memJs = new MemoryStream())
            {
                using (StreamWriter wr = new StreamWriter(memJs))
                {
                    wr.Write(jsonString);
                    wr.Flush(); // close하지 않고 buffer에 저장해두겠다
                    memJs.Position = 0;
                    Person2 pObj = (Person2)js.ReadObject(memJs);
                    System.Diagnostics.Debug.WriteLine(pObj.Name);
                }                
            }
        }
    }

    [DataContract]
    class Person2
    {
        [DataMember]
        public int Id;
        [DataMember]
        public string Name;
    }
}
```
