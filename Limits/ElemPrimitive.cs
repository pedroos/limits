using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Limits.ElemPrimitive
{
    using Limits.ElementTyped;

    // The elements of this set are sets.
    // After elements (either sets or set elements) are added, they can't be retrieved 
    // individually, because they can't be referenced individually (a set is not positional). 
    // Elements can only (internally) be read sequentially. This is not so useful externally 
    // however as to find a specific element it'd be necessary to test every element for equality. 
    // Thus, the caller has to keep references to elements. It can be tested then whether a set 
    // contains a certain element.

    // To be able to verify a set for presence of elements (sets or set elements), set 
    // equality will have to be defined. A set should never equal a set element, even if 
    // the set only contains that element. A set element should equal another set element 
    // with the same value (x). A set should equal another set all elements of which are 
    // equal to its elements (either sets or set elements, order not considered).

    // Only the set elements have values.
    public class Set<T> : IEquatable<Set<T>> 
        where T : IEquatable<T>
    {
        protected readonly HashSet<Set<T>> elems;
        public event Func<Set<T>,bool> BeforeElementAdded;
        public event Action<Set<T>> ElementAdded;
        public int ElemCount { get; private set; } // Count only non-set elements
        public Set()
        {
            ElemCount = 0;
            elems = new HashSet<Set<T>>();
        }
        public virtual void Add(Set<T> elem)
        {
            if (elems.Contains(elem)) return;
            if (BeforeElementAdded != null && !BeforeElementAdded.Invoke(elem)) return;
            elems.Add(elem);
            ElemCount = elems.OfType<SetElement<T>>().Count();
            ElementAdded?.Invoke(elem);
        }
        public bool Contains(Set<T> set)
        {
            // Element comparison
            if (set is SetElement <T>)
            {
                return elems.OfType<SetElement<T>>().Any(e => e.Equals((SetElement<T>)set));
            }
            // Set comparison
            return elems.Any(e => e.Equals(set));
        }
        public override string ToString()
        {
            return string.Format("{{{0}}}", string.Join(", ", elems.Select(e => e.ToString())));
        }
        public virtual bool Equals(Set<T> other)
        {
            if (other is SetElement<T>)
                return ((SetElement<T>)other).Equals(this);
            if (other.elems.Any(e => !Contains(e))) return false;
            if (elems.Any(e => !other.Contains(e))) return false;
            return true;
        }
    }

    // A set element is akin to a singleton set. It is a set but also has a special function. 
    // It is necessary because without this type, the "first set" could not be befined; sets 
    // could only have been made of sets and no set would have an element.
    // A "set element" is any element in a set which is not a set in itself.
    // A set element only has one value and cannot contain sets.
    public class SetElement<T> : Set<T>, IEquatable<SetElement<T>> 
        where T : IEquatable<T>
    {
        public readonly T x;

        public SetElement(T x) 
        {
            if (typeof(T) == typeof(Set<T>))
                throw new InvalidSetElementException("The value of a set element can not " + 
                    "be a set.");
            this.x = x;
        }
        public override void Add(Set<T> elem)
        {
            throw new InvalidSetOperationException("A set element can not have elements added " + 
                "to it.");
        }
        public override bool Equals(Set<T> other)
        {
            return other is SetElement<T> ? Equals((SetElement<T>)other) : false;
        }
        public bool Equals(SetElement<T> other)
        {
            return x.Equals(other.x);
        }
        public override string ToString()
        {
            return string.Format("{0}", x);
        }
        public override int GetHashCode()
        {
            return x.GetHashCode();
        }
    }

    public class InvalidSetElementException : Exception
    {
        public InvalidSetElementException(string message) : base(message) { }
    }

    public class InvalidSetOperationException : Exception
    {
        public InvalidSetOperationException(string message) : base(message) { }
    }

    /// <summary>
    /// An exception to be thrown when a set is added to a set which should only hold elements
    /// </summary>
    public class ElementOnlySetException : Exception
    {

    }

    #region Single-type tuples

    // Nuple serves only as a common base class for TupleElement and Tuple.
    public abstract class Nuple<T>
        where T : IEquatable<T> { }

    public class TupleElement<T> : Nuple<T>
        where T : IEquatable<T>
    {
        public readonly T x;
        public TupleElement(T x)
        {
            this.x = x;
        }

        public override string ToString()
        {
            return x.ToString();
        }
    }

    // Note: this is a single-type tuple
    public class Tuple<T> : Nuple<T>, IEquatable<Tuple<T>>
        where T : IEquatable<T>
    {
        public readonly Nuple<T> a;
        public readonly Nuple<T> b;
        public Tuple(Nuple<T> a, Nuple<T> b)
        {
            this.a = a;
            this.b = b;
        }
        // The constructor determines the number of elements.
        public Tuple(Nuple<T> a, Nuple<T> b, Nuple<T> c) : this(a, new Tuple<T>(b, c)) { }
        public Tuple(Nuple<T> a, Nuple<T> b, Nuple<T> c, Nuple<T> d) : this(a, new Tuple<T>(b,
            new Tuple<T>(c, d))) { }
        public Tuple(Nuple<T> a, Nuple<T> b, Nuple<T> c, Nuple<T> d, Nuple<T> e) : this(a, new Tuple<T>(b,
            new Tuple<T>(c, new Tuple<T>(d, e)))) { }

        public Set<T> AsSet()
        {
            var set = new Set<T>();
            set.Add(
                a is TupleElement<T> ?
                    new SetElement<T>(((TupleElement<T>)a).x) : 
                (a is System.Tuple<T> ? 
                    new SetElement<T>(default(T)) : 
                ((Tuple<T>)a).AsSet())
                // Parou
            );
            return set;
        }

        public bool Equals(Tuple<T> other)
        {
            // Isto deve ser recursivo... pois as estruturas das tuplas podem ser diferentes.
            // Uma tupla pode ter uma tupla onde a outra tupla tem um elemento.
            // Parece que aqui haverá problema... pois teremos que checar igualdade de Tuples e 
            // TupleElements, que têm membros diferentes... Poderia ser implementado na Nuple e 
            // checando os tipos e só compara os tipos iguais?
            return a.Equals(other.a) && b.Equals(other.b);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", a, b);
        }
    }

    #endregion

    public class Relation2<T> : Set<Tuple2<T, T>>
        where T : IEquatable<T>
    {
        public bool Symmetric { get; }
        public bool Reflexive { get; }
        public bool Transitive { get; }
        public Relation2(bool symmetric = false, bool reflexive = false, bool transitive = false)
        {
            Symmetric = symmetric;
            Reflexive = reflexive;
            Transitive = transitive;
        }
        public override void Add(Set<Tuple2<T, T>> elem) {
            // Element-only set.
            if (!(elem is SetElement<Tuple2<T, T>>)) 
                throw new ElementOnlySetException();

            base.Add(elem);

            var tuple = ((SetElement<Tuple2<T, T>>)elem).x;

            if (Symmetric)
            {
                base.Add(new Tuple2<T, T>(tuple.b, tuple.a));
            }

            if (Reflexive)
            {
                base.Add(new Tuple2<T, T>(tuple.a, tuple.a));
                base.Add(new Tuple2<T, T>(tuple.b, tuple.b));
            }

            if (Transitive)
            {
                // For all x with which b relates, inserts (a, x)
                // For all x which are related to a, inserts (x, b)
                var xWithB = elems
                    .Cast<SetElement<Tuple2<T, T>>>()
                    .Where(e => e.x.a.Equals(tuple.b))
                    .Select(e => e.x.b).ToList();
                var xWithA = elems
                    .Cast<SetElement<Tuple2<T, T>>>()
                    .Where(e => e.x.b.Equals(tuple.a))
                    .Select(e => e.x.a).ToList();
                foreach (var x in xWithB)
                    base.Add(new Tuple2<T, T>(tuple.a, x));
                foreach (var x in xWithA)
                    base.Add(new Tuple2<T, T>(x, tuple.b));
            }
        }
    }
}
