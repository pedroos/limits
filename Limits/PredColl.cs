using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Limits.PredColl
{
    public class PredicateCollection
    {
        private readonly HashSet<Func<object, bool>> preds;
        private readonly HashSet<object> elems;
        public bool CheckValid { get; set; }
        public PredicateCollection()
        {
            preds = new HashSet<Func<object, bool>>();
            elems = new HashSet<object>();
            CheckValid = true;
        }
        public void Add(Func<object,bool> pred, object elem)
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
        // TODO: if the predicate contains a type constraint, this should return that type.
        public object Get(Func<object,bool> pred)
        {
            return elems.Single(pred);
        }
        public bool Valid()
        {
            return preds.All(p => elems.SingleOrDefault(e => p(e)) != default);
        }
    }

    public static class CollectionPredicates
    {
        /// <summary>
        /// Certifies that all elements satisfy the element predicate
        /// </summary>
        public static Func<IEnumerable<object>, bool> All(Func<object, bool> elemPred)
        {
            return (IEnumerable<object> elems) => elems.All(elemPred);
        }

        /// <summary>
        /// Certifies that a specified numbers of elements satisfy the element predicate
        /// </summary>
        public static Func<IEnumerable<object>, bool> Count(Func<object, bool> elemPred, int count)
        {
            return (IEnumerable<object> elems) => elems.Count(elemPred) == count;
        }

        /// <summary>
        /// Certifies that the elements at the specified positions satisfy the element predicate
        /// </summary>
        /// <remarks>Position is 0-based</remarks>
        public static Func<IEnumerable<object>, bool> Position(Func<object, bool> elemPred, Dictionary<int, object> 
            positionsMap, params int[] positions)
        {
            return (IEnumerable<object> elems) => positions.All(p => positionsMap.ContainsKey(p) && 
                elemPred(positionsMap[p]));
        }
    }

    public interface IPositionObject
    {
        public int Position { get; }
    }

    public class ManyPredicateCollection
    {
        private readonly HashSet<Func<IEnumerable<object>, bool>> collPreds;
        private readonly HashSet<object> elems;
        public readonly Dictionary<int, object> elemPositions; // Set partial ordering
        public bool CheckValid { get; set; }
        public ManyPredicateCollection()
        {
            collPreds = new HashSet<Func<IEnumerable<object>, bool>>();
            elems = new HashSet<object>();
            elemPositions = new Dictionary<int, object>();
            CheckValid = true;
        }

        public void AddPred(Func<IEnumerable<object>, bool> collPred)
        {
            if (!collPred(elems))
                throw new InvalidPredicateException();
            collPreds.Add(collPred);

            //if (CheckValid && false)
                if (!Valid()) throw new InvalidCollectionException();
        }

        // No position intelligence possible because position predicates can't be interpreted. Positions must be 
        // informed.

        /// <remarks>
        /// Non-atomic operation.
        /// </remarks>
        public void Add(params object[] elems)
        {
            foreach (var elem in elems)
            {
                var elemsConcat = new HashSet<object>(this.elems);
                elemsConcat.Add(elem);
                if (collPreds.Any(p => !p(elemsConcat)))
                    throw new InvalidElementException();

                this.elems.Add(elem);

                //if (CheckValid && false)
                if (!Valid()) throw new InvalidCollectionException();
            }
        }

        /// <remarks>
        /// Non-atomic operation.
        /// </remarks>
        public void AddToPos(params ValueTuple<object, int>[] elemsPositions)
        {
            foreach (var elemPosition in elemsPositions)
            {
                var elem = elemPosition.Item1;
                int position = elemPosition.Item2;
                var elemsConcat = new HashSet<object>(elems);
                elemsConcat.Add(elem);
                if (collPreds.Any(p => !p(elemsConcat)))
                    throw new InvalidElementException(); // Check for elements first
                elems.Add(elem);

                if (elemPositions.ContainsKey(position))
                    elemPositions[position] = elem;
                else
                    elemPositions.Add(position, elem);
                if (collPreds.Any(p => !p(elemsConcat)))
                    throw new InvalidElementException(); // Then check for positions

                //if (CheckValid && false)
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

        public void RemovePred(Func<IEnumerable<object>, bool> collPred)
        {
            collPreds.Remove(collPred);
        }

        public IEnumerable<object> Get(Func<object, bool> elemPred)
        {
            return elems.Where(elemPred);
        }
        public IEnumerable<object> GetByPos(params int[] positions)
        {
            return positions.Select(p => elemPositions[p]);
        }
        public bool Valid()
        {
            return collPreds.All(p => p(elems));
        }
    }

    #region Exceptions

    public class InvalidCollectionException : Exception { }

    public class ExistingElementException : InvalidCollectionException { }

    public class InvalidElementException : InvalidCollectionException { }

    public class InvalidPredicateException : InvalidCollectionException { }

    #endregion
}
