using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResizeFormWithoutBorderViaMouse
{
    public partial class MainForm : Form
    {
        #region 消息常量
        const int WM_NCHITTEST = 0x0084;
        const int HT_LEFT = 10;
        const int HT_RIGHT = 11;
        const int HT_TOP = 12;
        const int HT_TOPLEFT = 13;
        const int HT_TOPRIGHT = 14;
        const int HT_BOTTOM = 15;
        const int HT_BOTTOMLEFT = 16;
        const int HT_BOTTOMRIGHT = 17;
        const int HT_CAPTION = 2;
        #endregion

        #region 复写消息处理函数 WndProc
        protected override void WndProc(ref Message Msg)
        {
            switch (Msg.Msg)
            {
                //鼠标拖动改变大小
                case WM_NCHITTEST:
                    {
                        // 获取鼠标位置
                        int nPosX = (Msg.LParam.ToInt32() & 65535);
                        int nPosY = (Msg.LParam.ToInt32() >> 16);

                        if (nPosX >= this.Right - 6 && nPosY >= this.Bottom - 6)
                        {
                            //右下角
                            Msg.Result = new IntPtr(HT_BOTTOMRIGHT);
                        }
                        else if (nPosX <= this.Left + 6 && nPosY <= this.Top + 6)
                        {
                            //左上角
                            Msg.Result = new IntPtr(HT_TOPLEFT);
                        }
                        else if (nPosX <= this.Left + 6 && nPosY >= this.Bottom - 6)
                        {
                            //左下角
                            Msg.Result = new IntPtr(HT_BOTTOMLEFT);
                        }
                        else if (nPosX >= this.Right - 6 && nPosY <= this.Top + 6)
                        {
                            //右上角
                            Msg.Result = new IntPtr(HT_TOPRIGHT);
                        }
                        else if (nPosX >= this.Right - 2)
                        {
                            //右边框
                            Msg.Result = new IntPtr(HT_RIGHT);
                        }
                        else if (nPosY >= this.Bottom - 2)
                        {
                            //底边框
                            Msg.Result = new IntPtr(HT_BOTTOM);
                        }
                        else if (nPosX <= this.Left + 2)
                        {
                            //左边框
                            Msg.Result = new IntPtr(HT_LEFT);
                        }
                        else if (nPosY <= this.Top + 2)
                        {
                            //上边框
                            Msg.Result = new IntPtr(HT_TOP);
                        }
                        else if (nPosY <= this.Left + 20)
                        {
                            //上方 2~20 像素作为标题栏拖动
                            Msg.Result = new IntPtr(HT_CAPTION);
                        }
                        /*
                        else
                        {
                            //其他区域返回拖动标题栏消息
                            Msg.Result = new IntPtr(HT_CAPTION);
                        }
                        */
                        break;
                    }
                default:
                    {
                        base.WndProc(ref Msg);
                        break;
                    }
            }
        }
        #endregion

        #region 为窗体增加圆角效果
        public void SetWindowRegion()
        {
            System.Drawing.Drawing2D.GraphicsPath FormPath;
            FormPath = new System.Drawing.Drawing2D.GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            FormPath = GetRoundedRectPath(rect, 7);
            this.Region = new Region(FormPath);

        }
        private System.Drawing.Drawing2D.GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            int diameter = radius;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();

            // 左上角
            path.AddArc(arcRect, 180, 90);

            // 右上角
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);

            // 右下角
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);

            // 左下角
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure();//闭合曲线
            return path;
        }
        #endregion

        #region 为窗体添加阴影
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams expr_06 = base.CreateParams;
                expr_06.ClassStyle |= 131072;
                expr_06.ExStyle |= 33554432;
                return expr_06;
            }
        }
        #endregion

        public MainForm()
        {
            InitializeComponent();

            SetWindowRegion();
            this.Resize += new EventHandler((s,e) =>{SetWindowRegion();});
        }
    }
}
