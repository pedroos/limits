using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Limits.Continuity
{
    public class FiniteFunction<TD, TI>
    {
        readonly Func<TD, TI> body;
        public IEnumerable<TD> Domain { get; }
        public FiniteFunction(Func<TD, TI> body, IEnumerable<TD> domain)
        {
            this.body = body;
            Domain = domain;
        }
        public TI Image(TD x) => body(x);
        public IEnumerable<TI> Image() => Domain.Select(x => body(x));
    }

    // There exist no discrete discontinuities.

    public static class Extensions
    {
        public static bool IsDiscontinuousAt(this FiniteFunction<int, int> fun, int b, int epsilon)
        {
            if (!(fun.Domain is IOrderedEnumerable<int>)) throw new ArgumentException(nameof(fun));

            var domain = (IOrderedEnumerable<int>)fun.Domain;

            if (b < domain.First() || b > domain.Last()) throw new ArgumentException(nameof(b));

            int imageB = fun.Image(b);
            int epsilonRange = Math.Abs(epsilon - imageB);
            for (int x = domain.First(); x < domain.Last(); ++x)
            {
                if (x == b) continue;
                int imageX = fun.Image(x);
                int imageRange = Math.Abs(imageX - imageB);
                if (imageRange > epsilonRange) 
                    return true;
            }
            return false;
        }
    }
}