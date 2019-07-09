using nRank.DataStructures;
using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace nRank.console.FileProcessors
{
    class InformationTableReader
    {
        private string _label;
        private string _decisionAttribute;
        private List<string> _attributes;

        public IInformationTable Read(string path)
        {
            var lines = File.ReadAllLines(path).ToList();
            var table = CreateBaseTable(lines);
            FillTable(lines, table);
            return table;
        }

        private IInformationTable CreateBaseTable(List<string> lines)
        {
            var preferences = GetSection(lines, "**PREFERENCES");

            var isAttributeCost = preferences
                .Select(x => x.Split(new[] { ": " }, StringSplitOptions.None))
                .ToDictionary(x => x[0], x => x[1] == "cost");


            var attributes = GetSection(lines, "**ATTRIBUTES");
            var decisionAttribute = attributes
                .Single(x => x.StartsWith("decision:"))
                .Replace("decision:", "")
                .Trim();

            var regex = new Regex("^[-+] (?<label>.*):");
            var labelLine = attributes
                .SingleOrDefault(x => x.EndsWith("description"));
            if (labelLine != null)
            {
                var label = regex.Match(labelLine).Groups["label"].Value;
                isAttributeCost.Remove(label);
                _label = label;
            }

            var isDecisionAttributeCost = isAttributeCost[decisionAttribute];
            isAttributeCost.Remove(decisionAttribute);

            _decisionAttribute = decisionAttribute;
            _attributes = isAttributeCost.Select(x => x.Key).ToList();
            
            return new InformationTable(isAttributeCost, decisionAttribute, isDecisionAttributeCost);
        }

        private List<string> GetSection(List<string> lines, string name)
        {
            var endMark = new[] { "**END", "" };

            return lines
                .SkipWhile(x => x != name)
                .Skip(1)
                .TakeWhile(x => !endMark.Contains(x))
                .ToList();
        }

        private void FillTable(List<string> lines, IInformationTable table)
        {
            var examples = GetSection(lines, "**EXAMPLES");
            var preferences = GetSection(lines, "**PREFERENCES");

            var allAttributes = preferences
                .Select(x => x.Split(new[] { ": " }, StringSplitOptions.None))
                .Select(x => x[0])
                .ToList();
            var index = 0;
            foreach (var line in examples)
            {
                var record = line
                    .Split(new[] { ",","\t" }, StringSplitOptions.None)
                    .Select(x => x.Trim())
                    .Zip(allAttributes, (x, y) => new { Key = y, Value = x })
                    .ToDictionary(x => x.Key, x => x.Value);
                var decisionAttribute = int.Parse(record[_decisionAttribute]);
                record.Remove(_decisionAttribute);
                string label;
                if (_label != null)
                {
                    label = record[_label];
                    record.Remove(_label);
                }
                else
                {
                    label = index.ToString();
                    index++;
                }
                var values = record.ToDictionary(x => x.Key, x => float.Parse(x.Value.Replace('.', ',')));
                table.AddObject(label, values, decisionAttribute);
            }
        }
    }
}
