using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.DataStructures
{
    class Approximation : IApproximation
    {
        public Approximation(IInformationTable appriximatedInformationTable, IInformationTable originalInformationTable, IEnumerable<int> classes, 
                IEnumerable<string> allowedOperators, string symbol, IUnion union, IList<string> positiveRegion)
        {
            ApproximatedInformationTable = appriximatedInformationTable;
            OriginalInformationTable = originalInformationTable;
            Classes = new HashSet<int>(classes);
            AllowedOperators = new HashSet<string>(allowedOperators);
            Symbol = symbol;
            Union = union;
            PositiveRegion = positiveRegion;
        }

        public IInformationTable ApproximatedInformationTable { get; }
        public IInformationTable OriginalInformationTable { get; }
        public ISet<int> Classes { get; }
        public ISet<string> AllowedOperators { get; }
        public string Symbol { get; }
        public IUnion Union { get; }
        public IList<string> PositiveRegion { get; }

        public IInformationTable GetNegatedApproximatedInformationTable()
        {
            return ApproximatedInformationTable.Negation(OriginalInformationTable);
        }
    }
}
