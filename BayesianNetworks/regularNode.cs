using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bayas
{
    class regularNode : basicNode
    {
        DataTable ProbabiltyTable;

        public void CreateProbabiltyTable()
        {
            ProbabiltyTable = new DataTable();
            ProbabiltyTable.Columns.Add("Val");            
            ProbabiltyTable.Columns.Add("P");
            ProbabiltyTable.Columns["P"].DataType = typeof(double);
            foreach (var parent in ParentsNames)
            {
                ProbabiltyTable.Columns.Add(parent);                
            }
        }

        public void AddProbabiltyRow(object val, double p, Dictionary<String, object> parents)
        {
            var row = ProbabiltyTable.NewRow();
            row["P"] = p;
            row["Val"] = val;
            foreach (var item in parents)
            {
                row[item.Key] = item.Value;
            }
            ProbabiltyTable.Rows.Add(row);
        }

        public override double GetProbabilityByParents(object val, Dictionary<string, object> evidance)
        {
            var chosenRow = ProbabiltyTable.AsEnumerable().Single(row => row["Val"].Equals(val) && checkEvidance(row, evidance));
            return (double)chosenRow["P"];
        }

        private bool checkEvidance(DataRow row, Dictionary<string, object> evidance)
        {
            foreach (var item in evidance)
            {
                if (ProbabiltyTable.Columns.Contains(item.Key) && !row[item.Key].Equals(item.Value))
                    return false;
            }
            return true;
        }
    }
}
