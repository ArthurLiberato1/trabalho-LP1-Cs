using ArquivosLibrary.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquivosLibrary.Repository
{
    public class CidadeRepository
    {
        private readonly DbContext _dbContext;

        public CidadeRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<bool> AdicionarAsync(Cidade cidade)
        {
            try
            {
                await using var con = await _dbContext.GetConnectionAsync();
                await using var cmd = con.CreateCommand();
                int x = -1;
                cmd.CommandText = "insert into aluno1.Cidade (CidadeId, Nome, Sigla, IBGEMunicipio,Latitude, Longitude) values (@CidadeId, @Nome, @Sigla, @IBGEMunicipio, @Latitude, @Longitude)";
                cmd.Parameters.AddWithValue("@CidadeId", cidade.CidadeId);
                //adicionar verificação para quando a stirng não tiver ""
                cmd.Parameters.AddWithValue("@Nome", cidade.Nome);
                cmd.Parameters.AddWithValue("@Sigla", cidade.Sigla);
                cmd.Parameters.AddWithValue("@IBGEMunicipio", cidade.IBGEMunicipio);
                cmd.Parameters.AddWithValue("@Latitude", cidade.Latitude);
                cmd.Parameters.AddWithValue("@Longitude", cidade.Longitude);
                await cmd.ExecuteNonQueryAsync();//erro aqui apontado no debug

                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Erro ao adicionar Cidade: {cidade.Nome}");
            }
        }

        public async Task<IEnumerable<Cidade>> ObterTodasAsync()
        {
            try
            {
                List<Cidade> cidades = new List<Cidade>();

                await using var con = await _dbContext.GetConnectionAsync();
                await using var cmd = con.CreateCommand();
                cmd.CommandText = "select * from Cidade";

                await using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    var cidade = new Cidade();
                    cidade.CidadeId = (int)dr["CidadeId"];
                    cidade.Nome = dr["Nome"].ToString();
                    cidade.Sigla = dr["Sigla"].ToString();
                    cidade.IBGEMunicipio = (int)dr["IBGEMunicipio"];
                    cidade.Longitude = dr["Longitude"].ToString();
                    cidade.Latitude = dr["Latitude"].ToString();


                    cidades.Add(cidade);
                }

                return cidades;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<string>> ObterTodosUF()
        {
            try
            {
                List<string> UFs = new List<string>();
                await using var con = await _dbContext.GetConnectionAsync();
                await using var cmd = con.CreateCommand();
                cmd.CommandText = "select distinct Sigla from Cidade";

                await using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    UFs.Add(dr["Sigla"].ToString());
                }

                return UFs;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Cidade?> ObterPorIdAsync(int cidadeId)
        {
            try
            {
                Cidade? cidade = null;

                await using var con = await _dbContext.GetConnectionAsync();
                await using var cmd = con.CreateCommand();
                cmd.CommandText = "select * from Cidade where CidadeId = " + cidadeId;

                await using var dr = await cmd.ExecuteReaderAsync();

                if (await dr.ReadAsync())
                {
                    cidade = new Cidade();
                    cidade.CidadeId = (int)dr["CidadeId"];
                    cidade.Nome = dr["Nome"].ToString();
                    cidade.IBGEMunicipio = (int)dr["IBGEMunicipio"];
                    cidade.Sigla = dr["Sigla"].ToString();
                    cidade.Latitude = dr["Latitude"].ToString();
                    cidade.Longitude = dr["Longitude"].ToString();
                }

                return cidade;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Cidade>> ObterCidadesPorUfAsync(string Sigla)
        {
            try
            {
                List<Cidade> cidades = new List<Cidade>();

                await using var con = await _dbContext.GetConnectionAsync();
                await using var cmd = con.CreateCommand();
                cmd.CommandText = "select * from Cidade where Sigla = " + Sigla;

                await using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    var cidade = new Cidade();
                    cidade.CidadeId = (int)dr["CidadeId"];
                    cidade.Nome = dr["Nome"].ToString();
                    cidade.Sigla = dr["Sigla"].ToString();
                    cidade.IBGEMunicipio = (int)dr["IBGEMunicipio"];
                    cidade.Longitude = dr["Longitude"].ToString();
                    cidade.Latitude = dr["Latitude"].ToString();


                    cidades.Add(cidade);
                }

                return cidades;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
