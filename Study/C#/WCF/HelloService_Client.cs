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
using System.ServiceModel.Description; // 서비스의 종점을 WCF에게 알려주기 위해 필요한 클래스
using HellowWorldService;

namespace Client
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void C_Run_Click(object sender, EventArgs e)
        {
            //uri 생성
            Uri uri_c = new Uri("http://127.0.0.1:8080/WCFHello");

            //종점 생성
            ServiceEndpoint endPoint = new ServiceEndpoint(
                ContractDescription.GetContract(typeof(IHelloWorld)),
                new BasicHttpBinding(), //서버 호스트에서 BasicHttpBinding으로 종점을 지정했기 때문에 여기서도 이걸 쓴다.
                                        //BasicHttpBinding
                                        //WCF(Windows Communication Foundation) 서비스가 ASMX 기반 웹 서비스 및 클라이언트,
                                        //그리고 WS-I Basic Profile 1.1을 따르는 기타 서비스와 통신할 수 있는 엔드포인트를 구성 및 노출하는 데 사용할 수 있는 바인딩을 나타냅니다.
                                        /*
                                         * 어떤 프로토콜을 사용할 것인가?
                                            예) TCP, HTTP, FTP 등

                                            * 데이터 포맷은 어떤 방식을 사용할 것인가?
                                            예) 바이너리, 텍스트, MIME 등

                                            * 네트워킹 보안을 적용할 것인가?
                                            예)SSL, 암호화 등

                                            * 트랜잭션 처리, 비동기 전송 등의 진보된 기능을 사용할 것인가? 
                                         */
                new EndpointAddress(uri_c)
            );

            //채널 생성
            //채널이란, 서비스 바인딩에 정의된 메시지 처리를 담당하는 한 부분이다.
            //전송채널은 전송 프로토콜을 이용해 통신에 대한 처리를 한다
            //트랜잭션 체널은 트랜잭션을 관리하는 채널이다.

            //이런 각각의 기능을 하는 채널들이 바인딩의 정의에 다라 여러개가 쌓여서, 메세지들이 각 채널을 통과하며 각각의 처리과정을 거치게 된다.
            //채널들이 여러개 쌓여있는 것을 채널 스택이라고 한다

            //각각의 채널들은 파이프라인 형태로 메세지를 넘기게 된다
            //그리고 결국 최종적으로는 트랜스포트 채널을 통과하게 된다.
            //채널 파이프라인이란, 채널에서 발생한 이벤트에 대한 이벤트에 대한 처리를 해줄 ChannelHandler 들의 리스트이다.
            //풀어말하면, "채널 파이프라인"은 채널에서 발생한 이벤트가 이동하는 통로이며,
            //이동 하는 이벤트를 처리하는 클래스를 "이벤트 핸들러"라고 한다.
            //이때 이벤트 핸들러를 상속받아서 구현한 구현체들을 "코덱"이라고 한다.

            //채널의 파이프라인을 구성해 주는 것이 ChannelFactory이다.
            //A factory that creates channels of different types that are used by clients to send messages to variously configured service endpoints.
            ChannelFactory<IHelloWorld> factory = //IHelloWorld 타입의 채널을 선언한다
                new ChannelFactory<IHelloWorld>(endPoint);
            IHelloWorld proxy = factory.CreateChannel(); //이걸 IHelloWorld 객체에 집어넣네..?
                                                         // 이거 proxy라는 IHelloWorld 객체를 오버라이딩한 거라고 생각하면 되는 것 같은데.. 정확하진 않음.
             
            //proxy란 대리자라는 뜻으로, 인터넷 통신 시 빠른 엑세스나 안전한 통신 등을 확보하기 위한 중계 서버를 말함.
            //End-Point가 클라이언트의 프론트 프록시라고 할 수 있음.

            //서비스의 메소드 호출
            string result = proxy.SayHello();
            (proxy as IDisposable).Dispose(); //proxy 사용이 끝나면 Dispose()를 호출하고 메모리가 해제됨.

            //결과 출력
            label1.Text=result;
        }

       
    }
}
