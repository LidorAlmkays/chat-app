using Common.DTOs;
namespace Gateway.Application.UserManager
{
    public interface IUserManager
    {
        /// <summary>
        /// Attempts to insert a new user into the database, using a stored procedure to handle the insertion process.
        /// </summary>
        /// <param name="user">The user model containing the user's details, including username, role, age, password, and email.</param>
        /// <returns>A <see cref="Guid"/> representing the unique identifier of the inserted user.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the provided user model is null.</exception>
        /// <exception cref="ConnectionException">
        /// Thrown if there is a failure in establishing a connection to the database.
        /// </exception>
        /// <exception cref="ConstraintViolationException">
        /// Thrown when a constraint violation occurs during the user insertion process. 
        /// For example:
        /// <list type="bullet">
        ///     <item><description>ValidEmail: Invalid email format.</description></item>
        ///     <item><description>CheckAge: The user's age does not meet the required criteria (e.g., less than 18).</description></item>
        ///     <item><description>UsernameNotNull: The username cannot be null.</description></item>
        ///     <item><description>ValidRole: The provided role is invalid.</description></item>
        ///     <item><description>UsernameNotEmpty: The username cannot be empty.</description></item>
        ///     <item><description>CheckPasswordLength: The password length is shorter than the required minimum.</description></item>
        ///     <item><description>EmailAlreadyExists: The email already exists in the system.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the insertion process.</exception>
        Task<ResponseCreateUserDTO> AddUserAsync(RequestCreateUserDTO user);
        /// <summary>
        /// Asynchronously deletes a user by their email from an external source (e.g., database, file, etc.).
        /// </summary>
        /// <param name="userDeleteData">The request data containing the email of the user to be deleted.</param>
        /// <returns>
        /// A task that resolves to a <see cref="ResponseDeleteUserByEmailDTO"/> containing the result of the deletion.
        /// </returns>
        /// <exception cref="UserNotFoundException">
        /// Thrown when no user is found with the specified email.
        /// </exception>
        /// <exception cref="ConnectionException">
        /// Thrown when there is a failure connecting to the external source (e.g., database).
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when an unexpected error occurs during user deletion.
        /// </exception>
        Task<ResponseDeleteUserByEmailDTO> DeleteUserByEmailAsync(RequestDeleteUserByEmailDTO userDeleteData);
        /// <summary>
        /// Retrieves a user by their email address and converts the result into a DTO.
        /// </summary>
        /// <param name="userGetData">An object containing the email of the user to retrieve.</param>
        /// <returns>
        /// A <see cref="ResponseGetUserByEmailDTO"/> representing the retrieved user.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="userGetData"/> is null.</exception>
        /// <exception cref="UserNotFoundException">Thrown if no user is found with the specified email.</exception>
        /// <exception cref="ConnectionException">Thrown if there is an error establishing a database connection.</exception>
        /// <exception cref="Exception">Thrown if an unexpected error occurs during execution.</exception>
        Task<ResponseGetUserByEmailDTO> GetUserByEmailAsync(RequestGetUserByEmailDTO userGetData);
    }
}