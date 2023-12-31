﻿using MessagePack;
using System.Drawing;

namespace Agarme_Server.CostomType
{
    [MessagePackObject]
    public class HkRect
    {
        [Key(0)]
        public double X { get; set; }
        [Key(1)]
        public double Y { get; set; }
        [Key(2)]
        public double Width { get; set; }
        [Key(3)]
        public double Height { get; set; }

        public HkRect(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        [IgnoreMember]
        public double Left => X;
        [IgnoreMember]
        public double Top => Y;
        [IgnoreMember]
        public double Right => X + Width;
        [IgnoreMember]
        public double Bottom => Y + Height;

        [IgnoreMember]
        public double Area => Width * Height;

        public bool Contains(double x, double y) => x >= X && x <= X + Width && y >= Y && y <= Y + Height;

        public bool Contains(HkRect rect) => rect.Left >= Left && rect.Right <= Right && rect.Top >= Top && rect.Bottom <= Bottom;

        public bool Intersects(HkRect rect) => !(rect.Left > Right || rect.Right < Left || rect.Top > Bottom || rect.Bottom < Top);

        public HkRect Intersection(HkRect rect)
        {
            double x = Math.Max(Left, rect.Left);
            double y = Math.Max(Top, rect.Top);
            double width = Math.Min(Right, rect.Right) - x;
            double height = Math.Min(Bottom, rect.Bottom) - y;

            if (width < 0 || height < 0)
                return null;

            return new HkRect(x, y, width, height);
        }

        public HkRect Union(HkRect rect)
        {
            double x = Math.Min(Left, rect.Left);
            double y = Math.Min(Top, rect.Top);
            double width = Math.Max(Right, rect.Right) - x;
            double height = Math.Max(Bottom, rect.Bottom) - y;

            return new HkRect(x, y, width, height);
        }

        public override string ToString() => $"X: {X}, Y: {Y}, Width: {Width}, Height: {Height}";

        public static explicit operator RectangleF(HkRect rect) => new RectangleF((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
    }
}
