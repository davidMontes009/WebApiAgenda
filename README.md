# WebApiAgenda

## Pasos para ejecutar la web API
1.- Crear la base de datos en local BD_Agenda
2.- Ejecutar el Query que se adjunta al final de este manual 
3.-En el archivo appsettings.json revisar que el conection string coincida con los datos de su localhost en ConnectionStrings:ConnectionDev
4.- Correr este proyecto antes de ejecutar el proyecto de web app 

## Recomendaciones 
1.- Correr el proyecto con visual studio 2019
2.- Crear la base en SQL server 2018


## Adjunto query para crear base de datos 
USE [BD_Agenda]
GO
/****** Object:  Table [dbo].[TBL_CONTACTO]    Script Date: 19/06/2020 11:12:53 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_CONTACTO](
	[IdContacto] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NULL,
	[Apellidos] [varchar](50) NULL,
	[Telefono] [varchar](20) NULL,
	[TipoTel] [int] NULL,
	[Email] [varchar](50) NULL,
	[FechaCracion] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_TIPO_TELEFONO]    Script Date: 19/06/2020 11:12:53 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_TIPO_TELEFONO](
	[IdTipoTel] [int] NULL,
	[Nombre] [varchar](20) NULL,
	[Descripcion] [varchar](100) NULL,
	[Estatus] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[CRUD_CONTACT]    Script Date: 19/06/2020 11:12:53 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/**		 
	 * PROCEDURE : CRUD_CONTACT		
	 * DESCRIPCION : ADMINISTRA EL CATALOGO DE CONTACTOS
	 * AUTOR  : David Montes		
	 * FECHA  : 19/06/20
	 * MODIFICACIONES:		 
	**/
CREATE PROCEDURE [dbo].[CRUD_CONTACT]

	@OPERATION		INT			=	0,
	@ID_CONTACTO	INT			=	NULL,
	@NOMBRE			VARCHAR(50)	=	NULL,
	@APELLIDOS		VARCHAR(50)	=	NULL,
	@TELEFONO		VARCHAR(20)	=	NULL,
	@TIPO_TEL		INT			=	NULL,
	@EMAIL			VARCHAR(50)	=	NULL
		
		
