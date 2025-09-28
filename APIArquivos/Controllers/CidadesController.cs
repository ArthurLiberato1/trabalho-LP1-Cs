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

        /// <summary>
        /// Obter todas as cidades.
        /// </summary>
        /// <returns></returns>
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
        /*
        [HttpPut("/{id}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AlterarCidadesAsync([FromRoute]int id,[FromBody] CidadeAlterarRequest request)
        {
            try
            {
                var cidade = new Cidade
                {
                    Nome = request.Nome,
                    Sigla = request.Sigla,
                    IBGEMunicipio = request.IBGEMunicipio,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude
                };

                var alterou = await _cidadesService.AlterarCidadeAsync(id, cidade);
                
                if(!alterou)
                    return NotFound($"Cidade {id} não encontrada para alteração.");
                return Ok($"Cidade {id}, {cidade.Nome} alterada com sucesso!");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletarCidadeAsync(int id)
        {
            try
            {
                var deletou = await _cidadesService.RemoverCidadeAsync(id);
                if(!deletou)
                    return NotFound($"Cidade {id} não encontrada para remoção.");
                return Ok($"Cidade {id} removida com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }

        }*/

        /// <summary>
        /// Retorna uma cidade pelo CidadeId.
        /// </summary>
        /// <param name="id">Id da cidade a ser obtida.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Retorna todas os estados.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Retorna todas as cidades de um estado específico.
        /// </summary>
        /// <param name="uf">UF do estado</param>
        /// <returns></returns>
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
                if (cidades.Count()==0 || cidades ==null)
                    return NotFound($"Não foi possível encontrar nenhuma cidade no estado: {uf}!");
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


        /// <summary>
        /// Receber o arquivo e importá-lo.
        /// </summary>
        /// <param name="arquivo">arquivo.csv a ser importado</param>
        /// <returns></returns>
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
            if (Path.GetExtension(arquivo.FileName).ToLower() != ".csv")
            {
                return BadRequest("Tipo de arquivo inválido. Por favor, envie um arquivo .csv!");
            }


            using (var stream = arquivo.OpenReadStream())
            {
                try
                {
                    var importacao = await _cidadesService.ImportarCidadesLoteAsync(stream);

                    if (importacao)
                    {
                        return Ok($"Arquivo {arquivo.Name} processado com sucesso!");
                    }
                    else
                    {
                        return BadRequest($"Ocorreu um erro ao processar o arquivo: {arquivo.Name}");
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
