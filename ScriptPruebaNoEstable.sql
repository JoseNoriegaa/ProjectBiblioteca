CREATE DATABASE Biblioteca

use Biblioteca
--TABLAS--

CREATE TABLE Libro
(
	Id_Libro varchar(30) primary key,
	ISBN varchar(30) NOT NULL,
	Titulo varchar(max) NOT NULL,
	Año int,
	Clasificacion varchar(30) NOT NULL,
	Autor varchar(80) NOT NULL,
	Descripcion nvarchar(max) default 'DESCRIPCIÓN NO AGREGADA',
	Editorial varchar(30),
	Lugar varchar(50),
	Edicion varchar(30),
	estatus bit
)

CREATE TABLE Carrera
(
	Carrera varchar(50) primary key
)

CREATE TABLE Alumno
(
	Matricula int primary key,
	Nombre varchar(50) NOT NULL,
	Correo varchar(50),
	Telefono VARCHAR(10),
	Carrera varchar(50) constraint fk_Alumno_Carrera
	foreign key (Carrera) 
	references Carrera (Carrera) not null,
	cuatrimestre int not null
)
alter table Alumno
add libroEstado bit

create table Ocupacion
(
Puesto varchar(50) primary key
)

CREATE TABLE Personal
(
	Numero_De_Empleado int primary key,
	Nombre varchar(50) NOT NULL,
	Ocupacion varchar(50) foreign key references Ocupacion(Puesto) not null,
	Correo varchar(30) NOT NULL,
	Telefono varchar(10)
)


CREATE TABLE Prestamo_alumno
 (
	Id_Prestamo int identity(1,1) primary key,
	Id_Libro varchar(30) constraint fk_Libro_Prestamo 
	foreign key (Id_Libro) 
	references Libro (Id_Libro) NOT NULL,
	Matricula int constraint fk_Alumno_Prestamo	
	foreign key (Matricula)	
	references Alumno (Matricula) not null,
	Fecha_Prestamo date,
	Fecha_Entrega date,
	Dias_De_Prestamo int check(Dias_De_Prestamo = 3 or Dias_De_Prestamo = null),
	Estado_Del_Libro nvarchar(max) default 'Descripción no agregada',
	Estado bit
 )

 create table Prestamo_Personal
(
	Id_Prestamo int identity (1,1) primary key,
	Libro varchar(30) foreign key references Libro(Id_Libro) not null,
	Personal int constraint fk_Personal_Prestamo
	foreign key (Personal)
	references Personal(Numero_De_Empleado) not null,
	Fecha_Prestamo date,
	Fecha_Entrega date,
	Dias_De_Prestamo int check(Dias_De_Prestamo = 3 or Dias_De_Prestamo = null),
	Estado_Del_Libro nvarchar(max) default 'Descripción no agregada',
	Estado bit
)





create procedure verificarLibro
@id varchar(30)
as
select count(Libro.Id_Libro) from Libro where Id_Libro=@id
go




Create procedure Agregar_Libro
@IdLibro varchar(30),
@ISBN varchar(30),
@Titulo varchar(max),
@Año varchar(30),
@Clasificacion varchar(30),
@Autor varchar(80),
@Descripcion nvarchar(max),
@Editorial varchar(30),
@Lugar varchar(50),
@Edicion varchar(30),
@Status bit
as
if(@Descripcion='default')
Insert into Libro
Values(@IdLibro, @ISBN, @Titulo, @Año, @Clasificacion, 
@Autor, default, @Editorial, @Lugar, @Edicion, @Status)
else
Insert into Libro
Values(@IdLibro, @ISBN, @Titulo, @Año, @Clasificacion, 
@Autor, @Descripcion, @Edicion, @Lugar, @Edicion, @Status)

go


create procedure BuscarLibros_NoPrestados
as
select * from Libro where Libro.estatus=0
go

create procedure BuscarLibros_Todos
as
select * from Libro 
go

create procedure Buscar_Libro
@IdLibro varchar (30),
@Autor varchar (80),
@Descripcion nvarchar(max),
@Titulo varchar(max),
@ISBN varchar(30)
as
Select * from Libro
where Id_Libro like '%'+@IdLibro+'%' or Libro.ISBN like '%'+@ISBN+'%' or Titulo like '%'+@Titulo+'%' or Autor like '%'+@Autor+'%' or Descripcion like '%'+@Descripcion+'%'
go


