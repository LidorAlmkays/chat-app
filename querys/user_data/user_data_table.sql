-- Active: 1740481149570@@127.0.0.1@5432@my_chat_app
CREATE TABLE user_data(  
    id int NOT NULL PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    username TEXT NOT NULL,
    age INTEGER,
    email TEXT NOT NULL UNIQUE,
    password TEXT NOT NULL,
    role TEXT NOT NULL,
    password_key TEXT NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);
COMMENT ON TABLE user_data IS '';
COMMENT ON COLUMN user_data.name IS '';