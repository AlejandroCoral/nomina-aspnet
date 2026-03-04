-- Datos para las Tablas (3 para cada uno)
USE nomina_db;
GO

INSERT INTO employees (emp_no, ci, birth_date, first_name, last_name, gender, hire_date, correo)
VALUES
(1, '1723456789', '1998-05-10', 'Alejandro', 'Coral', 'M', '2023-01-15', 'alejandro@empresa.com'),
(2, '1723456790', '1995-08-21', 'Maria', 'Lopez', 'F', '2022-03-10', 'maria@empresa.com'),
(3, '1723456791', '1992-11-30', 'Carlos', 'Perez', 'M', '2021-07-01', 'carlos@empresa.com');

------------------------------

INSERT INTO departments (dept_no, dept_name)
VALUES
(10, 'Recursos Humanos'),
(20, 'Tecnologia');

----------------------------

INSERT INTO users (emp_no, usuario, clave)
VALUES
(1, 'admin', 'admin123');

-----------------------------

INSERT INTO dept_emp (emp_no, dept_no, from_date, to_date)
VALUES
(1, 10, '2023-01-15', '9999-01-01'),
(2, 10, '2022-03-10', '9999-01-01'),
(3, 20, '2021-07-01', '9999-01-01');

--------------------------------

INSERT INTO salaries (emp_no, salary, from_date, to_date)
VALUES
(1, 1200, '2023-01-15', NULL),
(2, 1000, '2022-03-10', NULL),
(3, 1500, '2021-07-01', NULL);

----------------------------------

INSERT INTO dept_manager (emp_no, dept_no, from_date, to_date)
VALUES
(1, 10, '2023-01-15', '9999-01-01'),
(3, 20, '2021-07-01', '9999-01-01');

-----------------------------------

INSERT INTO titles (emp_no, title, from_date, to_date)
VALUES
(1, 'Jefe RRHH', '2023-01-15', NULL),
(2, 'Asistente RRHH', '2022-03-10', NULL),
(3, 'Desarrollador', '2021-07-01', NULL);

----------------------------------------
--Comprobación
SELECT * FROM employees;
SELECT * FROM departments;
SELECT * FROM salaries;









