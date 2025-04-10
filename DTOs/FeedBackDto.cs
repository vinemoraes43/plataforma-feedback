namespace PlataformaFbj.DTOs.Feedback
{
    public class FeedbackDto
    {
        public int Id { get; set; }
        public string Comentario { get; set; }
        public int Nota { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}