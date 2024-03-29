﻿namespace MuscleGain.Core.Models.Proteins
{
    public class ProteinListingViewModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Grams { get; init; }
        public string Flavour { get; init; }
        public decimal? Price { get; init; }
        public string ImageUrl { get; init; }
        public string Category { get; init; }
    }
}
