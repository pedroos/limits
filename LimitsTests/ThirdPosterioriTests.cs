using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Collections.Generic;

namespace LimitsTests.ThirdPosteriori
{
    using Limits.ThirdPosteriori;

    [TestClass]
    public partial class LimitsTests
    {   
        [TestMethod]
        public void ElementaryRelationTest()
        {
            var s = new Set<int>();
            s.elems.Add(2);
            s.elems.Add(4);

            var r = new Relation2<int>();
            r.elems.Add(new OrderedTuple2<int>(2, 4));
            s.relations.Add(r);

            Assert.AreEqual((typeof(RelationKindAttribute), true), s.Is<Relation2<int>>(2, 4));
            Assert.AreEqual((typeof(RelationKindAttribute), false), s.Is<Relation2<int>>(2, 3));
        }

        [TestMethod]
        public void SymmetricRelationTest()
        {
            var s = new Set<int>();
            s.elems.Add(2);
            s.elems.Add(4);

            var r = new SymmetricRelation2<int>();
            r.elems.Add(new OrderedTuple2<int>(2, 4));
            s.relations.Add(r);

            Assert.AreEqual((typeof(SymmetricRelationAttribute), true), 
                s.Is<SymmetricRelation2<int>>(4, 2));
            Assert.AreEqual((typeof(RelationKindAttribute), true), 
                s.Is<SymmetricRelation2<int>>(2, 4));
            Assert.AreEqual((typeof(RelationKindAttribute), false),
                s.Is<SymmetricRelation2<int>>(2, 3));
        }

        [TestMethod]
        public void ReflexiveRelationTest()
        {
            var s = new Set<int>();
            s.elems.Add(2);

            // In the reflexive check, it is not necessary to have the elements 
            // in the relation, as every element in the set will participate (?)
            var r = new ReflexiveRelation2<int>();
            s.relations.Add(r);

            Assert.AreEqual((typeof(ReflexiveRelationAttribute), true), 
                s.Is<ReflexiveRelation2<int>>(2, 2));
            Assert.AreEqual((typeof(RelationKindAttribute), false), 
                s.Is<ReflexiveRelation2<int>>(1, 1));
            Assert.AreEqual((typeof(RelationKindAttribute), false), 
                s.Is<ReflexiveRelation2<int>>(2, 3));
        }

        [TestMethod]
        public void SymmetricReflexiveRelationTest()
        {
            var s = new Set<int>();
            s.elems.Add(2);
            s.elems.Add(4);

            var r = new SymmetricReflexiveRelation2<int>();
            r.elems.Add(new OrderedTuple2<int>(2, 4));
            s.relations.Add(r);

            Assert.AreEqual((typeof(SymmetricRelationAttribute), true),
                s.Is<SymmetricReflexiveRelation2<int>>(4, 2));
            Assert.AreEqual((typeof(RelationKindAttribute), true),
                s.Is<SymmetricReflexiveRelation2<int>>(2, 4));
            Assert.AreEqual((typeof(RelationKindAttribute), false),
                s.Is<SymmetricReflexiveRelation2<int>>(2, 3));

            Assert.AreEqual((typeof(ReflexiveRelationAttribute), true),
                s.Is<SymmetricReflexiveRelation2<int>>(2, 2));
            Assert.AreEqual((typeof(RelationKindAttribute), false),
                s.Is<SymmetricReflexiveRelation2<int>>(1, 1));
            Assert.AreEqual((typeof(RelationKindAttribute), false),
                s.Is<SymmetricReflexiveRelation2<int>>(2, 3));
        }

        [TestMethod]
        public void TransitiveRelationTest()
        {
            var s = new Set<int>();
            s.elems.Add(2);
            s.elems.Add(3);
            s.elems.Add(4);

            var r = new TransitiveRelation2<int>();
            r.elems.Add(new OrderedTuple2<int>(2, 3));
            r.elems.Add(new OrderedTuple2<int>(3, 4));
            s.relations.Add(r);

            Assert.AreEqual((typeof(TransitiveRelationAttribute), true),
                s.Is<TransitiveRelation2<int>>(2, 4));
            Assert.AreEqual((typeof(RelationKindAttribute), true),
                s.Is<TransitiveRelation2<int>>(2, 3));
            Assert.AreEqual((typeof(RelationKindAttribute), false),
                s.Is<TransitiveRelation2<int>>(2, 5));
            Assert.AreEqual((typeof(RelationKindAttribute), false),
                s.Is<TransitiveRelation2<int>>(4, 2));
        }

        [TestMethod]
        public void TransitiveSymmetricRelationTest()
        {
            var s = new Set<int>();
            s.elems.Add(2);
            s.elems.Add(3);
            s.elems.Add(4);

            var r = new TransitiveSymmetricRelation2<int>();
            r.elems.Add(new OrderedTuple2<int>(2, 3));
            r.elems.Add(new OrderedTuple2<int>(3, 4));
            s.relations.Add(r);

            Assert.AreEqual((typeof(TransitiveRelationAttribute), true),
                s.Is<TransitiveSymmetricRelation2<int>>(2, 4));
            Assert.AreEqual((typeof(RelationKindAttribute), true),
                s.Is<TransitiveSymmetricRelation2<int>>(2, 3));
            Assert.AreEqual((typeof(RelationKindAttribute), false),
                s.Is<TransitiveSymmetricRelation2<int>>(2, 5));
            Assert.AreEqual((typeof(RelationKindAttribute), false),
                s.Is<TransitiveSymmetricRelation2<int>>(4, 2));

            Assert.AreEqual((typeof(SymmetricRelationAttribute), true),
                s.Is<TransitiveSymmetricRelation2<int>>(3, 2));
            Assert.AreEqual((typeof(SymmetricRelationAttribute), true),
                s.Is<TransitiveSymmetricRelation2<int>>(4, 3));
        }
    }
}
