CREATE OR REPLACE FUNCTION PUBLIC.function_get_user_by_email(in_email TEXT)
RETURNS SETOF user_data
LANGUAGE plpgsql
AS $$
BEGIN
    RAISE NOTICE 'Trying to get user with email: %', in_email;
    RETURN QUERY 
    SELECT * 
    FROM user_data  
    WHERE email = in_email
    LIMIT 1;
END;
$$;
DROP FUNCTION PUBLIC.function_get_user_by_email;

SELECT * FROM PUBLIC.function_get_user_by_email('john121doe@example.com');
