using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace Gateway.Infrastructure.db
{
    public class NpgsqlDbConnectionFactory(string connectionString) : IDbConnectionFactory
    {
        private readonly string _connectionString = connectionString;

        public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync(token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new ConnectionException("Failed to open a connection with postgres database.", ex);
            }
            return connection;
        }
    }
}