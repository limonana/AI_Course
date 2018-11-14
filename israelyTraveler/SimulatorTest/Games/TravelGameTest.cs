using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using World;
using Agents;
using Agents.Agents.GameAgents;
using Agents.Agents.SearchAgents;
using Agents.World;

namespace SimulatorTest
{
    [TestClass]
    public class TravelGameTest
    {
        BaseTraveler agent;
        
        [TestMethod]
        public void basicGame1()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(2, 3, 4);
            world.AddWay(3, 2, 1);
            world.AddWay(3, 4, 1);
            world.AddWay(2, 4, 10);                    
            
            Human human = new Human(2);
            human.costs.Fire = 20;
            human.costs.Pickup = 5;
            human.Goal = 4;
                 
            agent = new CoopertiveAgent(2, 4, human,2,10);
            agent.costs.Fire = 20;
            agent.costs.Pickup = 5;

            world.AddPlayer(human);
            world.AddPlayer(agent);

            agent.GetNextAction(world)(world);
            Assert.AreEqual(3, agent.CurrentLocation);

            agent.GetNextAction(world)(world);
            Assert.AreEqual(4, agent.CurrentLocation);
        }

        [TestMethod]
        public void basicGame2()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(2, 3, 4);
            world.AddWay(3, 2, 1);
            world.AddWay(3, 4, 1);
            world.AddWay(2, 4, 10);            

            Human human = new Human(2);
            human.costs.Pickup = 2;
            human.costs.Fire = 6;

            agent = new CompetetiveAgent(2, 4, human,3,10);
            agent.costs.Pickup = 2;
            agent.costs.Fire = 6;

            world.AddPlayer(human);
            world.AddPlayer(agent);

            agent.GetNextAction(world)(world);
            Assert.AreEqual(3, agent.CurrentLocation);

            agent.GetNextAction(world)(world);
            Assert.AreEqual(4, agent.CurrentLocation);
        }

        private bool basicCutOf(TravelGameState state)
        {
            return state.locations[agent] == 4 || state.totalMoves[agent] >=4;
        }
    }
}