create procedure EliminarLibro
@IdLibro varchar(30)
as
delete Libro
where Libro.Id_Libro = @IdLibro
go


create procedure EditarLibro
@IdLibro varchar(30),
@ISBN varchar(30),
@Titulo varchar(max),
@Año varchar(30),
@Clasificacion varchar(30),
@Autor varchar(80),
@Descripcion nvarchar(max),
@Editorial varchar(30),
@Lugar varchar(50),
@Edicion varchar(30),
@IdLibroViejo varchar(30),
@Status bit
as

if(@Descripcion='default')
update Libro 
set Id_Libro=@IdLibro, ISBN=@ISBN, Titulo=@Titulo, Año=@Año, Clasificacion=@Clasificacion, Autor=@Autor, Descripcion=default,
Editorial=@Editorial, Lugar=@Lugar, Edicion=@Edicion, estatus=@Status
where Id_Libro=@IdLibroViejo
else
update Libro
set Id_Libro=@IdLibro, ISBN=@ISBN, Titulo=@Titulo, Año=@Año, Clasificacion=@Clasificacion, Autor=@Autor, Descripcion=@Descripcion,
Editorial=@Editorial, Lugar=@Lugar, Edicion=@Edicion, estatus=@Status
where Id_Libro=@IdLibroViejo
go


--||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||


create procedure AgregarCarrera
@Carrera varchar(50)
as
insert into Carrera
values(@Carrera)
go


create procedure EditarCarrera
@CarreraViejo varchar(50),
@Carrera varchar(50)
as
update Carrera
set Carrera=@Carrera
where Carrera=@CarreraViejo
go


create procedure VerCarreras
as
select * from Carrera
go


create procedure VerCarrera
@Carrera varchar(50)
as
select * from Carrera
where Carrera=@Carrera
go


create procedure EliminarCarrera
@Carrera varchar(50)
as
delete Carrera
where Carrera=@Carrera 
go


--||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

create procedure Alta_Alumno
@Matricula int,
@Nombre varchar(50),
@Correo varchar(50),
@Telefono VARCHAR(10),
@Carrera varchar(50),
@cuatrimestre int
as
Insert into Alumno (Matricula,Nombre,Correo,Telefono,Carrera,cuatrimestre,libroEstado) Values(@Matricula, @Nombre, @Correo, @Telefono, @Carrera,@cuatrimestre,0)
go

create procedure Mostrar_Alumnos
as
select * from Alumno 
go

create procedure Mostrar_Alumnos_SinLibro
as
select * from Alumno a where a.libroEstado=0
go

create procedure Buscar_Alumno
@coincidencia varchar(50)
as
select * from Alumno 
where Nombre like '%'+@coincidencia+'%' or str(Matricula) like '%'+@coincidencia+'%' 
go



create procedure verificarAlumno
@Matricula int
as
select count(Matricula) from Alumno where Matricula=@Matricula
go



create procedure Editar_Alumno
@Matricula int,
@MatriculaVieja int,
@Nombre varchar(50),
@Correo varchar(50),
@Telefono VARCHAR(10),
@Carrera varchar(50),
@Cuatrimestre int

as
update Alumno
set Matricula=@MAtricula,cuatrimestre=@Cuatrimestre, Nombre=@Nombre, Correo=@Correo, Telefono=@Telefono, Carrera=@Carrera
where Matricula=@MatriculaVieja
go

create procedure Eliminar_Alumno
@Matricula int
as
delete Prestamo_alumno where Matricula=@Matricula
Delete Alumno
where Matricula=@Matricula
go
--|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
--procedures-Pendiente

create procedure Agregar_Ocupacion

@Puesto varchar(50)
as
insert into Ocupacion values (@Puesto)
go



create procedure Mostrar_Ocupacion
as
select * from Ocupacion
go

--||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||


create procedure verificar_Personal
@NumeroDeEmpleado int
as
select count(Personal.Numero_De_Empleado) from Personal where Numero_De_Empleado=@NumeroDeEmpleado
go

