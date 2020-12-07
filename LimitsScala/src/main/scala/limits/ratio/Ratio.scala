package limits.ratio

sealed trait InfInt
case object InfInt {
  implicit def IntToInfInt(x: Int): InfInt = MyInt(x)
}

case class MyInt(x: Int) extends InfInt
case object MyInt {
  implicit def IntToMyInt(x: Int): MyInt = MyInt(x)
}

case object Inf extends InfInt

case class Quantity(
  override val num: InfInt,
  override val den: InfInt
) extends Ratio[InfInt](num, den)
  with Ordered[Quantity]
{
  override def compare(that: Quantity): Int = if (num == MyInt(1) && den == MyInt(1) && that.num == Inf && that.den == Inf) 0 else -1
    // PAROU: avaliar o sistema de frações para
    // extrair as propriedades de comparação
}

class Ratio[T](
  val num: T,
  val den: T
)
{

}
