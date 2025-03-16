-- Active: 1740481149570@@127.0.0.1@5432@my_chat_app
CREATE OR REPLACE FUNCTION PUBLIC.function_delete_user_by_email(p_email TEXT)
 RETURNS TABLE(    
     email TEXT,
     birthday DATE,
     username TEXT,
     role TEXT,
     id UUID,
     created_at DATE,
     password TEXT,
     password_key TEXT
 ) 
LANGUAGE plpgsql
AS $$
BEGIN 
    DELETE FROM user_data u
    WHERE u.email = p_email
    RETURNING u.email, u.birthday, u.username, u.role, u.id,u.created_at,u.password,u.password_key INTO email, birthday, username, role, id, created_at,password,password_key ;

    -- If no row was deleted, return nothing (empty result set)
    IF email IS NULL THEN
        RETURN;
    END IF;
    RETURN NEXT;
END;
$$;
DROP FUNCTION IF EXISTS PUBLIC.function_delete_user_by_email;
