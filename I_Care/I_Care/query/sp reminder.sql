USE [FMS]
GO

/****** Object:  StoredProcedure [dbo].[genGetdata]    Script Date: 03/08/2023 15:04:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[genGetdata] @userlogin varchar(100), @code varchar(100)
, @combojson nvarchar(max)=null
, @filterjson nvarchar(max)=null
as
--declare @userlogin varchar(100) = 'dev.andre', @code varchar(100) ='t_reminder', @status varchar(100)
--, @combojson nvarchar(max)=
--'{"ReminderType" : {"combo":"M_reminder_type","keys":"Id","values": "ReminderName"}}'
--, @filterjson nvarchar(max)=null

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
		select 'and '+concat([key], ' = ', ''''+[value]+''' ')
		FROM OPENJSON(@filterjson) js
	for xml path('')), 1, 3, ''), '') 
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

/****** Object:  StoredProcedure [dbo].[genSaveData]    Script Date: 03/08/2023 15:04:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[genSaveData] @userlogin varchar(100), @code varchar(20), @mode  varchar(20), @header NVARCHAR(MAX), @detail  nvarchar(max) 
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
  
------------------------------------------------------/*UPDATE*/
	--declare @transfer_id nvarchar(50) = ''

	--SELECT @transfer_id = transfer_id 
	--FROM OPENJSON(@header, '$') WITH (
	--	transfer_id VARCHAR(50)	
	--) AS a;

	--if @transfer_id is null or not exists(select * from T_TransferStock where transfer_id = @transfer_id) begin 
	--	print 'insert'
	--	declare @last_id varchar(50)=''
	--	DECLARE @InsertedData TABLE (last_id varchar(100))
	
	--	/*insert T_TransferStock*/
	--	INSERT INTO T_TransferStock (from_location_id, to_location_id, transfer_date, status)
	--	OUTPUT INSERTED.transfer_id INTO @InsertedData

	--	SELECT from_location_id, to_location_id, transfer_date, 'New'
	--	FROM OPENJSON(@header, '$') 
	--	WITH (
	--		transfer_date VARCHAR(50),
	--		from_location_id INT,
	--		to_location_id INT,
	--		remarks nvarchar(max)
	--	) AS a;

	--	/*Get last insertedid*/	
	--	SELECT @last_id = last_id FROM @InsertedData
		
	--	/*insert T_TransferStockDet*/
	--	insert into T_TransferStockDet (transfer_id,Itemid,Qty)
	--	SELECT @last_id, Itemid,Qty
	--	FROM OPENJSON(@detail, '$') 
	--	WITH (
	--		Itemid VARCHAR(50),
	--		Qty INT
	--	) AS b;

		
	--	select  'success' msg,  @last_id id
	--end
	--else begin
	--	print 'update head'
	--	update b 
	--	set b.transfer_date = a.transfer_date, b.from_location_id=a.from_location_id, b.to_location_id = a.to_location_id, b.remarks = a.remarks
	--	--SELECT *
	--	FROM OPENJSON(@header, '$') 
	--	WITH (
	--		transfer_id VARCHAR(50),
	--		transfer_date VARCHAR(50),
	--		from_location_id INT,
	--		to_location_id INT,remarks nvarchar(max)
	--	) AS a
	--	inner join T_TransferStock b on b.transfer_id = a.transfer_id
	--	where b.transfer_id = @transfer_id

	--	--detailnya
	--	print 'insert det'
	--	insert into T_TransferStockDet (transfer_id,Itemid,Qty)
	--	SELECT @transfer_id, itemid, qty
	--	FROM OPENJSON(@detail, '$') 
	--	WITH (
	--		transfer_detail_id int,
	--		Itemid VARCHAR(50),
	--		Qty INT
	--	) AS b
	--	where isnull(transfer_detail_id,'') = ''
		
	--	print 'update det'
	--	update b 
	--	set b.Itemid = a.itemid, b.Qty=a.qty
	--	--SELECT *
	--	FROM OPENJSON(@detail, '$') 
	--	WITH (
	--		transfer_detail_id int,
	--		Itemid VARCHAR(50),
	--		Qty INT
	--	) AS a
	--	inner join T_TransferStockDet b on b.transfer_detail_id = a.transfer_detail_id
		
	--	select  'success' msg, @transfer_id id


	--end
