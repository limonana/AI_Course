using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bayas
{
    class Program
    {
        static void Main(string[] args)
        {
            Network network = CreateNetwork("Data.txt");
            Dictionary<string, object> evidance = new Dictionary<string, object>();
            while (true)
            {
                Console.WriteLine("waiting for command:");
                string command = Console.ReadLine();
                if (command.StartsWith("quit")) break;
                if (command.StartsWith("add"))
                {
                    var parts = command.Split(' ');
                    evidance.Add(parts[1], parts[2]);
                }
                
                if (command.StartsWith("reset"))
                {
                    evidance.Clear();
                }
                if (command.StartsWith("query"))
                {
                    var parts = command.Split(' ');
                    var res = network.Query(parts[1], evidance);
                    foreach (var item in res)
                    {
                        Console.WriteLine("{0}={1}: {2}", parts[1], item.Key, item.Value);
                    }
                }
            }
        }

        private static Network CreateNetwork(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            int n = int.Parse(lines[0].Split(' ')[1]);
            lines = lines.Skip(1).ToArray();
            Network network = new Network();
            for (int i = 0; i < n; i++)
            {
                checkValitidy(lines);
                var parts = lines[0].Split(' ');
                string type = parts.Last();
                string name = parts[1];
                parts = lines[1].Split(' ');
                List<string> parents = parts.Skip(1).ToList();

                basicNode node = null;
                if (type == "boolean")
                {
                    node = new regularNode();
                    node.values = new List<object> { "true", "false" };

                }
                if (type == "multie")
                {
                    node = new regularNode();
                    parts = lines[2].Split(' ');
                    node.values = parts.Skip(1).Select(x => (object)x).ToList();
                }
                if (type == "noisyOR")
                {
                    node = new noisyORNode();
                    node.values = new List<object> { "true", "false" };
                }

                node.Name = name;
                node.ParentsNames = parents;
                network.AddNode(node);

                if (type == "noisyOR")
                {
                    var disturbution = lines[3].Split(' ').Skip(1).Select(x => double.Parse(x));
                    ((noisyORNode)node).AddProbabilities(disturbution.ToArray());
                    lines = lines.Skip(5).ToArray();
                }
                else
                {
                    int distLines = int.Parse(lines[3].Split(' ')[1]);
                    ((regularNode)node).CreateProbabiltyTable();
                    for (int j = 0; j < distLines; j++)
                    {
                        parts = lines[4 + j].Split(' ');
                        if (type == "boolean")
                        {
                            double p = double.Parse(parts.Last());
                            Dictionary<string, object> parentsVals = getParentsVals(parts.Take(parts.Length - 1));
                            ((regularNode)node).AddProbabiltyRow("true", p, parentsVals);
                            ((regularNode)node).AddProbabiltyRow("false", 1 - p, parentsVals);
                        }
                        if (type == "multie")
                        {
                            double p = double.Parse(parts.Last());
                            Dictionary<string, object> parentsVals = getParentsVals(parts.Take(parts.Length - 2));
                            ((regularNode)node).AddProbabiltyRow(parts[parts.Length - 2], p, parentsVals);
                        }
                    }

                    lines = lines.Skip(4 + distLines + 1).ToArray();
                }

            }
            return network;
        }

        private static void checkValitidy(string[] lines)
        {
            bool ok = lines[0].StartsWith("NODE");
            ok = ok && lines[1].StartsWith("PARENTS");
            ok = ok && (lines[2].StartsWith("VALUES") || lines[2].StartsWith("boolean"));
            ok = ok && lines[3].StartsWith("DISTRIBUTION");
            if (!ok)
                throw new Exception("file not in correct format");
        }

        private static Dictionary<string, object> getParentsVals(IEnumerable<string> parents)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            foreach (var item in parents)
            {
                string[] parts = item.Split('=');
                res.Add(parts[0], parts[1]);
            }
            return res;
        }
    }
}
