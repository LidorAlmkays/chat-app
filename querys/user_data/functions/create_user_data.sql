-- Active: 1740481149570@@127.0.0.1@5432@my_chat_app
CREATE OR REPLACE FUNCTION PUBLIC.function_create_user_data(
    p_email TEXT,
    p_age INTEGER,
    p_username TEXT,
    p_password TEXT,
    p_password_key TEXT,
    p_role TEXT
 ) RETURNS UUID
LANGUAGE plpgsql
AS $$
 DECLARE
     new_user_id UUID;
     p_created_at TIMESTAMP;
 BEGIN
     INSERT INTO user_data (email, username, password, role, password_key, age,created_at)
     VALUES (p_email, p_username, p_password, p_role, p_password_key, p_age,p_created_at)
     RETURNING id INTO new_user_id;
    

     RETURN new_user_id;
END;
$$;
-- DROP FUNCTION PUBLIC.function_create_user_data;
