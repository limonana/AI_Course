using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World
{    
    public class TravelEdge : QuickGraph.Edge<int>
    {
        public TravelEdge(int souce, int target) : base(souce, target) {}                                

        public int getAnotherPlace(int place)
        {
            if (Source == place)
                return Target;
            else
                return Source;
        }        
    }
}
