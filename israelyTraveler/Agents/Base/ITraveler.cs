using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World;

namespace Agents
{
    public interface ITraveler
    {
        bool drive(TravelWorld world, int adjacentPlace);
        bool startAfire(TravelWorld world, int adjacentPlace);
        bool pickupWater(TravelWorld world);
        bool noOpertion(TravelWorld world);
        int CurrentLocation { get; }
        bool CarryWater { get;}
    }
}
