using System.Net.Http.Headers;
using WebClient.Infrastructure;
using WebClient.Models.Products;

namespace WebClient.Services
{
    public class ProductApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpClient CreateClientWithToken()
        {
            var client = _httpClientFactory.CreateClient("ProductService");
            var token = _httpContextAccessor.HttpContext?.Session.GetString(SessionKeys.JwtToken);

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        public async Task<List<ProductViewModel>> GetAllAsync()
        {
            var client = CreateClientWithToken();
            var products = await client.GetFromJsonAsync<List<ProductViewModel>>("/api/product");
            return products ?? new List<ProductViewModel>();
        }

        public async Task<ProductCreateEditViewModel?> GetByIdAsync(Guid id)
        {
            var client = CreateClientWithToken();
            var result = await client.GetFromJsonAsync<ProductCreateEditViewModel>($"/api/product/{id}");
            return result;
        }

        public async Task<bool> CreateAsync(ProductCreateEditViewModel model)
        {
            var client = CreateClientWithToken();
            var response = await client.PostAsJsonAsync("/api/product", new
            {
                name = model.Name,
                description = model.Description,
                price = model.Price,
                category = model.Category,
                isActive = model.IsActive
            });

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(ProductCreateEditViewModel model)
        {
            if (model.Id == null)
                throw new ArgumentException("Id requerido para actualizar");

            var client = CreateClientWithToken();
            var response = await client.PutAsJsonAsync($"/api/product/{model.Id}", new
            {
                name = model.Name,
                description = model.Description,
                price = model.Price,
                category = model.Category,
                isActive = model.IsActive
            });

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var client = CreateClientWithToken();
            var response = await client.DeleteAsync($"/api/product/{id}");
            return response.IsSuccessStatusCode;
        }

    }
}
