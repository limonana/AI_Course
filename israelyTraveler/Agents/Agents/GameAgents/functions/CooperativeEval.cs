using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World;

namespace Agents.Agents.GameAgents
{
    public class CoperativeEval
    {
        BaseTraveler _player;
        BaseTraveler _rival;
        //int _goal;
        //int _rivalGoal;
        int _totalMoves;
        public void SetParams(BaseTraveler player, BaseTraveler rival, int totalMoves)
        {
            _player = player;
            //_goal = goal;
            _totalMoves = totalMoves;
            _rival = rival;
            //_rivalGoal = rivalGoal;
        }

        public double Run(TravelGameState state)
        {
            int a = 10;
            int b = a;
            int c = 2;
            int d = c;
            var world = state.ToWorld();
            
            bool real;
            double distanceFromGoal = GetDistanceFromGoal(world,state.locations[_player], _player.Goal.Value,out real);
            if (!real)
                a -= 2;

            double avoidPenality = (_totalMoves - state.totalMoves[_player]) - distanceFromGoal;
            int rivalDistance = 0;
            double avoidRivalPenality =0;
            if (_rival.Goal != null)
            {
                bool realRivalDest;
                rivalDistance = GetDistanceFromGoal(world, state.locations[_rival], _rival.Goal.Value,out realRivalDest);
                if (!realRivalDest)
                    b-=2;
                avoidRivalPenality = (_totalMoves - state.totalMoves[_rival]) - rivalDistance;
            }
            

            return -a * distanceFromGoal - b * rivalDistance+ c* avoidPenality  +d * avoidRivalPenality;
        }

        private int GetDistanceFromGoal(TravelWorld world,int currentLoc,int goal,out bool realDistance)
        {
            if (currentLoc == goal)
            {
                realDistance = true;
                return 0;
            }

            var path = world.findCheapestCleartPath(currentLoc, goal);

            if (path != null)
            {
                realDistance = true;
                return path.Count;
            }
            else
            {
                realDistance = false;
                path = world.findCheapestPath(currentLoc, goal);
                if (path == null)
                    return int.MaxValue;
                else
                    return path.Count;
            }
        }
    }
}
