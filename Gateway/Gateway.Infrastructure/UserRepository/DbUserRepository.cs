using Dapper;
using Domain.Exceptions;
using Domain.models;
using Gateway.Domain.Exceptions;
using Gateway.Infrastructure.db;
using System.Data;


namespace Gateway.Infrastructure.UserRepository
{
    public class DbUserRepository(IDbConnectionFactory dbConnectionFactory) : IUserRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

        public async Task<bool> DeleteUserByEmail(string userEmail)
        {
            ArgumentNullException.ThrowIfNull(userEmail);
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
            parameters.Add("p_email", userEmail, dbType: DbType.String, direction: ParameterDirection.InputOutput);
            parameters.Add("p_age", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("p_username", dbType: DbType.String, direction: ParameterDirection.Output);
            parameters.Add("p_role", dbType: DbType.String, direction: ParameterDirection.Output);
            try
            {
                using (dbConnection)
                {
                    await dbConnection.ExecuteAsync("delete_user_by_email", parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                var errorMessage = "Error when trying to delete user from db";
                throw new DeleteUserByEmailException(errorMessage, ex);
            }
            return !string.IsNullOrEmpty(parameters.Get<string>("p_email"));

        }

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
                throw new CreateUserEmailTakenException(errorMessage, ex);
            }
            return parameters.Get<int>("user_id");
        }
    }
}