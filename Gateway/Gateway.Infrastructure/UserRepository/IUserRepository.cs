using Gateway.Domain.models;

namespace Gateway.Infrastructure.UserRepository
{
    public interface IUserRepository
    {
        /// <summary>
        /// Attempts to update a user's information in the database using a stored procedure, identified by their email.
        /// </summary>
        /// <param name="email">The existing email of the user whose data is being updated.</param>
        /// <param name="newData">
        /// The updated user model containing new values for fields such as email, username, birthday, role, password, and password key.
        /// </param>
        /// <returns>A <see cref="Task"/> representing the asynchronous update operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the provided <paramref name="newData"/> is null.</exception>
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
        Task<Guid> InsertUserAsync(UserModel user);
        /// <summary>
        /// Asynchronously deletes a user from the database by their email address.
        /// </summary>
        /// <param name="userEmail">The email of the user to be deleted.</param>
        /// <returns>
        /// A task that resolves to a <see cref="UserModel"/> containing the deleted user's details if the deletion is successful, 
        /// or throws an exception if the deletion fails.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the provided email is null or empty.
        /// </exception>
        /// <exception cref="UserNotFoundException">
        /// Thrown when no user is found with the specified email during the deletion attempt.
        /// </exception>
        /// <exception cref="ConnectionException">
        /// Thrown when there is a failure connecting to the database.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown for any unexpected errors that occur during the deletion process.
        /// </exception>
        Task<UserModel> DeleteUserByEmailAsync(string userEmail);
        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>
        /// A <see cref="UserModel"/> representing the user if found; otherwise, throws a <see cref="UserNotFoundException"/>.
        /// </returns>
        /// <exception cref="UserNotFoundException">Thrown if no user is found with the specified email.</exception>
        /// <exception cref="ConnectionException">Thrown if there is an error establishing a database connection.</exception>
        /// <exception cref="Exception">Thrown if an unexpected error occurs during execution.</exception>
        Task<UserModel> GetUserByEmailAsync(string email);

        /// <summary>
        /// Attempts to update a user's information in the database using a stored procedure, identified by their email.
        /// </summary>
        /// <param name="email">The existing email of the user whose data is being updated.</param>
        /// <param name="newData">
        /// The updated user model containing new values for fields such as email, username, birthday, role, password, and password key.
        /// </param>
        /// <returns>A <see cref="Task"/> representing the asynchronous update operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the provided <paramref name="newData"/> is null.</exception>
        /// <exception cref="ConnectionException">
        /// Thrown if there is a failure in establishing a connection to the database.
        /// </exception>
        /// <exception cref="ConstraintViolationException">
        /// Thrown when a constraint violation occurs during the update process. The specific violation is indicated by 
        /// the <see cref="ConstraintViolationException.ConstraintType"/> property. Possible violations include:
        /// <list type="bullet">
        ///     <item><description><see cref="ConstraintType.ValidEmail"/>: The provided email format is invalid.</description></item>
        ///     <item><description><see cref="ConstraintType.CheckBirthday"/>: The user's age does not meet the required criteria.</description></item>
        ///     <item><description><see cref="ConstraintType.ValidRole"/>: The provided role is invalid.</description></item>
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
        Task UpdateByEmailAsync(string email, UpdateUserModel newData);
    }
}