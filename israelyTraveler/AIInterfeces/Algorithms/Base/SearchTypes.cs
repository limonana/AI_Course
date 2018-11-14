using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBasic.Algorithms.Base
{
    public class SearchTypes<TSearchState>
    {
        public delegate StateAndCost ActionType(TSearchState state);
        
        public delegate double HuristicMethodType(TSearchState state);
        public class ActionsCollection:List<ActionType>{}        
        public delegate ActionsCollection OpertorsMethodType(TSearchState state);
        protected struct SearchStateAndAction
        {
            public TSearchState state;
            public ActionType action;
        }
        
        public struct StateAndCost
        {
            public StateAndCost(TSearchState state, double cost)
            {
                Cost = cost;
                State = state;
                
            }
            public TSearchState State;
            public double Cost;
        }

        public class SearchPath:Path<ActionType>{}

        public delegate bool GoalMethodType(TSearchState state);       
    }
}
