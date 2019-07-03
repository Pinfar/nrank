using nRank.DataStructures;
using nRank.DSetGenerators;
using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.ApproximationsGeneratorsVC
{
    class UpperApproximationOfDownwardUnionGeneratorVC : AbstractUpperApproximationGeneratorVC<DDominatingSetGenerator>
    {
        protected override AbstractLowerApproximationGeneratorVC<DDominatingSetGenerator> _oppositeGenerator => new LowerApproximationOfUpwardUnionGeneratorVC();

        protected override IEnumerable<string> _allowedOperators => new[] { "<=" };
    }
}