create procedure Agregar_Personal
@Numero_De_Empleado int,
@Nombre varchar(50) ,
@Ocupacion varchar(50),
@Correo varchar(30),
@Telefono varchar(10)
as
insert into Personal
values(@Numero_De_Empleado, @Nombre, @Ocupacion, @Correo, @Telefono)
go


create procedure Buscar_Personal

@coincidencia varchar(50)
as
select * from Personal
where Nombre like '%'+@coincidencia+'%' or str(Numero_De_Empleado) like '%'+@coincidencia+'%'
go

create procedure Mostrar_Personal
as
select * from Personal
go

create procedure Editar_Personal
@Numero_De_Empleado int,
@Numero_De_Empleado_Viejo int,
@Nombre varchar(50) ,
@Ocupacion varchar(50),
@Correo varchar(30),
@Telefono varchar(10)
as
update Personal
set Numero_De_Empleado=@Numero_De_Empleado, Nombre=@Nombre, Ocupacion=@Ocupacion, Correo=@Correo, Telefono=@Telefono 
where Numero_De_Empleado=@Numero_De_Empleado_Viejo
go

create procedure Eliminar_Personal
@Numero_De_Empleado int
as
delete from Prestamo_Personal where Personal=@Numero_De_Empleado
delete from Personal
where Numero_De_Empleado=@Numero_De_Empleado
go
--|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

create procedure MostrarPrestamos_Alumno
as
select a.Matricula, b.Nombre,c.Titulo, c.ISBN,a.Id_Libro,a.Id_Prestamo,a.Fecha_Entrega from Prestamo_alumno a,Alumno b,Libro c where a.Matricula=b.Matricula and a.Id_Libro=c.Id_Libro and a.Estado=1
go
create procedure historialPrestamos_Alumno
as
select a.Matricula, b.Nombre,c.Titulo, c.ISBN,a.Id_Libro,a.Id_Prestamo,a.Fecha_Entrega,a.Fecha_Prestamo from Prestamo_alumno a,Alumno b,Libro c where a.Matricula=b.Matricula and a.Id_Libro=c.Id_Libro
ORDER BY (a.Fecha_Prestamo) asc
go

--drop table Prestamo_alumno
--drop table Prestamo_Personal

create procedure MostrarPrestamos_Personal
as
select a.Personal, b.Nombre,c.Titulo, c.ISBN,a.Libro,a.Id_Prestamo,a.Fecha_Entrega from Prestamo_Personal a,Personal b,Libro c where a.Personal=b.Numero_De_Empleado and a.Libro=c.Id_Libro and a.Estado=1
go

create procedure historialPrestamos_Personal
as
select a.Personal, b.Nombre,c.Titulo, c.ISBN,a.Libro,a.Id_Prestamo,a.Fecha_Entrega,a.Fecha_Prestamo from Prestamo_Personal a,Personal b,Libro c where a.Personal=b.Numero_De_Empleado and a.Libro=c.Id_Libro
ORDER BY (a.Fecha_Prestamo) asc
go

create procedure BuscarPrestamo_Personal
@Coincidencia varchar(max)
as
select p.Personal, per.Nombre, l.Titulo, l.ISBN,p.Libro,p.Id_Prestamo,p.Fecha_Entrega ,p.Fecha_Prestamo
from Prestamo_Personal p,Personal per,Libro l 
where (p.Personal=per.Numero_De_Empleado and p.Libro=l.Id_Libro)
and(per.Nombre like '%'+@Coincidencia+'%' or l.Titulo like '%'+@Coincidencia+'%'
or p.Fecha_Prestamo like '%'+@Coincidencia+'%' or l.ISBN like '%'+@Coincidencia+'%')
go

create procedure BuscarPrestamo_Alumno
@Coincidencia varchar(max)
as
select p.Matricula, a.Nombre, l.Titulo, l.ISBN,p.Id_Libro,p.Id_Prestamo,p.Fecha_Entrega ,p.Fecha_Prestamo
from Prestamo_alumno p,Alumno a,Libro l 
where (p.Matricula=a.Matricula and p.Id_Libro=l.Id_Libro)
and(a.Nombre like '%'+@Coincidencia+'%' or l.Titulo like '%'+@Coincidencia+'%'
or p.Fecha_Prestamo like '%'+@Coincidencia+'%' or l.ISBN like '%'+@Coincidencia+'%')

go



