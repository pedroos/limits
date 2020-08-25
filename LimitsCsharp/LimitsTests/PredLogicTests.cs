using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Text;

namespace LimitsTests
{
    using Limits.PredLogic;
    using Limits.ElemOjbsUnf;

    [TestClass]
    public partial class PredLogicTests
    {
        // An example: exercise 5 in v\FundAnálise\Aula09_Exercícios_3.pdf

        [TestMethod]
        public void TermPairToString()
        {
            var a = new Variable(Set.Reals, TermName.a);
            var zero = new Element(Set.Reals, TermName.zero);
            var pair1 = new TermPair(a, zero, TermRelation.GreaterThan);
            Assert.AreEqual("a GreaterThan zero", pair1.ToString());
        }

        [TestMethod]
        public void TermSetToString()
        {
            var a = new Variable(Set.Reals, TermName.a);
            var zero = new Element(Set.Reals, TermName.zero);
            var minusOne = new Element(Set.Reals, TermName.m_one);
            var pair1 = new TermPair(a, zero, TermRelation.GreaterThan);
            var pair2 = new TermPair(a, minusOne, TermRelation.LowerThan);
            Assert.AreEqual("a GreaterThan zero", pair1.ToString());
        }

        [TestMethod]
        public void Statement1()
        {
            var a = new Variable(Set.Reals, TermName.a);
            var zero = new Element(Set.Reals, TermName.zero);
            var pair1 = new TermPair(a, zero, TermRelation.GreaterThan);
            var stat = new Statement(pair1).WithQuant(a, Quantifier.ForAll);

        }

        //[TestMethod]
        //public void Negate1()
        //{
        //    var uni = new Universe();

        //    var u = new Variable();

        //    uni.AddVariable(u);

        //    u is defined.
        //    forall epsilon >= 0 belongs to reals, 
        //     exists a' belongs to A such that u - epsilon <= a' <= u.
        //     or
        //     forall epsilon belongs to reals such that epsilon >= 0, 
        //     exists a' belongs to A such that u - epsilon <= a' <= u.
        //    var exp = new Statement(Variable.epsilon, Quantifier.ForAll, Set.Reals) { Universe = uni };
        //    var neg = exp.Negate();
        //}
    }
}
