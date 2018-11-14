using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBasic
{
    //use abstract class instead of interface so i can declare a delegate
    public abstract class BaseAgent<TReturnType,TWorld>
    {
        public delegate TReturnType ActionType(TWorld world);
        
        public  abstract ActionType GetNextAction(TWorld world);
        public abstract double TotalCost { get; }
        public abstract void AddCost(double cost);
        public abstract int NumOfActions { get; }
        public abstract string Name { get; }
    }
}
