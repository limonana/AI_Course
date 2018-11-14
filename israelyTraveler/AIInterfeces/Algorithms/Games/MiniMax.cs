using AIBasic.Algorithms.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBasic.Algorithms.Games
{
    //write MiniMax that consider costs .
    //the resular miniMax "assume" all cost are the same
    public class MiniMax<TGameState>:BaseGameAlgorithm<TGameState>
    {      
        protected override ActionType Run(TGameState initialState)
        {
            return MaxAction(initialState);
        }

        private ActionType MaxAction(TGameState initialState)
        {
            ActionType maxAction = null;
            double alpha = double.MinValue;
            double beta = double.MaxValue;
            foreach (var op in _Operators1(initialState))
            {
                var res = op(initialState);
                double val = MinValue(res.State, alpha, beta,res.Cost);
                if (val>alpha)
                {
                    alpha = val;
                    maxAction = op;
                }
            }

            return maxAction;
        }

        private double MaxValue(TGameState state, double alpha, double beta,double currentCost)
        {
            if (_cuffOFFunc(state))
                return _Eval(state)-currentCost;

            foreach (var action in _Operators1(state))
            {
                var res = action(state);
                alpha = Math.Max(alpha, MinValue(res.State, alpha, beta,res.Cost+currentCost));
                if (alpha >= beta)
                    return beta;
            }

            return alpha;
        }

        private double MinValue(TGameState state, double alpha, double beta,double currentCost)
        {
            if (_cuffOFFunc(state)) return _Eval(state)-currentCost;

            foreach (var action in _Operators2(state))
            {
                var res = action(state);
                beta = Math.Min(beta, MaxValue(res.State, alpha, beta,currentCost-res.Cost));
                if (beta <= alpha)
                    return alpha;
            }

            return beta;
        }      
    }
}
