using System;
using System.Collections.Generic;
using System.Text;
using Limits.ElemPrimitive;
using Limits.ElementTyped;

namespace Limits.Graph
{
    public class Node : IEquatable<Node>
    {
        public bool Equals(Node other)
        {
            return other.GetHashCode() == GetHashCode();
        }
    }

    // An (undirected) graph is an ordered pair of a set of nodes and a set of 
    // (unordered) pairs of nodes (denoting the relationships between them).
    // Here we find the typing problem. The tuples currently support only a 
    // single type of element. However, the definition of graph is of a tuple 
    // with two different types of elements. The tuples in the system (C#) are 
    // element-typed, but we should use no system types.
    // A directed graph is like an undirected graph, but the pairs of nodes are 
    // ordered (denoting the directions of the relationships).
    // TODO: Because the relationships unordered pairs have a constraint of 
    // unique elements (a-priori avoiding node-to-itself relations), the structure 
    // is a two-set (implement it).
    // For now, we let simply the order on the relationships pairs be ignored 
    // for the undirected graphs.

    // Another definition: a graph is NOT an element-typed set: there exist no 
    // element-typed sets. That
    // Element 1: A set V of vertexes
    // Element 2: A relation R on V
    public class Graph : Tuple<Set<Node>, Set<Limits.ElemPrimitive.Tuple<Node>>>
    {
        public Graph(Set<Node> a, Set<Limits.ElemPrimitive.Tuple<Node>> b) : 
            base(a, b) { }
    }
}
