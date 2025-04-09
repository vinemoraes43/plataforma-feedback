using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaFbj.Models
{
    public class Jogo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Descricao { get; set; }

        [Required]
        public string Plataforma { get; set; } // Ex: PC, Console, Mobile

        [ForeignKey("Desenvolvedor")]
        public int DesenvolvedorId { get; set; }

        public Usuario Desenvolvedor { get; set; }

        public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}