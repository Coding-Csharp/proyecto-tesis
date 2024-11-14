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
	zone_access varchar(500) NOT NULL

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