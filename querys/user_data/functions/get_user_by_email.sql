CREATE OR REPLACE FUNCTION PUBLIC.function_get_user_by_email(p_email TEXT)
RETURNS user_data_model
LANGUAGE plpgsql
AS $$
DECLARE 
    user_record user_data_model;
BEGIN
    SELECT u.username ,u.age ,u.email, u.password, u.password_key, u.role, u.created_at 
    INTO user_record
    FROM user_data u  
    WHERE u.email = p_email;

    IF NOT FOUND THEN
        RETURN NULL;
    END IF;

    RETURN user_record;
END;
$$;
SELECT * FROM function_get_user_by_email('johndoe2@example.com');
