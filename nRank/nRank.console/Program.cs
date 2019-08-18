using nRank.console.FileProcessors;
using nRank.console.Statistics;
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

            var splitter = new Splitter();
            var split = splitter.Split(table, 0.75f);

            var vcDomLem = new VCDomLEM();
            var model = vcDomLem.GenerateDecisionRules(split.TrainingTable, consistencyValue);

            var resultDir = Path.GetFileNameWithoutExtension(file);
            SaveRulesFile(resultDir, model);
            SavePredictedFile(resultDir, table, model);
            SaveResultFile(resultDir, split, model);

        }

        private static void SavePredictedFile(string resultDir, IInformationTable table, IModel model)
        {
            var predicted = model.Predict(table.GetAllObjectIdentifiers().ToList(), table);
            var all = table
                .GetDecisionAttribute()
                .Zip(predicted, (x, y) => new { shouldbe = x, was = y })
                .Select(x => $"{x.shouldbe.Key};{x.shouldbe.Value};{x.was}");

            File.WriteAllLines(Path.Combine(resultDir, "predicted.csv"), all);
        }

        private static void SaveRulesFile(string resultDir, VCDomLEMAbstractions.IModel model)
        {
            var result = model.Rules
                .Select(x => $"{x.ToString()} z a = {x.Accuracy}")
                .ToList();


            Directory.CreateDirectory(Path.Combine(".", resultDir));
            File.WriteAllLines(Path.Combine(resultDir, "rules.txt"), result);
        }

        private static void SaveResultFile(string resultDir, SplitInformationTable tables, IModel model)
        {
            var predictedTraining = model.Predict(tables.TrainingTable.GetAllObjectIdentifiers().ToList(), tables.TrainingTable);
            var rmseTraining = Metrics.RMSE(tables.TrainingTable.GetDecisionAttribute().Select(x => x.Value), predictedTraining);

            var predictedTesting = model.Predict(tables.TestingTable.GetAllObjectIdentifiers().ToList(), tables.TestingTable);
            var rmseTesting = Metrics.RMSE(tables.TestingTable.GetDecisionAttribute().Select(x => x.Value), predictedTesting);

            var resultFileContent = new[]
            {
                $"RMSE dla zbioru uczącego: {rmseTraining}",
                $"RMSE dla zbioru testowego: {rmseTesting}"
            };

            File.WriteAllLines(Path.Combine(resultDir, "result.txt"), resultFileContent);
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
