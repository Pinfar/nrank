using nRank.DSetGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.ApproximationsGeneratorsVC
{
    class LowerApproximationOfDownwardUnionGeneratorVC : AbstractLowerApproximationGeneratorVC<DDominatedSetGenerator>
    {
        protected override string _allowedGainOperator =>  "<=" ;
    }
}
