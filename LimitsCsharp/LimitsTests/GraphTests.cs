using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Collections.Generic;

namespace LimitsTests
{
    using Limits.Graph;
    using Limits.ElemPrimitive;

    [TestClass]
    public partial class GraphTests
    {
        [TestMethod]
        public void Test1()
        {
            var node1 = new Node();
            var node2 = new Node();
            var nodeRelation1 = new Tuple<Node>(
                new TupleElement<Node>(node1), 
                new TupleElement<Node>(node2)
            );
            var vertices = new Set<Node>();
            vertices.Add(new SetElement<Node>(node1));
            vertices.Add(new SetElement<Node>(node2));
            var edges = new Set<Tuple<Node>>();
            edges.Add(new SetElement<Tuple<Node>>(nodeRelation1));
            var graph = new Graph(vertices, edges);
            Assert.AreEqual(graph.Item1.ElemCount, 2);
            Assert.IsTrue(graph.Item1.Contains(new SetElement<Node>(node1)));
            Assert.IsTrue(graph.Item1.Contains(new SetElement<Node>(node2)));
        }
    }
}
