using Agarme_Server.Misc;
using Agarme_Server.Router;
using Agarme_Server.CostomType;
using Agarme_Server.World;

namespace Agarme_Server.Entity
{
    public class Minion : Cell
    {
        public MinionRouter Owner;
        public double CollisionC = 0;//碰撞计时
        public double FusionC = 0;//合体计时
        public double EjectC = 10;//吐球间隔计时
        public double LossC = 0;//质量衰减间隔计时

        public Minion(Map map, MinionRouter owner) : base(map)
        {
            this.map = map;
            Owner = owner;

            Type = EntityType.Minion;
            Location = new HKVector(HKRand.Double(0, map.config.MapWidth), HKRand.Double(0, map.config.MapHeight));
            Name = Owner.Name;
            Mass = map.config.InitialPlayerMass;
            Color = HKColor.colorTable[HKRand.Int(0, HKColor.colorTable.Length - 1)];
            Owner.Color = Color;

            map.minions.Add(this);
        }

        public Minion(Map map, MinionRouter owner, double x, double y) : base(map, x, y)
        {
            this.map = map;
            Owner = owner;

            Type = EntityType.Minion;
            Name = Owner.Name;
            Mass = map.config.InitialPlayerMass;
            Color = HKColor.colorTable[HKRand.Int(0, HKColor.colorTable.Length - 1)];
            Owner.Color = Color;

            map.minions.Add(this);
        }
    }
}
