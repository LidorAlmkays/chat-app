-- Active: 1740481149570@@127.0.0.1@5432@my_chat_app
DROP FUNCTION PUBLIC.function_create_user_data;

CREATE OR REPLACE FUNCTION PUBLIC.function_create_user_data(
    p_email TEXT,
    p_age INTEGER,
    p_username TEXT,
    p_password TEXT,
    p_password_key TEXT,
    p_role TEXT DEFAULT 'user'
) RETURNS INTEGER
LANGUAGE plpgsql
AS $$
DECLARE
    new_user_id INT;
BEGIN
    INSERT INTO user_data (email, username, password, role, password_key, age)
    VALUES (p_email, p_username, p_password, p_role, p_password_key, p_age)
    RETURNING id INTO new_user_id;

    RETURN new_user_id;
END;
$$;