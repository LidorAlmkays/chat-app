-- Active: 1740481149570@@127.0.0.1@5432@my_chat_app
CREATE OR REPLACE FUNCTION PUBLIC.function_create_user_data(
    user_info user_data_model
) RETURNS user_data_model
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO user_data (email, username, password, role, password_key, age)
    VALUES (user_info.email, user_info.username, user_info.password, user_info.role, user_info.password_key, user_info.age)
    RETURNING email, age, username, password, role, password_key, created_at INTO user_info;
    RETURN user_info;
END;
$$;
-- DROP FUNCTION PUBLIC.function_create_user_data;
