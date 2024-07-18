CREATE TABLE "dmc_thread"
(
    id SERIAL PRIMARY KEY,
    name VARCHAR(120) NOT NULL,
    floss VARCHAR(20) NOT NULL,
    web_color CHAR(7) NOT NULL DEFAULT '#FFFFFF',
    created_At TIMESTAMP,
    updated_at TIMESTAMP
);

CREATE INDEX idx_dmc_thread_name
    ON dmc_thread (name);

CREATE INDEX idx_dmc_thread_floss
    ON dmc_thread (floss);


CREATE TABLE user_thread
(
    user_id VARCHAR(50) NOT NULL,
    thread_id INT NOT NULL,
    quantity INT NOT NULL DEFAULT 0,
    created_at TIMESTAMP NOT NULL,
    updated_at TIMESTAMP NOT NULL,
    
    PRIMARY KEY (user_id, thread_id)
);

CREATE INDEX  idx_user_thread_user_id
    ON user_thread (user_id);

CREATE INDEX  idx_user_thread_thread_id
    ON user_thread (thread_id);