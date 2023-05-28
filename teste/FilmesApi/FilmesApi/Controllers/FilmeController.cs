using Microsoft.AspNetCore.Mvc;
using FilmesApi.Models;
using System.Diagnostics;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace FilmesApi.Controllers
{
    //Controller serve como local das requisições e respota do cliente
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
        /// Adiciona um filme
        /// </summary>
        /// <param name="filmeDto">Objetos necessários para criação de um filme</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso inserção seja feita com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AdicionarFilme([FromBody]CreateFilmeDto filmeDto)
        {
            Filme filme = _mapper.Map<Filme>(filmeDto);
            _context.Filmes.Add(filme);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaFilmePorId), new {id = filme.Id}, filme);
        }
        /// <summary>
        /// Obtem filmes
        /// </summary>
        /// <param name="filmeDto">Visualização dos filmes</param>
        /// <returns>IEnumerable</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet]
        public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery]int skip = 0, [FromQuery] int take = 50, [FromQuery] string? nomeCinema = null)
        {
            if(nomeCinema == null)
            {
                return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take).ToList());
            }
            return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take).Where(filme => filme.Sessoes.Any(sessao => sessao.Cinema.Nome == nomeCinema)).ToList());
        }
        /// <summary>
        /// Obtem filme específico
        /// </summary>
        /// <param name="filmeDto">Visualização de um filme específico</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet("{id}")]
        public IActionResult RecuperaFilmePorId(int id)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if(filme == null) return NotFound();
            var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
            return Ok(filmeDto);
        }
        /// <summary>
        /// Atualiza filme
        /// </summary>
        /// <param name="filmeDto">Atualização de filme específico</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Sucesso</response>
        [HttpPut("{id}")]
        public IActionResult AtualizaFilme(int id, [FromBody]UpdateFilmeDto filmeDto)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if(filme == null) return NotFound();
            _mapper.Map(filmeDto, filme);
            _context.SaveChanges();
            //Para retornar um status atualizado, se usa NoContent()
            return NoContent();

        }
        /// <summary>
        /// Atualiza filme parcial
        /// </summary>
        /// <param name="filmeDto">Atualização de filme específico e parcial</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Sucesso</response>
        [HttpPatch("{id}")]
        public IActionResult AtualizaFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if (filme == null) return NotFound();
            var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);
            patch.ApplyTo(filmeParaAtualizar, ModelState);
            if (!TryValidateModel(filmeParaAtualizar))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(filmeParaAtualizar, filme);
            _context.SaveChanges();
            //Para retornar um status atualizado, se usa NoContent()
            return NoContent();

        }
        /// <summary>
        /// Deleta filme específico
        /// </summary>
        /// <param name="filmeDto">Deleção de filme específico</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Sucesso</response>
        [HttpDelete("{id}")]
        public IActionResult RemoveFilme(int id)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if (filme == null) return NotFound();
            _context.Remove(filme);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
