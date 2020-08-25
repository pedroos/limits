using System;
using System.Collections.Generic;
using System.Linq;

namespace Limits.ElemAsSet
{
    public class Singleton<T> : Set<T> 
        where T : IEquatable<T>
    {
        protected T x;

        public Singleton(T x)
        {
            this.x = x;
        }

        public override string ToString()
        {
            return string.Format("{0}", x);
        }
    }

    public class Empty<T> : Singleton<T> 
        where T : IEquatable<T>
    {
        public Empty() : base(default(T)) { }
    }

    public class Set<T>
        where T : IEquatable<T>
    {
        private List<Set<T>> elems;

        protected Set()
        {
            elems = new List<Set<T>>();
        }

        //public Set(T elem)
        //{
        //    elems = new List<Set<T>>();
        //    elems.Add(new Set<T>(elem)); // Infinitely recursive
        //}

        public Set(IEnumerable<Set<T>> elems) : this()
        {
            this.elems.AddRange(elems);
        }

        public override string ToString()
        {
            return string.Format("{{{0}}}", string.Join(", ", elems.Select(e => e.ToString())));
        }
    }

    // - An ordered pair (a,b) is defined as the set {{a},{a,b}}
    // - (b,a) would be {{b},{a,b}} or {{b},{b,a}} or {{b,a},{b}}
    // - Ordered pair equality should reduce to set equality (test for that)

    public class OrderedPair<T> : Set<T> 
        where T : IEquatable<T>
    {
        /// <summary>
        /// Create an ordered pair from two sets A, B as a set of form {A, {A, B}}
        /// </summary>
        public OrderedPair(Set<T> a, Set<T> b) : base(
            new List<Set<T>>
            {
                a, new Set<T>(new List<Set<T>>
                {
                    a, b
                })
            }
        ) { }
    }

    // - An ordered triple (a,b,c) is defined as ((a,b),c) which is {{{a},{a,c}},{b}}
    // - (b,c,a) would be {{{b},{b,c}},{a}} or {{a},{{b,c},{c}}}
    // - (a,b,c,d) would be (((a,b),c),d)

    public class OrderedTriple<T> : OrderedPair<T>
        where T : IEquatable<T>
    {
        public OrderedTriple(Set<T> a, Set<T> b, Set<T> c) : base(
            new OrderedPair<T>(a, b), c
        ) { }
    }

    //public class OrderedTuple<T> : IEnumerable<T>, OrderedPair<T> 
    //    where T : IEquatable<T>
    //{
    //    // Build from list
    //}
}
