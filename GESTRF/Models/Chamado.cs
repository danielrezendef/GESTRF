using System.ComponentModel.DataAnnotations.Schema;

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
        public string Alteracao
        {
            get
            {
                string result = string.Empty;
                TimeSpan op = DateTime.Now - DataCri;

                if ((int)op.TotalDays > 0)
                    result = string.Format($"{(int)op.TotalDays} Dias");

                if ((int)op.TotalHours > 0)
                    result = string.Format($"{(int)op.TotalHours} Horas");

                if ((int)op.TotalMinutes > 0)
                    result = string.Format($"{(int)op.TotalMinutes} Minutos");

                return result;
            }
            set { }
        }
    }
}
