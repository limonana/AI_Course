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
    public class FireFigherTest
    {
        [TestMethod]
        public void noWater()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2, 3);
            world.AddWay(1, 2,2);
            world.AddWay(3, 2, 2);
            world.SetFire(1, 2);
            FireFighter agent = new FireFighter(1);
            (agent.GetNextAction(world)).Invoke(world);
            Assert.AreEqual(agent.CurrentLocation, 1);
            Assert.AreEqual(agent.TotalCost, agent.costs.Epsilon);
        }

        [TestMethod]
        public void HaveWaterHere()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2, 3);
            world.AddWay(1, 2, 2);
            world.AddWay(3, 2, 2);
            world.PutWater(1);
            world.PutWater(3);
            FireFighter agent = new FireFighter(1);
            (agent.GetNextAction(world)).Invoke(world);
            Assert.AreEqual(agent.CurrentLocation, 1);
            Assert.AreEqual(agent.TotalCost, Costs.Instance.Pickup);
            Assert.IsFalse(world.HaveWater(1));
            Assert.IsTrue(world.HaveWater(3));
        }

        [TestMethod]
        public void HaveWaterFar()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2, 3);
            world.AddWay(1, 2, 2);
            world.AddWay(3, 2, 2);
            world.AddWay(3, 1, 10);            
            world.PutWater(3);
            FireFighter agent = new FireFighter(1);
            //go to place 2
            (agent.GetNextAction(world)).Invoke(world);
            Assert.AreEqual(agent.CurrentLocation, 2);
            Assert.AreEqual(agent.TotalCost, world.getCostWay(1,2));
            double lastCost = agent.TotalCost;
            //go to place 3
            (agent.GetNextAction(world)).Invoke(world);
            Assert.AreEqual(agent.CurrentLocation, 3);
            Assert.AreEqual(agent.TotalCost-lastCost, world.getCostWay(3, 2));
            lastCost = agent.TotalCost;
            //pickwater
            (agent.GetNextAction(world)).Invoke(world);
            Assert.AreEqual(agent.CurrentLocation, 3);
            Assert.AreEqual(agent.TotalCost - lastCost, Costs.Instance.Pickup);
            Assert.IsFalse(world.HaveWater(3));            
        }

        [TestMethod]
        public void stopFire()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2, 3);
            world.AddWay(1, 2, 2);
            world.AddWay(3, 2, 2);
            world.AddWay(3, 1, 10);
            world.SetFire(2,3);
            world.PutWater(2);

            FireFighter agent = new FireFighter(1);
            //go to the place with water
            (agent.GetNextAction(world))(world);
            Assert.AreEqual(agent.CurrentLocation, 2);
            Assert.AreEqual(agent.TotalCost, 2);
            double lastCost = agent.TotalCost;
            //PickWater
            (agent.GetNextAction(world)).Invoke(world);
            Assert.AreEqual(agent.CurrentLocation, 2);
            Assert.AreEqual(agent.TotalCost - lastCost, Costs.Instance.Pickup);
            Assert.IsFalse(world.HaveWater(2));
            lastCost = agent.TotalCost;

            //stop fire
            (agent.GetNextAction(world))(world);
            Assert.AreEqual(agent.CurrentLocation, 3);
            Assert.AreEqual(agent.TotalCost - lastCost, 2*world.getCostWay(2,3));
            Assert.IsFalse(agent.CarryWater);
            Assert.IsTrue(world.isClear(2, 3));
        }

    }
}
