using Agarme_Server.Misc;
using Agarme_Server.CostomType;
using Agarme_Server.World;

namespace Agarme_Server.Entity
{
    public struct EntityType
    {
        public const byte Food = 0;
        public const byte Player = 1;
        public const byte Virus = 2;
        public const byte RedVirus = 3;
        public const byte Eject = 4;
        public const byte Minion = 5;
    }

    public abstract class Cell
    {
        public Map map { get; set; }
        public HKVector Location { get; set; }
        public Boost Boosting { get; set; }
        public string Name { get; set; }
        public byte Type { get; protected set; }
        public uint Id { get; protected set; }
        public uint Color { get; set; }
        public bool Deleted { get; protected set; }
        public double R { get; protected set; }
        public double Mass { get; set; }
        public double Speed { get; set; }

        public double X { get => Location.X; }
        public double Y { get => Location.Y; }
        public HkRect Range { get => new HkRect(X - R, Y - R, 2 * R, 2 * R); }

        private static readonly IdAllocator Allocator = new IdAllocator();

        public Cell(Map map)
        {
            this.map = map;
            AllocateId();

            map.cells.Add(this);
        }

        public Cell(Map map, double x, double y)
        {
            this.map = map;
            AllocateId();

            Location = new HKVector(x,y);

            map.cells.Add(this);
        }

        public virtual void Tick() { }
        public virtual void Remove() { Deleted = true; }


        public void MonitorBorderCollide(bool isBoarderCollision, bool isBoarderCollisionBounce, double boarderCover, HkRect map)
        {
            if (!isBoarderCollision)
                return;

            (double minX, double maxX) = (map.Left + R * boarderCover, map.Right - R * boarderCover);
            (double minY, double maxY) = (map.Top + R * boarderCover, map.Bottom - R * boarderCover);

            if (X > maxX)
            {
                Location.X = maxX;
                if (isBoarderCollisionBounce)
                    Boosting.ChangeX(-Boosting.X);
            }
            else if (X < minX)
            {
                Location.X = minX;
                if (isBoarderCollisionBounce)
                    Boosting.ChangeX(-Boosting.X);
            }

            if (Y > maxY)
            {
                Location.Y = maxY;
                if (isBoarderCollisionBounce)
                    Boosting.ChangeY(-Boosting.Y);
            }
            else if (Y < minY)
            {
                Location.Y = minY;
                if (isBoarderCollisionBounce)
                    Boosting.ChangeY(-Boosting.Y);
            }
        }


        public void CollideWith(Cell other)
        {

        }


        public void MoveTo(HKVector des, double speed)
        {
            if (des.Equals(Location))
                return;

            double dx = des.X - X;
            double dy = des.Y - Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance <= speed)
            {
                Location.X = des.X;
                Location.Y = des.Y;
            }
            else
            {
                var angle = Math.Atan2(dy, dx); //求出夹角的弧度
                Location.X += Math.Cos(angle) * speed;
                Location.Y += Math.Sin(angle) * speed;
            }
        }


        public void UpdateMass(double mass)
        {
            Mass = mass;
        }


        public void AllocateId()
        {
            Id = Allocator.Allocate();
        }


        public void RecycleId(uint identity)
        {
            Allocator.Recycle(identity);
        }


        public void ResetIds()
        {
            Allocator.Reset();
        }


        public double Distance(HKVector des) => (new HKVector(des.X - X, des.Y - Y)).CalculateMagnitude();


        public double Distance(Cell other) => Distance(other.Location);


        public bool InRect(HkRect rect) => rect.Contains(Range);


        public HKColor GetColor() => new HKColor(Color);

        public override string ToString() => $"[Location{Location},R:{R},ID:{Id},Color:{Color},Type:{Type}]";
    }
}
