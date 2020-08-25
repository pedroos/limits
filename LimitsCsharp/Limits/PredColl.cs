using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Limits.PredColl
{
    // Delegate types can't be subclassed.
    public delegate bool Predicate(object elem);

    // An element type assertion.
    public delegate bool ElementTypePredicate<T>(object elem);

    // A property of a type
    public delegate bool ForTypePredicate<T>(T elem);

    // A property of a collection
    public delegate bool Quantifier(IEnumerable<object> elems);

    // A property of all elements of a specific type in a collection
    public delegate bool ForTypeQuantifier<T>(IEnumerable<T> elems);

    public static class Predicates
    {
        // Confirms that an untyped object is of the specified type
        public static ElementTypePredicate<T> ElementTypePredicate<T>()
        {
            return e => e is T;
        }

        // Delegates which operate on typed elements can't be stored -- for those delegates, we implement 
        // conversions to equivalent ones which operate on untyped elements

        public static Predicate ToPredicate<T>(this ElementTypePredicate<T> elemTypePred)
        {
            return new Predicate((object e) => e is T);
        }

        public static Predicate ToPredicate<T>(this ForTypePredicate<T> forTypePred)
        {
            // Must be satisfiable for an element of the specified type only.
            return new Predicate((object e) => !(e is T) || forTypePred((T)e));
        }

        // Confirms that, in addition to satisfying the original predicate, the element is of the specified type
        public static ElementTypePredicate<T> ToElementTypePredicate<T>(this Predicate pred)
        {
            return new ElementTypePredicate<T>((object e) => e is T && pred(e));
        }
    }

    // There can't be conversions between delegate types, so quantifiers must be defined for each 
    // delegate/predicate type (no predicate hierarchies).
    public static class Quantifiers
    {
        // Certifies that all elements satisfy the element predicate
        public static Quantifier All(Predicate pred)
        {
            return (IEnumerable<object> elems) => elems.All(e => pred(e));
        }

        public static Quantifier All<T>(ElementTypePredicate<T> pred)
        {
            return (IEnumerable<object> elems) => elems.All(e => pred(e));
        }

        // Certifies that a specified number of elements satisfy the element predicate
        public static Quantifier Count(Predicate pred, int count)
        {
            return (IEnumerable<object> elems) => elems.Count(e => pred(e)) == count;
        }

        public static Quantifier Count<T>(ElementTypePredicate<T> pred, int count)
        {
            return (IEnumerable<object> elems) => elems.Count(e => pred(e)) == count;
        }

        // Certifies that the elements at the specified positions satisfy the element predicate
        // Position is 0-based
        public static Quantifier Position(Predicate pred, Dictionary<int, object>
            positionsMap, params int[] positions)
        {
            return (IEnumerable<object> elems) => positions.All(p => positionsMap.ContainsKey(p) &&
                pred(positionsMap[p]));
        }

        public static Quantifier Position<T>(ElementTypePredicate<T> pred,
            Dictionary<int, object> positionsMap, params int[] positions)
        {
            return (IEnumerable<object> elems) => positions.All(p => positionsMap.ContainsKey(p) &&
                pred(positionsMap[p]));
        }
    }

    // Permits one element per predicate.
    public class PredicateCollection
    {
        private readonly HashSet<Predicate> preds;
        private readonly HashSet<object> elems;

        public bool CheckValid { get; set; }

        public PredicateCollection()
        {
            preds = new HashSet<Predicate>();
            elems = new HashSet<object>();
            CheckValid = true;
        }

        public void Add(Predicate pred, object elem)
        {
            if (!pred(elem))
            {
                throw new InvalidElementException();
            }
            if (elems.Any(e => !e.Equals(elem) && pred(e)))
            {
                // Would return more than one element for the same predicate.
                throw new ExistingElementException();
            }
            if (preds.Any(p => p(elem)))
            {
                // Another true predicate (or the same predicate) exists for the element. If the element exists, 
                // add the predicate, if new. It the element doesn't exist, don't store more than one element for 
                // a single predicate (any of those two predicates).
                if (elems.Any(e => e.Equals(elem)))
                    preds.Add(pred);
                else
                    throw new ExistingElementException();
            }
            // New and non-overlappping predicate, and new element
            elems.Add(elem);
            preds.Add(pred);

            if (CheckValid)
                if (!Valid()) throw new InvalidCollectionException();
        }

        public void Add<T>(ElementTypePredicate<T> pred, object elem)
        {
            Add(pred.ToPredicate(), elem);
        }

        public object Get(Predicate pred)
        {
            return elems.Single(e => pred(e));
        }

        public T Get<T>(ElementTypePredicate<T> pred)
        {
            return (T)elems.Single(e => pred(e));
        }

        public bool Valid()
        {
            return preds.All(p => elems.SingleOrDefault(e => p(e)) != default);
        }
    }

    // Permits multiple elements per predicate.

    public class ManyPredicateCollection
    {
        private readonly HashSet<Quantifier> quants;
        private readonly HashSet<object> elems;
        private readonly Dictionary<int, object> elemPositions; // Set partial ordering
        public bool CheckValid { get; set; }
        public ManyPredicateCollection()
        {
            quants = new HashSet<Quantifier>();
            elems = new HashSet<object>();
            elemPositions = new Dictionary<int, object>();
            CheckValid = true;
        }

        public Quantifier BindPositionQuantifier(Predicate pred, params int[] positions)
        {
            return Quantifiers.Position(pred, elemPositions, positions);
        }

        public Quantifier BindPositionQuantifier<T>(ElementTypePredicate<T> pred, 
            params int[] positions)
        {
            return Quantifiers.Position(pred, elemPositions, positions);
        }

        public void AddQuant(Quantifier quant)
        {
            if (!quant(elems))
                throw new InvalidPredicateException();
            quants.Add(quant);

            if (CheckValid)
                if (!Valid()) throw new InvalidCollectionException();
        }

        // Adding is not atomic.
        public void Add(params object[] elems)
        {
            foreach (var elem in elems)
            {
                var elemsConcat = new HashSet<object>(this.elems);
                elemsConcat.Add(elem);
                if (quants.Any(p => !p(elemsConcat)))
                    throw new InvalidElementException();

                this.elems.Add(elem);

                if (CheckValid)
                    if (!Valid()) throw new InvalidCollectionException();
            }
        }

        public void AddToPos(params ValueTuple<object, int>[] elemsPositions)
        {
            foreach (var elemPosition in elemsPositions)
            {
                var elem = elemPosition.Item1;
                int position = elemPosition.Item2;
                var elemsConcat = new HashSet<object>(elems);
                elemsConcat.Add(elem);
                if (quants.Any(p => !p(elemsConcat)))
                    throw new InvalidElementException(); // Check for elements first
                elems.Add(elem);

                if (elemPositions.ContainsKey(position))
                    elemPositions[position] = elem;
                else
                    elemPositions.Add(position, elem);
                if (quants.Any(p => !p(elemsConcat)))
                    throw new InvalidElementException(); // Then check for positions

                if (CheckValid)
                    if (!Valid()) throw new InvalidCollectionException();
            }
        }

        public void Remove(params object[] elems)
        {
            foreach (var elem in elems)
            {
                this.elems.Remove(elem);
                if (elemPositions.ContainsValue(elem))
                {
                    var elemPosition = elemPositions.SingleOrDefault(ep => ep.Value.Equals(elem));
                    elemPositions.Remove(elemPosition.Key);
                }
            }
        }

        public void RemoveQuant(Quantifier quant)
        {
            quants.Remove(quant);
        }

        public IEnumerable<object> Get(Predicate pred)
        {
            return elems.Where(e => pred(e));
        }

        public IEnumerable<T> Get<T>(ElementTypePredicate<T> pred)
        {
            return elems.Where(e => pred(e)).Cast<T>();
        }

        public IEnumerable<object> GetByPos(params int[] positions)
        {
            return positions.Select(p => elemPositions[p]);
        }

        public bool Valid()
        {
            return quants.All(p => p(elems));
        }
    }

    #region Exceptions

    public class InvalidCollectionException : Exception { }

    public class ExistingElementException : InvalidCollectionException { }

    public class InvalidElementException : InvalidCollectionException { }

    public class InvalidPredicateException : InvalidCollectionException { }

    #endregion
}
