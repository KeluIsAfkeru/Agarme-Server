namespace Agarme_Server.CostomType
{
    public interface IPoint
    {
        double X { get; set; } // X坐标
        double Y { get; set; } // Y坐标

        double CalculateDistance(IPoint target); // 计算到目标点的距离
        double CalculateMagnitude(); // 计算点的模长
    }
}
