using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MiniDraw
{
    public delegate void LineSelectedHandler(object sender, LineSelectedEventArgs e);
    public delegate void LineDeselectedHandler(object sender, EventArgs e);

    class Line
    {
        LineInfo lineInfo;
        Form form;
        bool isPerpendicularLength = false;
        bool isCaptured = false;
        Point clickedPoint = Point.Empty;

        public event LineSelectedHandler LineSelected;
        public event LineDeselectedHandler LineDeselected;

        Pen pen;
        public Pen Pen
        {
            get { return this.pen; }

            set
            {
                this.pen = value;
                this.form.Invalidate();
            }
        }

        Pen selectionPen;
        public Pen SelectionPen
        {
            get { return this.pen; }

            set
            {
                this.selectionPen = value;
                this.form.Invalidate();
            }
        }

        public Line(Form form, Point startPoint, Point endPoint, 
            Pen pen = null, Pen selectionPen = null)
        {
            this.form = form;
            this.lineInfo = new LineInfo(startPoint, endPoint);
            this.lineInfo.CriticalPerpendicularLenght = 10;

            if (pen == null)
                this.pen = Pens.Blue;
            else
                this.pen = pen;

            if (selectionPen == null)
                this.selectionPen = Pens.Red;
            else
                this.selectionPen = selectionPen;

            this.form.Paint += Form_Paint;
            this.form.MouseDown += Form_MouseDown;
            this.form.MouseMove += Form_MouseMove;
            this.form.MouseUp += Form_MouseUp;
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            this.isCaptured = false;
            this.clickedPoint = Point.Empty;
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isCaptured && this.isPerpendicularLength)
            {
                Size size = Size.Subtract(new Size(e.Location), new Size(clickedPoint));
                lineInfo.Move(size);
                this.clickedPoint = e.Location;
                this.form.Invalidate();
                return;
            }

            checkCriticalLenght(e.Location);
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.isPerpendicularLength)
            {
                this.isCaptured = true;
                this.clickedPoint = e.Location;
            }
        }

        private void Form_Paint(object sender, PaintEventArgs e)
        {
            Pen p;
            if (isPerpendicularLength)
            {
                p = this.selectionPen;
                this.form.Cursor = Cursors.NoMove2D;
            }
            else
            {
                p = this.pen;
                this.form.Cursor = Cursors.Default;
            }

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.DrawLine(p, lineInfo.StartPoint, lineInfo.EndPoint);
        }

        void checkCriticalLenght()
        {
            Point point = this.form.PointToClient(Cursor.Position);
            checkCriticalLenght(point);
        }

        void checkCriticalLenght(Point point)
        {
            bool b = lineInfo.IsCriticalPerpendicularLength(point);

            if (b != isPerpendicularLength)
            {
                if (b)
                {
                    LineSelectedEventArgs args = new MiniDraw.LineSelectedEventArgs(point);
                    LineSelected(this, args);

                    if (args.Cancel)
                        return;
                }
                else
                    LineDeselected(this, new EventArgs());

                isPerpendicularLength = b;
                this.form.Invalidate();
            }
        }
    }

    public class LineSelectedEventArgs
    {
        Point point;
        public Point Point
        {
            get { return this.point; }
        }

        public bool Cancel { get; set; }

        public LineSelectedEventArgs(Point point)
        {
            this.point = point;
        }
    }
}