create procedure Registrar_Prestamo
	
	@Libro varchar(30),
	@idPersona int ,
	@Fecha_Prestamo date,
	@Fecha_Entrega date,
	@Dias_De_Prestamo int ,
	@Estado_Del_Libro nvarchar(max) ,
	@tipoPrestamo varchar(15),
	@Estado bit
	as
	update Libro set Libro.estatus=1 where Libro.Id_Libro=@Libro
	if(@tipoPrestamo='Alumno')
		begin
		update Alumno set libroEstado=1 
			
		if(@Estado_Del_Libro='default')
			
			insert into Prestamo_alumno values(@Libro, @idPersona,@Fecha_Prestamo,@Fecha_Entrega,@Dias_De_Prestamo,default,@Estado)
		else
		
		
			insert into Prestamo_alumno values(@Libro, @idPersona,@Fecha_Prestamo,@Fecha_Entrega,@Dias_De_Prestamo,@Estado_Del_Libro,@Estado)
		end
	else
		if(@Estado_Del_Libro='default')
			insert into Prestamo_Personal values(@Libro, @idPersona,@Fecha_Prestamo,@Fecha_Entrega,@Dias_De_Prestamo,default,@Estado)
		else
			insert into Prestamo_Personal values(@Libro, @idPersona,@Fecha_Prestamo,@Fecha_Entrega,@Dias_De_Prestamo,@Estado_Del_Libro,@Estado)		
	go


create procedure RegistrarDevolucion
@IdPrestamo int,
@IdLibro varchar(30)
as
	if((select count(Id_Prestamo) from Prestamo_alumno where Id_Libro=@IdLibro and Estado=1)=1)
		update Prestamo_alumno set Estado='' where Id_Prestamo=@IdPrestamo
		update Libro set estatus=0 where Id_Libro=@IdLibro
		update Alumno set libroEstado=0
		
	if((select count(Id_Prestamo) from Prestamo_Personal a where a.Libro=@IdLibro and a.Estado=1)=1)
		update Prestamo_Personal set Estado=0 where Id_Prestamo=@IdPrestamo
		update Libro set estatus=0 where Id_Libro=@IdLibro
go








create procedure mostrarPrestamosVigentes
as
SELECT SUM(T1.sumas)as Prestamos FROM
 (SELECT count(Id_Prestamo)as sumas
     FROM Prestamo_Personal WHERE Estado=1
	 union all
  SELECT count(Id_Prestamo)
      FROM Prestamo_alumno WHERE Estado=1)as T1
	  go



--procedures para las graficas
create procedure librosPrestados_Alumno
as
select b.Titulo, count(*) as Prestamos from Prestamo_alumno a,Libro b where a.Id_Libro=b.Id_Libro group by a.Id_Libro,b.Titulo
go
create procedure librosPrestados_Personal
as
select b.Titulo, count(*) as Prestamos from Prestamo_Personal a,Libro b where a.Libro=b.Id_Libro group by a.Libro,b.Titulo
go
create procedure carreraPrestamos
as
select a.Carrera,COUNT(Id_Prestamo) as Prestamos from Prestamo_alumno p,Alumno a where p.Matricula=a.Matricula group by a.Carrera,p.Matricula
go
create procedure puestosPrestamos
as
select a.Ocupacion,COUNT(Id_Prestamo) as Prestamos from Prestamo_Personal p,Personal a  where p.Personal=a.Numero_De_Empleado group by a.Ocupacion,p.Personal
go

create procedure fechaPrestamos
as
select MONTH( Fecha_Prestamo)as mes ,YEAR(Fecha_Prestamo)as año,COUNT(*) as Prestamos from Prestamo_Personal group by MONTH( Fecha_Prestamo)
,YEAR(Fecha_Prestamo),
MONTH( Fecha_Prestamo) union all
select MONTH( Fecha_Prestamo)as mes ,YEAR(Fecha_Prestamo)as año,COUNT(*) as Prestamos from Prestamo_alumno group by MONTH( Fecha_Prestamo)
,YEAR(Fecha_Prestamo),
MONTH( Fecha_Prestamo)
go




