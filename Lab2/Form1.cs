using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Lab2
{
    public delegate void BallDelegate();
    public partial class Form1 : Form
    {
        private Ball red, red2; 
        private Ball blue, blue2;
        public Form1()
        {
            InitializeComponent();
            timer1.Enabled = true;

            red = new Ball(Color.Red, 0, 200, 10); red2 = new Ball(Color.Red, 0, 100, 10);
            blue = new Ball(Color.Blue, 540, 200, -10); blue2 = new Ball(Color.Blue, 540, 100, -10);

            red.Finish += blue.Start; red2.Finish += blue2.Start;
            blue.Finish += red.Start; blue2.Finish += red2.Start;

            BallDelegate thread = new BallDelegate(red.Go); BallDelegate thread2 = new BallDelegate(red2.Go);

            IAsyncResult asyncResult = thread.BeginInvoke(new AsyncCallback(red.CallBack), null); 
            IAsyncResult asyncResult2 = thread2.BeginInvoke(new AsyncCallback(red2.CallBack), null);
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            red?.Draw(e.Graphics); red2?.Draw(e.Graphics);
            blue?.Draw(e.Graphics); blue2?.Draw(e.Graphics);
        }
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
    public class Ball
    {
        Random random = new Random();
        private SolidBrush br;
        private int x, y, dx;
        public Ball(Color c, int _x, int _y, int _dx)
        {
            br = new SolidBrush(c);
            x = _x;
            y = _y;
            dx = _dx;
        }
        public event BallDelegate Finish;
        public void Draw(Graphics g)
        {
            g.FillEllipse(br, x, y, 50, 50);
        }
        public void Go()
        {
            if (dx>0)
            {
                while (x < 540)
                {
                    x += dx;
                    Thread.Sleep(50);
                }
                Finish.BeginInvoke(new AsyncCallback(CallBack), null);
            }
            else
            {
                while (x > 0)
                {
                    x += dx;
                    Thread.Sleep(50);
                }
                Finish.BeginInvoke(new AsyncCallback(CallBack), null);
            }
        }
        public void Start()
        {
            if (dx > 0) x = 0;
            else x = 540;
            Go();
        }
        public void CallBack(IAsyncResult asyncResult)
        {
            br.Color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)); ;
        }
    }
}
