using System;
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

        public LowerApproximationGeneratorVC(IPSetGenerator psetGenerator)
        {
            this.psetGenerator = psetGenerator;
        }

        public PApproximation GetApproximation(List<InformationObjectPair> positiveDefinedPairs, List<InformationObjectPair> negativeDefinedPairs, InformationTable originalTable, float consistencyLevel)
        {
            var positiveDefinedPairsSet = new HashSet<InformationObjectPair>(negativeDefinedPairs);
            var approximation = positiveDefinedPairs
                .Concat(negativeDefinedPairs)
                .Where(x => 
                     IsInApproximationEpsilon(originalTable, x, positiveDefinedPairsSet, consistencyLevel)
                )
                .ToList();

            var positiveRegion = approximation
                .SelectMany(x => psetGenerator.Generate(originalTable, x))
                .Distinct()
                .ToList();
            return new PApproximation(approximation, positiveRegion);
        }

        private bool IsInApproximationEpsilon(InformationTable originalTable, InformationObjectPair obj, HashSet<InformationObjectPair> objectsInNegativeRelation, float consistencyLevel)
        {
            var dset = new HashSet<InformationObjectPair>(psetGenerator.Generate(originalTable, obj));
            var negSet = objectsInNegativeRelation;
            float commonPart = dset.Intersect(negSet).Count();
            float negSetCount = negSet.Count();
            return (commonPart / negSetCount) <= consistencyLevel;
        }
    }
}
