using Agents;
using AIBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World;

namespace ConsoleSimulator
{
    public class Simulator
    {
        IEnumerable<BaseTraveler> _agents;
        TravelWorld _world;
private BaseTraveler _human;
private BaseTraveler _gameAgent;

public Simulator(TravelWorld world, params BaseTraveler[] agents)
        {
            _world = world;
            _agents = agents;            
            _human = agents[0];
            _gameAgent = agents[1];            
        }

        public void Run(int iterations,bool sum0)
        {
            for (int i = 1; i<=iterations; i++)
            {
                Console.WriteLine("round {0}...", i);
                foreach (var agent in _agents)
                {
                    Console.WriteLine(agent.Name + ":");
                    var orgOut  = Console.Out;
                    Console.SetOut(new StringWriter());
                    var action = agent.GetNextAction(_world);                    
                    Console.SetOut(orgOut);
                    double prevCost = agent.TotalCost;
                    bool res = action.Invoke(_world);
                    if (sum0 && agent == _human)
                        _gameAgent.AddCost(-(agent.TotalCost - prevCost));

                    Console.WriteLine(agent);                    
                    Console.WriteLine(_world);
                }                
            }
            Console.WriteLine("game end:");
            foreach (var agent in _agents)
            {
                if (agent.CurrentLocation != agent.Goal.Value)
                    agent.AddCost(100);

                Console.WriteLine(agent);    
            }
            Console.WriteLine("press enter to continue");
            Console.ReadLine();


        }

    }
}
