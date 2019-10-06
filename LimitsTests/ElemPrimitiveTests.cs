using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Collections.Generic;

namespace LimitsTests.ElemPrimitive
{
    using Limits.ElemPrimitive;

    [TestClass]
    public partial class LimitsTests
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
            var s = new Set<int>(new List<Set<int>> { });
            Assert.AreEqual("{}", s.ToString());
        }

        [TestMethod]
        public void SetToStringTest()
        {
            var s = new Set<int>(new List<Set<int>>
            {
                new SetElement<int>(1),
                new SetElement<int>(2)
            });
            Assert.AreEqual("{1, 2}", s.ToString());
        }

        [TestMethod]
        public void UnarySetToStringTest()
        {
            var s = new Set<int>(new List<Set<int>>
            {
                new Set<int>(new List<Set<int>>
                {
                    new Set<int>(new List<Set<int>>
                    {
                        new SetElement<int>(1)
                    })
                })
            });
            Assert.AreEqual("{{{1}}}", s.ToString());
        }

        [TestMethod]
        public void TupleToStringTest()
        {
            var p = new Tuple<int>(
                new TupleElement<int>(1), 
                new TupleElement<int>(2)
            );
            Assert.AreEqual("(1, 2)", p.ToString());

            p = new Tuple<int>(
                new TupleElement<int>(1),
                new TupleElement<int>(2),
                new TupleElement<int>(3)
            );
            Assert.AreEqual("(1, (2, 3))", p.ToString());

            p = new Tuple<int>(
                new TupleElement<int>(1),
                new TupleElement<int>(2),
                new TupleElement<int>(3),
                new TupleElement<int>(4)
            );
            Assert.AreEqual("(1, (2, (3, 4)))", p.ToString());

            p = new Tuple<int>(
                new TupleElement<int>(1),
                new TupleElement<int>(2),
                new TupleElement<int>(3),
                new TupleElement<int>(4),
                new TupleElement<int>(5)
            );
            Assert.AreEqual("(1, (2, (3, (4, 5))))", p.ToString());
        }

        [TestMethod]
        public void TupleEqualsTest()
        {

        }
    }
}
