CREATE DATABASE DB_Biblioteca_Prueba

use DB_Biblioteca_Prueba

--TABLAS--

CREATE TABLE Libro
(
	Id_Libro varchar(30) primary key,
	ISBN varchar(30) NOT NULL,
	Titulo varchar(200) NOT NULL,
	Año int,
	Clasificacion varchar(200) NOT NULL,
	Autor varchar(200) NOT NULL,
	Descripcion nvarchar(max) default 'DESCRIPCIÓN NO AGREGADA',
	Editorial varchar(200),
	Lugar varchar(200),
	Edicion varchar(200),
	estatus bit,
	Precio money
)

CREATE TABLE Carrera
(
	IdCarrera varchar(10) primary key,
	Carrera varchar(50) not null
)

create table Correo
(
	Correo varchar(200) primary key,
	Contraseña varchar(200) not null,
	Asunto varchar(200) not null,
	Cuerpo varchar(max) not null,
	AsuntoNotificacion varchar(200) not null,
	CuerpoNotificacion varchar(max) not null
)

CREATE TABLE Alumno
(
	Matricula int primary key,
	Nombre varchar(100) NOT NULL,
	Apellido varchar(100) NOT NULL,
	Correo varchar(200),
	Telefono VARCHAR(10),
	Carrera varchar(10) not null,
	cuatrimestre int not null,
	LibroEstado bit
)

create table Ocupacion
(
	Puesto varchar(200) primary key
)

CREATE TABLE Personal
(
	Numero_De_Empleado int primary key,
	Nombre varchar(50) NOT NULL,
	Ocupacion varchar(50) not null,
	Correo varchar(50) NOT NULL,
	Telefono varchar(10)
)


CREATE TABLE Prestamo_alumno
 (
	Id_Prestamo int identity(1,1) primary key,
	Id_Libro varchar(30) NOT NULL,
	Matricula int not null,
	Fecha_Prestamo date,
	Fecha_Entrega date,
	Dias_De_Prestamo int check(Dias_De_Prestamo = 3 or Dias_De_Prestamo = null),
	Estado_Del_Libro nvarchar(max) default 'Descripción no agregada',
	Estado bit
 )

 create table Prestamo_Personal
(
	Id_Prestamo int identity (1,1) primary key,
	Libro varchar(30) not null,
	Personal int not null,
	Fecha_Prestamo date,
	Dias_De_Prestamo int check(Dias_De_Prestamo = 3 or Dias_De_Prestamo = null),
	Estado_Del_Libro nvarchar(max) default 'Descripción no agregada',
	Estado bit
)

















create procedure countCorreo
as
select count(Correo) from Correo
go

create procedure countISBN
@ISBN varchar(30)
as
select count(ISBN) from Libro where ISBN=@ISBN
go
----------------------------------------------------------------------------------------------------
create procedure verCorreo
as
select * from Correo
go

create procedure ingresarCorreo
@Correo varchar(200),
@Contraseña varchar(200),
@Asunto varchar(200),
@Cuerpo nvarchar(max)
as
if ((select count(Correo) from Correo)=0)
insert into Correo values(@Correo,@Contraseña,@Asunto,@Cuerpo,'','')
else
update Correo set Correo=@Correo, Contraseña=@Contraseña, Asunto=@Asunto, Cuerpo=@Cuerpo
go

create procedure adddNotificacionesCorreo
@Asunto varchar(200),
@Cuerpo nvarchar(max)
as
update Correo set Asunto=@Asunto, Cuerpo=@Cuerpo
go

create procedure verificarLibro
@id varchar(30)
as
select count(Libro.Id_Libro) from Libro where Id_Libro=@id
go

create procedure verificarLibroPrestado
@id varchar(30)
as
select estatus from Libro where Id_Libro=@id
go

Create procedure Agregar_Libro
@IdLibro varchar(30),
@ISBN varchar(30),
@Titulo varchar(max),
@Año int,
@Clasificacion varchar(200),
@Autor varchar(200),
@Descripcion nvarchar(max),
@Editorial varchar(200),
@Lugar varchar(200),
@Edicion varchar(200),
@Status bit,
@Precio money
as
if(@Descripcion='default')
Insert into Libro
Values(@IdLibro, @ISBN, @Titulo, @Año, @Clasificacion, 
@Autor, default, @Editorial, @Lugar, @Edicion, @Status,@Precio)
else
Insert into Libro
Values(@IdLibro, @ISBN, @Titulo, @Año, @Clasificacion, 
@Autor, @Descripcion, @Edicion, @Lugar, @Edicion, @Status,@Precio)

