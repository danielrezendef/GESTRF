namespace GESTRF.Models
{
    public class Chamado
    {
        public int ChamadoId { get; set; }
        public string Assunto { get; set; }
        public string Tecnico { get; set; }
        public string Setor { get; set; }
        public DateTime DataCri { get; set; }
        public string Status { get; set; }
        public string? Doc { get; set; }
    }
}
