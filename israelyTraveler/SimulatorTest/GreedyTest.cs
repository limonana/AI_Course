using Agents;
using Agents.World;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World;

namespace SimulatorTest
{    
    [TestClass]
    public class GreedyTester
    {
        [TestMethod]
        public  void noOperation()
        {
            Greedy agent = new Greedy(2, 1);
            noOperation(agent);
        }

        public void noOperation(BaseTraveler agent)
        {            
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1,2,3,4);
            world.AddWay(1,2,1);
            world.AddWay(3,2,1);
            world.AddWay(3,4,1);
            world.SetFire(1,2);
            double prevCost = agent.TotalCost;
            (agent.GetNextAction(world)).Invoke(world);
            Assert.AreEqual(agent.TotalCost - prevCost, Costs.Instance.Epsilon);
            Assert.AreEqual(agent.CurrentLocation, 2);            
        }

        [TestMethod]
        public void GoToDestOnce()
        {
            Greedy agent = new Greedy(2, 4);
            GoToDestOnce(agent);
        }

        public void GoToDestOnce(BaseTraveler agent)
        {            
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2, 3, 4);
            world.AddWay(1, 2, 1);
            world.AddWay(3, 2, 1);
            world.AddWay(3, 4, 1);
            world.AddWay(2, 4, 20);
            
            double prevCost = agent.TotalCost;
            Assert.IsTrue((agent.GetNextAction(world)).Invoke(world));
            Assert.AreEqual(agent.TotalCost - prevCost, 20);
            Assert.AreEqual(agent.CurrentLocation, 4);
        }

        [TestMethod]
        public void GoToDest()
        {
            Greedy agent = new Greedy(2, 4);
            GoToDest(agent);
        }

        public  void GoToDest(BaseTraveler agent)
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2, 3, 4);
            world.AddWay(1, 2, 1);
            world.AddWay(3, 2, 1);
            world.AddWay(3, 4, 1);                        
            Assert.IsTrue((agent.GetNextAction(world)).Invoke(world));            
            Assert.AreEqual(agent.CurrentLocation, 3);            
            Assert.IsTrue((agent.GetNextAction(world)).Invoke(world));            
            Assert.AreEqual(agent.CurrentLocation, 4);
        }
    }
}
