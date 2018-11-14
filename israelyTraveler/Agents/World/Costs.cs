using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.World
{
    public class Costs
    {
        public double Pickup { get; set; }
        public double Fire { get; set; }
        public double Epsilon { get {return 0.05;}}
    }
}
