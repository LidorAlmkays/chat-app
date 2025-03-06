-- Active: 1740481149570@@127.0.0.1@5432@my_chat_app
CREATE OR REPLACE PROCEDURE PUBLIC.delete_user_by_email(
    IN p_email TEXT,
    OUT user_info user_data_model
)
LANGUAGE plpgsql
AS $$
BEGIN
    SELECT * INTO user_info 
    FROM PUBLIC.function_delete_user_by_email(p_email);

    IF NOT FOUND THEN
        user_info := NULL;
    END IF;

    RAISE NOTICE 'Request, delete user by email: % ,Result: %', p_email, row_to_json(user_info);
END;
$$;

-- DROP PROCEDURE delete_user_by_email;
DO $$ 
DECLARE 
    user_info user_data_model;
BEGIN 
    CALL PUBLIC.delete_user_by_email('lidor1@gmail.com',user_info);
END;
$$;