go

create procedure Buscar_Alumno_sinLibro
@coincidencia varchar(50)
as
select * from Alumno 
where LibroEstado=0 and(Nombre like '%'+@coincidencia+'%' or Apellido like '%'+@coincidencia+'%' or str(Matricula) like '%'+@coincidencia+'%')
go

create procedure BuscarLibros_NoPrestados
as
select * from Libro where Libro.estatus=0
order by titulo asc
go

create procedure BusquedaLibros_NoPrestados
@coincidencia varchar(50)
as
select * from Libro where Libro.estatus=0 and(Titulo like '%'+@coincidencia+'%' or Autor like '%'+@coincidencia+'%')
go

create procedure BuscarLibros_Todos
as
select * from Libro 
go

create procedure buscarLibro
@ISBN varchar(30)
as
select Id_Libro from Libro where ISBN=@ISBN order by Id_Libro asc
go

create procedure Buscar_Libro
@IdLibro varchar (30),
@Autor varchar (200),
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
delete Prestamo_alumno where Id_Libro=@IdLibro
delete Prestamo_Personal where Libro=@IdLibro
delete Libro where Libro.Id_Libro = @IdLibro
go

create procedure buscarLibro_ID
@id varchar(30)
as
select * from Libro where Id_Libro=@id
go

create procedure EditarLibro
@IdLibro varchar(30),
@ISBN varchar(30),
@Titulo varchar(max),
@Año varchar(30),
@Clasificacion varchar(50),
@Autor varchar(80),
@Descripcion nvarchar(max),
@Editorial varchar(50),
@Lugar varchar(50),
@Edicion varchar(50),
@IdLibroViejo varchar(50),
@Status bit,
@Precio money
as

if(@Descripcion='default')
update Libro 
set Id_Libro=@IdLibro, ISBN=@ISBN, Titulo=@Titulo, Año=@Año, 
Clasificacion=@Clasificacion, Autor=@Autor, Descripcion=default,
Editorial=@Editorial, Lugar=@Lugar, Edicion=@Edicion, estatus=@Status, Precio=@Precio
where Id_Libro=@IdLibroViejo
else
update Libro
set Id_Libro=@IdLibro, ISBN=@ISBN, Titulo=@Titulo, Año=@Año, Clasificacion=@Clasificacion, Autor=@Autor, Descripcion=@Descripcion,
Editorial=@Editorial, Lugar=@Lugar, Edicion=@Edicion, estatus=@Status,Precio=@Precio
where Id_Libro=@IdLibroViejo

update Prestamo_alumno set Id_Libro=@IdLibro where Id_Libro=@IdLibroViejo

update Prestamo_Personal set Libro=@IdLibro where Libro=@IdLibroViejo

go


create procedure AgregarCarrera
@IdCarrera varchar(10),
@Carrera varchar(50)
as
insert into Carrera
values(@IdCarrera, @Carrera)
go


create procedure VerCarreras
as
select * from Carrera
go


create procedure EliminarCarrera
@IdCarrera varchar(10)
as
delete alumno where Carrera=@IdCarrera
delete Carrera
where Carrera=@IdCarrera 
go


create procedure EditarCarrera
@IdCarreraViejo varchar(10),
@IdCarrera varchar(10),
@NombreCarrera varchar(50)

as
update alumno set Carrera=@IdCarrera where Carrera=@IdCarreraViejo
update Carrera set IdCarrera=@IdCarrera, Carrera=@NombreCarrera where IdCarrera=@IdCarreraViejo

go


create procedure Alta_Alumno
@Matricula int,
@Nombre varchar(200),
@Apellidos varchar(200),
@Correo varchar(200),
@Telefono VARCHAR(10),
@Carrera varchar(10),
@cuatrimestre int
as
Insert into Alumno (Matricula,Nombre,Apellido,Correo,Telefono,Carrera,cuatrimestre,libroEstado) 
Values(@Matricula, @Nombre, @Apellidos,@Correo, @Telefono, @Carrera,@cuatrimestre,0)
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
where Nombre like '%'+@coincidencia+'%' or str(Matricula) like '%'+@coincidencia+'%' or Apellido like '%'+@coincidencia+'%'
go


