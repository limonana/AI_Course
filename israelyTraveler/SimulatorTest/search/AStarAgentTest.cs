using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using World;
using Agents.Agents.SearchAgents;
using Agents.Agents.Search;

namespace SimulatorTest
{
    [TestClass]
    public class AStarAgentTest : GreedySearchAgentTest
    {
        [TestMethod]
        public void greedyTest()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(2, 3, 4);            
            world.AddWay(3, 2, 1);
            world.AddWay(3, 4, 1);
            world.AddWay(2, 4, 10);
            AStartAgent agent = new AStartAgent(greedyHuristic(4), 2, 4);
            
            //chose the long path because  its cost is mush lower even the huristic 
            //is for chosing the shortest path.

            //go to 3
            Assert.IsTrue(agent.GetNextAction(world)(world));
            Assert.AreEqual(3,agent.CurrentLocation);

            //go to 4
            Assert.IsTrue(agent.GetNextAction(world)(world));
            Assert.AreEqual(agent.CurrentLocation, 4);
        }

        [TestMethod]
        public void MainUristicCheckAstar()
        {
            //create a world where the least cost is to pickup water and go throgh a fire
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2, 3, 4);
            world.AddWay(1, 2, 1);
            world.AddWay(3, 2, 1);
            world.AddWay(3, 4, 1);
            world.AddWay(2, 4, 100);
            world.PutWater(1);
            world.SetFire(3, 4);
            world.PickupCost = 1;
            int goal = 4;
            //the path should be 2->1->pickup->2->3->4 

            AStartAgent agnet = new AStartAgent(new OneFireHuristic(goal).Run, 2,goal);

            Assert.IsTrue(agnet.GetNextAction(world)(world));
            Assert.AreEqual(1, agnet.CurrentLocation);

            Assert.IsTrue(agnet.GetNextAction(world)(world));
            Assert.AreEqual(1, agnet.CurrentLocation);

            Assert.IsTrue(agnet.GetNextAction(world)(world));
            Assert.AreEqual(2, agnet.CurrentLocation);

            Assert.IsTrue(agnet.GetNextAction(world)(world));
            Assert.AreEqual(3, agnet.CurrentLocation);

            Assert.IsTrue(agnet.GetNextAction(world)(world));
            Assert.AreEqual(4, agnet.CurrentLocation);
        }
    }
}
