using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;


using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form2 : Form
    {
        int check = 0;
        public Form2()
        {
            InitializeComponent();
            listView1.AllowDrop = true;
            listView2.AllowDrop = true;
            
        }

        private void label1_MouseDown(object sender, MouseEventArgs e) // 소스(Drop Source), 끌어다 놓을 거
        {
            // Drop Source에서 해야되는 거
                // DoDragDrop 매서드(Control 클래스에 기본으로 있음) 호출해서 드래그-앤-드롭 시작하기
                // 대부분 MouseDown 이벤트 핸들러에서 실행하지만, ListView나 TreeView등 일부 컨트롤에서는 ItemDrag 이벤트 핸들러 안에서 실행함.
            DoDragDrop(label1.Text, DragDropEffects.Copy); // DoDragDrop(복사할 거, DragDropEffects종류. 복사할건지 뭐할건지...)
            if (check == 1)
            {
                label1.Text = "";  // 만약 Move 이면 소스를 이렇게 지움
                check = 0;
            }
        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(label2.Text, DragDropEffects.Copy); // DoDragDrop(복사할 거, DragDropEffects종류. 복사할건지 뭐할건지...)
            if (check == 1)
            {
                label2.Text = "";  // 만약 Move 이면 소스를 이렇게 지움
                check = 0;
            }


        }


        // DropTarget, 가져다 놓을 곳
        // 여기서는 listView1
        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            //Drop Target에서 해야할 일
            // 1. 일단 AllowDrop 속성을 true로 설정한다.
            // listView.AllowDrop = true;
            // 2. DragEnter 핸들러를 구현한다. => e.Effect 속성 지정.
            //DragEnter는 마우스가 타겟 컨트롤 내로 들어올 때 발생하는 이벤트. 일단 닿았다 싶으면 바로 실행.
            //드래그해온 데이터를 받아들일지 말지 결정함.
            //DragEnter 대신 DragEnter 바로 다음에 발생하는 DragOver(컨트롤 위로 개체를 드래그 할 때)에서 Effect 속성 지정해도 됨.
            // 상황따라 하십쇼
          
            if (e.Data.GetDataPresent(typeof(string)))
            {
                e.Effect = DragDropEffects.Copy; //e.Effect에 DragDropEffects지정
            }
            else
            {   
                e.Effect = DragDropEffects.None; // 드랍 허용하지 않음.
            }

        }
            // 3. DragDrop 이벤트 핸들러를 구현한다.
                // 마우스에서 손 떼서 Drop이 실제로 이루어질 때 발생
        private void listView1_DragDrop(object sender, DragEventArgs e) 
        {
            var item = (string)e.Data.GetData(DataFormats.StringFormat);
            listView1.Items.Add(item);
            check = 1;
            
            
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Copy);

        }

        private void listView2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                label2.Text = (string)e.Data.GetData(DataFormats.StringFormat);

                e.Effect = DragDropEffects.None; 
            }
        }

        private void listView2_DragDrop(object sender, DragEventArgs e)
        {
            var item = e.Data.GetData(typeof(ListViewItem)) as ListViewItem;
            listView2.Items.Add(item.Text);
            
           
        }
    }
}
