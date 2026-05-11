using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SistemaAcademia.Models;

namespace SistemaAcademia.BLL.Services
{
    public class ViaCepService
    {
        private readonly HttpClient _httpClient;

        public ViaCepService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://viacep.com.br/ws/");
        }

        public async Task<EnderecoViaCep> BuscarEnderecoPorCepAsync(string cep)
        {
            // Remove o traço para a API
            string cepLimpo = cep.Replace("-", "").Trim();
            
            if (cepLimpo.Length != 8)
                throw new ArgumentException("CEP inválido para busca. Utilize o formato apenas com 8 números.");

            HttpResponseMessage response = await _httpClient.GetAsync($"{cepLimpo}/json/");
            
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                
                if (json.Contains("\"erro\""))
                {
                    return new EnderecoViaCep { Erro = true };
                }

                var endereco = JsonSerializer.Deserialize<EnderecoViaCep>(json);
                return endereco;
            }

            return null;
        }
    }
}
