CREATE DATABASE Tesis
GO

USE Tesis
GO

CREATE TABLE specialties
(
	id int identity (1,1) NOT NULL,
	name varchar(100) NOT NULL

	CONSTRAINT pk_specialty_id PRIMARY KEY (id)
)
GO
CREATE TABLE areas
(
	id int identity (1,1) NOT NULL,
	name varchar(100) NOT NULL

	CONSTRAINT pk_area_id PRIMARY KEY (id)
)
GO
CREATE TABLE positions
(
	id int identity (1,1) NOT NULL,
	areas_id int NOT NULL,
	name varchar(100) NOT NULL,

	CONSTRAINT pk_position_id PRIMARY KEY (id),

	CONSTRAINT fk_positions_areas_id FOREIGN KEY (areas_id)
	REFERENCES areas(id)
)
GO
CREATE TABLE employees
(
	id char(8) NOT NULL,
	specialties_id int NOT NULL,
	date_entry date NOT NULL,
	type_document varchar(20) NOT NULL,
	firstname varchar(50) NOT NULL,
	lastname varchar(50) NOT NULL,
	birthdate date NOT NULL,
	nationality varchar(50) NOT NULL,
	genre varchar(20) NOT NULL,
	phone int NOT NULL,
	email varchar(100) NOT NULL,
	address varchar(100) NOT NULL,
	zone_access varchar(500) NOT NULL

	CONSTRAINT pk_employee_id PRIMARY KEY (id),

	CONSTRAINT fk_employees_specialties_id FOREIGN KEY (specialties_id)
	REFERENCES specialties(id)
)
GO
CREATE TABLE employees_credentials
(
	employees_id char(8) NOT NULL,
	code varchar(50) NOT NULL

	CONSTRAINT pk_employee_credential_employees_id PRIMARY KEY (employees_id)

	CONSTRAINT fk_employees_credentials_employees_id FOREIGN KEY (employees_id)
	REFERENCES employees(id)
)
GO
CREATE TABLE admins
(
	id char(8) NOT NULL,
	specialties_id int NOT NULL,
	date_entry date NOT NULL,
	type_document varchar(20) NOT NULL,
	firstname varchar(50) NOT NULL,
	lastname varchar(50) NOT NULL,
	birthdate date NOT NULL,
	nationality varchar(50) NOT NULL,
	genre varchar(20) NOT NULL,
	phone int NOT NULL,
	email varchar(100) NOT NULL,
	address varchar(100) NOT NULL,

	CONSTRAINT pk_admin_id PRIMARY KEY (id),

	CONSTRAINT fk_admins_specialties_id FOREIGN KEY (specialties_id)
	REFERENCES specialties(id)
)
GO
CREATE TABLE admins_credentials
(
	admins_id char(8) NOT NULL,
	code varchar(50) NOT NULL

	CONSTRAINT pk_admin_credential_employees_id PRIMARY KEY (admins_id)

	CONSTRAINT fk_admins_credentials_employees_id FOREIGN KEY (admins_id)
	REFERENCES admins(id)
)
GO
CREATE TABLE assigns
(
	id int identity (1,1) NOT NULL,
	admins_id char(8) NULL,
	employees_id char(8) NULL,
	positions_id int NOT NULL

	CONSTRAINT pk_assign_id PRIMARY KEY (id)

	CONSTRAINT fk_assigns_admins_id FOREIGN KEY (admins_id)
	REFERENCES admins(id),
	
	CONSTRAINT fk_assigns_employees_id FOREIGN KEY (employees_id)
	REFERENCES employees(id),

	CONSTRAINT fk_assigns_positions_id FOREIGN KEY (positions_id)
	REFERENCES positions(id)
)
GO
CREATE TABLE assists
(
	id int identity (1,1) NOT NULL,
	admins_id char(8) NULL,
	employees_id char(8) NULL,
	check_in datetime NULL,
	check_out datetime NULL,
	minute_late int NULL,
	emotional_state varchar(20) NOT NULL

	CONSTRAINT pk_assist_id PRIMARY KEY (id)

	CONSTRAINT fk_assists_admins_id FOREIGN KEY (admins_id)
	REFERENCES admins(id),

	CONSTRAINT fk_assists_employees_id FOREIGN KEY (employees_id)
	REFERENCES employees(id)
)
GO

