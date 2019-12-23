using System;
using System.Collections.Generic;
using System.Text;

namespace Limits.RecordGraph
{
    public class Vertex : IEquatable<Vertex>
    {
        public bool Equals(Vertex other)
        {
            return other.GetHashCode().Equals(GetHashCode());
        }
        // Implicit conversion to set element
        public static implicit operator ElemPrimitive.SetElement<Vertex>(Vertex v) => 
            new ElemPrimitive.SetElement<Vertex>(v);
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
        public ElemPrimitive.Set<Vertex> VertexSet { get; }
        public ElemPrimitive.Relation2<Vertex> EdgeSet { get; }
        public Graph()
        {
            VertexSet = new ElemPrimitive.Set<Vertex>();
            EdgeSet = new ElemPrimitive.Relation2<Vertex>();
            EdgeSet.ElementAdded += (elem) =>
            {
                var edge = (elem as ElemPrimitive.SetElement<ElementTyped.Tuple2
                    <Vertex,Vertex>>).x;
                if (!VertexSet.Contains(edge.a))
                    throw new InexistantVertexException(edge.a);
                if (!VertexSet.Contains(edge.b))
                    throw new InexistantVertexException(edge.b);
            };
        }
    }
}
