using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Collections.Generic;

namespace LimitsTests.Intensional
{
    using Limits.Extensional;

    [TestClass]
    public partial class LimitsTests
    {
        [TestMethod]
        public void FunctionDuplicateElementInImage()
        {
            var f = new Function<int, int>();
            f.Add(new OrderedTuple2<int, int>(1, 2));
            Assert.ThrowsException<DuplicateElementInFunctionImageException>(
                () => f.Add(new OrderedTuple2<int, int>(1, 3))
            );
        }

        [TestMethod]
        public void MinimumTest()
        {
            var s = new TotallyOrderedSet<Number>();
            s.Add(new Number(2));
            s.Add(new Number(4));
            s.Add(new Number(7));
            Assert.IsTrue(s.Minimum().Equals(new Number(2)));
        }

        [TestMethod]
        public void MaximumTest()
        {
            OrderedSet<Number> s = new OrderedSet<Number>();
            s.Add(new Number(2));
            s.Add(new Number(4));
            s.Add(new Number(7));
            Assert.IsTrue(s.Maximum().Equals(new Number(7)));
        }

        class ReflexiveSet<T> : Set<T>, ReflexiveRelation2<T>
            where T : IEquatable<T>
        {
            public override bool Is(T a, T b)
            {
                if (a.Equals(b)) return true;
                return false;
            }
        }

        class SymmetricSet<T> : Set<T>, SymmetricRelation2<T>
            where T : IEquatable<T>
        {
            public override bool Is(T a, T b)
            {
                if (((Set<T>)this).Is(a, b)) return true;

                if (relationExtension[typeof(SymmetricRelation2<T>)]
                    .Any(e => e.Equals(new OrderedTuple2<T>(b, a))))
                    return true;
                return false;
            }
        }

        [TestMethod]
        public void ElementaryRelationTest()
        {
            var s = new Set<int>();
            s.Add(2);
            s.Add(4);

            var l = new List<OrderedTuple2<int>>();
            l.Add(new OrderedTuple2<int>(2, 4));
            s.AddRelationExtension<Relation2<int>>(l);

            Assert.IsTrue(s.Is(2, 4));
        }

        [TestMethod]
        public void ReflexiveRelationTest()
        {
            var s = new ReflexiveSet<int>();
            s.Add(2);

            Assert.IsTrue(s.Is(2, 2));
        }

        [TestMethod]
        public void SymmetricRelationTest()
        {
            var s = new SymmetricSet<int>();
            s.Add(2);
            s.Add(4);

            var l = new List<OrderedTuple2<int>>();
            l.Add(new OrderedTuple2<int>(2, 4));
            s.AddRelationExtension<SymmetricRelation2<int>>(l);

            Assert.IsTrue(s.Is(2, 4));
            //Assert.IsTrue(s.Is(4, 2));
        }
    }
}
