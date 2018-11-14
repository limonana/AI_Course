using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph.Algorithms;
using System.Text.RegularExpressions;
using World;
using AIBasic;
using Agents;
using System.IO;
using Agents.Agents.SearchAgents;
using Agents.World;
using Agents.Agents.GameAgents;

namespace ConsoleSimulator
{
    public class Program
    {
        private static int _humanStartLocation;
        private static int _humanGoal;
        private static int _humanPickup;
        private static int _humanStartFire;
        private static int _gameStartLocation;
        private static int _gameGoal;
        private static int _gamePickup;
        private static int _GameStartFire;
        private static int _horizon;
        static void Main(string[] args)
        {
            Console.WriteLine("enter the world describtion file:");
            string pathFile = Console.ReadLine();
            GetPlayersSettings();

            Console.WriteLine("cooperative Game");
            var world = CreateWorld(pathFile);            
            var players = CooperativePlayers();
            foreach (var item in players)
	        {
		         world.AddPlayer(item);
	        }
            Simulator simulator = new
                Simulator(world,players );
            simulator.Run(_horizon * 2,false);

            
            Console.WriteLine("rival Game");
            world = CreateWorld(pathFile);
            players = RivalPlayers();
            foreach (var item in players)
            {
                world.AddPlayer(item);
            }            
            simulator = new
                Simulator(world, players);
            simulator.Run(_horizon*2,true);

            Console.WriteLine("Natural Game");
            world = CreateWorld(pathFile);
            players = NaturalPlayers();
            foreach (var item in players)
            {
                world.AddPlayer(item);
            }            
            simulator = new
                Simulator(world, players);
            simulator.Run(_horizon * 2,false);

            
            Console.ReadLine();            
        }

        private static BaseTraveler[] RivalPlayers()
        {
            Human human = new Human(_humanStartLocation);
            human.Goal = _humanGoal;
            human.costs.Pickup = _humanPickup;
            human.costs.Fire = _humanStartFire;

            CompetetiveAgent agent = new CompetetiveAgent(_gameStartLocation, _gameGoal, human, 2, _horizon);
            agent.costs.Fire = _GameStartFire;
            agent.costs.Pickup = _gamePickup;
            return new BaseTraveler[] { human, agent };
        }

        private static BaseTraveler[] NaturalPlayers()
        {
            Human human = new Human(_humanStartLocation);
            human.Goal = _humanGoal;
            human.costs.Pickup = _humanPickup;
            human.costs.Fire = _humanStartFire;

            NeutralAgent agent = new NeutralAgent(_gameStartLocation, _gameGoal, human, 2, _horizon);
            agent.costs.Fire = _GameStartFire;
            agent.costs.Pickup = _gamePickup;
            return new BaseTraveler[] { human, agent };
        }

        private static BaseTraveler[] CooperativePlayers()
        {
            Human human = new Human(_humanStartLocation);
            human.Goal = _humanGoal;
            human.costs.Pickup = _humanPickup;
            human.costs.Fire = _humanStartFire;

            CoopertiveAgent agent = new CoopertiveAgent(_gameStartLocation, _gameGoal, human, 2, _horizon);
            agent.costs.Fire = _GameStartFire;
            agent.costs.Pickup = _gamePickup;
            return new BaseTraveler[]{ human, agent };

        }

        private static void GetPlayersSettings()
        {
            Console.WriteLine("human:");
            Console.Write("what is the start location?");
            _humanStartLocation = int.Parse(Console.ReadLine());
            Console.Write("what is the goal?");
            _humanGoal = int.Parse(Console.ReadLine());

            Console.Write("what is the pickup cost?");
            _humanPickup = int.Parse(Console.ReadLine());

            Console.Write("what is the start fire cost?");
            _humanStartFire = int.Parse(Console.ReadLine());

            Console.WriteLine("game agent:");
            Console.Write("what is the start location?");
            _gameStartLocation = int.Parse(Console.ReadLine());
            Console.Write("what is the goal?");
            _gameGoal = int.Parse(Console.ReadLine());

            Console.Write("what is the pickup cost?");
            _gamePickup = int.Parse(Console.ReadLine());

            Console.Write("what is the start fire cost?");
            _GameStartFire = int.Parse(Console.ReadLine());

            Console.WriteLine("horizon? (max moves)");
            _horizon = int.Parse(Console.ReadLine());
        }


        public static TravelWorld CreateWorld(string fileName)
        {
            List<string> lines = File.ReadLines(fileName).ToList();
            if (!lines[0].StartsWith("#V"))
                throw new Exception("file not in correct format");
            TravelWorld world = new TravelWorld();
            //get number of vertices
            int amount = int.Parse(lines[0].Split(' ')[1]);
            int[] places = new int[amount];
            for (int i = 0; i < amount; i++)
            {
                places[i] = i + 1;
            }
            world.AddPlaces(places);
            for (int i = 1; i < lines.Count; i++)
            {
                string line = lines[i];
                string[] param = line.Split(' ');
                if (line.StartsWith("#E"))
                {
                    bool clear = param[4] == "C";

                    double weight = 0;
                    if (param[3].StartsWith("W"))
                        weight = int.Parse(param[3][1].ToString());
                    world.AddWay(int.Parse(param[1]), int.Parse(param[2]), weight, clear);
                }

                if (line.StartsWith("#W"))
                {
                    world.PutWater(int.Parse(param[1]));
                }
            }

            //TODO: move to agents
            //Console.Write("enter Cost of pick up:");
            //Costs.Instance.Pickup= int.Parse(Console.ReadLine());
            return world;
        }
    }
}
