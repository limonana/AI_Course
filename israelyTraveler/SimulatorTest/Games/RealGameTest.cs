using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using World;
using Agents;
using Agents.World;
using Agents.Agents.GameAgents;
using Agents.Agents.SearchAgents;

namespace SimulatorTest.Games
{
    [TestClass]
    public class RealGameTest
    {
        NeutralAgent agent;
        [TestMethod]
        //Example 2
        public void RivalsGame()
        {
            TravelWorld world = CommonWorld();

            Human human = CommonHuman();

            var agent = new CompetetiveAgent(1, 3, human, 2, 10);
            agent.costs = CommonCosts();

            world.AddPlayer(human);
            world.AddPlayer(agent);

            //set fire to 2
            agent.GetNextAction(world)(world);
            Assert.AreEqual(2, agent.CurrentLocation);
            Assert.IsTrue(!world.isClear(2, 1));

            human.pickupWater(world);

            //set fire to 3
            agent.GetNextAction(world)(world);
            Assert.AreEqual(3, agent.CurrentLocation);
            Assert.IsTrue(!world.isClear(2, 3));

            human.drive(world, 3);
            
            Assert.AreEqual(3, agent.TotalCost-human.TotalCost);            

        }

        [TestMethod]
        //Example1
        public void CoperativeGame()
        {
            TravelWorld world = CommonWorld();           

            Human human = CommonHuman();
         
            var agent = new CoopertiveAgent(1, 3, human,2,10);
            agent.costs = CommonCosts();

            world.AddPlayer(human);
            world.AddPlayer(agent);

            //pickupwater
            agent.GetNextAction(world)(world);
            Assert.AreEqual(1, agent.CurrentLocation);
            Assert.IsTrue(!world.HaveWater(1));

            human.noOpertion(world);
            //drive to 3
            agent.GetNextAction(world)(world);
            Assert.AreEqual(3, agent.CurrentLocation);
            Assert.IsTrue(world.isClear(1, 3));

            human.drive(world, 3);                                                

            Assert.AreEqual(3, agent.CurrentLocation);
            Assert.AreEqual(3, agent.TotalCost);
            Assert.AreEqual(1.05, human.TotalCost);

        }

        private Costs CommonCosts()
        {
            Costs costs = new Costs();
            costs.Fire = 3;
            costs.Pickup = 1;
            return costs;
        }

        private Human CommonHuman()
        {
            Human human = new Human(1);
            human.costs = CommonCosts();            
            human.Goal = 3;
            return human;
        }

        private static TravelWorld CommonWorld()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2, 3);
            world.AddWay(1, 2, 10);           
            world.AddWay(1, 3, 1);
            world.AddWay(3, 2, 10);            
            world.SetFire(3, 1);            
            world.PutWater(1);
            return world;
        }

        [TestMethod]
        public void NaturalGame()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(2, 3, 4);
            world.AddWay(3, 2, 2);
            world.AddWay(3, 4, 2);
            world.AddWay(2, 4, 10);
            world.SetFire(2, 4);


            Human human = new Human(2);
            human.costs.Fire = 1;
            human.costs.Pickup = 5;
            human.Goal = 4;

            BasicCuttof cuttOf = new BasicCuttof();
            agent = new NeutralAgent(2, 4, human,3,10);
            agent.costs.Fire = 10;
            agent.costs.Pickup = 5;

            world.AddPlayer(human);
            world.AddPlayer(agent);

            //eval.SetParams(agent, human, 10);
            cuttOf.SetParams(agent, 3);

            agent.GetNextAction(world)(world);
            Assert.AreEqual(3, agent.CurrentLocation);
            Assert.IsTrue(world.isClear(2, 3));

            agent.GetNextAction(world)(world);
            Assert.AreEqual(4, agent.CurrentLocation);
            Assert.IsTrue(world.isClear(4, 3));

        }
    }
}
