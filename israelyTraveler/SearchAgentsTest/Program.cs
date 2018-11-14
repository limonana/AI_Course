using Agents.Agents.SearchAgents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World;

namespace SearchAgentsTest
{
    class Program
    {
        static void Main(string[] args)
        {            
            Console.WriteLine("enter the world describtion file:");
            string pathFile = Console.ReadLine();
            var world = ConsoleSimulator.Program.CreateWorld(pathFile);
            // Simulator<TravelWorld, bool> simulator = new 
            //   Simulator<TravelWorld, bool>(world, CreateAgents().ToArray());
            //simulator.Run(50);

            CompareSearchAgents(world);
            Console.ReadLine();     
        }

        private static void CompareSearchAgents(TravelWorld world)
        {
            #region create agents
            Console.WriteLine("what is the goal place of search agents?");
            int goal = int.Parse(Console.ReadLine());
            Console.WriteLine("what is the start place of search agents?");
            int start = int.Parse(Console.ReadLine());
            Console.WriteLine("what is the limit of real time search agent?");
            int limit = int.Parse(Console.ReadLine());
            var h = new OneFireHuristic(goal);
            List<BaseSearchAgent> agents = new List<BaseSearchAgent>();
            agents.Add(new AStartAgent(h.Run, start, goal));
            agents.Add(new GreedySearchAgent(h.Run, start));
            agents.Add(new RealTimeAStarAgent(h.Run, start, goal, limit));
            #endregion

            foreach (var agent in agents)
            {
                TravelWorld worldCpy = world.Clone() as TravelWorld;
                while (agent.CurrentLocation != goal)
                {
                    agent.GetNextAction(world)(worldCpy);
                }
            }
            int[] fValues = { 1, 100, 10000 };
            foreach (var f in fValues)
            {
                Console.WriteLine("f={0}", f);
                foreach (var agent in agents)
                    Console.WriteLine("{3}: S={0},T={1},P={2}", agent.TotalCost, agent.Expanstions, f * agent.TotalCost + agent.Expanstions, agent.Name);
            }
        }
    }
}