create procedure verificarAlumno
@Matricula int
as
select count(Matricula) from Alumno where Matricula=@Matricula
go

create procedure libroEstadoAlumno
@Matricula int
as
select LibroEstado from Alumno where Matricula=@Matricula
go


create procedure Editar_Alumno
@Matricula int,
@MatriculaVieja int,
@Nombre varchar(200),
@Apellido varchar(200),
@Correo varchar(200),
@Telefono VARCHAR(10),
@Carrera varchar(10),
@Cuatrimestre int

as
update Alumno
set Matricula=@Matricula,cuatrimestre=@Cuatrimestre, Nombre=@Nombre, Apellido=@Apellido,
Correo=@Correo, Telefono=@Telefono, Carrera=@Carrera
where Matricula=@MatriculaVieja

update Prestamo_alumno set Matricula=@Matricula where Matricula=@MatriculaVieja

go

create procedure Eliminar_Alumno
@Matricula int
as
delete Prestamo_alumno where Matricula=@Matricula

Delete Alumno where Matricula=@Matricula

go


create procedure Agregar_Ocupacion
@Puesto varchar(50)
as
insert into Ocupacion values (@Puesto)
go


create procedure Mostrar_Ocupacion
as
select * from Ocupacion
go


create procedure verificar_Personal
@NumeroDeEmpleado int
as
select count(Personal.Numero_De_Empleado) from Personal where Numero_De_Empleado=@NumeroDeEmpleado
go

create procedure verificar_Libro_Personal
@NumeroDeEmpleado int
as
select count(Personal) from Prestamo_Personal where Personal=@NumeroDeEmpleado
go

create procedure Agregar_Personal
@Numero_De_Empleado int,
@Nombre varchar(50) ,
@Ocupacion varchar(50),
@Correo varchar(50),
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
@Nombre varchar(200) ,
@Ocupacion varchar(200),
@Correo varchar(200),
@Telefono varchar(10)
as
update Personal
set Numero_De_Empleado=@Numero_De_Empleado, Nombre=@Nombre, Ocupacion=@Ocupacion, Correo=@Correo, Telefono=@Telefono 
where Numero_De_Empleado=@Numero_De_Empleado_Viejo

update Prestamo_Personal set Personal=@Numero_De_Empleado where Personal=@Numero_De_Empleado_Viejo

go

create procedure Eliminar_Personal
@Numero_De_Empleado int
as
delete from Prestamo_Personal where Personal=@Numero_De_Empleado
delete from Personal
where Numero_De_Empleado=@Numero_De_Empleado
go


alter procedure MostrarPrestamos_Alumno
as
select p.Id_Prestamo,p.Matricula, a.Nombre, a.Apellido, p.Id_Libro, l.Titulo, l.ISBN ,p.Fecha_Entrega
from Prestamo_alumno p,Alumno a,Libro l 
where (p.Matricula=a.Matricula and p.Id_Libro=l.Id_Libro) and p.Estado=1 and p.Estado=1
go


create procedure historialPrestamos_Alumno
as
select a.Id_Prestamo,a.Matricula, b.Nombre, b.Apellido,a.Id_Libro,c.Titulo, c.ISBN,a.Fecha_Entrega,a.Fecha_Prestamo 
from Prestamo_alumno a,Alumno b,Libro c 
where a.Matricula=b.Matricula and a.Id_Libro=c.Id_Libro
ORDER BY (a.Fecha_Prestamo) asc
go


alter procedure borrarHistorial
as
truncate table Prestamo_alumno
truncate table Prestamo_Personal
go

create procedure MostrarPrestamos_Personal
as
select p.Id_Prestamo,p.Personal, per.Nombre, p.Libro,l.Titulo, l.ISBN,p.Fecha_Prestamo
from Prestamo_Personal p,Personal per,Libro l 
where (p.Personal=per.Numero_De_Empleado and p.Libro=l.Id_Libro) and p.Estado=1
go

create procedure historialPrestamos_Personal
as
select a.Id_Prestamo,a.Personal, b.Nombre,a.Libro,c.Titulo, c.ISBN,a.Fecha_Prestamo
from Prestamo_Personal a,Personal b,Libro c 
where a.Personal=b.Numero_De_Empleado and a.Libro=c.Id_Libro
ORDER BY (a.Fecha_Prestamo) asc
go

