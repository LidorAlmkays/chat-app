using DTOs;
namespace Application.UserManager
{
    public interface IUserManager
    {
        /// <summary>
        /// Attempts to create a user and insert into the Registry.
        /// </summary>
        /// <param name="user">The user model containing user details.</param>
        /// <exception cref="ConnectionException">Thrown when there is a failure with the Registry.</exception>
        /// <exception cref="CreateUserEmailTakenException">Thrown when user creation fails due to business rules or constraints.</exception>
        Task<int> AddUserAsync(RequestCreateUserDTO user);
        /// <summary>
        /// Asynchronously deletes a user by their email from an external source (e.g., database, file, etc.).
        /// </summary>
        /// <param name="userEmail">The email of the user to be deleted.</param>
        /// <returns>
        /// A task that resolves to <c>true</c> if the deletion was successful, 
        /// otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="ConnectionException">
        /// Thrown when there is a failure connecting to the external source.
        /// </exception>
        /// <exception cref="DeleteUserByEmailException">
        /// Thrown when user deletion fails due to business rules or constraints specific 
        /// to the external source (e.g., database, file system).
        /// </exception>
        Task<bool> DeleteUserByEmailAsync(string userEmail);

    }
}