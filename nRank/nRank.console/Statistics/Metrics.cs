using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.console.Statistics
{
    static class Metrics
    {
        public static double RMSE(IEnumerable<int> expected, IEnumerable<int> actual)
        {
            var mse = expected
                .Zip(actual, (x, y) => (x - y) * (x - y))
                .Average();
            var rmse = Math.Sqrt(mse);
            return rmse;
        }
    }
}
