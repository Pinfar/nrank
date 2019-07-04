using nRank.TestCommons;
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
            var informationTable = new InformationTableGenerator().GetInformationTable();
            var decisionRules = new VCDomLEM().GenerateDecisionRules(informationTable, 0.8f);
        }
    }
}
