USE [FMS]
GO

/****** Object:  StoredProcedure [dbo].[procGetReminderList]    Script Date: 04/08/2023 15:44:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[procGetReminderList] @userlogin varchar(100), @code varchar(100)
, @combojson nvarchar(max)=null
, @filterjson nvarchar(max)=null
as
--declare @userlogin varchar(100) = 'dev.andre', @code varchar(100) ='t_reminder', @status varchar(100)
--, @combojson nvarchar(max)=
--'{"ReminderType" : {"combo":"M_reminder_type","keys":"Id","values": "ReminderName"}}'
--, @filterjson nvarchar(max)='{"ReminderRef1##like":"tes"}'

--declare @userlogin varchar(100) = 'dev.andre', @code varchar(100) ='t_reminder_mail', @status varchar(100)=''
--, @combojson nvarchar(max)=null
--, @findjson nvarchar(max)=
--'{"reminderid":"14"}'

--DECLARE
--@AllowAllAction INT,
--@AllowApprove INT,
--@AllowReject INT,
--@AllowDelete INT,
--@AllowResubmit INT

--SET @AllowAllAction = ( SELECT SSO.dbo.fnGetExtendAcessModul(@userlogin, 163))
--SET @AllowApprove = ( SELECT SSO.dbo.fnGetExtendAcessModul(@userlogin, 164))

declare @combo nvarchar(max)='', @comboselect nvarchar(max)=''
	if isnull(@combojson,'') != '' begin
	set @combo = 
		isnull(STUFF((
			SELECT distinct
			  char(10)+' '+concat('LEFT JOIN (select * from ', JSON_VALUE(js.[value], '$.combo'), ') as ', js.[key], '_combo ', 'on a.', js.[key], ' = ', js.[key], '_combo.', JSON_VALUE(js.[value], '$.keys')) 
			FROM OPENJSON(@combojson) js
			CROSS APPLY OPENJSON(js.[value]) j	
		for xml path('')), 1, 2, ''), '') 

	set @comboselect =
	isnull(STUFF((
		SELECT distinct ', '
		--+concat(js.[key], '_combo.', JSON_VALUE(js.[value], '$.keys'), ' [', js.[key] ,'_key]') + ', ' +
		+concat(js.[key], '_combo.', JSON_VALUE(js.[value], '$.values'), ' [', js.[key] ,'Name]')
		FROM OPENJSON(@combojson) js
		CROSS APPLY OPENJSON(js.[value]) j	
	for xml path('')), 1, 2, ''), '') 
end

declare @wherecondition nvarchar(max)=''
if isnull(@filterjson,'') != '' begin
	set @wherecondition = 
	isnull(STUFF((
		select 'and '+
			case when [key] like '%like%' then concat(replace([key],'__like', ' like '), '''%'+[value]+'%'' ')
			else concat([key], ' = ', ''''+[value]+''' ') end
		FROM OPENJSON(@filterjson) js
	for xml path('')), 1, 3, ''), '') 
	
	set @wherecondition = replace(@wherecondition, '##', '')
end

declare @strcol nvarchar(max)='', @col nvarchar(max)=''
set @strcol = 
'
set @col= 
 STUFF((
	 SELECT '', '' + 
			case when data_type = ''date'' then concat(''format('', concat(''a.'', quotename(COLUMN_NAME)), '',''''yyyy/MM/dd'''') '', quotename(COLUMN_NAME))
			else concat(''a.'', quotename(COLUMN_NAME)) end
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = '''+@code+'''
FOR XML PATH('''')), 1, 2, '''') 

'

EXEC sp_executesql @strcol, N'@col nvarchar(max) OUTPUT', @col = @col OUTPUT;
print  concat('col : ' ,@col)

declare @sqlstr nvarchar(max) = ''
set @sqlstr = 
'
SELECT 
ROW_NUMBER() OVER(ORDER BY a.createdate ASC) recid, 
'+@col+', 
''Create By. ''+ A.CreateUser + '' On: ''+ convert(varchar, A.CreateDate, 9) AS Creator
'+iif(isnull(@comboselect,'')<>'', ', '+@comboselect, '')+'
FROM '+@CODE+' a
 '+isnull(@COMBO,'')+'
where a.createuser = '''+@userlogin+'''
'+iif(isnull(@wherecondition,'')<>'','and '+@wherecondition,'') +'
'

print concat('combo : ' ,@combo)	
PRINT concat('query :' ,@SQLSTR)	

exec (@sqlstr)

GO

/****** Object:  StoredProcedure [dbo].[procgetreminderlistmail]    Script Date: 04/08/2023 15:44:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[procgetreminderlistmail]
as
select  a.Id, ReminderName, ReminderRef1, ReminderRef2, ReminderRef3, ReminderType, ReminderDateStart, ReminderDueDate, Remark, MailAddress, MailName 

from (
select * from T_Reminder where ReminderType = '1' and convert(date,getdate()) = ReminderDateStart
union
select * from T_Reminder where ReminderType = '2' and convert(date,getdate()) between ReminderDateStart and ReminderDueDate
union
select * from T_Reminder where ReminderType = '3' 
and convert(date,getdate()) between ReminderDateStart and ReminderDueDate
and ( 
	(datename(WEEKDAY, getdate()) = 'Monday'	and Day1 ='1' ) or
	(datename(WEEKDAY, getdate()) = 'Tuesday'	and Day2 ='1' ) or
	(datename(WEEKDAY, getdate()) = 'Wednesday' and Day3 ='1' ) or
	(datename(WEEKDAY, getdate()) = 'Thursday'	and Day4 ='1' ) or
	(datename(WEEKDAY, getdate()) = 'Friday'	and Day5 ='1' ) or
	(datename(WEEKDAY, getdate()) = 'Saturday'	and Day6 ='1' ) or
	(datename(WEEKDAY, getdate()) = 'Sunday'	and Day7 ='1' ) 
)
) a
inner join T_Reminder_Mail b on b.ReminderId = a.id
GO

/****** Object:  StoredProcedure [dbo].[procSaveReminder]    Script Date: 04/08/2023 15:44:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[procSaveReminder] @userlogin varchar(100), @code varchar(20), @mode  varchar(20), @header NVARCHAR(MAX), @detail  nvarchar(max) 
, @formkey varchar(200), @formkeyvalue varchar(200)
as

--DECLARE @userlogin varchar(100) = 'dev.andre', @code varchar(20)='t_reminder', @mode varchar(20)='update'
--, @header NVARCHAR(MAX) = '{"ReminderName":"asdasddaD","ReminderRef1":"tyrtytrytryrt","ReminderRef2":"","ReminderRef3":"","ReminderType":"1","ReminderDateStart":"2023-07-08T00:00:00","ReminderDueDate":"2023-07-15T00:00:00","Day1":"0","Day2":"0","Day3":"0","Day4":"0","Day5":"0","Day6":"0","Day7":"0","IsRepeat":"0","Remark":"asdasdsadad"}'
--, @detail nvarchar(max) = 
--'{"t_reminder_mail":[{"recid":1,"Id":4,"ReminderId":"14","MailAddress":"asdas","MailName":"asdasdada","IsDeleted":0,"CreateDate":null,"CreateUser":"dev.andre","Creator":null},{"recid":2,"Id":5,"ReminderId":"14","MailAddress":"bbb","MailName":"bbbb","IsDeleted":0,"CreateDate":null,"CreateUser":"dev.andre","Creator":null}],"t_reminder_mail_parentkey":"ReminderId","t_reminder_mail_primarykey":"Id"}
--',
--@formkey varchar(200) = 'Id', @formkeyvalue varchar(200) ='14'
	

declare @sqlstr nvarchar(max)='', @insertField nvarchar(max)='', @insertStr nvarchar(max)='', @updateStr  nvarchar(max)=''
, @primarykey nvarchar(50), @primarykeyvalue nvarchar(50)
declare @id nvarchar(100)=''
------------------------------------------------------/*ADD*/
  if @mode = 'add'  begin
		set @insertField = 
				isnull(STUFF((
				SELECT  ', ['+replace(cast([key] as nvarchar(50)), '-', '')+']'
				FROM OPENJSON(@header)
				for xml path('')), 1, 2, ''),'')

		set @insertStr = 
			isnull(STUFF((
			SELECT  ', '''+replace(cast([value] as nvarchar(50)), '-', '')+''''
			FROM OPENJSON(@header)
			for xml path('')), 1, 2, ''), '') 
	
		print @insertfield
		print @insertstr

		select @sqlstr=
		'
		insert into '+@code+' ('+@insertfield+',isdeleted , createdate, createuser)'+char(10)+'
		values (' + @insertStr +', 0, getdate(), '''+@userlogin+''')'+char(10)+'
		SET @id = SCOPE_IDENTITY();


		'

		--select @sqlstr
		declare @msg nvarchar(max)=''
   		
		BEGIN TRY 
			
			EXEC sp_executesql @sqlstr, N'@id nvarchar(100) OUTPUT', @id = @id OUTPUT;
			print @sqlstr
			declare @query nvarchar(max) = ''

			exec dbo.genSaveData_child @userlogin, @detail, @id, @query = @query output

			exec (@query)

			select @msg = 'success' 

			
		END TRY  
		BEGIN CATCH  
			select @msg= 'error :'+ ERROR_MESSAGE() 
		END CATCH;

		select @msg msg,  @id id
	end
------------------------------------------------------/*ADD*/
------------------------------------------------------/*UPDATE*/
else  if @mode = 'update'  begin
  	set @updateStr = STUFF((
		SELECT  ', '+'['+replace(cast([key] as nvarchar(50)), '-', '')+'] = '''+cast([value] as nvarchar(max))+''''+char(10)
		FROM OPENJSON(@header)
		for xml path('')), 1, 2, '')

		select @sqlstr = 
		'update ' + @code + ' set ' + @updateStr +', updatedate=getdate(),updateuser='''+@userlogin+''''
		+ ' where ' + @formkey + ' = ''' + CAST(@formkeyvalue as varchar(500)) + ''''

	BEGIN TRY 
		print @sqlstr
		exec (@sqlstr) 

		select  @Id = @formkeyvalue
		
		exec dbo.genSaveData_child @userlogin, @detail, @id, @query = @query output

		exec (@query)
		
		select @msg = 'success' 
	END TRY  
	BEGIN CATCH  
		select ERROR_MESSAGE() AS msg; 
	END CATCH;   

	select @msg msg,  @id id, @query q
  end
  
GO


