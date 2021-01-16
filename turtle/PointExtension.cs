using System.Drawing;

namespace turtle
{
    public static class PointExtension
    {
        public static Point Multiply(this Point point, Point otherPoint)
        {
            return new Point(point.X * otherPoint.X, point.X * otherPoint.Y);
        }

        public static Point Add(this Point point, Point otherPoint)
        {
            return new Point(point.X + otherPoint.X, point.X + otherPoint.Y);
        }

        public static Point MoveAxisSwap(this Point point)
        {
            return new Point(point.Y, point.X);
        }
    }
}
