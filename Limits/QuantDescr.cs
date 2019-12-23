using System;
using System.Collections.Generic;
using System.Linq;

namespace Limits.QuantDescr
{
    // The type argument in these classes follow the type argument in QuantDescr. It could be Type 
    // or another class.
    public class ResultPositionMissing<Q> {
        public Q Quality { get; }
        public int Position { get; }
        public ResultPositionMissing(Q quality, int position)
        {
            Quality = quality;
            Position = position;
        }
    }
    public class ResultQuantityError<Q>
    {
        public Q Quality { get; }
        public int Quantity { get; } // Negative for missing, positive for remaining
        public ResultQuantityError(Q quality, int quantity)
        {
            Quality = quality;
            Quantity = quantity;
        }
    }

    public class ResultComplete { }

    public class SizeException : Exception { }

    // Q (or 'Quality') could be initially Type, or some variety of Element. Because at this point 
    // it represents an opaque classification, it is genericized. Q represents any kind of attribute 
    // by which QuantDescr elements can be compared. For example, it could not be Type, but another 
    // characteristic of objects to compare against, represented by another class. 
    // For Type, it is specifiable as: QuantDescr<Type>.
    public class QuantDescr<Q>
    {
        public int Size { get; }
        private Dictionary<Q, int> quantities;
        private Dictionary<int, Q> positions;
        public QuantDescr(int size)
        {
            Size = size;
            quantities = new Dictionary<Q, int>();
            positions = new Dictionary<int, Q>();
        }
        public void SetQuantity(Q e, int quantity)
        {
            if (quantity > Size) throw new SizeException();
            if (quantities.ContainsKey(e))
                quantities[e] = quantity;
            else quantities.Add(e, quantity);
        }
        public void SetPosition(int position, Q e)
        {
            if (position > Size) throw new SizeException();
            if (positions.ContainsKey(position))
                positions[position] = e;
            else positions.Add(position, e);
        }
        public object Complete()
        {
            // The challenge: integrate those two verifications

            var positionsFilled = new HashSet<int>();

            // Iterate over positions
            foreach (int position in positions.Keys)
            {
                // There should be no position duplicates
                positionsFilled.Add(position);
            }

            // Check that all possible positions have a position specification
            for (int position = 1; position < Size; ++position)
            {
                if (!positionsFilled.Contains(position))
                    return new ResultPositionMissing<Q>(default, position);
            }

            // Check quantities
            foreach (Q quantityQuality in quantities.Keys)
            {
                // Check for positions. Subtract positions from quantity.
                var quantityPositions = positions.Where(p => p.Value.Equals(quantityQuality));
                int quantityDiff = quantities[quantityQuality] - quantityPositions.Count();
                // If there are more positions than the quantity, it is incorrect.
                if (quantityDiff < 0)
                    return new ResultQuantityError<Q>(quantityQuality, quantityDiff);
                // If there are less positions than the quantity, it is also incorrect.
                if (quantityDiff > 0)
                    return new ResultQuantityError<Q>(quantityQuality, quantityDiff * -1);
            }

            //// All positions in the description must have a position specification
            //// TODO: too unespecific; we should be able to point which position specification is 
            //// missing.
            //for (int position = 1; position <= Size; ++position) 
            //    if (!positions.ContainsKey(position))
            //        return new ResultPositionMissing<T>(position);

            //// The sum of specified quantities must match the size of the description
            //// TODO: maybe also too unespecific; we should be able to point which quantities are 
            //// missing in the total (?)
            //int actualQuantity = quantities.Values.Sum();
            //if (actualQuantity != Size)
            //    return new ResultQuantityMissing(Size - actualQuantity);

            return new ResultComplete();
        }
    }
}
