CREATE OR REPLACE PROCEDURE update_user_data_by_email(
    IN p_email TEXT,
    IN p_new_email TEXT DEFAULT NULL,
    IN p_username TEXT DEFAULT NULL,
    IN p_birthday DATE DEFAULT NULL,
    IN p_password TEXT DEFAULT NULL,
    IN p_role TEXT DEFAULT NULL,
    IN p_password_key TEXT DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
BEGIN
    UPDATE user_data
    SET 
        email = COALESCE(p_new_email, email),
        username = COALESCE(p_username, username),
        birthday = COALESCE(p_birthday, birthday),
        password = COALESCE(p_password, password),
        role = COALESCE(p_role, role),
        password_key = COALESCE(p_password_key, password_key),
        last_updated_at = CURRENT_TIMESTAMP
    WHERE email = p_email;
    IF NOT FOUND THEN
        RAISE EXCEPTION 'No user found or no changes made for email: %', p_email;
    END IF;
END;
$$;


CALL update_user_data_by_email(
    'new_email@example.com',
    -- 'new_email@example.com',
    NULL
    ,
    'new_username',
    -- '1990-01-01',
    NULL,
    'new_password',
    'Admin',
    'new_password_key'
);