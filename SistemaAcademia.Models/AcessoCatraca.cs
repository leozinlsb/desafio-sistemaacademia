using System;

namespace SistemaAcademia.Models
{
    public class AcessoCatraca
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime DataAcesso { get; set; }
        public bool AcessoLiberado { get; set; }
        public string MotivoBloqueio { get; set; }
    }
}
