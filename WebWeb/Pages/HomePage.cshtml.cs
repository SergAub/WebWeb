using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebWeB.Model;

namespace WebWeB.Pages
{
    public class HomePageModel : PageModel
    {
        private readonly DbWebContext _context;

        public HomePageModel(DbWebContext context)
        {
            _context = context;
        }

        public IList<Good> Goods { get; set; }
        public IList<Order> Orders { get; set; }

        // ����� ��� �������� ������� � ������� ������������
        public async Task OnGetAsync()
        {
            Goods = await _context.Goods.ToListAsync();

            // �������� UserId �� ������
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                // ��������� ������ ��� ����� ������������
                Orders = await _context.Orders
                    .Where(o => o.UserId == userId.Value)
                    .Include(o => o.Goods) // �������� ���������� � ������
                    .ToListAsync();
            }
        }

        // ����� ��� �������� ������
        public async Task<IActionResult> OnPostCreateOrderAsync(int goodsId, int quantity)
        {
            // �������� UserId �� ������
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                // ���������, ���������� �� ����� � ���� �� �� � �������
                var goods = await _context.Goods.FindAsync(goodsId);
                if (goods != null && goods.InStock && quantity > 0)
                {
                    // ������� �����
                    var order = new Order
                    {
                        UserId = userId.Value,  // ���������� UserId ��� int
                        GoodsId = goods.Id,
                        Count = quantity
                    };

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "����� ������� ��������!";
                    return RedirectToPage();
                }

                // ���� ����� �� ������ ��� ��� ��� � ������� (��� �������)
                TempData["Error"] = "������ ��� �������� ������.";
                return RedirectToPage();
            }

            // ���� UserId �� ������ � ������ (��� �������)
            TempData["Error"] = "������������ �� ������.";
            return RedirectToPage();
        }
    }

}
