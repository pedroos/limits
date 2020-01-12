using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Collections.Generic;

namespace LimitsTests
{
    [TestClass]
    public partial class CSharpTests
    {
        struct EmptyStruct { }
        struct SampleStruct
        {
            public int Value;
        }

        [TestMethod]
        public void StructEqualityTest()
        {
            Assert.AreEqual(new EmptyStruct(), new EmptyStruct());
            Assert.IsTrue(new EmptyStruct().Equals(new EmptyStruct()));

            Assert.IsTrue(new SampleStruct().Equals(new SampleStruct()));
            Assert.IsTrue(new SampleStruct() { Value = 1 }.Equals(new SampleStruct()
            { Value = 1 }));
            Assert.IsFalse(new SampleStruct() { Value = 1 }.Equals(new SampleStruct()
            { Value = 2 }));
        }
    }
}
