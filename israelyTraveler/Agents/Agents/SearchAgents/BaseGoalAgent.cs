using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIBasic.Algorithms.Base;
using World;

namespace Agents.Agents.SearchAgents
{
    using HeuristicMethodType = BaseSearchAlgorithm<TravelSearchState>.HuristicMethodType;
    
    public abstract class BaseGoalAgent:BaseSearchAgent
    {
        public int Goal { get; protected set; }
        public BaseGoalAgent(HeuristicMethodType h, int startLocation, int goal)
            : base(h, startLocation)
        {
            Goal = goal;
        }

        public override ActionType GetNextAction(TravelWorld world)
        {
            if (CurrentLocation == Goal)
                return noOpertion;
            else
                return InnerGetNextAction(world);
        }

        protected abstract ActionType InnerGetNextAction(TravelWorld world);
    }
}
