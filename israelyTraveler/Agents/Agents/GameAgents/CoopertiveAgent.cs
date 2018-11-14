using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIBasic.Algorithms.Base;
using World;
using AIBasic.Algorithms.Games;

namespace Agents.Agents.GameAgents
{
    using evalMethodType = SearchTypes<TravelGameState>.HuristicMethodType;
    using StateActionsCollection = SearchTypes<TravelGameState>.ActionsCollection;
    using StateAndCost = SearchTypes<TravelGameState>.StateAndCost;
    using StateActionType = SearchTypes<TravelGameState>.ActionType;
    using cuttOfType = SearchTypes<TravelGameState>.GoalMethodType;
    

    public class CoopertiveAgent:BaseGameAgent
    {
        public CoopertiveAgent(int startLocation,int goal,UndoGameAgent otherPlayer,int depth,int MaxMoves)
            : base(startLocation)
        {
            var cuttOf = new CooperativeCuttof();
            cuttOf.SetParams(this, otherPlayer,depth);
            CoperativeEval eval = new CoperativeEval();
            eval.SetParams(this,otherPlayer, MaxMoves);
            Init(cuttOf.Run, eval.Run, new MaxiMax<TravelGameState>(true), goal, otherPlayer);
        }

        public override string Name
        {
            get { return "Coopertive Agent"; }
        }
    }
}
