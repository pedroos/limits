using System;
using System.Collections.Generic;
using System.Linq;

namespace Limits.ElemPrimitiveSum
{
    // Same relationship as ElemPrimitive, but instead of a subclass SetElement is part of a sum type with Set 
    // with respect to consumers of sets/set elements

    // Common type for Set and SetElement
    public interface ISetItem<T>
        where T : IEquatable<T>
    { }

    // Same class as ElemPrimitive, but implementing ISetItem
    public class Set<T> : ISetItem<T>, IEquatable<Set<T>>
        where T : IEquatable<T>
    {
        protected readonly HashSet<ISetItem<T>> elems;
        public event Func<ISetItem<T>, bool> BeforeElementAdded;
        public event Action<ISetItem<T>> ElementAdded;
        public int ElemCount { get; private set; } // Only non-set elements
        public Set()
        {
            ElemCount = 0;
            elems = new HashSet<ISetItem<T>>();
        }
        public virtual void Add(ISetItem<T> elem)
        {
            if (elems.Contains(elem)) return;
            if (BeforeElementAdded != null && !BeforeElementAdded.Invoke(elem)) return;
            elems.Add(elem);
            ElemCount = elems.OfType<SetElement<T>>().Count();
            ElementAdded?.Invoke(elem);
        }
        public bool Contains(ISetItem<T> set) => 
            set is SetElement<T> se ?                                      // Element comparison
                elems.OfType<SetElement<T>>().Any(e => e.Equals(se)) : 
                elems.Cast<Set<T>>().Any(e => e.Equals((Set<T>)set));      // Set comparison
        public override string ToString() => string.Format("{{{0}}}", string.Join(", ", 
            elems.Select(e => e.ToString())));
        public virtual bool Equals(Set<T> other) => 
            other is SetElement<T> se ? 
                se.Equals(this) : 
                !(
                    other.elems.Any(e => !Contains(e)) ||    // Set equality: each set contains all other 
                    elems.Any(e => !other.Contains(e))       // set's elements
                );
    }

    // Not anymore a child of Set
    public class SetElement<T> : ISetItem<T>, IEquatable<ISetItem<T>>, IEquatable<SetElement<T>>
        where T : IEquatable<T>
    {
        public readonly T x;

        public SetElement(T x)
        {
            if (typeof(T) == typeof(Set<T>))
                throw new ElemPrimitive.InvalidSetElementException("The value of a set element can not " +
                    "be a set.");
            this.x = x;
        }
        public bool Equals(ISetItem<T> other) => other is SetElement<T> se && Equals(se);
        public bool Equals(SetElement<T> other) => x.Equals(other.x);
        public override string ToString() => string.Format("{0}", x);
        public override int GetHashCode() => x.GetHashCode();
    }

    // Nuple serves only as a common base class for TupleElement and Tuple.
    // This Nuple is a SetItem.
    public abstract class Nuple<T> : ISetItem<T>, IEquatable<Nuple<T>>
        where T : IEquatable<T>
    {
        public bool Equals(Nuple<T> other) => this is TupleElement<T> thisTe && other is TupleElement<T> otherTe && 
            thisTe.Equals(otherTe);
    }

    public class TupleElement<T> : Nuple<T>, IEquatable<TupleElement<T>>
        where T : IEquatable<T>
    {
        public readonly T x;
        public TupleElement(T x) => this.x = x;
        public bool Equals(TupleElement<T> other) => x.Equals(other.x);
        public override string ToString() => x.ToString();
    }

    public class Tuple1<T> : Nuple<T>, IEquatable<Tuple1<T>>
        where T : IEquatable<T>
    {
        public readonly Nuple<T> a;
        public Tuple1(Nuple<T> a) => this.a = a;

        public Set<T> AsSet()
        {
            var set = new Set<T>();
            set.Add(
                a is TupleElement<T> te ?
                    new SetElement<T>(te.x) :
                (a is System.Tuple<T> ?
                    new SetElement<T>(default) :
                (ISetItem<T>)((Tuple1<T>)a).AsSet())
            // Parou
            );
            return set;
        }
        // Returns whether it is a one-uple, pair, triple, etc...
        public virtual int Degree()
        {
            // TODO: recursive checking of b element
            return 1;
        }
        public bool Equals(Tuple1<T> other) => a.Equals(other.a);

        public override string ToString() => $"({a})";
    }

