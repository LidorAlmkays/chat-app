-- Active: 1740481149570@@127.0.0.1@5432@my_chat_app
CREATE OR REPLACE PROCEDURE PUBLIC.delete_user_by_email(
    OUT out_age INTEGER,
    OUT out_username TEXT,
    OUT out_role TEXT,
    OUT out_email TEXT,
    OUT out_user_id INTEGER,
    IN in_email TEXT
)
LANGUAGE plpgsql
AS $$
BEGIN
    Select * FROM public.function_delete_user_by_email(in_email)
    INTO out_email, out_age, out_username, out_role,out_user_id;

    IF out_email IS NULL THEN
        RAISE EXCEPTION 'No user found with email: %', in_email;
    ELSE
        RAISE NOTICE 'Deleted User - Email: %, Age: %, Username: %, Role: %', 
                     out_email, out_age, out_username, out_role;
    END IF;
END;
$$;

-- DROP PROCEDURE delete_user_by_email;
DO $$ 
DECLARE 
    p_email TEXT='johndoe@example.com';
    p_age INTEGER;
    p_username TEXT;
    p_role TEXT; 
    result_user_id INTEGER;
BEGIN 
    CALL PUBLIC.delete_user_by_email( p_age, p_username,p_role,p_email, result_user_id,p_email);
END;
$$;