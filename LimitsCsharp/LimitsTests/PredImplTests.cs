using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Collections.Generic;

namespace LimitsTests
{
    using Limits.PredImpl;

    [TestClass]
    public partial class PredImplTests
    {
        [TestMethod]
        public void Test1()
        {
            // An implication is a function which results the same as its outer function.
            //var a = new Predicate<int>(i => i > 2);
            //var b = new Predicate<int>(i => i > 0);
            // a implies b. If a is true, b is automatically true.
            Func<int, bool> a = x => x > 2;
            var b = F.Iff(a);
            Assert.IsTrue(a(3) && b(3));
            Assert.IsFalse(a(3) && !b(3));
            Assert.IsTrue(!a(1) && !b(1));
            Assert.IsFalse(!a(1) && b(1));
        }
    }
}
