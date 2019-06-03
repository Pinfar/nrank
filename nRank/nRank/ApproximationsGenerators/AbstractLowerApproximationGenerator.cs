﻿using nRank.DSetGenerators;
using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.ApproximationsGenerators
{
    abstract class AbstractLowerApproximationGenerator<T> : IApproximationsGenerator where T : IDDSetGenerator, new ()
    {
        public IInformationTable GetApproximation(IInformationTable union, IInformationTable originalTable)
        {
            var dsetGenerator = new T();
            var objectsInUnion = union.GetAllObjectIdentifiers();
            var pattern = originalTable.GetAllObjectIdentifiers()
                .ToDictionary(
                    x => x,
                    x => dsetGenerator.Generate(originalTable, x).GetAllObjectIdentifiers().All(y => objectsInUnion.Contains(y))
                );
            return originalTable.Filter(pattern);
        }
    }
}
