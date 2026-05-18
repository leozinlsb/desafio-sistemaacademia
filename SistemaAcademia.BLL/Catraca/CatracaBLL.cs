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

            bool temPagamento = dal.VerificarPagamentoAtivo(cpf);
            dal.RegistrarLogAcesso(cpf, temPagamento);

            return temPagamento;
        }
    }
}
