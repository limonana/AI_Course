using AIBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World;
using AIBasic.Algorithms.Base;

namespace Agents.Agents.SearchAgents
{
    using HeuristicMethodType = SearchTypes<TravelSearchState>.HuristicMethodType;
    using StateActionsCollection = SearchTypes<TravelSearchState>.ActionsCollection;
    using SearchStateAndCost = SearchTypes<TravelSearchState>.StateAndCost;
    using StateActionType = SearchTypes<TravelSearchState>.ActionType;
    using GoalMethodType = SearchTypes<TravelSearchState>.GoalMethodType;
    using SearchPath = SearchTypes<TravelSearchState>.SearchPath;    
    public abstract class BaseSearchAgent : BaseTraveler
    {
        public int Expanstions { get; protected set; }

        protected HeuristicMethodType _heuristic = null;        
        protected Dictionary<StateActionType,ActionType> _actionsMap = 
            new Dictionary<StateActionType,ActionType>();
        public BaseSearchAgent(HeuristicMethodType h, int startLocation)
            : base(startLocation)
        {
            _heuristic = h;
            Expanstions = 0;
        }
        
        private int _prevLocation;
        private double _prevCost;
        private int _prevNumOfActions;
        private bool _prevCarryWater;        

        protected StateActionsCollection getActions(TravelWorld world)
        {
            return getActions(ToSearchState(world));            
        }

        protected virtual StateActionsCollection getActions(TravelSearchState state)
        {
            //basic :only pickup water and drive
            TravelWorld world = state.ToWorld();
            StateActionsCollection res = new StateActionsCollection();
            if (CanPickupWaterNow(ref state, world))
            {
                if (!_actionsMap.ContainsKey(PickupWater))
                {
                    ActionType action = pickupWater;
                    StateActionType stateAction = PickupWater;
                    _actionsMap.Add(stateAction, action);
                }
                res.Add(PickupWater);
            }
                     
            var actions = getDriveActions(state, world);
            foreach (var action in actions)
            {
                var stateAction = ConvertToStateAction(action);
                _actionsMap.Add(stateAction, action);
                res.Add(stateAction);    
            }            
            return res;
        }

        protected bool CanPickupWaterNow(ref TravelSearchState state, TravelWorld world)
        {
            return !state.CarryWatter && world.HaveWater(state.CurrentLocation);
        }

        protected IEnumerable<ActionType> getDriveActions(TravelSearchState state,TravelWorld world)
        {
            List<ActionType> res = new List<ActionType>();
            IEnumerable<TravelEdge> ways;
            if (state.CarryWatter)
                ways = world.GetWays(state.CurrentLocation);
            else
                ways = world.GetClearWays(state.CurrentLocation);

            foreach (var way in ways)
            {
                int dest = way.getAnotherPlace(state.CurrentLocation);
                var action = new ActionType(w => drive(w, dest));
                res.Add(action);
            }
            return res;
        }

        protected StateActionType ConvertToStateAction(ActionType action)
        {
            var stateAction = new StateActionType(state1 => rollBackAction(state1, action));
            return stateAction;
        }        

        void backup()
        {
            _prevLocation = CurrentLocation;
            _prevCost = TotalCost;
            _prevNumOfActions = NumOfActions;
            _prevCarryWater = CarryWater;
        }

        void rollBack()
        {
            CurrentLocation = _prevLocation;
            _cost = _prevCost;
            _numOfActions = _prevNumOfActions;
            CarryWater = _prevCarryWater;
        }

        protected SearchStateAndCost PickupWater(TravelSearchState state)
        {
            return rollBackAction(state,pickupWater);
        }

        private SearchStateAndCost rollBackAction(TravelSearchState state, ActionType action)
        {
            backup();
            CurrentLocation = state.CurrentLocation;
            CarryWater = state.CarryWatter;
            var newWorld = state.ToWorld();
            bool sucseed = action(newWorld);
            if (!sucseed)
                throw new WrongActionException();
            var neweState = ToSearchState(newWorld);
            double cost = _cost - _prevCost;
            rollBack();
            return new SearchStateAndCost(neweState, cost);
        }

        protected TravelSearchState ToSearchState(TravelWorld newWorld)
        {
            var newState = new TravelSearchState();
            newState.CarryWatter = CarryWater;
            newState.CurrentLocation = CurrentLocation;
            newState.WaterPlaces = new List<int>(newWorld.GetWaterPlaces());
            newState.FireWays = new List<TravelEdge>(newWorld.GetFireWays());
            newState.WorldGraph = newWorld.GetGraph();
            return newState;
        }

        protected GoalMethodType GetGoalFunc(int target)
        {
            return new GoalMethodType(state =>
                { return (state.CurrentLocation == target); });
        }

        protected Path<ActionType> ConvertToActionsPath(SearchPath path)
        {
            Path<ActionType> res = new Path<ActionType>();
            foreach (var item in path)
            {
                res.Add(_actionsMap[item]);
            }
            return res;
        }
    }

    class WrongActionException : Exception
    {
    }
}
