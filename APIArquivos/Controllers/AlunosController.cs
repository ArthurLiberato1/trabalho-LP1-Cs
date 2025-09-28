
using APIArquivos.DTOs.Alunos;
using ArquivosLibrary.Entidades;
using ArquivosLibrary.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIArquivos.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]

    public class AlunosController : ControllerBase
    {

        private readonly AlunosService _alunosService;
        private readonly string _caminhoUpload;
        
        public AlunosController(AlunosService alunosService)
        {
            _alunosService = alunosService;
            // Pasta de uploads na raiz do projeto
            _caminhoUpload = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            if (!Directory.Exists(_caminhoUpload))
                Directory.CreateDirectory(_caminhoUpload);
        }

        /// <summary>
        /// recebe o código do aluno e sua respectiva foto (que é armazenada).
        /// </summary>
        /// <param name="id"> Id do aluno a quem pertence a foto</param>
        /// <param name="arquivo"> Arquivo da imagem do aluno (.png, .jpeg, .jpg)</param>
        /// <returns></returns>
        [HttpPost("{id}/foto")]
        public async Task<IActionResult> SalvarFotoAluno(int id, IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
                return BadRequest("Nenhum arquivo enviado.");
            if (Path.GetExtension(arquivo.FileName).ToLower() != ".png" && Path.GetExtension(arquivo.FileName).ToLower() != ".jpg" && Path.GetExtension(arquivo.FileName).ToLower() != ".jpeg")
                return BadRequest("Tipo de arquivo inválido. Por favor, envie um arquivo de imagem válido! (.png, .jpeg, .jpg)");
            var aluno = await _alunosService.ObterAlunoPorIdAsync(id);
            if (aluno == null)
                return NotFound($"Não foi possível encontrar o aluno de id: {id}");
            var extensao = Path.GetExtension(arquivo.FileName);
            var nomeArquivo = $"{id}{extensao}";
            var caminhoArquivo = Path.Combine(_caminhoUpload, nomeArquivo);
            using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return Ok(new { mensagem = "Foto salva com sucesso!", nomeArquivo });

        }
        /// <summary>
        /// Retorna a foto em Base64.
        /// </summary>
        /// <param name="id"> Id do aluno para retornar a foto</param>
        /// <returns></returns>
        [HttpGet("{id}/foto")]
        public async Task <IActionResult> GetFoto(int id)
        {
            var aluno = await _alunosService.ObterAlunoPorIdAsync(id);
            if(aluno==null)
                return NotFound($"Aluno {id} não encontrado.");
            var arquivos = Directory.GetFiles(_caminhoUpload, $"{id}.*");
            if (arquivos.Length == 0)
                return NotFound($"Foto do aluno {id} não encontrada.");

            var bytes = System.IO.File.ReadAllBytes(arquivos[0]);
            var base64 = Convert.ToBase64String(bytes);

            return Ok(new { id, fotoBase64 = base64 });
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AdicionarAluno([FromBody] DTOs.Alunos.AlunoCriarRequest request)
        {
            try
            {
                Aluno aluno = new Aluno
                {
                    Nome = request.Nome
                };
                var result = await _alunosService.AdicionarAlunoAsync(aluno);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
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
        public async Task<IActionResult> ExcluirAluno(int id)
        {
            try
            {
                var result = await _alunosService.ExcluirAlunoAsync(id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
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
        public async Task<IActionResult> ObterAlunoPorId(int id)
        {
            try
            {
                var aluno = await _alunosService.ObterAlunoPorIdAsync(id);
                if (aluno == null)
                    return NotFound("Aluno não encontrado.");

                AlunoObterResponse alunoResponse = new()
                {
                    Id = aluno.Id,
                    Nome = aluno.Nome,
                    RA = aluno.RA
                };

                return Ok(alunoResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterTodosAlunos()
        {
            try
            {
                var alunos = await _alunosService.ObterTodosAlunosAsync();


                List<AlunoObterResponse> alunosResponse = new List<AlunoObterResponse>();

                foreach (var aluno in alunos)
                {
                    alunosResponse.Add(new AlunoObterResponse
                    {
                        Id = aluno.Id,
                        Nome = aluno.Nome,
                        RA = aluno.RA
                    });
                }


                return Ok(alunosResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

    }
}
