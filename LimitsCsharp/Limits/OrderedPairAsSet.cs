using System;
using System.Collections.Generic;
using System.Text;

namespace Limits.OrderedPairAsSet
{
    public class Set<T> : IEquatable<Set<T>>
        where T : IEquatable<T>
    {
        protected List<T> elems;
        public List<T> relations;

        public Set(IEnumerable<T> elems)
        {
            this.elems = new List<T>();
            this.elems.AddRange(elems);

            relations = new List<T>();
        }

        /// <summary>
        /// Create a set with an element x
        /// </summary>
        public static Set<T> FromX(T x)
        {
            var s = new Set<T>(new List<T> { x });
            return s;
        }

        public int Count
        {
            get { return elems.Count; }
        }

        public bool Equals(Set<T> other)
        {
            throw new NotImplementedException();
        }
    }

    namespace Limits.OrderedPairAsSet.Kuratowski
    {
        public class OrderedPair<T> : Set<Set<T>>
            where T : IEquatable<T>
        {
            public OrderedPair(T a, T b) : base(
                // Make a list of the form: [{a}, {a, b}]
                new List<Set<T>>()
                {
                Set<T>.FromX(a),
                new Set<T>(new List<T> { a, b })
                }
            )
            { }
        }

        public class OrderedTriple<T> : Set<Set<T>>
            where T : IEquatable<T>
        {
            public OrderedTriple(T a, T b, T c) : base(
                new List<Set<T>>()
                {
                    //new OrderedPair<T>(a, b), 
                    //new OrderedPair<> // Problema: não conseguimos definir um tipo 
                    // para este segundo par ordenado ({{{a},{a,b}},c}}, c tem tipo 
                    // diferente de {{a},{a,b}}
                }
            ) { }
        }

        //public class OrderedTuple<T> : Set<Set<T>> 
        //    where T : IEquatable<T>
        //{
        //    public OrderedTuple(IEnumerable<T> elems) : base(

        //    ) { }
        //}
    }

    namespace Limits.OrderedPairAsSet.Hausdorff
    {
        public class OrderedPair<T> : Set<Set<T>> where 
            T : IEquatable<T>
        {
            public OrderedPair(T a, T b) : base(
                // Make a list of the form: [{a,1},{b,2}]
                new List<Set<T>>()
                {
                    //new Set<T> // Não funciona: não conseguimos 
                    // definir um tipo para os subconjuntos do par. Só funcionaria com 
                    // elementos tipo inteiro (int)
                }
            ) { }
        }
    }
}
