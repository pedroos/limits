An exploration of foundations of mathematics in programming languages. Begun with C# and Object Orientation modeling Sets, Tuples and progressing towards other objects.

#### Description of some namespaces in the C# project

- ElemPrimitive: an implementation of a Set in Inheritance by modeling a Set Element with a value which is also a Set. It is possible to verify element membership and set equality, including nested Sets. Also defines recursive (single-typed) Ordered Tuples, and tries to implement converting to/from Sets.

- ThirdPriori: some logic for Relations (as Sets) and for checking (at runtime) their various properties. Needing porting to the above Set class.

- ElementTyped: recursively defined Tuples with Types per-Element and implicit conversion to Set Elements.

- RecordGraph: with Sets and Relations, Graphs are possible; it is possible to implement Graph logic by handling Set events.

- QuantDescr: an initial test of a notion of fixed-size Collections defined by Quantities and Positions, i.e.: a List containing two Integers, plus a String in the second position. More specifically, the interactions between Quantities and Positions. Could be termed an 'Irregular Collection' or a 'Family'.

- NewDefs: an yet unfinished mapping of possible permutations of types of Collections according to Ordering, Uniqueness of Elements, and Typedness (Untyped, Singly-Typed or Type-Per-Element).

- ElemObjs: a Universe which formalizes Sets when they are included as children of the first Set or one of its children, by a system similar to references. All elements are subclasses of Set, forming a Set Graph. Set equality can be checked by reference, or by Set value, computed from the Set to its children (both procedures should give the same results). Additionally, the reference system provides differentiating potentially valueless objects, like Vertices without any additional information. Sets are untyped.

- ElemObjsUnf: (in progress) removing formalization from ElemObjs to explore Sets as Graphs, using structural equality instead (values/GetHashCode()). Set elements without elements, as Urelements. SetElement is now the parent class of Sets and Urelements, allowing graph traversal: Sets have sub-elements and Urelements don't. The Regularity Axiom as cycle checking, toggable per-Set.

- PredColl: exploration of irregular collections as collections with predicates.

- ElemAsRelation: per the Wikipedia article on Urelements, modeling a Set element as a Set with a unary relation, and possibilities from modeling properties as unary relations in objects.

As of Aug/2020, the code is progressively being ported to *Scala*. The two main motivations are:
* Explore class generic covariance, where type parameter covariance implementing an Interface in C# is uncomfortable
* Explore Sum types for communal supertypes without inheritance (without discarding Object Orientation)

#### Scala packages

- ElemPrimitive: Sum type for Set element. Set and Set element can be used interchangeably through a SetItem "supertype"; the lack of inheritance expresses the lack of similarity between a Set element and a Set.
