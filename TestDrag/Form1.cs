using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestWinformDrag
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.lbTarget.AllowDrop = true;
        }


        //-----------------------------------------
        // 拖动
        //-----------------------------------------
        // 拖动lisbox
        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            this.DoDragDrop(lbSource.SelectedItem?.ToString(), DragDropEffects.Copy);
        }

        // 拖动label
        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            this.DoDragDrop("label1", DragDropEffects.Copy);
        }

        // 拖动radiobox（且不影响radio功能）
        bool mouseDown;
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void radioButton1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            //this.DoDragDrop("radioButton1", DragDropEffects.Copy);
        }
        private void radioButton1_MouseLeave(object sender, EventArgs e)
        {
            if (mouseDown)
                this.DoDragDrop("radioButton1", DragDropEffects.Copy);
        }
        private void radioButton2_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            //this.DoDragDrop("radioButton2", DragDropEffects.Copy);
        }
        private void radioButton2_MouseLeave(object sender, EventArgs e)
        {
            if (mouseDown)
                this.DoDragDrop("radioButton2", DragDropEffects.Copy);
        }



        //-----------------------------------------
        // 放置
        //-----------------------------------------
        // 拖入时判断是否接受拖放，如果effect不是none的话才允许放下
        private void listBox2_DragEnter(object sender, DragEventArgs e)
        {
            // 允许拖拽文件及文本
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        // 放下时，解析放下的内容，并做相应处理
        private void listBox2_DragDrop(object sender, DragEventArgs e)
        {
            // 放置文件及文本
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = e.Data.GetData(DataFormats.FileDrop) as Array;
                foreach (var file in files)
                    lbTarget.Items.Add(file);
            }
            else if (e.Data.GetDataPresent(DataFormats.Text))
            {
                var txt = e.Data.GetData(DataFormats.Text) as string;
                lbTarget.Items.Add(txt);
            }
        }

    }
}
