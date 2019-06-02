using nRank.VCDomLEMAbstractions;
using NSubstitute;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank
{
    [TestFixture]
    class TestVCDomLEM
    {
        [Test]
        public void GenerateDecisionRules()
        {
            var decisionRuleGenerator = Substitute.For<IDecisionRuleGenerator>();
            var roughSetGenerator = Substitute.For<IUnionGenerator>();
            var informationTable = Substitute.For<IInformationTable>();
            var objectFilter = Substitute.For<IApproximationsGenerator>();
            var lowerApproximation1 = Substitute.For<IInformationTable>();
            var lowerApproximation2 = Substitute.For<IInformationTable>();
            roughSetGenerator
                .GenerateUnions(informationTable)
                .Returns(new List<IInformationTable> { lowerApproximation1, lowerApproximation2 });
            var rule1 = Substitute.For<IDecisionRule>();
            var rule2 = Substitute.For<IDecisionRule>();
            var rule3 = Substitute.For<IDecisionRule>();
            objectFilter.GetApproximation(lowerApproximation1, informationTable).Returns(lowerApproximation1);
            objectFilter.GetApproximation(lowerApproximation2, informationTable).Returns(lowerApproximation2);
            decisionRuleGenerator
                .GenerateRulesFrom(lowerApproximation1)
                .Returns(new List<IDecisionRule> { rule1, rule2 });
            decisionRuleGenerator
                .GenerateRulesFrom(lowerApproximation2)
                .Returns(new List<IDecisionRule> { rule2, rule3 });

            var domlem = new VCDomLEM(roughSetGenerator, decisionRuleGenerator, objectFilter);
            var generatedRules = domlem.GenerateDecisionRules(informationTable);

            generatedRules.ShouldBe(new HashSet<IDecisionRule> { rule1, rule2, rule3 });
        }
    }
}
