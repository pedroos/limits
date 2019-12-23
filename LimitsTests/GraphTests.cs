using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Collections.Generic;

namespace LimitsTests.Graph
{
    using Limits.Graph;
    using Limits.ElemPrimitive;

    [TestClass]
    public partial class LimitsTests
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
            var elems = graph.Item1.Elems;
            Assert.AreEqual(elems.Count(), 2);
            // A set element can be a set
            Assert.IsInstanceOfType(elems.ElementAt(0), typeof(SetElement<Node>));
            Assert.AreSame((elems.ElementAt(0) as SetElement<Node>).x, node1);
            Assert.IsInstanceOfType(elems.ElementAt(1), typeof(SetElement<Node>));
            Assert.AreSame((elems.ElementAt(1) as SetElement<Node>).x, node2);
        }
    }
}
