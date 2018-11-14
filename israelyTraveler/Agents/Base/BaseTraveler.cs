using Agents.World;
using AIBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World;

namespace Agents
{    
    public abstract class BaseTraveler : BaseAgent<bool,TravelWorld>,ITraveler
    {

        public Costs costs { get; set; }
        public int? Goal { get; set; }

        protected double _cost;
        protected int _numOfActions;
        public BaseTraveler(int startLocation)
        {
            CurrentLocation = startLocation;
            costs = new Costs();
        }

        #region ITraveler

        public virtual bool drive(TravelWorld world, int adjacentPlace)
        {
            if (CurrentLocation == adjacentPlace)
                return false;

            try
            {
                Console.WriteLine("drive to {0}", adjacentPlace);
                double cost = world.getCostWay(CurrentLocation, adjacentPlace);
                if (world.isClear(CurrentLocation, adjacentPlace))
                {
                    if (CarryWater)
                        _cost += cost * 2;
                    else
                        _cost += cost;

                    CurrentLocation = adjacentPlace;
                    world.SetLocation(this, CurrentLocation);
                    ++_numOfActions;
                    return true;
                }
                else
                {
                    if (CarryWater)
                    {
                        MoveWithWater(world, adjacentPlace);
                        _cost += cost * 2;
                        ++_numOfActions;
                        return true;
                    }
                    else
                    {
                        //_cost += cost;
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void MoveWithWater(TravelWorld world, int adjacentPlace)
        {
            //pass through the fire,use the water to stop the fire
            CarryWater = false;
            world.StopFire(CurrentLocation,adjacentPlace);
            CurrentLocation = adjacentPlace;
        }

        public virtual bool startAfire(TravelWorld world, int adjacentPlace)
        {
            if (!world.isClear(CurrentLocation, adjacentPlace))
                return false;

            Console.WriteLine("start a fire at {0},{1}", CurrentLocation, adjacentPlace);
            world.SetFire(CurrentLocation, adjacentPlace);
            CurrentLocation = adjacentPlace;
            ++_numOfActions;
            _cost += costs.Fire;
            return true;

        }

        public bool  pickupWater(TravelWorld world)
        {
            Console.WriteLine("pickup water");
            if (world.TakeWater(CurrentLocation))
            {
                CarryWater = true;
                _cost += costs.Pickup;
                ++_numOfActions;
                return true;
            }
            else
                return false;
        }

        public bool noOpertion(TravelWorld world)
        {
            Console.WriteLine("do no-op");
            _cost += costs.Epsilon;
            ++_numOfActions;
            return true;
        }

        #endregion

        public virtual int CurrentLocation
        {
            get;
            protected set;
        }

        public virtual bool CarryWater
        {
            get;
            protected set;
        }        

        public override double TotalCost
        {
            get { return _cost; }            
        }

        public override int NumOfActions
        {
            get { return _numOfActions; }            
        }

        public override void AddCost(double cost)
        {
            _cost += cost;
        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            b.AppendLine("name: " + Name);
            b.AppendLine("location: " + CurrentLocation);
            b.AppendLine("cost: " + TotalCost);
            if (CarryWater)
                b.AppendLine("Carry");
            b.AppendLine("num of actions: " + NumOfActions);
            return b.ToString();
        }
    }
}