INSERT INTO Carrera
VALUES('TIC')
INSERT INTO Carrera
VALUES('Mineria')
INSERT INTO Carrera
VALUES('Desarrollo De Negocios')
INSERT INTO Carrera
VALUES('Paramedicos')
INSERT INTO Carrera
VALUES('Gastronomia')
INSERT INTO Carrera
VALUES('Mantenimiento')

INSERT INTO Alumno
VALUES(16304001, 'Ana Cristina', 'cristinafbte17@hotmail.com', 6381252277, 'Gastronomia')
INSERT INTO Alumno
VALUES(16304007, 'Jose', 'thejose123654@hotmail.com', 6381252278, 'Tecnologias De La Información Y De La Comunicación')
INSERT INTO Alumno
VALUES(16304004, 'Wendy', 'momma@gmail.com', 6381252248, 'Mineria')
INSERT INTO Alumno
VALUES(16304011, 'Abiel', 'darkcry@gmail.com', 6381257812, 'Tecnologias De La Información Y De La Comunicación')
INSERT INTO Alumno
VALUES(16304012, 'Mario', 'shapo@gmail.com', 6381521684, 'Paramedicos')

INSERT INTO Libro
VALUES('13510416', 'dfg1d1g6fkgf4', 'El Diario De Ana Frank', 1988, 'Diario', 'Ana Frank', default, null, null, null,0)
INSERT INTO Libro
VALUES('13510716', 'gf2kd8v8dv5p', 'Harry Potter ', 1990, 'Fantasia', 'J.K Rowling', default, null, null, null,0)
INSERT INTO Libro
VALUES('13510816', 'dg8s6g9bkgf4', '50 Sombras De Grey', 2000, 'Romance', 'Susane Collins', default, null, null, null,0)
INSERT INTO Libro
VALUES('13410416', 'df7ho9n5j9fc', 'Anaya', 2003, 'Enciclopedia', 'Jeff Ferguson', default, null, null, null,0)
INSERT INTO Libro
VALUES('23504168', 'sdvsd5v8c7d', 'Crear O Morir', 1995, 'jhgrf', 'Andres Oppenhaimer', default, null, null, null,0)


select A.Carrera from Alumno A
inner join Carrera on A.Carrera = Carrera.Carrera

select A.Carrera from Alumno A
left join Carrera on A.Carrera = Carrera.Carrera

select A.Carrera from Alumno A
right join Carrera on A.Carrera = Carrera.Carrera

select A.Carrera from Alumno A
full outer join Carrera on A.Carrera = Carrera.Carrera
where A.Carrera is null and Carrera.Carrera is null


--CREATE TABLE Maestro
--(
--	Numero_De_Empleado int primary key,
--	Nombre varchar(50) NOT NULL,
--	Ocupacion varchar(50) not null,
--	Correo varchar(30) NOT NULL,
--	Telefono varchar(10)
--)
--INSERT INTO Maestro
--VALUES(152, 'Juan Soto', 'juansoto@utpp.edu.mx', null)
--INSERT INTO Maestro
--VALUES(153, 'Raquel cibrian', 'raquelcibrian@utpp.edu.mx', null)
--INSERT INTO Maestro
--VALUES(154, 'DanielFflores', 'danielflores@utpp.edu.mx', null)
--INSERT INTO Maestro
--VALUES(155, 'Karen Illingworth', 'illingworth@utpp.edu.mx', null)
--INSERT INTO Maestro
--VALUES(156, 'Bertha Murrieta', 'bamurrieta@utpp.edu.mx', null)

--CREATE TABLE Prestamo
--(
--	Id_Prestamo int identity(1,1) primary key,
--	Id_Libro varchar(30) constraint fk_Libro_Prestamo
--	foreign key (Id_Libro)
--	references Libro (Id_Libro) NOT NULL,
--	Matricula int constraint fk_Alumno_Prestamo
--	foreign key (Matricula) 
--	references Alumno (Matricula),
--	Numero_De_Empleado int constraint fk_Maestro_Prestamo
--	foreign key (Numero_De_Empleado) 
--	references Maestro (Numero_De_Empleado),
--	Fecha_Prestamo datetime,
--	Fecha_Entrega datetime,
--	Dias_De_Prestamo int check(Dias_De_Prestamo = 3 or Dias_De_Prestamo = null),
--	Estado_Del_Libro nvarchar(max) default 'Descripción no agregada'
--)

