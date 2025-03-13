-- Active: 1740481149570@@127.0.0.1@5432@my_chat_app
CREATE OR REPLACE PROCEDURE PUBLIC.create_user_data(
    OUT out_user_id UUID,
    IN p_email TEXT,
    IN p_age INTEGER,
    IN p_username TEXT,
    IN p_password TEXT,
    IN p_password_key TEXT,
    IN p_role TEXT DEFAULT 'User'
)
LANGUAGE plpgsql
AS $$
BEGIN
    out_user_id := public.function_create_user_data(p_email, p_age, p_username, p_password,p_password_key, p_role);
    RAISE NOTICE 'User added and received this id: %',out_user_id;
 END;
$$;
-- DROP PROCEDURE public.create_user_data;
DO $$ 
DECLARE
    p_email TEXT='johndoe@example.com';
    p_age INTEGER=30;
    p_username TEXT='JohnDoe';
    p_password TEXT= 'hashedpass1word';
    p_password_key TEXT='userSalt';
    p_role TEXT='user'; 
    result_user_id INTEGER;
    BEGIN 
        CALL PUBLIC.create_user_data(result_user_id,p_email, p_age,p_username ,p_password, p_password_key,p_role);
    END;
$$;
