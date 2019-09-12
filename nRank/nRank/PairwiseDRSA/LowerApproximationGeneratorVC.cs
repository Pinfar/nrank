﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.PairwiseDRSA
{
    class LowerApproximationGeneratorVC
    {
        //abstract protected string _allowedGainOperator { get; }

        private IPSetGenerator psetGenerator;
        private PairwiseComparisonTable.RelationType _relation;

        public LowerApproximationGeneratorVC(PairwiseComparisonTable.RelationType relation)
        {
            _relation = relation;
            if (_relation == PairwiseComparisonTable.RelationType.S)
            {
                psetGenerator = new PDominatingSetGenerator();
            }
            else
            {
                psetGenerator = new PDominatedSetGenerator();
            }
        }

        public PApproximation GetApproximation(PairwiseComparisonTable table, InformationTable originalTable, float consistencyLevel)
        {
            List<InformationObjectPair> positiveDefinedPairs = table.Filter(x => x.Relation == _relation).AsInformationObjectPairs();
            List<InformationObjectPair> negativeDefinedPairs = table.Filter(x => x.Relation != _relation).AsInformationObjectPairs();
            var negativeDefinedPairsSet = new HashSet<InformationObjectPair>(negativeDefinedPairs);
            var approximation = positiveDefinedPairs
                .Concat(negativeDefinedPairs)
                .Where(x => 
                     IsInApproximationEpsilon(originalTable, x, negativeDefinedPairsSet, consistencyLevel)
                )
                .ToList();

            var positiveRegion = approximation
                .SelectMany(x => psetGenerator.Generate(originalTable, x))
                .Distinct()
                .ToList();
            return new PApproximation(approximation, positiveRegion, table);
        }

        private bool IsInApproximationEpsilon(InformationTable originalTable, InformationObjectPair obj, HashSet<InformationObjectPair> objectsInNegativeRelation, float consistencyLevel)
        {
            var dset = new HashSet<InformationObjectPair>(psetGenerator.Generate(originalTable, obj));
            float commonPart = dset.Intersect(objectsInNegativeRelation).Count();
            float negSetCount = objectsInNegativeRelation.Count();
            return (commonPart / negSetCount) <= consistencyLevel;
        }
    }
}
