-- Active: 1740481149570@@127.0.0.1@5432@my_chat_app
CREATE OR REPLACE FUNCTION PUBLIC.function_delete_user_by_email(p_email TEXT)
RETURNS user_data  -- Returning a single row
LANGUAGE plpgsql
AS $$
DECLARE 
    deleted_user user_data;  -- Variable to store deleted user info
BEGIN 
    -- Delete and store the deleted user data
    DELETE FROM user_data
    WHERE email = p_email
    RETURNING * INTO deleted_user;

    -- If no row was deleted, return NULL
    IF deleted_user IS NULL THEN
        RETURN NULL;
    END IF;

    RETURN deleted_user;
END;
$$;
DROP FUNCTION IF EXISTS PUBLIC.function_delete_user_by_email;
SELECT * FROM PUBLIC.function_delete_user_by_email('john121doe@example.com');
