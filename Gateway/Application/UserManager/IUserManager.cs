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
        /// <exception cref="CreateUserException">Thrown when user creation fails due to business rules or constraints.</exception>
        Task<int> AddUserAsync(RequestCreateUserDTO user);
    }
}