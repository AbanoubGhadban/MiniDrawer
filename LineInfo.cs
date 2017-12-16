using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using static System.Math;

namespace MiniDraw
{
    class LineInfo
    {
        Point startPoint = Point.Empty;
        public Point StartPoint {
            get { return this.startPoint; }
            set
            {
                if (this.isTheSamePoints(endPoint, value))
                    this.ThrowEmptyOrSamePointsException();

                this.startPoint = value;
                this.Refresh();
            }
        }

        Point endPoint = Point.Empty;
        public Point EndPoint
        {
            get { return this.endPoint; }
            set
            {
                if (this.isTheSamePoints(startPoint, value))
                    this.ThrowEmptyOrSamePointsException();

                this.endPoint = value;
                this.Refresh();
            }
        }

        float criticalPerpendicularLenght;
        public float CriticalPerpendicularLenght {
            get
            {
                return this.criticalPerpendicularLenght;
            }

            set
            {
                this.criticalPerpendicularLenght = value;
            }
        }

        public LineInfo(Point startPoint, Point endPoint)
        {
            if (isTheSamePoints(startPoint, endPoint))
                this.ThrowEmptyOrSamePointsException();

            this.startPoint = startPoint;
            this.endPoint = endPoint;

            this.Refresh();
        }

        public float Slope {
            get
            {
                float deltaY = startPoint.Y - endPoint.Y;
                float deltaX = startPoint.X - endPoint.X;
                return (deltaY / deltaX);
            }
        }

        public float YNod {
            get
            {
                Point point = getNonEmptyPoint();
                float y = point.Y;
                float x = point.X;
                float slope = this.Slope;

                //Y = X(slope) + YNod
                float d = slope * x;
                float f = y - d;
                return (y - slope * x);
            }
        }

        public float PerpendicularLength(Point point)
        {
            float A = 1;
            float B = -(this.Slope);
            float C = -(this.YNod);

            int x = point.X, y = point.Y;

            //Look for equation of length of perpendicular from point to line
            return (Abs(A * y + B * x + C) / (float)Sqrt((Pow(A, 2) + (Pow(B, 2)))));
        }

        public bool IsCriticalPerpendicularLength(Point point)
        {
            float l = PerpendicularLength(point);
            if (l <= this.CriticalPerpendicularLenght)
            {
                int minY = Min(startPoint.Y, endPoint.Y);
                int maxY = Max(startPoint.Y, endPoint.Y);
                int currentY = point.Y;

                if (currentY >= minY && currentY <= maxY)
                    return true;
            }
            return false;
        }

        public void Move(Size size)
        {
            this.startPoint = Point.Add(this.startPoint, size);
            this.endPoint = Point.Add(this.endPoint, size);
        }

        public void Refresh()
        {

        }

        bool isTheSamePoints(Point point1, Point point2)
        {
            return (point1 == point2);
        }

        void ThrowEmptyOrSamePointsException()
        {
            throw new ArgumentException("You send the same point, that's not valid");
        }

        Point getNonEmptyPoint()
        {
            if (this.startPoint == Point.Empty)
                return this.endPoint;
            else
                return this.startPoint;
        }
    }
}
