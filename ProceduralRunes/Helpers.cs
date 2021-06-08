using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProceduralRunes
{
    public static class Helpers
    {
        public static T Random<T>(this IEnumerable<T> elements, Random rnd = null)
        {
            if (rnd == null)
            {
                rnd = new Random();
            }
            var index = rnd.Next(elements.Count());
            return elements.ElementAt(index);
        }

        public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> elements, out T randomElem, Random rnd = null)
        {
            if (rnd == null)
            {
                rnd = new Random();
            }
            var index = rnd.Next(elements.Count());
            randomElem = elements.ElementAt(index);
            return elements.Take(index).Concat(elements.Skip(index + 1).Take(elements.Count()));
        }
    }
}
