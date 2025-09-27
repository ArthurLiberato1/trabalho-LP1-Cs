using APIArquivos.DTOs.Cidades;
using ArquivosLibrary.Services;
using Microsoft.AspNetCore.Http;
using ArquivosLibrary.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace APIArquivos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CidadesController : ControllerBase
    {
        /*Injeção de dependência*/
        private readonly CidadesService _cidadesService;
        public CidadesController(CidadesService cidadesService)
        {
            _cidadesService = cidadesService;
        }
        /*Injeção de dependência*/


        [HttpGet]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterTodasCidades()
        {
            try
            {
                var cidades = await _cidadesService.ObterTodasCidadesAsync();


                List<CidadeObterResponse> cidadesResponse = new List<CidadeObterResponse>();

                foreach (var cidade in cidades)
                {
                    cidadesResponse.Add(new CidadeObterResponse
                    {
                        CidadeId = cidade.CidadeId,
                        Nome = cidade.Nome,
                        Sigla = cidade.Sigla
                    });
                }


                return Ok(cidadesResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterCidadeId(int id)
        {
            try
            {
                var cidade = await _cidadesService.ObterCidadePorIdAsync(id);
                if (cidade == null)
                    return NotFound($"Não foi possível encontrar a cidade: {id}.");
                CidadeObterResponse cidadeObterResponse = new CidadeObterResponse
                {
                    CidadeId = cidade.CidadeId,
                    Nome = cidade.Nome,
                    Sigla = cidade.Sigla
                };
                return Ok(cidadeObterResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        //analisar melhor
        [HttpGet("estados")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterTodosEstados()
        {
            try
            {
                var estados = await _cidadesService.ObterTodosEstadosAsync();

                List<string> _estados = new List<string>();

                foreach (var estado in estados)
                {

                    _estados.Add(estado);
                }

                return Ok(_estados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("estado/{uf}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterCidadesPorUf(string uf)
        {
            try
            {
                List<CidadeObterResponse> cidadesResponse = new List<CidadeObterResponse>();
                var cidades = await _cidadesService.ObterCidadesPorUfAsync(uf);
                if (cidades == null)
                    return NotFound($"Não foi possível encontrar nenhuma cidade no estado: {uf}.");
                foreach (var cidade in cidades)
                {
                    CidadeObterResponse cidadeObterResponse = new CidadeObterResponse
                    {
                        CidadeId = cidade.CidadeId,
                        Nome = cidade.Nome,
                        Sigla = cidade.Sigla
                    };
                    cidadesResponse.Add(cidadeObterResponse);
                }

                return Ok(cidadesResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }



        [HttpPost("importar")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ImportarArquivo(IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                return BadRequest("Nenhum arquivo foi enviado.");
            }
            if (arquivo.ContentType != "text/csv")
            {
                return BadRequest("Tipo de arquivo inválido. Por favor, envie um arquivo .csv.");
            }


            using (var stream = arquivo.OpenReadStream())
            {
                try
                {
                    var importacao = await _cidadesService.ImportarCidadesAsync(stream);

                    if (importacao)
                    {
                        return Ok($"Arquivo ${arquivo.Name} processado com sucesso!");
                    }
                    else
                    {
                        return BadRequest($"Ocorreu um erro ao processar o arquivo: ${arquivo.Name}");
                    }
                }
                catch (ArgumentException ex)
                {
                    return BadRequest($"Erro na importação do arquivo: {arquivo.FileName}");
                }
            }
        }

    }

}
