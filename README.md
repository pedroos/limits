# limits
An exploration of modeling set-theoretic and other related objects in C#.

The objective is to model a simplified verifiable set of behaviors to build more elaborate structures atop.

This repository is in flux and is being constantly changed.

Some of the most developed namespaces are up to now:

- ElemPrimitive: an implementation of a Set in Inheritance by modeling a Set Element with a value which is also a Set. It is possible to verify element membership and set equality, including nested Sets. Also defines recursive (single-typed) Ordered Tuples, and tries to implement converting to/from Sets.

- ThirdPriori: some logic for Relations (as Sets) and for checking (at runtime) their various properties. Needing porting to the above Set class.

- ElementTyped: recursively defined Tuples with Types per-Element and implicit conversion to Set Elements.

- RecordGraph: with Sets and Relations, Graphs are possible; according to Chartrand [1], "graphs are (...) a set of vertices together with a relation on this set". 'togetherness' is not an Ordered Pair (because Pairs are positional), and element order is not relevant -- only that one of each of the elements exists. Other than a fixed-size Set with individual Types for each Element (which seems departed from the concept of Set), a Record seems to fit, being the single structure which can address its Elements directly, and having Types-per-Element. A class in C# is analog to a Record, so that's what's used. It is possible to implement Graph logic by handling Set events.

- QuantDescr: an initial test of a notion of fixed-size Collections defined by Quantities and Positions, i.e.: a List containing two Integers, plus a String in the second position. More specifically, the interactions between Quantities and Positions. Could be termed an 'Irregular Collection' or a 'Family'.

- NewDefs: an yet unfinished mapping of possible permutations of types of Collections according to Ordering, Uniqueness of Elements, and Typedness (Untyped, Singly-Typed or Type-Per-Element).

- ElemObjs: a Universe which formalizes Sets when they are included as children of the first Set or one of its children, by a system similar to references. All elements are subclasses of Set, forming a Set Graph. Set equality can be checked by reference, or by Set value, computed from the Set to its children (both procedures should give the same results). Additionally, the reference system provides differentiating potentially valueless objects, like Vertices without any additional information. Sets are untyped.

- ElemObjsUnf: (in progress) removing formalization from ElemObjs to explore Sets as Graphs, using structural equality instead (values/GetHashCode()). Identified SetElements, the set elements without elements, as Urelements [2] instead. SetElement is now the parent class of Sets and Urelements, allowing graph traversal: Sets have sub-elements and Urelements don't. The Regularity Axiom implemented as cycle checking. The Axiom is togglable on a per-Set basis, allowing activation and deactivation for new sub-elements if the conditions are met.

<<<<<<< HEAD
- PredColl: an implementation of irregular collections as collections plus collection predicates and element predicates. Collection predicates specify constraints on parts of the collection and element predicates specify conditions on elements, such that the same element conditions used to constrain and guarantee properties about the inserted elements of the collection can be used to retrieve and identify the elements. Collection predicates for quantities and positions are included. More or less models the behavior of classes (with untyped members), with the difference that the collection specification can be changed at runtime.
=======
- PredColl: an implementation of irregular collections as collections plus collection predicates and element predicates. Collection predicates specify constraints on parts of the collection and element predicates specify conditions on elements, such that the same element conditions used to constrain and guarantee properties about the inserted elements of the collection can be used to retrieve and identify them. Collection predicates for quantities and positions are included. More or less models the behavior of classes (with untyped members), with the difference that the collection specification can be changed at runtime.
>>>>>>> f331bafa1147923f91e6791543b383727507a027

References:

1. Chartrand, Gary. 1984. Introductory Graph Theory. New York: Dover Publications.
2. https://en.wikipedia.org/wiki/Urelement
