using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World;
using Agents.Agents.GameAgents;

namespace Agents
{
    public class Greedy :UndoGameAgent
    {       
        public Greedy(int startLocation,int targetPlace) : base(startLocation)
        {
            _targetPlace = targetPlace;
        }

        int _targetPlace;        
        public override ActionType GetNextAction(TravelWorld world)
        {
            if (CurrentLocation == _targetPlace)
                return noOpertion;

            TravelPath path = world.ShortestClearPath(CurrentLocation,_targetPlace);

            if (path == null || path.Count() == 0)
                return noOpertion;

            int nextPlace = path.First();            
            return new ActionType(w => drive(w, nextPlace));
        }

        public override string Name
        {
            get { return "greedy"; }
        }
    }
}
