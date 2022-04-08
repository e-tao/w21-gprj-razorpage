#nullable disable


namespace Products
{
    public class IndexModel : PageModel
    {
        private readonly DBContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(DBContext context)
        {
            _context = context;
            // _logger = logger;
        }

        // public IndexModel(DBContext context, ILogger<IndexModel> logger)
        // {
        //     _context = context;
        //     _logger = logger;
        // }

        public IList<Employee> Employees { get; set; }
        public IList<Product> Product { get; set; }
        public IDictionary<string, string> LowStock { get; set; } = new Dictionary<string, string>();
        public IDictionary<string, string> AlmostExpire { get; set; } = new Dictionary<string, string>();
        public IDictionary<string, string> Expired { get; set; } = new Dictionary<string, string>();

        public int NumberOfNotification { get; set; }

        public async Task OnGetAsync()
        {
            Product = await _context.Product.ToListAsync();
            await StockCheck();
        }

        public async Task StockCheck()
        {
            Product = await _context.Product.ToListAsync();
            string msg = "";

            foreach (var item in Product)
            {
                if (item.Quantity < 10)
                {
                    LowStock.Add(item.ProductName, item.Quantity.ToString());
                    msg += "<li>" + item.ProductName + " with batch number " + item.BatchNumber + " current stcok is low, please order soon</li>";
                }
                else if ((item.BestBefore - DateTime.Today).TotalDays < 0)
                {
                    Expired.Add(item.ProductName, item.BatchNumber);
                    msg += "<li>" + item.ProductName + " with batch number " + item.BatchNumber + " is expering in 7 days.</li>";
                }
                else if ((item.BestBefore - DateTime.Today).TotalDays <= 7)
                {
                    AlmostExpire.Add(item.ProductName, item.BatchNumber);
                    msg += "<li>" + item.ProductName + " with batch number " + item.BatchNumber + " is already expired.</li>";
                }
            }

            NumberOfNotification = Notification.NotificationNo(LowStock.Count, Expired.Count, AlmostExpire.Count);

            // _logger.Log(LogLevel.Information, Notification.EmailSend.ToString());
            // _logger.Log(LogLevel.Information, (DateTime.Now - Notification.SendDate).TotalDays.ToString());
            // _logger.Log(LogLevel.Information, NumberOfNotification.ToString());

            //when the app runs for the first time, the EmailSend condition will always met and make sure the email is send at least once
            //if the notification is greater than 0
            if (Notification.EmailSend == false || ((DateTime.Now - Notification.SendDate).TotalDays > 1) && NumberOfNotification > 0)
            {
                Employees = await _context.Employees.ToListAsync();
                var emails = Employees.Where(e => e.Title == Position.Supervisor).Select(e => e.Email).ToList();

                foreach (var email in emails)
                {
                    await Notification.EmailNotification(email, "Inventory System Notification", msg);
                }
            }
        }
    }
}