AS
BEGIN
	/* Returns the set of affected rows */
	SET NOCOUNT ON;

	

	DECLARE @ERRORS			TABLE(	[ERROR_MESSAGE]	VARCHAR(50))

	DECLARE @INFO_CONTACTO	TABLE (ID_CONTACTO	INT,
									NOMBRE		VARCHAR(50)	,
									APELLIDOS	VARCHAR(50),
									TELEFONO	VARCHAR(20),
									TIPO_TEL	INT			,
									EMAIL		VARCHAR(50),
									DESC_TIPO	VARCHAR(20)
									)

	

	/*Valida si trae una acción a ejecutar en caso contrario registra error*/
	IF @OPERATION NOT IN (0,1,2,3,4,5)
	BEGIN
		INSERT INTO @ERRORS ([ERROR_MESSAGE]) VALUES ('SE REQUIERE UN TIPO DE ACCION A EJECUTAR')
		SELECT [ERROR_MESSAGE] FROM @ERRORS
		RETURN
	END

	/*-----------------------------------------------------------------------------------------------
	-- REGISTRA UN NUEVO CONTACTO
	-----------------------------------------------------------------------------------------------*/
	IF (@OPERATION IN (1))
	BEGIN 
		
		IF (SELECT TOP 1 Nombre FROM TBL_CONTACTO WHERE Telefono = @TELEFONO ) IS NOT NULL
		BEGIN
			INSERT INTO @ERRORS ([ERROR_MESSAGE]) VALUES ('EL NUMERO TELEFONICO A REGISTRAR YA EXISTE')
			SELECT [ERROR_MESSAGE] FROM @ERRORS
			RETURN
		END

		INSERT INTO [dbo].[TBL_CONTACTO]
           ([Nombre]
           ,[Apellidos]
           ,[Telefono]
           ,[TipoTel]
           ,[Email]
           ,[FechaCracion])
		 VALUES
			   (@NOMBRE
			   ,@APELLIDOS
			   ,@TELEFONO
			   ,@TIPO_TEL
			   ,@EMAIL
			   ,GETDATE())
			
			DECLARE @IDRNT INT  = @@ROWCOUNT

			INSERT INTO @INFO_CONTACTO (ID_CONTACTO,NOMBRE,APELLIDOS,TELEFONO,TIPO_TEL,EMAIL)
			SELECT @IDRNT,
					@NOMBRE,
					@APELLIDOS,
					@TELEFONO,
					@TIPO_TEL,
					@EMAIL
	END

	/*-----------------------------------------------------------------------------------------------
	-- ACTUALIZA UN NUEVO CONTACTO
	-----------------------------------------------------------------------------------------------*/
	IF (@OPERATION IN (2))
	BEGIN 

		IF (SELECT TOP 1 Nombre FROM TBL_CONTACTO WHERE IdContacto = @ID_CONTACTO ) IS  NULL
		BEGIN
			INSERT INTO @ERRORS ([ERROR_MESSAGE]) VALUES ('EL CONTACTO NO EXISTE')
			SELECT [ERROR_MESSAGE] FROM @ERRORS
			RETURN
		END

		UPDATE [dbo].[TBL_CONTACTO]
		   SET [Nombre] = @NOMBRE
			  ,[Apellidos] = @APELLIDOS
			  ,[Telefono] = @TELEFONO
			  ,[TipoTel] = @TIPO_TEL
			  ,[Email] = @EMAIL
		WHERE IdContacto = @ID_CONTACTO

	END

	/*-----------------------------------------------------------------------------------------------
	-- ELIMINA UN NUEVO CONTACTO
	-----------------------------------------------------------------------------------------------*/
	IF (@OPERATION IN (3))
	BEGIN 

		IF (SELECT TOP 1 Nombre FROM TBL_CONTACTO WHERE IdContacto = @ID_CONTACTO ) IS  NULL
		BEGIN
			INSERT INTO @ERRORS ([ERROR_MESSAGE]) VALUES ('EL CONTACTO NO EXISTE')
			SELECT [ERROR_MESSAGE] FROM @ERRORS
			RETURN
		END
		ELSE
		BEGIN 
			DELETE[dbo].[TBL_CONTACTO]
			WHERE IdContacto = @ID_CONTACTO
		END
		
		SELECT [ERROR_MESSAGE] FROM @ERRORS


	END

	/*-----------------------------------------------------------------------------------------------
	-- CONSULTA UN NUEVO CONTACTO
	-----------------------------------------------------------------------------------------------*/
	IF (@OPERATION IN (4))
	BEGIN 
		IF (SELECT TOP 1 Nombre FROM TBL_CONTACTO WHERE IdContacto = @ID_CONTACTO ) IS  NULL
		BEGIN
			INSERT INTO @ERRORS ([ERROR_MESSAGE]) VALUES ('EL CONTACTO NO EXISTE')
			SELECT [ERROR_MESSAGE] FROM @ERRORS
			RETURN
		END


		INSERT INTO  @INFO_CONTACTO	(ID_CONTACTO,
									NOMBRE,
									APELLIDOS,
									TELEFONO,
									TIPO_TEL,
									EMAIL,
									DESC_TIPO)
		SELECT C.IdContacto
		  ,isnull(C.Nombre,'')
		  ,isnull(C.Apellidos,'')
		  ,isnull(C.Telefono,'')
		  ,isnull(C.TipoTel,'')
		  ,isnull(C.Email,'')
		  ,TT.Nombre
	  FROM [dbo].[TBL_CONTACTO] C
		INNER JOIN TBL_TIPO_TELEFONO TT
			ON	C.TipoTel = TT.IdTipoTel
		WHERE IdContacto = @ID_CONTACTO

		SELECT * FROM @INFO_CONTACTO

	END

	/*-----------------------------------------------------------------------------------------------
	-- CONSULTA TODOS LOS CONTACTOS 
	-----------------------------------------------------------------------------------------------*/
	IF (@OPERATION IN (5))
	BEGIN 

		SELECT C.IdContacto
		  ,isnull(C.Nombre,'')
		  ,isnull(C.Apellidos,'')
		  ,isnull(C.Telefono,'')
		  ,isnull(C.TipoTel,'')
		  ,isnull(C.Email,'')
		  ,TT.Nombre
	  FROM [dbo].[TBL_CONTACTO] C
		INNER JOIN TBL_TIPO_TELEFONO TT
			ON	C.TipoTel = TT.IdTipoTel

	END
	
	/*------------------------------------------------------------------
	--	PRINT RESULTS
	------------------------------------------------------------------*/
	IF (@OPERATION IN (1,2))
	BEGIN 
		
		SELECT [ERROR_MESSAGE] FROM @ERRORS
		SELECT * FROM @INFO_CONTACTO
	END
