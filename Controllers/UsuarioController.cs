using Microsoft.AspNetCore.Mvc;
using StartTrekGS.src.StartTrekGS.Application.DTO;
using StartTrekGS.src.StartTrekGS.Application.Service;

namespace StartTrekGS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> ListarUsuarios(
            int page = 1,
            int tamanho = 10)
        {
            var pagina = await _usuarioService.ListarUsuariosPaginadoAsync(page, tamanho);
            return Ok(pagina);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] UsuarioCreateDto dto)
        {
            try
            {
                var salvo = await _usuarioService.CadastrarUsuarioAsync(dto);
                return Created("", salvo);
            }
            catch (Exception e)
            {
                return BadRequest(new { erro = e.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            try
            {
                var usuario = await _usuarioService.BuscarPorIdAsync(id);

                var links = new[]
                {
                    new
                    {
                        rel = "self",
                        href = Url.Action(nameof(BuscarPorId), new { id }),
                        method = "GET"
                    },
                    new
                    {
                        rel = "update",
                        href = Url.Action(nameof(Atualizar), new { id }),
                        method = "PUT"
                    },
                    new
                    {
                        rel = "delete",
                        href = Url.Action(nameof(Deletar), new { id }),
                        method = "DELETE"
                    },
                    new
                    {
                        rel = "search",
                        href = Url.Action(nameof(Search)),
                        method = "GET"
                    }
                };

                return Ok(new
                {
                    data = usuario,
                    links
                });
            }
            catch (Exception e)
            {
                return Problem(
                    statusCode: 404,
                    title: "Usuário não encontrado",
                    detail: e.Message,
                    instance: HttpContext.Request.Path
                );
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(
            int id,
            [FromBody] UsuarioUpdateDto dto)
        {
            try
            {
                var atualizado = await _usuarioService.AtualizarUsuarioAsync(id, dto);
                return Ok(atualizado);
            }
            catch (Exception e)
            {
                return BadRequest(new { erro = e.Message });
            }
        }

        [HttpPatch("{id}/foto")]
        public async Task<IActionResult> AtualizarFoto(
            int id,
            [FromForm] UsuarioFotoDto dto)
        {
            try
            {
                var atualizado = await _usuarioService.AtualizarFotoAsync(id, dto);
                return Ok(atualizado);
            }
            catch (Exception)
            {
                return BadRequest(new { erro = "Erro ao enviar a foto." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {
                await _usuarioService.DeletarUsuarioAsync(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return NotFound(new { erro = e.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? nome,
            [FromQuery] bool? ativo,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanho = 10,
            [FromQuery] string ordenarPor = "nome",
            [FromQuery] string direcao = "asc")
        {
            try
            {
                var resultado = await _usuarioService.PesquisarAsync(
                    nome, ativo, pagina, tamanho, ordenarPor, direcao);

                var links = new[]
                {
                    new
                    {
                        rel = "self",
                        href = Url.Action(nameof(Search), new { nome, ativo, pagina, tamanho, ordenarPor, direcao }),
                        method = "GET"
                    },
                    new
                    {
                        rel = "create",
                        href = Url.Action(nameof(Cadastrar)),
                        method = "POST"
                    }
                };

                return Ok(new
                {
                    resultado.Pagina,
                    resultado.Tamanho,
                    resultado.TotalRegistros,
                    dados = resultado.Dados,
                    links
                });
            }
            catch (Exception e)
            {
                return Problem(
                    statusCode: 400,
                    title: "Erro ao pesquisar usuários",
                    detail: e.Message,
                    instance: HttpContext.Request.Path
                );
            }
        }
    }
}
