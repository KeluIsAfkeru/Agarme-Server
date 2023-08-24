namespace Agarme_Server.CostomType
{
    public interface IRgba<T> : IRgb<T>
    {
        T A { get; set; }//透明度
    }
}