    // Single-type tuple
    public class Tuple<T> : Tuple1<T>, IEquatable<Tuple<T>>
        where T : IEquatable<T>
    {
        public readonly Nuple<T> b;
        public Tuple(Nuple<T> a, Nuple<T> b) : base(a) => this.b = b;

        // The constructor determines the number of elements.
        public Tuple(Nuple<T> a, Nuple<T> b, Nuple<T> c) : this(a, new Tuple<T>(b, c)) { }
        public Tuple(Nuple<T> a, Nuple<T> b, Nuple<T> c, Nuple<T> d) : this(a, new Tuple<T>(b,
            new Tuple<T>(c, d)))
        { }
        public Tuple(Nuple<T> a, Nuple<T> b, Nuple<T> c, Nuple<T> d, Nuple<T> e) : this(a, new Tuple<T>(b,
            new Tuple<T>(c, new Tuple<T>(d, e))))
        { }

        // Isto deve ser recursivo... pois as estruturas das tuplas podem ser diferentes.
        // Uma tupla pode ter uma tupla onde a outra tupla tem um elemento.
        // Parece que aqui haverá problema... pois teremos que checar igualdade de Tuples e 
        // TupleElements, que têm membros diferentes... Poderia ser implementado na Nuple e 
        // checando os tipos e só compara os tipos iguais?
        public bool Equals(Tuple<T> other) => a.Equals(other.a) && b.Equals(other.b);
        public override string ToString() => string.Format("({0}, {1})", a, b);
    }

    public class WrongDegreeTupleException : Exception { }

    // A Relation is a Set of Tuples because there are many Tuples in the Relation.
    // On a tuple of Sets intensionally, a relation means every tuple, of same size as the suple of sets, constructible 
    // with elements from both sets.
    public abstract class Relation1<T> : Set<Tuple1<T>> 
        where T : IEquatable<T>
    {
        public virtual bool Symmetric { get; } = true;
        public virtual bool Reflexive { get; } = false;
        public virtual bool Transitive { get; } = true;
        public Relation1(bool symmetric = false, bool reflexive = false, bool transitive = false)
        {
            Symmetric = symmetric;
            Reflexive = reflexive;
            Transitive = transitive;
        }
        public override void Add(ISetItem<Tuple1<T>> elem)
        {
            // Element-only set.
            if (!(elem is SetElement<Tuple1<T>>)) 
                throw new ElemPrimitive.ElementOnlySetException();

            base.Add(elem);
        }
    }

    public abstract class Relation<T> : Relation1<T>
        where T : IEquatable<T>
    {
        public Relation(bool symmetric = false, bool reflexive = false, bool transitive = false) : base(symmetric,
            reflexive, transitive) { }

        /// <summary>
        /// Adds pairs of related elements to the relation
        /// </summary>
        /// <param name="elem">The pairs of related elements</param>
        public override void Add(ISetItem<Tuple1<T>> elem)
        {
            //// Element-only set.
            //if (!(elem is SetElement<Tuple1<T>> setElem))
            //    throw new ElementOnlySetException();

            //// Pairs only.
            //setElem.x is Tupl

            //if (setElem.x.Degree() != 1)
            //    throw new WrongDegreeTupleException();

            //base.Add(elem);

            //var tuple = setElem.x;

            // PAROU
            //if (Symmetric)
            //{
            //    base.Add(new Tuple<T, T>(tuple.b, tuple.a));
            //}

            //if (Reflexive)
            //{
            //    base.Add(new Tuple<T, T>(tuple.a, tuple.a));
            //    base.Add(new Tuple<T, T>(tuple.b, tuple.b));
            //}

            //if (Transitive)
            //{
            //    // For all x with which b relates, inserts (a, x)
            //    // For all x which are related to a, inserts (x, b)
            //    var xWithB = elems
            //        .Cast<SetElement<Tuple2<T, T>>>()
            //        .Where(e => e.x.a.Equals(tuple.b))
            //        .Select(e => e.x.b).ToList();
            //    var xWithA = elems
            //        .Cast<SetElement<Tuple2<T, T>>>()
            //        .Where(e => e.x.b.Equals(tuple.a))
            //        .Select(e => e.x.a).ToList();
            //    foreach (var x in xWithB)
            //        base.Add(new Tuple2<T, T>(tuple.a, x));
            //    foreach (var x in xWithA)
            //        base.Add(new Tuple2<T, T>(x, tuple.b));
            //}
        }
    }

    public static class Extensions
    {
        /// <summary>
        /// Converts a tuple element to a 1-tuple set element
        /// </summary>
        public static SetElement<Tuple1<T>> SetElement<T>(this TupleElement<T> te)
            where T : IEquatable<T> => new SetElement<Tuple1<T>>(new Tuple1<T>(te));
    }
}
