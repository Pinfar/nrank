using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace nRank.console.Statistics
{
    [TestFixture]
    class MetricsTest
    {
        [Test]
        public void TestRmse()
        {
            Metrics.RMSE(new[] { 1, 2 }, new[] { 2, 3 }).ShouldBe(1);
            Metrics.RMSE(new[] { 1, 2 }, new[] { 4, 5 }).ShouldBe(3);
        }

    }
}
