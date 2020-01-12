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
        public void UnformalizedEqualityTest()
        {
            var set1 = new Set();
            Assert.AreEqual(set1, set1); // Succeeds because short-circuited in the test 
                                         // library; we shouldn't use AreEqual()
            Assert.AreEqual(set1.GetHashCode(), set1.GetHashCode());
            Assert.IsTrue(set1.Equals(set1)); // Equals() verification
            var set2 = new Set();
            Assert.IsTrue(set1.Equals(set2)); // Unformalized sets always match
        }

        [TestMethod]
        public void FormalizedInequalityTest()
        {
            var set1 = new Set();
            var set2 = new Set();
            set1.Add(set2);
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

        //[TestMethod]
        //public void WalkDownTest1()
        //{
        //    var set1 = new Set();
        //    var set2 = new Set();
        //    var set3 = new Set();
        //    set2.Add(set3, false);
        //    set1.Add(set2, false);
        //    var walk1 = set1.WalkDown();
        //    Assert.AreEqual(3, walk1.Count());
        //    Assert.IsTrue(walk1.First().Equals(set1));
        //    Assert.IsTrue(walk1.Skip(1).First().Equals(set2));
        //    Assert.IsTrue(walk1.Skip(2).First().Equals(set3));
        //}
    }
}
