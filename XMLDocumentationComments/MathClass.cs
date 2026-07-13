using System;
using System.Collections.Generic;
using System.Text;

namespace XMLDocumentationComments
{
    internal class MathClass
    {
        /// <summary>
        /// Divides two numbers.
        /// </summary>
        /// <param name="a">Dividend.</param>
        /// <param name="b">Divisor.</param>
        /// <returns>The division result.</returns>
        /// <exception cref="DivideByZeroException">
        /// Thrown when b is zero.
        /// </exception>
        public double Divide(double a, double b)
        {
            return a / b;
        }
    }
}
