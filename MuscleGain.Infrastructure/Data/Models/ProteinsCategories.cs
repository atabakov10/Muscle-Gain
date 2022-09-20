using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuscleGain.Infrastructure.Data.Models
{
    public class ProteinsCategories
    {
        public int Id { get; init; }
        public string Name { get; set; }
        public IEnumerable<Protein> Proteins { get; init; } = new List<Protein>();
    }
}
