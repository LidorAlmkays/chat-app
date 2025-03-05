-- Active: 1740481149570@@127.0.0.1@5432@my_chat_app
DROP FUNCTION IF EXISTS PUBLIC.function_delete_user_by_email(TEXT);
CREATE OR REPLACE FUNCTION PUBLIC.function_delete_user_by_email(p_email TEXT)
RETURNS TABLE(    
    email TEXT,
    age INTEGER,
    username TEXT,
    role TEXT
) 
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    DELETE FROM user_data u
    WHERE u.email = p_email
    RETURNING u.email, u.age, u.username, u.role;
END;
$$;
