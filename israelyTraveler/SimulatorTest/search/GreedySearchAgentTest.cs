using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using World;
using AIBasic.Algorithms.Base;
using Agents.Agents.SearchAgents;
using System.Linq;

namespace SimulatorTest
{
    using HeuristicMethodType = BaseSearchAlgorithm<TravelSearchState>.HuristicMethodType;    

    [TestClass]
    public class GreedySearchAgentTest
    {
        [TestMethod]
        public void goToLowerPlaceTest()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2, 3, 4);
            world.AddWay(1, 2, 10);
            world.AddWay(3, 2, 1);
            world.AddWay(3, 4, 1);
            world.AddWay(2, 4, 10);            
            
            GreedySearchAgent agent = new GreedySearchAgent(goToLowerPlace, 4);
            var action = agent.GetNextAction(world);

            //check if the world is the same before doing an action
            Assert.AreEqual(world.GetGraph().VertexCount, 4);
            Assert.AreEqual(world.GetGraph().EdgeCount, 4);            

            //go to 2
            Assert.IsTrue(action(world));
            Assert.AreEqual(agent.CurrentLocation, 2);
            Assert.AreEqual(agent.TotalCost, world.getCostWay(4,2));
            double prevCost = agent.TotalCost;

            //go to 1
            action = agent.GetNextAction(world);
            Assert.IsTrue(action(world));
            Assert.AreEqual(agent.CurrentLocation, 1);
            Assert.AreEqual(agent.TotalCost, prevCost+ world.getCostWay(1, 2));
            prevCost = agent.TotalCost;

            world.SetFire(1, 2);
            //do nothing 
            action = agent.GetNextAction(world);
            Assert.IsTrue(action(world));
            Assert.AreEqual(agent.CurrentLocation, 1);            
            Assert.AreEqual(agent.TotalCost, prevCost + world.EpsilonCost);
        }

        public double goToLowerPlace(TravelSearchState state)
        {
            return state.CurrentLocation - 1;
        }


        [TestMethod]
        public void TakeWaterTest()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2, 3, 4);
            world.AddWay(1, 2, 10);
            world.AddWay(3, 2, 1);
            world.AddWay(3, 4, 1);
            world.AddWay(1, 4, 10);
            world.PutWater(1);
            world.PickupCost = 6;

            GreedySearchAgent agent = new GreedySearchAgent(takeWaterHuristic, 4);
            var action = agent.GetNextAction(world);

            //check if the world is the same before doing an action
            Assert.AreEqual(world.GetGraph().VertexCount, 4);
            Assert.AreEqual(world.GetGraph().EdgeCount, 4);
            Assert.AreEqual(world.GetWaterPlaces().Count(), 1);
            Assert.IsTrue(world.GetWaterPlaces().Contains(1));

            //go to 1
            Assert.IsTrue(action(world));
            Assert.AreEqual(agent.CurrentLocation, 1);
            Assert.AreEqual(agent.TotalCost, world.getCostWay(4, 1));
            double prevCost = agent.TotalCost;

            //pickup water
            action = agent.GetNextAction(world);
            Assert.IsTrue(action(world));
            Assert.AreEqual(agent.CurrentLocation, 1);
            Assert.IsTrue(agent.CarryWater);
            Assert.AreEqual(agent.TotalCost, prevCost + world.PickupCost);            
        }

        public double takeWaterHuristic(TravelSearchState state)
        {            
            if (state.CarryWatter)
                return 0;
            TravelWorld world = state.ToWorld();
            if (world.HaveWater(state.CurrentLocation))
                return 1;

            var paths = world.findCheapestWaterPaths(state.CurrentLocation);
            if (paths== null || paths.Count() == 0)
                return double.MaxValue;

            TravelPath path = paths.First();
            return path.Cost();

        }

        [TestMethod]
        public void GreedyTest1()
        {
            var tester = new GreedyTester();
            GreedySearchAgent agent = new GreedySearchAgent(greedyHuristic(4), 2);
            tester.GoToDest(agent);
        }

        [TestMethod]
        public void GreedyTest2()
        {
            var tester = new GreedyTester();
            GreedySearchAgent agent = new GreedySearchAgent(greedyHuristic(4), 2);
            tester.GoToDestOnce(agent);
        } 

        protected HeuristicMethodType greedyHuristic(int targetLocation)
        {
            return new HeuristicMethodType(state => 
                {
                    if (targetLocation == state.CurrentLocation)
                        return 0;

                    var path = state.ToWorld().ShortestClearPath(state.CurrentLocation, targetLocation);                    
                    if (path == null)
                        return double.MaxValue;
                    else
                         return path.Count; 
                });
        }

        [TestMethod]
        public void MainUristicCheck()
        {
            //create a world where the least cost is to pickup water and go throgh a fire
            TravelWorld world = new TravelWorld();            
            world.AddPlaces(1, 2, 3, 4);
            world.AddWay(1, 2, 1);
            world.AddWay(1, 3, 1);
            world.AddWay(3, 2, 1);
            world.AddWay(3, 4, 1);            
            world.PutWater(1);
            world.SetFire(1, 2);
            world.PickupCost = 10;
            int goal = 4;
            //the path should be 1->3->4

            GreedySearchAgent agnet = new GreedySearchAgent(new OneFireHuristic(goal).Run, 1);
                        
            Assert.IsTrue(agnet.GetNextAction(world)(world));
            Assert.AreEqual(3, agnet.CurrentLocation);

            Assert.IsTrue(agnet.GetNextAction(world)(world));
            Assert.AreEqual(4, agnet.CurrentLocation);           
        }

    }
}
