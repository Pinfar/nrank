using nRank.UnionGenerators;
using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.ApproximationsGeneratorsVC
{
    class ApproximationsGeneratorVC : IAllApproximationsGenerator
    {
        public IEnumerable<IApproximation> GetApproximations(IInformationTable originalTable, float consistencyLevel)
        {
            var uuGenerator = new UpwardUnionGenerator();
            var upwardUnions = uuGenerator.GenerateUnions(originalTable).ToList();
            var duGenerator = new DownwardUnionGenerator();
            var downwardUnions = duGenerator.GenerateUnions(originalTable).ToList();


            var upwardApproxGenerator = new LowerApproximationOfUpwardUnionGeneratorVC();
            foreach (var union in upwardUnions)
            {
                yield return upwardApproxGenerator.GetApproximation(union, originalTable, consistencyLevel);
            }
            var downwardApproxGenerator = new LowerApproximationOfDownwardUnionGeneratorVC();
            foreach (var union in downwardUnions)
            {
                yield return downwardApproxGenerator.GetApproximation(union, originalTable, consistencyLevel);
            }
        }
    }
}
