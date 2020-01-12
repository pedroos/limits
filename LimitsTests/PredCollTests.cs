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
        public void PredicateCollectionBasicTest()
        {
            var coll = new PredicateCollection();
            Func<object, bool> pred = (object obj) => obj.GetType() == typeof(int);

            // Invalid element
            Assert.ThrowsException<InvalidElementException>(() => coll.Add(pred, ""));

            // New predicate and element, ok
            coll.Add(pred, 1);
            Assert.AreEqual(1, coll.Get(pred));

            // Existing predicate and element, ignored
            coll.Add(pred, 1);
            Assert.AreEqual(1, coll.Get(pred));

            // New predicate, existing element, element ignored
            coll.Add((obj) => obj.GetType() != typeof(char), 1);

            // Match querying by each predicate
            Assert.AreEqual(coll.Get(pred), coll.Get((obj) => obj.GetType() != typeof(char)));

            // Existing predicate, new element, not ok
            Assert.ThrowsException<ExistingElementException>(() => coll.Add(pred, 2));
        }


        [TestMethod]
        public void PredicateCollectionReferenceTypeTest()
        {
            var coll = new PredicateCollection();
            Func<object, bool> pred = (obj) => obj.GetType() == typeof(string);
            Assert.ThrowsException<InvalidElementException>(() => coll.Add(pred, 1));
            string elem = "a";
            coll.Add(pred, elem);
            Assert.AreSame(elem, coll.Get(pred));
            Assert.ThrowsException<ExistingElementException>(() => coll.Add(pred, "b"));
            Assert.IsTrue(coll.Valid());
        }

        [TestMethod]
        public void ManyPredicateCollectionBasicTest()
        {
            var coll = new ManyPredicateCollection();

            // The element predicate
            Func<object, bool> elemPred = e => e.GetType() == typeof(int);

            // The collection predicate
            var collPred = CollectionPredicates.All(elemPred);

            coll.Add(1);
            coll.AddPred(collPred);
            coll.AddPred(collPred); // Ignored

            // Invalid element
            Assert.ThrowsException<InvalidElementException>(() => coll.Add("a"));

            Assert.IsTrue(new object[] { 1 }.SequenceEqual(coll.Get(elemPred)));
        }

        [TestMethod]
        public void ManyPredicateCollectionQuantityTest()
        {
            var coll = new ManyPredicateCollection();
            Func<object, bool> elemPred = e => e.GetType() == typeof(int);
            var collPred = CollectionPredicates.Count(elemPred, 2);
            Assert.ThrowsException<InvalidPredicateException>(() => coll.AddPred(collPred));
            coll.Add(1);
            Assert.ThrowsException<InvalidPredicateException>(() => coll.AddPred(collPred));
            coll.Add(2);
            coll.AddPred(collPred);
            Assert.IsTrue(new object[] { 1, 2 }.SequenceEqual(coll.Get(elemPred)));
            coll.Add(1, 2); // Repeated elements, ok
            Assert.ThrowsException<InvalidElementException>(() => coll.Add(3));
            Assert.IsTrue(new object[] { 1, 2 }.SequenceEqual(coll.Get(elemPred)));
        }

        [TestMethod]
        public void ManyPredicateCollectionPositionTest()
        {
            var coll = new ManyPredicateCollection();
            Func<object, bool> elemPred = e => e.GetType() == typeof(int);
            var collPred = CollectionPredicates.Position(elemPred, coll.elemPositions, 2, 3);

            // No elements
            Assert.ThrowsException<InvalidPredicateException>(() => coll.AddPred(collPred));

            coll.Add("a", "b");
            coll.AddToPos(("c", 1));
            coll.AddToPos(("d", 2));

            // Wrong type element
            Assert.ThrowsException<InvalidPredicateException>(() => coll.AddPred(collPred));
            coll.Remove("d");

            coll.AddToPos((1, 2));

            // Unfulfilled position
            Assert.ThrowsException<InvalidPredicateException>(() => coll.AddPred(collPred));

            coll.AddToPos((2, 3));

            // Satisfied
            coll.AddPred(collPred);

            // Replace position
            coll.AddToPos((4, 3));
            Assert.IsTrue(new object[] { 1, 4 }.SequenceEqual(coll.GetByPos(2, 3)));
            Assert.ThrowsException<InvalidElementException>(() => coll.AddToPos(("e", 3)));

            // Remove constraint
            coll.RemovePred(collPred);

            // Add again
            coll.AddToPos(("e", 3));
            Assert.IsTrue(new object[] { 1, "e" }.SequenceEqual(coll.GetByPos(2, 3)));
        }

        // TODO: testar casos negativos de conjuntos de predicates de coleção.
        // Por exemplo, conjuntos de predicates que tornam impossível haver um elemento.
        // Buscar que constraints são possíveis expressar com conjuntos de predicates.
    }
}
