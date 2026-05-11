using System;
using SistemaAcademia.DAL;
using SistemaAcademia.Models;

namespace SistemaAcademia.BLL.Catraca
{
    public class CatracaBLL
    {
        private readonly UsuarioDAL _usuarioDAL;
        private readonly PagamentoDAL _pagamentoDAL;
        private readonly CatracaDAL _catracaDAL;

        public CatracaBLL()
        {
            _usuarioDAL = new UsuarioDAL();
            _pagamentoDAL = new PagamentoDAL();
            _catracaDAL = new CatracaDAL();
        }

        public bool VerificarAcessoCatraca(string cpf)
        {
            CatracaDAL dal = new CatracaDAL();

            // 1. Verifica no banco se tem pagamento
            bool temPagamento = dal.VerificarPagamentoAtivo(cpf);

            // 2. Registra o log (histórico de entrada) se passou ou se foi bloqueado
            dal.RegistrarLogAcesso(cpf, temPagamento);

            // 3. Retorna o resultado para a tela piscar Verde (true) ou Vermelho (false)
            return temPagamento;
        }
    }
}
