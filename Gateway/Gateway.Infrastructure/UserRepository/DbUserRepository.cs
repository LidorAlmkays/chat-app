using Dapper;
using Gateway.Domain.Exceptions.database;
using Gateway.Domain.models;
using Gateway.Domain.Exceptions;
using Gateway.Infrastructure.db;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;
using Domain.Exceptions;



namespace Gateway.Infrastructure.UserRepository
{
    public class DbUserRepository(IDbConnectionFactory dbConnectionFactory, ILogger<DbUserRepository> logger) : IUserRepository
    {
        private readonly ILogger<DbUserRepository> _logger = logger;
        private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

        public async Task<bool> DeleteUserByEmail(string userEmail)
        {
            ArgumentNullException.ThrowIfNull(userEmail);
            DynamicParameters parameters = new();
            IDbConnection dbConnection = await GetConnection().ConfigureAwait(false);

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

        public async Task<Guid> InsertUser(UserModel user)
        {
            ArgumentNullException.ThrowIfNull(user);
            DynamicParameters parameters = new();
            IDbConnection dbConnection = await GetConnection().ConfigureAwait(false);

            parameters.Add("p_username", user.Username);
            parameters.Add("p_role", user.Role);
            parameters.Add("p_age", user.Age);
            parameters.Add("p_password_key", user.PasswordKey);
            parameters.Add("p_password", user.Password);
            parameters.Add("p_email", user.Email);
            parameters.Add("out_user_id", direction: ParameterDirection.Output);
            try
            {
                using (dbConnection)
                {
                    await dbConnection.ExecuteAsync("create_user_data", parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                }
            }
            catch (DbException ex)
            {
                throw SpecificConstraintExceptionFactory.CreateException(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}");
                throw;
            }
            return parameters.Get<Guid>("out_user_id");
        }

        private async Task<IDbConnection> GetConnection()
        {
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
            return dbConnection;
        }
    }


}