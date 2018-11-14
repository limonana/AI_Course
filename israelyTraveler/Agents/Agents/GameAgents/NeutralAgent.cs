using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIBasic.Algorithms.Base;
using World;
using AIBasic.Algorithms.Games;
using Agents.Agents.SearchAgents;

namespace Agents.Agents.GameAgents
{
    using evalMethodType = SearchTypes<TravelGameState>.HuristicMethodType;
    using StateActionsCollection = SearchTypes<TravelGameState>.ActionsCollection;
    using StateAndCost = SearchTypes<TravelGameState>.StateAndCost;
    using StateActionType = SearchTypes<TravelGameState>.ActionType;
    using cuttOfType = SearchTypes<TravelGameState>.GoalMethodType;   
    

    public class NeutralAgent:BaseGameAgent
    {
        public NeutralAgent(int startLocation, int goal, UndoGameAgent otherPlayer,int depth,int MaxMoves):base(startLocation)
        {
            var cuttOf = new BasicCuttof();
            cuttOf.SetParams(this, depth);
            NaturalEval eval = new NaturalEval(goal, this,MaxMoves);
            Init(cuttOf.Run, eval.Run, new MaxiMax<TravelGameState>(false), goal, otherPlayer);
        }            

        public override string Name
        {
            get { return "Natural Agent"; }
        }
    }
}
