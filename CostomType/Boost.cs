using Agarme_Server.Misc;

namespace Agarme_Server.CostomType
{
    /// <summary>
    /// 代表在某个方向上的加速。
    /// </summary>
    public class Boost
    {
        public double X { get => Orientation.X; }
        public double Y { get => Orientation.Y; }

        /// <summary>
        /// 距离
        /// </summary>
        public double Distance { get; private set; }

        private HKVector Orientation;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Boost(double x, double y, double d)
        {
            Orientation = new HKVector(x, y);
            Distance = d;
        }

        public Boost(HKVector orientation, double d)
        {
            Orientation = new HKVector(orientation.X, orientation.Y); // 创建一个新的实例以避免外部变化
            Distance = d;
        }

        /// <summary>
        /// 改变偏移方向
        /// </summary>
        public void ChangeOrientation(double x,double y)
        {
            Orientation.X = x;
            Orientation.Y = y;
        }

        /// <summary>
        /// 改变X方向
        /// </summary>
        public void ChangeX(double x)
        {
            ChangeOrientation(x, Y);
        }

        /// <summary>
        /// 改变Y方向
        /// </summary>
        public void ChangeY(double y)
        {
            ChangeOrientation(X, y);
        }

        /// <summary>
        /// 改变距离
        /// </summary>
        public void ChangeDistance(double distance)
        {
            Distance = distance;
        }

        /// <summary>
        /// 获取Orientation的深拷贝
        /// </summary>
        public HKVector GetOrientation()
        {
            return new HKVector(Orientation.X, Orientation.Y);
        }
    }
}