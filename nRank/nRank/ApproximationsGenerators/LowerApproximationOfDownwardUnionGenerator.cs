﻿using nRank.DSetGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.ApproximationsGenerators
{
    class LowerApproximationOfDownwardUnionGenerator : AbstractLowerApproximationGenerator<DDominatedSetGenerator>
    {
        protected override IEnumerable<string> _allowedOperators => new[] { "<=" };
    }
}
