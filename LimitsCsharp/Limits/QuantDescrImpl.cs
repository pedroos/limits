using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Limits.QuantDescrImpl
{
    //public class Number<Q1> { }
    //public class Number<Q1, Q2> : Number<Q1> where Q2 : Number<Q1> { }
    //public class Number<Q1, Q2, Q3> : Number<Q1, Q2> where Q2 : Number<Q1> where Q3 : Number<Q2> { }

    //public class MPC<N> where N : Number<N> { }

    public abstract class Natural { }
    public class One : Natural { }
    public class Two : Natural { }

    /// <remarks>
    /// Arrays are used (even though static bounds checking is impossible) to also store positions 
    /// in them. Simulate sets by ignoring repeat insertions.
    /// </remarks>
    // We gain typed elements.
    // TODO: implement positions.
    public abstract class IrrColl
    {
        protected T[] Initialize<T, TQ>()
            where TQ : Natural
        {
            return new T[typeof(TQ) == typeof(Two) ? 2 : 1];
        }

        private int Number<TQ>()
            where TQ : Natural
        {
            return typeof(TQ) == typeof(Two) ? 2 : 1;
        }

        /// <param name="t">The type's collection to add to (from the subclass)</param>
        /// <param name="x">The element to add</param>
        protected void AddT<T, TQ>(T[] t, T x)
            where TQ : Natural
        {
            if (t.Contains(x)) return;
            int size = Number<TQ>();
            // Scan the collection for the first unset position
            for (int i = 0; i < size; ++i)
            {
                // Would need nullable reference types
                if (t[i] == null || t[i].Equals(default(T)))
                {
                    t[i] = x;
                    return;
                }
            }
            throw new InvalidElementException();
        }
        protected IEnumerable<T> GetT<T>(T[] t)
        {
            return t;
        }
    }
    public class IrrColl<A, AQ> : IrrColl
        where AQ : Natural
    {
        readonly A[] a;
        public IrrColl()
        {
            a = Initialize<A, AQ>();
        }
        public void AddA(A x)
        {
            AddT<A, AQ>(a, x);
        }
        public IEnumerable<A> GetA()
        {
            return GetT(a);
        }
    }
    public class IrrColl<A, AQ, B, BQ> : IrrColl<A, AQ>
        where AQ : Natural
        where BQ : Natural
    {
        readonly B[] b;
        public IrrColl() : base()
        {
            b = Initialize<B, BQ>();
        }
        public void AddB(B x)
        {
            AddT<B, BQ>(b, x);
        }
        public IEnumerable<B> GetB()
        {
            return GetT(b);
        }
    }
    public class IrrColl<A, AQ, B, BQ, C, CQ> : IrrColl<A, AQ, B, BQ>
        where AQ : Natural
        where BQ : Natural
        where CQ : Natural
    {
        readonly C[] c;
        public IrrColl() : base()
        {
            c = Initialize<C, CQ>();
        }
        public void AddC(C x)
        {
            AddT<C, CQ>(c, x);
        }
        public IEnumerable<C> GetC()
        {
            return GetT(c);
        }
    }

    //public class IrrCollN<Q>
    //    where Q : Natural
    //{
    //    readonly protected object[] xs;
    //    readonly protected int number;
    //    public MPC2()
    //    {
    //        number = typeof(Q) == typeof(Two) ? 2 : 1;
    //        xs = new object[number];
    //    }
    //    public void AddT<T, TQ>(T[] t, T x)
    //    {
    //        int size = typeof(TQ) == typeof(Two) ? 2 : 1;
    //        for (int i = 0; i <= size; ++i)
    //        {
    //            if (i == size) throw new Exception("Type partition full");
    //            if (t.Equals(default(T))) continue;
    //            t[i] = x;
    //            break;
    //        }
    //    }
    //    //protected IEnumerable<T> GetT<T>(T[] t)
    //    //{
    //    //    return t;
    //    //}
    //}

    #region Exceptions

    public class InvalidCollectionException : Exception { }

    public class InvalidElementException : InvalidCollectionException { }

    #endregion
}
