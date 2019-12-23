using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Limits.ElemPrimitive
{
    // The elements of this set are sets.
    // After elements (either sets or set elements) are added, they can't be retrieved 
    // individually, because they can't be referenced individually (a set is not positional). 
    // Elements can only be read sequentially. This is not so useful however as to find a 
    // certain element it'd be necessary to test every element for equality. The caller has 
    // to keep references to elements.
    // Only the set elements have values.

    // To be able to verify a set for presence of elements (sets or set elements), set 
    // equality will have to be defined. A set should never equal a set element, even if 
    // the set only contains that element. A set element should equal another set element 
    // with the same value (x). A set should equal another set all elements of which are 
    // equal to its elements (order not considered).
    public class Set<T> : IEquatable<Set<T>> 
        where T : IEquatable<T>
    {
        protected readonly HashSet<Set<T>> elems;
        
        public event Action<Set<T>> ElementAdded;
        public IEnumerable<Set<T>> Elems
        {
            get { 
                return elems.AsEnumerable();
            }
        }
        public Set()
        {
            elems = new HashSet<Set<T>>();
        }
        public virtual void Add(Set<T> elem)
        {
            if (!elems.Add(elem)) return;
            ElementAdded?.Invoke(elem);
        }
        // Don't verify elements of elements which are sets (i.e. subsets); only direct set 
        // elements.
        public bool Contains(Set<T> set)
        {
            // Element comparison
            if (set is SetElement <T>)
            {
                return elems.OfType<SetElement<T>>().Any(e => (e as SetElement<T>).Equals(
                    set as SetElement<T>));
            }
            // Set comparison
            return elems.Any(e => e.Equals(set));
        }
        public override string ToString()
        {
            return string.Format("{{{0}}}", string.Join(", ", elems.Select(e => e.ToString())));
        }
        // Sets in comparison must contain all elements of each other
        public virtual bool Equals(Set<T> other)
        {
            if (other is SetElement<T>)
                return (other as SetElement<T>).Equals(this);
            if (other.Elems.Any(e => !Contains(e as SetElement<T>))) return false;
            if (Elems.Any(e => !other.Contains(e as SetElement<T>))) return false;
            return true;
        }
    }

    public class InvalidSetElementException : Exception {
        public InvalidSetElementException(string message) : base(message) { }
    }

    public class InvalidSetOperationException : Exception
    {
        public InvalidSetOperationException(string message) : base(message) { }
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
        //public override bool ContainsElement(Set<T> elem)
        //{
        //    return false;
        //}
        public override bool Equals(Set<T> other)
        {
            return false;
        }
        public bool Equals(SetElement<T> other)
        {
            return x.Equals(other.x);
        }
        public override string ToString()
        {
            return string.Format("{0}", x);
        }
    }

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

    public class Relation2<T> : Set<ElementTyped.Tuple2<T,T>> 
        where T : IEquatable<T>
    {

    }
}
