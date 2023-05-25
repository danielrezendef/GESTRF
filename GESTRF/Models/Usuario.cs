namespace GESTRF.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Username { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public string Perfil { get; set; }
        public string? image { get; set; }
    }
}
