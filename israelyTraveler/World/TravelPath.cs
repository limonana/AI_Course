using AIBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World
{
    public class TravelPath:IPath<int>,IEnumerable<int>
    {        
        protected double _cost = 0;
        Path<int> _path = new Path<int>();
  
        public int Destination { get { return this.Last(); } }
        
        public void Add(int location,double cost)
        {
            _path.Add(location);
            _cost += cost;            
        }
        

        public void Insert(int index,int location, double cost)
        {
            _path.Insert(index, location);
            _cost += cost;
        }

        public void InsertAtStart(int location, double cost)
        {
            Insert(0, location, cost);
        }



        public double Cost()
        {
            return _cost;
        }

        public int TakeNextStep()
        {
            return _path.TakeNextStep();
        }

        public bool Replace(int orgItem, int newItem)
        {
            return _path.Replace(orgItem, newItem);
        }

        public IEnumerator<int> GetEnumerator()
        {
            return _path.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _path.GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            TravelPath other = obj as TravelPath;
            if (other != null)
            {
                //put cost first because it more efficent
                return _cost.Equals(other.Cost()) &&
                    _path.Equals(other._path);                    
            }
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int Count
        {
            get { return _path.Count; }
        }
    }
}
