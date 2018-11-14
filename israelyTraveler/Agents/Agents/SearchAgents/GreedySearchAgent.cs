using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIBasic;
using World;
using AIBasic.Algorithms.Base;

namespace Agents.Agents.SearchAgents
{
    using HeuristicMethodType = BaseSearchAlgorithm<TravelSearchState>.HuristicMethodType;    
    using StateActionType = BaseSearchAlgorithm<TravelSearchState>.ActionType;
    public class GreedySearchAgent:BaseSearchAgent  
    {
        public GreedySearchAgent(HeuristicMethodType h, int startLocation) : base(h, startLocation) 
        {
            Expanstions = 1;
        }        

        public override ActionType GetNextAction(TravelWorld world)
        {
            var actions = getActions(world);
            if (actions == null || actions.Count() == 0)
                return noOpertion;

            //there is not point to evealute the huristic function
            if (actions.Count() == 1)
                return _actionsMap[actions.First()];

            var searchState = ToSearchState(world);
            double min = double.MaxValue;
            StateActionType chosenAction = null;
            foreach (var action in actions)
            {
                var cost = _heuristic(action(searchState).State);
                if (cost < min)
                {
                    min = cost;
                    chosenAction = action;
                }
            }
            
            return _actionsMap[chosenAction];
        }

        public override string Name
        {
            get { return "greedy search"; }
        }
    }
}
