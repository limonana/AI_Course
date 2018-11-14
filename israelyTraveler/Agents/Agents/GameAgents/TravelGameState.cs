using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using World;
using Agents.Agents.SearchAgents;

namespace Agents.Agents.GameAgents
{
    public class TravelGameState
    {
        public Dictionary<BaseTraveler,int> totalMoves;
        public Dictionary<BaseTraveler, int> locations;        

        public bool CarryWatter;
        public List<int> WaterPlaces;
        public List<TravelEdge> FireWays;
        public TravelGraph WorldGraph;

        public TravelWorld ToWorld()
        {
            var world = new TravelWorld(WorldGraph);
            world.PutWater(WaterPlaces.ToArray());
            foreach (var fireWay in FireWays)
            {
                world.SetFire(fireWay.Source, fireWay.Target);
            }
            foreach (var item in locations)
            {
                world.SetLocation(item.Key, item.Value);
            }
            return world;
        }
    }
}
