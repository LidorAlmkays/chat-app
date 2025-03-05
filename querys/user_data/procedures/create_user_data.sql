-- Active: 1740481149570@@127.0.0.1@5432@my_chat_app
DROP PROCEDURE public.create_user_data;

CREATE OR REPLACE PROCEDURE PUBLIC.create_user_data(
    IN p_email TEXT,
    IN p_age INTEGER,
    IN p_username TEXT,
    IN p_password TEXT,
    IN p_password_key TEXT,
    IN p_role TEXT,
    OUT user_id INT
)
LANGUAGE plpgsql
AS $$
BEGIN
    user_id := public.function_create_user_data(p_email, p_age, p_username, p_password,p_password_key, p_role);
END;
$$;

DO $$ 
DECLARE result_user_id INT; 
BEGIN 
    CALL PUBLIC.create_user_data('johndoe2@example.com', 30, 'JohnDoe', 'hashedpassword', 'userSalt','user', result_user_id);
    RAISE NOTICE 'User ID: %', result_user_id;
END $$;