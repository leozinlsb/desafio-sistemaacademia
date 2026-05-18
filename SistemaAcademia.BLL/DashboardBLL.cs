using SistemaAcademia.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaAcademia.BLL
{
    /// <summary>
    /// BLL (Business Logic Layer) do DASHBOARD ADMINISTRATIVO.
    /// 
    /// Faz a ponte entre a tela admin (FormAdmin) e a DAL (DashboardDAL).
    /// Atualmente apenas repassa a chamada, mas aqui poderiam ser adicionadas
    /// regras como: filtrar por alunos inadimplentes, calcular estatísticas, etc.
    /// 
    /// Chamada por: FormAdmin.CarregarDados()
    /// </summary>
    public class DashboardBLL
    {
        /// <summary>
        /// Retorna a visão geral de todos os alunos (DataTable).
        /// A UI usa esse DataTable como DataSource da DataGridView para exibir a tabela.
        /// </summary>
        public DataTable ListarVisaoGeralAlunos()
        {
            DashboardDAL dal = new DashboardDAL();
            return dal.ObterVisaoGeralAlunos();
        }
    }
}
