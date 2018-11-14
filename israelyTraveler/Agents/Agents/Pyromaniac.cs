using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World;
using Agents.Agents.GameAgents;

namespace Agents
{
    public class Pyromaniac : UndoGameAgent
    {
        public Pyromaniac(int sleepMoves,int startLocation) : base(startLocation)
        {
            this.sleepMoves = sleepMoves;
        }

        int sleepMoves;
        int numOfMoves = 0;
        

        public override ActionType GetNextAction(TravelWorld world)
        {
            ++numOfMoves;
            if (numOfMoves == sleepMoves)
            {
                numOfMoves = 0;
                var ways = world.GetClearWays(CurrentLocation);
                if (ways == null || ways.Count() == 0)
                    return noOpertion;
                else
                {
                    double minDistance = ways.Min(way => world.getCostWay(way));
                    ways = ways.Where(way => world.getCostWay(way) == minDistance);
                    int minDest = ways.Min(way => way.getAnotherPlace(CurrentLocation));
                    return new ActionType(w => startAfire(w, minDest));
                }
            }
            else
                return new ActionType(w => false);

        }

        public override string Name
        {
            get { return "Pyromaniac"; }
        }
    }
}