create procedure BuscarPrestamo_Personal_NoDevuelto
@Coincidencia varchar(max)
as
select p.Personal, per.Nombre,l.Titulo, l.ISBN, p.Libro,p.Id_Prestamo,p.Fecha_Prestamo
from Prestamo_Personal p,Personal per,Libro l 
where (p.Personal=per.Numero_De_Empleado and p.Libro=l.Id_Libro)
and(per.Nombre like '%'+@Coincidencia+'%' or l.Titulo like '%'+@Coincidencia+'%'
or p.Fecha_Prestamo like '%'+@Coincidencia+'%' or l.ISBN like '%'+@Coincidencia+'%') and p.Estado=1
go

create procedure BuscarPrestamo_Personal
@Coincidencia varchar(max)
as
select p.Id_Prestamo,p.Personal, per.Nombre, p.Libro,l.Titulo, l.ISBN,p.Fecha_Prestamo
from Prestamo_Personal p,Personal per,Libro l 
where (p.Personal=per.Numero_De_Empleado and p.Libro=l.Id_Libro)
and(per.Nombre like '%'+@Coincidencia+'%' or l.Titulo like '%'+@Coincidencia+'%'
or p.Fecha_Prestamo like '%'+@Coincidencia+'%' or l.ISBN like '%'+@Coincidencia+'%')
go

create procedure BuscarPrestamo_Alumno
@Coincidencia varchar(max)
as
select p.Id_Prestamo,p.Matricula, a.Nombre, a.Apellido, p.Id_Libro, l.Titulo, l.ISBN ,p.Fecha_Prestamo,p.Fecha_Entrega
from Prestamo_alumno p,Alumno a,Libro l 
where (p.Matricula=a.Matricula and p.Id_Libro=l.Id_Libro)
and(a.Nombre like '%'+@Coincidencia+'%' or a.Apellido like '%'+@Coincidencia+'%' or l.Titulo like '%'+@Coincidencia+'%'
or p.Fecha_Prestamo like '%'+@Coincidencia+'%' or l.ISBN like '%'+@Coincidencia+'%' or p.Matricula like '%'+@Coincidencia+'%' or p.Id_Libro like '%'+@Coincidencia+'%') and p.Estado=1
go

create procedure BuscarPrestamo_Alumno_NoDevuelto
@Coincidencia varchar(max)
as
select p.Matricula, a.Nombre, a.Apellido,  l.Titulo, l.ISBN ,p.Id_Libro,p.Id_Prestamo,p.Fecha_Prestamo
from Prestamo_alumno p,Alumno a,Libro l 
where (p.Matricula=a.Matricula and p.Id_Libro=l.Id_Libro)
and(a.Nombre like '%'+@Coincidencia+'%' or a.Apellido like '%'+@Coincidencia+'%' or l.Titulo like '%'+@Coincidencia+'%'
or p.Fecha_Prestamo like '%'+@Coincidencia+'%' or l.ISBN like '%'+@Coincidencia+'%' or p.Matricula like '%'+@Coincidencia+'%' or p.Id_Libro like '%'+@Coincidencia+'%') and p.Estado=1
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
	if(@tipoPrestamo='Alumno')
		begin
			
		if(@Estado_Del_Libro='default')
			
			insert into Prestamo_alumno values(@Libro, @idPersona,@Fecha_Prestamo,@Fecha_Entrega,@Dias_De_Prestamo,default,@Estado)
		else
		
			insert into Prestamo_alumno values(@Libro, @idPersona,@Fecha_Prestamo,@Fecha_Entrega,@Dias_De_Prestamo,@Estado_Del_Libro,@Estado)
		end
	else
		if(@Estado_Del_Libro='default')
			insert into Prestamo_Personal values(@Libro, @idPersona,@Fecha_Prestamo,@Dias_De_Prestamo,default,@Estado)
		else
			insert into Prestamo_Personal values(@Libro, @idPersona,@Fecha_Prestamo,@Dias_De_Prestamo,@Estado_Del_Libro,@Estado)		
	go


