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
    using SearchPath = BaseSearchAlgorithm<TravelSearchState>.SearchPath;
    using HeuristicMethodType = BaseSearchAlgorithm<TravelSearchState>.HuristicMethodType;

    public abstract class SearchAlgoAgent : BaseGoalAgent
    {        
        BaseSearchAlgorithm<TravelSearchState> _algo;        

        public SearchAlgoAgent(HeuristicMethodType h, int startLocation,int target,BaseSearchAlgorithm<TravelSearchState> algo) : base(h, startLocation,target) 
        {                    
            _algo = algo;
        }
                
        protected Path<ActionType> GetNewPath(TravelWorld world)
        {
            var path = _algo.Run(ToSearchState(world), this.getActions, this.GetGoalFunc(Goal), _heuristic);
            Expanstions += _algo.Expansations;
            if (path == null || path.Count == 0)
                return null;

            return ConvertToActionsPath(path);            
        }  

        public override string Name
        {
            get { return "genral serach agent"; }
        }
    }
}
