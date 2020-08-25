using System;
using System.Collections.Generic;
using System.Linq;

namespace Limits.Intensional
{
    // In descending order of elementarity.

    public class OrderedTuple2<TA, TB> : IEquatable<OrderedTuple2<TA, TB>> 
        where TA: IEquatable<TA>
        where TB: IEquatable<TB>
    {
        public TA a;
        public TB b;
        public OrderedTuple2(TA a, TB b)
        {
            this.a = a;
            this.b = b;
        }

        public bool Equals(Intensional.OrderedTuple2<TA, TB> other)
        {
            return a.Equals(other.a) && b.Equals(other.b);
        }
    }

    public class OrderedTuple2<T> : OrderedTuple2<T, T> 
        where T:IEquatable<T>
    {
        public OrderedTuple2(T a, T b) : base(a, b) { }
    }

    public class OrderedTuple3<T>
    {
        public T a;
        public T b;
        public T c;
    }

    public class Set<T> : Relation2<T>
        where T:IEquatable<T>
    {
        protected List<T> elems;
        protected Dictionary<Type, List<OrderedTuple2<T>>> relationExtension;

        public Set()
        {
            elems = new List<T>();
            relationExtension = new Dictionary<Type, List<OrderedTuple2<T>>>();
        }

        public virtual void Add(T elem)
        {
            elems.Add(elem);
        }

        public List<T>.Enumerator Elems
        {
            get { return elems.GetEnumerator(); }
        }

        public void AddRelationExtension<Relation2>(IEnumerable<OrderedTuple2<T>> extension)
        {
            Type relationType = typeof(Relation2);
            if (!relationExtension.ContainsKey(relationType))
                relationExtension[relationType] = new List<OrderedTuple2<T>>();
            relationExtension[relationType].AddRange(extension);
        }

        public virtual bool Is(T a, T b)
        {
            // A chave deveria ser polimórfica...
            if (relationExtension[typeof(Relation2<T>)]
                .Any(e => e.Equals(new OrderedTuple2<T>(a, b))))
                return true;

            //// If relation is reflexive
            //if (typeof(ReflexiveRelation2<T>).IsAssignableFrom(typeof(TRelation)))
            //{
            //    // Check whether inverted order of arguments returns relation
            //    if (AreRelated<TRelation>(b, a)) return true;
            //}
            //// Other than that, we would have to check ALL relations on the set to 
            //// see how we would determine if and how any two elements a, b are related
            ///
            return false;
        }
    }

    public class OrderedSet<T> : Set<T>, OrderingRelation2<T>
        where T : IEquatable<T>, IComparable<T>
    {
        public override bool Is(T a, T b)
        {
            // Reflexiva, antissimétrica e transitiva (parcial)
            // Assimétrica e transitiva (total)

            // Evaluation order: from this relation (ordering) to less specific 
            // (parent relations)

            int compare = a.CompareTo(b);
            bool orderingIs = compare == -1;
            if (orderingIs) return orderingIs;

            bool symmetricIs = (this as SymmetricRelation2<T>).Is(a, b);
            if (symmetricIs) return symmetricIs;

            bool setIs = (this as Set<T>).Is(a, b);
            if (setIs) return setIs;

            return false;
        }
    }

    //public class TotallyOrderedSet<T> : OrderedSet<T>
    //{
    //public T Minimum()
    //{
    //    if (elems.Count == 0) return default;
    //    T min = elems.First();
    //    foreach (T a in elems)
    //    {
    //        if (LessThan(a, min))
    //            min = a;
    //    }
    //    return min;
    //}

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
    //}

    //public class Relation2<TA, TB> : Set<OrderedTuple2<TA, TB>> { }

    //public class Relation2<T> : Relation2<T, T> { }

    //public class Relation3<T> : Set<OrderedTuple3<T>> { }

    public interface Relation2<T>
    {
        bool Is(T a, T b);
    }

    public interface EqualityRelation2<T> : Relation2<T>
        where T : IEquatable<T> { }

    public interface ReflexiveRelation2<T> : EqualityRelation2<T>
        where T : IEquatable<T>
    { }

    public interface SymmetricRelation2<T> : EqualityRelation2<T>
        where T : IEquatable<T> { }

    public interface OrderingRelation2<T> : SymmetricRelation2<T> 
        where T : IEquatable<T>, IComparable<T> { }

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
}
