using System.Data.Common;
using System.Text.RegularExpressions;

namespace Gateway.Domain.Exceptions.database
{
    public static class SpecificConstraintExceptionFactory
    {
        private static readonly Dictionary<ConstraintType, string> errorMessages = new()
        {
            { ConstraintType.ValidEmail, "Invalid email format. Please provide a valid email address." },
            { ConstraintType.CheckAge, "Age must be 18 or older." },
            { ConstraintType.ValidRole, "Invalid role. Allowed roles: Guest, User, Admin." },
            { ConstraintType.UsernameNotEmpty, "Username cannot be empty." },
            { ConstraintType.UsernameNotNull, "Username is required and cannot be null." },
            { ConstraintType.CheckPasswordLength, "Password must be at least 8 characters long." },
            { ConstraintType.UniqueEmail, "This email is already in use. Please use a different email." },
            { ConstraintType.PasswordNotNull, "Password is required and cannot be null." },
            { ConstraintType.RoleNotNull, "Role is required and cannot be null." },
            { ConstraintType.PasswordKeyNotNull, "Password key is required and cannot be null." },
            { ConstraintType.EmailNotNull, "Email is required and cannot be null." }
        };

        public static Exception CreateException(DbException ex)
        {
            ArgumentNullException.ThrowIfNull(ex);
            string constraintName = ExtractConstraintName(ex.Message);

            var constraintType = MapConstraintNameToEnum(constraintName);

            if (constraintType.HasValue && errorMessages.TryGetValue(constraintType.Value, out string? errorMessage))
            {
                return new ConstraintViolationException(constraintType.Value, errorMessage);
            }

            return new Exception($"Unknown constraint violation: {constraintName}");
        }

        private static ConstraintType? MapConstraintNameToEnum(string constraintName)
        {
            var mapping = new Dictionary<string, ConstraintType>(StringComparer.OrdinalIgnoreCase)
    {
        { "valid_email", ConstraintType.ValidEmail },
        { "check_age", ConstraintType.CheckAge },
        { "valid_role", ConstraintType.ValidRole },
        { "username_not_empty", ConstraintType.UsernameNotEmpty },
        { "check_password_length", ConstraintType.CheckPasswordLength },
        { "unique_email", ConstraintType.UniqueEmail },
        { "username", ConstraintType.UsernameNotNull },
        { "password", ConstraintType.PasswordNotNull },
        { "role", ConstraintType.RoleNotNull },
        { "password_key", ConstraintType.PasswordKeyNotNull },
        { "email", ConstraintType.EmailNotNull }
    };

            return mapping.TryGetValue(constraintName, out var constraintType) ? constraintType : null;
        }
        private static string ExtractConstraintName(string errorMessage)
        {
            Regex regex = new(@"""([^""]+)""");
            var matches = regex.Matches(errorMessage);
            return matches.Count > 0 ? matches[^1].Groups[1].Value : "didn't find constraint name, error message: " + errorMessage;
        }

    }
}