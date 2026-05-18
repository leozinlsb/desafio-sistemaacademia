using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SistemaAcademia.Models;

namespace SistemaAcademia.BLL.Services
{
    /// <summary>
    /// SERVIÇO DE CONSULTA DE CEP via API ViaCEP.
    /// 
    /// Esta classe faz uma requisição HTTP (chamada de API) para o serviço público
    /// ViaCEP (https://viacep.com.br/) para buscar dados de endereço a partir de um CEP.
    /// 
    /// Fluxo:
    ///   1. O usuário digita o CEP na tela (FormRegistro ou FormEditarAluno) e clica "Buscar"
    ///   2. A tela chama este serviço passando o CEP
    ///   3. Este serviço faz uma requisição GET para https://viacep.com.br/ws/{cep}/json/
    ///   4. A API retorna um JSON com Rua, Bairro, Cidade e Estado
    ///   5. O JSON é convertido (deserializado) para o objeto EnderecoViaCep
    ///   6. A tela preenche os campos de endereço automaticamente
    /// 
    /// Isso evita que o usuário precise digitar o endereço completo manualmente!
    /// 
    /// Chamada por: FormRegistro.btnBuscarCep_Click() e FormEditarAluno.btnBuscarCep_Click()
    /// </summary>
    public class ViaCepService
    {
        // HttpClient: Classe do .NET para fazer requisições HTTP (GET, POST, etc.)
        // É como um "navegador invisível" que acessa URLs e traz a resposta.
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Construtor: Cria o HttpClient e define o endereço base da API ViaCEP.
        /// Todas as requisições feitas depois usam este endereço como prefixo.
        /// </summary>
        public ViaCepService()
        {
            _httpClient = new HttpClient();
            // BaseAddress: URL base. Depois, basta adicionar o CEP no final.
            // Ex: BaseAddress = "https://viacep.com.br/ws/" + "01001000/json/" → URL completa
            _httpClient.BaseAddress = new Uri("https://viacep.com.br/ws/");
        }

        /// <summary>
        /// BUSCA O ENDEREÇO pelo CEP na API ViaCEP (método ASSÍNCRONO).
        /// 
        /// É "async" porque a requisição HTTP pode demorar (depende da internet)
        /// e não queremos "travar" a tela enquanto espera a resposta.
        /// O "await" pausa a execução DESTE método até a resposta chegar,
        /// mas a tela continua funcionando normalmente.
        /// 
        /// Retorna:
        ///   - EnderecoViaCep com dados preenchidos → CEP válido
        ///   - EnderecoViaCep com Erro = true → CEP não existe
        ///   - null → Erro de conexão com a API
        /// </summary>
        /// <param name="cep">CEP com ou sem máscara (ex: "01001-000" ou "01001000")</param>
        public async Task<EnderecoViaCep> BuscarEnderecoPorCepAsync(string cep)
        {
            // Remove o traço para montar a URL da API (a API espera apenas números)
            string cepLimpo = cep.Replace("-", "").Trim();
            
            // Validação: CEP deve ter exatamente 8 dígitos
            if (cepLimpo.Length != 8)
                throw new ArgumentException("CEP inválido para busca. Utilize o formato apenas com 8 números.");

            // Faz a requisição GET para a API ViaCEP
            // Ex: GET https://viacep.com.br/ws/01001000/json/
            HttpResponseMessage response = await _httpClient.GetAsync($"{cepLimpo}/json/");
            
            // IsSuccessStatusCode: Verifica se a API respondeu com sucesso (código HTTP 200)
            if (response.IsSuccessStatusCode)
            {
                // Lê o corpo da resposta como string (JSON)
                string json = await response.Content.ReadAsStringAsync();
                
                // Se o JSON contém "erro", significa que o CEP não existe na base dos Correios
                // A API retorna {"erro": true} nesse caso
                if (json.Contains("\"erro\""))
                {
                    return new EnderecoViaCep { Erro = true };
                }

                // JsonSerializer.Deserialize: Converte a string JSON em um objeto EnderecoViaCep.
                // Os [JsonPropertyName] definidos na classe EnderecoViaCep fazem o mapeamento
                // dos nomes do JSON (ex: "logradouro") para as propriedades C# (ex: Rua).
                var endereco = JsonSerializer.Deserialize<EnderecoViaCep>(json);
                return endereco;
            }

            // Se a API não respondeu com sucesso, retorna null
            return null;
        }
    }
}
