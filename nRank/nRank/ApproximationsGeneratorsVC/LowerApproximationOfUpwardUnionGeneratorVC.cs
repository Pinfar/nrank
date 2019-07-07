using nRank.DSetGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.ApproximationsGeneratorsVC
{
    class LowerApproximationOfUpwardUnionGeneratorVC : AbstractLowerApproximationGeneratorVC<DDominatingSetGenerator>
    {
        protected override string _allowedGainOperator =>  ">=" ;
    }
}
