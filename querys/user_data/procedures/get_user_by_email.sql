CREATE OR REPLACE PROCEDURE PUBLIC.get_user_by_email(
    IN p_email TEXT,
    OUT user_info user_data_model
)
LANGUAGE plpgsql
AS $$
BEGIN
    user_info := PUBLIC.function_get_user_by_email(p_email);
    RAISE NOTICE 'Request, get user by email: % ,Result: %', p_email,row_to_json(user_info);
END;
$$;
-- DROP PROCEDURE get_user_by_email;

DO $$
DECLARE
    p_email Text;
    user_info user_data_model;
BEGIN
    CALL public.get_user_by_email('lidor12@gmail.com', user_info);
END;
$$;