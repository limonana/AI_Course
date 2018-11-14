using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleSimulator;
using World;
using Agents;
using AIBasic;
using System.Collections.Generic;
using System.IO;

namespace SimulatorTest
{
    [TestClass]
    public class SimulatorTest
    {        
        [TestMethod]
        public void TestMethod1()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2, 3,4,5,6,7,8,9);
            world.AddWay(1, 2, 1);
            world.AddWay(3, 2, 1);
            world.AddWay(3, 9, 1);
            world.AddWay(8, 9, 10);
            world.AddWay(8, 7, 10);
            world.AddWay(6, 7, 10);
            world.AddWay(6, 5, 10);
            world.AddWay(4, 5, 10);
            world.AddWay(4, 1, 10);
            world.SetFire(1, 2);
            world.PutWater(2);

            var agents = new List<BaseAgent<bool,TravelWorld>>();
            
            Simulator<TravelWorld, bool> sim = new Simulator<TravelWorld, bool>(world,
                new FireFighter(3), new Greedy(1, 3));
            Console.SetOut(new StreamWriter("run.txt"));                
            sim.Run(50);            
        }
    }
}
