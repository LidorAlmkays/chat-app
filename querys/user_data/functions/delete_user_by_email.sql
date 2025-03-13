-- Active: 1740481149570@@127.0.0.1@5432@my_chat_app
CREATE OR REPLACE FUNCTION PUBLIC.function_delete_user_by_email(p_email TEXT)
 RETURNS TABLE(    
     email TEXT,
     age INTEGER,
     username TEXT,
     role TEXT,
     id INTEGER
 ) 
LANGUAGE plpgsql
AS $$
BEGIN 
    DELETE FROM user_data u
    WHERE u.email = p_email
    RETURNING u.email, u.age, u.username, u.role, u.id INTO email, age, username, role, id;

    -- If no row was deleted, return nothing (empty result set)
    IF email IS NULL THEN
        RETURN;
    END IF;
    RETURN NEXT;
END;
$$;
-- DROP FUNCTION IF EXISTS PUBLIC.function_delete_user_by_email;
