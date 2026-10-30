CREATE TABLE IF NOT EXISTS "Users" (
    "Username" text PRIMARY KEY,
    "Email" text NOT NULL,
    "Password" text NOT NULL,
    "Role" integer NOT NULL
);
