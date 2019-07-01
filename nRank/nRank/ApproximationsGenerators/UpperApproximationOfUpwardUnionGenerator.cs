using nRank.DSetGenerators;
using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.ApproximationsGenerators
{
    class UpperApproximationOfUpwardUnionGenerator : AbstractUpperApproximationGenerator<DDominatingSetGenerator>
    {
        protected override IEnumerable<string> _allowedOperators => new[] { ">=" };

        protected override string GetSymbol(IEnumerable<int> classes)
        {
            return $"Cl{classes.Min()}{_allowedOperators.Single()}";
        }
    }
}
