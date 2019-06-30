using nRank.DSetGenerators;
using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.ApproximationsGenerators
{
    //B_(Clt>=)
    class LowerApproximationOfUpwardUnionGenerator : AbstractLowerApproximationGenerator<DDominatingSetGenerator>
    {
        protected override IEnumerable<string> _allowedOperators => new[] { ">=" };
    }
}
