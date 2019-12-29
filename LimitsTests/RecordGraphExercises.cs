using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Collections.Generic;

namespace LimitsTests
{
    using Limits.RecordGraph;
    using Limits.ElemPrimitive;
    using Limits.ElementTyped;

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
