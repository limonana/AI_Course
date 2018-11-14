using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AIBasic.Algorithms.Games;
using AIBasic.Algorithms.Base;
namespace SimulatorTest
{
    using hType = SearchTypes<int>.HuristicMethodType;
    using goalType = SearchTypes<int>.GoalMethodType;
    using ActionsCollection = SearchTypes<int>.ActionsCollection;
    using ActionsType = SearchTypes<int>.ActionType;
    using StateAndCost = SearchTypes<int>.StateAndCost;
    [TestClass]
    public class BaseGameTests
    {

        double basicEval(int state)
        {
           return state;
        }

        bool basicCuttof(int state)
        {
            return (state >=4);
        }

        ActionsCollection miniMaxOps(int state)
        {
            var arr = new ActionsCollection();
            arr.Add(plus1);
            arr.Add(plus3);
            return arr;
        }

        ActionsCollection maxiMaxOp(int state)
        {
            var arr = new ActionsCollection();
            arr.Add(plus2);
            arr.Add(plus3);
            return arr;
        }

        StateAndCost plus1(int state)
        {

            return new StateAndCost(state + 1, 1);
        }

        StateAndCost plus2(int state)
        {

            return new StateAndCost(state + 2, 1);
        }
        
        
        StateAndCost plus3(int state)
        {
            return new StateAndCost(state +3, 1);
        }
       
        [TestMethod]
        public void basicTestMiniMax()
        {
            MiniMax<int> algo = new MiniMax<int>();
            ActionsType res = algo.Run(0,miniMaxOps, miniMaxOps, basicCuttof, basicEval);
            Assert.AreEqual("plus1", res.Method.Name);
        }

        [TestMethod]
        public void basicTestMaxiMax()
        {
            MiniMax<int> algo = new MiniMax<int>();
            ActionsType res = algo.Run(0, maxiMaxOp, maxiMaxOp, basicCuttof, basicEval);
            Assert.AreEqual("plus3", res.Method.Name);
        }     

        [TestMethod]
        public void differentCostMaxiMax()
        {
            MaxiMax<int> algo = new MaxiMax<int>(true);
            ActionsType res = algo.Run(1, OpsDifferentCosts, OpsDifferentCosts, basicCuttof, basicEval);
            Assert.AreEqual("plus3Cost3", res.Method.Name);
        }

        private ActionsCollection OpsDifferentCosts(int state)
        {
            var arr = new ActionsCollection();
            arr.Add(plus1cost2);
            arr.Add(plus3Cost3);
            return arr;
        }

        private StateAndCost plus3Cost3(int state)
        {
            return new StateAndCost(state + 3, 3);
        }

        private StateAndCost plus1cost2(int state)
        {
            return new StateAndCost(state + 1, 2);
        }
    }
}
