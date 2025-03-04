using Dapper;
using Domain.Exceptions;
using Domain.models;
using Infrastructure.db;
using System.Data;


namespace Infrastructure.UserRepository
{
    public class DbUserRepository(IDbConnectionFactory dbConnectionFactory) : IUserRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

        public async Task<int> InsertUser(UserModel user)
        {
            ArgumentNullException.ThrowIfNull(user);
            DynamicParameters parameters = new();
            IDbConnection dbConnection;
            try
            {
                dbConnection = await _dbConnectionFactory.CreateConnectionAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var errorMessage = "Error when trying to start connection";
                throw new ConnectionException(errorMessage, ex);
            }

            parameters.Add("p_username", user.UserName);
            parameters.Add("p_role", user.Role);
            parameters.Add("p_age", user.Age);
            parameters.Add("p_password", user.Password);
            parameters.Add("p_email", user.Email);
            parameters.Add("user_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            try
            {
                using (dbConnection)
                {
                    await dbConnection.ExecuteAsync("create_user_data", parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                var errorMessage = "Error when trying to create user in db";
                throw new UserEmailTakenException(errorMessage, ex);
            }
            return parameters.Get<int>("user_id");
        }
    }
}