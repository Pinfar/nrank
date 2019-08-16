using nRank.VCDomLEMAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.DataStructures
{
    class TrainedModel : IModel
    {
        public TrainedModel(IList<IDecisionRule> rules)
        {
            Rules = rules;
        }

        public IList<IDecisionRule> Rules { get; }

        public IList<int> Predict(IList<string> identifiers, IInformationTable table)
        {
            var resolvers = identifiers
                .Select(x => Rules
                    .Where(y => y.IsSatisfiedFor(table, x))
                    .Select(y => new ClassResolver(y))
                    .Aggregate((obj1, obj2) => obj1 + obj2)
                );
            return resolvers
                .Select(x => x.GetMostPossibleClass())
                .ToList();
        }
    }
}
