using nRank.UnionGenerators;
using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.ApproximationsGenerators
{
    class ApproximationsGenerator : IAllApproximationsGenerator
    {
        public IEnumerable<IApproximation> GetApproximations(IInformationTable originalTable, float consistencyLevel)
        {            
            var uuGenerator = new UpwardUnionGenerator();
            var upwardUnions = uuGenerator.GenerateUnions(originalTable).ToList();
            var firstType = new LowerApproximationOfUpwardUnionGenerator();
            foreach(var union in upwardUnions)
            {
                yield return firstType.GetApproximation(union, originalTable);
            }
            //var secondType = new UpperApproximationOfUpwardUnionGenerator();
            //foreach (var union in upwardUnions)
            //{
            //    yield return secondType.GetApproximation(union, originalTable);
            //}

            var duGenerator = new DownwardUnionGenerator();
            var downwardUnions = duGenerator.GenerateUnions(originalTable).ToList();
            var thrirdType = new LowerApproximationOfDownwardUnionGenerator();
            foreach (var union in downwardUnions)
            {
                yield return thrirdType.GetApproximation(union, originalTable);
            }
            //var fourthType = new UpperApproximationOfDownwardUnionGenerator();
            //foreach (var union in downwardUnions)
            //{
            //    yield return fourthType.GetApproximation(union, originalTable);
            //}

            var fifthType = new BoundaryApproximationGenerator(new LowerApproximationOfDownwardUnionGenerator(), new UpperApproximationOfDownwardUnionGenerator());
            foreach (var union in downwardUnions)
            {
                yield return fifthType.GetApproximation(union, originalTable);
            }
        }
    }
}
