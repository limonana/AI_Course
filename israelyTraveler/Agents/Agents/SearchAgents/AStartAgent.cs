using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIBasic;
using AIBasic.Algorithms.Base;
using World;
using AIBasic.Algorithms.Search;

namespace Agents.Agents.SearchAgents
{
    using SearchPath = BaseSearchAlgorithm<TravelSearchState>.SearchPath;
    using HeuristicMethodType = BaseSearchAlgorithm<TravelSearchState>.HuristicMethodType;
    

    public class AStartAgent:SearchAlgoAgent
    {
        Path<ActionType> _path = null;
        public AStartAgent(HeuristicMethodType h, int startLocation, int target) : base(h, startLocation, target, new AStarAlgorithm<TravelSearchState>()) { }

        public override string Name
        {
            get { return "A* agent"; }
        }

        protected override ActionType InnerGetNextAction(TravelWorld world)
        {
             if (_path == null || _path.Count == 0)
                    _path = GetNewPath(world);
             return _path.TakeNextStep(); 
        }        
    }
    
}
