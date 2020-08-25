using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Collections.Generic;

namespace LimitsTests
{
    using Limits.Continuity;

    [TestClass]
    public partial class ContinuityTests
    {
        [TestMethod]
        public void FiniteFunctionImageTest()
        {
            var fun1 = new FiniteFunction<int, int>(
                x => 2 * x,
                new int[] { 1, 2, 3, 4 }
            );
            var image = fun1.Image().ToArray();
            Assert.IsTrue(image.SequenceEqual(new int[] { 2, 4, 6, 8 }));
        }

        [TestMethod]
        public void ContinuityTest1()
        {
            var fun1 = new FiniteFunction<int, int>(
                x => 2 * x,
                new int[] { 1, 2, 3, 4 }.OrderBy(e => e)
            );
            for (int epsilon = 1; epsilon < 10; ++epsilon) 
                fun1.Domain.ToList().ForEach(x =>
                    Assert.IsTrue(fun1.IsDiscontinuousAt(x, epsilon)));
        }
    }
}