INSERT INTO specialties (name) VALUES ('Cardiología');
INSERT INTO specialties (name) VALUES ('Neurología');
INSERT INTO specialties (name) VALUES ('Pediatría');
INSERT INTO specialties (name) VALUES ('Oncología');
INSERT INTO specialties (name) VALUES ('Dermatología');

INSERT INTO areas (name) VALUES ('Medicina');
INSERT INTO areas (name) VALUES ('Enfermería');
INSERT INTO areas (name) VALUES ('Laboratorio');
INSERT INTO areas (name) VALUES ('Administración');
INSERT INTO areas (name) VALUES ('Radiología');

INSERT INTO positions (areas_id, name) VALUES (1, 'Doctor');
INSERT INTO positions (areas_id, name) VALUES (2, 'Enfermero');
INSERT INTO positions (areas_id, name) VALUES (3, 'Técnico de laboratorio');
INSERT INTO positions (areas_id, name) VALUES (4, 'Recepcionista');
INSERT INTO positions (areas_id, name) VALUES (5, 'Técnico en rayos X');

INSERT INTO employees (id, specialties_id, date_entry, type_document, firstname, lastname, birthdate, nationality, genre, phone, email, address, zone_access) 
VALUES ('45515113', 1, '2022-01-15', 'DNI', 'Juan', 'Pérez', '1985-06-24', 'Peru', 'Masculino', 901256232, 'juan.perez@example.com', 'Calle Falsa 123', 'Zona A');

INSERT INTO employees (id, specialties_id, date_entry, type_document, firstname, lastname, birthdate, nationality, genre, phone, email, address, zone_access) 
VALUES ('65515453', 2, '2021-05-10', 'DNI', 'María', 'Gómez', '1990-12-12', 'Peru', 'Femenino', 941453532, 'maria.gomez@example.com', 'Avenida Real 456', 'Zona B');

INSERT INTO employees (id, specialties_id, date_entry, type_document, firstname, lastname, birthdate, nationality, genre, phone, email, address, zone_access) 
VALUES ('16389451', 3, '2020-03-20', 'DNI', 'Carlos', 'López', '1978-03-05', 'Peru', 'Masculino', 941256342, 'carlos.lopez@example.com', 'Paseo Central 789', 'Zona C');

INSERT INTO employees (id, specialties_id, date_entry, type_document, firstname, lastname, birthdate, nationality, genre, phone, email, address, zone_access) 
VALUES ('39451515', 4, '2019-08-25', 'DNI', 'Ana', 'Martínez', '1982-11-19', 'Peru', 'Femenino', 941366532, 'ana.martinez@example.com', 'Boulevard Norte 101', 'Zona D');

INSERT INTO employees (id, specialties_id, date_entry, type_document, firstname, lastname, birthdate, nationality, genre, phone, email, address, zone_access) 
VALUES ('10548122', 5, '2023-04-30', 'DNI', 'Laura', 'Fernández', '1995-07-23', 'Peru', 'Femenino', 931246532, 'laura.fernandez@example.com', 'Camino Sur 202', 'Zona E');

INSERT INTO employees_credentials (employees_id, code) VALUES ('45515113', 'dev');
INSERT INTO employees_credentials (employees_id, code) VALUES ('65515453', 'dev');
INSERT INTO employees_credentials (employees_id, code) VALUES ('16389451', 'dev');
INSERT INTO employees_credentials (employees_id, code) VALUES ('39451515', 'dev');
INSERT INTO employees_credentials (employees_id, code) VALUES ('10548122', 'dev');

INSERT INTO admins (id, specialties_id, date_entry, type_document, firstname, lastname, birthdate, nationality, genre, phone, email, address) 
VALUES ('32112512', 1, '2022-02-10', 'DNI', 'Luis', 'Ramírez', '1987-01-15', 'Perú', 'Masculino', 900000001, 'luis.ramirez@example.com', 'Av. Los Olivos 123');

