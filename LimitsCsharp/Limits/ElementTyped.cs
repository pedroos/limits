using System;
using System.Collections.Generic;
using System.Text;

namespace Limits.ElementTyped
{
    // For each tuple, we define exactly the type of each element. We do this because 
    // it's possible in the language and we don't have to specify quantities in types.
    // Note: there are no element typed sets.
    public class Tuple2<T1,T2> : IEquatable<Tuple2<T1,T2>>
    {
        public T1 a;
        public T2 b;
        public Tuple2(T1 a, T2 b)
        {
            this.a = a;
            this.b = b;
        }
        public bool Equals(Tuple2<T1, T2> other)
        {
            return other.a.Equals(a) && other.b.Equals(b);
        }
        public override int GetHashCode()
        {
            return a.GetHashCode() ^ b.GetHashCode();
        }
        // Implicit conversion to set element
        public static implicit operator ElemPrimitive.SetElement<Tuple2<T1,T2>>(Tuple2<T1,T2> t) => 
            new ElemPrimitive.SetElement<Tuple2<T1,T2>>(t);
    }

    public class Tuple3<T1,T2,T3> : Tuple2<T1,Tuple2<T2,T3>>
    {
        public Tuple3(T1 a, Tuple2<T2,T3> b) : base(a, b) { }
    }

    public class Tuple4<T1,T2,T3,T4> : Tuple2<T1,Tuple3<T2,T3,T4>>
    {
        public Tuple4(T1 a, Tuple3<T2,T3,T4> b) : base(a, b) { }
    }

    public class Tuple5<T1,T2,T3,T4,T5> : Tuple2<T1,Tuple4<T2,T3,T4,T5>>
    {
        public Tuple5(T1 a, Tuple4<T2,T3,T4,T5> b) : base(a, b) { }
    }
}
