using ArquivosLibrary.Entidades;
using ArquivosLibrary.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquivosLibrary.Services
{
    public class CidadesService
    {
        /*Injeção de dependência*/
        private readonly CidadeRepository _cidadesRepository;

        public CidadesService(CidadeRepository cidadesRepository)
        {
            _cidadesRepository = cidadesRepository;
        }
        /*Injeção de dependência*/

        public async Task<bool> ImportarCidadesAsync(Stream arquivo)
        {
            try
            {
                var reader = new StreamReader(arquivo);
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var linha = reader.ReadLine();
                    if (string.IsNullOrEmpty(linha))
                        throw new ArgumentException("Erro ao ler a linha do arquivo.");

                    var colunas = linha.Split(',');//retorna vetor
                    colunas[1] = colunas[1].Trim('\'', '"').Trim();
                    var cidade = new Cidade
                    {
                        CidadeId = int.Parse(colunas[0]),
                        Nome = colunas[1],
                        Sigla = colunas[2],
                        IBGEMunicipio = int.Parse(colunas[3]),
                        Latitude = colunas[4],
                        Longitude = colunas[5],
                    };
                    var inseriu = await _cidadesRepository.AdicionarAsync(cidade);
                    if (!inseriu)
                        throw new ArgumentException($"Falha ao ler a cidade {cidade.Nome}.");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro na leitura do arquivo");
            }
        }

        public async Task<bool> ImportarCidadesLoteAsync(Stream arquivo)
        {
            try
            {
                List <Cidade> cidades= new List<Cidade>();

                var reader = new StreamReader(arquivo);
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var linha = reader.ReadLine();
                    if (string.IsNullOrEmpty(linha))
                        throw new ArgumentException("Erro ao ler a linha do arquivo.");

                    var colunas = linha.Split(',');//retorna vetor
                    colunas[1] = colunas[1].Trim('\'', '"').Trim();
                    var cidade = new Cidade
                    {
                        CidadeId = int.Parse(colunas[0]),
                        Nome = colunas[1],
                        Sigla = colunas[2],
                        IBGEMunicipio = int.Parse(colunas[3]),
                        Latitude = colunas[4],
                        Longitude = colunas[5],
                    };
                    cidades.Add(cidade);
                }
                var inseriu = await _cidadesRepository.AdicionarLoteAsync(cidades);
                if(!inseriu)
                    throw new ArgumentException("Erro na leitura do arquivo");
                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro na leitura do arquivo");
            }
        }

        public async Task<IEnumerable<Cidade>> ObterTodasCidadesAsync()
        {
            return await _cidadesRepository.ObterTodasAsync();
        }

        public async Task<bool> RemoverCidadeAsync(int id)
        {
            var _cidade = await ObterCidadePorIdAsync(id);
            if (_cidade == null)
                throw new ArgumentException($"A cidade {id} não existe!");

            return await _cidadesRepository.RemoverCidadeAsync(id);
        }

        public async Task<bool> AlterarCidadeAsync(int id, Cidade cidade)
        {
            var _cidade = await ObterCidadePorIdAsync(id);
            if(_cidade==null)
                throw new ArgumentException($"A cidade {id} não existe!");

            return await _cidadesRepository.AlterarCidadeAsync(id,cidade);
        }
        public async Task<Cidade?> ObterCidadePorIdAsync(int CidadeId)
        {
            return await _cidadesRepository.ObterPorIdAsync(CidadeId);
        }

        public async Task<IEnumerable<string>> ObterTodosEstadosAsync()
        {
            return await _cidadesRepository.ObterTodosUF();
        }

        public async Task<IEnumerable<Cidade>> ObterCidadesPorUfAsync(string uf)
        {
            return await _cidadesRepository.ObterCidadesPorUfAsync(uf.ToUpper());
        }

    }
}
