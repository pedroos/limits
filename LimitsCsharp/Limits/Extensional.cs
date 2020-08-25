using System;
using System.Collections.Generic;
using System.Linq;

namespace Limits.Extensional
{
    // In descending order of elementarity.

    public class OrderedTuple2<TA, TB>
    {
        public TA a;
        public TB b;
        public OrderedTuple2(TA a, TB b)
        {
            this.a = a;
            this.b = b;
        }
    }

    public class OrderedTuple2<T> : OrderedTuple2<T, T>
    {
        public OrderedTuple2(T a, T b) : base(a, b) { }
    }

    public class OrderedTuple3<T>
    {
        public T a;
        public T b;
        public T c;
    }

    public class Set<T>
    {
        protected List<T> elems;

        public Set()
        {
            elems = new List<T>();
        }

        public virtual void Add(T elem)
        {
            elems.Add(elem);
        }

        public List<T>.Enumerator Elems
        {
            get { return elems.GetEnumerator(); }
        }
    }

    public class Number : IEquatable<Number>
    {
        public int n;
        public Number(int n)
        {
            this.n = n;
        }

        public bool Equals(Number other)
        {
            return n == other.n;
        }
    }

    public interface IOrdered<T>
    {
        bool LessThan(T a, T b);
    }

    public class OrderedSet<T> : Set<T>, IOrdered<T>
    {
        public bool LessThan(T a, T b)
        {
            // Como definir?
            // Reflexiva, antissimétrica e transitiva (parcial)
            // Assimétrica e transitiva (total)
            //return a < b;
            return false;
        }
    }

    public class TotallyOrderedSet<T> : OrderedSet<T>
    {
        public T Minimum()
        {
            if (elems.Count == 0) return default;
            T min = elems.First();
            foreach (T a in elems)
            {
                if (LessThan(a, min))
                    min = a;
            }
            return min;
        }

        //public T Maximum()
        //{
        //    if (elems.Count == 0) return default;
        //    T max = elems.First();
        //    foreach (T a in elems)
        //    {
        //        if (!a.LessThan(max))
        //            max = a;
        //    }
        //    return max;
        //}

        //public T Infimum(OrderedSet<T> subset)
        //{

        //}

        //public T Supremum(OrderedSet<T> subset)
        //{

        //}
    }

    public class Relation2<TA, TB> : Set<OrderedTuple2<TA, TB>> { }

    public class Relation2<T> : Relation2<T, T> { }

    public class Relation3<T> : Set<OrderedTuple3<T>> { }

    /// <summary>
    /// Denotes an error in the formation of an object.
    /// </summary>
    public class ConstraintException : Exception { }

    public class DuplicateElementInFunctionImageException : ConstraintException { }

    public class Function<TDomain, TImage> : Relation2<TDomain, TImage>
        where TDomain : IEquatable<TDomain>
    {
        public override void Add(OrderedTuple2<TDomain, TImage> elem)
        {
            if (elems.Any(e => e.a.Equals(elem.a)))
                throw new DuplicateElementInFunctionImageException();
            elems.Add(elem);
        }
    }
}