INSERT INTO admins (id, specialties_id, date_entry, type_document, firstname, lastname, birthdate, nationality, genre, phone, email, address) 
VALUES ('36451511', 2, '2021-06-15', 'DNI', 'Carmen', 'Soto', '1985-05-23', 'Perú', 'Femenino', 900000002, 'carmen.soto@example.com', 'Calle Lima 456');

INSERT INTO admins (id, specialties_id, date_entry, type_document, firstname, lastname, birthdate, nationality, genre, phone, email, address) 
VALUES ('02451544', 3, '2020-09-01', 'DNI', 'José', 'Mendoza', '1990-09-10', 'Perú', 'Masculino', 900000003, 'jose.mendoza@example.com', 'Paseo de la Republica 789');

INSERT INTO admins (id, specialties_id, date_entry, type_document, firstname, lastname, birthdate, nationality, genre, phone, email, address) 
VALUES ('39450051', 4, '2019-11-25', 'DNI', 'Lucía', 'García', '1989-03-12', 'Perú', 'Femenino', 900000004, 'lucia.garcia@example.com', 'Jr. Primavera 101');

INSERT INTO admins (id, specialties_id, date_entry, type_document, firstname, lastname, birthdate, nationality, genre, phone, email, address) 
VALUES ('05481214', 5, '2023-04-14', 'DNI', 'Ricardo', 'Torres', '1992-07-20', 'Perú', 'Masculino', 900000005, 'ricardo.torres@example.com', 'Av. Independencia 202');

INSERT INTO admins_credentials (admins_id, code) VALUES ('32112512', 'dev');
INSERT INTO admins_credentials (admins_id, code) VALUES ('36451511', 'dev');
INSERT INTO admins_credentials (admins_id, code) VALUES ('02451544', 'dev');
INSERT INTO admins_credentials (admins_id, code) VALUES ('39450051', 'dev');
INSERT INTO admins_credentials (admins_id, code) VALUES ('05481214', 'dev');

INSERT INTO assigns (admins_id, employees_id, positions_id)
VALUES 
('32112512', NULL, 1),
('36451511', NULL, 2),
('02451544', NULL, 3),
('39450051', NULL, 4),
('05481214', NULL, 5);

INSERT INTO assigns (admins_id, employees_id, positions_id)
VALUES 
(NULL, '45515113', 1),
(NULL, '65515453', 2),
(NULL, '16389451', 3),
(NULL, '39451515', 4),
(NULL, '10548122', 5);

INSERT INTO assists (admins_id, employees_id, check_in, check_out, minute_late, emotional_state) 
VALUES ('32112512', NULL, '2024-11-01 08:05:00', '2024-11-01 17:00:00', 5, 'Contento');

INSERT INTO assists (admins_id, employees_id, check_in, check_out, minute_late, emotional_state) 
VALUES (NULL, '16389451', '2024-11-02 08:00:00', '2024-11-02 17:05:00', 0, 'Motivado');

INSERT INTO assists (admins_id, employees_id, check_in, check_out, minute_late, emotional_state) 
VALUES ('36451511', NULL, '2024-11-03 08:10:00', '2024-11-03 17:00:00', 10, 'Relajado');

INSERT INTO assists (admins_id, employees_id, check_in, check_out, minute_late, emotional_state) 
VALUES (NULL, '45515113', '2024-11-04 08:15:00', '2024-11-04 17:10:00', 15, 'Cansado');

INSERT INTO assists (admins_id, employees_id, check_in, check_out, minute_late, emotional_state) 
VALUES ('02451544', NULL, '2024-11-05 07:55:00', '2024-11-05 16:50:00', 0, 'Feliz');

INSERT INTO assists (admins_id, employees_id, check_in, check_out, minute_late, emotional_state) 
VALUES (NULL, '10548122', '2024-11-06 08:00:00', '2024-11-06 17:05:00', 0, 'Motivado');