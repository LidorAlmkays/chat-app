CREATE OR REPLACE PROCEDURE public.create_user_data(
    IN p_email TEXT,
    IN p_age INTEGER,
    IN p_username TEXT,
    IN p_password TEXT,
    IN p_role TEXT,
    OUT user_id INT
)
LANGUAGE plpgsql
AS $$
BEGIN
    user_id := public.insert_user(p_email, p_age, p_username, p_password, p_role);
END;
$$;

DROP PROCEDURE create_user_data;
CALL create_user_data('johndoe2@example.com', 30, 'JohnDoe', 'hashedpassword', 'user');