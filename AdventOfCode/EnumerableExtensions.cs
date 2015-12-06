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

        public static int Count<T>(this T[,] mesh, Predicate<T> predicate)
        {
            int count = 0;
            foreach (T item in mesh)
            {
                if (predicate(item))
                {
                    ++count;
                }
            }
            return count;
        }
    }
}
