using nRank.DataStructures;
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

        public IApproximation GetApproximation(IUnion union, IInformationTable originalTable)
        {
            var lowerApproximation = LowerApproximationGenerator
                .GetApproximation(union, originalTable);
            var upperApproximation = UpperApproximationGenerator
                .GetApproximation(union, originalTable);

            var lowerItems = lowerApproximation.ApproximatedInformationTable.GetAllObjectIdentifiers();
            var mask = upperApproximation.ApproximatedInformationTable.GetAllObjectIdentifiers().ToDictionary(x => x, x => !lowerItems.Contains(x));

            return new Approximation(upperApproximation.ApproximatedInformationTable.Filter(mask), originalTable, new[] { union.Classes.Max(), union.Classes.Max() + 1 }, new string[] {">=", "<=" }, $"Cl{union.Classes.Max()} u Cl{union.Classes.Max()+1}");
        }
    }
}
