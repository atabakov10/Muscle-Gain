namespace MuscleGain.Core.Models.Home
{
    public class IndexViewModel
    {
        public int TotalProteins { get; init; }
        public int TotalsOrders { get; init; }  
        public List<ProteinIndexViewModel> Proteins { get; init; }
    }
}
