public class UsuarioTestersDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Tipo { get; set; }
    public List<FeedbackDto> Feedbacks { get; set; }
}

public class FeedbackDto
{
    public int Id { get; set; }
    public string Comentario { get; set; }
    public int Nota { get; set; }
    public DateTime DataCriacao { get; set; }
}
