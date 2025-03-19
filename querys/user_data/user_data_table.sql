-- Active: 1740481149570@@127.0.0.1@5432@my_chat_app
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE user_data(  
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    username TEXT,
    birthday DATE,
    email TEXT,
    password TEXT,
    role TEXT,
    password_key TEXT,
    last_updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);
ALTER TABLE user_data
ADD CONSTRAINT valid_email CHECK (email ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$'),
ADD CONSTRAINT check_birthday CHECK (birthday <= CURRENT_DATE - INTERVAL '18 years')
ADD CONSTRAINT valid_role CHECK (role IN ('Guest', 'User', 'Admin')),
ADD CONSTRAINT username_not_empty CHECK (username != ''),
ADD CONSTRAINT check_password_length CHECK (LENGTH(password) >= 8),
ADD CONSTRAINT unique_email UNIQUE (email),
ALTER COLUMN birthday SET NOT NULL,
ALTER COLUMN username SET NOT NULL,
ALTER COLUMN password SET NOT NULL,
ALTER COLUMN role SET NOT NULL,
ALTER COLUMN password_key SET NOT NULL,
ALTER COLUMN email SET NOT NULL;

-- ALTER TABLE user_data
-- DROP CONSTRAINT valid_role;


-- COMMENT ON TABLE user_data IS '';
-- COMMENT ON COLUMN user_data.name IS '';

-- ALTER TABLE user_data
-- ADD COLUMN last_updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP;
