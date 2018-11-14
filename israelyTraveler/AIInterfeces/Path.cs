using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBasic
{
    public class Path<T>:List<T>,IPath<T>
    {
        public T TakeNextStep()
        {
            T res = this[0];
            this.RemoveAt(0);
            return res;
        }        

        public override bool Equals(object obj)
        {
            if (obj is Path<T>)
            {
                return this.SequenceEqual((Path < T >)obj);
            }
            else
                return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Replace(T orgItem, T newItem)
        {
            int index = this.FindIndex(x => x.Equals(orgItem));
            if (index >= 0)
            {
                this[index] = newItem;
                return true;
            }
            else
                return false;
        }

    

        public void InsertAtStart(T item)
        {
           Insert(0,item);
        }
}

    public interface IPath<T>
    {
        T TakeNextStep();
        bool Replace(T orgItem, T newItem);       
    }
}
