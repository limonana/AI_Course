using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.Agents.GameAgents
{
    public class BasicCuttof
    {
        //private int _goal;
        private int _depth;
        private BaseTraveler _player;
        public void SetParams(BaseTraveler player, int depth)
        {
           // _goal = goal;
            _depth = depth;
            _player = player;
        }


        public bool Run(TravelGameState state)
        {
            return (state.totalMoves[_player] >= _depth) || (state.locations[_player] == _player.Goal.Value);
                
        }
    }
}
