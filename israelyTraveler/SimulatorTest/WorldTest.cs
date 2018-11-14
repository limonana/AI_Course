using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using World;
using System.Linq;

namespace SimulatorTest
{
    [TestClass]
    public class WorldTest
    {        
        [TestMethod]
        public void simpleWaterTest()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlace(1);
            world.PutWater(1);
            Assert.IsTrue(world.HaveWater(1));            
        }

        [TestMethod]
        public void TakeWaterTest()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlace(1);
            world.PutWater(1);
            bool res = world.TakeWater(1);
            Assert.IsTrue(res == true && !world.HaveWater(1));
        }

        [TestMethod]
        public void FireTest()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1,2);
            world.AddWay(1, 2, 1);
            world.SetFire(1,2);
            Assert.IsFalse(world.isClear(1,2));
        }

        [TestMethod]
        public void NoFireTest()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2,3);
            world.AddWay(1, 2, 1);
            world.AddWay(3, 2, 1);
            world.SetFire(2,3);
            Assert.IsTrue(world.isClear(1, 2));            
        }

        [TestMethod]
        public void SetFireTest()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2, 3);            
            world.AddWay(1, 3, 1);
            world.SetFire(1,3);
            world.StopFire(1,3);
            Assert.IsTrue(world.isClear(1, 3));
        }

        [TestMethod]
        public void clearWays()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2, 3);
            world.AddWay(1, 2, 1);
            world.AddWay(1, 3, 1);
            world.AddWay(3, 2, 1);
            world.SetFire(1,3);
            world.SetFire(2, 3);
            var ways = world.GetClearWays(1);
            Assert.AreEqual(ways.Count(),1);
            Assert.IsFalse(!world.isClear(ways.First()));            
        }

        [TestMethod]
        public void NoClearWays()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2, 3);            
            world.AddWay(1, 3, 1);            
            world.SetFire(1,3);
            var ways = world.GetClearWays(1);
            Assert.IsTrue(ways.Count() == 0);
        }

        [TestMethod]
        public void ClearPathTest()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2, 3,4,5);
            for (int i = 1; i <=4; i++)
			{
			    world.AddWay(i, i+1, 1);    
			}
            world.AddWay(5,1,1);
            world.SetFire(1,2);
            var path = world.ShortestClearPath(1, 3);
            var resPath = new TravelPath();
            resPath.Add(5,1);
            resPath.Add(4,1);
            resPath.Add(3,1);

            Assert.IsTrue(resPath.Equals(path));
        }

        [TestMethod]
        public void cheapestWaterPathWithoutFire1()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2, 3);
            world.AddWay(1, 2, 10);
            world.AddWay(2, 3, 5);
            world.AddWay(1, 3, 12);
            world.PutWater(1);

            var paths = world.findCheapestWaterPaths(2);
            if (paths.Count() != 1)
                Assert.Fail();
            var path = paths.First();
            var resPath = new TravelPath();
            resPath.Add(1, 10);
            Assert.AreEqual(path, resPath);
        }

        [TestMethod]
        public void cheapestWaterPathWithoutFire2()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2, 3,4);
            world.AddWay(1, 2, 10);
            world.AddWay(2, 3, 5);
            world.AddWay(4, 3, 12);
            world.PutWater(1,4);            

            var paths = world.findCheapestWaterPaths(2);
            if (paths.Count() != 1)
                Assert.Fail();
            var path = paths.First();
            var resPath = new TravelPath();
            resPath.Add(1, 10);
            Assert.AreEqual(path, resPath);
        }

        [TestMethod]
        public void cheapestWaterPathWithFire()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2, 3, 4);
            world.AddWay(1, 2, 10);
            world.AddWay(2, 3, 1);
            world.AddWay(4, 3, 1);
            world.PutWater(1, 4);
            world.SetFire(2,3);
            world.SetFire(4, 3);


            var paths = world.findCheapestWaterPaths(2);
            if (paths.Count() != 1)
                Assert.Fail();
            var path = paths.First();
            var resPath = new TravelPath();
            resPath.Add(1, 10);
            Assert.AreEqual(path, resPath);
        }

        [TestMethod]
        public void cheapestFirePathTest()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2, 3, 4);
            world.AddWay(1, 2, 10);
            world.AddWay(2, 3, 5);
            world.AddWay(4, 3, 5);                        
            world.SetFire(1,2);
            world.SetFire(4,3);
            var paths = world.findCheapestFirePaths(2).ToArray();
            if (paths.Count() != 2)
                Assert.Fail();

            var path1 = paths[0];
            var path2 = paths[1];
            var resPath1 = new TravelPath();
            resPath1.Add(1, 10);
            var resPath2 = new TravelPath();
            resPath2.Add(3, 5);
            resPath2.Add(4, 5);
            Assert.IsTrue(
                path1.Equals(resPath1) && path2.Equals(resPath2) ||
                path1.Equals(resPath2) && path2.Equals(resPath1));
        }

        [TestMethod]
        public void cheapestFirePathTest2()
        {
            TravelWorld world = new TravelWorld();
            world.AddPlaces(1, 2, 3, 4);
            world.AddWay(1, 2, 10);
            world.AddWay(2, 3, 5);
            world.AddWay(4, 3, 5);
            world.SetFire(1,2);            
            var paths = world.findCheapestFirePaths(2).ToArray();
            if (paths.Count() != 1)
                Assert.Fail();

            var path1 = paths.First();
            var resPath1 = new TravelPath();
            resPath1.Add(1,10);
            Assert.AreEqual(path1, resPath1);            
        }
    }
}
