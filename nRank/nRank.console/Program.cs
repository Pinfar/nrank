using nRank.console.FileProcessors;
using nRank.VCDomLEMAbstractions;
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
            string file, path, consistency;
            HandleInputParams(args, out file, out path, out consistency);
            float consistencyValue = float.Parse(consistency);

            var reader = new InformationTableReader();
            var table = reader.Read(path);
            var vcDomLem = new VCDomLEM();
            var model = vcDomLem.GenerateDecisionRules(table, consistencyValue);

            var resultDir = Path.GetFileNameWithoutExtension(file);
            SaveResultFile(resultDir, model);
            SaveScoreFile(resultDir, table, model);

        }

        private static void SaveScoreFile(string resultDir, IInformationTable table, IModel model)
        {
            var predicted = model.Predict(table.GetAllObjectIdentifiers().ToList(), table);
            var all = table
                .GetDecisionAttribute()
                .Zip(predicted, (x, y) => new { shouldbe = x, was = y })
                .Select(x => $"{x.shouldbe.Key};{x.shouldbe.Value};{x.was}");

            File.WriteAllLines(Path.Combine(resultDir, "predicted.csv"), all);
        }

        private static void SaveResultFile(string resultDir, VCDomLEMAbstractions.IModel model)
        {
            //var coveredItems = rules
            //    .Select(x => x.GetCoveredItems())
            //    .Select(x => $"{{ {string.Join(", ", x)} }}");
            var result = model.Rules
                //.Zip(coveredItems, (x, y) => $"{x.ToString()} z a = {x.Accuracy} {y}")
                .Select(x => $"{x.ToString()} z a = {x.Accuracy}")
                .ToList();

            
            Directory.CreateDirectory(Path.Combine(".", resultDir));
            File.WriteAllLines(Path.Combine(resultDir, "result.txt"), result);
        }

        private static void HandleInputParams(string[] args, out string file, out string path, out string consistency)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Insert experiment file name!");
                file = Console.ReadLine();
            }
            else
            {
                file = args[0];
            }
            path = Path.Combine(".", file);
            if (args.Length < 2)
            {
                Console.WriteLine("Insert consistency treshold!");
                consistency = Console.ReadLine();
            }
            else
            {
                consistency = args[1];
            }
        }
    }
}
