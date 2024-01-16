/*
 # DataGridView #
 테이블 형태의 데이터를 화면에 뿌릴 때 사용.

 - Binding 모드 : 데이터를 데이터 소스와 바인딩해서 사용
                  DataSource 속성을 테이블 등의 소스에 설정해서 바인딩(bind=묶다).
                  - DataSource는 ListView의 항목들을 콜렉션으로 묶을 때 사용하는 속성이다.
                  - A data source control allows you to retrieve data from a database and use it to populate a grid.

 - Unbound 모드 : 데이터를 개발자가 수동으로 갱신해줘야 함.
                  Rows 컬렉션 수동 갱신으로 데이터 핸들링.

 */


/*
 Table을 DataGridView에 뿌려보자

 1. SQL 테이블 데이터를 DataSet 객체로 가져오기
 2. DataSet의 DataTable을 그리드 컨트롤에 바인딩하기

 일반적인 절차는 아래와 같다.
 (1) DataGridView 컨트롤을 Form 위에 놓는다
 (2) DataSet을 리턴하는 클래스 혹은 메서드를 만든다. (예: GetData())
 (3) DataGridView 객체의 DataSource 속성에 해당 DataTable을 지정한다.
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
using System.Data.SqlClient; //DB 사용을 위해 추가! SqlConnection 사용

namespace GridView
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'testDataSet.Scores' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.scoresTableAdapter.Fill(this.testDataSet.Scores);
            DataSet ds = GetData();

            if (ds != null)
            {
                dataGridView1.DataSource = ds.Tables[0];

            }
            else this.Close();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public string StrConn() // SqlConnection()이용해 서버와 접속 시 사용하는 Connection String
        {
            string str;

            string dbAddr = ".";
            string dbName = "Test";


            str = $"Data Source={dbAddr};" +
                  $"Initial Catalog={dbName};" +
                  "Integrated Security = SSPI;";

            /*
             [SqlConnection에 들어가야할 문장 형태]

             Standard Security
                "Server=myServerAddress,
                Database=myDataBase,
                UserId=myUsername,
                Password=myPassword;" => SQL 인증 방식의 경우


            "Data Source=.;Initial Catalog=Test;Integrated Security=SSPI;"; => Window 인증 방식의 경우. 왜 Data Source 값이 .인지는 몰랑
            
            #주의#
            인증 방식, 의도, 상황에 따라 넣어야 할 값이 다름.

             */

            return str;

        }
        private DataSet GetData()
        {
            SqlConnection conn = new SqlConnection(StrConn());
            DataSet ds = new DataSet();

            /*
             * Using *
            파일 핸들이나 데이타베이스 Connection과 같은 리소스는 .NET에서 자동으로 닫거나 해지하지 못하는데, 이를 Unmanaged Resource라 부른다.
            SqlConnection과 같은 Unmanaged Resource포함 클래스를 사용할 때, using을 사용하면 자동을 리소스를 해지할 수 있다.
            해당 using 블럭이 끝나면 자동으로 IDisposable 인터페이스의 Dispose() 메소드를 호출함. IDisposable 인터페이스는 관리되지 않은 리소스를 헤제할 때 사용됨.
            finally에서 conn.Close(0) 따로 안 해줘도 됨. try~finally using으로 퉁치기 가능
             */
            using (conn) //using블록 내에서 예외상황 발생하면 그냥 Dispose가 호출되기 때문에, 이 사람은 using 안에 try, catch문을 써서 에러를 잡았음.
            {
                try
                {
                    conn.Open();
                    MessageBox.Show("Success");
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM dbo.Scores WHERE Class=10", conn); //SQL Server에서 data를 클라이언트로 가져온 후, 연결을 끊고 데이터를 사용할 수 있게 함. 그래서 close가 가능한가뫄
                    // SqlDataReader는 데이터를 가져온 후 연결을 유지함.
                    adapter.Fill(ds);
                    

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Fail");
                    ds = null; //DataSet 객체를 리턴하긴 해야 되서, 구분되라고 null값 넣어줌
                }
            }
            return ds;
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Row헤더, Column헤더인 경우 그냥 리턴
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
            {
                
                return; // RowIndex = 현재 인덱스를 가져옴. ColumnIndex = 현재셀의 열 인덱스를 가져옴.
            } 
            //헤더 index 값 -1인가봄... 여기는 선택이 안 되게 해 놓음. Cell Mouse Down이니까.. 클릭을 일단 하면 생기는 이벤트에 대한 거구나

            // 오른쪽 마우스 버튼인 경우
            if (e.Button == MouseButtons.Right)
            {
                var grid = sender as DataGridView; //sender를 DataGridView로 캐스팅

                // 마우스 RightClick해도 현재 Cell 을 선택함
                grid.CurrentCell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex]; //CurrentCell => 현재 활성화 된 Cell을 가져오거나 설정.
                label1.Text = String.Format("row = {0}, column = {1}", (grid.CurrentCell.RowIndex).ToString(), (grid.CurrentCell.ColumnIndex).ToString());
            }
        }
    }
}

/*
 # Data Paging#
 SqlDataAdapter는 전체 데이타는 물론 페이지별로 일부 데이타만 리턴하는 기능도 가지고 있다.
 즉, SqlDataAdapter.Fill() 메서드에서 두번째 파라미터에 페이지 시작위치를, 세번째 파라미터에 리턴되는 레코드 최대 ROW 수를 지정하면 지정된 일부 데이타만 리턴할 수 있다.
 예를 들어, adapter.Fill(ds, 10, 20, tblName)와 같이 지정하면, 전체 데이타 중 10번째 Row부터 20개의 Row를 리턴하게 된다.
 */