create procedure RegistrarDevolucion
@IdPrestamo int,
@IdLibro varchar(30),
@Matricula int
as
	if((select count(Id_Prestamo) from Prestamo_alumno where Id_Libro=@IdLibro and Estado=1)=1)
		update Prestamo_alumno set Estado=0 where Id_Prestamo=@IdPrestamo
		update Libro set estatus=0 where Id_Libro=@IdLibro
		update Alumno set libroEstado=0 where Matricula = @Matricula
		
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
VALUES('TIC','TECNOLOGIAS DE LA INFORMACION Y LA COMUNICACION')
INSERT INTO Carrera
VALUES('MIN','MINERÍA')
INSERT INTO Carrera
VALUES('DN','DESARROLLO DE NEGOCIOS')
INSERT INTO Carrera
VALUES('PARAM','PARAMÉDICOS')
INSERT INTO Carrera
VALUES('GA','GASTRONOMÍA')
INSERT INTO Carrera
VALUES('LGA','LICENCIATURA EN GASTRONOMÍA')
INSERT INTO Carrera
VALUES('LPC','LICENCIATURA EN PROTECCIÓN CIVIL')
INSERT INTO Carrera
VALUES('IDIE','INGENIERÍA EN DESARROLLO E INNOVACIÓN EMPRESARIAL')


----------------triggers

create trigger altaPrestamosAlumnos
on Prestamo_alumno
for insert
as
declare @Matricula int
declare @Libro varchar(30)
select @Matricula = (select Matricula from inserted)
select @Libro = (select Id_Libro from inserted)

update Alumno set LibroEstado=1 where Matricula=@Matricula
update Libro set estatus=1 where Id_Libro=@Libro

go

create trigger altaPrestamosPesonal
on Prestamo_Personal
for insert
as
declare @Libro varchar(30)
select @Libro = (select Libro from inserted)

update Libro set estatus=1 where Id_Libro=@Libro
go

-----------------inserciones entre BDs-----------

---------------LIBROS
 INSERT INTO  DB_Biblioteca_Prueba.dbo.Libro 
 ([Id_Libro],[Titulo],[Año],[Clasificacion],[Autor],[Editorial],[Lugar],[Edicion],[Precio],[ISBN],[Descripcion])
 
 SELECT e.EjemplarID,l.NombreLibro,l.Año,l.Clasificacion,l.Autor,l.Editorial,l.Lugar,l.edicion,l.Precio,e.LibroId,e.Descripcion

 FROM BIBLIOTECA.dbo.Tabla_Libros l ,BIBLIOTECA.dbo.EJEMPLARES e

 WHERE l.ID_Libros=e.LibroId

 order by e.EjemplarID

 UPDATE DB_Biblioteca_Prueba.dbo.Libro SET estatus=0


------------ALUMNOS
 INSERT INTO [DB_Biblioteca_Prueba].[dbo].[Alumno]
([Matricula],[Nombre],[Apellido],[Telefono],[cuatrimestre],[Carrera],[Correo])
 
 SELECT 
 [ID_Usuario],[NombreUsuario],[ApellidoUsuario],CAST([Telefono] AS VARCHAR(10)),[Cuatrimestre],
 CAST([Grupo] AS VARCHAR(10)),CAST([E-Mail] AS VARCHAR (200))
 FROM [BIBLIOTECA].[dbo].[Tabla_Usuario] 

 UPDATE [DB_Biblioteca_Prueba].[dbo].[Alumno] set Libroestado =0
 update DB_Biblioteca_Prueba.dbo.alumno set Carrera = 'GA' where Carrera like '%ga%' and cuatrimestre<7
 update DB_Biblioteca_Prueba.dbo.alumno set Carrera = 'LGA' where Carrera like '%ga%' and cuatrimestre>6
 update DB_Biblioteca_Prueba.dbo.alumno set Carrera = 'TIC' where Carrera like '%tic%'
 update DB_Biblioteca_Prueba.dbo.alumno set Carrera = 'PARAM' where Carrera like '%pa%' 
 update DB_Biblioteca_Prueba.dbo.alumno set Carrera = 'LPC' where Carrera like '%lpc%' or Carrera like '%pa%' and cuatrimestre>6
 update DB_Biblioteca_Prueba.dbo.alumno set Carrera = 'DN' where Carrera like '%dn%' OR Carrera like '%desa%' and cuatrimestre<7
 update DB_Biblioteca_Prueba.dbo.alumno set Carrera = 'IDIE' where Carrera like '%dn%' OR Carrera like '%desa%' and cuatrimestre>6
 update DB_Biblioteca_Prueba.dbo.alumno set Carrera = 'MIN' where Carrera like '%min%'
 update DB_Biblioteca_Prueba.dbo.alumno set Carrera = 'MANT' where Carrera like '%mant%'
 --**Falta saber si camiaria nombre en lic e ing**----
 ------------