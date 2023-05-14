using RentalOfPremises.Models;

namespace RentalOfPremises.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Placement> Placements { get; }
        public PageViewModel PageViewModel { get; }
        public IndexViewModel(IEnumerable<Placement> placements, PageViewModel viewModel)
        {
            this.Placements = placements;
            this.PageViewModel = viewModel;
        }
    }
}
