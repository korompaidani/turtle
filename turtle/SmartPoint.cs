using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace turtle
{
    public class SmartPoint : IEquatable<SmartPoint>
    {
        private Point dir;
        private Point moveAxis;

        private int x;
        private int y;

        public int X { get{ return x;} }
        public int Y { get{ return y;} }

        public SmartPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
            dir = new Point(1, 1);
            moveAxis = new Point(1, 0);
        }

        public SmartPoint(Point point) : this(point.X, point.Y)
        {            
        }

        public static SmartPoint operator +(SmartPoint sp1, SmartPoint sp2)
        {
            return new SmartPoint(sp1.X + sp2.X, sp1.Y + sp2.Y);
        }

        public static SmartPoint operator *(SmartPoint sp1, SmartPoint sp2)
        {
            return new SmartPoint(sp1.X * sp2.X, sp1.Y * sp2.Y);
        }

        public static bool operator ==(SmartPoint sp1, SmartPoint sp2)
        {
            if (sp2 is null)
            {
                return sp1 is null;
            }
            return sp1.X == sp2.X && sp1.Y == sp2.Y;
        }            

        public static bool operator !=(SmartPoint sp1, SmartPoint sp2)
        {
            return !(sp1==sp2);
        }

        public override int GetHashCode()
        {
            return x * 13 ^ y;
        }

        public bool Equals([AllowNull]SmartPoint other)
        {
            if (other == null)
            {
                return false;
            }

            return (this.x == other.x)
                && (this.y == other.y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is SmartPoint))
            {
                return false;
            }
            return (this.x == ((SmartPoint)obj).x)
                && (this.y == ((SmartPoint)obj).y);
        }

        public bool IsPointInRange(SmartPoint rangeEndCoordinates)
        {
            var rangeStartCoordinates = 0;
            return X <= rangeEndCoordinates.X && Y <= rangeEndCoordinates.Y
                && X >= rangeStartCoordinates && Y >= rangeStartCoordinates;
        }

        public SmartPoint Move()
        {
            var temp = new SmartPoint(x, y);
            var d = new SmartPoint(dir);
            var m = new SmartPoint(moveAxis);
            var result = temp + d * m;
            result.dir.X = dir.X;
            result.dir.Y = dir.Y;
            result.moveAxis.X = moveAxis.X;
            result.moveAxis.Y = moveAxis.Y;
            return result;
        }

        /// <summary>
        /// 90 degrees to clockwise repl. (x,y)  with (y,−x) it's not magic number just math :).
        /// </summary>
        public void CounterClockWise()
        {            
            int temp = dir.X;
            dir.X = dir.Y;
            dir.Y = temp * -1;

            moveAxis = moveAxis.MoveAxisSwap();
        }

        /// <summary>
        /// 90 degrees to counter clockwise repl. (x,y)  with (−y,x) it's not magic number just math :).
        /// </summary>
        public void ClockWise()
        {
            int temp = dir.X;
            dir.X = dir.Y * -1;
            dir.Y = temp;

            moveAxis = moveAxis.MoveAxisSwap();
        }

        public void SetNord() { dir.X = 1; dir.Y = 0; moveAxis.X = 1; moveAxis.Y = 0; }

        public void SetSouth() { dir.X = -1; dir.Y = 0; moveAxis.X = 1; moveAxis.Y = 0; }

        public void SetEast() { dir.X = 0; dir.Y = 1; moveAxis.X = 0; moveAxis.Y = 1; }

        public void SetWest() { dir.X = 1; dir.Y = -1; moveAxis.X = 0; moveAxis.Y = 1; }
    }
}
