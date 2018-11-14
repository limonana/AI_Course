using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace bayas
{
    public class Network
    {
        /*public void SetNumberNodes(int num)
        {
            //variables = new List<string>(num);
            name2Node = new Dictionary<string, basicNode>(num);
        }*/         

        //EnumerationAsk
        public Dictionary<object, double> Query(string QueryVar, Dictionary<string, object> evidance)
        {
            Dictionary<object, double> res = new Dictionary<object, double>();
            foreach (var val in name2Node[QueryVar].values)
	        {
                var newEvidance = ExtendEvidance(QueryVar, val, evidance);
                res.Add(val, EnumrateAll(name2Node.Keys, newEvidance));
	        }
            Normalize(ref res);
            return res;            
        }

        private Dictionary<string, object> ExtendEvidance(string QueryVar, object val, Dictionary<string, object> evidance)
        {
            Dictionary<string, object> newEvidance = new Dictionary<string, object>(evidance);
            newEvidance.Add(QueryVar, val);
            return newEvidance;
        }

        private double EnumrateAll(ICollection<string> vars, Dictionary<string, object> evidance)
        {
            if (vars.Count == 0) return 1;

            string Y = vars.First();
            var node = name2Node[Y];
            if (evidance.ContainsKey(Y))
                return node.GetProbabilityByParents(evidance[Y], evidance) * EnumrateAll(vars.Skip(1).ToList(), evidance);
            else
            {
                double sum = 0;
                foreach (var val in node.values)
                {
                    var newEvidance = ExtendEvidance(Y,val,evidance);
                    sum += node.GetProbabilityByParents(val, evidance) * EnumrateAll(vars.Skip(1).ToList(), newEvidance);
                }
                return sum;
            }
        }        

        private void Normalize(ref Dictionary<object, double> Q)
        {            
            var sum = Q.Values.Sum();
            var keys = Q.Keys.ToList();
            foreach (var item in keys)
            {
                Q[item] = Q[item] / sum;
            }
        }


        //List<String> variables;
        Dictionary<string, basicNode> name2Node = new Dictionary<string,basicNode>();

        public void AddNode(basicNode node)
        {
            //this.variables.Add(node.Name);
            this.name2Node.Add(node.Name,node);            
        }

    }   
}

