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

        public uint StockThreshold { get; set; } = 10;

        public uint BestBeforeThreshold { get; set; } = 7;

        public string Resend { get; set; }

        public async Task OnGetAsync()
        {
            Product = await _context.Product.ToListAsync();
            await StockCheck(Product);
        }

        public async Task OnPostAsync()
        {
            Product = await _context.Product.ToListAsync();
            StockThreshold = uint.Parse(Request.Form["StockThreshold"]);
            BestBeforeThreshold = uint.Parse(Request.Form["BestBeforeThreshold"]);
            Resend = Request.Form["SendEmail"];
            Notification.EmailSend = Resend == "on" ? false : true; //switch for resend email everytime the threshode changes;
            await StockCheck(Product);
        }

        public async Task StockCheck(IList<Product> product)
        {
            Product = product;
            string msg = "";

            foreach (var item in Product)
            {
                if (item.Quantity < StockThreshold)
                {
                    LowStock.Add(item.ProductName, item.Quantity.ToString());
                    msg += "<li>" + item.ProductName + " with batch number " + item.BatchNumber + " current stcok is low, please order soon</li>";
                }
                else if ((item.BestBefore - DateTime.Today).TotalDays < 0)
                {
                    Expired.Add(item.ProductName, item.BatchNumber);
                    msg += "<li>" + item.ProductName + " with batch number " + item.BatchNumber + " is already expired.</li>";
                }
                else if ((item.BestBefore - DateTime.Today).TotalDays <= BestBeforeThreshold)
                {
                    AlmostExpire.Add(item.ProductName, item.BatchNumber);
                    msg += "<li>" + item.ProductName + " with batch number " + item.BatchNumber + " is expering in " + BestBeforeThreshold + " days.</li>";
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

        public async Task OnGetSortBestbeforeAsc()
        {
            Product = await _context.Product.OrderBy(e => e.BestBefore).ToListAsync();
            await StockCheck(Product);
        }

        public async Task OnGetSortBestbeforeDesc()
        {
            Product = await _context.Product.OrderByDescending(e => e.BestBefore).ToListAsync();
            await StockCheck(Product);
        }

        public async Task OnGetSortQuantityAsc()
        {
            Product = await _context.Product.OrderBy(e => e.Quantity).ToListAsync();
            await StockCheck(Product);
        }

        public async Task OnGetSortQuantityDesc()
        {
            Product = await _context.Product.OrderByDescending(e => e.Quantity).ToListAsync();
            await StockCheck(Product);
        }
    }
}
