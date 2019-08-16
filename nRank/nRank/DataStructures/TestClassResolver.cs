using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using NSubstitute;
using nRank.VCDomLEMAbstractions;

namespace nRank.DataStructures
{
    [TestFixture]
    class TestClassResolver
    {
        [Test]
        public void TestGetMostPossibleClass()
        {
            var rule = Substitute.For<IDecisionRule>();
            rule.Accuracy.Returns(0f);
            rule.Classes.Returns(new HashSet<int> { 1 });
            var resolver = new ClassResolver(rule);
            resolver.GetMostPossibleClass().ShouldBe(1);
        }

        [Test]
        public void TestGetMostPossibleClassWithAddition()
        {
            var rule = Substitute.For<IDecisionRule>();
            rule.Accuracy.Returns(0f);
            rule.Classes.Returns(new HashSet<int> { 1 });
            var resolver1 = new ClassResolver(rule);

            var rule2 = Substitute.For<IDecisionRule>();
            rule2.Accuracy.Returns(0.1f);
            rule2.Classes.Returns(new HashSet<int> { 2 });
            var resolver2 = new ClassResolver(rule2);

            var resolver = resolver1 + resolver2;

            resolver.GetMostPossibleClass().ShouldBe(1);
        }
    }
}
