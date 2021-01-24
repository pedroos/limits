namespace LimitsFsharp

module ElemAsSet = 
    type Set<'t>(elems: seq<Set<'t>>) = 
        class end

    type Singleton<'t>(x: option<'t>) = 
        inherit Set<'t>([]) 
        member this.x: option<'t> = x

    type Empty<'t>() = 
        inherit Singleton<'t>(None)
    
    (* 
    - An ordered pair (a,b) is defined as the set {{a},{a,b}}
    - (b,a) would be {{b},{a,b}} or {{b},{b,a}} or {{b,a},{b}}
    - Ordered pair equality should reduce to set equality (test for that)
    *)
    
    (* 
    Create an ordered pair from two sets A, B as a set of form {A, {A, B}} 
    *)
    type OrderedPair<'t>(a: Set<'t>, b: Set<'t>) = 
        inherit Set<'t>([a; new Set<'t>([a; b])])

    (*
    - An ordered triple (a,b,c) is defined as ((a,b),c) which is {{{a},{a,c}},{b}}
    - (b,c,a) would be {{{b},{b,c}},{a}} or {{a},{{b,c},{c}}}
    - (a,b,c,d) would be (((a,b),c),d)
    *)
    type OrderedTriple<'t>(a: Set<'t>, b: Set<'t>, c: Set<'t>) = 
        inherit OrderedPair<'t>(new OrderedPair<'t>(a, b), c)
