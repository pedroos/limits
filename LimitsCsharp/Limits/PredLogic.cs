using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Limits.PredLogic
{
    using Limits.ElemOjbsUnf;

    public enum Quantifier
    {
        //ForOne, 
        ForExactlyOne, 
        ForAll
    }
    public enum TermName
    {
        a, b, c, d,
        a_line, b_line, c_line, d_line,
        alpha, beta, gamma, delta, epsilon,
        m_one, zero, one
    }

    public enum TermRelation
    {
        LowerThan, 
        GreaterThan
    }

    public abstract class Term
    {
        public Quantifier Quantifier { get; } // Not related to the variable quantifier in a statement
        public Set Set { get; }
        public TermName Name { get; }
        public Term(Quantifier quantifier, Set set, TermName name)
        {
            Quantifier = quantifier;
            Set = set;
            Name = name;
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }

    public class Variable : Term
    {
        public Variable(Set set, TermName name) : base(Quantifier.ForAll, set, name) { }
    }

    public class Element : Term
    {
        public Element(Set set, TermName name) : base(Quantifier.ForExactlyOne, set, name) { }
    }

    public interface IEvaluatable
    {
        public bool GetTruthValue();
    }

    public class TermSet
    {
        public TermSet Set1 { get; }
        public TermSet Set2 { get; }
        public StatementRelation Relation { get; }
        public TermSet() { } // This constructor should not exist
        public TermSet(TermSet set1, TermSet set2, StatementRelation relation)
        {
            Set1 = set1;
            Set2 = set2;
            Relation = relation;
        }

        //public bool Evaluate()
        //{
        //    bool eval1 = Set1.Evaluate();
        //    bool eval2 = Set2.Evaluate();
        //    switch (Relation)
        //    {
        //        case StatementRelation.Conjunction:
        //            return eval1 && eval2;
        //        case StatementRelation.Disjunction:
        //            return eval1 || eval2;
        //        case StatementRelation.Implication:
        //            return (eval1 == eval2) || (!eval1 && eval2);
        //        case StatementRelation.Equivalence:
        //            return eval1 == eval2;
        //        default:
        //            throw new NotImplementedException();
        //    }
        //}

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Set1, Relation, Set2);
        }

        //public IEnumerable<TermSet> Traverse()
        //{

        //}
    }


    // The "primordial" term set.
    public class TermPair : TermSet
    {
        public Term Term1 { get; }
        public Term Term2 { get; }
        public new TermRelation Relation { get; }
        public TermPair(Term term1, Term term2, TermRelation relation) // Problem: TermPair too different from TermSet
        {
            Term1 = term1;
            Term2 = term2;
            Relation = relation;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Term1, Relation, Term2);
        }

        // Term pairs can only be evaluated together with quantification (statements)
    }

    public enum StatementRelation
    {
        Conjunction, 
        Disjunction, 
        Implication, 
        Equivalence
    }

    // The "primordial" statement pair. (?)
    // A statement evaluates a term pair.
    public class Statement/* : StatementPair*/
    {
        public TermPair TermPair { get; }
        Dictionary<Variable, Quantifier> termQuantifiers;

        public Statement(TermPair termPair)
        {
            TermPair = termPair;
            termQuantifiers = new Dictionary<Variable, Quantifier>();
        }

        public Statement WithQuant(Variable variable, Quantifier quantifier)
        {
            termQuantifiers.Add(variable, quantifier);
            return this;
        }

        public Statement Negate()
        {
            //if (Universe == default)
            //    throw new NoUniverseException(); // For variables instead
            return default;
        }

        (bool Ok, string Error) TryValidate()
        {
            (bool Ok, string Error) CheckQuantified(Term term)
            {
                if (term is Variable && !termQuantifiers.ContainsKey((Variable)term))
                    return (false, string.Format("Variable {0} is not quantified", term));
                return (true, null);
            }
            var res = CheckQuantified(TermPair.Term1);
            if (!res.Ok) return res;
            res = CheckQuantified(TermPair.Term2);
            if (!res.Ok) return res;
            return (true, null);
        }

        public bool Evaluate()
        {
            var val = TryValidate();
            if (!val.Ok) throw new MalformedStatementException(val.Error);
            return default;
        }

        public override string ToString()
        {
            var val = TryValidate();
            if (!val.Ok) 
                return string.Format("Malformed statement about {0}", TermPair);
            return string.Format("{0}, {1}", TermPair, termQuantifiers.Keys.Select(var => string.Format("{0} {1}", 
                termQuantifiers[var], var)));
        }

        //public bool EvaluateWith(StatementRelation relation, Statement statement2)
        //{
        //    return default;
        //}
    }

    //public class StatementPair
    //{
    //    public Statement Statement1 { get; }
    //    public Statement Statement2 { get; }
    //    public StatementRelation Relation { get; }
    //    public StatementPair() { }
    //    public StatementPair(Statement statement1, Statement statement2, StatementRelation relation)
    //    {
    //        Statement1 = statement1;
    //        Statement2 = statement2;
    //        Relation = relation;
    //    }
    //}

    //public static class Symbols
    //{
    //    public static Dictionary<Quantifier, string> Quantifiers { get; }
    //    public static Dictionary<TermName, string> TermNames { get; }
    //    public static Dictionary<TermRelation, string> TermRelations { get; }
    //    public static Dictionary<StatementRelation, string> StatementRelations { get; }
    //    static Symbols()
    //    {
    //        Quantifiers = new Dictionary<Quantifier, string>()
    //        {
    //            { Quantifier.ForAll, "\u220x" }
    //        };
    //    }
    //}

    public class MalformedStatementException : Exception
    {
        public MalformedStatementException(string message) : base(message) { }
    }
}
