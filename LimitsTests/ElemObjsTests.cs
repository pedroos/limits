using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Collections.Generic;

namespace LimitsTests
{
    using Limits.ElemOjbs;

    [TestClass]
    public partial class ElemOjbsTests
    {
        [TestMethod]
        public void DetachedInequalityTest()
        {
            var set1 = new Set();
            Assert.AreEqual(set1, set1); // Succeeds because short-circuited in the test 
                // library; we shouldn't use AreEqual()
            Assert.IsFalse(set1.Equals(set1)); // Equals() verification
            var set2 = new Set();
            Assert.AreNotEqual(set1, set2); // Hashcode verification
            Assert.IsFalse(set1.Equals(set2));
        }
        
        [TestMethod]
        public void ElementInequalityTest()
        {
            var set = new Set();
            var setEl1 = new EmptySetElement();
            var setEl2 = new EmptySetElement();
            set.Add(setEl1);
            set.Add(setEl2);
            Assert.IsFalse(set.Enumerable.First().Equals(set.Enumerable.Skip(1).First()));
        }

        [TestMethod]
        public void EnumerationTest()
        {
            var set = new Set();
            var setEl1 = new EmptySetElement();
            var setEl2 = new EmptySetElement();
            set.Add(setEl1);
            set.Add(setEl2);
            Assert.IsTrue(set.Enumerable.First().Equals(setEl1));
            Assert.IsTrue(set.Enumerable.Skip(1).First().Equals(setEl2));
        }

        [TestMethod]
        public void DistinctParentEqualityTest()
        {
            var set1 = new Set();
            var setEl = new EmptySetElement();
            set1.Add(setEl);
            var set2 = new Set();
            set2.Add(setEl);
            Assert.IsTrue(set1.Enumerable.First().Equals(set2.Enumerable.First()));
        }

        [TestMethod]
        public void ObjectPropertyTest()
        {
            var graph = new Graph();
            Assert.IsTrue(graph.Enumerable.First().Equals(graph.VerticesSet));
        }

        [TestMethod]
        public void ObjectChildElementTest()
        {
            var graph = new Graph();
            var vertex = new Vertex();
            graph.VerticesSet.Add(vertex);
            Assert.IsTrue(graph.VerticesSet.Enumerable.First().Equals(vertex));
        }

        [TestMethod]
        public void WrongTypeObjectTest()
        {
            var graph = new Graph();
            var tuple2 = new Tuple2();
            // Wrong type Sets should be accepted as elements
            graph.VerticesSet.Add(tuple2);
            Assert.IsInstanceOfType(graph.VerticesSet.Enumerable.First(), typeof(Tuple2));
            Assert.IsTrue(tuple2.Equals(graph.VerticesSet.Enumerable.First()));
        }

        [TestMethod]
        public void MaxSizeTest()
        {
            var tuple2 = new Tuple2();
            tuple2.Add(new Set());
            tuple2.Add(new Set());
            Assert.ThrowsException<MaxSetSizeException>(() => tuple2.Add(new Set()));
        }

        // TODO: traverse a Set tree
    }
}
