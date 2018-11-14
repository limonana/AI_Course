using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBasic.Algorithms.Base
{
    public abstract class BaseSearchAlgorithm<TSearchState> : SearchTypes<TSearchState>
    {                
        public int Expansations { get; protected set; }
        public BaseSearchAlgorithm() { }

        public abstract SearchPath Run(TSearchState initialState, OpertorsMethodType releventOpertorsFunc,
            GoalMethodType goalFunc,HuristicMethodType huristicFunc);        
    }
}
