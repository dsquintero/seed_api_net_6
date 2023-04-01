using Api.Interfaces;
using Api.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Api.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly EnvironmentConfig env;

        public AuthenticationRepository(EnvironmentConfig env)
        {
            this.env = env;
        }

        public async Task<User> GetByUserCode(string UserCode)
        {
            string query = "SELECT [Id],[UserCode],[Email],[Password],[LastName],[FirsName],[Active] FROM [dbo].[User] WHERE [UserCode] = @UserCode AND [Active] = 'Y' ";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserCode", UserCode, DbType.String);

            using (var connection = new SqlConnection(env.MSSQL_CONNECTION_STRING))
            {
                connection.Open();

                var result = await connection.QueryFirstOrDefaultAsync<User>(query, parameters);

                connection.Close();
                return result;
            }
        }
    }
}
