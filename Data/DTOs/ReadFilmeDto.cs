namespace FilmesApi.Data.DTOs;

public class ReadFilmeDto
{
    public string titulo { get; set; }
    public string Genero { get; set; }
    public int Duracao { get; set; }

    public DateTime HoraDaConsulta { get; set; } = DateTime.Now;
}
