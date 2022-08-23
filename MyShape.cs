using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;

namespace Paint_app
{
    /// <summary>
    /// sealed prevent class from being inherited by another class
    /// </summary>
    [Serializable]
    public sealed class MyPoint
    {
        public float X { get; set; } 
        public float Y { get; set; }
        
        public MyPoint()
        {
            X = 0;
            Y = 0;
        }

        public MyPoint(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    #region MyShape
    [Serializable]
    public abstract class MyShape 
    {
        public MyPoint StartPoint { get; set; }

        public Color myColor = new Color();
        public MyShape(MyPoint point,Color color)  //constructor -- gets fron inheritances classes the values of x and y//
        {
            StartPoint = point;
            this.myColor = color;
        }

        /// <summary>
        /// abstract function --lets the user draw the shape with graphics
        /// </summary>
        /// <param name="graphics"></param>
        public abstract void Draw(Graphics graphics);

        /// <summary>
        /// abstract function --lets the user check is the point in in the shape area
        /// </summary>
        /// <returns></returns>
        public abstract bool IsInShape(MyPoint point); 

        /// <summary>
        /// Destructor!!
        /// </summary>
        ~MyShape() { }  
    }

    #endregion MyShape

    #region MyRectangle
    [Serializable]
    public class MyRectangle : MyShape                        
    {
        #region Properties
        public MyPoint EndPoint { get; set; }

        /// <summary>
        /// get, set to width
        /// </summary>
        public float Width             
        {
            get
            {
                return EndPoint.X - StartPoint.X;
            }
            set { }
            
        }

        /// <summary>
        /// get, set to height
        /// </summary>
        public float Height            
        {
            get
            {
                return EndPoint.Y - StartPoint.Y;
            }
        }

        #endregion Properties

        public MyRectangle(MyPoint startPoint, MyPoint endPoint,Color myColor) 
            : base(startPoint, myColor)
        {
            EndPoint = endPoint;
        }

        #region Override

        /// <summary>
        /// draws a rectangle after the object gets all his values
        /// after mouse up!!
        /// </summary>
        /// <param name="graphics"></param>
        public override void Draw(Graphics graphics)
        {
            SolidBrush myBrush = new SolidBrush(myColor);
            DrawRectangle(graphics, StartPoint.X, StartPoint.Y, Width, Height);
        }

        public override bool IsInShape(MyPoint point)
        {
            bool isInShape = point.X > StartPoint.X && point.X < EndPoint.X && point.Y > StartPoint.Y && point.Y < EndPoint.Y;

            return isInShape;
        }

        protected void DrawRectangle(Graphics graphic, float x, float y, float witdh, float height)
        {
            SolidBrush myBrush = new SolidBrush(myColor);
            graphic.FillRectangle(myBrush, x, y, witdh, height);
        }

        #endregion Override

    }

    #endregion MyRectangle

    #region MyCircle
    [Serializable]
    public class MyCircle : MyShape
    {
        public MyPoint EndPoint { get; set; }
        public MyCircle(MyPoint startPoint, MyPoint endPoint,Color myColor)
           : base(startPoint,myColor)
        {
            EndPoint = endPoint;
        }
        public float Radius
        {
            get
            {
                double sumX = (StartPoint.X - EndPoint.X) * (StartPoint.X - EndPoint.X);
                double  sumY = (StartPoint.Y - EndPoint.Y) * (StartPoint.Y - EndPoint.Y);
                double sumAll = Math.Sqrt(sumY + sumX);
                return (float)(sumAll);
            }
        }
        
        public override void Draw(Graphics graphics)
        {
            SolidBrush myBrush = new SolidBrush(myColor);
            graphics.FillEllipse(myBrush, StartPoint.X - Radius, StartPoint.Y - Radius, 2 * Radius, 2 * Radius);
        }

        public override bool IsInShape(MyPoint point)
        {
            bool isInShape = (point.X - StartPoint.X) * (point.X - EndPoint.X) + (point.Y - StartPoint.Y) * (point.Y - EndPoint.Y) <= Radius * Radius;

            return isInShape;
        }
    }

    #endregion MyCircle

    #region MySquare

    [Serializable]
    public class MySquare : MyRectangle
    {
        public MySquare(MyPoint startPoint, MyPoint endPoint,Color myColor) : base(startPoint, endPoint, myColor)
        {
            Width = Height;
        }

        public override void Draw(Graphics graphic)
        {
            SolidBrush myBrush = new SolidBrush(myColor);
            DrawRectangle(graphic, StartPoint.X, StartPoint.Y, Height, Height);
        }                       
    }

    #endregion MySquare

    #region MyRhombus
    [Serializable]
    public class MyRhombus : MyRectangle             
    {
        public override void Draw(Graphics graphics)
        {
            SolidBrush myBrush = new SolidBrush(myColor);
            Width = Height;
            using (GraphicsPath myPath = new GraphicsPath())
            {
                myPath.AddLines(new[]
                {
                    new PointF(StartPoint.X, StartPoint.Y),
                    new PointF(StartPoint.X + Width, StartPoint.Y),
                    new PointF(StartPoint.X + Width + 50, StartPoint.Y + Height),
                    new PointF(StartPoint.X + 50, StartPoint.Y + Height),
                    new PointF(StartPoint.X, StartPoint.Y)
                });
                graphics.FillPath(myBrush, myPath);
            }
        }

        public MyRhombus(MyPoint startPoint, MyPoint endPoint,Color myColor) 
            : base(startPoint, endPoint, myColor) 
        {
        }

        public override bool IsInShape(MyPoint point)
        {
            bool isInShape = point.X > StartPoint.X && point.X < EndPoint.X && point.Y > StartPoint.Y && point.Y < EndPoint.Y;

            return isInShape;
        }

    }

    #endregion MyRhombus
}
