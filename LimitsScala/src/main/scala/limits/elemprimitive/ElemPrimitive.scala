package limits.elemprimitive

sealed trait SetItem

case class Set[T](
  private val elems: scala.collection.mutable.Set[SetItem] = scala.collection.mutable.Set[SetItem]()
) extends SetItem
{
  var valueCount: Int = 0 // Counts only SetElements

  def add(elem: SetItem): Unit = {
    if (elems contains elem) return
    elems += elem
    if (elem.isInstanceOf[SetElement[T]]) valueCount += 1
  }

  //  def contains(set: Set[T]) = elems contains set
}

case class SetElement[T](x: T) extends SetItem {
}