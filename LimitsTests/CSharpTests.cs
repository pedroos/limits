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

        class PropClass
        {
            public static bool Prop(object elem)
            {
                return true;
            }

            public static bool PropGen<T>(T elem)
            {
                return true;
            }
        }

        class PropClass<T>
        {
            public static bool Prop(object elem)
            {
                return true;
            }

            public static bool PropGen(T elem)
            {
                return true;
            }
        }

        delegate bool PropDel1(object elem);
        delegate bool PropDel2(object elem);
        delegate bool PropDel3<T>(T elem);
        delegate bool PropDel4<T>(T elem);

        [TestMethod]
        public void DelegateTest()
        {
            // Each delegate declaration creates a delegate type (safe pointer), irrespective of their 
            // equivalences in respect to their signatures.
            // Delegates are equal when they are created from the same delegate type and point to the 
            // same function.
            var propDel1 = Delegate.CreateDelegate(typeof(PropDel1), typeof(PropClass).GetMethod("Prop"));
            Assert.AreEqual(propDel1, propDel1);
            var propDel1b = Delegate.CreateDelegate(typeof(PropDel1), typeof(PropClass).GetMethod("Prop"));
            Assert.AreEqual(propDel1, propDel1b);
            var propDel2 = Delegate.CreateDelegate(typeof(PropDel2), typeof(PropClass).GetMethod("Prop"));
            Assert.AreNotEqual(propDel1, propDel2);
            Assert.IsFalse(propDel1.Equals(propDel2));

            // Same as above but with a type argument.
            // A type must be chosen. But can't bind <int> to <T>.
            Assert.ThrowsException<ArgumentException>(() => Delegate.CreateDelegate(typeof(PropDel3<int>),
                typeof(PropClass).GetMethod("PropGen")));

            // Can bind. Equal behavior to non-generic.
            var propDelGen1 = Delegate.CreateDelegate(typeof(PropDel3<int>), typeof(PropClass<int>)
                .GetMethod("PropGen"));
            Assert.AreEqual(propDelGen1, propDelGen1);
            var propDelGen1b = Delegate.CreateDelegate(typeof(PropDel3<int>), typeof(PropClass<int>)
                .GetMethod("PropGen"));
            Assert.AreEqual(propDelGen1, propDelGen1b);
            var propDelGen2 = Delegate.CreateDelegate(typeof(PropDel4<int>), typeof(PropClass<int>)
                .GetMethod("PropGen"));
            Assert.AreNotEqual(propDelGen1, propDelGen2);
            Assert.IsFalse(propDelGen1.Equals(propDelGen2));

            // Delegates to lambdas are not equal because they do not point to the same functions.
            var del1 = new PropDel1(_ => true);
            Assert.AreEqual(del1, del1);
            var del2 = new PropDel1(_ => true);
            Assert.AreNotEqual(del1, del2);
            Assert.IsFalse(del1.Equals(del2));

            // But their delegate types may be equal
            Assert.IsTrue(del1.GetType().Equals(del2.GetType()));

            // Same-type delegates to the same func variables are equal.
            Func<object, bool> func1 = _ => true;
            var funcDel1 = new PropDel1(func1);
            Assert.AreEqual(funcDel1, funcDel1);
            var funcDel2 = new PropDel1(func1);
            Assert.AreEqual(funcDel1, funcDel2);
            Assert.IsTrue(funcDel1.Equals(funcDel2));

            // Dynamically invoke a delegate of the wrong type
            Delegate wrongTypeArg = new PropDel3<int>((int a) => true);
            Assert.ThrowsException<ArgumentException>(() => wrongTypeArg.DynamicInvoke("a"));
        }

        [TestMethod]
        public void FunctionTest()
        {
            // Funcs are only equal by reference.
            Assert.AreNotEqual(new Func<object, bool>(_ => true), new Func<object, bool>(_ => true));
            Func<object, bool> func = _ => true;
            Assert.AreEqual(func, func);
            Func<object, bool> func2 = _ => true;
            Assert.AreNotEqual(func, func2);
            Assert.IsFalse(func.Equals(func2));
            Func<object, bool> func3 = func;
            Assert.AreEqual(func, func3);
        }
    }
}
