using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StockIT.Pages
{
    public class OperationCompleteModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Title"] = "Operation Complete";
        }
    }
}
