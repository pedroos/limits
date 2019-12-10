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
            return new Set<Event<T>>(new List<Set<Event<T>>> { }) 
                as SampleSpace<T>;
        }
    }

    public class Point<T> : SetElement<T> 
        where T : IEquatable<T>
    {
        public Point(T x) : base(x) {}
    }

    public class Event<T> : Set<Point<T>> 
        where T : IEquatable<T>
    {
    }

    public class SampleSpace<T> : Set<Event<T>> 
        where T : IEquatable<T>
    {

    }

    public class Sample<T> : Set<Event<T>> 
        where T : IEquatable<T>
    {

    }

    public class Variable<T>
    {

    }

    public class RandomVariable<T>
    {

    }
}
