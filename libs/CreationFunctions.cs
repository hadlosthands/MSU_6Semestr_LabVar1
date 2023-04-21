using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libs
{
    public static class CreationFunctions
    {
        public static double LinearFunction(double x)
        {
            return 3 * x + 1;
        }
        public static double CubicPolynomial(double x)
        {
            return 2 * x * x * x;
        }

        public static double PseudoRandomGenerator(double x)
        {
            Random rand = new();
            double val = 20 * rand.NextDouble() - 10;
            return x + val;
        }
    }
}
