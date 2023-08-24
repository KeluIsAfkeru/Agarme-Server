using Agarme_Server.Misc;
using Agarme_Server.CostomType;
using Agarme_Server.World;

namespace Agarme_Server.Entity
{
    public class Food: Cell
    {
        public Food(Map map) :base(map)
        {
            Location = new HKVector(HKRand.Double(0, map.config.MapWidth), HKRand.Double(0, map.config.MapHeight));
            Color = HKColor.colorTable[HKRand.Int(0, HKColor.colorTable.Length - 1)];
            Type = EntityType.Food;
            Mass = HKRand.Double(map.config.MinFoodSize, map.config.MaxFoodSize);
            Name = "";
            map.foods.Add(this);
        }
    }
}
