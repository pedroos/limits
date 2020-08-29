using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Collections.Generic;

namespace LimitsTests
{
    using Limits.ElemOjbsUnf;

    [TestClass]
    public partial class ElemOjbsUnfTests
    {
        // (Irregular) Traversal (graph) precedes equality.

        [TestMethod]
        public void WalkUpTest1()
        {
            var set1 = new Set(regularity: false);
            var ure1 = new Urelement();
            set1.Add(ure1);
            var walkUp = ure1.WalkUp();
            Assert.AreEqual(1, walkUp.Count());
            Assert.AreSame(set1, walkUp.First()); // This test is outside the system
        }

        // Equality precedes regularity.

        [TestMethod]
        public void EmptySetEqualityTest()
        {
            var set1 = new Set();
            var set2 = new Set();
            Assert.IsTrue(set1.Equals(set2));
            set1.Add(new Set());
            Assert.IsFalse(set1.Equals(set2));
        }

        //[TestMethod]
        //public void DistinctParentEqualityTest()
        //{
        //    var set1 = new Set();
        //    var set2 = new Set();
        //    set1.Add(set2);
        //    var set3 = new Set();
        //    set3.Add(set2);
        //    Assert.IsTrue(set1.Enumerable.First().Equals(set3.Enumerable.First()));
        //}

        // Regularity.

        [TestMethod]
        public void EmptySetIrregularityTest()
        {
            // TODO: está falhando porque valor por elementos não está implementado
            Assert.ThrowsException<CycleException>(() => new Set().Add(new Set()));
        }

        [TestMethod]
        public void EmptySetIrregularityTest2()
        {
            // Infinite recursion when walking
            new Set(regularity: false).Add(new Set());
        }

        [TestMethod]
        public void EmptySetIrregularityTest3()
        {
            var set1 = new Set();
            var set2 = new Set(regularity: false);
            set1.Add(set2);
            // Infinite recursion when walking
            set2.Add(set1);
        }

        // TODO: hangs?
        //[TestMethod]
        //public void EmptySetIndirectIrregularityTest()
        //{
        //    var set1 = new Set(regularity: false);
        //    var set2 = new Set();
        //    set1.Add(set2);
        //    set2.Add(set1);
        //    var set3 = new Set();
        //    Assert.ThrowsException<CycleException>(() => set2.Add(set1));
        //}

        //[TestMethod]
        //public void EnumerationTest()
        //{
        //    var set1 = new Set();
        //    var set2 = new Set();
        //    var set3 = new Set();
        //    set1.Add(set2);
        //    set1.Add(set3);
        //    Assert.IsTrue(set1.Enumerable.First().Equals(set2));
        //    Assert.IsTrue(set1.Enumerable.Skip(1).First().Equals(set3));
        //}

        //[TestMethod]
        //public void ObjectChildElementTest()
        //{
        //    var graph = new Graph();
        //    var vertex = new Vertex();
        //    graph.VerticesSet.Add(vertex);
        //    Assert.IsTrue(graph.VerticesSet.Enumerable.First().Equals(vertex));
        //}

        [TestMethod]
        public void MaxSizeTest()
        {
            var set = new Set(maxSize: 2);
            set.Add(new Set());
            set.Add(new Set());
            Assert.ThrowsException<MaxSetSizeException>(() => set.Add(new Set()));
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

        [TestMethod]
        public void CycleTest1Positive()
        {

            var set1 = new Set();
            Assert.ThrowsException<CycleException>(() => set1.Add(set1));
        }

        [TestMethod]
        public void CycleTest2Positive()
        {
            var set1 = new Set();
            var set2 = new Set();
            set1.Add(set2);
            Assert.ThrowsException<CycleException>(() => set2.Add(set1));
        }
        
        [TestMethod]
        public void CycleTest2Negative2()
        {
            var set1 = new Set();
            var set2 = new Set();
            set1.Add(set2);
            var set3 = new Set();
            set3.Add(set2); // ok
            Assert.ThrowsException<CycleException>(() => set2.Add(set1));
        }
    }
}
