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
            var graph = new Graph(
                new Set<Node>(new List<SetElement<Node>> {
                    new SetElement<Node>(node1),
                    new SetElement<Node>(node2)
                }), 
                new Set<Tuple<Node>>(new List<SetElement<Tuple<Node>>> {
                    new SetElement<Tuple<Node>>(nodeRelation1)
                }));
            var elems = graph.a.Elems;
            elems.MoveNext();
            // A set element can be a set
            Assert.IsInstanceOfType(elems.Current, typeof(SetElement<Node>));
            Assert.AreSame((elems.Current as SetElement<Node>).x, node1);
            elems.MoveNext();
            Assert.IsInstanceOfType(elems.Current, typeof(SetElement<Node>));
            Assert.AreSame((elems.Current as SetElement<Node>).x, node2);
            Assert.IsFalse(elems.MoveNext());
        }
    }
}
