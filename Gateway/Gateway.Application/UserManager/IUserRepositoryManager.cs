using Common.DTOs;
namespace Gateway.Application.UserManager
{
    public interface IUserManager
    {
        /// <summary>
        /// Attempts to insert a new user into the database using a stored procedure to handle the insertion process.
        /// </summary>
        /// <param name="user">
        /// The request model containing the user's details, including username, role, birthday, password, and email.
        /// </param>
        /// <returns>
        /// A <see cref="ResponseCreateUserDTO"/> representing the result of the user insertion process.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if the provided <paramref name="user"/> is null.</exception>
        /// <exception cref="ConnectionException">
        /// Thrown if there is a failure in establishing a connection to the database.
        /// </exception>
        /// <exception cref="ConstraintViolationException">
        /// Thrown when a constraint violation occurs during the user insertion process. The specific violation is indicated by
        /// the <see cref="ConstraintViolationException.ConstraintType"/> property. Possible violations include:
        /// <list type="bullet">
        ///     <item><description><see cref="ConstraintType.ValidEmail"/>: The provided email format is invalid.</description></item>
        ///     <item><description><see cref="ConstraintType.CheckBirthday"/>: The user's age does not meet the required criteria.</description></item>
        ///     <item><description><see cref="ConstraintType.ValidRole"/>: The provided role is invalid. Allowed roles: Guest, User, Admin.</description></item>
        ///     <item><description><see cref="ConstraintType.UsernameNotEmpty"/>: The username cannot be empty.</description></item>
        ///     <item><description><see cref="ConstraintType.UsernameNotNull"/>: The username is required and cannot be null.</description></item>
        ///     <item><description><see cref="ConstraintType.CheckPasswordLength"/>: The password must be at least 8 characters long.</description></item>
        ///     <item><description><see cref="ConstraintType.UniqueEmail"/>: The email is already in use by another user.</description></item>
        ///     <item><description><see cref="ConstraintType.PasswordNotNull"/>: The password is required and cannot be null.</description></item>
        ///     <item><description><see cref="ConstraintType.RoleNotNull"/>: The role is required and cannot be null.</description></item>
        ///     <item><description><see cref="ConstraintType.PasswordKeyNotNull"/>: The password key is required and cannot be null.</description></item>
        ///     <item><description><see cref="ConstraintType.EmailNotNull"/>: The email is required and cannot be null.</description></item>
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
        /// <summary>
        /// Updates a user's information in the database based on their email.
        /// </summary>
        /// <param name="data">
        /// The request model containing the user's email and the updated user details, such as username, birthday, role, and password.
        /// </param>
        /// <returns>
        /// A <see cref="ResponseUpdateUserByEmailDTO"/> indicating the result of the update operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if the provided <paramref name="data"/> is null.</exception>
        /// <exception cref="ConnectionException">
        /// Thrown if there is a failure in establishing a connection to the database.
        /// </exception>
        /// <exception cref="ConstraintViolationException">
        /// Thrown when a constraint violation occurs during the update process. The specific violation is indicated by 
        /// the <see cref="ConstraintViolationException.ConstraintType"/> property. Possible violations include:
        /// <list type="bullet">
        ///     <item><description><see cref="ConstraintType.ValidEmail"/>: The provided email format is invalid.</description></item>
        ///     <item><description><see cref="ConstraintType.CheckBirthday"/>: The user's age does not meet the required criteria.</description></item>
        ///     <item><description><see cref="ConstraintType.ValidRole"/>: The provided role is invalid. Allowed roles: Guest, User, Admin.</description></item>
        ///     <item><description><see cref="ConstraintType.UsernameNotEmpty"/>: The username cannot be empty.</description></item>
        ///     <item><description><see cref="ConstraintType.UsernameNotNull"/>: The username is required and cannot be null.</description></item>
        ///     <item><description><see cref="ConstraintType.CheckPasswordLength"/>: The password must be at least 8 characters long.</description></item>
        ///     <item><description><see cref="ConstraintType.UniqueEmail"/>: The new email is already in use by another user.</description></item>
        ///     <item><description><see cref="ConstraintType.PasswordNotNull"/>: The password is required and cannot be null.</description></item>
        ///     <item><description><see cref="ConstraintType.RoleNotNull"/>: The role is required and cannot be null.</description></item>
        ///     <item><description><see cref="ConstraintType.PasswordKeyNotNull"/>: The password key is required and cannot be null.</description></item>
        ///     <item><description><see cref="ConstraintType.EmailNotNull"/>: The email is required and cannot be null.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the update process.</exception>
        Task<ResponseUpdateUserByEmailDTO> UpdateUserByEmailAsync(RequestUpdateUserByEmailDTO data);

    }
}