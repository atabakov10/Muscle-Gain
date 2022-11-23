namespace MuscleGain.Infrastructure.Data
{
    public static class DataConstants
    {
        //Protein
        public const int ProteinNameMinLength = 4;
        public const int ProteinNameMaxLength = 30;
        public const int ProteinPriceMaxLength = 1000;
        public const int ProteinPriceMinLength = 10;
        public const int ProteinGramsMinLength = 3;
        public const int ProteinGramsMaxLength = 6;
        public const int ProteinFlavorMinLength = 3;
        public const int ProteinFlavorMaxLength = 30;
        public const int ProteinDescriptionMinLength = 10;
        //Account
        public const int FirstNameMaxLength = 20;
        public const int LastNameMaxLength = 20;
        //Review
        public const int CommentMaxLength = 500;
        public const int RatingMaxLength = 5;
    }
}
