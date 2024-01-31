using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Models;

public class Filme
{

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50, ErrorMessage = "Erro"), MinLength(1)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MaxLength(50, ErrorMessage = ""), MinLength(1)]
    public string Genero { get; set; } = "";

    [Required]
    public int Duracao { get; set; }
}
