using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnderecoController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        public EnderecoController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        //Seguindo arquitetura REST
        /// <summary>
        /// Adiciona um endereço
        /// </summary>
        /// <param name="filmeDto">Objetos necessários para criação de uma endereço</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Sucesso</response>
        [HttpPost]
        public IActionResult AdicionaEndereco([FromBody] CreateEnderecoDto enderecoDto)
        {
            Endereco endereco = _mapper.Map<Endereco>(enderecoDto);
            _context.Enderecos.Add(endereco);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaEnderecosPorId), new { id = endereco.Id }, endereco);
        }
        /// <summary>
        /// Obtem endereços
        /// </summary>
        /// <param name="filmeDto">Visualização dos endereços</param>
        /// <returns>IEnumerable</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet]
        public IEnumerable<ReadEnderecoDto> RecuperaEndereco()
        {
            return _mapper.Map<List<ReadEnderecoDto>>(_context.Enderecos);
        }
        /// <summary>
        /// Obtem endereço específico
        /// </summary>
        /// <param name="filmeDto">Visualização de um endereço específico</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet("id")]
        public IActionResult RecuperaEnderecosPorId(int id)
        {
            Endereco endereco = _context.Enderecos.FirstOrDefault(endereco => endereco.Id == id);
            if (endereco != null)
            {
                ReadEnderecoDto enderecoDto = _mapper.Map<ReadEnderecoDto>(endereco);

                return Ok(enderecoDto);
            }
            return NotFound();
        }
        /// <summary>
        /// Atualiza endereço específico
        /// </summary>
        /// <param name="filmeDto">Atualização de endereço específico</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Sucesso</response>
        [HttpPut("{id}")]
        public IActionResult AtualizaEndereco(int id, [FromBody] UptadeEnderecoDto enderecoDto)
        {
            Endereco endereco = _context.Enderecos.FirstOrDefault(endereco => endereco.Id == id);
            if (endereco == null)
            {
                return NotFound();
            }
            _mapper.Map(enderecoDto, endereco);
            _context.SaveChanges();
            return NoContent();
        }
        /// <summary>
        /// Deleta filme específico
        /// </summary>
        /// <param name="filmeDto">Deleção de endereço específico</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Sucesso</response>
        [HttpDelete("{id}")]
        public IActionResult DeletaEndereco(int id)
        {
            Endereco endereco = _context.Enderecos.FirstOrDefault(endereco => endereco.Id == id);
            if (endereco == null)
            {
                return NotFound();
            }
            _context.Remove(endereco);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
