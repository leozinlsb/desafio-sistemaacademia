using System;

namespace SistemaAcademia.Models
{
    public class Pagamento
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
        public int MesReferencia { get; set; }
        public int AnoReferencia { get; set; }
    }
}
