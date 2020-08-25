using System;
using System.Collections.Generic;
using System.Text;

namespace Limits.NewDefs
{
    public class TypedSet<T>
    {
        // TODO: requisitos
        // - Elems tem de não inserir duplicatas
        public IEnumerable<T> Elems { get; }
        public void Add(T elem)
        {

        }
    }

    public class UntypedSet : TypedSet<object> { }

    public class OrderedTypedSet<T> : TypedSet<T> { }

    public class OrderedUntypedSet : UntypedSet { }

    // Necessarily ordered
    public class TypedTuple2<T1,T2>
    {
        // TODO: requisitos
        // - Ser um tipo único para todos os tamanhos
        public T1 Get1 { get; }
        public void Set1(T1 elem1) { }
        public T2 Get2 { get; }
        public void Set2(T2 elem1) { }
    }

    // Same as an untyped list
    public class UntypedTuple2 : TypedTuple2<object, object> { }

    // A set with repetitions
    public class TypedList<T>
    {
        public IEnumerable<T> Elems { get; }
        public void Add(T elem)
        {

        }
    }

    // Same as an unordered untyped list
    public class UnorderedUntypedTuple2 : TypedList<object> { }
}
