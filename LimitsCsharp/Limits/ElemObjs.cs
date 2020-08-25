using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Limits.ElemOjbs
{
    //public static class Universe
    //{
    //    // Set tree.
    //    public static Set First { get; }
    //    static Universe()
    //    {
    //        First = new Set();
    //    }
    //}

    // Unformalized sets are equal and the first set used to formalize the following sets.
    // Implement walking down and comparing value equality to reference equality.
    public class Set : IEquatable<Set>
    {
        protected Guid? id;
        private readonly HashSet<Set> elems;
        public IEnumerable<Set> Enumerable { get { return elems.AsEnumerable(); } }
        protected int? maxSize;
        public Set()
        {
            elems = new HashSet<Set>();
        }
        public virtual void Add(Set elem)
        {
            if (maxSize.HasValue && elems.Count == maxSize)
                throw new MaxSetSizeException(maxSize.Value);
            // Uniquely and globally identify the Set
            if (!elem.id.HasValue) elem.id = Guid.NewGuid();
            elems.Add(elem);
        }
        public int ElemCount()
        {
            return elems.OfType<SetElement>().Count();
        }
        public override string ToString()
        {
            return id.HasValue ? "Set " + id.ToString() : "Detached set";
        }
        public bool Equals(Set other)
        {
            // Unformalized sets are equal
            // Other sets are equal if their ids match
            return (!id.HasValue && !other.id.HasValue) ||
                id.HasValue && other.id.HasValue && id.Value.Equals(other.id.Value);
        }
        public override int GetHashCode()
        {
            if (!id.HasValue) return int.MinValue; // Always match
            return id.Value.GetHashCode();
        }
        //public IEnumerable<Set> WalkDown()
        //{
        //    var curr = this;
        //    while (true)
        //    {
        //        yield return curr;
        //        foreach (var elem in curr.elems)
        //        {
        //            yield return elem;
        //        }

        //    }
        //    yield return this;
        //    foreach (Set set in elems)
        //        yield return set;
        //}
    }

    public abstract class SetElement : Set
    {
    }

    public class EmptySetElement : SetElement
    {
    }

    public class Tuple2 : Set
    {
        public Tuple2()
        {
            maxSize = 2;
        }
    }

    public class Relation2 : Set // Set of Tuple2s
    {
    }

    // An object may initialize its elements and be a record
    // TODO: handle set events (Vertex element type, etc.)
    public class Graph : Tuple2
    {
        public Set VerticesSet { get; }
        public Set EdgesSet { get; }
        public Graph()
        {
            VerticesSet = new Set();
            Add(VerticesSet);
            EdgesSet = new Set();
            Add(EdgesSet);
        }
    }

    public class Integer : SetElement
    {
        public int Value { get; }
    }

    public class Vertex : SetElement
    {
    }

    #region Exceptions

    public class MalformedSetException : Exception { }

    public class CycleException : MalformedSetException { }

    public class MaxSetSizeException : MalformedSetException
    {
        public int MaxSize { get; }
        public MaxSetSizeException(int maxSize)
        {
            MaxSize = maxSize;
        }
    }

    #endregion
}
