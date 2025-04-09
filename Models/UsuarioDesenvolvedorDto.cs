public class DesenvolvedorDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Tipo { get; set; }
    public List<JogoDto> Jogos { get; set; }
}

public class JogoDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public string Plataforma { get; set; }
}