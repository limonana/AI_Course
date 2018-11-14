using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using World;
using Agents;
using Agents.World;

namespace SimulatorTest
{
    [TestClass]    
    public class HumanTest
    {
        TravelWorld world;

        public HumanTest()
        {
            world = new TravelWorld();                        
            world.AddPlaces(1, 2, 3, 4);
            world.AddWay(1, 2, 1);      
            world.AddWay(3, 2, 1);      
            world.AddWay(3, 4, 1);
            world.PutWater(4);                    
        }

        [TestMethod]
        [TestCategory("aaa")]
        public void canWalkNoFire()
        {
            Human agent = new Human(1);            
            Assert.IsTrue(agent.drive(world, 2));
            Assert.AreEqual(agent.TotalCost, world.getCostWay(1, 2));
        }

        [TestMethod]
        public void canWalkwithFire()
        {
            Human agent = new Human(1);
            world.PutWater(1);
            world.SetFire(1,2);
            bool res = agent.pickupWater(world);
            bool res2= agent.drive(world, 2);
            Assert.IsTrue(res == true && res2 == true);
            Assert.AreEqual(agent.TotalCost, 2*world.getCostWay(1, 2) + agent.costs.Pickup);
        }

        [TestMethod]
        public void canNotWalk()
        {
            Human agent = new Human(1);
            world.SetFire(1,2);
            bool res = agent.drive(world, 2);
            Assert.IsFalse(res);
        }

        [TestMethod]
        public void WalkwithWater()
        {
            Human agent = new Human(1);
            world.PutWater(1);            
            bool res = agent.pickupWater(world);
            bool res2 = agent.drive(world, 2);
            Assert.IsTrue(res == true && res2 == true);
            Assert.AreEqual(agent.TotalCost, 2*world.getCostWay(1, 2) + Costs.Instance.Pickup);
        }  
    }
}
