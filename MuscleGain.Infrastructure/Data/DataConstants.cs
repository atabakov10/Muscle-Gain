using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuscleGain.Infrastructure.Data
{
    public static class DataConstants
    {
        //Protein
        public const int ProteinNameMinLength = 4;
        public const int ProteinNameMaxLength = 30;
        public const int ProteinPriceMaxLength = 1000;
        public const int ProteinPriceMinLength = 10;
        public const int ProteinGramsMinLength = 250;
        public const int ProteinGramsMaxLength = 5000;
        public const int ProteinFlavorMinLength = 3;
        public const int ProteinFlavorMaxLength = 30;
        public const int ProteinDescriptionMinLength = 10;
        //Account
        public const int FirstNameMaxLength = 20;
        public const int LastNameMaxLength = 20;
    }
}
