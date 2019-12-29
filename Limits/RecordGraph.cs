using System;
using System.Collections.Generic;
using System.Text;

namespace Limits.RecordGraph
{
    using Limits.ElemPrimitive;
    public struct Vertex : IEquatable<Vertex>
    {
        // PAROU: Problem: equality should never be by instance. But there is usually no instrinsic 
        // identifying value in objects to use as id. We should be able to express intention to 
        // create separate objects. 
        // Maybe a New() method to replace using new. But should be able to disable construction by 
        // new: private constructor.
        // This should be only for value-less objects... Otherwise two New()ed objects with the same 
        // value would still be the same object.
        // The problem is... even without a value, we should be able to refer to the same object 
        // from another new instance, and we shouldn't have to reference 'internal values'. We must 
        // create an idiom to refer universally to objects, as many times as we want (including 
        // creating new 'copies' or representations of existing objects -- still being the same 
        // object).
        // Not even time is an identifier... beacause two objects could be created simultaneously. 
        // So, such a register shouldn't be enumerable. It is a set of objects.
        // How do we refer to objects, long after we've created them? We label them. Labels should 
        // be different. But labels should be arbitrary structures, not machine values. We could 
        // type sets by label type, for example. We could create an easy-to-use sequential label 
        // type which models "remembering objects by sequence of creation". And use it for most 
        // cases. But how else could we possibly need to label a set?
        // We could use dynamic.
        // Restating the problem: we need to identify objects. We could use values, but not all 
        // objects have values. We could use references instead, but new instances with the same 
        // value would be different objects. We need an identifier. But it can't be a machine value 
        // because we'd have to treat it like so and store it in a variable to access it repeatedly. 
        // We can model how we refer to unique objects, by labeling them with arbitrary labels. 
        // The dynamic feature fits well, and we can use a standard "order of creation" ({n = x}) 
        // label type to label objects where a simple sequential "remembering" is enough, or more 
        // complicated labels should it be helpful (we're helping our memories).
        // Thus, when each object is created, it must be assigned a label; the collection of created 
        // objects is an untyped map of labels to objects (a map's keys form a set) (thus labels are 
        // external to the objects). It is globally accessible and ethernal.
        // A variation could be that labels are a property on sets (and the global is a set). Is 
        // this less realistic? A set's label could be changed either way. There is a difference: 
        // this way, the global set will have a label as well. This is less realistic. Labels are 
        // "external" objects to the reality simulated by the sets, actually.
        public bool Equals(Vertex other)
        {
            return other.GetHashCode().Equals(GetHashCode());
        }
        // Implicit conversion to set element
        public static implicit operator SetElement<Vertex>(Vertex v) => new SetElement<Vertex>(v);
    }
    public class InexistantVertexException : Exception 
    {
        public Vertex Vertex { get; }
        public InexistantVertexException(Vertex vertex)
        {
            Vertex = vertex;
        }
    }
    public class Graph
    {
        public Set<Vertex> VertexSet { get; }
        public Relation2<Vertex> EdgeSet { get; }
        public int Order { get; private set; } = 0;
        public int Size { get; private set; } = 0;
        public Graph()
        {
            VertexSet = new Set<Vertex>();
            EdgeSet = new Relation2<Vertex>();
            VertexSet.ElementAdded += (elem) =>
            {
                if (!(elem is SetElement<Vertex>))
                    throw new ElementOnlySetException();
                Order = VertexSet.ElemCount;
            };
            EdgeSet.BeforeElementAdded += (elem) =>
            {
                // PAROU: implementar tipos de relações para considerar ou não edges simétricos na 
                // composição de um edge.
                if (!(elem is SetElement<ElementTyped.Tuple2<Vertex, Vertex>>))
                    throw new ElementOnlySetException();
                var edge = ((SetElement<ElementTyped.Tuple2<Vertex,Vertex>>)elem).x;
                if (!VertexSet.Contains(edge.a))
                    throw new InexistantVertexException(edge.a);
                if (!VertexSet.Contains(edge.b))
                    throw new InexistantVertexException(edge.b);
                return true;
            };
            EdgeSet.ElementAdded += (elem) =>
            {
                Size = EdgeSet.ElemCount;
            };
        }
    }
}
