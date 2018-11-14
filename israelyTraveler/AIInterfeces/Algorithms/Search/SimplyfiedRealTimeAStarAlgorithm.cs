using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBasic.Algorithms.Search
{
    public class SimplyfiedRealTimeAStarAlgorithm<TSeachState>:AStarAlgorithm<TSeachState>
    {
        int _limit;
        public SimplyfiedRealTimeAStarAlgorithm(int limit)
        {
            _limit = limit;
        }
        public override SearchPath Run(TSeachState initialState, OpertorsMethodType releventOpertorsFunc, GoalMethodType goalFunc, HuristicMethodType huristicFunc)
        {
            var path = base.Run(initialState, releventOpertorsFunc, goalFunc, huristicFunc);
            if (path == null)
            {
                var res = openSet.DequeueItem();
                return reconstruct_path(res);
            }
            else
                return path;
        }

        protected override bool toStop()
        {
            return Expansations == _limit;
        }
    }
}
