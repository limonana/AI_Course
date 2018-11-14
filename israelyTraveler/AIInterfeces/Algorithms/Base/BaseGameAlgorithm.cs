
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBasic.Algorithms.Base
{
    public abstract class BaseGameAlgorithm<TGameState> : SearchTypes<TGameState>
    {
        protected GoalMethodType _cuffOFFunc;
        protected OpertorsMethodType _Operators1;
        protected OpertorsMethodType _Operators2;
        protected HuristicMethodType _Eval;

        public ActionType Run(TGameState initialState, OpertorsMethodType OpertorsFunc1, OpertorsMethodType OpertorsFunc2,
            GoalMethodType cuttFunc, HuristicMethodType evalFunc)
        {
            _cuffOFFunc = cuttFunc;
            _Operators1 = OpertorsFunc1;
            _Operators2 = OpertorsFunc2;
            _Eval = evalFunc;
            return Run(initialState);

        }

        protected abstract ActionType Run(TGameState initialState);        


    }
}
