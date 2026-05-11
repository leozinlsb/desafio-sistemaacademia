using System.Text.Json.Serialization;

namespace SistemaAcademia.Models
{
    public class EnderecoViaCep
    {
        [JsonPropertyName("cep")]
        public string Cep { get; set; }

        [JsonPropertyName("logradouro")]
        public string Rua { get; set; }

        [JsonPropertyName("bairro")]
        public string Bairro { get; set; }

        [JsonPropertyName("localidade")]
        public string Cidade { get; set; }

        [JsonPropertyName("uf")]
        public string Estado { get; set; }

        [JsonPropertyName("erro")]
        public bool Erro { get; set; }
    }
}
