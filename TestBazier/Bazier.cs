using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBazier
{
    /// <summary>贝塞尔点的组成部分（点击判断时用）</summary>
    [Flags]
    public enum BazierPointPart
    {
        None = 0,
        Point = 1,
        PrevControl = 2,
        NextControl = 4
    }


    /// <summary>
    /// 贝塞尔曲线的点
    /// </summary>
    public class BazierPoint
    {
        /// <summary>曲线上的点</summary>
        public PointF Point { get; set; }

        /// <summary>前控制点</summary>
        public PointF PrevControl { get; set; }

        /// <summary>后控制点</summary>
        public PointF NextControl { get; set; }

        /// <summary>选中的部分</summary>
        public BazierPointPart HitPart { get; set; } = BazierPointPart.None;

        /// <summary>构造函数</summary>
        public BazierPoint(PointF point, PointF prevPoint, PointF nextPoint)
        {
            this.Point = point;
            this.PrevControl = prevPoint;
            this.NextControl = nextPoint;
        }

        /// <summary>测试点击的位置（并设置SelectedPart值）</summary>
        /// <param name="ctrls">需要测试的控制部件</param>
        public void HitTest(PointF p, BazierPointPart ctrls=BazierPointPart.PrevControl | BazierPointPart.NextControl)
        {
            this.HitPart = BazierPointPart.None;
            if (HitTestPoint(this.Point, p)) 
                this.HitPart = BazierPointPart.Point;

            if (ctrls.HasFlag(BazierPointPart.PrevControl) && HitTestPoint(this.PrevControl, p))      
                this.HitPart = BazierPointPart.PrevControl;

            if (ctrls.HasFlag(BazierPointPart.NextControl) && HitTestPoint(this.NextControl, p)) 
                this.HitPart = BazierPointPart.NextControl;
        }

        /// <summary>测试点是否被点中</summary>
        /// <param name="target">目标点</param>
        /// <param name="cursor">光标点</param>
        static bool HitTestPoint(PointF target, PointF cursor)
        {
            var rect = new RectangleF(target, SizeF.Empty);
            rect.Inflate(5, 5);
            return rect.Contains(cursor);
        }

        /// <summary>绘制贝塞尔曲线控制点</summary>
        /// <param name="ctrls">要显示的控制点</param>
        public void Draw(Graphics g, BazierPointPart ctrls = BazierPointPart.PrevControl | BazierPointPart.NextControl)
        {
            var pen = new Pen(Color.Black);
            if (HitPart == BazierPointPart.None)
            {
                Painter.DrawDot(g, this.Point, Color.Black, false);
            }
            else
            {
                Painter.DrawDot(g, this.Point, Color.Black, HitPart == BazierPointPart.Point);
                if (ctrls.HasFlag(BazierPointPart.PrevControl))
                {
                    g.DrawLine(pen, this.Point, this.PrevControl);
                    Painter.DrawDot(g, this.PrevControl, Color.Black, HitPart == BazierPointPart.PrevControl);
                }
                if (ctrls.HasFlag(BazierPointPart.NextControl))
                {
                    g.DrawLine(pen, this.Point, this.NextControl);
                    Painter.DrawDot(g, this.NextControl, Color.Black, HitPart == BazierPointPart.NextControl);
                }
            }
        }
    }

    /// <summary>
    /// 绘制辅助类
    /// </summary>
    public class Painter
    {
        /// <summary>绘制贝塞尔曲线</summary>
        public static void DrawBazier(Graphics g, List<BazierPoint> ps, Pen pen)
        {
            for(int i=0; i<ps.Count-1; i++)
            {
                var p1 = ps[i];
                var p2 = ps[i + 1];
                g.DrawBezier(pen, p1.Point, p1.NextControl, p2.PrevControl, p2.Point);
                if (i == 0)
                    p1.Draw(g, BazierPointPart.NextControl);   // 第一个点只绘制后句柄
                else
                    p1.Draw(g);
            }
            ps.Last().Draw(g, BazierPointPart.PrevControl);   // 最后一个点只绘制前句柄
        }


        /// <summary>绘制句柄（4x4）</summary>
        /// <param name="solid">是否实心</param>
        public static void DrawDot(Graphics g, PointF pt, Color color, bool solid)
        {
            if (solid)
            {
                var brush = new SolidBrush(color);
                g.FillRectangle(brush, pt.X - 2, pt.Y - 2, 4, 4);
            }
            else
            {
                var brush = new SolidBrush(Color.White);
                var pen = new Pen(color);
                g.FillRectangle(brush, pt.X - 2, pt.Y - 2, 4, 4);
                g.DrawRectangle(pen,   pt.X - 2, pt.Y - 2, 4, 4);
            }
        }

    }
}
