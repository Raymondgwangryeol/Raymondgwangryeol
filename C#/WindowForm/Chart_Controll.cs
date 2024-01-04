/*
 Serise: X,Y 값들
 ChartArea: 차트 그래프+ 범례 그 한 세트. 하나의 차트 안에 여러개의 ChartArea들이 들어갈수 있음
 Legends: 범례. 0개면 생략
 Titles: 차트 컨트롤 상단에 표시되는 제목, 복수일 경우 위에서부터 한 라인씩 표시함.

 여기서 가장 중요한 속성은 Serise임. Serise는 다양한 파라미터를 갖는데,
 특히 자주 쓰이는 속성들을 예로 들자면 차트의 종류를 지정하는 ChartType(라인차트, 파이차트 등),
 데이터를 관리할 수 있는 Points,
 차트 영역을 지정할 수 있는 ChartArea가 있다.
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
using System.Data.SqlClient;
using DConn1;

namespace Chart
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //차트에 값을 넣어보자
            //1. 진짜 직접 때려넣기
            /*
            chart1.Series["Series1"].Points.Clear(); //Points => 평면상의 한 점을 표시하기 위한 자료형.
                                                     //x, y좌표의 형식으로 위치를 표시.
            chart1.Series["Series1"].Points.Add(100); //100은 Y좌표로 간주됨. X는 자동으로 1로 부여
            chart1.Series["Series1"].Points.AddXY(20, 200);
            chart1.Series["Series1"].Points.AddXY(30, 50);
            

            //다양한 차트 종류
            // Serise 객체의 ChartType 속성을 지정해 변경한다.

            chart1.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.SplineArea;
            //Pie: 원형 비율 그래프
            //Line: 꺾은선 그래프
            //Bar: 가로 막대 그래프
            */

            //2. 데이터를 가져와서 차트 컨트롤에 바인딩하기
            // 배열, 리스트(List<T> 동적 배열), 혹은 IEnumerable 지원 컬렉션을 바인딩 가능.
            // DataSet, DataView, 외부 데이터 소스에 대한 DataReader등도 바인딩 가능.

            //데이터 바인딩 방법음 크게 5가지가 있다. 각 방법마다 장단점이 있음.
            //1. Series.Points.DataBindY() / DataBindXY()
            //2. Series.Points.DataBind()
            //3. Chart.DataBindTable()
            //4. Chart.DataSource / Chart.DataBind()
            //5. Chart.DataBindCrossTable()


            //배열/컬렉션에 대한 바인딩
            //  => Series.Points.DataBindY() 또는 DataBindXY()로 바인딩 가능.
            //  => 클래스 객체 컬렉션인 경우, Points.DataBind(컬렉션, X,Y축 속성, 추가 옵션)로 바인딩 가능.

            /*
            // (1) Y값 배열 데이터바인딩
            double[] scores = new double[] { 80, 90, 85, 70, 95 };
            chart1.Series[0].Points.DataBindY(scores);

            // (2) X, Y값 List<T> 데이터바인딩
            List<string> x = new List<string> { "철수", "영희", "길동", "재동", "민희" };
            List<double> y = new List<double> { 80, 90, 85, 70, 95 };
            chart2.Series[0].Points.DataBindXY(x, y);

            // (3) 객체 컬렉션에 대한 데이터바인딩
            List<Student> students = new List<Student>();
            students.Add(new Student("철수", 80));
            students.Add(new Student("영희", 90));
            students.Add(new Student("길동", 85));

            // X축: Name, Y축: Score
            chart3.Series[0].Points.DataBind(students, "Name", "Score", null);

            // (참고) DataBindTable() 사용시. (X축: Name, Y축: 자동검색)
            // chart3.DataBindTable(students, "Name"); 
            */

            //테이블 데이터 바인딩
            //Chart.DataBindTable, Chart.DataSource/ Chart.DataBlnd, Series.Points.DataBind() 사용하면 좋음.

        }
        /*
        class Student //student는 private 클래스임.(private가 디폴트임)
        {
           public string Name { get; set; }// 다른 곳에서 이런 특정 클래스(private, protected)에 접근하기 위해,
                                           //get, set을 정의해 사용함.
           public double Score { get; set; }//지금 이 코드는 C#7.0부터 제공되는 자동 구현 프로퍼티 코드임.
                                            // 들어온 인자값 Score에 받고, 받아온 값 Score에 넣고 이런 뜻임.

           //Student 클래스의 생성자임.
           //1. 클래스와 같은 이름의 메소드
           //2. public으로 정의 되어야 함
           //3. return형식의 반환을 사용할 수 없음. 하지만 void는 못 씀
           public Student(string name, double score) //  Name.set() + Score.set()이라 생각하면 됨.

           {
               this.Name = name;
               this.Score = score;
           }

        }
        */
        private void button1_Click(object sender, EventArgs e)
        {
            string strConn;
            string sql = "SELECT id, Class, Score  FROM dbo.Scores";
            DClass dc = new DClass();

            strConn = dc.StrConn("Test");
            using (SqlConnection sql_con = new SqlConnection(strConn))
            {
                sql_con.Open();
                SqlCommand cmd = new SqlCommand(sql, sql_con);
                

                chart1.Series[0].Points.Clear();

                // (1) DataBindTable 사용
                //SqlDataReader dataReader = cmd.ExecuteReader(); // SqlDataReader=> 한 줄씩 받아옴.
                //ExecuteReader(): SQL 명령을 실행하고 SqlDataReader 결과셋을 반환. 주로 Select문을 실행할 때 사용함.
                //While(reader.Read()){} 해 놓고 한 줄씩 내용 출력한다거나 그럴 경우 씀.
                //chart1.DataBindTable(dataReader, "Score"); //사용할 dataset과 X축 필드만 정해주면 됨. Y는 지가 찾음.

                // (2) DataSource와 DataBind() 사용
                /*
                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);

                chart1.DataSource = ds.Tables[0];
                chart1.Series[0].XValueMember = "Class";
                chart1.Series[0].YValueMembers = "Score";
                chart1.DataBind(); //XValue, YValue 지정한 다음에 묶는 듯
                */



                //3. Points.DataBind() 사용
                //SqlDataReader dataReader = cmd.ExecuteReader(); //얘는 객체 선언 안 하는구나..

                //chart1.Series[0].Points.DataBind(dataReader, "Class", "Score", "Tooltip=Class");
                                                //(dataSource, X축, Y축, 툴팁과 같이 다른 필드 지정 가능.)

                

            }


            //DataBindCrossTable() 바인딩
            //테이블 데이터를 사용하면서 X,Y축 이외에 데이터를 그룹으로 묶는 컬럼을 지정함.
            // X,Y축은 변함이 없는데 이걸 다양한 상황에 쓰고싶다 할 때 쓰는 거
            // DataBindCrossTable() 사용

            DataTable dt = new DataTable("Order");
            dt.Columns.Add("customer"); // X축
            dt.Columns.Add("product"); // 그룹으로 묶을 속성
            dt.Columns.Add("orders"); // Y축
            dt.Rows.Add("Tom", "USB", 10);
            dt.Rows.Add("Tom", "HDD", 2);
            dt.Rows.Add("Tom", "Monitor", 1);
            dt.Rows.Add("Jane", "USB", 3);
            dt.Rows.Add("Jane", "HDD", 1);
            dt.Rows.Add("Jane", "Monitor", 2);

            // Product별로 Series를 하나씩 자동으로 생성
            // X축은 customer 컬럼, Y축은 orders 컬럼
            // 각 그래프 상단에 product명으로 Label을 붙임
            chart1.DataBindCrossTable(dt.AsEnumerable(), "product", "customer", "orders", "Label=product");




        }

    }
}
