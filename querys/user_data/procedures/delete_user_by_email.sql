-- Active: 1740481149570@@127.0.0.1@5432@my_chat_app
CREATE OR REPLACE PROCEDURE PUBLIC.delete_user_by_email(
    IN in_email TEXT,
    OUT out_is_deleted BOOLEAN
)
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM user_data
    WHERE id = (SELECT id FROM user_data WHERE email = in_email ORDER BY id LIMIT 1);
    out_is_deleted:=FOUND;
    IF FOUND THEN
        RAISE NOTICE 'User was deleted.';
    ELSE
        RAISE NOTICE 'No user was deleted.';
    END IF;
END;
$$;

DROP PROCEDURE delete_user_by_email;

CALL PUBLIC.delete_user_by_email('joh1na121doe@example.com');
