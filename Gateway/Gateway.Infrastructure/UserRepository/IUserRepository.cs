using Gateway.Domain.models;

namespace Gateway.Infrastructure.UserRepository
{
    public interface IUserRepository
    {
        /// <summary>
        /// Attempts to insert a new user into the database, using stored procedure to handle the insertion process.
        /// </summary>
        /// <param name="user">The user model containing the user's details, including username, role, age, password, and email.</param>
        /// <returns>A <see cref="Guid"/> representing the unique identifier of the inserted user.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the provided user model is null.</exception>
        /// <exception cref="ConnectionException">
        /// Thrown if there is a failure in establishing a connection to the database.
        /// </exception>
        /// <exception cref="ConstraintViolationException">
        /// Thrown when a constraint violation occurs during the user insertion process. The specific constraint violation is 
        /// indicated by the <see cref="ConstraintViolationException.ConstraintType"/> property. Possible violations include:
        /// <list type="bullet">
        ///     <item><description><see cref="ConstraintType.ValidEmail"/>: Invalid email format.</description></item>
        ///     <item><description><see cref="ConstraintType.CheckAge"/>: The user's age does not meet the required criteria (e.g., less than 18).</description></item>
        ///     <item><description><see cref="ConstraintType.UsernameNotNull"/>: The username cannot be null.</description></item>
        ///     <item><description><see cref="ConstraintType.ValidRole"/>: The provided role is invalid.</description></item>
        ///     <item><description><see cref="ConstraintType.UsernameNotEmpty"/>: The username cannot be empty.</description></item>
        ///     <item><description><see cref="ConstraintType.CheckPasswordLength"/>: The password length is shorter than the required minimum.</description></item>
        ///     <item><description><see cref="ConstraintType.EmailAlreadyExists"/>: The email already exists in the system.</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the insertion process.</exception>
        Task<Guid> InsertUser(UserModel user);
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
        Task<UserModel> DeleteUserByEmail(string userEmail);

    }
}