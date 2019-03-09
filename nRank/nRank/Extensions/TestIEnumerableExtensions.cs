using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.Extensions
{
    [TestFixture]
    class TestIEnumerableExtensions
    {
        [Test]
        public void TestWhereIsTrue()
        {
            var table = new[] { 1, 2, 3, 10 };
            var pattern = new[] { true, false, false, true };
            var expectedFilteredTable = new[] { 1, 10 };
            table.WhereIsTrue(pattern).ShouldBe(expectedFilteredTable);
        }
    }
}
