import org.scalatest.funsuite.AnyFunSuite
import limits.elemprimitive.{Set, SetElement}

class ElemPrimitiveSuite extends AnyFunSuite {
  test(testName="SetValueCount1") {
    val s = Set()
    assert(s.valueCount == 0)
    val e = SetElement()
    s.add(e)
    assert(s.valueCount == 1)
    val s2 = Set()
    s.add(s2)
    assert(s.valueCount == 1)
  }

  test(testName="SetElementEquality1") {
    val e1 = SetElement[Int](1)
    val e2 = SetElement[Int](1)
    assert(e1 == e2)
  }

  test(testName="SetElementEquality2") {
    var e1 = SetElement[Int](1)
    val e2 = SetElement[Int](2)
    assert(e1 != e2)
  }

  test(testName="SetEquality1") {
    val s1 = Set()
    val s2 = Set()
    assert(s1 == s2)
  }

  test(testName="SetEquality2") {
    val s1 = Set()
    val s2 = Set[Int]()
    assert(s1 == s2) // Not expected, but accepted
  }

  test(testName="SetEquality3") {
    val s1 = Set()
    val s2 = Set()
    s1.add(SetElement())
    assert(s1 != s2)
  }

  test(testName="SetEquality5") {
    val s1 = Set()
    val e = SetElement()
    s1.add(e)
    val s2 = Set()
    s2.add(e)
    assert(s1 == s2)
  }

  test(testName="SetEquality6") {
    val s1 = Set()
    val e1 = SetElement[Int](1)
    s1.add(e1)
    val s2 = Set()
    val e2 = SetElement[Int](2)
    s2.add(e2)
    assert(s1 != s2)
  }

  test(testName="SetEquality7") {
    val s1 = Set()
    val e1 = SetElement[Int](1)
    s1.add(e1)
    val s2 = Set()
    val e2 = SetElement[Int](1)
    s2.add(e2)
    assert(s1 == s2)
  }

  test(testName="SetEquality8") {
    val s1 = Set()
    val s2 = Set[Int]()
    val e1 = SetElement[Int](1)
    s2.add(e1)
    s1.add(s2)
    assert(s1 != s2)
  }

  test(testName="SetEquality9") {
    val s1 = Set()
    val s2 = Set[Int]()
    val e1 = SetElement[Int](1)
    s2.add(e1)
    s1.add(s2)
    val s3 = Set()
    val s4 = Set[Int]()
    val e2 = SetElement[Int](1)
    s4.add(e2)
    s3.add(s4)
    assert(s4 == s2)
    assert(s3 == s1)
  }

  test(testName="SetEquality10") {
    val s1 = Set()
    val s2 = Set()
    val e1 = SetElement[Int](1)
    val e2 = SetElement[Int](2)
    s1.add(e1)
    s1.add(e2)
    s2.add(e2)
    s2.add(e1)
    assert(s1 == s2)
  }
}
