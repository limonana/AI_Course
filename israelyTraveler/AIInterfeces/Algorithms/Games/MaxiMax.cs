using AIBasic.Algorithms.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBasic.Algorithms.Games
{
    //can't implement pruning
    public class MaxiMax<TGameState>:BaseGameAlgorithm<TGameState>
    {
        bool _cooperative;
        public MaxiMax(bool cooperative)
        {
            _cooperative = cooperative;
        }

        protected override ActionType Run(TGameState initialState)
        {     
            return maxAction(initialState);
        }

        private ActionType maxAction(TGameState initialState)
        {
            double max = double.MinValue;
            ActionType maxOperator = null;
            foreach (var op in _Operators1(initialState))
            {
                var res = op(initialState);
                var val = MaxValue(res.State,_Operators2,res.Cost);
                if (val >max)
                {
                    max = val;
                    maxOperator = op;
                }
            }
            return maxOperator;
        }

        private double MaxValue(TGameState state,OpertorsMethodType operatorFunc,double currentCost)
        {
            if (_cuffOFFunc(state))
                return _Eval(state) - currentCost;

            double max = double.MinValue;
            foreach (var action in operatorFunc(state))
            {
                var res = action(state);
                TGameState newState = res.State;
                double newCost = currentCost;
                if (_cooperative)
                    newCost += res.Cost;
                else
                {
                    if (operatorFunc == _Operators1)
                        newCost += res.Cost;
                }
                max = Math.Max(max, MaxValue(newState,switchOperators(operatorFunc),newCost));
            }

            return max;
        }

        private OpertorsMethodType switchOperators(OpertorsMethodType operatorFunc)
        {
            if (operatorFunc == _Operators1)
                return _Operators2;
            if (operatorFunc == _Operators2)
                return _Operators1;
            throw new Exception("operators can't be not one of two players");
        }     
    }
}
