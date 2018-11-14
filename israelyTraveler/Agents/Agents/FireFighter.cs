using AIBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World;
using Agents.Agents.GameAgents;

namespace Agents
{
    public class FireFighter : UndoGameAgent
    {
        TravelPath _firePath = null;
        TravelPath _waterPath = null;

        public FireFighter(int startLocation) : base(startLocation) { }


        public override ActionType GetNextAction(TravelWorld world)
        {
            if (CarryWater)
            {
                return actionToStopFire(world);

            }
            else
            {
                return actionToFindWater(world);
            }

        }

        private ActionType actionToFindWater(TravelWorld currWorld)
        {
            if (currWorld.HaveWater(CurrentLocation))
                return new ActionType(world => { return pickupWater(world); });

            if (_waterPath == null || _waterPath.Count() ==0 || currWorld.isPathClear(_waterPath))
                _waterPath = findWaterPath(currWorld);


            if (_waterPath != null && _waterPath.Count > 0)
            {
                int nextPlace = _waterPath.TakeNextStep();
                return new ActionType(world => { return drive(world, nextPlace); });
            }
            else
                return noOpertion;
        }

        private ActionType actionToStopFire(TravelWorld currWorld)
        {
            if (_firePath == null)
                _firePath = findFirePath(currWorld);

            if (_firePath != null && _firePath.Count > 0)
            {
                int nextPlace = _firePath.TakeNextStep();                
                return new ActionType(world => { return drive(world, nextPlace); });
            }
            else
                return noOpertion;
        }

        private TravelPath findWaterPath(TravelWorld world)
        {            
            var paths =  world.findCheapestWaterPaths(CurrentLocation);
            
            if (paths == null || paths.Count() == 0)
                return null;

            return findMinDest(paths);
        }

        private TravelPath findMinDest(IEnumerable<TravelPath> paths)
        {
            if (paths.Count() == 0)
                return null;

            int minDest = paths.Min(path => path.Destination);
            return paths.Single(path => path.Destination == minDest);
        }

        private TravelPath findFirePath(TravelWorld world)
        {
            var paths = world.findCheapestFirePaths(CurrentLocation);
            if (paths == null || paths.Count() == 0)
                return null;

            return findMinDest(paths);
        }

        public override string Name
        {
            get { return "fireFighther"; }
        }
    }
}
