using Dapper;
using Gateway.Domain.Exceptions.SpecificConstraint;
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

        public async Task<UserModel> DeleteUserByEmail(string userEmail)
        {
            ArgumentNullException.ThrowIfNull(userEmail);
            IDbConnection dbConnection = await GetConnection().ConfigureAwait(false);
            var parameters = new DynamicParameters();
            parameters.Add("in_email", userEmail, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("out_birthday", dbType: DbType.Date, direction: ParameterDirection.Output);
            parameters.Add("out_created_at", dbType: DbType.Date, direction: ParameterDirection.Output);
            parameters.Add("out_password_key", dbType: DbType.String, direction: ParameterDirection.Output);
            parameters.Add("out_password", dbType: DbType.String, direction: ParameterDirection.Output);
            parameters.Add("out_username", dbType: DbType.String, direction: ParameterDirection.Output);
            parameters.Add("out_role", dbType: DbType.String, direction: ParameterDirection.Output);
            parameters.Add("out_email", dbType: DbType.String, direction: ParameterDirection.Output);
            parameters.Add("out_user_id", dbType: DbType.Guid, direction: ParameterDirection.Output);

            try
            {
                UserModel? user;
                using (dbConnection)
                {
                    await dbConnection.QueryFirstOrDefaultAsync<UserModel>("delete_user_by_email", parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                    user = new UserModel
                    {
                        Email = parameters.Get<string>("out_email"),
                        Birthday = parameters.Get<DateTime>("out_birthday"),
                        Username = parameters.Get<string>("out_username"),
                        Role = parameters.Get<string>("out_role"),
                        Id = parameters.Get<Guid>("out_user_id"),
                        PasswordKey = parameters.Get<string>("out_password_key"),
                        Password = parameters.Get<string>("out_password"),
                        CreatedAt = parameters.Get<DateTimeOffset?>("out_created_at") // Nullable DateTime
                    };
                }
                ArgumentNullException.ThrowIfNull(user);
                _logger.LogWarning($"User successfully deleted: {user.Email}");
                return user;
            }
            catch (ArgumentNullException ex)
            {
                string message = $"User deleted failed user was not found, with this email: {userEmail}";
                _logger.LogInformation(message);
                throw new UserNotFoundException(message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to delete user from db, email:{userEmail}, error: " + ex.Message);
                throw;
            }

        }

        public async Task<UserModel> GetUserByEmail(string email)
        {
            IDbConnection dbConnection = await GetConnection().ConfigureAwait(false);
            DynamicParameters parameters = new();
            var query = @"SELECT *, created_at AS ""CreatedAt"" FROM function_get_user_by_email(@in_email);";
            parameters.Add("in_email", email, dbType: DbType.String, direction: ParameterDirection.Input);
            try
            {
                using (dbConnection)
                {
                    UserModel user = await dbConnection.QueryFirstOrDefaultAsync<UserModel>(query,
                    parameters, commandType: CommandType.Text).ConfigureAwait(false)
                    ?? throw new UserNotFoundException($"User with email '{email}' not found.");
                    return user;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}");
                throw;
            }

        }

        public async Task<Guid> InsertUser(UserModel user)
        {
            ArgumentNullException.ThrowIfNull(user);
            DynamicParameters parameters = new();
            IDbConnection dbConnection = await GetConnection().ConfigureAwait(false);

            //I know i don't need to point out the type and direction.
            //But i would like people to know the query without looking at the query.
            parameters.Add("p_username", user.Username, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("p_role", user.Role, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("p_birthday", user.Birthday, dbType: DbType.Date, direction: ParameterDirection.Input);
            parameters.Add("p_password_key", user.PasswordKey, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("p_password", user.Password, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("p_email", user.Email, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("out_user_id", dbType: DbType.Guid, direction: ParameterDirection.Output);
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