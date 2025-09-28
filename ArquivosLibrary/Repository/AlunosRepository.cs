using MySql.Data.MySqlClient;
using ArquivosLibrary.Entidades;

namespace ArquivosLibrary.Repository
{
    public class AlunosRepository
    {
        private readonly DbContext _context;
        
        public AlunosRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<bool> AdicionarAsync(Aluno aluno)
        {
            try
            {
                await using var con = await _context.GetConnectionAsync();
                await using var cmd = con.CreateCommand();
                cmd.CommandText = "insert into aluno1.Aluno (Nome, RA) values (@Nome, @RA)";
                cmd.Parameters.AddWithValue("@Nome", aluno.Nome);
                cmd.Parameters.AddWithValue("@RA", aluno.RA);
                await cmd.ExecuteNonQueryAsync();
                aluno.Id = (int)cmd.LastInsertedId;

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            try
            {
                await using var con = await _context.GetConnectionAsync();
                await using var cmd = con.CreateCommand();
                cmd.CommandText = "delete from aluno1.Aluno where AlunoId = " + id;
                int qtdeLinhas = await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Aluno?> ObterPorIdAsync(int alunoId)
        {
            try
            {
                Aluno? aluno = null;

                await using var con = await _context.GetConnectionAsync();
                await using var cmd = con.CreateCommand();
                cmd.CommandText = "select * from aluno1.Aluno where AlunoId = " + alunoId;

                await using var dr = await cmd.ExecuteReaderAsync();

                if (await dr.ReadAsync())
                {
                    aluno = new Aluno();
                    aluno.Id = (int)dr["AlunoId"];
                    aluno.Nome = dr["Nome"].ToString();
                    aluno.RA = dr["RA"].ToString();
                }

                return aluno;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Aluno>> ObterTodosAsync()
        {
            try
            {
                List<Aluno> alunos = new List<Aluno>();

                await using var con = await _context.GetConnectionAsync();
                await using var cmd = con.CreateCommand();
                cmd.CommandText = "select * from aluno1.Aluno";

                await using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    var aluno = new Aluno();
                    aluno.Id = (int)dr["AlunoId"];
                    aluno.Nome = dr["Nome"].ToString();
                    aluno.RA = dr["RA"].ToString();

                    alunos.Add(aluno);
                }

                return alunos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> ContarAlunosAsync()
        {
            try
            {
                int conta = 0;

                await using var con = await _context.GetConnectionAsync();
                await using var cmd = con.CreateCommand();
                cmd.CommandText = "select count(*) from Aluno";

                var aux = await cmd.ExecuteScalarAsync();

                conta = Convert.ToInt32(aux);

                return conta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<bool> AdicionarLoteAsync(List<Aluno> alunos)
        {
            try
            {
                await using var con = await _context.GetConnectionAsync();
                await using var cmd = con.CreateCommand();

                cmd.CommandText = "insert into Aluno (Nome, RA) values (@Nome, @RA)";

                MySqlTransaction transaction = await con.BeginTransactionAsync();

                try
                {
                    foreach (var aluno in alunos)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Nome", aluno.Nome);
                        cmd.Parameters.AddWithValue("@RA", aluno.RA);

                        int qtdeLinhas = await cmd.ExecuteNonQueryAsync();
                        aluno.Id = (int)cmd.LastInsertedId;
                    }
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }
}
