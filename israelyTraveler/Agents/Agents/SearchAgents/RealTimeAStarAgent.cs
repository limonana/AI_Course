using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIBasic;
using AIBasic.Algorithms.Search;
using AIBasic.Algorithms.Base;

namespace Agents.Agents.SearchAgents
{
    using SearchPath = SearchTypes<TravelSearchState>.SearchPath;
    using HeuristicMethodType = SearchTypes<TravelSearchState>.HuristicMethodType;
    using World;

    public class RealTimeAStarAgent:SearchAlgoAgent
    {
        public RealTimeAStarAgent(HeuristicMethodType h, int startLocation, int target,int limit) : base(h, startLocation, target, new SimplyfiedRealTimeAStarAlgorithm<TravelSearchState>(limit)) { }

        public override string Name
        {
            get { return "RealTimeA* agent"; }
        }

        protected override ActionType InnerGetNextAction(TravelWorld world)
        {           
            var path = GetNewPath(world);
            return path.TakeNextStep();            
        }
    }


}
