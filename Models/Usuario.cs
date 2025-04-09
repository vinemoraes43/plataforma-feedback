using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaFbj.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;

        [Required]
        public string Tipo { get; set; } = string.Empty; // "Desenvolvedor" ou "BetaTester"

        // Desenvolvedor
        public List<Jogo> Jogos { get; set; } = new();

        // BetaTester
        [InverseProperty("Usuario")]
        public List<Feedback> Feedbacks { get; set; } = new();
    }

    public class LoginRequest
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;
    }
}