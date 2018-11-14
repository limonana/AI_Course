using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.Agents.GameAgents
{
    public class CooperativeCuttof
    {
        //private int _goal;
        private int _depth;
        private BaseTraveler _player1;
        private BaseTraveler _player2;
        public void SetParams(BaseTraveler player1,BaseTraveler player2, int depth)
        {
           // _goal = goal;
            _depth = depth;
            _player1 = player1;
            _player2 = player2;
        }


        public bool Run(TravelGameState state)
        {
            return 
                (state.totalMoves[_player1] >= _depth && state.totalMoves[_player2] >= _depth)
                || (state.locations[_player1] == _player1.Goal.Value && state.locations[_player2] == _player2.Goal.Value);
                
        }
    }
}
