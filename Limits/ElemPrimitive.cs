using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Limits.ElemPrimitive
{
    public class Set<T>
    {
        protected readonly List<Set<T>> elems;

        // Constructor only called by the elements constructor
        protected Set()
        {
            elems = new List<Set<T>>();
        }

        public Set(IEnumerable<Set<T>> elems) : this()
        {
            this.elems.AddRange(elems);
        }

        public override string ToString()
        {
            return string.Format("{{{0}}}", string.Join(", ", elems.Select(e => e.ToString())));
        }
    }

    // A set element is akin to a singleton set. It is a set but also has a special function. 
    // It is necessary because without this type, the "first set" could not be befined; sets 
    // could only have been made of sets and no set would have an element.
    public class SetElement<T> : Set<T>, IEquatable<SetElement<T>> 
        where T : IEquatable<T>
    {
        // Breaks SOLID?
        public readonly T x;

        public SetElement(T x) 
        {
            this.x = x;
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
        public Tuple(Nuple<T> a, Nuple<T> b, Nuple<T> c) : this(a, new Tuple<T>(b, c)) { }
        public Tuple(Nuple<T> a, Nuple<T> b, Nuple<T> c, Nuple<T> d) : this(a, new Tuple<T>(b,
            new Tuple<T>(c, d))) { }
        public Tuple(Nuple<T> a, Nuple<T> b, Nuple<T> c, Nuple<T> d, Nuple<T> e) : this(a, new Tuple<T>(b,
            new Tuple<T>(c, new Tuple<T>(d, e)))) { }

        public Set<T> AsSet()
        {
            return new Set<T>(new List<Set<T>>
            {
                a is TupleElement<T> ?
                    new SetElement<T>(((TupleElement<T>)a).x) : 
                (a is System.Tuple<T> ? 
                    new SetElement<T>(default(T)) : 
                ((Tuple<T>)a).AsSet()), 
                // Parou
            });
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
}
