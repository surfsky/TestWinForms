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

namespace TestDraw
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            float tension = (float)(this.trackBar1.Value / 10.0);
            DrawCurvePointTension(e, tension);
        }

        private void DrawCurvePointTension(PaintEventArgs e, float tension)
        {
            // Create pens.
            Pen redPen = new Pen(Color.Red, 3);
            Pen greenPen = new Pen(Color.Green, 3);
            Pen bluePen = new Pen(Color.Blue, 1);
            Brush brush = new SolidBrush(Color.White);

            // Create points that define curve.
            Point point1 = new Point(50, 50);
            Point point2 = new Point(100, 25);
            Point point3 = new Point(200, 5);
            Point point4 = new Point(250, 50);
            Point point5 = new Point(300, 100);
            Point point6 = new Point(350, 200);
            Point point7 = new Point(250, 250);
            Point[] curvePoints = { point1, point2, point3, point4, point5, point6, point7 };

            // Draw lines between original points to screen.
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawLines(redPen, curvePoints);

            // Create tension.
            // Draw curve to screen.
            g.DrawCurve(greenPen, curvePoints, tension);

            // Draw Dots
            foreach (var p in curvePoints)
            {
                var rect = new Rectangle(p.X - 2, p.Y - 2, 4, 4);
                g.FillRectangle(brush, rect);
                g.DrawRectangle(bluePen, rect);
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            float tension = (float)(this.trackBar1.Value / 10.0);
            lblTension.Text = tension.ToString();
            panel1.Invalidate();
        }
    }
}
