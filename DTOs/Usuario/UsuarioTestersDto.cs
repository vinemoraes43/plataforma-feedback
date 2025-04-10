using PlataformaFbj.DTOs.Feedback;

namespace PlataformaFbj.DTOs.Usuario
{
    public class UsuarioTestersDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Tipo { get; set; }
        public List<FeedbackDto> Feedbacks { get; set; }
    }

    
}
