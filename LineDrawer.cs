using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using input = System.Windows.Input;
using System.Windows.Forms;

namespace MiniDraw
{
    internal delegate void LineAddedEventHandler(Object obj, LineAddedEventArgs e);
    class LineDrawer
    {
        Point startPoint;
        Point endPoint;

        public event LineAddedEventHandler LineAdded;

        public LineDrawer(Form form, Point startPoint)
        {
            this.form = form;
            form.MouseMove += Form_MouseMove;
            form.MouseUp += Form_MouseUp;
            form.Paint += Form_Paint;
            this.startPoint = startPoint;
        }

        private void Form_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.DrawLine(Pens.Blue, startPoint, endPoint);
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            this.form.Paint -= Form_Paint;
            form.MouseMove -= Form_MouseMove;
            form.MouseUp -= Form_MouseUp;

            form.Invalidate();

            LineAddedEventArgs args = new LineAddedEventArgs(this.form, this.startPoint, e.Location);
            LineAdded(this, args);
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            this.endPoint = e.Location;
            if (input.Keyboard.IsKeyDown(input.Key.LeftShift) || input.Keyboard.IsKeyDown(input.Key.RightShift))
            {
                int deltY = Math.Abs(this.startPoint.Y - this.endPoint.Y);
                int deltX = Math.Abs(this.startPoint.X - this.endPoint.X);

                if (deltY > deltX)
                    this.endPoint.X = this.startPoint.X;
                else
                    this.endPoint.Y = this.startPoint.Y;
            }

            form.Invalidate();
        }

        Form form;
        public Form Form
        {
            get { return this.form; }
        }
    }

    class LineAddedEventArgs : EventArgs
    {
        Point startPoint;
        public Point StartPoint
        {
            get { return startPoint; }
        }

        Point endPoint;
        public Point EndPoint
        {
            get { return endPoint; }
        }

        Form form;
        public Form Form
        {
            get { return this.form; }
        }

        public LineAddedEventArgs(Form form, Point startPoint, Point endPoint)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.form = form;
        }
    }
}
