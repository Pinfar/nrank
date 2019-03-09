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
    }
}
