using nRank.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nRank.DataStructures
{
    class InformationAttribute<T>
    {
        public string Name { get; set; }
        public bool IsCost { get; set; }
        public List<T> Values { get; set; }

        public InformationAttribute<T> WhereIsTrue(IEnumerable<bool> filterPattern)
        {
            return new InformationAttribute<T>
            {
                Name = Name,
                IsCost = IsCost,
                Values = Values.WhereIsTrue(filterPattern).ToList()
            };
        }
    }
}
