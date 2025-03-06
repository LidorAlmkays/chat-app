CREATE TYPE public.user_data_model AS (
    email TEXT,
    age INTEGER,
    username TEXT,
    password TEXT,
    role TEXT,
    password_key TEXT,
    created_at TIMESTAMP
);
DROP TYPE IF EXISTS public.user_data_model CASCADE;