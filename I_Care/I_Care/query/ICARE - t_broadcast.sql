USE FMS

alter table t_broadcast add BroadCastType int 
alter table t_broadcast add Day1 bit
alter table t_broadcast add Day2 bit
alter table t_broadcast add Day3 bit
alter table t_broadcast add Day4 bit
alter table t_broadcast add Day5 bit
alter table t_broadcast add Day6 bit
alter table t_broadcast add Day7 bit
alter table t_broadcast add IsRepeat bit
GO



ALter PROCEDURE [dbo].[procGetBroadcastList](
@Field VARCHAR(100),
@Keyword VARCHAR(200),
@Username VARCHAR(100)
)
AS
BEGIN
SET NOCOUNT ON;
DECLARE
@AllowAllAction INT,
@AllowApprove INT,
@AllowReject INT,
@AllowDelete INT,
@AllowResubmit INT

SET @AllowAllAction = ( SELECT SSO.dbo.fnGetExtendAcessModul(@Username, 163))
SET @AllowApprove = ( SELECT SSO.dbo.fnGetExtendAcessModul(@Username, 164))


CREATE TABLE [dbo].#temp_broadcast(
	[Id] [INT] NOT NULL,
	[Title] [VARCHAR](50) NULL,
	[Fungsi] [VARCHAR](50) NULL,
	[Descriptions] [VARCHAR](50) NULL,
	[EmbededLink] [VARCHAR](MAX) NULL,
	[Status] [INT] NULL,
	[StartDate] [DATE] NULL,
	[EndDate] [DATE] NULL,
	BroadcastType [int], Day1 [bit], Day2  [bit], Day3  [bit], Day4  [bit], Day5  [bit], Day6  [bit], Day7  [bit], IsRepeat  [bit],
	[CreateDate] [DATETIME] NULL,
	[CreateUser] [VARCHAR](50) NULL,
	[UpdateDate] [DATETIME] NULL,
	[UpdateUser] [VARCHAR](50) NULL,
	[IsDeleted] [INT] NULL,
	[SendStatus] INT NULL
)




IF @AllowAllAction = 1 
BEGIN

	INSERT INTO #temp_broadcast
	(	Id,
	    Title,
	    Fungsi,
	    Descriptions,
		[EmbededLink],
	    Status,
	    StartDate,
	    EndDate,
		BroadcastType, Day1, Day2, Day3, Day4, Day5, Day6, Day7, IsRepeat,
	    CreateDate,
	    CreateUser,
	    UpdateDate,
	    UpdateUser,
	    IsDeleted,
		SendStatus
	)
	SELECT 
		Id,
		Title,
	    Fungsi,
	    Descriptions,
		[EmbededLink],
	    Status,
	    StartDate,
	    EndDate,
		BroadcastType, Day1, Day2, Day3, Day4, Day5, Day6, Day7, IsRepeat,
	    CreateDate,
	    CreateUser,
	    UpdateDate,
	    UpdateUser,
	    IsDeleted,
		SendStatus
	FROM FMS.dbo.t_broadcast  

