using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Limits.PredImpl
{
    public static class F
    {
        public static Func<T, bool> Iff<T>(Func<T, bool> f)
        {
            return (T obj) => f(obj);
        }
    }

    #region Exceptions

    #endregion
}
