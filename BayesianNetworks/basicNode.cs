using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bayas
{
    public abstract class basicNode
    {
        public string Name;        
        public List<object> values;
        public List<string> ParentsNames;
        

        

        public abstract double GetProbabilityByParents(object val, Dictionary<string, object> evidance);        
    }
}
