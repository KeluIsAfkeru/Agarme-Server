namespace Agarme_Server.CostomType
{
    public class HKVector : IPoint
    {
        public double X { get; set; }
        public double Y { get; set; }

        public HKVector() { }

        public HKVector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public HKVector(HKVector point)
        {
            X = point.X;
            Y = point.Y;
        }

        public static HKVector operator +(HKVector point1, HKVector point2) =>
            new HKVector(point1.X + point2.X, point1.Y + point2.Y);

        public static HKVector operator -(HKVector point1, HKVector point2) =>
            new HKVector(point1.X - point2.X, point1.Y - point2.Y);

        public static double operator *(HKVector point1, HKVector point2) =>
            point1.X * point1.X + point1.Y * point1.Y;

        public double CalculateMagnitude() =>
            Math.Sqrt(X * X + Y * Y);

        public double CalculateDistance(IPoint target)
        {
            double deltaX = target.X - X;
            double deltaY = target.Y - Y;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        public override string ToString() =>
            $"X: {X}, Y: {Y}";

        public bool Equals(HKVector other) => X == other.X && Y == other.Y;

        public static explicit operator System.Drawing.Point(HKVector point) =>
            new System.Drawing.Point((int)point.X, (int)point.Y);

        public static explicit operator System.Drawing.PointF(HKVector point) =>
            new System.Drawing.PointF((float)point.X, (float)point.Y);
    }
}
