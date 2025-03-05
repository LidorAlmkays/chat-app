-- Active: 1740481149570@@127.0.0.1@5432@my_chat_app
CREATE OR REPLACE PROCEDURE PUBLIC.delete_user_by_email(
    INOUT p_email TEXT,
    OUT p_age INTEGER,
    OUT p_username TEXT,
    OUT p_role TEXT
)
LANGUAGE plpgsql
AS $$
BEGIN
    SELECT email, age, username, role
    INTO p_email, p_age, p_username, p_role
    FROM PUBLIC.function_delete_user_by_email(p_email);
END;
$$;

DROP PROCEDURE delete_user_by_email;
DO $$ 
DECLARE 
    p_email TEXT;
    p_age INTEGER;
    p_username TEXT;
    p_role TEXT; 
BEGIN 
    CALL PUBLIC.delete_user_by_email('johndoe2@example.com', 30, 'JohnDoe', 'hashedpassword', 'userSalt','user', result_user_id);
    RAISE NOTICE 'User ID: %', result_user_id;
END $$;