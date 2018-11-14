using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using World;
using Agents.Agents.SearchAgents;

namespace SimulatorTest
{
    [TestClass]
    public class realTimeAStarAgentTest
    {        

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

            RealTimeAStarAgent agnet = new RealTimeAStarAgent(new OneFireHuristic(goal).Run, 2, goal,3);

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
        [TestMethod]
        public void notFollwingOrgPath()
        {                    
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
            //the path should be 2->1->2->3->4 

            RealTimeAStarAgent agnet = new RealTimeAStarAgent(new OneFireHuristic(goal).Run, 2, goal, 3);
            Assert.IsTrue(agnet.GetNextAction(world)(world));
            Assert.AreEqual(1, agnet.CurrentLocation);

            world.StopFire(3, 4);

            Assert.IsTrue(agnet.GetNextAction(world)(world));
            Assert.AreEqual(2, agnet.CurrentLocation);

            Assert.IsTrue(agnet.GetNextAction(world)(world));
            Assert.AreEqual(3, agnet.CurrentLocation);

            Assert.IsTrue(agnet.GetNextAction(world)(world));
            Assert.AreEqual(4, agnet.CurrentLocation);

        }
    }
}
