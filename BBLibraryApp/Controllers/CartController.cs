using BBLibraryApp.Data;
using BBLibraryApp.Models;
using BBLibraryApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BBLibraryApp.Controllers
{
    [Authorize(Policy = "AdministratorOnly")]
    public class CartController : Controller
    {
        private readonly BBLibraryContext _context;
        private Cart cart;

        public CartController(BBLibraryContext context, Cart cartService)
        {
            _context = context;
            cart = cartService;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectResult AddToCart(int id, string returnUrl)
        {
            Chart chart = _context.Charts
                .AsNoTracking()
                .FirstOrDefault(c => c.ID == id);

            if (chart != null)
            {
                cart.AddItem(chart);
            }
            return Redirect(returnUrl);
        }

        public RedirectToActionResult RemoveFromCart(int id, string returnUrl)
        {
            Chart chart = _context.Charts
                .AsNoTracking()
                .FirstOrDefault(c => c.ID == id);
            if (chart != null)
            {
                cart.RemoveChart(id);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public RedirectToActionResult ClearCart(string returnUrl)
        {
            cart.Clear();
            return RedirectToAction("Index", new { returnUrl });
        }
    }
}
