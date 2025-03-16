-- Active: 1740481149570@@127.0.0.1@5432@my_chat_app
CREATE OR REPLACE PROCEDURE PUBLIC.delete_user_by_email(
    OUT out_birthday DATE,
    OUT out_created_at DATE,
    OUT out_password_key TEXT,
    OUT out_password TEXT,
    OUT out_username TEXT,
    OUT out_role TEXT,
    OUT out_email TEXT,
    OUT out_user_id UUID,
    IN in_email TEXT
)
LANGUAGE plpgsql
AS $$
BEGIN
    Select * FROM public.function_delete_user_by_email(in_email)
    INTO out_email, out_birthday, out_username, out_role,out_user_id,out_created_at,out_password,out_password_key;

    IF out_email IS NULL THEN
        RAISE NOTICE 'Failed delete user with email %, user was not found.', in_email;
        out_user_id:=NULL;
    ELSE
        RAISE NOTICE 'Delete User successfully, Email: %, Birthday: %, Username: %, Role: %', 
                     out_email, out_birthday, out_username, out_role;
    END IF;
END;
$$;

DROP PROCEDURE delete_user_by_email;
DO $$ 
DECLARE 
    p_email TEXT='lido2r1@gmail.com';
    p_birthday DATE;
    p_username TEXT;
    p_role TEXT; 
    result_user_id UUID;
BEGIN 
    CALL PUBLIC.delete_user_by_email( p_birthday, p_username,p_role,p_email, result_user_id,p_email);
    RAISE NOTICE 'Id of deleted user: %.', result_user_id;

END;
$$;