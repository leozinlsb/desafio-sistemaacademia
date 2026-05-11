using SistemaAcademia.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaAcademia.BLL
{
    public class DashboardBLL
    {
        public DataTable ListarVisaoGeralAlunos()
        {
            DashboardDAL dal = new DashboardDAL();
            return dal.ObterVisaoGeralAlunos();
        }
    }
}
