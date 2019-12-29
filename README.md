# limits
An exploration of modeling set-theoretic and other objects in C#.

The objective is to model a simplified verifiable set of behaviors to build more elaborate structures atop.

This repository is in flux and is being constantly changed.

Some of the most worked on namespaces are up to now:

- ElemPrimitive: an implementation of a Set in Inheritance by modeling a Set Element with a value which is also a Set. It is possible to verify element membership and set equality, including nested Sets. Also defines recursive (single-typed) Ordered Tuples, and tries to implement converting to/from Sets.

- ThirdPriori: some logic for Relations (as Sets) and for checking (at runtime) their various properties. Needing porting to the above Set class.

- ElementTyped: recursively defined Tuples with Types per-Element and implicit conversion to Set Elements.

- RecordGraph: with Sets and Relations, Graphs are possible; according to Chartrand (1), "graphs are (...) a set of vertices together with a relation on this set". 'togetherness' is not an Ordered Pair (because Pairs are positional), and element order is not relevant -- only that one of each of the elements exists. Other than a fixed-site Set with individual Types for each Element (which seems departed from the concept of Set), a Record seems to fit, being the single structure which can address its Elements directly, and having Types-per-Element. A class in C# is analog to a Record, so that's what's used. It is possible to implement Graph logic by handling Set events.

- QuantDescr: an initial test of a notion of fixed-size Collections defined by Quantities and Positions, i.e.: a List containing two Integers, plus a String in the second position. More specifically, the interactions between Quantities and Positions. Could be termed an 'Irregular Collection' or a 'Family'.

- NewDefs: an yet unfinished mapping of possible permutations of types of Collections according to Ordering, Uniqueness of Elements, and Typedness (Untyped, Singly-Typed or Type-Per-Element).

- ElemObjs: a Set formalization scheme; unformalized Sets don't equal anything including themselves, until they are included in a parent Set, when they become uniquely identifiable across all present or future parent Sets. Valueless objects can be distinct; all objects inherit from Set, which are untyped. An Universe implementation with a single initial Set providing a Set tree.