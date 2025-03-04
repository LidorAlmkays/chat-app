using Domain.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.UserRepository
{
    public interface IUserRepository
    {
        /// <summary>
        /// Attempts to insert a user in the database.
        /// </summary>
        /// <param name="user">The user model containing user details.</param>
        /// <exception cref="ConnectionException">Thrown when there is a failure connecting to the database.</exception>
        /// <exception cref="CreateUserException">Thrown when user insertion fails due to business rules or database constraints.</exception>
        Task<int> InsertUser(UserModel user);

    }
}