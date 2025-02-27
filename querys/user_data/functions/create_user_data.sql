CREATE OR REPLACE FUNCTION public.insert_user(
    p_email TEXT,
    p_age INTEGER,
    p_username TEXT,
    p_password TEXT,
    p_role TEXT
) RETURNS INTEGER
LANGUAGE plpgsql
AS $$
DECLARE
    new_user_id INT;
BEGIN
    INSERT INTO user_data (email, username, password, role, age)
    VALUES (p_email, p_username, p_password, p_role, p_age)
    RETURNING id INTO new_user_id;

    RETURN new_user_id;
END;
$$;