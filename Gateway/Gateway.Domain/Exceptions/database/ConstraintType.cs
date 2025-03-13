namespace Gateway.Domain.Exceptions.database
{
    public enum ConstraintType
    {

        ValidEmail,            // Matches: valid_email
        CheckAge,              // Matches: check_age
        ValidRole,             // Matches: valid_role
        UsernameNotEmpty,      // Matches: username_not_empty
        CheckPasswordLength,   // Matches: check_password_length
        UniqueEmail,           // Matches: unique_email
        UsernameNotNull,       // Matches: ALTER COLUMN username SET NOT NULL
        PasswordNotNull,       // Matches: ALTER COLUMN password SET NOT NULL
        RoleNotNull,           // Matches: ALTER COLUMN role SET NOT NULL
        PasswordKeyNotNull,    // Matches: ALTER COLUMN password_key SET NOT NULL
        EmailNotNull           // Matches: ALTER COLUMN email SET NOT NULL
    }
}