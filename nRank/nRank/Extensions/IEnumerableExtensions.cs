using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.Extensions
{
    static class IEnumerableExtensions
    {
        public static IEnumerable<T> WhereIsTrue<T>(this IEnumerable<T> source, IEnumerable<bool> filterPattern)
        {
            return source
                .Zip(filterPattern, (item, includeInResult) => new { item, includeInResult })
                .Where(x => x.includeInResult)
                .Select(x => x.item);
        }

        public static Dictionary<string, T> ToDictionary<T>(this IEnumerable<T> source)
        {
            return source
                .Select((x, i) => new { Key = i.ToString(), Value = x })
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public static bool IsSubsetOf<T>( this IEnumerable<T> first, IEnumerable<T> second )
        {
            var set = new HashSet<T>(second);
            return first.All(x => set.Contains(x));
        }
    }
}
