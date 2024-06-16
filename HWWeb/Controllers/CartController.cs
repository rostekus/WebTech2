using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HWWeb.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using HWWeb.Services;

namespace HWWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDBContext _context;

        public CartController(AppDBContext context)
        {
            _context = context;
        }

        private Cart GetCart(string userId)
        {
            return _context.Cart
                           .Include(c => c.Items)
                           .ThenInclude(ci => ci.Phone)
                           .FirstOrDefault(c => c.UserId == userId)
                   ?? new Cart { UserId = userId };
        }

        public IActionResult Index()
        {
            var userId = User.Identity.Name; // Assuming user authentication is handled
            var cart = GetCart(userId);
            return View(cart);
        }

        public IActionResult AddToCart(int phoneId, int quantity)
        {
            var userId = User.Identity.Name; // Assuming user authentication is handled
            var cart = GetCart(userId);

            var cartItem = cart.Items.FirstOrDefault(ci => ci.PhoneId == phoneId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    PhoneId = phoneId,
                    Quantity = quantity,
                    Phone = _context.Phone.Find(phoneId)
                });
            }

            if (cart.Id == 0) // New cart
            {
                _context.Cart.Add(cart);
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int phoneId)
        {
            var userId = User.Identity.Name; // Assuming user authentication is handled
            var cart = GetCart(userId);

            var cartItem = cart.Items.FirstOrDefault(ci => ci.PhoneId == phoneId);
            if (cartItem != null)
            {
                cart.Items.Remove(cartItem);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
