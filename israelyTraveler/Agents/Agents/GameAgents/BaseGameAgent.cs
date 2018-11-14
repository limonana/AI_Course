using AIBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World;
using AIBasic.Algorithms.Base;
using AIBasic.Algorithms.Games;

namespace Agents.Agents.GameAgents
{
    using evalMethodType = SearchTypes<TravelGameState>.HuristicMethodType;
    using StateActionsCollection = SearchTypes<TravelGameState>.ActionsCollection;
    using StateAndCost = SearchTypes<TravelGameState>.StateAndCost;
    using StateActionType = SearchTypes<TravelGameState>.ActionType;
    using cuttOfType = SearchTypes<TravelGameState>.GoalMethodType;  
    public abstract class BaseGameAgent : UndoGameAgent
    {        
        cuttOfType _cuttFunc;
        evalMethodType _evalFunc;
        BaseGameAlgorithm<TravelGameState> _algo;
        protected UndoGameAgent _otherPlayer;

        public BaseGameAgent(int startLocation):base(startLocation) { }
        
        public BaseGameAgent(cuttOfType cuttFunc, evalMethodType evalFunc,
            BaseGameAlgorithm<TravelGameState> algo, int startLocation, int goal, UndoGameAgent otherPlayer)
            : base(startLocation)
        {
            Init(cuttFunc, evalFunc, algo, goal, otherPlayer);
        }

        protected void Init(cuttOfType cuttFunc, evalMethodType evalFunc, BaseGameAlgorithm<TravelGameState> algo, int goal, UndoGameAgent otherPlayer)
        {
            _algo = algo;
            _cuttFunc = cuttFunc;
            _evalFunc = evalFunc;
            Goal = goal;
            _otherPlayer = otherPlayer;
        }       

        public override ActionType GetNextAction(TravelWorld world)
        {
            if (CurrentLocation == Goal)
                return noOpertion;

            var stateAction = _algo.Run(ToState(world), getActions, _otherPlayer.getActions, _cuttFunc, _evalFunc);
            return _actionsMap[stateAction];
        }
    }
}
