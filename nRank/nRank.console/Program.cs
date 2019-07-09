﻿using nRank.console.FileProcessors;
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
                //path = @"AirlinesIntCardinal.isf";
                file = args[0];
            }
            var path = Path.Combine(".", file);

            var reader = new InformationTableReader();
            var table = reader.Read(path);
            var vcDomLem = new VCDomLEM();
            var rules = vcDomLem.GenerateDecisionRules(table, 0.1f);
            var coveredItems = rules
                .Select(x => x.GetCoveredItems())
                .Select(x => $"{{ {string.Join(", ", x)} }}");
            var result = rules
                .Zip(coveredItems, (x, y) => $"{x.ToString()} z a = {x.Accuracy} {y}")
                .ToList();
            var resultDir = Path.GetFileNameWithoutExtension(file);
            Directory.CreateDirectory(Path.Combine(".",resultDir));
            File.WriteAllLines(Path.Combine(resultDir, "result.txt"), result);
        }
    }
}
