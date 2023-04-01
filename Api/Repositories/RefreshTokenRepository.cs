using Api.Interfaces;
using Api.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Api.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly EnvironmentConfig env;

        public RefreshTokenRepository(EnvironmentConfig env)
        {
            this.env = env;
        }

        public async Task<int> Create(RefreshToken refreshToken)
        {
            var query = "INSERT INTO RefreshToken(Id,JWT,UserCode) " +
                "VALUES(@Id, @JWT, @UserCode)";
            var parameters = new DynamicParameters();
            parameters.Add("Id", Guid.NewGuid().ToString(), DbType.String);
            parameters.Add("JWT", refreshToken.JWT, DbType.String);
            parameters.Add("UserCode", refreshToken.UserCode, DbType.String);

            using (var connection = new SqlConnection(env.MSSQL_CONNECTION_STRING))
            {
                connection.Open();

                var result = await connection.ExecuteAsync(query, parameters);

                connection.Close();
                return result;
            }

        }

        public async Task<int> Delete(string UserCode)
        {
            var query = "DELETE FROM RefreshToken WHERE UserCode = @UserCode";

            var parameters = new DynamicParameters();
            parameters.Add("UserCode", UserCode, DbType.String);

            using (var connection = new SqlConnection(env.MSSQL_CONNECTION_STRING))
            {
                connection.Open();

                var result = await connection.ExecuteAsync(query, parameters);

                connection.Close();
                return result;
            }
        }

        public async Task<RefreshToken> GetByJWT(string JWT)
        {

            var query = "SELECT Id,JWT,UserCode FROM RefreshToken WHERE JWT = @JWT";

            var parameters = new DynamicParameters();
            parameters.Add("JWT", JWT, DbType.String);

            using (var connection = new SqlConnection(env.MSSQL_CONNECTION_STRING))
            {
                connection.Open();

                var result = await connection.QueryFirstOrDefaultAsync<RefreshToken>(query, parameters);

                connection.Close();
                return result;
            }
        }
    }
}
