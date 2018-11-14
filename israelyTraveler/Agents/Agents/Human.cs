using AIBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using World;
using Agents.Agents.GameAgents;

namespace Agents
{
    public class Human : UndoGameAgent
    {
        public Human(int startLocation) : base(startLocation) { }

        public override ActionType GetNextAction(TravelWorld currrWorld)
        {
            while (true)
            {
                Console.WriteLine("HUMAN:what is my next action?");
                string action = Console.ReadLine();

                if (action == "no-op")
                    return noOpertion;
                if (action == "pickup")
                    return pickupWater;
                if (action.StartsWith("start"))
                {
                    int place = int.Parse(action.Split(' ')[1]);
                    return new ActionType(world => startAfire(world, place));
                }
                if (action.StartsWith("drive"))
                {
                    int place = int.Parse(action.Split(' ')[1]);
                    return new ActionType(world => drive(world, place));
                }

                Console.WriteLine("not known command!");
            }
        }

        public override string Name
        {
            get { return "human"; }
        }
    }
}
