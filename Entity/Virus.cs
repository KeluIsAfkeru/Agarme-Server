using Agarme_Server.CostomType;
using Agarme_Server.Misc;
using Agarme_Server.World;

namespace Agarme_Server.Entity
{
    public class Virus: Cell
    {
        public Virus(Map map) : base(map)
        {
            Color = HKColor.colorTable[0]; // 病毒默认为黄色
            Location = new HKVector(HKRand.Double(0, map.config.MapWidth), HKRand.Double(0, map.config.MapHeight));
            Type = EntityType.Virus;
            Mass = HKRand.Double(map.config.VirusSize);
            Name = "";
            map.viruses.Add(this);
        }
    }
}
