using System.Text.RegularExpressions;

namespace SistemaAcademia.BLL.Validations
{
    /// <summary>
    /// VALIDADOR usando EXPRESSÕES REGULARES (Regex).
    /// 
    /// Regex é uma "linguagem de padrões" que permite verificar se um texto
    /// segue um formato específico. É usada aqui para validar e-mail, telefone e CEP
    /// antes de salvar no banco de dados.
    /// 
    /// A classe é ESTÁTICA (static) → não precisa criar instância para usar.
    /// Basta chamar: RegexValidator.ValidarEmail("teste@email.com")
    /// 
    /// Chamada por: AuthBLL.Registrar() e AuthBLL.Atualizar()
    /// </summary>
    public static class RegexValidator
    {
        /// <summary>
        /// Valida se o E-MAIL está em um formato válido.
        /// 
        /// Padrão: ^[^@\s]+@[^@\s]+\.[^@\s]+$
        /// Explicação do padrão:
        ///   ^           → Início do texto
        ///   [^@\s]+     → Um ou mais caracteres que NÃO sejam @ nem espaço (parte antes do @)
        ///   @           → O símbolo @ obrigatório
        ///   [^@\s]+     → Um ou mais caracteres que NÃO sejam @ nem espaço (domínio)
        ///   \.          → Um ponto obrigatório (o \ escapa o ponto, que no regex significa "qualquer caractere")
        ///   [^@\s]+     → Um ou mais caracteres finais (extensão: com, br, etc.)
        ///   $           → Fim do texto
        /// 
        /// Exemplos válidos: "joao@gmail.com", "maria@empresa.com.br"
        /// Exemplos inválidos: "joao@", "@gmail.com", "joao gmail.com"
        /// </summary>
        public static bool ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            string padrao = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, padrao);
        }

        /// <summary>
        /// Valida se o TELEFONE está no formato brasileiro com DDD.
        /// 
        /// Padrão: ^\(\d{2}\) \d{4,5}-\d{4}$
        /// Explicação:
        ///   ^           → Início
        ///   \(          → Parêntese de abertura literal (\ escapa o caractere especial)
        ///   \d{2}       → Exatamente 2 dígitos (DDD, ex: 11, 21)
        ///   \)          → Parêntese de fechamento literal
        ///   (espaço)    → Um espaço obrigatório após o DDD
        ///   \d{4,5}     → De 4 a 5 dígitos (telefone fixo tem 4, celular tem 5)
        ///   -           → Hífen obrigatório
        ///   \d{4}       → Exatamente 4 dígitos finais
        ///   $           → Fim
        /// 
        /// Exemplos válidos: "(11) 99999-9999", "(21) 3333-4444"
        /// Exemplos inválidos: "11999999999", "(11)99999-9999" (sem espaço)
        /// </summary>
        public static bool ValidarTelefone(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone)) return false;
            string padrao = @"^\(\d{2}\) \d{4,5}-\d{4}$";
            return Regex.IsMatch(telefone, padrao);
        }

        /// <summary>
        /// Valida se o CEP está no formato brasileiro XXXXX-XXX.
        /// 
        /// Padrão: ^\d{5}-\d{3}$
        /// Explicação:
        ///   ^       → Início
        ///   \d{5}   → Exatamente 5 dígitos
        ///   -       → Hífen obrigatório
        ///   \d{3}   → Exatamente 3 dígitos
        ///   $       → Fim
        /// 
        /// Exemplos válidos: "01001-000", "12345-678"
        /// Exemplos inválidos: "01001000" (sem hífen), "1234-567" (faltam dígitos)
        /// </summary>
        public static bool ValidarCep(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep)) return false;
            string padrao = @"^\d{5}-\d{3}$";
            return Regex.IsMatch(cep, padrao);
        }
    }
}
