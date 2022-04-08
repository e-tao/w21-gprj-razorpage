#nullable disable
namespace Products
{
    public class IndexModel : PageModel
    {
        private readonly DBContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(DBContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<Product> Product { get; set; }
        public IDictionary<string, int> LowStock { get; set; } = new Dictionary<string, int>();
        public IDictionary<string, string> AlmostExpire { get; set; } = new Dictionary<string, string>();
        public IDictionary<string, string> Expired { get; set; } = new Dictionary<string, string>();

        public int NumberOfNotification { get; set; }

        public async Task OnGetAsync()
        {
            Product = await _context.Product.ToListAsync();
            // List<string> lowStock = new();
            foreach (var item in Product)
            {
                if (item.Quantity < 10)
                {
                    LowStock.Add(item.ProductName, item.Quantity);
                }
                else if ((item.BestBefore - DateTime.Today).TotalDays < 0)
                {
                    Expired.Add(item.ProductName, item.BatchNumber);
                }
                else if ((item.BestBefore - DateTime.Today).TotalDays <= 7)
                {
                    AlmostExpire.Add(item.ProductName, item.BatchNumber);
                }
            }

            NumberOfNotification = Notification.NotificationNo(LowStock.Count, Expired.Count, AlmostExpire.Count);
        }
    }
}
