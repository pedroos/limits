using System;
using System.Collections.Generic;
using System.Text;

namespace Limits.NumberTypes
{
    public abstract class Natural { }

    public class One : Natural { }

    public class Two : Natural { }

    public class Three : Natural { }

    public class PairElement<T> : Nuple<T>
        where T : IEquatable<T>
    {
        public T x;
        public PairElement(T x)
        {
            this.x = x;
        }
    }

    public abstract class Nuple<T> { }

    public class OrderedPair<T> : Nuple<T>
        where T : IEquatable<T>
    {
        public Nuple<T> a;
        public Nuple<T> b;
        public OrderedPair(Nuple<T> a, Nuple<T> b)
        {
            this.a = a;
            this.b = b;
        }
    }

    public class Tuple<N, T> : OrderedPair<T>
        where N : Natural
        where T : IEquatable<T>
    {
        //public static OrderedPair<T> Create<N>(params Nuple<T>[] x)
        //{
        //    // Doesn't work. Should return a Tuple, but an OrderedPair isn't a Tuple.
        //    if (typeof(N) == typeof(Two))
        //        return new OrderedPair<T>(x[0], x[1]);
        //    else if (typeof(N) == typeof(Three))
        //        return new OrderedPair<T>(x[0], new OrderedPair<T>(x[1], x[2]));
        //    return null;
        //}

        //public Tuple(params Nuple<T>[] x) : base(
        //    // Doesn't work. Can't use expressions inside base constructor definition.
        //// 2
        //    typeof(N) == typeof(Two) ? 
        //    x[0], x[1] : 
        //// 3
        //    x[0], new OrderedPair<T>(x[1], x[2])
        //)
        //{

        //}

        // Best remaining option, but N (the dependent type) becomes irrelevant
        public Tuple(Nuple<T> a, Nuple<T> b) : base(a, b) { }
        public Tuple(Nuple<T> a, Nuple<T> b, Nuple<T> c) : base(a, new OrderedPair<T>(b, c)) { }
    }
}
