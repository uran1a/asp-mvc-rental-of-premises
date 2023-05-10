using RentalOfPremises.Models;
using System.Data.Entity;

namespace RentalOfPremises.ViewComponent
{
    public class MessageViewComponent
    {
        private readonly ApplicationContext _db;

        public MessageViewComponent(ApplicationContext context)
        {
            _db = context;
        }
        public string Invoke(int userId)
        {
            List<Deal> deals = _db.Deals
                .Where(d => d.OwnerId == userId)
                .Where(d => d.DateOfConclusion == null).ToList();
            return deals.Count().ToString();
        }
    }
}
