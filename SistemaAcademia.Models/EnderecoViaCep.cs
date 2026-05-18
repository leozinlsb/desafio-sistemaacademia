using System.Text.Json.Serialization;

namespace SistemaAcademia.Models
{
    /// <summary>
    /// MODELO (Model) que representa a RESPOSTA DA API VIACEP.
    /// 
    /// Quando o sistema faz uma requisição HTTP para "https://viacep.com.br/ws/{cep}/json/",
    /// a API retorna um JSON com os dados do endereço. Esta classe mapeia esse JSON
    /// para um objeto C#.
    /// 
    /// Os atributos [JsonPropertyName] dizem ao serializador JSON qual campo do JSON
    /// corresponde a qual propriedade da classe. Isso é necessário porque os nomes
    /// no JSON da API (em minúsculo) são diferentes dos nomes das propriedades C# 
    /// (em PascalCase).
    /// 
    /// Exemplo de resposta da API para o CEP "01001-000":
    /// {
    ///   "cep": "01001-000",
    ///   "logradouro": "Praça da Sé",
    ///   "bairro": "Sé",
    ///   "localidade": "São Paulo",
    ///   "uf": "SP"
    /// }
    /// </summary>
    public class EnderecoViaCep
    {
        // Mapeia o campo "cep" do JSON para a propriedade Cep
        [JsonPropertyName("cep")]
        public string Cep { get; set; }

        // Mapeia o campo "logradouro" do JSON para a propriedade Rua
        // (a API chama de "logradouro", mas no sistema usamos "Rua" para ficar mais simples)
        [JsonPropertyName("logradouro")]
        public string Rua { get; set; }

        // Mapeia o campo "bairro" do JSON para a propriedade Bairro
        [JsonPropertyName("bairro")]
        public string Bairro { get; set; }

        // Mapeia o campo "localidade" do JSON para a propriedade Cidade
        // (a API chama de "localidade", mas no sistema usamos "Cidade")
        [JsonPropertyName("localidade")]
        public string Cidade { get; set; }

        // Mapeia o campo "uf" do JSON para a propriedade Estado
        // (a API chama de "uf" - Unidade Federativa, mas usamos "Estado")
        [JsonPropertyName("uf")]
        public string Estado { get; set; }

        // Mapeia o campo "erro" do JSON para a propriedade Erro.
        // Quando um CEP não existe, a API retorna {"erro": true}.
        // Usamos este campo para exibir a mensagem "CEP não existente" na tela.
        [JsonPropertyName("erro")]
        public bool Erro { get; set; }
    }
}
