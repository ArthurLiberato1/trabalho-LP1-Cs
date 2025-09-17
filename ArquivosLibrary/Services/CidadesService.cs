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

        public  CidadesService(CidadeRepository cidadesRepository)
        {
            _cidadesRepository = cidadesRepository;
        }
        /*Injeção de dependência*/

        public async Task<bool> ImportarCidadesAsync(Stream arquivo)
        {
            //adicionar verificação do tipo de arquivo
            try
            {
                var reader = new StreamReader(arquivo);
                while (!reader.EndOfStream)
                {
                    var linha=reader.ReadLine();
                    if(string.IsNullOrEmpty(linha))
                        throw new ArgumentException("Erro ao ler a linha do arquivo.");
                        
                    var colunas = linha.Split(',');
                    var cidade = new Cidade
                    {
                        CidadeId = int.Parse(colunas[0]),
                        Nome=colunas[1],
                        Sigla=colunas[2],
                        IBGEMunicipio = int.Parse(colunas[3]),
                        Latitude=colunas[4],
                        Longitude=colunas[5],
                    };
                    var inseriu= await _cidadesRepository.AdicionarAsync(cidade);
                    if(!inseriu)
                        throw new ArgumentException($"Falha ao ler a cidade {cidade.Nome}.");
                }
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

        public async Task<Cidade?> ObterCidadePorIdAsync(int CidadeId)
        {
            return await _cidadesRepository.ObterPorIdAsync(CidadeId);
        }

        public async Task<IEnumerable<string>> ObterTodosEstadosAsync()
        {
            return await _cidadesRepository.ObterTodosUF();
        }

        public async Task<Cidade?> ObterCidadePorUfAsync(string uf)
        {
            return await _cidadesRepository.ObterCidadesPorUfAsync(uf);
        }

    }
}
