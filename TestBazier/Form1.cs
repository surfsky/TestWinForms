using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestBazier
{
    public partial class Form1 : Form
    {
        List<BazierPoint> _points = new List<BazierPoint>()
        {
            new BazierPoint(new PointF(50, 50),     new PointF(50, 50),    new PointF(150, 50)),
            new BazierPoint(new PointF(100, 100),   new PointF(50, 100),   new PointF(150, 100)),
            new BazierPoint(new PointF(200, 200),   new PointF(150, 200),  new PointF(200, 200)),
        };


        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);  // 双缓冲绘图
        }

        /// <summary>根据贝塞尔曲线数据绘制曲线</summary>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var pen = new Pen(Color.Black);
            Painter.DrawBazier(g, _points, pen);
        }

        /// <summary>鼠标按下时，测试点击位置并记录下来</summary>
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (var p in _points)
                p.HitTest(e.Location);
            _points.First().HitTest(e.Location, BazierPointPart.NextControl);
            _points.Last().HitTest(e.Location, BazierPointPart.PrevControl);
            Refresh();
        }

        /// <summary>鼠标按下并移动时，设置选中的控制点位置等于当前光标位置</summary>
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            foreach (var p in _points)
            {
                if (p.HitPart == BazierPointPart.None)               continue;
                if (p.HitPart == BazierPointPart.Point)              p.Point = e.Location;
                else if (p.HitPart == BazierPointPart.PrevControl)   p.PrevControl = e.Location;
                else if (p.HitPart == BazierPointPart.NextControl)   p.NextControl = e.Location;
            }

            Refresh();
        }

        /// <summary>鼠标提起时，清空所有选中状态</summary>
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            //foreach (var p in _points)
            //    p.HitPart = HitPart.None;
            Refresh();
        }
    }
}
