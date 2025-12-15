using Microsoft.AspNetCore.Mvc;
using WebClient.Infrastructure;
using WebClient.Models.Products;
using WebClient.Services;

namespace WebClient.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductApiClient _apiClient;

        public ProductsController(ProductApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString(SessionKeys.JwtToken);
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var products = await _apiClient.GetAllAsync();
            ViewBag.UserRole = HttpContext.Session.GetString(SessionKeys.UserRole);
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            var role = HttpContext.Session.GetString(SessionKeys.UserRole);
            if (role != "Admin")
                return Forbid();

            return View(new ProductCreateEditViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateEditViewModel model)
        {
            var role = HttpContext.Session.GetString(SessionKeys.UserRole);
            if (role != "Admin")
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            var ok = await _apiClient.CreateAsync(model);
            if (!ok)
            {
                ModelState.AddModelError(string.Empty, "Error al crear el producto.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var role = HttpContext.Session.GetString(SessionKeys.UserRole);
            if (role != "Admin")
                return Forbid();

            var product = await _apiClient.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductCreateEditViewModel model)
        {
            var role = HttpContext.Session.GetString(SessionKeys.UserRole);
            if (role != "Admin")
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            var ok = await _apiClient.UpdateAsync(model);
            if (!ok)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar el producto.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var role = HttpContext.Session.GetString(SessionKeys.UserRole);
            if (role != "Admin")
                return Forbid();

            var ok = await _apiClient.DeleteAsync(id);
            if (!ok)
            {
                // Podrías mostrar una vista de error, por simplicidad redirigimos
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
