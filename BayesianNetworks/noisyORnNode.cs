using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bayas
{
    class noisyORNode : basicNode
    {
        public noisyORNode()
        {
            values = new List<object>();
            values.Add("true");
            values.Add("false");
        }

        Dictionary<string, double> q = new Dictionary<string, double>();    

        public override double GetProbabilityByParents(object val, Dictionary<string, object> evidance)
        {
            var trueParents = evidance.Where(x => x.Value.Equals("true")).Select(x => x.Key);
            double multipie = calcMultipe(trueParents);
            if (val.Equals("true"))
                return 1 - multipie;
            else
                return multipie;
        }

        private double calcMultipe(IEnumerable<string> trueParents)
        {
            double multipe = 1;
            foreach (var item in q)
            {
                if (trueParents.Contains(item.Key))
                    multipe *= item.Value;
            }
            return multipe;
        }

        public void AddProbabilities(double[] p)
        {
            for (int i = 0; i < ParentsNames.Count; i++)
            {
                q.Add(ParentsNames[i], p[i]);
            }
        }
    }
}
