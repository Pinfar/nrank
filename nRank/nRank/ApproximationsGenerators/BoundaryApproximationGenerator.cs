using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.ApproximationsGenerators
{
    class BoundaryApproximationGenerator : IApproximationsGenerator
    {
        private IApproximationsGenerator LowerApproximationGenerator;
        private IApproximationsGenerator UpperApproximationGenerator;

        public BoundaryApproximationGenerator(IApproximationsGenerator lowerApproximationGenerator, IApproximationsGenerator upperApproximationGenerator)
        {
            LowerApproximationGenerator = lowerApproximationGenerator;
            UpperApproximationGenerator = upperApproximationGenerator;
        }

        public IInformationTable GetApproximation(IInformationTable union, IInformationTable originalTable)
        {
            var lowerApproximation = LowerApproximationGenerator
                .GetApproximation(union, originalTable);
            var upperApproximation = UpperApproximationGenerator
                .GetApproximation(union, originalTable);

            var lowerItems = lowerApproximation.GetAllObjectIdentifiers();
            var mask = upperApproximation.GetAllObjectIdentifiers().ToDictionary(x => x, x => !lowerItems.Contains(x));

            return upperApproximation.Filter(mask);
        }
    }
}
