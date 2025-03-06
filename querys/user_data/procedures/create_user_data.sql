-- Active: 1740481149570@@127.0.0.1@5432@my_chat_app
CREATE OR REPLACE PROCEDURE PUBLIC.create_user_data(
    INOUT user_info user_data_model
)
LANGUAGE plpgsql
AS $$
BEGIN
    user_info.role := COALESCE(user_info.role, 'user');
    user_info.created_at := COALESCE(current_timestamp);

    SELECT public.function_create_user_data(user_info) INTO user_info;
    RAISE NOTICE 'User Added with email: %', user_info.email;
 END;
$$;
-- DROP PROCEDURE public.create_user_data;
DO $$ 
DECLARE
    user_info user_data_model; 
BEGIN
    user_info.email := 'lidor12@gmail.com';  
    user_info.age := 30;
    user_info.username := 'lidor';
    user_info.password := '12345678';
    user_info.role := 'admin';
    user_info.password_key := 'passwordKey';

    CALL PUBLIC.create_user_data(user_info);
END $$;