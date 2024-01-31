using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Data.DTOs;

public class CreateFilmeDto
{
    [Required]
    [StringLength(50, ErrorMessage = "Digite um valor entre 1 e 50")]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(50, ErrorMessage = "Digite um valor entre 1 e 50")]
    public string Genero { get; set; } = "";

    [Required]
    [Range(70,300, ErrorMessage = "A duração deve ter entre 70 e 300 minutos")]
    public int Duracao { get; set; }
}        
