using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MiniDraw
{
    public partial class Form1 : Form
    {
        List<Line> selectedLines = new List<Line>();
        List<LineDrawer> linesInDraw = new List<LineDrawer>();

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.selectedLines.Count == 0 && this.linesInDraw.Count == 0)
            {
                LineDrawer ld = new LineDrawer(this, e.Location);
                ld.LineAdded += Ld_LineAdded;
                this.linesInDraw.Add(ld);
            }
        }

        private void Ld_LineAdded(object obj, LineAddedEventArgs e)
        {
            Line line = new Line(this, e.StartPoint, e.EndPoint);
            line.LineSelected += Line_LineSelected;
            line.LineDeselected += Line_LineDeselected;
            this.linesInDraw.Remove((LineDrawer)obj);
        }

        private void Line_LineDeselected(object sender, EventArgs e)
        {
            this.selectedLines.Remove((Line)sender);
        }

        private void Line_LineSelected(object sender, LineSelectedEventArgs e)
        {
            if (this.selectedLines.Count == 0 && this.linesInDraw.Count == 0)
                this.selectedLines.Add((Line)sender);
            else
                e.Cancel = true;
        }
    }
}
