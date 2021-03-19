using System;
using System.Drawing;

namespace MathGrapher
{
    public  class DrawGraph
    {
        private Graphics graphics;
        private int canvasWidth;
        private int canvasHeight;
        private double domainRangeMinX = -100;
        private double domainRangeMaxX = 100;
        private double domainRangeMinY = -100;
        private double domainRangeMaxY= 100;


        /// <summary>
        /// Creates an object to drawn on
        /// </summary>
        /// <param name="g">The Graphics object to draw on</param>
        /// <param name="canvasWidth">The pixel size</param>
        /// <param name="canvasHeight"></param>
        public DrawGraph(Graphics g , int canvasWidth, int canvasHeight)
        {
            graphics =g;
            this.canvasWidth = canvasWidth;
            this.canvasHeight = canvasHeight;
        }
        
        public void DrawAxis()
        {
            Pen blackPen = new Pen(Color.Black, 3);

            Point p1 = new Point(0, canvasHeight / 2);
            Point p2 = new Point(canvasWidth, canvasHeight / 2);
            
            graphics.DrawLine(blackPen, p1, p2);

            p1 = new Point(canvasWidth / 2, 0);
            p2 = new Point(canvasWidth / 2, canvasHeight);
            graphics.DrawLine(blackPen, p1, p2);
        }

        public void SetDomainRangeforX(double startX, double endX)
        {
            this.domainRangeMinX = startX;
            this.domainRangeMaxX = endX;
        }

        public void SetDomainRangeforY(double startY, double endY)
        {
            this.domainRangeMinY = startY;
            this.domainRangeMaxY = endY;
        }

        public double XResoulution 
        { 
            get
            {
                return  (domainRangeMaxX - domainRangeMinX) / canvasWidth;
            }
        
        }

        public double YResoulution
        {
            get
            {
                return (domainRangeMaxY - domainRangeMinY) / canvasWidth;
            }

        }

        private Point TransformToPixels(double x, double y)
        {
            Point result = new Point();
            result.X = (int) ( (((x - domainRangeMinX) / (domainRangeMaxX - domainRangeMinX)) * canvasWidth));
            result.Y = canvasHeight- (int)((((y - domainRangeMinY) / (domainRangeMaxY - domainRangeMinY)) * canvasHeight));
            return result;
        }

        private bool IsPointInCanvas(Point p)
        {
            return (p.X > 0 && p.X < canvasWidth && p.Y > 0 && p.Y < canvasHeight);
        }
        public void DrawEquation(string equation )
        {

            MathLibrary.PostFixExpression exp = new MathLibrary.PostFixExpression();
            exp.TryParsePostFixExpression(equation);

            double increment = (domainRangeMaxX - domainRangeMinX) / canvasWidth;

            double xPosition = domainRangeMinX;
            Point lastPoint = new Point();

            while (xPosition < domainRangeMaxX)
            {
                double y = 0;
                exp.TryToEvaluateForVariable("x", xPosition, ref y);
                Point p1 = TransformToPixels(xPosition, y);
                if (IsPointInCanvas(p1)&& IsPointInCanvas(lastPoint))
                {
                    graphics.DrawLine(System.Drawing.Pens.DarkRed, lastPoint, p1);
                }

                lastPoint = p1;
                xPosition = xPosition + increment;
            }
            
        }

    }
}
