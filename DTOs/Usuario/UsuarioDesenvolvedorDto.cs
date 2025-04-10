using PlataformaFbj.DTOs.Jogo;

namespace PlataformaFbj.DTOs.Usuario
{
    public class DesenvolvedorDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Tipo { get; set; }
        public List<JogoDto> Jogos { get; set; }
    }

}