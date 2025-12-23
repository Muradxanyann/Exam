CREATE TABLE customer(

                         id SERIAL PRIMARY KEY,
                         name VARCHAR(100) NOT NULL,
                         email VARCHAR(255) NOT NULL UNIQUE,
                         phone_number VARCHAR(64) NOT NULL,
                         password_hash VARCHAR(100) NOT NULL,
                         password_salt VARCHAR(128) NOT NULL,
                         balance decimal not null check (balance >= 0),
                         is_active bool not null default true,
                         created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
                         updated_at TIMESTAMPTZ
)
--

CREATE TABLE operation_type(
                               id SERIAL PRIMARY KEY,
                               name VARCHAR (100) NOT NULL
)
--

CREATE TABLE operation_status(
                                 id SERIAL PRIMARY KEY,
                                 name VARCHAR (100) NOT NULL
)
--

CREATE TABLE history(
                        id SERIAL PRIMARY KEY,
                        operation_date TIMESTAMPTZ NOT NULL DEFAULT NOW(),
                        amount decimal not null check (amount >= 0),
                        operation_type_id int not null references operation_type(id),
                        operation_status_id int not null references operation_status(id),
                        sender_customer_id int references customer(id),
                        receiver_customer_id int references customer(id)
)

--
insert into operation_status(name)
values ('Completed'), ('Failed')

--
insert into operation_type(name)
values ('Deposit'), ('Withdraw'), ('Transfer')




