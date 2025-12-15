using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebClient.Infrastructure;
using WebClient.Models.Auth;

namespace WebClient.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("UserService");

            var response = await client.PostAsJsonAsync("/api/auth/login", new
            {
                email = model.Email,
                password = model.Password
            });

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas");
                return View(model);
            }

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var token = root.GetProperty("token").GetString();
            var name = root.GetProperty("name").GetString();
            var role = root.GetProperty("role").GetString();

            HttpContext.Session.SetString(SessionKeys.JwtToken, token ?? string.Empty);
            HttpContext.Session.SetString(SessionKeys.UserName, name ?? string.Empty);
            HttpContext.Session.SetString(SessionKeys.UserRole, role ?? string.Empty);

            return RedirectToAction("Index", "Products");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
