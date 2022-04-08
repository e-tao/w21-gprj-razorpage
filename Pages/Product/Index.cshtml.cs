#nullable disable


namespace Products
{
    public class IndexModel : PageModel
    {
        private readonly DBContext _context;

        public IndexModel(DBContext context)
        {
            _context = context;
        }

        public IList<Employee> Employees { get; set; }
        public IList<Product> Product { get; set; }
        public IDictionary<string, string> LowStock { get; set; } = new Dictionary<string, string>();
        public IDictionary<string, string> AlmostExpire { get; set; } = new Dictionary<string, string>();
        public IDictionary<string, string> Expired { get; set; } = new Dictionary<string, string>();

        public int NumberOfNotification { get; set; }

        public async Task OnGetAsync()
        {
            Product = await _context.Product.ToListAsync();
        }

        public async Task StockCheck()
        {
            Product = await _context.Product.ToListAsync();

            foreach (var item in Product)
            {
                if (item.Quantity < 10)
                {
                    LowStock.Add(item.ProductName, item.Quantity.ToString());
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

            if (NumberOfNotification > 0)
            {
                Employees = await _context.Employees.ToListAsync();
                var emails = Employees.Where(e => e.Title == Position.Supervisor).Select(e => e.Email).ToList();

                foreach (var email in emails)
                {
                    await Notification.EmailNotification(email, "Inventory System Notification");
                }
            }
        }
    }
}
