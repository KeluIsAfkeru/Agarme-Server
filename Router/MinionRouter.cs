using Agarme_Server.Misc;
using Agarme_Server.CostomType;

namespace Agarme_Server.Router
{
    public class MinionRouter: Router
    {
        public PlayerRouter Owner;
        public HKVector Destination { set; get; }

        public MinionRouter(PlayerRouter owner) : base()
        {
            Owner = owner;

            double center_x = Config.MapWidth * 0.5;
            double center_y = Config.MapHeight * 0.5;

            Destination = new(center_x, center_y);
        }

    }
}
