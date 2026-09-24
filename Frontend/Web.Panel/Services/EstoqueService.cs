using Serviço.Model.Dtos;
using System.Runtime.CompilerServices;

namespace Web.Panel.Services
{
    public class EstoqueService
    {
        private HttpClient _httpClient;

        public EstoqueService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("EstoqueAPI");
        }

        public async Task<List<Produto>> GetAllProductsAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Produto>>("api/estoque") ?? new List<Produto>();
            return result;
        }        

        public async Task<Produto?> SaveAsync(Produto produto)
        {
            if (produto != null)
            {
                var response = await _httpClient.PostAsJsonAsync("api/estoque", produto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Produto>();
                }

                return null;
            }
            return null;
        }


        
    }
}
