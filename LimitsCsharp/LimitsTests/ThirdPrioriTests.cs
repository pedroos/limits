using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Collections.Generic;

namespace LimitsTests
{
    using Limits.ThirdPriori;

    [TestClass]
    public partial class ThirdPrioriTests
    {
        [TestMethod]
        public void ElementaryRelationTest()
        {
            var s = new Set<int>();
            s.Add(2);
            s.Add(4);

            var r = new Relation2<int>();
            r.Add(new OrderedTuple2<int>(2, 4));
            s.relations.Add(r);

            Assert.AreEqual(true, s.Is<Relation2<int>>(2, 4));
            Assert.AreEqual(false, s.Is<Relation2<int>>(2, 3));
        }

        [TestMethod]
        public void SymmetricRelationTest()
        {
            var s = new Set<int>();
            s.Add(2);
            s.Add(4);

            var r = new SymmetricRelation2<int>();
            r.Add(new OrderedTuple2<int>(2, 4));
            s.relations.Add(r);

            Assert.AreEqual(true, s.Is<SymmetricRelation2<int>>(4, 2));
            Assert.AreEqual(true, s.Is<SymmetricRelation2<int>>(2, 4));
            Assert.AreEqual(false, s.Is<SymmetricRelation2<int>>(2, 3));
        }

        [TestMethod]
        public void ReflexiveRelationTest()
        {
            var s = new Set<int>();
            s.Add(2);

            // In the reflexive check, it is not necessary to have the elements 
            // in the relation, as every element in the set will participate (?)
            var r = new ReflexiveRelation2<int>();
            r.Add(new OrderedTuple2<int>(2, 4));
            s.relations.Add(r);

            Assert.AreEqual(true, s.Is<ReflexiveRelation2<int>>(2, 2));
            Assert.AreEqual(true, s.Is<ReflexiveRelation2<int>>(4, 4));
            Assert.AreEqual(false, s.Is<ReflexiveRelation2<int>>(1, 1));
            Assert.AreEqual(false, s.Is<ReflexiveRelation2<int>>(2, 3));
        }

        [TestMethod]
        public void SymmetricReflexiveRelationTest()
        {
            var s = new Set<int>();
            s.Add(2);
            s.Add(4);

            var r = new SymmetricReflexiveRelation2<int>();
            r.Add(new OrderedTuple2<int>(2, 4));
            s.relations.Add(r);

            Assert.AreEqual(true, s.Is<SymmetricReflexiveRelation2<int>>(4, 2));
            Assert.AreEqual(true, s.Is<SymmetricReflexiveRelation2<int>>(2, 4));
            Assert.AreEqual(false, s.Is<SymmetricReflexiveRelation2<int>>(2, 3));

            Assert.AreEqual(true, s.Is<SymmetricReflexiveRelation2<int>>(2, 2));
            Assert.AreEqual(false, s.Is<SymmetricReflexiveRelation2<int>>(1, 1));
            Assert.AreEqual(false, s.Is<SymmetricReflexiveRelation2<int>>(2, 3));
        }

        [TestMethod]
        public void TransitiveRelationTest()
        {
            var s = new Set<int>();
            s.Add(2);
            s.Add(3);
            s.Add(4);

            var r = new TransitiveRelation2<int>();
            r.Add(new OrderedTuple2<int>(2, 3));
            r.Add(new OrderedTuple2<int>(3, 4));
            s.relations.Add(r);

            Assert.AreEqual(true, s.Is<TransitiveRelation2<int>>(2, 4));
            Assert.AreEqual(true, s.Is<TransitiveRelation2<int>>(2, 3));
            Assert.AreEqual(false, s.Is<TransitiveRelation2<int>>(2, 5));
            Assert.AreEqual(false, s.Is<TransitiveRelation2<int>>(4, 2));
        }
    }
}
