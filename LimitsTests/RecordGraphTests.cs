using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Collections.Generic;

namespace LimitsTests.RecordGraph
{
    using Limits.RecordGraph;
    using Limits.ElementTyped;

    [TestClass]
    public partial class LimitsTests
    {
        [TestMethod]
        public void Valid()
        {
            var vertex1 = new Vertex();
            var vertex2 = new Vertex();
            var vertex3 = new Vertex();
            var graph = new Graph();
            graph.VertexSet.Add(vertex1);
            graph.VertexSet.Add(vertex2);
            graph.VertexSet.Add(vertex3);
            graph.EdgeSet.Add(new Tuple2<Vertex,Vertex>(vertex1, vertex2));
            graph.EdgeSet.Add(new Tuple2<Vertex,Vertex>(vertex1, vertex3));
            graph.EdgeSet.Add(new Tuple2<Vertex,Vertex>(vertex3, vertex1));
        }

        [TestMethod]
        public void InvalidInexistantVertex()
        {
            var vertex1 = new Vertex();
            var vertex2 = new Vertex();
            var graph = new Graph();
            graph.VertexSet.Add(vertex1);
            Assert.ThrowsException<InexistantVertexException>(() => graph.EdgeSet.Add(
                new Tuple2<Vertex,Vertex>(vertex1, vertex2)));
        }
    }
}
