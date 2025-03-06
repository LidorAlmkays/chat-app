using Domain.models;

namespace Gateway.Infrastructure.UserRepository
{
    public interface IUserRepository
    {
        /// <summary>
        /// Attempts to insert a user in the database.
        /// </summary>
        /// <param name="user">The user model containing user details.</param>
        /// <exception cref="ConnectionException">Thrown when there is a failure connecting to the database.</exception>
        /// <exception cref="CreateUserEmailTakenException">Thrown when user insertion fails due to business rules or database constraints.</exception>
        Task<int> InsertUser(UserModel user);
        /// <summary>
        /// Deletes a user from the database by their email.
        /// </summary>
        /// <param name="userEmail">The email of the user to be deleted.</param>
        /// <returns>
        /// A task that resolves to <c>true</c> if the deletion was successful 
        /// and the user was removed, otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="ConnectionException">
        /// Thrown when there is a failure connecting to the database.
        /// </exception>
        /// <exception cref="DeleteUserByEmailException">
        /// Thrown when user deletion fails due to business rules or database constraints.
        /// </exception>
        Task<bool> DeleteUserByEmail(string userEmail);

    }
}