END

/************************************************************************************************************************************/
GO
/****** Object:  StoredProcedure [dbo].[CRUD_TIPO_TEL]    Script Date: 19/06/2020 11:12:53 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/**		 
	 * PROCEDURE : CRUD_TIPO_TEL		
	 * DESCRIPCION : ADMINISTRA EL CATALOGO DE TIPOS DE TELEFONO 
	 * AUTOR  : David Montes		
	 * FECHA  : 19/06/20
	 * MODIFICACIONES:		 
	**/
CREATE PROCEDURE [dbo].[CRUD_TIPO_TEL]

	@OPERATION		INT			=	0 
AS
BEGIN
	/* Returns the set of affected rows */
	SET NOCOUNT ON; 
	

	/*-----------------------------------------------------------------------------------------------
	-- CONSULTA TODOS LOS TIPOS DE TELEFONO
	-----------------------------------------------------------------------------------------------*/
	IF (@OPERATION IN (5))
	BEGIN 

		SELECT [IdTipoTel]
		  ,[Nombre]
	  FROM [dbo].[TBL_TIPO_TELEFONO]
	  WHERE Estatus = 1

	END
	
	
END

/************************************************************************************************************************************/
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificar único del contacto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_CONTACTO', @level2type=N'COLUMN',@level2name=N'IdContacto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nombre del contacot ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_CONTACTO', @level2type=N'COLUMN',@level2name=N'Nombre'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Apellidos del cotacto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_CONTACTO', @level2type=N'COLUMN',@level2name=N'Apellidos'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Numero telefonico' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_CONTACTO', @level2type=N'COLUMN',@level2name=N'Telefono'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo de teléfono' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_CONTACTO', @level2type=N'COLUMN',@level2name=N'TipoTel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Correo electrónico' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_CONTACTO', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha en que se registro el contacto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_CONTACTO', @level2type=N'COLUMN',@level2name=N'FechaCracion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificador único del catálogo Tipo de teléfono' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TIPO_TELEFONO', @level2type=N'COLUMN',@level2name=N'IdTipoTel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nombre del tipo teléfono' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TIPO_TELEFONO', @level2type=N'COLUMN',@level2name=N'Nombre'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción del  tipo teléfono' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TIPO_TELEFONO', @level2type=N'COLUMN',@level2name=N'Descripcion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Estatus del tipo teléfono' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TIPO_TELEFONO', @level2type=N'COLUMN',@level2name=N'Estatus'
GO
INSERT INTO [dbo].[TBL_TIPO_TELEFONO]([IdTipoTel],[Nombre],[Descripcion],[Estatus])
     VALUES
           (1,'Celular','Celular',1)
		   INSERT INTO [dbo].[TBL_TIPO_TELEFONO]([IdTipoTel],[Nombre],[Descripcion],[Estatus])
     VALUES
           (2,'Casa','Casa',1)
		   INSERT INTO [dbo].[TBL_TIPO_TELEFONO]([IdTipoTel],[Nombre],[Descripcion],[Estatus])
     VALUES
           (3,'Iphone','Iphone',1)
		   INSERT INTO [dbo].[TBL_TIPO_TELEFONO]([IdTipoTel],[Nombre],[Descripcion],[Estatus])
     VALUES
           (4,'Oficina','Oficina',1)
GO

INSERT INTO [dbo].[TBL_CONTACTO]
([Nombre]
           ,[Apellidos]
           ,[Telefono]
           ,[TipoTel]
           ,[Email]
           ,[FechaCracion])
     VALUES
           ('David'
           ,'Montes Rodriguez'
           ,'5534443491'
           ,3
           ,'davidmr_1312@hotmail.com'
           ,GETDATE())
           
   GO
   
