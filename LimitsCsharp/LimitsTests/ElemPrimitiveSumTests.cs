using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Collections.Generic;

namespace LimitsTests
{
    using Limits.ElemPrimitiveSum;
    //using IntTupleElement = Limits.ElemPrimitive.TupleElement<int>;
    //using Limits.ElementTyped;

    [TestClass]
    public partial class ElemPrimitiveSumTests
    {
        [TestMethod]
        public void ElementToStringTest()
        {
            var s = new SetElement<int>(1);
            Assert.AreEqual("1", s.ToString());
        }

        [TestMethod]
        public void EmptyToStringTest()
        {
            var s = new Set<int>();
            Assert.AreEqual("{}", s.ToString());
        }

        [TestMethod]
        public void SetToStringTest()
        {
            var s = new Set<int>();
            s.Add(new SetElement<int>(1));
            s.Add(new SetElement<int>(2));
            Assert.AreEqual("{1, 2}", s.ToString());
        }

        [TestMethod]
        public void NestedSetToStringTest()
        {
            var s1 = new Set<int>();
            var s2 = new Set<int>();
            var s3 = new Set<int>();
            s3.Add(new SetElement<int>(1));
            s2.Add(s3);
            s1.Add(s2);
            Assert.AreEqual("{{{1}}}", s1.ToString());
        }

        [TestMethod]
        public void ElementEqualsPositiveTest()
        {
            var se1 = new SetElement<int>(1);
            var se2 = new SetElement<int>(1);
            Assert.IsTrue(se1.Equals(se2));
        }

        [TestMethod]
        public void ElementEqualsNegativeTest()
        {
            var se1 = new SetElement<int>(1);
            var se2 = new SetElement<int>(2);
            Assert.IsFalse(se1.Equals(se2));
        }

        [TestMethod]
        public void ElementEqualsSetNegativeTest()
        {
            var se1 = new SetElement<int>(1);
            var s1 = new Set<int>();
            Assert.IsFalse(se1.Equals(s1));
        }

        [TestMethod]
        public void SetContainsElementPositiveTest()
        {
            var s1 = new Set<int>();
            var se1 = new SetElement<int>(1);
            s1.Add(se1);
            Assert.IsTrue(s1.Contains(se1));
        }

        [TestMethod]
        public void SetContainsElementNegativeTest()
        {
            var s1 = new Set<int>();
            var se1 = new SetElement<int>(1);
            Assert.IsFalse(s1.Contains(se1));
        }

        [TestMethod]
        public void SetContainsElementNegativeNestedTest()
        {
            var s1 = new Set<int>();
            var s2 = new Set<int>();
            var se1 = new SetElement<int>(1);
            s2.Add(se1);
            Assert.IsFalse(s1.Contains(se1));
        }

        [TestMethod]
        public void SetEqualsPositiveTest()
        {
            var s1 = new Set<int>();
            var se1 = new SetElement<int>(1);
            s1.Add(se1);
            var s2 = new Set<int>();
            s2.Add(se1);
            Assert.IsTrue(s1.Equals(s2));
        }

        [TestMethod]
        public void SetEqualsPositiveNoOrderTest()
        {
            var s1 = new Set<int>();
            var se1 = new SetElement<int>(1);
            var se2 = new SetElement<int>(2);
            s1.Add(se1);
            s1.Add(se2);
            var s2 = new Set<int>();
            s2.Add(se2);
            s2.Add(se1);
            Assert.IsTrue(s1.Equals(s2));
        }

        [TestMethod]
        public void SetEqualsPositiveSubsetTest()
        {
            var s1 = new Set<int>();
            var s2 = new Set<int>();
            s1.Add(s2);

            var s3 = new Set<int>();
            var s4 = new Set<int>();
            s3.Add(s4);

            Assert.IsTrue(s1.Equals(s3));
        }

        [TestMethod]
        public void SetEqualsPositiveSubsetWithElementsTest()
        {
            var s1 = new Set<int>();
            var s2 = new Set<int>();
            var se1 = new SetElement<int>(1);
            s2.Add(se1);
            s1.Add(s2);

            var s3 = new Set<int>();
            var s4 = new Set<int>();
            var se2 = new SetElement<int>(1);
            s4.Add(se2);
            s3.Add(s4);

            Assert.IsTrue(s1.Equals(s3));
        }

        [TestMethod]
        public void SetEqualsNegativeTest()
        {
            var s1 = new Set<int>();
            var se1 = new SetElement<int>(1);
            s1.Add(se1);
            var s2 = new Set<int>();
            Assert.IsFalse(s1.Equals(s2));
        }

        [TestMethod]
        public void SetEqualsElementNegativeTest()
        {
            var s1 = new Set<int>();
            var se1 = new SetElement<int>(1);
            Assert.IsFalse(s1.Equals(se1));
        }

        [TestMethod]
        public void SetContainsSetPositiveTest()
        {
            var s1 = new Set<int>();
            var s2 = new Set<int>();
            var se1 = new SetElement<int>(1);
            s2.Add(se1);
            s1.Add(s2);
            Assert.IsTrue(s1.Contains(s2));
        }

        [TestMethod]
        public void SetContainsSetNegativeTest()
        {
            var s1 = new Set<int>();
            var s2 = new Set<int>();
            Assert.IsFalse(s1.Contains(s2));
        }

        [TestMethod]
        public void SetContainsSetNegativeNestedTest()
        {
            var s1 = new Set<int>();
            var s2 = new Set<int>();
            var s3 = new Set<int>();
            s2.Add(s3);
            s1.Add(s2);
            // S3 is one level too deep
            Assert.IsFalse(s1.Contains(s3));
        }

        class SymmetricRelation<T> : Relation2<T> 
            where T : IEquatable<T>
        {
            public SymmetricRelation() : base(symmetric: true) { }
        }

        //[TestMethod]
        //public void RelationSymmetricTest()
        //{
        //    var r = new SymmetricRelation<int>();
        //    var setElement = new TupleElement<int>(1).SetElement();
        //    r.Add(setElement);
        //    Assert.IsTrue(r.Contains(setElement));
        //}

        [TestMethod]
        public void RelationSymmetricEquality1Test()
        {
            var r1 = new SymmetricRelation<int>();
            var setElement = new TupleElement<int>(1).SetElement();
            r1.Add(setElement);
            var r2 = new SymmetricRelation<int>();
            r2.Add(setElement);
            Assert.IsTrue(r1.Equals(r2));
        }

        [TestMethod]
        public void RelationSymmetricEquality2Test()
        {
            var r1 = new SymmetricRelation<int>();
            r1.Add(new TupleElement<int>(1).SetElement());
            var r2 = new SymmetricRelation<int>();
            r2.Add(new TupleElement<int>(1).SetElement());
            Assert.IsTrue(r1.Equals(r2));
        }

        [TestMethod]
        public void LabelTest()
        {
            dynamic label1 = new { a = 1, b = new { x = 2, y = 3 } };
            dynamic label2 = new { a = 1, b = new { x = 2, y = 3 } };
            Assert.AreEqual(label1, label2);
            dynamic label3 = new { a = 1, b = new { x = 2, y = 3 }, c = 4 };
            Assert.AreNotEqual(label1, label3);
        }
    }
}
