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
    public class PyromaniacTest
    {
        TravelWorld world = new TravelWorld();
        public PyromaniacTest()
        {
            world.AddPlaces(1, 2, 3, 4);
            world.AddWay(1, 2,1);
            world.AddWay(2, 3,2);
            world.AddWay(4, 3,3);
        }

        [TestMethod]        
        public void stuck1()
        {
            world.SetFire(1, 2);
            Pyromaniac agent = new Pyromaniac(1, 1);
            for (int i = 0; i < 20; i++)
            {
                double lastCost = agent.TotalCost;
                (agent.GetNextAction(world)).Invoke(world);
                Assert.AreEqual(agent.CurrentLocation, 1);                
            }
            Assert.AreEqual(agent.TotalCost, agent.costs.Epsilon* 20);
        }

        [TestMethod]
        public void stuck2()
        {
            world.SetFire(1, 2);
            Pyromaniac agent = new Pyromaniac(2, 1);
            for (int i = 0; i < 20; i++)
            {
                double lastCost = agent.TotalCost;
                (agent.GetNextAction(world)).Invoke(world);
                Assert.AreEqual(agent.CurrentLocation, 1);
            }
            Assert.AreEqual(agent.TotalCost, Costs.Instance.Epsilon* 10);
        }

        [TestMethod]
        public void setFireOnce()
        {
            Pyromaniac agent = new Pyromaniac(1, 1);
            //set fire to (1,2)
            (agent.GetNextAction(world)).Invoke(world);
            Assert.IsFalse(world.isClear(1, 2));
            Assert.AreEqual(agent.CurrentLocation, 2);
            Assert.AreEqual(agent.TotalCost, 0);
        }

        [TestMethod]
        public void setFireTwice()
        {
            Pyromaniac agent = new Pyromaniac(1, 1);
            //set fire to (1,2)
            (agent.GetNextAction(world)).Invoke(world);
            Assert.IsFalse(world.isClear(1, 2));
            Assert.AreEqual(agent.CurrentLocation, 2);
            Assert.AreEqual(agent.TotalCost, 0);

            //set fire to (2,3)
            (agent.GetNextAction(world)).Invoke(world);
            Assert.IsFalse(world.isClear(2, 3));
            Assert.AreEqual(agent.CurrentLocation, 3);
            Assert.AreEqual(agent.TotalCost, 0);
        }




    }
}
