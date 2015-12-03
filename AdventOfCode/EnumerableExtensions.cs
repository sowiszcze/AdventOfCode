using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public static class EnumerableExtensions
    {
        public static int Product(this IEnumerable<int> collection)
        {
            int result = 1;
            foreach (int number in collection)
            {
                result *= number;
            }
            return result;
        }
    }
}
