using System;
using System.Collections.Generic;
using System.Text;

namespace Limits.UntypedSet
{
    public class Equatable : IEquatable<object> // Can't implement equality
    {

    }

    public class Set<T>
        where T : IEquatable<T> // We expect untyped instances to have T <= IEquatable
    {
        private List<T> elems;

        public Set(IEnumerable<T> elems)
        {
            this.elems = new List<T>();
            this.elems.AddRange(elems);
        }
    }

    public class OrderedPair<T> : Set<T>
        where T : IEquatable<T>
    {
        public OrderedPair(T a, T b) : base(
            new List<T>() { a, b }
            ) { }
    }
}
