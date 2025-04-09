using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaFbj.Models
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Comentario { get; set; }

        [Range(0, 10)]
        public int Nota { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [ForeignKey("Jogo")]
        public int JogoId { get; set; }

        public Jogo Jogo { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }
    }
}