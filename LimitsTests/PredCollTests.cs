using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Collections.Generic;

namespace LimitsTests
{
    using Limits.PredColl;

    [TestClass]
    public partial class PredCollTests
    {
        [TestMethod]
        public void PCBasicTest()
        {
            var coll = new PredicateCollection();
            var pred = Predicates.ElementTypePredicate<int>();

            // Invalid element
            Assert.ThrowsException<InvalidElementException>(() => coll.Add(pred, ""));

            // New predicate and element, ok
            coll.Add(pred, 1);
            Assert.AreEqual(1, coll.Get(pred));

            // Existing predicate and element, ignored
            coll.Add(pred, 1);
            Assert.AreEqual(1, coll.Get(pred));

            // New predicate, existing element, element ignored
            coll.Add((Predicate)((obj) => obj.GetType() != typeof(char)), 1);

            // Match querying by each predicate
            Assert.AreEqual(coll.Get(pred), coll.Get((obj) => obj.GetType() != typeof(char)));

            // Existing predicate, new element, not ok
            Assert.ThrowsException<ExistingElementException>(() => coll.Add(pred, 2));
        }

        [TestMethod]
        public void PCReferenceTypeTest()
        {
            var coll = new PredicateCollection();
            var pred = Predicates.ElementTypePredicate<string>();
            Assert.ThrowsException<InvalidElementException>(() => coll.Add(pred, 1));
            string elem = "a";
            coll.Add(pred, elem);
            Assert.AreSame(elem, coll.Get(pred));
            Assert.ThrowsException<ExistingElementException>(() => coll.Add(pred, "b"));
            Assert.IsTrue(coll.Valid());
        }

        [TestMethod]
        public void MPCBasicTest()
        {
            var coll = new ManyPredicateCollection();

            // The element predicate
            var pred = Predicates.ElementTypePredicate<int>();

            // The quantifier
            var quant = Quantifiers.All(pred);

            coll.Add(1);
            coll.AddQuant(quant);
            coll.AddQuant(quant); // Ignored

            // Invalid element
            Assert.ThrowsException<InvalidElementException>(() => coll.Add("a"));

            Assert.IsTrue(new int[] { 1 }.SequenceEqual(coll.Get(pred)));
        }

        [TestMethod]
        public void MPCQuantityTest()
        {
            var coll = new ManyPredicateCollection();
            var pred = Predicates.ElementTypePredicate<int>();
            var quant = Quantifiers.Count(pred, 2);
            Assert.ThrowsException<InvalidPredicateException>(() => coll.AddQuant(quant));
            coll.Add(1);
            Assert.ThrowsException<InvalidPredicateException>(() => coll.AddQuant(quant));
            coll.Add(2);
            coll.AddQuant(quant);
            Assert.IsTrue(new int[] { 1, 2 }.SequenceEqual(coll.Get(pred)));
            coll.Add(1, 2); // Repeated elements, ok
            Assert.ThrowsException<InvalidElementException>(() => coll.Add(3));
            Assert.IsTrue(new int[] { 1, 2 }.SequenceEqual(coll.Get(pred)));
        }

        [TestMethod]
        public void MPCPositionTest()
        {
            var coll = new ManyPredicateCollection();
            var pred = Predicates.ElementTypePredicate<int>();
            var quant = coll.BindPositionQuantifier(pred, 2, 3);

            // No elements
            Assert.ThrowsException<InvalidPredicateException>(() => coll.AddQuant(quant));

            coll.Add("a", "b");
            coll.AddToPos(("c", 1));
            coll.AddToPos(("d", 2));

            // Wrong type element
            Assert.ThrowsException<InvalidPredicateException>(() => coll.AddQuant(quant));
            coll.Remove("d");

            coll.AddToPos((1, 2));

            // Unfulfilled position
            Assert.ThrowsException<InvalidPredicateException>(() => coll.AddQuant(quant));

            coll.AddToPos((2, 3));

            // Satisfied
            coll.AddQuant(quant);

            // Replace position
            coll.AddToPos((4, 3));
            Assert.IsTrue(new object[] { 1, 4 }.SequenceEqual(coll.GetByPos(2, 3)));
            Assert.ThrowsException<InvalidElementException>(() => coll.AddToPos(("e", 3)));

            // Remove constraint
            coll.RemoveQuant(quant);

            // Add again
            coll.AddToPos(("e", 3));
            Assert.IsTrue(new object[] { 1, "e" }.SequenceEqual(coll.GetByPos(2, 3)));
        }

