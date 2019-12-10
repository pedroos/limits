using System;
using System.Collections.Generic;
using System.Text;
using Limits.ElementTyped;
using Limits.ElemPrimitive;

namespace Limits.Graph
{
    public class Node : IEquatable<Node>
    {
        public bool Equals(Node other)
        {
            throw new NotImplementedException();
        }
    }

    // An (undirected) graph is an ordered pair of a set of nodes and a set of 
    // (unordered) pairs of nodes (denoting the relationships between them).
    // Here we find the typing problem. The tuples currently support only a 
    // single type of element. However, the definition of graph is of a tuple 
    // with two different types of elements.
    // A directed graph is like an undirected graph, but the pairs of nodes are 
    // ordered (denoting the directions of the relationships).
    // TODO: Because the relationships unordered pairs have a constraint of 
    // unique elements (a-priori avoiding node-to-itself relations), the structure 
    // is a two-set (implement it).
    // For now, we let simply the order on the relationships pairs be ignored 
    // for the undirected graphs.
    public class Graph : Tuple2<Set<Node>, Set<Limits.ElemPrimitive.Tuple<Node>>>
    {
        public Graph(Set<Node> a, Set<Limits.ElemPrimitive.Tuple<Node>> b) : 
            base(a, b) { }
    }
}
