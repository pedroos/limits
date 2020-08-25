using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Collections.Generic;

namespace LimitsTests
{
    using Limits.RecordGraph;
    using Limits.ElemPrimitive;
    using Limits.ElementTyped;

    // Node: same as vertex
    // Edge: two vertices
    // Arc: directed edge
    // Adjacent vertices: in the same edge
    // Vertex degree: count of incident edges
    // Path: sequence of connected edges
    // Trail: path without repeated edges
    // Circuit: trail from a vertex to itself with at least three edges. With one single vertex 
    //   would be a reflecting edge, and with two edges isn't possible. Trail from a vertex to 
    //   itself with more than one edge. May repeat edges.
    // Cycle: circuit without repeating edges.
    // Regular graph: all vertices with same degree.
    // Complete graph: all vertices are adjacent.
    // Bridge: edge only connection between two other edges.
    // Component: largest connected subgraphs of a graph (separated by disconnections)

    [TestClass]
    public partial class RecordGraphExercises
    {
        [TestMethod]
        public void IrreflexiveSymmetricRelation()
        {
            var vertex1 = new Vertex();
            var vertex2 = new Vertex();
            var vertex3 = new Vertex();
            var vertex4 = new Vertex();
            var vertex5 = new Vertex();
            var graph = new Graph();
            graph.VertexSet.Add(vertex1);
            graph.VertexSet.Add(vertex2);
            graph.VertexSet.Add(vertex3);
            graph.VertexSet.Add(vertex4);
            graph.VertexSet.Add(vertex5);
            graph.EdgeSet.Add(new Tuple2<Vertex, Vertex>(vertex1, vertex2));
            graph.EdgeSet.Add(new Tuple2<Vertex, Vertex>(vertex1, vertex4));
            graph.EdgeSet.Add(new Tuple2<Vertex, Vertex>(vertex1, vertex5));
            graph.EdgeSet.Add(new Tuple2<Vertex, Vertex>(vertex2, vertex3));
            graph.EdgeSet.Add(new Tuple2<Vertex, Vertex>(vertex3, vertex5));
            graph.EdgeSet.Add(new Tuple2<Vertex, Vertex>(vertex4, vertex5));
        }
    }
}