        struct CustomType1<T>
        {
            public T Value { get; set; }
            public bool Prop { get; set; }
        }

        [TestMethod]
        public void MPCPropertyTest1()
        {
            var coll = new ManyPredicateCollection();
            
            var typePred = Predicates.ElementTypePredicate<CustomType1<int>>();
            var typeQuant = Quantifiers.All(typePred);

            // Two equivalent predicates. The second one is converted to the first one.
            var propPred1 = new Predicate((object elem) => !(elem is CustomType1<int>) || 
                ((CustomType1<int>)elem).Value == 2);
            var propQuant1 = Quantifiers.All(propPred1);

            var propPred1Typed = new ForTypePredicate<CustomType1<int>>((CustomType1<int> elem) => elem.Value == 2)
                .ToPredicate();
            var propQuant1Typed = Quantifiers.All(propPred1Typed);

            var propPred2 = new Predicate((object elem) => ((CustomType1<int>)elem).Prop);
            var propQuant2 = Quantifiers.All(propPred2);

            coll.AddQuant(typeQuant);
            coll.AddQuant(propQuant1);
            coll.AddQuant(propQuant2);

            Assert.ThrowsException<InvalidElementException>(() => coll.Add(1));
            Assert.ThrowsException<InvalidElementException>(() => coll.Add(new CustomType1<int>() { 
                Value = 2, Prop = false }));
            Assert.ThrowsException<InvalidElementException>(() => coll.Add(new CustomType1<int>() {
                Value = 1, Prop = true }));
            coll.Add(new CustomType1<int>() { Value = 2, Prop = true });

            // Typed get and predicate
            Assert.IsTrue(new CustomType1<int>[] { new CustomType1<int>() { Value = 2, Prop = true } }
                .SequenceEqual(coll.Get(typePred)));
        }

        [TestMethod]
        public void PredicateConversionTest()
        {
            var coll = new object[] { 1, 2 };
            var pred = Predicates.ElementTypePredicate<int>();
            Assert.IsTrue(Quantifiers.All(pred)(coll));
            var pred2 = pred.ToPredicate();
            Assert.IsTrue(Quantifiers.All(pred2)(coll));
            var pred3 = pred2.ToElementTypePredicate<double>();
            Assert.IsFalse(Quantifiers.All(pred3)(coll));
            var pred3b = pred2.ToElementTypePredicate<int>();
            Assert.IsTrue(Quantifiers.All(pred3b)(coll));
        }

        class T
        {
            public bool A { get; set; } = false;
            public bool B { get; set; } = false;
            public bool C { get; set; } = false;
            public bool D { get; set; } = false;
            public bool E { get; set; } = false;
        }

        [TestMethod]
        public void TheoremTest1()
        {
            // Suppes p. 62
            // No Episcopalian (A) or Presbyterian (B) is a Unitarian (C). John was a Unitarian. Therefore, he was 
            // not an Episcopalian.
            var pred1 = new ForTypePredicate<T>((T x) => !(x.A && x.C));
            var pred2 = new ForTypePredicate<T>((T x) => !(x.B && x.C));
            var a = new T() { C = true };
            Func<bool> pro = () => !a.A;
            Assert.IsTrue(pred1(a) && pred2(a) && pro());

            // All scientists (A) are rationalists (B). No British (C) philosophers (D) are rationalists. Therefore, 
            // no British philosophers are scientists.
            pred1 = new ForTypePredicate<T>((T x) => !x.A || x.B);
            pred2 = new ForTypePredicate<T>((T x) => !(x.C && x.D) || !x.B);
            var counter = new ForTypePredicate<T>((T x) => !(x.C && x.D) || !x.A);
            var s = new T() { C = true, D = true, A = true };
            Assert.IsTrue(!pred1(s) && !pred2(s)); // PAROU: Why failing?
            Assert.IsTrue(!pred1(s) && !pred2(s) && !counter(s));
            Assert.IsTrue(!counter(s));
        }
    }
}
