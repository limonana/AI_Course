using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using World;

namespace Agents.Agents.SearchAgents
{
    public class TravelSearchState : IEquatable<TravelSearchState>
    {        
        public int CurrentLocation;        
                
        public bool CarryWatter;
        public List<int> WaterPlaces;
        public List<TravelEdge> FireWays;
        public TravelGraph WorldGraph;

        public  TravelWorld ToWorld()
        {
            var world = new TravelWorld(WorldGraph);
            world.PutWater(WaterPlaces.ToArray());
            foreach (var fireWay in FireWays)
            {
                world.SetFire(fireWay.Source, fireWay.Target);
            }
            return world;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is TravelSearchState)
            {
                return Equals((TravelSearchState)obj);
            }
            else
                return false;
        }

        public bool Equals(TravelSearchState a)
        {
            return CurrentLocation.Equals(a.CurrentLocation) &&
               CarryWatter.Equals(a.CarryWatter) &&
               FireWays.SequenceEqual(a.FireWays) &&
               WaterPlaces.SequenceEqual(a.WaterPlaces);
        }
    }
}
