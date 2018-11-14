using Agents.Agents.GameAgents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World;

namespace Agents.Agents.SearchAgents
{
    public class NaturalEval
    {
         int _goal;
         BaseTraveler _player;
         private int _MaxMoves;
         public NaturalEval(int goal,BaseTraveler player,int MaxMoves)
        {
            _goal = goal;
            _player = player;
            _MaxMoves = MaxMoves;
        }

         public double Run(TravelGameState state)
         {
             if (state.locations[_player] == _goal)
                 return 0;
             TravelWorld world = state.ToWorld();
             TravelPath cheapPath = world.findCheapestPath(state.locations[_player], _goal);
             var closeToPenality = (_MaxMoves- state.totalMoves[_player])-cheapPath.Count;
             var res=  -cheapPath.Cost();
             if (closeToPenality <= 0)
                 res += 100;
             return res;
         }
    }
}
