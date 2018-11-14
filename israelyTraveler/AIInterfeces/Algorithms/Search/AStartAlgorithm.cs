using AIBasic.Algorithms.Base;
using DataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBasic.Algorithms.Search
{
    public class AStarAlgorithm<TSearchState> : BaseSearchAlgorithm<TSearchState>
    {        
        Dictionary<TSearchState, SearchStateAndAction> came_from = new Dictionary<TSearchState, SearchStateAndAction>();
        Dictionary<TSearchState, double> g = new Dictionary<TSearchState, double>();
        Dictionary<TSearchState, double> f = new Dictionary<TSearchState, double>();

        protected PriorityQueue<double, TSearchState> openSet;

        //chose HashSet because need to do contains and add quickly
        HashSet<TSearchState> closedSet;

        private void init(TSearchState initialState)
        {                                                
            openSet = new PriorityQueue<double, TSearchState>();
            //dont matter because only one item in the queue
            openSet.Enqueue(0,initialState);

            closedSet = new HashSet<TSearchState>();
            f = new Dictionary<TSearchState, double>();
            g = new Dictionary<TSearchState, double>();
            g[initialState] = 0;
            came_from = new Dictionary<TSearchState, SearchStateAndAction>();
            Expansations = 0;
        }

        public override SearchPath Run(TSearchState initialState, OpertorsMethodType releventOpertorsFunc,
           GoalMethodType goalFunc, HuristicMethodType huristicFunc)
        {
            init(initialState);
            while (!openSet.IsEmpty && !toStop())
            {
                var current = openSet.DequeueItem();
                if (goalFunc(current))
                    return reconstruct_path(current);

                closedSet.Add(current);
                ++Expansations;
                var operators = releventOpertorsFunc(current);
                if (operators == null)
                    operators = new ActionsCollection();
                foreach (var opertor in operators)
                {
                    var neighboorStateAndCost = opertor(current);
                    var neighboor = neighboorStateAndCost.State;

                    //if the state exists at the closedSet it means  that is have a better f already
                    // so no need to calculate the huristic
                    if (closedSet.Contains(neighboor))
                        continue;
                    
                    double gValue = neighboorStateAndCost.Cost + g[current];
                    double fValue = gValue + huristicFunc(neighboor);
                    SearchStateAndAction stateAndAction = new SearchStateAndAction() { state = current, action = opertor };
                    HandleDuplicatesInOpenSet(neighboor,gValue,fValue,stateAndAction);
                }
            }
            return null;
        }

        protected virtual bool toStop()
        {
            return  false;
        }

        private void HandleDuplicatesInOpenSet(TSearchState state,double gValue,double fValue,SearchStateAndAction parent)
        {
            var duplicates = openSet.Where(x => x.Value.Equals(state));
            if (duplicates.Count() > 0)
            {
                //supposed to be only one duplicate because we check before any insertion
                var same_f = duplicates.First();
                var same = duplicates.First().Value;

                if (fValue < f[same])
                {
                    RemoveFromOpenSet(same_f);
                    AddToOpenSet(state, gValue, fValue, parent);                    
                }             
            }
            else
                AddToOpenSet(state, gValue, fValue, parent);
        }

        private void AddToOpenSet(TSearchState state, double gValue, double fValue, SearchStateAndAction parent)
        {
            f[state] = fValue;
            g[state] = gValue;
            came_from[state] = parent;

            openSet.Enqueue(f[state], state);            
        }

        private void RemoveFromOpenSet( KeyValuePair<double,TSearchState> cost_state)
        {
            openSet.Remove(cost_state);
            f.Remove(cost_state.Value);
            g.Remove(cost_state.Value);
            came_from.Remove(cost_state.Value);
        }

        protected SearchPath reconstruct_path(TSearchState current)
        {
            SearchPath path = new SearchPath();
            while (came_from.ContainsKey(current))
            {
                var tmp = came_from[current];
                path.Insert(0, tmp.action);
                current = tmp.state;
            }
            return path;
        }
    }
}
