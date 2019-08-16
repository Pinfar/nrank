using nRank.console.FileProcessors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.console
{
    class Program
    {
        static void Main(string[] args)
        {
            string file;
            if (args.Length < 1)
            {
                Console.WriteLine("Insert experiment file name!");
                file = Console.ReadLine();
            }
            else
            {
                file = args[0];
            }
            var path = Path.Combine(".", file);

            string consistency;
            if (args.Length < 2)
            {
                Console.WriteLine("Insert consistency treshold!");
                consistency = Console.ReadLine();
            }
            else
            {
                consistency = args[1];
            }
            float consistencyValue = float.Parse(consistency);

            var reader = new InformationTableReader();
            var table = reader.Read(path);
            var vcDomLem = new VCDomLEM();
            var model = vcDomLem.GenerateDecisionRules(table, consistencyValue);
            //var coveredItems = rules
            //    .Select(x => x.GetCoveredItems())
            //    .Select(x => $"{{ {string.Join(", ", x)} }}");
            var result = model.Rules
                //.Zip(coveredItems, (x, y) => $"{x.ToString()} z a = {x.Accuracy} {y}")
                .Select(x => $"{x.ToString()} z a = {x.Accuracy}")
                .ToList();

            var predicted = model.Predict(table.GetAllObjectIdentifiers().ToList(), table);
            var all = table.GetDecisionAttribute().Zip(predicted, (x, y) => new { shouldbe = x, was = y }).ToDictionary(x => x.shouldbe.Key, x => new { shouldbe = x.shouldbe.Value, x.was });
            var resultDir = Path.GetFileNameWithoutExtension(file);
            Directory.CreateDirectory(Path.Combine(".",resultDir));
            File.WriteAllLines(Path.Combine(resultDir, "result.txt"), result);
        }
    }
}
