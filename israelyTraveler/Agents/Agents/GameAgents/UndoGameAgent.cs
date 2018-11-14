using AIBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World;
using AIBasic.Algorithms.Base;
using AIBasic.Algorithms.Games;

namespace Agents.Agents.GameAgents
{
    using evalMethodType = SearchTypes<TravelGameState>.HuristicMethodType;
    using StateActionsCollection = SearchTypes<TravelGameState>.ActionsCollection;
    using StateAndCost = SearchTypes<TravelGameState>.StateAndCost;
    using StateActionType = SearchTypes<TravelGameState>.ActionType;
    using cuttOfType = SearchTypes<TravelGameState>.GoalMethodType;  
    public abstract class UndoGameAgent : BaseTraveler
    {

        public UndoGameAgent(int startLocation)
            : base(startLocation){}

        protected Dictionary<StateActionType, ActionType> _actionsMap =
            new Dictionary<StateActionType, ActionType>();
        
        private int _prevLocation;
        private double _prevCost;
        private int _prevNumOfActions;
        private bool _prevCarryWater;        

        public virtual StateActionsCollection getActions(TravelGameState state)
        {
            //TODO:edit
            //only pickup water and drive
            TravelWorld world = state.ToWorld();
            StateActionsCollection res = new StateActionsCollection();
            if (CanPickupWaterNow(state, world))
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

            actions = getFireActions(state, world);
            foreach (var action in actions)
            {
                var stateAction = ConvertToStateAction(action);
                _actionsMap.Add(stateAction, action);
                res.Add(stateAction);
            }

            //last because is last prefable
            if (!_actionsMap.ContainsKey(noOperation))
            {
                ActionType action = noOpertion;
                StateActionType stateAction = noOperation;
                _actionsMap.Add(stateAction, action);
            }
            res.Add(noOperation);
            return res;
        }

        private IEnumerable<ActionType> getFireActions(TravelGameState state, TravelWorld world)
        {
            List<ActionType> res = new List<ActionType>();
            var ways = world.GetClearWays(state.locations[this]);
            foreach (var way in ways)
            {
                int dest = way.getAnotherPlace(state.locations[this]);
                var action = new ActionType(w => startAfire(w, dest));
                res.Add(action);
            }
            return res;
        }

        protected bool CanPickupWaterNow(TravelGameState state, TravelWorld world)
        {
            return !state.CarryWatter && world.HaveWater(state.locations[this]);
        }

        protected IEnumerable<ActionType> getDriveActions(TravelGameState state,TravelWorld world)
        {
            List<ActionType> res = new List<ActionType>();
            IEnumerable<TravelEdge> ways;
            if (state.CarryWatter)
                ways = world.GetWays(state.locations[this]);
            else
                ways = world.GetClearWays(state.locations[this]);

            foreach (var way in ways)
            {
                int dest = way.getAnotherPlace(state.locations[this]);
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

        protected StateAndCost PickupWater(TravelGameState state)
        {
            return rollBackAction(state,pickupWater);
        }

        protected StateAndCost noOperation(TravelGameState state)
        {
            return rollBackAction(state, noOpertion);
        }

        private StateAndCost rollBackAction(TravelGameState state, ActionType action)
        {
            backup();
            CurrentLocation = state.locations[this];
            CarryWater = state.CarryWatter;
            var newWorld = state.ToWorld();
            bool sucseed = action(newWorld);
            if (!sucseed)
                throw new WrongActionException();
            var neweState = ToState(newWorld);
            neweState.totalMoves = new Dictionary<BaseTraveler, int>(state.totalMoves);
            neweState.totalMoves[this] += 1;
            double cost = _cost - _prevCost;
            rollBack();
            return new StateAndCost(neweState, cost);
        }

        protected virtual TravelGameState ToState(TravelWorld newWorld)
        {
            var newState = new TravelGameState();
            newState.CarryWatter = CarryWater;
            newState.locations = new Dictionary<BaseTraveler, int>(newWorld.GetPlayersLocations());
            newState.locations[this] = CurrentLocation;
            newState.totalMoves = new Dictionary<BaseTraveler, int>();
            foreach (var item in newState.locations.Keys)
            {
                newState.totalMoves.Add(item, 0);
            }
            newState.WaterPlaces = new List<int>(newWorld.GetWaterPlaces());
            newState.FireWays = new List<TravelEdge>(newWorld.GetFireWays());
            newState.WorldGraph = newWorld.GetGraph();            
            return newState;
        }
    }

    class WrongActionException : Exception
    {
    }
}