END
ELSE
BEGIN

	INSERT INTO #temp_broadcast
	(
		Id,
	    Title,
	    Fungsi,
	    Descriptions,
		[EmbededLink],
	    Status,
	    StartDate,
	    EndDate,
		BroadcastType, Day1, Day2, Day3, Day4, Day5, Day6, Day7, IsRepeat,
	    CreateDate,
	    CreateUser,
	    UpdateDate,
	    UpdateUser,
	    IsDeleted,
		SendStatus
	)
	SELECT 
		Id,
		Title,
	    Fungsi,
	    Descriptions,
		[EmbededLink],
	    Status,
	    StartDate,
	    EndDate,
		BroadcastType, Day1, Day2, Day3, Day4, Day5, Day6, Day7, IsRepeat,
	    CreateDate,
	    CreateUser,
	    UpdateDate,
	    UpdateUser,
	    IsDeleted,
		SendStatus
		
	FROM dbo.t_broadcast  WHERE IsDeleted = 0 AND CreateUser = @Username

	IF @AllowApprove = 1
	BEGIN 

	INSERT INTO #temp_broadcast
	(
		Id,
	    Title,
	    Fungsi,
	    Descriptions,
		[EmbededLink],
	    Status,
	    StartDate,
	    EndDate,
		BroadcastType, Day1, Day2, Day3, Day4, Day5, Day6, Day7, IsRepeat,
	    CreateDate,
	    CreateUser,
	    UpdateDate,
	    UpdateUser,
	    IsDeleted,
		SendStatus
	)
	SELECT 
		Id,
		Title,
	    Fungsi,
	    Descriptions,
		[EmbededLink],
	    Status,
	    StartDate,
	    EndDate,
		BroadcastType, Day1, Day2, Day3, Day4, Day5, Day6, Day7, IsRepeat,
	    CreateDate,
	    CreateUser,
	    UpdateDate,
	    UpdateUser,
	    IsDeleted,
		SendStatus
		
	FROM FMS.dbo.t_broadcast  WHERE IsDeleted = 0  AND [Status] = 1
	AND Id NOT IN ( SELECT Id FROM #temp_broadcast) 


	END

END



	IF @Field = 'All'
	BEGIN

		SELECT A.Id AS recid,
		A.Id,
		A.Title,
		A.Fungsi,
		F.NM_UNIT_ORG FungsiName,
		A.Descriptions,
		A.Status,
		CONVERT(VARCHAR(10),A.StartDate,23) AS StartDate,
		CONVERT(VARCHAR(10),A.EndDate,23) AS EndDate,
		BroadcastType, 
		c.ReminderName BroadcastTypeName,
		Day1, Day2, Day3, Day4, Day5, Day6, Day7, IsRepeat,
		BroadcastType, Day1, Day2, Day3, Day4, Day5, Day6, Day7, IsRepeat,
		B.ProcName,
		B.ProcTag,
		A.CreateDate,
		'Create By. '+ A.CreateUser + ' On: '+ convert(varchar, A.CreateDate, 9) AS Creator,
		'Update By. '+ A.UpdateUser + ' On: '+ convert(varchar, A.UpdateDate, 9) AS Updater,
		A.CreateUser,
		A.UpdateDate,
		A.UpdateUser,
		(DATEDIFF(day, A.[StartDate], A.[EndDate])) + 1 AS CountDay,
		A.SendStatus
		FROM #temp_broadcast AS A
		LEFT JOIN dbo.m_broadcast_process AS b
		ON A.Status = b.ProcId
		LEFT JOIN [SSO].[dbo].[M_Fungsi] F
		ON A.Fungsi = F.ID
		left join M_Reminder_Type c on c.Id = a.broadcasttype
		WHERE A.isDeleted = 0
		

	END
	ELSE
    BEGIN
		
		IF @Field = 'Title'
		BEGIN

			SELECT A.Id AS recid,
			A.Id,
			A.Title,
			A.Fungsi,
			F.NM_UNIT_ORG FungsiName,
			A.Descriptions,
			A.Status,
			CONVERT(VARCHAR(10),A.StartDate,23) AS StartDate,
			CONVERT(VARCHAR(10),A.EndDate,23) AS EndDate,
			  BroadcastType, 
			   c.ReminderName BroadcastTypeName,
			   Day1, Day2, Day3, Day4, Day5, Day6, Day7, IsRepeat,
			B.ProcName,
			B.ProcTag,
			A.CreateDate,
			'Create By. '+ A.CreateUser + ' On: '+ convert(varchar, A.CreateDate, 9) AS Creator,
			'Update By. '+ A.UpdateUser + ' On: '+ convert(varchar, A.UpdateDate, 9) AS Updater,
			A.CreateUser,
			A.UpdateDate,
			A.UpdateUser,
			(DATEDIFF(day, A.[StartDate], A.[EndDate])) + 1 AS CountDay,
			A.SendStatus
			FROM #temp_broadcast AS A
			LEFT JOIN dbo.m_broadcast_process AS b
			ON A.Status = b.ProcId
			LEFT JOIN [SSO].[dbo].[M_Fungsi] F
			ON A.Fungsi = F.ID
			left join M_Reminder_Type c on c.Id = a.broadcasttype
			WHERE A.isDeleted = 0
			AND a.Title LIKE '%'+@Keyword+'%'
			AND A.CreateUser = @Username

		END

		IF @Field = 'Descriptions'
		BEGIN

			SELECT A.Id AS recid,
			A.Id,
			A.Title,
			A.Fungsi,
			F.NM_UNIT_ORG FungsiName,
			A.Descriptions,
			A.Status,
			CONVERT(VARCHAR(10),A.StartDate,23) AS StartDate,
			CONVERT(VARCHAR(10),A.EndDate,23) AS EndDate,
			  BroadcastType, 
			   c.ReminderName BroadcastTypeName,
			   Day1, Day2, Day3, Day4, Day5, Day6, Day7, IsRepeat,
			B.ProcName,
			B.ProcTag,
			A.CreateDate,
			'Create By. '+ A.CreateUser + ' On: '+ convert(varchar, A.CreateDate, 9) AS Creator,
			'Update By. '+ A.UpdateUser + ' On: '+ convert(varchar, A.UpdateDate, 9) AS Updater,
			A.CreateUser,
			A.UpdateDate,
			A.UpdateUser,
			(DATEDIFF(day, A.[StartDate], A.[EndDate])) + 1 AS CountDay,
			A.SendStatus
			FROM #temp_broadcast AS A
			LEFT JOIN dbo.m_broadcast_process AS b
			ON A.Status = b.ProcId
			LEFT JOIN [SSO].[dbo].[M_Fungsi] F
			ON A.Fungsi = F.ID
			left join M_Reminder_Type c on c.Id = a.broadcasttype
			WHERE A.isDeleted = 0
			AND a.Descriptions LIKE '%'+@Keyword+'%'
			AND A.CreateUser = @Username

		END

	END

	DROP TABLE #temp_broadcast

END
	

GO

/****** Object:  StoredProcedure [dbo].[procGetBroadcastListByStatus]    Script Date: 25/08/2023 08:05:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALter PROCedure [dbo].[procGetBroadcastListByStatus]
(
    @Status INT,
    @Username VARCHAR(100)
)
AS
SET NOCOUNT ON;
BEGIN


DECLARE
@AllowAllAction INT,
@AllowApprove INT,
@AllowReject INT,
@AllowDelete INT,
@AllowResubmit INT

SET @AllowAllAction = ( SELECT SSO.dbo.fnGetExtendAcessModul(@Username, 163))
SET @AllowApprove = ( SELECT SSO.dbo.fnGetExtendAcessModul(@Username, 164))


CREATE TABLE [dbo].#temp_broadcast(
	[Id] [INT] NOT NULL,
	[Title] [VARCHAR](50) NULL,
	[Fungsi] [VARCHAR](50) NULL,
	[Descriptions] [VARCHAR](50) NULL,
	[EmbededLink] [VARCHAR](MAX) NULL,
	[Status] [INT] NULL,
	[StartDate] [DATE] NULL,
	[EndDate] [DATE] NULL,
	BroadcastType [int], Day1 [bit], Day2 [bit], Day3 [bit], Day4 [bit], Day5 [bit], Day6 [bit], Day7 [bit], IsRepeat [bit],
	[CreateDate] [DATETIME] NULL,
	[CreateUser] [VARCHAR](50) NULL,
	[UpdateDate] [DATETIME] NULL,
	[UpdateUser] [VARCHAR](50) NULL,
	[IsDeleted] [INT] NULL,
	[SendStatus] INT NULL
)




IF @AllowAllAction = 1 
BEGIN

	INSERT INTO #temp_broadcast
	(	Id,
	    Title,
	    Fungsi,
	    Descriptions,
		[EmbededLink],
	    Status,
	    StartDate,
	    EndDate,
		BroadcastType, Day1, Day2, Day3, Day4, Day5, Day6, Day7, IsRepeat,
	    CreateDate,
	    CreateUser,
	    UpdateDate,
	    UpdateUser,
	    IsDeleted,
		SendStatus
	)
	SELECT 
		Id,
		Title,
	    Fungsi,
	    Descriptions,
		[EmbededLink],
	    Status,
	    StartDate,
	    EndDate,
		BroadcastType, Day1, Day2, Day3, Day4, Day5, Day6, Day7, IsRepeat,
	    CreateDate,
	    CreateUser,
	    UpdateDate,
	    UpdateUser,
	    IsDeleted,
		SendStatus
	FROM FMS.dbo.t_broadcast  

END
ELSE
BEGIN

	INSERT INTO #temp_broadcast
	(
		Id,
	    Title,
	    Fungsi,
	    Descriptions,
		[EmbededLink],
	    Status,
	    StartDate,
	    EndDate,
		BroadcastType, Day1, Day2, Day3, Day4, Day5, Day6, Day7, IsRepeat,
	    CreateDate,
	    CreateUser,
	    UpdateDate,
	    UpdateUser,
	    IsDeleted,
		SendStatus
	)
	SELECT 
		Id,
		Title,
	    Fungsi,
	    Descriptions,
		[EmbededLink],
	    Status,
	    StartDate,
	    EndDate,
		BroadcastType, Day1, Day2, Day3, Day4, Day5, Day6, Day7, IsRepeat,
	    CreateDate,
	    CreateUser,
	    UpdateDate,
	    UpdateUser,
	    IsDeleted,
		SendStatus
		
	FROM dbo.t_broadcast  WHERE IsDeleted = 0 AND CreateUser = @Username

	IF @AllowApprove = 1
	BEGIN 

	INSERT INTO #temp_broadcast
	(
		Id,
	    Title,
	    Fungsi,
	    Descriptions,
		[EmbededLink],
	    Status,
	    StartDate,
	    EndDate,
		BroadcastType, Day1, Day2, Day3, Day4, Day5, Day6, Day7, IsRepeat,
	    CreateDate,
	    CreateUser,
	    UpdateDate,
	    UpdateUser,
	    IsDeleted,
		SendStatus
	)
	SELECT 
		Id,
		Title,
	    Fungsi,
	    Descriptions,
		[EmbededLink],
	    Status,
	    StartDate,
	    EndDate,
		BroadcastType, Day1, Day2, Day3, Day4, Day5, Day6, Day7, IsRepeat,
	    CreateDate,
	    CreateUser,
	    UpdateDate,
	    UpdateUser,
	    IsDeleted,
		SendStatus
		
	FROM FMS.dbo.t_broadcast  WHERE IsDeleted = 0  AND [Status] = 1
	AND Id NOT IN ( SELECT Id FROM #temp_broadcast) 


	END

END

	IF  @Status = 1000
	BEGIN
   
		SELECT A.Id AS recid,
			   A.Id,
			   A.Title,
			   A.Fungsi,
			   F.NM_UNIT_ORG FungsiName,
			   A.Descriptions,
			   A.[EmbededLink],
			   A.[Status],
			   CONVERT(VARCHAR(10),A.StartDate,23) AS StartDate,
			   CONVERT(VARCHAR(10),A.EndDate,23) AS EndDate,  
			   BroadcastType, 
			   c.ReminderName BroadcastTypeName,
			   Day1, Day2, Day3, Day4, Day5, Day6, Day7, IsRepeat,
			   B.ProcName,
			   B.ProcTag,
			   A.CreateDate,
			   'Create By. '+ A.CreateUser + ' On: '+ convert(varchar, A.CreateDate, 9) AS Creator,
				'Update By. '+ A.UpdateUser + ' On: '+ convert(varchar, A.UpdateDate, 9) AS Updater,
			   A.CreateUser,
			   A.UpdateDate,
			   A.UpdateUser,
			   (DATEDIFF(day, A.[StartDate], A.[EndDate])) + 1 AS CountDay,
			   A.SendStatus
		FROM dbo.#temp_broadcast AS A
			LEFT JOIN dbo.m_broadcast_process AS b
				ON A.[Status] = b.ProcId
			LEFT JOIN [SSO].[dbo].[M_Fungsi] F
					ON A.Fungsi = F.ID
			left join M_Reminder_Type c on c.Id = a.broadcasttype
		

	END
	ELSE
    BEGIN
    
		SELECT A.Id AS recid,
			   A.Id,
			   A.Title,
			   A.Fungsi,
			   F.NM_UNIT_ORG FungsiName,
			   A.Descriptions,
			   A.[EmbededLink],
			   A.[Status],
			   CONVERT(VARCHAR(10),A.StartDate,23) AS StartDate,
			   CONVERT(VARCHAR(10),A.EndDate,23) AS EndDate,
			   BroadcastType, 
			   c.ReminderName BroadcastTypeName,
			   Day1, Day2, Day3, Day4, Day5, Day6, Day7, IsRepeat,
			   B.ProcName,
			   B.ProcTag,
			   A.CreateDate,
			   'Create By. '+ A.CreateUser + ' On: '+ convert(varchar, A.CreateDate, 9) AS Creator,
				'Update By. '+ A.UpdateUser + ' On: '+ convert(varchar, A.UpdateDate, 9) AS Updater,
			   A.CreateUser,
			   A.UpdateDate,
			   A.UpdateUser,
			  (DATEDIFF(day, A.[StartDate], A.[EndDate])) + 1 AS CountDay,
			   A.SendStatus
		FROM dbo.#temp_broadcast AS A
			LEFT JOIN dbo.m_broadcast_process AS b
				ON A.[Status] = b.ProcId
			LEFT JOIN [SSO].[dbo].[M_Fungsi] F
					ON A.Fungsi = F.ID
			
			left join M_Reminder_Type c on c.Id = a.broadcasttype
			WHERE  A.[Status] = @Status
	END

	DROP table #temp_broadcast

END;

GO

/****** Object:  StoredProcedure [dbo].[procSaveBroadcast]    Script Date: 25/08/2023 08:05:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALter  PROCEDURE [dbo].[procSaveBroadcast]
(
    @FlagCrud VARCHAR(20),
	@BroadcastId INT =NULL,
    @Title VARCHAR(50),
    @Fungsi VARCHAR(10),
	@StartDate VARCHAR(10),
	@EndDate VARCHAR(10),
    @Descriptions VARCHAR(50),
    @FileUploadList TEXT = NULL,
	@Remark VARCHAR(MAX) = NULL,
	@Tautan VARCHAR(MAX) = NULL,
	@BroadcastType int=0,
	@Day1 bit =0,
	@Day2 bit =0,
	@Day3 bit =0,
	@Day4 bit =0,
	@Day5 bit =0,
	@Day6 bit =0,
	@Day7 bit =0,
	@IsRepeat bit =0,
    @Username VARCHAR(50),
    @outputVal VARCHAR(800) OUTPUT
)
AS
BEGIN

    DECLARE @COUNT INT,
            @LastId INT;
	IF @Tautan = ''
	BEGIN
    SET @Tautan = NULL;
	END

    BEGIN TRY

        IF @FlagCrud = 'ADD'
        BEGIN
			INSERT INTO t_broadcast
			(
			Title,
			Fungsi,
			Descriptions,
			[Status],
			isDeleted,
			StartDate,
			EndDate,
			BroadcastType,
			Day1,
			Day2,
			Day3,
			Day4,
			Day5,
			Day6,
			Day7,
			IsRepeat,
			CreateDate,
			CreateUser,
			EmbededLink
			)
			VALUES
			(@Title, @Fungsi, @Descriptions, 1, 0,@StartDate,@EndDate,@BroadcastType,@Day1,@Day2,@Day3,@Day4,@Day5,@Day6,@Day7,@IsRepeat, GETDATE(), @Username,@Tautan);
			SET @LastId = SCOPE_IDENTITY();


			--maping attachment
			SELECT
				 JSON_VALUE([value], '$.ID') AS [ID]
			   INTO #temp_mapping_attachment
			FROM OPENJSON(@FileUploadList)

			--update key2 memjadi ContractId, DocumentCatId = Lampiran
			update FMS.dbo.T_DocumentFile 
			set 
			 Key2 = @LastId
			 --DocumentCatID = 'Lampiran-E-Kontrak'
			where ID IN (SELECT ID FROM #temp_mapping_attachment )

			DROP TABLE #temp_mapping_attachment


			--save log
			INSERT INTO dbo.t_log_broadcast
			(
			    BroadcastId,
			    FlowFrom,
			    FlowTo,
			    Descriptions,
			    CreateDate,
			    CreateUser
			)
			VALUES
			(   @LastId,         -- BroadcastId - int
			    200,         -- FlowFrom - int
			    1,         -- FlowTo - int
			    'One broadcast submited',        -- Descriptions - varchar(max)
			    GETDATE(), -- CreateDate - datetime
			    @Username         -- CreateUser - varchar(100)
			    )


			SET @outputVal = @LastId;
        END;

		IF @FlagCrud = 'VIEW'
        BEGIN
			
			DECLARE @LastStatus INT,@NextStep int
			SET @LastStatus = (SELECT TOP 1 [Status] FROM dbo.t_broadcast WHERE Id = @BroadcastId)
			SET @NextStep = (SELECT TOP  1 NextStep FROM dbo.m_broadcast_process WHERE ProcId = @LastStatus)

			IF @NextStep = 1
			BEGIN
            
				UPDATE dbo.t_broadcast
				SET Title = @Title,
					Fungsi = @Fungsi,
					Descriptions = @Descriptions,
					[Status] = @NextStep,
					EmbededLink = @Tautan,
					BroadcastType = @BroadcastType,
					Day1 = @Day1,
					Day2 = @Day2,
					Day3 = @Day3,
					Day4 = @Day4,
					Day5 = @Day5,
					Day6 = @Day6,
					Day7 = @Day7,
					IsRepeat = @IsRepeat,
					UpdateDate = GETDATE(),
					UpdateUser = @Username
				WHERE Id = @BroadcastId

				--maping attachment
				SELECT
				JSON_VALUE([value], '$.ID') AS [ID]
				INTO #temp_mapping_attachment_2
				FROM OPENJSON(@FileUploadList)

				--update key2 memjadi ContractId, DocumentCatId = Lampiran
				update FMS.dbo.T_DocumentFile 
				set 
				Key2 = @BroadcastId
				--DocumentCatID = 'Lampiran-E-Kontrak'
				where ID IN (SELECT ID FROM #temp_mapping_attachment_2 )

				DROP TABLE #temp_mapping_attachment_2


				--save log
				INSERT INTO dbo.t_log_broadcast
				(
				BroadcastId,
				FlowFrom,
				FlowTo,
				Descriptions,
				CreateDate,
				CreateUser
				)
				VALUES
				(   @BroadcastId,         -- BroadcastId - int
				@LastStatus,         -- FlowFrom - int
				@NextStep,         -- FlowTo - int
				'Broadcast Resubmit',        -- Descriptions - varchar(max)
				GETDATE(), -- CreateDate - datetime
				@Username         -- CreateUser - varchar(100)
				)

			END


			IF @NextStep = 2
			BEGIN
            
				UPDATE dbo.t_broadcast
				SET 
					[Status] = @NextStep,
					UpdateDate = GETDATE(),
					UpdateUser = @Username
				WHERE Id = @BroadcastId


				--save log
				INSERT INTO dbo.t_log_broadcast
				(
				BroadcastId,
				FlowFrom,
				FlowTo,
				Descriptions,
				CreateDate,
				CreateUser
				)
				VALUES
				(   @BroadcastId,         -- BroadcastId - int
				@LastStatus,         -- FlowFrom - int
				@NextStep,         -- FlowTo - int
				'Broadcast Approved',        -- Descriptions - varchar(max)
				GETDATE(), -- CreateDate - datetime
				@Username         -- CreateUser - varchar(100)
				)

			END


			
		

		END

		IF @FlagCrud = 'REJECT'
		BEGIN
        
				UPDATE dbo.t_broadcast
				SET 
				[Status] = 0,
				UpdateDate = GETDATE(),
				UpdateUser = @Username
				WHERE Id = @BroadcastId


				--save log
				INSERT INTO dbo.t_log_broadcast
				(
				BroadcastId,
				FlowFrom,
				FlowTo,
				Descriptions,
				CreateDate,
				CreateUser
				)
				VALUES
				(   
				@BroadcastId,         -- BroadcastId - int
				1,         -- FlowFrom - int
				0,         -- FlowTo - int
				@Remark,        -- Descriptions - varchar(max)
				GETDATE(), -- CreateDate - datetime
				@Username         -- CreateUser - varchar(100)
				)


		END


    END TRY
    BEGIN CATCH
        SET @outputVal = 'error :' + ERROR_MESSAGE();
    END CATCH;

END;


GO