GO

/****** Object:  StoredProcedure [dbo].[genSaveData_child]    Script Date: 03/08/2023 15:04:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[genSaveData_child] @userlogin nvarchar(100)='', @detail nvarchar(max), @id nvarchar(max)='', @query nvarchar(max) = '' OUTPUT
as
--DECLARE @userlogin nvarchar(100)='dev.andre', @detail NVARCHAR(MAX) = 
--'
--{
--  "t_reminder_mail": [
--    {
--      "recid": 1,
--      "Id": 4,
--      "ReminderId": "14",
--      "MailAddress": "asdas",
--      "MailName": "asdasdada",
--      "IsDeleted": 0,
--      "CreateDate": null,
--      "CreateUser": "dev.andre",
--      "Creator": null
--    },
--    {
--      "MailName": "bbbb",
--      "MailAddress": "bbb",
--      "recid": 2
--    }
--  ],
--  "t_reminder_mail_parentkey": "ReminderId",
--  "t_reminder_mail_primarykey": "Id"
--}
          
--'
--,@id nvarchar(10) = '14'
--, @query nvarchar(max) = '' 


--SELECT ', ' + '''[hashdsad] = ' + isnull(JSON_VALUE(value, '$.MailAddress'),'') + ''', ''' + isnull(JSON_VALUE(value, '$.MailName'),'') + ''', ''' + ')'
--			FROM OPENJSON(@detail, '$.t_reminder_mail')
--			where isnull(JSON_VALUE(value, '$.Id'),'') = ''

--SELECT isnull(JSON_VALUE(value, '$.Id'),'')
--FROM OPENJSON(@detail, '$.t_reminder_mail') js
--where isnull(JSON_VALUE(value, '$.Id'),'') <> ''

DECLARE @TableName NVARCHAR(100), @ColumnNames NVARCHAR(MAX), @Values NVARCHAR(MAX)

-- Deklarasi cursor untuk membaca tabel dari JSON
DECLARE json_cursor CURSOR FOR

select [key]
FROM OPENJSON(@detail) js
where [key] not like '%parentkey%' and  [key] not like '%primarykey%'

-- Buka cursor
OPEN json_cursor;

-- Fetch pertama
FETCH NEXT FROM json_cursor INTO @TableName;

-- Loop untuk membaca data dari cursor
WHILE @@FETCH_STATUS = 0
BEGIN
	declare @primarykey nvarchar(max)='', @getpkvalue nvarchar(max)='', @pkvalue  nvarchar(max)='', @parentkey nvarchar(max)='', @insertstat nvarchar(max)='',@insertval nvarchar(max)='', @ins nvarchar(max)='', @ins2 nvarchar(max)='', @insval nvarchar(max)=''
	,@updatestr nvarchar(max)='',@updateval nvarchar(max)='',@updstr nvarchar(max)='',@updstr2 nvarchar(max)='',@updval nvarchar(max)=''

	SELECT @primarykey = [value] FROM OPENJSON(@detail) where [key] = @tablename+'_primarykey'

	set @getpkvalue = 
	'
	 SELECT @pkvalue = j.[value]
		FROM OPENJSON(@detail, ''$.'+@TableName+''') js
		CROSS APPLY OPENJSON(js.[value]) j	
		where lower(j.[key]) = '''+lower(@primarykey)+'''
	'
	EXEC sp_executesql @getpkvalue, N'@detail nvarchar(max), @pkvalue nvarchar(max) OUTPUT',@detail=@detail, @pkvalue = @pkvalue OUTPUT;

	SELECT @parentkey = [value] FROM OPENJSON(@detail) where [key] = @tablename+'_parentkey'
	
	set @updatestr = '
		set @updstr = isnull(STUFF((
			SELECT distinct '', ''''''+quotename(j.[key])+'' = '''' + [''+quotename(j.[key])+'']''
			FROM OPENJSON(@detail, ''$.'+@TableName+''') js
			CROSS APPLY OPENJSON(js.[value]) j	
			where lower(j.[key]) not in (''recid'', ''createdate'', ''createuser'', ''creator'',''isdeleted'', '''+lower(@primarykey)+''', '''+lower(@parentkey)+''')
		for xml path('''')), 1, 2, ''''),'''')
	'
	--print @updatestr

	EXEC sp_executesql @updatestr, N'@detail nvarchar(max), @updstr nvarchar(max) OUTPUT',@detail=@detail, @updstr = @updstr OUTPUT;
	set @updstr2 = replace(replace(replace(@updstr, ',',''), '[[', ''''''''' + isnull(JSON_VALUE(value, ''$.'), ']]', '''),'''') + '''''', ''  +')

	--print @updstr
	--print @updstr2
	set  @updateval ='
		set @updval = isnull(STUFF((
			SELECT char(10)+'' '' + ''update '+@TableName+' set ''+'+@updstr2+' ''''+''where '+QUOTENAME(@primarykey)+' = '''''+isnull(@pkvalue,'')+''''';''
			FROM OPENJSON(@detail, ''$.'+@TableName+''')
			where isnull(JSON_VALUE(value, ''$.'+@primarykey+'''),'''') <> ''''
		for xml path('''')), 1, 2, ''''),'''')
		'
	EXEC sp_executesql @updateval, N'@detail nvarchar(max),@updval nvarchar(max) OUTPUT',@detail=@detail, @updval = @updval OUTPUT;
	set @updval= replace(@updval, ', where', ' where')

	select @Query = isnull(@query,'') + isnull(@updval,'')+char(10)
	--print @updval
	--print 'x :'+@updateval
	
	
	--select @Query += @updstr
	--select @Query += concat('Update ', @TableName, ' set ', @updstr, ' where ', quotename(@primarykey) , ' = ', ''''+@pkvalue+'''' )+char(10)

	
		set @insertstat = '
		set @ins = isnull(STUFF((
			SELECT distinct '', [''+j.[key]+'']''
			FROM OPENJSON(@detail, ''$.'+@TableName+''') js
			CROSS APPLY OPENJSON(js.[value]) j	
			where lower(j.[key]) not in (''recid'', ''createdate'', ''createuser'', ''creator'',''isdeleted'', '''+lower(@primarykey)+''', '''+lower(@parentkey)+''')
		for xml path('''')), 1, 2, ''''),'''')
		'
		--print @insertstat
		--print @Ins
		EXEC sp_executesql @insertstat, N'@detail nvarchar(max), @ins nvarchar(max) OUTPUT',@detail=@detail, @ins = @ins OUTPUT;

		set @ins2 = replace(replace(replace(@ins, ',',''), '[', 'isnull(JSON_VALUE(value, ''$.'), ']', '''),'''') + '''''', '''''' +')

		set  @insertval ='
		set @insval = isnull(STUFF((
			SELECT '', '' + ''('''''' + '+@ins2+' '')''
			FROM OPENJSON(@detail, ''$.'+@TableName+''')
			where isnull(JSON_VALUE(value, ''$.'+@primarykey+'''),'''') = ''''
		for xml path('''')), 1, 2, ''''),'''')

		'
		--print @insertval
		EXEC sp_executesql @insertval, N'@detail nvarchar(max), @insval nvarchar(max) OUTPUT',@detail=@detail, @insval = @insval OUTPUT;
	
		
		set @insval = replace(@insval, ', '')', ','''+@userlogin+''','''+@id+''')'+char(10))

		--print @insval
		if isnull(@insval,'') <> '' begin 
			set @Query = isnull(@query,'') + concat('insert into ', @tablename, '(  ',@ins, ', [createuser]', ', '+quotename(@parentkey)+' )values' )+char(10)+@insval+char(10)
		end

		print @Query

	
    -- Fetch selanjutnya
    FETCH NEXT FROM json_cursor INTO @TableName;
END

-- Tutup cursor
CLOSE json_cursor;
DEALLOCATE json_cursor;

print @query
GO



create procedure [dbo].[getlistmail]
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