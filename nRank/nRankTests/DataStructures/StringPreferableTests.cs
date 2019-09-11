using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using nRank.PairwiseDRSA;
using NSubstitute;

namespace nRankTests.DataStructures
{
    [TestFixture]
    class StringPreferableTests
    {
        List<string> _order =  new List<string> { "weak", "average", "best" };

        [Test]
        public void PreferenceTest()
        {
            var strpref1 = new StringPreferable("best", _order);
            var strpref2 = new StringPreferable("weak", _order);
            strpref1.IsWeaklyPreferedTo(strpref2).ShouldBeTrue();
            strpref2.IsWeaklyPreferedTo(strpref1).ShouldBeFalse();
            strpref1.IsWeaklyPreferedTo(strpref1).ShouldBeTrue();
        }

        [Test]
        public void InvalidInitiationTest()
        {
            Should.Throw<InvalidOperationException>(() => new StringPreferable("good", _order));
        }

        [Test]
        public void InvalidTypePreferenceTest()
        {
            var strpref1 = new StringPreferable("best", _order);
            var strpref2 = Substitute.For<IPreferable>();
            Should.Throw<InvalidOperationException>(() => strpref1.IsWeaklyPreferedTo(strpref2));
        }

        [Test]
        public void UnevenPreferenceTest()
        {
            var strpref1 = new StringPreferable("best", _order);
            var strpref2 = new StringPreferable("best", new List<string> { "weak", "best", "average" });
            Should.Throw<InvalidOperationException>(() => strpref1.IsWeaklyPreferedTo(strpref2));
        }
    }
}
