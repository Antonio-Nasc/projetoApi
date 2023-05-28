using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CinemaController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        public CinemaController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        //Seguindo arquitetura REST
        /// <summary>
        /// Adiciona um cinema
        /// </summary>
        /// <param name="filmeDto">Objetos necessários para criação de um cinema</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso inserção seja feita com sucesso</response>
        [HttpPost]
        public IActionResult AdicionarCinema([FromBody]CreateCinemaDto cinemaDto)
        {
            Cinema cinema = _mapper.Map<Cinema>(cinemaDto);
            _context.Cinemas.Add(cinema);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaCinemaPorId), new { Id = cinema.Id }, cinemaDto);
        }
        /// <summary>
        /// Obtem cinemas
        /// </summary>
        /// <param name="filmeDto">Visualização dos cinemas</param>
        /// <returns>IEnumerable</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet]
        public IEnumerable<ReadCinemaDto> RecuperaCinemas([FromQuery] int? enderecoId = null)
        {
            if(enderecoId == null)
            {
                return _mapper.Map<List<ReadCinemaDto>>(_context.Cinemas.ToList());
            }
            //método permite a passagem de parâmetro de SQL puro para execução no banco.
            return _mapper.Map<List<ReadCinemaDto>>(_context.Cinemas.FromSqlRaw($"SELECT Id, Nome, EnderecoId FROM cinemas where cinemas.EnderecoId = {enderecoId}").ToList());
            
        }
        /// <summary>
        /// Obtem cinema específico
        /// </summary>
        /// <param name="filmeDto">Visualização de um cinema específico</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet("{id}")]
        public IActionResult RecuperaCinemaPorId(int id)
        {
            Cinema cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
            if(cinema != null) 
            {
                ReadCinemaDto cinemaDto = _mapper.Map<ReadCinemaDto>(cinema);
                return Ok(cinemaDto);
            }
            return NotFound();
        }
        /// <summary>
        /// Atualiza cinema
        /// </summary>
        /// <param name="filmeDto">Atualização de cinema específico</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Sucesso</response>
        [HttpPut("{id}")]
        public IActionResult AtualizarCinema(int id, [FromBody] UpdateCinemaDto cinemaDto)
        {
            Cinema cinema = _context.Cinemas.FirstOrDefault(cinema =>cinema.Id == id);
            if(cinema == null)
            {
                return NotFound();
            }
            _mapper.Map(cinemaDto, cinema);
            _context.SaveChanges();
            return NoContent();
        }
        /// <summary>
        /// Deleta cinema específico
        /// </summary>
        /// <param name="filmeDto">Deleção de cinema específico</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Sucesso</response>
        [HttpDelete("{id}")]
        public IActionResult DeletarCinema(int id)
        {
            Cinema cinema = _context.Cinemas.FirstOrDefault(cinema =>cinema.Id == id);
            if(cinema == null)
            {
                return NotFound();
            }
            _context.Remove(cinema);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
