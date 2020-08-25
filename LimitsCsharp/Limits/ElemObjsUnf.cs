using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Limits.ElemOjbsUnf
{
    // - Remove formalization to explore the Set graph.
    // - Problem: C# references won't work as equality because then each Set can't keep a 
    //   different parent. Use values as set equality (GetHashCode()).
    // - Urelement as a primordial non-set element (akin to ElemPrimitive.SetElement, but 
    //   as a common interface instead of subclass)
    // - Per-set axioms, operating on children:
    //   - Regularity = absence of cycles.

    public abstract class SetElement { 
        public Set Parent { get; set; }
        // WalkUp is necessarily a collection of Sets because no Urelement could exist in the 
        // path up from an element. It does not include the element from which it is called.
        public IEnumerable<Set> WalkUp()
        {
            var curr = Parent;
            while (true)
            {
                if (curr == default) yield break;
                yield return curr;
                curr = curr.Parent;
            }
        }
    }

    public sealed class Set : SetElement
    {
        // Axioms
        public bool Regularity { get; private set; }
        // Intrinsic properties
        private readonly HashSet<SetElement> elems;
        public IEnumerable<SetElement> Enumerable { get { return elems.AsEnumerable(); } }
        readonly int? maxSize;
        public Set(bool regularity = true, int? maxSize = default)
        {
            Regularity = regularity;
            this.maxSize = maxSize;
            elems = new HashSet<SetElement>();
        }
        public void Add(SetElement elem)
        {
            // Regularity check
            var walkUp = WalkUp();
            var lastRegular = walkUp.LastOrDefault(wu => wu.Regularity);
            var walkUpUntilLastRegular = walkUp.TakeWhile(wu => wu.Equals(lastRegular));
            foreach (var parentSet in walkUp)
            {
                // Walk until lastRegular... Needs equality...
            }
            if (walkUp.Any(wu => wu.Equals(elem)))
                throw new CycleException();

            if (maxSize.HasValue && elems.Count == maxSize)
                throw new MaxSetSizeException(maxSize.Value);
            elems.Add(elem);
            elem.Parent = this;
        }
        public int Count()
        {
            return elems.Count();
        }
        public override string ToString()
        {
            return "(set)";
        }
        public override int GetHashCode()
        {
            if (elems.Count() == 0) return int.MinValue;
            return new Random().Next(); // Not equal
        }
        public override bool Equals(object obj)
        {
            return GetHashCode() == obj.GetHashCode();
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

        static Set reals;

        static Set()
        {
            reals = new Set();
        }

        public static Set Reals
        {
            get
            {
                return reals;
            }
        }
    }

    public class Urelement : SetElement { }

    // An object may initialize its elements and be a record
    // TODO: handle set events (Vertex element type, etc.)
    //public class Graph : Tuple2
    //{
    //    public Set VerticesSet { get; }
    //    public Set EdgesSet { get; }
    //    public Graph()
    //    {
    //        VerticesSet = new Set();
    //        Add(VerticesSet);
    //        EdgesSet = new Set();
    //        Add(EdgesSet);
    //    }
    //}

    //public class Integer : Set
    //{
    //    public int Value { get; }
    //}

    // TODO: Urelement
    //public class Vertex : Set
    //{
    //}

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
