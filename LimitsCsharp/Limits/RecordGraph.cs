using System;
using System.Collections.Generic;
using System.Text;

namespace Limits.RecordGraph
{
    using Limits.ElemPrimitive;
    using Limits.ElemOjbsUnf;
    public struct Vertex : IEquatable<Vertex>
    {
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

    class EdgeSet : Relation2<Vertex> { }

    public class Graph
    {
        public Set<Vertex> VertexSet { get; }
        public Relation2<Vertex> EdgeSet { get; }
        public int Order { get; private set; } = 0;
        public int Size { get; private set; } = 0;
        public Graph()
        {
            VertexSet = new Set<Vertex>();
            EdgeSet = new EdgeSet();
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
