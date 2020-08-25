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
    public partial class RecordGraphTests
    {
        [TestMethod]
        public void ValidTest()
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
        public void VertexElementEqualityPositiveTest()
        {
            var vertex1 = new Vertex();
            var el1 = new SetElement<Vertex>(vertex1);
            var el2 = new SetElement<Vertex>(vertex1);
            Assert.IsTrue(el1.Equals(el2));
        }

        [TestMethod]
        public void VertexElementEqualityNegativeTest()
        {
            var vertex1 = new Vertex();
            var el1 = new SetElement<Vertex>(vertex1);
            var vertex2 = new Vertex();
            var el2 = new SetElement<Vertex>(vertex2);
            Assert.IsFalse(el1.Equals(el2));
        }

        [TestMethod]
        public void EdgeElementEqualityPositiveTest()
        {
            var vertex1 = new Vertex();
            var vertex2 = new Vertex();
            var el1 = new SetElement<Tuple2<Vertex, Vertex>>(new Tuple2<Vertex, Vertex>(vertex1, 
                vertex2));
            var el2 = new SetElement<Tuple2<Vertex, Vertex>>(new Tuple2<Vertex, Vertex>(vertex1, 
                vertex2));
            Assert.IsTrue(el1.Equals(el2));
        }

        [TestMethod]
        public void EdgeElementEqualityNegativeTest()
        {
            var vertex1 = new Vertex();
            var vertex2 = new Vertex();
            var vertex3 = new Vertex();
            var el1 = new SetElement<Tuple2<Vertex, Vertex>>(new Tuple2<Vertex, Vertex>(vertex1,
                vertex2));
            var el2 = new SetElement<Tuple2<Vertex, Vertex>>(new Tuple2<Vertex, Vertex>(vertex2,
                vertex3));
            Assert.IsFalse(el1.Equals(el2));
        }

        [TestMethod]
        public void InvalidInexistantVertexTest()
        {
            var vertex1 = new Vertex();
            var vertex2 = new Vertex();
            var graph = new Graph();
            graph.VertexSet.Add(vertex1);
            Assert.ThrowsException<InexistantVertexException>(() => graph.EdgeSet.Add(
                new Tuple2<Vertex,Vertex>(vertex1, vertex2)));
        }

        [TestMethod]
        public void ElementOnlySetTest()
        {
            var graph = new Graph();
            var vertexSet = new Set<Vertex>();
            Assert.ThrowsException<ElementOnlySetException>(() => graph.VertexSet.Add(vertexSet));
            var edgeSet = new Set<Tuple2<Vertex, Vertex>>();
            Assert.ThrowsException<ElementOnlySetException>(() => graph.EdgeSet.Add(edgeSet));
        }

        [TestMethod]
        public void OrderTest()
        {
            var vertex1 = new Vertex();
            var vertex2 = new Vertex();
            var graph = new Graph();
            graph.VertexSet.Add(vertex1);
            graph.VertexSet.Add(vertex2);
            Assert.AreEqual(2, graph.Order);
        }

        [TestMethod]
        public void SizeTest()
        {
            var vertex1 = new Vertex();
            var vertex2 = new Vertex();
            var vertex3 = new Vertex();
            var vertex4 = new Vertex();
            var graph = new Graph();
            graph.VertexSet.Add(vertex1);
            graph.VertexSet.Add(vertex2);
            graph.VertexSet.Add(vertex3);
            graph.VertexSet.Add(vertex4);
            graph.EdgeSet.Add(new Tuple2<Vertex, Vertex>(vertex1, vertex2));
            graph.EdgeSet.Add(new Tuple2<Vertex, Vertex>(vertex2, vertex3));
            Assert.AreEqual(2, graph.Size);
        }

        [TestMethod]
        public void ExistingVertexTest()
        {
            var graph = new Graph();
            var vertex1 = new Vertex();
            graph.VertexSet.Add(vertex1);
            int order = graph.Order;
            graph.VertexSet.Add(vertex1);
            Assert.AreEqual(graph.Order, order);
        }

        [TestMethod]
        public void ExistingEdgeTest()
        {
            var graph = new Graph();
            var vertex1 = new Vertex();
            var vertex2 = new Vertex();
            graph.VertexSet.Add(vertex1);
            graph.VertexSet.Add(vertex2);
            graph.EdgeSet.Add(new Tuple2<Vertex, Vertex>(vertex1, vertex2));
            int size = graph.Size;
            graph.EdgeSet.Add(new Tuple2<Vertex, Vertex>(vertex1, vertex2));
            Assert.AreEqual(graph.Size, size);
        }

        [TestMethod]
        public void SymmetricSizeTest()
        {
            // Size does not observe symmetry
            var graph = new Graph();
            var vertex1 = new Vertex();
            var vertex2 = new Vertex();
            graph.VertexSet.Add(vertex1);
            graph.VertexSet.Add(vertex2);
            graph.EdgeSet.Add(new Tuple2<Vertex, Vertex>(vertex1, vertex2));
            Assert.AreEqual(1, graph.Size);
        }
    }
}
