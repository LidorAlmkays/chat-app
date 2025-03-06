-- Active: 1740481149570@@127.0.0.1@5432@my_chat_app
CREATE OR REPLACE FUNCTION PUBLIC.function_delete_user_by_email(p_email TEXT)
RETURNS user_data_model
LANGUAGE plpgsql
AS $$
DECLARE
    deleted_user user_data_model;
BEGIN 
    DELETE FROM user_data u
    WHERE u.email = p_email
    RETURNING u.email, u.age, u.username, u.password, u.role, u.password_key, u.created_at 
    INTO deleted_user;

    IF NOT FOUND THEN
        RETURN NULL; -- return null if no user was found
    END IF;

    RETURN deleted_user;

END;
$$;
-- DROP FUNCTION IF EXISTS PUBLIC.function_delete_user_by_email;
