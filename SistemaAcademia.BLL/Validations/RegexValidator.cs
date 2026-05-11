using System.Text.RegularExpressions;

namespace SistemaAcademia.BLL.Validations
{
    public static class RegexValidator
    {
        public static bool ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            string padrao = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, padrao);
        }

        public static bool ValidarTelefone(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone)) return false;
            // Valida formatos como (XX) XXXXX-XXXX ou (XX) XXXX-XXXX
            string padrao = @"^\(\d{2}\) \d{4,5}-\d{4}$";
            return Regex.IsMatch(telefone, padrao);
        }

        public static bool ValidarCep(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep)) return false;
            // Valida formato XXXXX-XXX
            string padrao = @"^\d{5}-\d{3}$";
            return Regex.IsMatch(cep, padrao);
        }
    }
}
