using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Text;
using WebWeB.Model;

namespace WebWeB.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DbWebContext _context;

        [BindProperty]
        public string Login { get; set; }

        private string password;
        [BindProperty]
        public string Password 
        {
            get => password;
            set
            {
                password = Hash(value);
            } 
        }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, DbWebContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            var user = _context.Users
                            .FirstOrDefault(u => u.Login == Login && u.Password == Password);

            if (user != null)
            {
                if (user.Role == (int)StaticData.UserTypes.User)
                {
                    Console.WriteLine($"UserId из TempData: {TempData["UserId"]}");
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    return RedirectToPage("/HomePage");  // Переходим на главную страницу
                }
                else
                {
                    //HttpContext.Session.SetInt32("UserId", user.Id);
                    return RedirectToPage("/ModeratorPage");  // Переходим на страницу модератора
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Неправильный логин или пароль.");
                return Page();
            }
        }

        string Hash(string input)
        {
            if(input == null) return string.Empty;

            var inputBytes = Encoding.UTF8.GetBytes(input);
            var inputHash = SHA256.HashData(inputBytes);
            return Convert.ToHexString(inputHash);
        }
    }
}