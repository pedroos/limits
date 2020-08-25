using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Collections.Generic;

namespace LimitsTests
{
    using Limits.ElemAsSet;

    [TestClass]
    public partial class ElemAsSetTests
    {
        [TestMethod]
        public void SingletonToStringTest()
        {
            var s = new Singleton<int>(1);
            Assert.AreEqual("1", s.ToString());
        }

        [TestMethod]
        public void EmptyToStringTest()
        {
            var s = new Empty<int>();
            Assert.AreEqual("0", s.ToString());
        }

        [TestMethod]
        public void SetToStringTest()
        {
            var s = new Set<int>(new List<Set<int>>
            {
                new Singleton<int>(1),
                new Singleton<int>(2)
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
                        new Singleton<int>(1)
                    })
                })
            });
            Assert.AreEqual("{{{1}}}", s.ToString());
        }

        [TestMethod]
        public void OrderedPairToStringTest()
        {
            var p = new OrderedPair<int>(
                new Singleton<int>(1), 
                new Singleton<int>(2)
            );
            Assert.AreEqual("{1, {1, 2}}", p.ToString());
        }

        [TestMethod]
        public void OrderedTripleToStringTest()
        {
            var p = new OrderedTriple<int>(
                new Singleton<int>(1),
                new Singleton<int>(2),
                new Singleton<int>(3)
            );
            Assert.AreEqual("{{1, {1, 2}}, {{1, {1, 2}}, 3}}", p.ToString());
        }
    }
}
