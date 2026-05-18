using System;
using SistemaAcademia.DAL;
using SistemaAcademia.Models;

namespace SistemaAcademia.BLL.Catraca
{
    /// <summary>
    /// BLL (Business Logic Layer) da CATRACA.
    /// 
    /// Contém a regra de negócio principal da catraca:
    ///   1. Verificar se o aluno tem pagamento ativo no mês atual
    ///   2. Registrar o log (histórico) da tentativa de acesso
    ///   3. Retornar o resultado (liberado ou bloqueado) para a tela
    /// 
    /// Chamada por: FormLogin (quando aluno faz login) e FormAdmin (simulação de catraca)
    /// </summary>
    public class CatracaBLL
    {
        // Referências às DALs necessárias (instanciadas no construtor)
        private readonly UsuarioDAL _usuarioDAL;
        private readonly PagamentoDAL _pagamentoDAL;
        private readonly CatracaDAL _catracaDAL;

        /// <summary>
        /// Construtor: Cria as instâncias das DALs que serão usadas pelos métodos.
        /// O "readonly" garante que elas não possam ser reatribuídas após o construtor.
        /// </summary>
        public CatracaBLL()
        {
            _usuarioDAL = new UsuarioDAL();
            _pagamentoDAL = new PagamentoDAL();
            _catracaDAL = new CatracaDAL();
        }

        /// <summary>
        /// VERIFICA O ACESSO DO ALUNO NA CATRACA.
        /// 
        /// Fluxo:
        ///   1. Consulta no banco se existe pagamento registrado para o CPF no mês atual
        ///   2. Registra o log de acesso (passou ou não) na tabela AcessoCatraca
        ///   3. Retorna true (liberado/verde) ou false (bloqueado/vermelho) para a tela
        /// 
        /// Chamado por: FormLogin.btnLogin_Click() e FormAdmin.btnSimularCatraca_Click()
        /// </summary>
        /// <param name="cpf">CPF do aluno que está tentando acessar a academia</param>
        /// <returns>true = catraca liberada (pagamento em dia), false = catraca bloqueada</returns>
        public bool VerificarAcessoCatraca(string cpf)
        {
            CatracaDAL dal = new CatracaDAL();

            // 1. Verifica no banco se tem pagamento no mês atual para este CPF
            bool temPagamento = dal.VerificarPagamentoAtivo(cpf);

            // 2. Registra o log (histórico de entrada): se passou ou se foi bloqueado
            // Isso cria uma linha na tabela AcessoCatraca com data/hora e resultado
            dal.RegistrarLogAcesso(cpf, temPagamento);

            // 3. Retorna o resultado para a tela exibir mensagem de liberado ou bloqueado
            return temPagamento;
        }
    }
}
