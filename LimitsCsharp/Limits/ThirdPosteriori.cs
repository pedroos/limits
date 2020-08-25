using System;
using System.Collections.Generic;
using System.Linq;

namespace Limits.ThirdPosteriori
{
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
        where T : IEquatable<T> { }

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

    [TransitiveRelation]
    [SymmetricRelation]
    public class TransitiveSymmetricRelation2<T> : Relation2<T>
        where T : IEquatable<T> { }

    public class Set<T>
        where T : IEquatable<T>
    {
        public List<T> elems;
        public List<Relation2<T>> relations;

        public Set()
        {
            elems = new List<T>();
            relations = new List<Relation2<T>>();
        }

        /// <returns>The relation kind by which the result was found, and the result.</returns>
        public ValueTuple<Type, bool> Is<TRelation>(T a, T b)
            where TRelation:Relation2<T>
        {
            Type relationType = typeof(TRelation);
            var relation = relations.FirstOrDefault(r => r.GetType() == relationType);
            if (relation == null) return (typeof(RelationKindAttribute), false);

            if (relationType.GetCustomAttributes(typeof(SymmetricRelationAttribute), true).Length > 0)
            {
                if (relation.elems.Any(e => e.Equals(new OrderedTuple2<T>(b, a))))
                    return (typeof(SymmetricRelationAttribute), true);
            }

            if (relationType.GetCustomAttributes(typeof(ReflexiveRelationAttribute), true).Length > 0) 
            {
                if (a.Equals(b) && elems.Contains(a))
                    return (typeof(ReflexiveRelationAttribute), true);
            }

            if (relationType.GetCustomAttributes(typeof(TransitiveRelationAttribute), true).Length > 0)
            {
                // Searches for x related to both a and b
                var xWithA = relation.elems.Where(e => e.a.Equals(a)).Select(e => e.b);
                foreach (var x in xWithA)
                {
                    if (relation.elems.Any(e => e.a.Equals(x) && e.b.Equals(b)))
                        return (typeof(TransitiveRelationAttribute), true);
                }
            }

            if (relation.elems.Any(
                e => e.Equals(new OrderedTuple2<T>(a, b))))
                return (typeof(RelationKindAttribute), true);

            return (typeof(RelationKindAttribute), false);
        }
    }
}
