using nRank.console.FileProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.console
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"AirlinesIntCardinal.isf";
            var reader = new InformationTableReader();
            var table = reader.Read(path);
            var vcDomLem = new VCDomLEM();
            var rules = vcDomLem.GenerateDecisionRules(table, 0.2f);
            var coveredItems = rules
                .Select(x => x.GetCoveredItems())
                .Select(x => $"{{ {string.Join(" ,", x)} }}");
            var result = rules
                .Zip(coveredItems, (x, y) => $"{x.ToString()} z a = {x.Accuracy} {y}")
                .ToList();
        }
    }
}
