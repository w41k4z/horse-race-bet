CREATE DATABASE pmu OWNER walker;

CREATE TABLE bettor (
    id SERIAL PRIMARY KEY,
    name VARCHAR(25) NOT NULL,
    money DOUBLE PRECISION DEFAULT 0.
);

CREATE TABLE horse (
    id SERIAL PRIMARY KEY,
    name VARCHAR(25) NOT NULL,
    speed DOUBLE PRECISION DEFAULT 1.,
    endurance DOUBLE PRECISION DEFAULT 0.,
    endurance_activation DOUBLE PRECISION DEFAULT 50.,
    wins INTEGER DEFAULT 0,
    losses INTEGER DEFAULT 0,
    CHECK (speed > 0.),
    CHECK (endurance > -100. AND endurance < 100.),
    CHECK (endurance_activation > 0. AND endurance_activation < 100.),
    CHECK (wins >= 0),
    CHECK (losses >= 0)
);

CREATE TABLE bet (
    bet_date TIMESTAMP PRIMARY KEY,
    bettor1_id INTEGER REFERENCES bettor(id),
    bettor2_id INTEGER REFERENCES bettor(id),
    scale DOUBLE PRECISION DEFAULT 0.1,
    bet DOUBLE PRECISION DEFAULT 0.
    CHECK (scale > 0.),
    CHECK (bet >= 0.)
);
