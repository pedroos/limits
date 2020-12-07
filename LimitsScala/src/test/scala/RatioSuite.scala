import org.scalatest.funsuite.AnyFunSuite
import limits.ratio.{Quantity, Ratio, InfInt, MyInt, Inf}

class RatioSuite extends AnyFunSuite {
  test(testName="QuantityEquality") {
    assert(Quantity(1, 1) == Quantity(1, 1))
    assert(Quantity(1, 1) != Quantity(2, 1))
    assert(Quantity(1, 1) != Quantity(1, 2))
    assert(Quantity(Inf, Inf) == Quantity(Inf, Inf))
    assert(Quantity(1, Inf) == Quantity(1, Inf))
    assert(Quantity(1, Inf) != Quantity(Inf, Inf))
    assert(Quantity(1, Inf) != Quantity(Inf, 1))
  }
  test(testName="QuantityOrder") {
    assert(!(Quantity(1, 1) < Quantity(Inf, Inf)))
  }
}
