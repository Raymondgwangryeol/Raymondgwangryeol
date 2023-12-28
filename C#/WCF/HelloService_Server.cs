/*
   WCF(Windows Communication Foundation)

   간단히 정리하자면,

   서버와 클라이언트간(프로세스 간) 통신을 위한 미들웨어(?)
   서버의 함수들을 클라이언트에서 쉽게 호출할 수 있게 해줌.

   서버와 클라이언트한테 각각 End-Point를 만들어 주고, End-Point를 징검다리 삼아 통신하는 방식? 인것 같다

   양쪽 End-Point를 잘 설정해야 돌아간다.

   End-Point는 ABC로 구성되어 있는데,
   A: Address
   B: Binding => 어떻게 접속할지 (뭐 tcp를 쓸 거냐 udp를 쓸 거냐...)
   C: Contract => 접속에서 무엇을 할 지

   WCF를 위해서는 System.ServiceModel을 추가해줘야 한다.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel;
using HellowWorldService;

namespace WCF_test
{
    public partial class Form1 : Form
    {
        private ServiceHost host;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void send_Click(object sender, EventArgs e) //버튼을 누르면 host가 내가 설정한 함수를 실행할 수 있다.
        {
      

            //서비스 호스트 구현. 여기서 구현 안 하면 사용자가 요청할 때만 접속할 수 있게 됨.
            host = new ServiceHost(typeof(HelloWorldWCFService), new Uri("http://127.0.0.1:8080/WCFHello"));
            //바인딩은 서비스와 클라이언트가 네트워크를 통해 통신하기 위한 기능에 대한 명세
            //WCF 프레임워크는 자주 사용되는 형식을 BasicHttpBinding, NetTcpBinding 등을 기본적으로 제공
            //이를 시스템 바인딩이라고 한다
            //주의 VS관리자 권한으로 실행해야 함.


        host.AddServiceEndpoint( // 클라이언트와 통신할 서비스의 종점.
            //종점(endpoint)은 주소, 바인딩, 계약의 요소의 집합체
            //종점은 여러개를 가질 수 있다
                typeof(IHelloWorld), // 서비스 계약
                new BasicHttpBinding(), //서비스 바인딩
                "" //서비스 호스트에 명시된 베이스 주소를 그대로 사용하겠다
                );

            host.Open(); //클라이언트가 호출하면 Listener 구동, 클라이언트 호출을 수신한다.


        }

        private void stop_Click(object sender, EventArgs e)
        {
            host.Close(); // 클라이언트의 모든 호출이 처리되면 제어를 반환한다.
        }
    }
}
