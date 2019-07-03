using nRank.DSetGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.ApproximationsGeneratorsVC
{
    class UpperApproximationOfUpwardUnionGeneratorVC : AbstractUpperApproximationGeneratorVC<DDominatedSetGenerator>
    {
        protected override AbstractLowerApproximationGeneratorVC<DDominatedSetGenerator> _oppositeGenerator => new LowerApproximationOfDownwardUnionGeneratorVC();

        protected override IEnumerable<string> _allowedOperators => new[] { ">=" };
    }
}
