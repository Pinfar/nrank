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

        [Test]
        public void TestToDictionary()
        {
            var values = new[] { 1, 5, 6, 3, 5 };
            var expectedDictionary = new Dictionary<string, int>
            {
                {"0", 1 },
                {"1", 5 },
                {"2", 6 },
                {"3", 3 },
                {"4", 5 }
            };
            values.ToDictionary().ShouldBe(expectedDictionary);
        }

        [Test]
        public void TestIsSubsetOf()
        {
            new[] { 1, 2, 3 }.IsSubsetOf(new[] { 1, 2, 3 }).ShouldBeTrue();
            new[] { 1, 2 }.IsSubsetOf(new[] { 1, 2, 3 }).ShouldBeTrue();
            new[] { 1, 2, 3 }.IsSubsetOf(new[] { 1, 2 }).ShouldBeFalse();
        }
    }
}
