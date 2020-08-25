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

    // The problem is that "quantifiers" don't exist. An existence quantifier defines existence (of a variable?). 
    // A forall quantifier defines an assertion that a predicate is true for all elements which a variable represents. 
    // 'Exists' defines a variable which represents a number of elements. Forall asserts that a predicate is true for a variable.

    // A predicate is an expression which has a truth value.
    // The predicate has free variables which must be substituted for the predicate to give a truth value. 
    // This may be done by associating the predicate directly with the corresponding variables. An 'universe' may be called a 
    // collection of variables.

    // Associating variables to a predicate forms a statement.
    // But the predicate can be itself a statement, the truth value of which is the truth value of the predicate. 
    // So we're dealing with nested statements. The "parent" statement is true if all children statements are true. 
    // This means to say the parent statement has the truth value of the conjunction (for two statements, when any is false, 
    // then false. Otherwise, true) of all its children statements' truth values.

    // Let's examine 

    // u is defined.
    // forall epsilon >= 0 belongs to reals, 
    // exists a' belongs to A such that u - epsilon <= a' <= u.

    // or

    // forall epsilon belongs to reals such that epsilon >= 0, 
    // exists a' belongs to A such that u - epsilon <= a' <= u.

    // First, u is defined as one element of the reals.
    // Then, epsilon is defined as all elements of the reals.
    // Then, a predicate for epsilon is defined as doing:
    // - Define a' as elements of A
    // - Define a predicate for a', u, epsilon (all already defined) as doing:
    //   - u - epsilon <= a' <= u
    // - Assert that a' exists.

    // Asserting validity of a predicate for one or all elements represented by a variable. What's the difference between them? 
    // There being all implies there being one. We'll have to, for the moment, just differentiate between them with no other 
    // significance.

    // First, a variable is not defined as being 'one' or 'all' of a set. It is defined as being of a set, and then quantified as 
    // 'existing' or 'forall'. It's kinda like 'existing' is 'one existing' and 'forall' is 'all existing'. Exists asserts validity 
    // of a predicate for one element. Forall, for all elements.

    // So,
    // - u defined as belonging to the reals
    // - u quantified as existing unconditionally. (Since does not depend on any other statement being true, *this is an axiom*.)
    // - epsilon defined as belonging to the reals
    // - a' defined as belonging to the reals
    // - epsilon quantified as all existing, such that: (the predicate is a condition for the quantifier:)
    //   - u - epsilon <= a' <= u (note all variables were already defined)
    // That's all. The one little doubt is whether a' needed to be defined before the epsilon predicate was defined, or it could 
    // be defined inside of it.

    // So, there's a universe of defined variables, each belonging to a set. Maybe, there are scopes for this (as in defined for 
    // only some predicates.)
    // Then, each is quantified as there being one, or representing all elements of their sets.
    // Optionally, each such quantification has an associated predicate which must be true for the quantification to be true.
    // All variables in a predicate must be previously defined.

    // A predicate can contain an implication relation. (Or any other relation between statements?)
    // This works like a "restriction" on some variable of the predicate, or between variables of the predicate. Only variables 
    // satisfying the first statement in the implication validate the second. But this is implied from the implicaton relation.

    // How to represent statements connected by relations? A predicate is an arbitrary number of connected statements. A 
    // statement is a definition of variables (so *this* is the scope), plus quantification (with optional predication) of all 
    // variables. The predication consists of a sub-statement being true. That's all.
    // Connecting statements is there being a statement which is only a connection of (two) statements. Composite statements can be 
    // assembled from pairs of connected statements? Or statements could be by default collections of connected statements, of 
    // which there only being two connected statements is a special case. The thing is such "sub-statements" share the same 
    // variables.

    // Let's consider an statement an atom of certain variables, quantified, etc., which has a truth value. First, we must test 
    // composition and decomposition of statements into/from multiple/single statements.
    // Since we need an atomic definition (and that ain't a Statement), we'll defined StatementPair.

    // We get down to terms and term relations. Terms are single variables or 'values' (such as a number such as zero). But such 
    // values are just forone-quantified variables in a set: zero is a forone-quantified variable in the reals. (Which of the 
    // elements of the reals that represents depends on the ordering property of the reals, and is out of scope here.) So a 
    // variable is just a forall-quantified term, and a "number" (we'll call them 'element terms' is a forexactlyone-quantified 
    // term.

    // It seems 'for one (or more)' quantification is a bit out of place in the term context. We'll find out what it means later.

    // Kind of like set hierarchy, term pairs must be composable in some way which bears similarities. There can be term pairs of 
    // term pairs. But there must be a 'term pair element' so that an initial term pair can be defined. It's like this first one is 
    // an element, and from there above are only sets. A difference being that a set element has a value (which a set hasn't), and 
    // a term pair has two terms and a term relation (which a term set hasn't).

    // Now term sets become the units. A term set, given quantification of all its variables, has a truth value. Verify this.

    // A term set is a binary tree with terminal term pair nodes. The term pair is a term set so it can belong to a term set, so it 
    // can "fit into the tree" as a terminal node. They have nothing in common, though, so maybe it's better to have them inherit 
    // from a common interface which defines the thing they have in common (which is having a truth value when evaluated).

    // Quantification is not per-term pair. It is by term set... As term pairs can have terms in common, and variable terms are 
    // quantified. We can model this like quantification-propagation though: term pairs are quantified but they receive 
    // quantification from any pair upwards in the tree.

    // A & B & C & D -> A B & C D -> A B C D.

    // The thing is, what are we asserting? Relations. We may assert binary, but also unary or tertiary relations. An unary 
    // relation could be an element existing, or any "property" of an element. But an element having a "property" is just an 
    // element having a predicate satisfied (and the element is part of the predicate such that the predicate is not a tautology).
}
