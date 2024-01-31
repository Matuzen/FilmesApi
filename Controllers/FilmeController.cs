using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.DTOs;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    /// <summary>
    /// Consulta de filmes utilizando o skip e take
    /// </summary>
    /// <param name="take"></param>
    /// <returns></returns>
    [HttpGet("/m1")]
    public IEnumerable<ReadFilmeDto> RecuperaFilmes(
      [FromQuery] int skip = 0,
      [FromQuery] int take = 50)
    {
        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaFilmePorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        return Ok(filmeDto);
    }


    /// <summary>
    /// Pesquisa utilizando o Linq
    /// </summary>
    /// <returns></returns>

    [HttpGet("/m2")]
    public IActionResult RecuperarFilmeLinq()
    {
        var filme = _context.Filmes.Select(f => new
        {
            f.Id,
            f.Nome,
            f.Genero,
            f.Duracao
        }).ToList();
        return Ok(filme);
    }


    /// <summary>
    /// Pesquisa utilizando o list
    /// </summary>
    /// <returns></returns>

    [HttpGet("/m3")]
    public List<Filme> TodosOsFilmes()
    {
        List<Filme> listarFilmes = new List<Filme>();

        foreach (var filme in _context.Filmes)
        {
            Console.WriteLine(filme);
            listarFilmes.Add(filme);
        }

        return listarFilmes;
    }


    /// <summary>
    /// Pesquisa utilizando o List sem o foreach
    /// </summary>
    /// <returns></returns>

    [HttpGet("/m4")]
    public IEnumerable<Filme> TodosOsFilme()
    {
        return _context.Filmes.ToList();
    }


    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto"> Objeto com os campos necessários para a criação de um filme </param>
    /// <returns> IActionResult </returns>
    /// <respose code="201"> Caso inserção seja feita com sucesso </respose>

    [HttpPost("m1")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionarFilme([FromBody] CreateFilmeDto filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaFilmePorId), new { id = filme.Id }, filme);
    }


    /// <summary>
    /// Adiciona vários filmes ao banco de dados
    /// </summary>
    /// <param name="filmesDto"> Objeto com os campos necessários para a criação de um filme </param>
    /// <returns> IActionResult </returns>
    /// <respose code="201"> Caso inserção seja feita com sucesso </respose>
 
    [HttpPost("/m2")]
    public IActionResult AdicionarFilmes([FromBody] List<CreateFilmeDto> filmesDto)
    {
        List<Filme> filmes = _mapper.Map<List<Filme>>(filmesDto);

        _context.Filmes.AddRange(filmes);
        _context.SaveChanges();

        var idsDosFilmesAdicionados = filmes.Select(f => f.Id).ToList();
        return CreatedAtAction(nameof(TodosOsFilmes), new { ids = idsDosFilmesAdicionados }, filmes);
    }


   /// <summary>
   /// Adiciona filme ao banco de dados pelo Linq
   /// </summary>
   /// <param name="filme"></param>
   /// <returns></returns>
    
    [HttpPost("/m3")]
    public IActionResult AdicionarFilmePeloLinq([FromBody] Filme filme)
    {
        var novoFilme = new Filme
        {
            Nome = filme.Nome,
            Genero = filme.Genero,
            Duracao = filme.Duracao
        };

        _context.Filmes.Add(novoFilme);
        _context.SaveChanges();

        return CreatedAtAction(nameof(AdicionarFilmePeloLinq), new { id = novoFilme.Id }, novoFilme);
    }

    [HttpPut("{id}")]
    public IActionResult AtualizarFilme(int id, [FromBody]
    UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault(f => f.Id == id);
            if (filme == null) return NotFound();
        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizarFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
    { 
        var filme = _context.Filmes.FirstOrDefault(f => f.Id == id);
        if (filme == null) return NotFound();

        // converter o filme que pegamos do banco para um UpdateFilmeDto para aplicar as alterações
        // e se caso este filme estiver válido, vamos converter ele de novo para um filme

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);

        patch.ApplyTo(filmeParaAtualizar, ModelState);
        if (!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(filmeParaAtualizar, filme); 
        _context.SaveChanges();
        return NoContent(); 
    }

    [HttpDelete("{id}")]
    public IActionResult DeletarFilme(int id)
    {
        var deletarFilme = _context.Filmes.FirstOrDefault(f =>f.Id == id);
        if (deletarFilme == null) return NotFound();
        _context.Remove(deletarFilme);
        _context.SaveChanges();

        return NoContent();
    }
}
