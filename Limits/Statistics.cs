using System;
using System.Collections.Generic;
using System.Text;

using Limits.ElemPrimitive;

namespace Limits
{
    public class Experiment<T>
        where T : IEquatable<T>
    {
        public SampleSpace<T> Execute()
        {
            var set = new Set<Event<T>>();
            return set as SampleSpace<T>;
        }
    }

    public class Point<T> : SetElement<T>, IEquatable<Point<T>>
        where T : IEquatable<T>
    {
        public Point(T x) : base(x) {}

        public bool Equals(Point<T> other)
        {
            throw new NotImplementedException();
        }
    }

    public class Event<T> : Set<Point<T>>, IEquatable<Event<T>>
        where T : IEquatable<T>
    {
        public bool Equals(Event<T> other)
        {
            throw new NotImplementedException();
        }
    }

    public class SampleSpace<T> : Set<Event<T>> 
        where T : IEquatable<T>
    {

    }

    public class Sample<T> : Set<Event<T>>, IEquatable<Sample<T>>
        where T : IEquatable<T>
    {
        public bool Equals(Sample<T> other)
        {
            throw new NotImplementedException();
        }
    }

    public class Variable<T>
    {

    }

    public class RandomVariable<T>
    {

    }
}
