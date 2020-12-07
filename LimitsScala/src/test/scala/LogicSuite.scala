import limits.elemprimitive.{Set, SetElement}
import org.scalatest.funsuite.AnyFunSuite

class LogicSuite extends AnyFunSuite {
  test(testName="Logic1") {
    val s = Set[Int]()
    s.add(SetElement(1))
    s.add(SetElement(2))
    s.add(SetElement(3))
  }
}
