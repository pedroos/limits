using System;
using System.Collections.Generic;
using System.Linq;

namespace Limits.ThirdPriori
{
    public class Set<T>
        where T : IEquatable<T>
    {
        protected HashSet<T> elems;
        public List<Relation2<T>> relations;

        public virtual void Add(T elem)
        {
            if (!elems.Add(elem)) return;
        }

        public Set()
        {
            elems = new HashSet<T>();
            relations = new List<Relation2<T>>();
        }

        public bool Is<TRelation>(T a, T b)
            where TRelation : Relation2<T>
        {
            Type relationType = typeof(TRelation);
            var relation = relations.FirstOrDefault(r => r.GetType() == relationType);
            if (relation == null) return false;

            return relation.Is(a, b);
        }
    }

    public class OrderedTuple2<T> : IEquatable<OrderedTuple2<T>>
        where T : IEquatable<T>
    {
        public T a;
        public T b;
        public OrderedTuple2(T a, T b)
        {
            this.a = a;
            this.b = b;
        }

        public bool Equals(OrderedTuple2<T> other)
        {
            return a.Equals(other.a) && b.Equals(other.b);
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public abstract class RelationKindAttribute : Attribute { }
    public class SymmetricRelationAttribute : RelationKindAttribute { }

    public class ReflexiveRelationAttribute : RelationKindAttribute { }

    public class TransitiveRelationAttribute : RelationKindAttribute { }

    public class Relation2<T> : Set<OrderedTuple2<T>>
        where T : IEquatable<T>
    {
        public override void Add(OrderedTuple2<T> elem)
        {
            base.Add(elem);

            if (GetType().GetCustomAttributes(typeof(SymmetricRelationAttribute), true).Length > 0)
            {
                base.Add(new OrderedTuple2<T>(elem.b, elem.a));
            }

            if (GetType().GetCustomAttributes(typeof(ReflexiveRelationAttribute), true).Length > 0)
            {
                base.Add(new OrderedTuple2<T>(elem.a, elem.a));
                base.Add(new OrderedTuple2<T>(elem.b, elem.b));
            }

            if (GetType().GetCustomAttributes(typeof(TransitiveRelationAttribute), true).Length > 0)
            {
                // For all x with whom b relates, inserts (a, x)
                // Para all x which are related to a, inserts (x, b)
                var xWithB = elems.Where(e => e.a.Equals(elem.b)).Select(e => e.b).ToList();
                var xWithA = elems.Where(e => e.b.Equals(elem.a)).Select(e => e.a).ToList();
                foreach (var x in xWithB) 
                    base.Add(new OrderedTuple2<T>(elem.a, x));
                foreach (var x in xWithA)
                    base.Add(new OrderedTuple2<T>(x, elem.b));
            }
        }

        public bool Is(T a, T b)
        {
            return elems.Any(e => e.a.Equals(a) && e.b.Equals(b));
        }
    }

    [SymmetricRelation]
    public class SymmetricRelation2<T> : Relation2<T>
        where T : IEquatable<T> { }

    [ReflexiveRelation]
    public class ReflexiveRelation2<T> : Relation2<T>
        where T : IEquatable<T> { }

    [SymmetricRelation]
    [ReflexiveRelation]
    public class SymmetricReflexiveRelation2<T> : Relation2<T>
        where T : IEquatable<T> { }

    [TransitiveRelation]
    public class TransitiveRelation2<T> : Relation2<T>
        where T : IEquatable<T> { }
}
