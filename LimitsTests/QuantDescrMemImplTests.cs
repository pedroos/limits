using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LimitsTests
{
    [TestClass]
    public partial class QuantDescrMemImplTests
    {
        [StructLayout(LayoutKind.Explicit)]
        class Type1
        {

        }

        [StructLayout(LayoutKind.Explicit)]
        struct Type2
        {

        }

        [StructLayout(LayoutKind.Sequential)]
        class Type3
        {
            public DateTime dateTime;
            public List<double> listDoubles;
        }

        [StructLayout(LayoutKind.Sequential)]
        class Type4
        {
            public DateTime dateTime;
            public ArrayList list;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Type5
        {
            public DateTime dateTime;
            public int number;
            public override string ToString()
            {
                return dateTime.ToString("dd/MM/yyyy") + "," + number.ToString();
            }
        }

        [TestMethod]
        public void Test1()
        {
            unsafe
            {
                IntPtr iptr = Marshal.AllocHGlobal(1024);
                void* ptr = iptr.ToPointer();
                *(int*)ptr = 1034;
                Assert.AreEqual(1034, *((int*)ptr));
            }
        }

        [TestMethod]
        public void Test2()
        {
            unsafe
            {
                IntPtr iptr = Marshal.AllocHGlobal(1024);
                Type1 type1 = Marshal.PtrToStructure<Type1>(iptr);
                //void* ptr = iptr.ToPointer();
                //*((Type1*)ptr) = new Type();
                Assert.IsFalse(new Type1().Equals(type1));
            }
        }

        [TestMethod]
        public void Test3()
        {
            unsafe
            {
                IntPtr iptr = Marshal.AllocHGlobal(1024);
                Type2 type2 = Marshal.PtrToStructure<Type2>(iptr);
                //void* ptr = iptr.ToPointer();
                //*((Type1*)ptr) = new Type();
                Assert.IsTrue(new Type2().Equals(type2));
            }
        }

        [TestMethod]
        public void Test4()
        {
            unsafe
            {
                IntPtr iptr = Marshal.AllocHGlobal(10000);
                Type3 type3 = new Type3();
                // System.TypeLoadException: Cannot marshal field 'listDoubles' of type 'Type3': Generic types 
                // cannot be marshaled.
                Assert.ThrowsException<TypeLoadException>(() => Marshal.StructureToPtr(type3, iptr, false));
                //Type3 type3b = Marshal.PtrToStructure<Type3>(iptr);
                //Assert.IsTrue(type3b.dateTime.Equals(new DateTime(2001, 2, 3)));
            }
        }

        [TestMethod]
        public void Test5()
        {
            // Succeeds
            unsafe
            {
                IntPtr iptr = Marshal.AllocHGlobal(10000);
                Type4 type4 = new Type4() { dateTime = new DateTime(2001, 2, 3) };
                Marshal.StructureToPtr(type4, iptr, false);
                Type4 type4b = Marshal.PtrToStructure<Type4>(iptr);
                Assert.IsTrue(type4b.dateTime.Equals(new DateTime(2001, 2, 3)));
            }
        }

        [TestMethod]
        public void Test6()
        {
            // Succeeds (but struct)
            unsafe
            {
                IntPtr iptr = Marshal.AllocHGlobal(10000);
                Type5 type5 = new Type5() { dateTime = new DateTime(2001, 2, 3), number = 3012 };
                Marshal.StructureToPtr(type5, iptr, false);
                Type5 type5b = Marshal.PtrToStructure<Type5>(iptr);
                Assert.IsTrue(type5b.dateTime.Equals(new DateTime(2001, 2, 3)) && 
                    type5b.number.Equals(3012));
            }
        }

        [TestMethod]
        public void Test7()
        {
            unsafe
            {
                IntPtr iptr = Marshal.AllocHGlobal(10000);
                Type5 type5 = new Type5() { dateTime = new DateTime(2001, 2, 3), number = 3012 };

                void* ptr = iptr.ToPointer();
                *(Type5*)ptr = type5;
                Type5 type5b = *(Type5*)ptr;
                Assert.AreEqual(type5, type5b);

                ptr = ((Type5*)ptr) + 1;
                *(int*)ptr = 4391;
                Assert.AreEqual(4391, *(int*)ptr);

                ptr = ((Type5*)ptr) - 1;
                type5b = *(Type5*)ptr;
                Console.WriteLine(type5b.ToString());
                Assert.AreEqual(type5, type5b);

                ptr = ((Type5*)ptr) + 1;
                Assert.AreEqual(4391, *(int*)ptr);
            }
        }
    }
}
