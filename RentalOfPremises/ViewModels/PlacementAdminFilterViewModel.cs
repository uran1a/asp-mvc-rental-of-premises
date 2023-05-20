namespace RentalOfPremises.ViewModels
{
    public class PlacementAdminFilterViewModel
    {
        public List<PlacementAdminViewModel> AvailablePlacements { get; set; }
        public string TypeFilter { get; set; }
        public string Condition { get; set; }
        public int Count { get; set; }
        public bool HasCreate { get; set; }
        public bool HasUpdate { get; set; }
        public bool HasDelete { get; set; }
        public int NumberTypeFilter()
        {
            char firstLeter = this.TypeFilter[0];
            if (Char.IsDigit(firstLeter))
                return int.Parse(firstLeter.ToString());
            return -1;
        }
    }
}
