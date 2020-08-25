using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Collections.Generic;

namespace LimitsTests
{
    using Limits.QuantDescrImpl;

    [TestClass]
    public partial class QuantDescrImplTests
    {
        [TestMethod]
        public void IrrCollTest()
        {
            var coll = new IrrColl<int, Two, string, One, DateTime, One>();
            coll.AddA(1);
            coll.AddA(1); // Same element, ignored
            coll.AddA(2);
            Assert.ThrowsException<InvalidElementException>(() => coll.AddA(3));
            coll.AddB("a");
            coll.AddB("a");
            Assert.ThrowsException<InvalidElementException>(() => coll.AddB("b"));
            coll.AddC(new DateTime(1000000));
            coll.AddC(new DateTime(1000000));
            Assert.ThrowsException<InvalidElementException>(() => coll.AddC(new DateTime(2000000)));

            Assert.IsTrue(new int[] { 1, 2 }.SequenceEqual(coll.GetA()));
            Assert.IsTrue(new string[] { "a" }.SequenceEqual(coll.GetB()));
            Assert.IsTrue(new DateTime[] { new DateTime(1000000) }.SequenceEqual(coll.GetC()));
        }

        // TODO: Copiar o teste acima
        //[TestMethod]
        //public void IrrCollNTest()
        //{
        //}
    }
}
