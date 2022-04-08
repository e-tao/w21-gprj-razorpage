using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace w21_gprj_razorpage.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DBContext _context;

    public IndexModel(DBContext context, ILogger<IndexModel> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task OnGetAsync()
    {
        if (User.Identity != null)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            if (claimsIdentity.IsAuthenticated)
            {
                Products.IndexModel stock = new Products.IndexModel(_context);
                await stock.StockCheck();
            }
        }
    }
}
