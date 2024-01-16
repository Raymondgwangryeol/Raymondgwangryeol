//Configuration 파일 사용 버전

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
            
            ChannelFactory<IHelloWorld> factory = //IHelloWorld 타입의 채널을 선언한다
                new ChannelFactory<IHelloWorld>("HttpHelloWorld");// 우와아아 Generic 타입 맞다...

            IHelloWorld proxy = factory.CreateChannel();
            
            //서비스의 메소드 호출
            string result = proxy.SayHello(); 
            (proxy as IDisposable).Dispose(); //proxy 사용이 끝나면 Dispose()를 호출하고 메모리가 해제됨.

            //결과 출력
            label1.Text=result;
        }

       
    }
}
