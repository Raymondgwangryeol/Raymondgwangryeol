/*
 #Window Form이 뭐게#
 Form 클래스 객체를 생성해서, Application.Run()에 인자로 던져주고
 실행히먄 From 객체를 화면에 출력하고, 메시지 루프를 만들어서 마우스, 키보드 등의 입력을 UI 스레드에 전달함.
 STAThread(Single Threaded Arpartment Thread)모델로 실행.
    => 메시지 기반으로 동시 실행되는 여러 개체를 처리하는 틀. 하지만 단일 스레드라서 시간이 많이 걸리는 놈이 메모리 차지하면 ready큐에서 조난 당할 수 있음.
        => 아파트먼트는 같은 스레딩 모델을 공유하는 객체들이 존재하는 곳이다

 !!Form.Designer.cs랑 Form1.cs는 같은 클래스가 2개로 찢어진 Partial 클래스다...!! 대..박..
 - Form1.Designer.cs: 폼에 포함된 모든 UI 컨트롤 등에 대한 정보를 가짐. (건드는거 아니래..)
 - Form.cs: 디자이너 파일을 기반으로 폼을 해석(Rendering), VS 디자이너에게 보여줌. 이벤트 핸들링하는 파일
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

namespace Login
{
    public partial class Form1 : Form  // class를 여러 파일로 분할하기 위해 partial를 씀
    // 다른 Form끼리 값 전달하려고 인터페이스 추가로 가지게 함
    // Form 클래스들 자체가 Form 인터페이스 상속 받고 있음. 
    {
        int count = Constants.NUM;
        public class Constants // Class for declaration constant valuable NUM(int type)
        {
            // c#에서는 c++처럼 #define으로 정의가 안 돼서
            // 상수 정의 class를 만들어서 아래와 같이 정의해야 한다.

            public const int NUM = 5; // Declaration code for NUM
        }
        static public string form1Text; 

        public Form1()
        {
            InitializeComponent(); // 반드시 호출해야하는 생ㅇ성자. UI 컴포넌트 생성하는 역할
        }

        //------------------Textboxes in Form1 to recive a one's client's account data-------------------
        public void Id_TextChanged(object sender, EventArgs e)
        {
           
        }
        public void password_TextChanged_1(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        //해당 event 메소드들이 InitiallzeComponent()에 자동으로... 생성 되기 때문에, 무조건 선언해줘야 한다.
        //위의 두개의 메소드들은 값을 참조해야 해서 public으로 했음.

        // Button to submiting
        private void SignInButton_Click(object sender, EventArgs e)
        {
            // id와 password에 저장되어있는 string이 각각 아이디, 패스워드인데, 사용자가 입력한 값을 .Text해서 넣고 해당 값 사용.
            // 편법이긴 해
            // 여기가 많은 비중을 차지하는 곳이니까 private로 함

            // Passing a data from Form1 to Form2 using methods
            Form2 form2 = new Form2(this);  // Modifiers(니가 아는 그 제한자) 속성 이용, public이나 Internal로(id,password testbox)
            //Form2 열 거니까 객체 선언해 주고
            string userid = id.Text; 
            string userpw = password.Text;

            label2.Text = ("주의: " +""+(count-1).ToString() + "번 실패 시 로그인 버튼 비활성화");

            if ((userid == "root") && (userpw == "1234")) // .Equals가 있네 ㅋㅋ
            {
                MessageBox.Show("로그인 성공"); // 경고?창 뜨는 그거
               
                form2.Show(); //form2 창 띄우기 
                form2.SetText(userid); // 정보 전달할 쪽에 텍스트 넘기는 함수 정의
                this.Close(); // Form2 열면 Form1 닫기
                count = Constants.NUM; // 5로 초기화
            }
            else
            {
                MessageBox.Show("로그인 실패\n" + (--count).ToString() + "번 남았습니다.");
                if (count == 0)
                {
                    SignInButton.Enabled = false; // 버튼 비활성화
                    label2.Text = "로그인 불가. 당사에 문의하십시오";
                }
    
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SignInButton.Enabled = true; // 버튼 활성화
            count = Constants.NUM;
        }
    }
}
