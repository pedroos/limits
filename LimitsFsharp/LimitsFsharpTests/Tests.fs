namespace LimitsFsharpTests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open LimitsFsharp.ElemAsSet

[<TestClass>]
type TestClass () =

    [<TestMethod>]
    member this.TestMethodPassing () =
        Assert.IsTrue(true);

    [<TestMethod>]
    member this.SingletonToStringTest() = 
        let s = new Singleton<int>(Some(1))
        Assert.AreEqual("1", s.ToString)
