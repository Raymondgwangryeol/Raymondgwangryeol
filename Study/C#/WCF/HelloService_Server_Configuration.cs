/*
   Configuration 파일 사용 버전 - Server
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

        private void send_Click(object sender, EventArgs e) //End-point 바인딩 안 하고 호스트만 만들어서 넘겨준다
        {
      

            //서비스 호스트 구현. 여기서 구현 안 하면 사용자가 요청할 때만 접속할 수 있게 됨.
            host = new ServiceHost(typeof(HelloWorldWCFService));
        


            host.Open(); //클라이언트가 호출하면 Listener 구동, 클라이언트 호출을 수신한다.


        }

        private void stop_Click(object sender, EventArgs e)
        {
            host.Close(); // 클라이언트의 모든 호출이 처리되면 제어를 반환한다.
        }
    }
}
