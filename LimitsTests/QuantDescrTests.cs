using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Collections.Generic;

namespace LimitsTests
{
    using Limits.QuantDescr;

    [TestClass]
    public partial class QuantDescrTests
    {
        [TestMethod]
        public void PositionInvalidSizeTest()
        {
            var qd = new QuantDescr<Type>(3);
            Assert.ThrowsException<SizeException>(() => { qd.SetPosition(4, typeof(int)); });
        }

        [TestMethod]
        public void PositionInvalidMissingTest()
        {
            var qd = new QuantDescr<Type>(3);
            qd.SetPosition(1, typeof(int));
            qd.SetPosition(2, typeof(string));
            qd.SetPosition(2, typeof(double));
            var result = qd.Complete();
            Assert.IsInstanceOfType(result, typeof(ResultPositionMissing<Type>));
            Assert.AreEqual(3, (result as ResultPositionMissing<Type>).Position);
        }

        [TestMethod]
        public void PositionValidTest()
        {
            var qd = new QuantDescr<Type>(3);
            // Describe a tuple of int, string, double.
            qd.SetPosition(1, typeof(int));
            qd.SetPosition(2, typeof(string));
            qd.SetPosition(3, typeof(double));
            var result = qd.Complete();
            Assert.IsInstanceOfType(result, typeof(ResultComplete));
        }

        [TestMethod]
        public void QuantityInvalidSizeTest()
        {
            var qd = new QuantDescr<Type>(3);
            Assert.ThrowsException<SizeException>(() => { qd.SetQuantity(typeof(int), 4); });
        }

        [TestMethod]
        public void QuantityInvalidMissingTest()
        {
            var qd = new QuantDescr<Type>(3);
            qd.SetQuantity(typeof(int), 1);
            qd.SetQuantity(typeof(string), 1);
            var result = qd.Complete();
            Assert.IsInstanceOfType(result, typeof(ResultQuantityError<Type>));
            Assert.AreEqual(1, (result as ResultQuantityError<Type>).Quantity);
        }

        [TestMethod]
        public void QuantityValidTest()
        {
            var qd = new QuantDescr<Type>(3);
            // Describe a list with two ints and one string.
            qd.SetQuantity(typeof(int), 2);
            qd.SetQuantity(typeof(string), 1);
            var result = qd.Complete();
            Assert.IsInstanceOfType(result, typeof(ResultComplete));
        }

        [TestMethod]
        public void Mixed1Valid1Test()
        {
            var qd = new QuantDescr<Type>(2);
            // Describe a list with an int in the first position plus existence of an int
            qd.SetPosition(1, typeof(int));
            qd.SetQuantity(typeof(int), 1);
            // Coexistant position and existence conditions reduce to the position condition only, 
            // as satisfying one position for a type satisfies one existence for that type.
            var result = qd.Complete();
            Assert.IsInstanceOfType(result, typeof(ResultComplete));
        }

        [TestMethod]
        public void Mixed1Valid2Test()
        {
            var qd = new QuantDescr<Type>(2);
            qd.SetPosition(1, typeof(int));
            qd.SetPosition(2, typeof(int));
            qd.SetQuantity(typeof(int), 2);
            // Once again, for each quantity condition, a position condition satisfies that.
            var result = qd.Complete();
            Assert.IsInstanceOfType(result, typeof(ResultComplete));
        }

        [TestMethod]
        public void Mixed1InvalidRemainingTest()
        {
            var qd = new QuantDescr<Type>(2);
            qd.SetPosition(1, typeof(int));
            qd.SetPosition(2, typeof(string));
            qd.SetQuantity(typeof(string), 1);
            // There is a quantity remaining (string).
            var result = qd.Complete();
            Assert.IsInstanceOfType(result, typeof(ResultQuantityError<Type>));
            Assert.AreEqual(typeof(string), (result as ResultQuantityError<Type>).Quality);
            Assert.AreEqual(1, (result as ResultQuantityError<Type>).Quantity);
        }

        [TestMethod]
        public void Mixed1InvalidMissingTest()
        {
            var qd = new QuantDescr<Type>(2);
            qd.SetPosition(1, typeof(int));
            qd.SetPosition(2, typeof(string));
            qd.SetQuantity(typeof(int), 2);
            // There is a quantity missing (int).
            var result = qd.Complete();
            Assert.IsInstanceOfType(result, typeof(ResultQuantityError<Type>));
            Assert.AreEqual(typeof(int), (result as ResultQuantityError<Type>).Quality);
            Assert.AreEqual(-1, (result as ResultQuantityError<Type>).Quantity);
        }
    }
}
