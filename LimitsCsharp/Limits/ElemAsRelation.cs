using System;
using System.Collections.Generic;
using System.Text;

namespace Limits.ElemAsRelation
{
    using Limits.ElemPrimitive;

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
                ((Tuple1<T>)a).AsSet())
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

        public override void Add(Set<Tuple1<T>> elem)
        {
            // Element-only set.
            if (!(elem is SetElement<Tuple1<T>>)) 
                throw new ElementOnlySetException();

            base.Add(elem);
        }
    }

    public class WrongDegreeTupleException : Exception { }

    public abstract class Relation2<T> : Relation1<T>
        where T : IEquatable<T>
    {
        public override bool Symmetric { get; }
        public override bool Reflexive { get; }
        public override bool Transitive { get; }
        public Relation2(bool symmetric = false, bool reflexive = false, bool transitive = false)
        {
            Symmetric = symmetric;
            Reflexive = reflexive;
            Transitive = transitive;
        }

        /// <summary>
        /// Adds pairs of related elements to the relation
        /// </summary>
        /// <param name="elem">The pairs of related elements</param>
        public override void Add(Set<Tuple1<T>> elem)
        {
            // Element-only set.
            if (!(elem is SetElement<Tuple<T>> setElem))
                throw new ElementOnlySetException();

            if (setElem.x.Degree() != 1)
                throw new WrongDegreeTupleException();

            base.Add(elem);

            var tuple = setElem.x;

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
}
