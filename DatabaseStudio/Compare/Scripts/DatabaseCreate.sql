USE [master]
GO

if exists (select 1 from sys.databases where name = 'ToCleanDatabase')
drop database ToCleanDatabase
go

CREATE DATABASE [ToCleanDatabase] 
GO

if exists (select 1 from sys.databases where name = 'FromFullDatabase')
drop database FromFullDatabase
go

CREATE DATABASE [FromFullDatabase] 
GO

USE [FromFullDatabase]
GO
/****** Object:  User [portaluser]    Script Date: 12/30/2015 1:19:44 PM ******/
CREATE USER [portaluser] FOR LOGIN [portaluser] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [portaluser]
GO
/****** Object:  UserDefinedFunction [dbo].[Date]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[Date](@Year int, @Month int, @Day int)
-- returns a datetime value for the specified year, month and day
-- Thank you to Michael Valentine Jones for this formula (see comments).
returns datetime
as
    begin
    return dateadd(month,((@Year-1900)*12)+@Month-1,@Day-1)
    end

GO
/****** Object:  UserDefinedFunction [dbo].[DateTime]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[DateTime](@Year int, @Month int, @Day int, @Hour int, @Minute int, @Second int)
-- returns a dateTime value for the date and time specified.
returns datetime
as
    begin
    return dbo.Date(@Year,@Month,@Day) + dbo.Time(@Hour, @Minute,@Second)
    end

GO
/****** Object:  UserDefinedFunction [dbo].[fnOrderInformationCount_BySearchItem]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fnOrderInformationCount_BySearchItem]
	(@SearchItem varchar(10))
RETURNS int
AS
BEGIN
	-- Counts all Distinct Orders (OrderInformationID) that have a Customer referencing any
	-- of the OrderInformation records for the True Item associated with the SearchItem.
	-- SearchItem could be either an Alias or True Item.
	declare @Result int
	declare @TrueItem varchar(10)

	set @TrueItem = (select dbo.fnFindTrueItem(@SearchItem))
    if (@TrueItem is NULL or @TrueItem = '')
		return 0;

	set @Result = (SELECT COUNT(OrderInformationID) from OrderInformation where CustomerID in
	                           (select Customerid from Customer where  Item =	@TrueItem) AND OrderType <> 'CNI')
	
	RETURN @Result
END


GO
/****** Object:  UserDefinedFunction [dbo].[fnOrderInformationCount_BySearchItem_bak]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[fnOrderInformationCount_BySearchItem_bak]
	(@SearchItem varchar(10))
RETURNS int
AS
BEGIN
	-- Counts all Distinct Orders (OrderInformationID) that have a Customer referencing any
	-- of the OrderInformation records for the True Item associated with the SearchItem.
	-- SearchItem could be either an Alias or True Item.
	declare @Result int
	declare @TrueItem varchar(10)

	set @TrueItem = (select dbo.fnFindTrueItem(@SearchItem))
    if (@TrueItem is NULL or @TrueItem = '')
		return 0;

--	set @Result = (SELECT COUNT(*) from (
	set @Result = (SELECT COUNT(CustomerID) from (
		SELECT distinct cp.CustomerID
		FROM Customer AS cp
			INNER JOIN OrderInformation AS ci ON ci.CustomerID = cp.CustomerID
		WHERE cp.Item = @TrueItem
	) as Orders) 
	RETURN @Result
END



GO
/****** Object:  UserDefinedFunction [dbo].[fnFindOrderItem]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE function [dbo].[fnFindOrderItem]
	(@OrderNumber varchar(25))
RETURNS varchar(10)
as
begin
	declare @Item varchar(10)

	Set @Item = (Select top 1 Item from Customer cp
	   inner join OrderInformation ci on (ci.CustomerID = cp.CustomerID and ci.OrderNumber = @OrderNumber)
	   )
	return @Item
end
	

GO
/****** Object:  UserDefinedFunction [dbo].[fnFindCustomerAddressID]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fnFindCustomerAddressID]
    (@Company  varchar(55)
	,@CompanyID varchar(55)
	,@PrimaryKey varchar(55))
RETURNS int
AS
BEGIN
	declare @cpaID int
     
   if (@Company is NULL)
		return null;		
   if (@CompanyID is NULL)
		return null;			
   if (@PrimaryKey is NULL)
		return null;			
    set @cpaID = 
    (
      select top 1 CustomerAddressID from CustomerAddress where CustomerNameID = (
      select top 1 CustomerNameID from CustomerName where CustomerID = (
      select Customerid from Customer where Company = @Company and CompanyID = @CompanyID)) and EmployeeAddressID = @PrimaryKey 
      order by CustomerAddressID
    )

	if @cpaID is null
		set @cpaID = 0;

	RETURN @cpaID
END

GO
/****** Object:  UserDefinedFunction [dbo].[fnFindCustomerAttributeID]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[fnFindCustomerAttributeID]
    (@Company  varchar(55)
	,@CompanyID varchar(55)
	,@PrimaryKey varchar(55))
RETURNS int
AS
BEGIN
	declare @cpaID int
     
   if (@Company is NULL)
		return null;		
   if (@CompanyID is NULL)
		return null;			
   if (@PrimaryKey is NULL)
		return null;			
    set @cpaID = 
    (
      select top 1 CustomerAttributeID from CustomerAttribute where CustomerNameID = (
      select top 1 CustomerNameID from CustomerName where CustomerID = (
      select Customerid from Customer where Company = @Company and CompanyID = @CompanyID)) and EmployeeAttributeID = @PrimaryKey
      order by CustomerAttributeID
    )

	if @cpaID is null
		set @cpaID = 0;

	RETURN @cpaID;
END


GO
/****** Object:  UserDefinedFunction [dbo].[fnFindCustomerDescriptionID]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[fnFindCustomerDescriptionID]
    (@Company  varchar(55)
	,@CompanyID varchar(55)
	,@PrimaryKey varchar(55))
RETURNS int
AS
BEGIN
	declare @cpdID int
     
   if (@Company is NULL)
		return null;		
   if (@CompanyID is NULL)
		return null;			
   if (@PrimaryKey is NULL)
		return null;			
    set @cpdID = 
    (
      select top 1 CustomerDescriptionID from CustomerDescription where CustomerNameID = (
      select top 1 CustomerNameID from CustomerName where CustomerID = (
      select Customerid from Customer where Company = @Company and CompanyID = @CompanyID)) and EmployeeDescriptionID = @PrimaryKey
      order by CustomerDescriptionID
    )
	if @cpdID is null
		set @cpdID = 0;

	RETURN @cpdID
END


GO
/****** Object:  UserDefinedFunction [dbo].[fnFindCustomerIDByProxyID]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fnFindCustomerIDByProxyID]
    (@Company  varchar(55)
    ,@proxyID varchar(55))
RETURNS int
AS
BEGIN
	if (@proxyID is NULL)
		return null;			
    
	declare @cpID int
    set @cpID = (select CustomerID from OrderInformation where OrderTypeDescription = @proxyID)

	if @cpID is null
		set @cpID = 0;

	RETURN @cpID
END

GO
/****** Object:  UserDefinedFunction [dbo].[fnFindCustomerNameID]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[fnFindCustomerNameID]
    (@Company  varchar(55)
	,@CompanyID varchar(55)
	,@PrimaryKey varchar(55))
RETURNS int
AS
BEGIN
	declare @cpnID int
    if (@Company is NULL)
		return null;		
   if (@CompanyID is NULL)
		return null;			
   if (@PrimaryKey is NULL)
		return null;
					
    set @cpnID = (select top 1 CustomerNameID from CustomerName where CustomerID = (
      select Customerid from Customer where Company = @Company and CompanyID = @CompanyID) and EmployeeNameID = @PrimaryKey)

	if @cpnID is null
		set @cpnID = 0;

	RETURN @cpnID
END


GO
/****** Object:  UserDefinedFunction [dbo].[fnFindCustomerParentID]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[fnFindCustomerParentID]
     (@Company  varchar(55)
	,@CompanyID varchar(55)
	,@PrimaryKey varchar(55))
RETURNS int
AS
BEGIN
	declare @cppID int
     
   if (@Company is NULL)
		return null;		
   if (@CompanyID is NULL)
		return null;			
   if (@PrimaryKey is NULL)
		return null;			
    set @cppID = 
    (
      select top 1 CustomerParentID from CustomerParent where CustomerNameID = (
      select top 1 CustomerNameID from CustomerName where CustomerID = (
      select Customerid from Customer where Company = @Company and CompanyID = @CompanyID)) and EmployeeParentID = @PrimaryKey
      order by CustomerParentID
    )
	if @cppID is null
		set @cppID = 0;

	RETURN @cppID
END


GO
/****** Object:  UserDefinedFunction [dbo].[fnFindCustomerPhoneID]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[fnFindCustomerPhoneID]
     (@Company  varchar(55)
	,@CompanyID varchar(55)
	,@PrimaryKey varchar(55))
RETURNS int
AS
BEGIN
	declare @cppID int
     
   if (@Company is NULL)
		return null;		
   if (@CompanyID is NULL)
		return null;			
   if (@PrimaryKey is NULL)
		return null;			
    set @cppID = 
    (
      select top 1 CustomerPhoneID from CustomerPhone where CustomerNameID = 
      (select top 1 CustomerNameID from CustomerName where CustomerID = 
      (select Customerid from Customer where Company = @Company and CompanyID = @CompanyID)) and EmployeePhoneID = @PrimaryKey
      order by CustomerPhoneID
    )
	if @cppID is null
		set @cppID = 0;

	RETURN @cppID
END


GO
/****** Object:  UserDefinedFunction [dbo].[fnFindExchangeCodeTranslation]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[fnFindExchangeCodeTranslation]
	(@LookupTypeCode varchar(50),
	@LookupTypeValue varchar(30),
	@Company varchar(20))
RETURNS varchar(20)
AS
BEGIN

	DECLARE @translatedValue varchar(20);

	SET @translatedValue  =

		(SELECT     dbo.LookupTypeValueTranslation.CodeValue
		FROM         dbo.LookupType INNER JOIN
						  dbo.LookupTypeValue ON dbo.LookupType.LookupTypeCode = dbo.LookupTypeValue.LookupTypeCode INNER JOIN
						  dbo.LookupTypeValueTranslation ON dbo.LookupTypeValue.LookupTypeCode = dbo.LookupTypeValueTranslation.LookupTypeCode AND 
						  dbo.LookupTypeValue.LookupTypeValue = dbo.LookupTypeValueTranslation.LookupTypeValue
		WHERE     (dbo.LookupType.ExchangeUse = 1) AND (dbo.LookupType.LookupTypeCode = @LookupTypeCode) AND (dbo.LookupTypeValue.LookupTypeValue = @LookupTypeValue) AND 
						  (dbo.LookupTypeValueTranslation.Company = @Company));
						
	IF @translatedValue IS NULL
	BEGIN
		SET @translatedValue = @LookupTypeValue; --just set to the value coming in because a translation could not be found
	END                     
	RETURN @translatedValue;                      
END


GO
/****** Object:  UserDefinedFunction [dbo].[fnFindItemByLSId]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE function [dbo].[fnFindItemByLSId]
(@lsid varchar(10))
RETURNS varchar(10)
as
begin
	return (select distinct Item from EmployeeName where EmployeeNameID in (select [EmployeeNameID] from [dbo].[EmployeeAttribute]
		where ([AssignedIDNumber] = @lsid and [AssignedIDType] = 'LSID' and [PrimaryID] = 1)));
end

GO
/****** Object:  UserDefinedFunction [dbo].[fnFindOdysseyPartyIDFromItem]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[fnFindOdysseyPartyIDFromItem]
	(@item varchar(10))
RETURNS varchar(10)
as
begin
	declare @id int
	
	set @id = (select top 1 a.AssignedIDNumber
		from EmployeeAttribute a
			inner join Employeename n on n.EmployeeNameID = a.EmployeeNameID and n.Item = @item
		where a.AssignedIDType = 'Odyssey ID' and a.DataSource = 'Odyssey' and a.ContributingCompany = 'court'
		order by a.UpdateDate desc)
	
	if @id is null
		return 0;

	return @id
end
GO
/****** Object:  UserDefinedFunction [dbo].[fnFindEmployeeNameID]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE function [dbo].[fnFindEmployeeNameID]
	(@sourceID varchar(40))
returns int
as
begin
	declare @EmployeeNameID int

	if (@sourceID is NULL)
		return null;			

	set @EmployeeNameID = (select top 1 Employeenameid from Customername where sourceid = @sourceID order by Employeenameid desc)

	if @EmployeeNameID is null and LEN(@sourceID) > 10
	begin
		-- try again without sequece
		set @EmployeeNameID = (select top 1 Employeenameid from Customername where sourceid = substring(@sourceID, 1, 10) order by Employeenameid desc)
	end
	
	if @EmployeeNameID is null
		set @EmployeeNameID = 0;

	return @EmployeeNameID
end

GO
/****** Object:  UserDefinedFunction [dbo].[fnFindEmployeeNameIDFromOrderNumber]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE function [dbo].[fnFindEmployeeNameIDFromOrderNumber]
	(@caseNumber varchar(25))
RETURNS varchar(25)
as
begin
	declare @EmployeeNameID int
	
	set @EmployeeNameID = (select distinct Employeenameid
		from Customername n
			inner join caseinformation ci on ci.Customerid = n.Customerid 
			inner join Customer cp on cp.Customerid = n.Customerid 
		where aliasindicator = 'n' and caseinformationid in (select caseinformationid from caseinformation where casenumber = @caseNumber))

	if @EmployeeNameID is null
		set @EmployeeNameID = 0;

	return @EmployeeNameID
end


GO
/****** Object:  UserDefinedFunction [dbo].[fnFindPrimaryAttribute]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE function [dbo].[fnFindPrimaryAttribute]
 (@SearchItem varchar(10)
 ,@SearchIdType varchar(10))
RETURNS varchar(50)
as
begin
	declare @PrimaryIdNumber varchar(50)
	Set @PrimaryIdNumber = 'NONE'

	Set @PrimaryIdNumber = (Select [AssignedIDNumber] FROM [dbo].[EmployeeAttribute]
		WHERE ([EmployeeNameID] in (select [EmployeeNameID] from EmployeeNAME where Item=@SearchItem)) and [AssignedIDType] = @SearchIdType and [PrimaryID] = 1)
	
	if (@PrimaryIdNumber is NULL)
		Set @PrimaryIdNumber = '';
	
	return @PrimaryIdNumber
end


GO
/****** Object:  UserDefinedFunction [dbo].[fnFindPrimaryLSID]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE function [dbo].[fnFindPrimaryLSID]
(@SearchItem varchar(10))
RETURNS varchar(10)
as
begin
	declare @PrimaryLSID varchar(10)
	Set @PrimaryLSID = 'NONE'

	Set @PrimaryLSID = (Select [AssignedIDNumber] FROM [dbo].[EmployeeAttribute]
		WHERE ([EmployeeNameID] in (select [EmployeeNameID] from EmployeeNAME where Item=@SearchItem)) and [AssignedIDType] = 'LSID' and [PrimaryID] = 1)
	
	if (@PrimaryLSID is NULL)
		Set @PrimaryLSID = '';
	
	return @PrimaryLSID
end


GO
/****** Object:  UserDefinedFunction [dbo].[fnFindPrimaryItem4OrderNumber]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE function [dbo].[fnFindPrimaryItem4OrderNumber]
(
@Company varchar(50),
@CompanyOrderNumber varchar(50)
)

RETURNS varchar(10)
as
begin
	declare @item varchar(30);
	Select @item = (select Item from Customer Where CustomerID = (select top 1 Customerid from OrderInformation where OrderType = @Company and OrderNumber = @CompanyOrderNumber order by Customerid));	
	if @item is null
		Set @item =  ''
	return @item
end



GO
/****** Object:  UserDefinedFunction [dbo].[fnFindProbOrderNoByItem]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE function [dbo].[fnFindProbOrderNoByItem] (@Item varchar(10))
RETURNS varchar(10)
as
begin
	declare @ProbOrderNo varchar(10)
	Set @ProbOrderNo = 'NONE'

	set @ProbOrderNo = (select top 1 casenumber from OrderInformation where SourceCompany = 'Probation' and SourceID = @Item order by OrderInformationID);

	if (@ProbOrderNO is NULL)
		Set @ProbOrderNO = '';
	
	return @ProbOrderNO
	
end

GO
/****** Object:  UserDefinedFunction [dbo].[fnFindRelatedItems]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE function [dbo].[fnFindRelatedItems]
	(@SearchItem varchar(10))
RETURNS @Result TABLE (Item varchar (10))
AS 
begin
	declare @TrueItem varchar(10)
	set @TrueItem = (select dbo.fnFindTrueItem(@SearchItem))
	insert into @Result values(@TrueItem)
	insert into @Result 
		select AssignedIDNumber from EmployeeAttribute pa join EmployeeName pn 
				on (pa.EmployeeNameid=pn.EmployeeNameid and AssignedIDType='Item')
			where Item=@TrueItem

	RETURN
end
	

GO
/****** Object:  UserDefinedFunction [dbo].[fnFindSheriffIDFromItem]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[fnFindSheriffIDFromItem]
	(@item varchar(10))
RETURNS varchar(10)
as
begin
	declare @sheriffID int
	
	set @sheriffID = (select top 1 a.AssignedIDNumber
		from EmployeeAttribute a
			inner join Employeename n on n.EmployeeNameID = a.EmployeeNameID and n.Item = @item
		where a.AssignedIDType = 'jmspin' and a.DataSource = 'Sheriff' order by a.UpdateDate desc)
	
	if @sheriffID is null
		set @sheriffID = 0;

	return @sheriffID
end

GO
/****** Object:  UserDefinedFunction [dbo].[fnFindTrueItem]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE function [dbo].[fnFindTrueItem]
	(@SearchItem varchar(10))
RETURNS varchar(10)
as
begin
	declare @TrueItem varchar(10)

	Set @TrueItem = (Select top 1 Item from EmployeeName
		where EmployeeNameID  in (
          select EmployeeNameID from EmployeeAttribute
              where AssignedIDType='Item' and AssignedIDNumber=@SearchItem)
		or  EmployeeNameID = (select top 1 EmployeeNameID from EmployeeName
              where Item=@SearchItem))
	return @TrueItem
end
	

GO
/****** Object:  UserDefinedFunction [dbo].[fnGetAttorneyName]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE function [dbo].[fnGetAttorneyName]
	(@barID varchar(50))
returns varchar(50)
as
begin
	declare @attorneyName varchar(50)
	set @attorneyName = (select top 1 ValueDescription
					 from LookupTypeValue
					 where LookupTypeCode = 'Attorney' and LookupTypeValue = @barID)
						 
	--set @attorneyName = (select top 1 b.ValueDescription
	--					 from LookupTypeValue a
	--						left outer join LookupTypeValue b on b.LookupTypeValue = a.LookupTypeValue and b.LookupTypeCode = 'defenseAttorney'
	--					 where a.ValueDescription = @barID and a.LookupTypeCode = 'barid')
	
	return @attorneyName
end

GO
/****** Object:  UserDefinedFunction [dbo].[fnGetMNIRetryCount]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create FUNCTION [dbo].[fnGetMNIRetryCount] ()

RETURNS int
AS
BEGIN
	declare @retryCount varchar(30);
	set @retryCount = (select lookuptypevalue from lookuptypevalue where lookuptypecode = 'mniLookupDelayCount')
						
	if ((@retryCount is null) or (@retryCount = ''))
		return 0;
	
	return (select CONVERT(int, @retryCount));                     
END
GO
/****** Object:  UserDefinedFunction [dbo].[fnIsAuthorized]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE function [dbo].[fnIsAuthorized]
	( @caseNumberOrItem varchar(25), @accessAdultOnly int, @crossReference bit = 0 )
RETURNS bit
as
begin
	declare @count int;
	DECLARE @firstChar varchar(1)
	DECLARE @isItem bit = 0
	set @count = 0
	SET @firstChar = LEFT(@caseNumberorItem,1)
	IF(@firstChar = 'D')
	BEGIN
		SET @isItem = 1;
	END
	
	-- clean up DA case number - need to remove dash
	set @caseNumberOrItem = replace(@caseNumberOrItem, '079-', '079');
	
  if(@crossReference = 1)
  begin
	if (@accessAdultOnly = 1)
	begin
		IF @isItem = 1 -- If we are dealing with an Item, use this query instead
		BEGIN
		  set @count = (select count(n.CustomerID)
			  FROM [dbo].[OrderReferredOrder] r
				 inner join [dbo].[OrderInformation] c on c.OrderInformationID = r.OrderInformationID
				 inner join [dbo].[Customername] n on n.CustomerID = c.CustomerID and n.NameType = 'adult' and n.Item = @caseNumberOrItem
			  where (c.OrderType <> 'cni') and not exists (select *
					from OrderInformation i
						inner join Customername n on n.Customerid = i.Customerid and n.NameType = 'adult' and n.Item = @caseNumberOrItem
					where i.OrderType <> 'cni'
						and ((i.OrderNumber like '%jv%') or (i.OrderNumber like '%jd%') or (i.OrderNumber like '%ab%') or (i.OrderNumber like '%mj%'))));		
		END
		ELSE
		BEGIN
		  set @count = (select count(n.CustomerID)
			  FROM [dbo].[OrderReferredOrder] r
				 inner join [dbo].[OrderInformation] c on c.OrderInformationID = r.OrderInformationID
				 inner join [dbo].[Customername] n on n.CustomerID = c.CustomerID and n.NameType = 'adult'
					and (n.Customerid in (select Customerid from OrderInformation where OrderNumber = @caseNumberOrItem)
					or n.Customerid in (select Customerid from OrderInformation where caseinformationid in (select OrderInformationID from OrderReferredOrder where CompanyOrderNumber = @caseNumberOrItem)))
			  where (OrderNumber = @caseNumberOrItem or CompanyOrderNumber = @caseNumberOrItem));
		END
	end
	else
	begin -- has juvenile access so can view everything
		return 1;
	end
  end
  else  -- @crossReference is 0
  begin
	-- need to account for data not available
	IF @isItem = 1 -- If we are dealing with an Item, use this query instead
	BEGIN
		set @count = (select count(n.CustomerID)
			from OrderInformation i
				inner join Customername n on n.Customerid = i.Customerid
			where @caseNumberOrItem = n.Item);	
	END
	ELSE
	BEGIN
		set @count = (select count(n.CustomerID)
			from OrderInformation i
				inner join Customername n on n.Customerid = i.Customerid
			where (@caseNumberOrItem = i.OrderNumber));
	END
	if (@count = 0)  -- no data found
	begin
		return 1;
	end
	
	if (@accessAdultOnly = 1)
	begin
		IF @isItem = 1 -- If we are dealing with an Item, use this query instead
		BEGIN
			set @count = (select count(p.CustomerID)
			from OrderInformation c
				inner join Customername p on p.Customerid = c.Customerid and p.NameType = 'adult'
			where @caseNumberOrItem = p.Item and not exists (select *
					from OrderInformation i
						inner join Customername n on n.Customerid = i.Customerid and n.NameType = 'adult'
					where @caseNumberOrItem = n.Item and i.OrderType <> 'cni'
						and ((i.OrderNumber like '%jv%') or (i.OrderNumber like '%jd%') or (i.OrderNumber like '%ab%') or (i.OrderNumber like '%mj%'))));
		END
		ELSE
		BEGIN
			set @count = (select count(n.CustomerID)
			from OrderInformation i
				inner join CustomerName n on n.Customerid = i.Customerid and n.NameType = 'adult' and n.Customerid in (select Customerid from OrderInformation where OrderNumber = @caseNumberOrItem)
			where i.OrderType <> 'cni' and not exists (select *
					from OrderInformation i
						inner join CustomerName n on n.Customerid = i.Customerid and n.NameType = 'adult' and n.Customerid in (select Customerid from OrderInformation where OrderNumber = @caseNumberOrItem)
					where ((i.OrderNumber like '%jv%') or (i.OrderNumber like '%jd%') or (i.OrderNumber like '%ab%') or (i.OrderNumber like '%mj%'))));
		END
	end
	else
	begin  -- has juvenile access so can view everything
		return 1;
	end

  end
	if (@count = 0)  -- not authorized
		return 0;
	
	return 1; -- authorized;
end

GO
/****** Object:  UserDefinedFunction [dbo].[fnIsCourtExchangeTypeOn]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fnIsCourtExchangeTypeOn]
	(@exchangeType varchar(30))

RETURNS bit
AS
BEGIN

	declare @setting varchar(20);
	set @setting = (select valueDescription from lookuptypevalue where lookuptypecode = 'courtExchangeType' and lookuptypevalue = @exchangeType)
						
	if (@exchangeType is null)
		return 0;
	
	if (@exchangeType = 'on')
		return 1;
		
	return 0;                     
END


GO
/****** Object:  UserDefinedFunction [dbo].[fnItemMergeLog_MergeItem]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE function [dbo].[fnItemMergeLog_MergeItem]
	(@item varchar(10))
returns varchar(10)
as
begin
	-- Finds the Item that the incoming value was merged with
	-- or returns empty string.
	declare @toItem varchar(10)
	declare @possibleItem varchar(10)

	--set @toItem = (select distinct toItem from Item_merge_log where fromitem = @item)
	set @toItem = (select top 1 toItem from Item_merge_log where fromitem = @item order by updatedate desc)

	while (len(@toItem) > 0)
	begin
		set @possibleItem = @toItem
		set @toItem = (select distinct toItem from Item_merge_log where fromItem = @possibleItem)
	end

	if @possibleItem is null
		set @possibleItem = ''

	return @possibleItem
end

GO
/****** Object:  UserDefinedFunction [dbo].[fnEmployeeAddress_CountOrderLinks]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[fnEmployeeAddress_CountOrderLinks]
	(	@ID int	)
RETURNS int
AS
BEGIN
	declare @Result int

   if (@ID is NULL)
		return 0;

	set @Result = (
		SELECT COUNT(*)  
		FROM  CustomerAddress
		WHERE 
			EmployeeAddressID = @ID
		) 
	RETURN @Result
END


GO
/****** Object:  UserDefinedFunction [dbo].[fnEmployeeAttribute_CountOrderLinks]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[fnEmployeeAttribute_CountOrderLinks]
	(	@ID int	)
RETURNS int
AS
BEGIN
	declare @Result int

   if (@ID is NULL)
		return 0;

	set @Result = (
		SELECT COUNT(*)  
		FROM  CustomerAttribute
		WHERE 
			EmployeeAttributeID = @ID
		) 
	RETURN @Result
END


GO
/****** Object:  UserDefinedFunction [dbo].[fnEmployeeDescription_CountOrderLinks]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[fnEmployeeDescription_CountOrderLinks]
	(	@ID int	)
RETURNS int
AS
BEGIN
	declare @Result int

   if (@ID is NULL)
		return 0;

	set @Result = (
		SELECT COUNT(*)  
		FROM  CustomerDescription
		WHERE 
			EmployeeDescriptionID = @ID
		) 
	RETURN @Result
END


GO
/****** Object:  UserDefinedFunction [dbo].[fnEmployeeName_CountOrderLinks]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fnEmployeeName_CountOrderLinks]
      (     @ID int     )
RETURNS int
AS
BEGIN
      declare @Result1 int
 
   if (@ID is NULL)
            return 0;
 
      set @Result1 = (
            SELECT COUNT(*)  
            FROM  CustomerName
            WHERE 
                  EmployeeNameID = @ID
            ) 
      RETURN @Result1
END

GO
/****** Object:  UserDefinedFunction [dbo].[fnEmployeeName_CountRows]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fnEmployeeName_CountRows]
	(@FirstName varchar(50),
	  @LastName varchar(50)	)
RETURNS int
AS
BEGIN
	if (@FirstName is null and @LastName is null)
		return 0;
	
	if (ltrim(rtrim(@FirstName)) = '' and ltrim(rtrim(@LastName)) = '')
		return 0;

	declare @Result1 int
	
	set @Result1 = (
		SELECT count(EmployeeNameID)
	FROM [dbo].[EmployeeName]
	WHERE (FirstName like @FirstName or @FirstName is null)
		and (LastName like @LastName or @LastName is null)
	)
	RETURN @Result1
END







GO
/****** Object:  UserDefinedFunction [dbo].[fnEmployeeName_IsJuvenile]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create function [dbo].[fnEmployeeName_IsJuvenile]
	(	@item char(10)	)
returns int
AS
begin
	declare @result char(10)

	if (@item is NULL)
		return null;

	set @result = (select top 1 nametype from Employeename where item = @item order by updatedate desc)

	if (@result = 'juvenile')
		return 1;

	return 0;
end

GO
/****** Object:  UserDefinedFunction [dbo].[fnEmployeeParent_CountOrderLinks]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fnEmployeeParent_CountOrderLinks]
	(	@ID int	)
RETURNS int
AS
BEGIN
	declare @Result int

   if (@ID is NULL)
		return 0;

	set @Result = (
		SELECT COUNT(CustomerParentID)  
		FROM  CustomerParent
		WHERE 
			EmployeeParentID = @ID
		) 
	RETURN @Result
END

GO
/****** Object:  UserDefinedFunction [dbo].[fnEmployeePhone_CountOrderLinks]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fnEmployeePhone_CountOrderLinks]
	(	@ID int	)
RETURNS int
AS
BEGIN
	declare @Result int

   if (@ID is NULL)
		return 0;

	set @Result = (
		SELECT COUNT(CustomerPhoneID)  
		FROM  CustomerPhone
		WHERE 
			EmployeePhoneID = @ID
		) 
	RETURN @Result
END

GO
/****** Object:  UserDefinedFunction [dbo].[fnTitleOrder]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE  FUNCTION [dbo].[fnTitleOrder]  (@InputString VARCHAR(4000) )
RETURNS VARCHAR(4000)
            AS
    BEGIN
    DECLARE @Index          INT
    DECLARE @Char           CHAR(1)
    DECLARE @OutputString   VARCHAR(255)
        SET @OutputString = LOWER(@InputString)
        SET @Index = 2
        SET @OutputString =
                STUFF(@OutputString, 1, 1,UPPER(SUBSTRING(@InputString,1,1)))
    WHILE @Index <= LEN(@InputString)
        BEGIN
        SET @Char = SUBSTRING(@InputString, @Index, 1)
        IF @Char IN (' ', ';', ':', '!', '?', ',', '.', '_', '-', '/', '&','''',
                '(')
            IF @Index + 1 <= LEN(@InputString)
            BEGIN
            IF @Char != '''' 
                        OR
                        UPPER(SUBSTRING(@InputString, @Index + 1, 1))
                        != 'S'
            SET @OutputString =
                        STUFF(@OutputString, @Index + 1, 1,UPPER(SUBSTRING(@InputString, @Index
                                    + 1, 1)))
            END
        SET @Index = @Index + 1
        END
    RETURN ISNULL(@OutputString,'')
    END



GO
/****** Object:  UserDefinedFunction [dbo].[Time]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create function [dbo].[Time](@Hour int, @Minute int, @Second int)
-- Returns a datetime value for the specified time at the "base" date (1/1/1900)
-- Many thanks to MVJ for providing this formula (see comments). 
returns datetime
as
    begin
    return dateadd(ss,(@Hour*3600)+(@Minute*60)+@Second,0)
    end

GO
/****** Object:  Table [dbo].[audit_BusinessException]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[audit_BusinessException](
	[auditBusinessExceptionID] [int] IDENTITY(1,1) NOT NULL,
	[BusinessExceptionID] [int] NULL,
	[Item] [char](10) NULL,
	[Category] [varchar](10) NULL,
	[SubCategory] [varchar](10) NULL,
	[Reason] [varchar](100) NULL,
	[ValidationStatus] [char](1) NULL,
	[ValidationDate] [datetime] NULL,
	[MatchedItem] [varchar](100) NULL,
	[CustomerNameID] [int] NULL,
	[EmployeeNameID] [int] NULL,
	[NotificationID] [int] NULL,
	[CreateDate] [datetime] NULL,
	[Priority] [int] NULL,
	[DocProcessed] [char](1) NULL,
	[Company] [varchar](10) NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
	[TransactionType] [varchar](50) NULL,
 CONSTRAINT [auditBusinessExceptionID_PK] PRIMARY KEY CLUSTERED 
(
	[auditBusinessExceptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[audit_CustomerAddress]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[audit_CustomerAddress](
	[auditCustomerAddressID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerAddressID] [int] NOT NULL,
	[CustomerNameID] [int] NOT NULL,
	[EmployeeAddressID] [int] NULL,
	[AddressType] [varchar](10) NULL,
	[AddressLine1] [varchar](50) NULL,
	[AddressLine2] [varchar](50) NULL,
	[AddressLine3] [varchar](50) NULL,
	[StreetType] [varchar](4) NULL,
	[HouseNumber] [varchar](5) NULL,
	[HalfAddress] [varchar](5) NULL,
	[PrefixDirection] [varchar](2) NULL,
	[StreetName] [varchar](60) NULL,
	[PostDirection] [varchar](2) NULL,
	[Apartment] [varchar](10) NULL,
	[City] [varchar](20) NULL,
	[State] [varchar](2) NULL,
	[PostalCode] [varchar](9) NULL,
	[CountryCode] [varchar](10) NULL,
	[SourceID] [varchar](40) NULL,
	[DataSource] [varchar](10) NULL,
	[DateModified] [smalldatetime] NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
	[ContributingCompany] [varchar](50) NULL,
	[IDSourceCode] [varchar](50) NULL,
	[TransactionType] [char](10) NULL,
 CONSTRAINT [auditCustomerAddress_PK] PRIMARY KEY CLUSTERED 
(
	[auditCustomerAddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[audit_CustomerAttribute]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[audit_CustomerAttribute](
	[auditCustomerAttributeID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerAttributeID] [int] NULL,
	[CustomerNameID] [int] NOT NULL,
	[EmployeeAttributeID] [int] NULL,
	[AssignedIDType] [varchar](10) NOT NULL,
	[AssignedIDNumber] [varchar](50) NOT NULL,
	[AssignedIDState] [varchar](10) NULL,
	[PrimaryID] [bit] NOT NULL CONSTRAINT [DF_audit_CustomerAttribute_PrimaryID]  DEFAULT ((0)),
	[SourceID] [varchar](255) NULL,
	[DataSource] [varchar](10) NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
	[ContributingCompany] [varchar](50) NULL,
	[DateModified] [smalldatetime] NULL,
	[IDSourceCode] [varchar](50) NULL,
	[TransactionType] [char](10) NULL,
 CONSTRAINT [auditCustomerAttribute_PK] PRIMARY KEY CLUSTERED 
(
	[auditCustomerAttributeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[audit_CustomerDescription]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[audit_CustomerDescription](
	[auditCustomerDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerDescriptionID] [int] NOT NULL,
	[CustomerNameID] [int] NOT NULL,
	[EmployeeDescriptionID] [int] NULL,
	[Sex] [char](1) NULL,
	[Race] [varchar](10) NULL,
	[Height] [char](3) NULL,
	[Weight] [char](3) NULL,
	[EyeColor] [varchar](10) NULL,
	[BirthPlace] [varchar](40) NULL,
	[HairColor] [varchar](10) NULL,
	[Occupation] [varchar](50) NULL,
	[SourceID] [varchar](255) NULL,
	[DataSource] [varchar](10) NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
	[ContributingCompany] [varchar](50) NULL,
	[DateModified] [smalldatetime] NULL,
	[IDSourceCode] [varchar](50) NULL,
	[TransactionType] [char](10) NULL,
 CONSTRAINT [auditCustomerDescription_PK] PRIMARY KEY CLUSTERED 
(
	[auditCustomerDescriptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[audit_CustomerName]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[audit_CustomerName](
	[auditCustomerNameID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerNameID] [int] NOT NULL,
	[CustomerID] [int] NOT NULL,
	[EmployeeNameID] [int] NULL,
	[Item] [char](10) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[MiddleName] [varchar](50) NULL,
	[LastName] [varchar](50) NOT NULL,
	[SuffixName] [varchar](10) NULL,
	[NameTitle] [varchar](10) NULL,
	[BusinessName] [varchar](100) NULL,
	[DOB] [datetime] NULL,
	[DeathDate] [smalldatetime] NULL,
	[NameType] [varchar](10) NULL,
	[PhoneticLastName] [varchar](10) NULL,
	[SoundexFirstName] [varchar](5) NULL,
	[SoundexLastName] [varchar](5) NULL,
	[AliasIndicator] [char](1) NULL,
	[DataSource] [varchar](10) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUserName] [varchar](50) NOT NULL,
	[SourceID] [varchar](255) NULL,
	[ValidationIndicator] [char](1) NULL,
	[ValidationComments] [varchar](100) NULL,
	[DateModified] [smalldatetime] NULL,
	[Note] [varchar](100) NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
	[ContributingCompany] [varchar](50) NULL,
	[IDSourceCode] [varchar](50) NULL,
	[TransactionType] [char](10) NULL,
 CONSTRAINT [PK_auditCustomerName] PRIMARY KEY CLUSTERED 
(
	[auditCustomerNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[audit_CustomerParent]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[audit_CustomerParent](
	[auditCustomerParentID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerParentID] [int] NOT NULL,
	[CustomerNameID] [int] NOT NULL,
	[EmployeeParentID] [int] NULL,
	[PartyType] [varchar](15) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[MiddleName] [varchar](50) NULL,
	[LastName] [varchar](50) NOT NULL,
	[NameTitle] [varchar](10) NULL,
	[AddressLine1] [varchar](50) NULL,
	[AddressLine2] [varchar](50) NULL,
	[AddressLine3] [varchar](50) NULL,
	[City] [varchar](20) NULL,
	[State] [varchar](2) NULL,
	[PostalCode] [varchar](9) NULL,
	[SSN] [varchar](9) NULL,
	[PhoneNumber] [varchar](20) NULL,
	[Notes] [varchar](70) NULL,
	[SourceID] [varchar](40) NULL,
	[DataSource] [varchar](10) NOT NULL,
	[DateModified] [smalldatetime] NULL,
	[ContributingCompany] [varchar](50) NULL,
	[IDSourceCode] [varchar](50) NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
	[TransactionType] [char](10) NULL,
 CONSTRAINT [PK_auditCustomerParent] PRIMARY KEY CLUSTERED 
(
	[auditCustomerParentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[audit_CustomerPhone]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[audit_CustomerPhone](
	[auditCustomerPhoneID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerPhoneID] [int] NOT NULL,
	[CustomerNameID] [int] NOT NULL,
	[EmployeePhoneID] [int] NULL,
	[PhoneType] [varchar](10) NULL,
	[PhoneNumber] [varchar](20) NULL,
	[PhoneSuffix] [varchar](10) NULL,
	[SourceID] [varchar](40) NULL,
	[DataSource] [varchar](10) NULL,
	[DateModified] [smalldatetime] NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
	[ContributingCompany] [varchar](50) NULL,
	[IDSourceCode] [varchar](50) NULL,
	[TransactionType] [char](10) NULL,
 CONSTRAINT [auditCustomerPhone_PK] PRIMARY KEY CLUSTERED 
(
	[auditCustomerPhoneID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[audit_LogParameter]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[audit_LogParameter](
	[id] [int] NOT NULL,
	[parameterName] [varchar](100) NOT NULL,
	[parameterValue] [varchar](100) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[audit_LogQuery]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[audit_LogQuery](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[procedureName] [varchar](100) NOT NULL,
	[userName] [varchar](50) NOT NULL,
	[date] [datetime] NOT NULL,
 CONSTRAINT [PK_audit_queryLog] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[audit_ItemMergeLog]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[audit_ItemMergeLog](
	[auditItemMergeID] [int] IDENTITY(1,1) NOT NULL,
	[Item_MergeID] [int] NULL,
	[FromItem] [char](10) NULL,
	[ToItem] [char](10) NULL,
	[CustomerID] [int] NULL,
	[UpdateUserName] [varchar](50) NULL,
	[UpdateDate] [datetime] NULL,
	[VersionCount] [int] NULL,
	[TransactionType] [char](10) NULL,
 CONSTRAINT [auditItemMergeLog_PK] PRIMARY KEY CLUSTERED 
(
	[auditItemMergeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[audit_EmployeeAddress]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[audit_EmployeeAddress](
	[auditEmployeeAddressID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeAddressID] [int] NULL,
	[EmployeeNameID] [int] NULL,
	[AddressType] [varchar](10) NULL,
	[AddressLine1] [varchar](50) NULL,
	[AddressLine2] [varchar](50) NULL,
	[AddressLine3] [varchar](50) NULL,
	[StreetType] [varchar](4) NULL,
	[HouseNumber] [varchar](5) NULL,
	[HalfAddress] [varchar](5) NULL,
	[PrefixDirection] [varchar](2) NULL,
	[StreetName] [varchar](60) NULL,
	[PostDirection] [varchar](2) NULL,
	[Apartment] [varchar](10) NULL,
	[City] [varchar](20) NULL,
	[State] [varchar](2) NULL,
	[PostalCode] [varchar](9) NULL,
	[CountryCode] [varchar](10) NULL,
	[ContributingCompany] [varchar](50) NULL,
	[DateModified] [smalldatetime] NULL,
	[IDSourceCode] [varchar](50) NULL,
	[SourceID] [varchar](40) NULL,
	[DataSource] [varchar](10) NULL,
	[UpdateUserName] [varchar](50) NULL,
	[UpdateDate] [datetime] NULL,
	[VersionCount] [int] NULL,
	[TransactionType] [char](10) NULL,
 CONSTRAINT [auditEmployeeAddress_PK] PRIMARY KEY CLUSTERED 
(
	[auditEmployeeAddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[audit_EmployeeAttribute]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[audit_EmployeeAttribute](
	[auditEmployeeAttributeID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeAttributeID] [int] NULL,
	[EmployeeNameID] [int] NULL,
	[AssignedIDType] [varchar](10) NULL,
	[AssignedIDNumber] [varchar](50) NULL,
	[AssignedIDState] [varchar](10) NULL,
	[PrimaryID] [bit] NULL CONSTRAINT [DF_audit_EmployeeAttribute_PrimaryID]  DEFAULT ((0)),
	[ContributingCompany] [varchar](50) NULL,
	[DateModified] [smalldatetime] NULL,
	[IDSourceCode] [varchar](50) NULL,
	[SourceID] [varchar](255) NULL,
	[DataSource] [varchar](10) NULL,
	[UpdateUserName] [varchar](50) NULL,
	[UpdateDate] [datetime] NULL,
	[VersionCount] [int] NULL,
	[TransactionType] [char](10) NULL,
 CONSTRAINT [auditEmployeeAttribute_PK] PRIMARY KEY CLUSTERED 
(
	[auditEmployeeAttributeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[audit_EmployeeDescription]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[audit_EmployeeDescription](
	[auditEmployeeDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeDescriptionID] [int] NULL,
	[EmployeeNameID] [int] NULL,
	[Sex] [char](1) NULL,
	[Race] [varchar](10) NULL,
	[Height] [char](3) NULL,
	[Weight] [char](3) NULL,
	[EyeColor] [varchar](10) NULL,
	[BirthPlace] [varchar](40) NULL,
	[HairColor] [varchar](10) NULL,
	[Occupation] [varchar](50) NULL,
	[ContributingCompany] [varchar](50) NULL,
	[DateModified] [smalldatetime] NULL,
	[IDSourceCode] [varchar](50) NULL,
	[SourceID] [varchar](255) NULL,
	[DataSource] [varchar](10) NULL,
	[UpdateUserName] [varchar](50) NULL,
	[UpdateDate] [datetime] NULL,
	[VersionCount] [int] NULL,
	[TransactionType] [char](10) NULL,
 CONSTRAINT [auditEmployeeDescription_PK] PRIMARY KEY CLUSTERED 
(
	[auditEmployeeDescriptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[audit_EmployeeName]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[audit_EmployeeName](
	[auditEmployeeNameID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeNameID] [int] NULL,
	[Item] [char](10) NULL,
	[FirstName] [varchar](50) NULL,
	[MiddleName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[SuffixName] [varchar](10) NULL,
	[NameTitle] [varchar](10) NULL,
	[BusinessName] [varchar](100) NULL,
	[DOB] [datetime] NULL,
	[DeathDate] [smalldatetime] NULL,
	[NameType] [varchar](10) NULL,
	[PhoneticLastName] [varchar](10) NULL,
	[SoundexFirstName] [varchar](5) NULL,
	[SoundexLastName] [varchar](5) NULL,
	[AliasIndicator] [char](1) NULL,
	[DataSource] [varchar](10) NULL,
	[CreateDate] [datetime] NULL,
	[CreateUserName] [varchar](50) NULL,
	[SourceID] [varchar](255) NULL,
	[ValidationIndicator] [char](1) NULL,
	[ValidationComments] [varchar](100) NULL,
	[ContributingCompany] [varchar](50) NULL,
	[DateModified] [smalldatetime] NULL,
	[Note] [varchar](100) NULL,
	[IDSourceCode] [varchar](50) NULL,
	[UpdateUserName] [varchar](50) NULL,
	[UpdateDate] [datetime] NULL,
	[VersionCount] [int] NULL,
	[TransactionType] [char](10) NULL,
 CONSTRAINT [auditEmployeeName_PK] PRIMARY KEY CLUSTERED 
(
	[auditEmployeeNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[audit_EmployeeParent]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[audit_EmployeeParent](
	[auditEmployeeParentID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeParentID] [int] NOT NULL,
	[EmployeeNameID] [int] NOT NULL,
	[PartyType] [varchar](15) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[MiddleName] [varchar](50) NULL,
	[LastName] [varchar](50) NOT NULL,
	[NameTitle] [varchar](10) NULL,
	[AddressLine1] [varchar](50) NULL,
	[AddressLine2] [varchar](50) NULL,
	[AddressLine3] [varchar](50) NULL,
	[City] [varchar](20) NULL,
	[State] [varchar](2) NULL,
	[PostalCode] [varchar](9) NULL,
	[SSN] [varchar](9) NULL,
	[PhoneNumber] [varchar](20) NULL,
	[Notes] [varchar](70) NULL,
	[SourceID] [varchar](40) NULL,
	[DataSource] [varchar](10) NOT NULL,
	[DateModified] [smalldatetime] NULL,
	[ContributingCompany] [varchar](50) NULL,
	[IDSourceCode] [varchar](50) NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
	[TransactionType] [char](10) NULL,
 CONSTRAINT [PK_auditEmployeeParent] PRIMARY KEY CLUSTERED 
(
	[auditEmployeeParentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[audit_EmployeePhone]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[audit_EmployeePhone](
	[auditEmployeePhoneID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeePhoneID] [int] NULL,
	[EmployeeNameID] [int] NULL,
	[PhoneType] [varchar](10) NULL,
	[PhoneNumber] [varchar](20) NULL,
	[PhoneSuffix] [varchar](10) NULL,
	[SourceID] [varchar](40) NULL,
	[ContributingCompany] [varchar](50) NULL,
	[DateModified] [smalldatetime] NULL,
	[IDSourceCode] [varchar](50) NULL,
	[DataSource] [varchar](10) NULL,
	[UpdateUserName] [varchar](50) NULL,
	[UpdateDate] [datetime] NULL,
	[VersionCount] [int] NULL,
	[TransactionType] [char](10) NULL,
 CONSTRAINT [auditEmployeePhone_PK] PRIMARY KEY CLUSTERED 
(
	[auditEmployeePhoneID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessException]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BusinessException](
	[BusinessExceptionID] [int] IDENTITY(1,1) NOT NULL,
	[Item] [char](10) NULL,
	[Category] [varchar](10) NULL,
	[SubCategory] [varchar](10) NULL,
	[Reason] [varchar](100) NULL,
	[ValidationStatus] [char](1) NULL,
	[ValidationDate] [datetime] NULL,
	[MatchedItem] [varchar](100) NULL,
	[CustomerNameID] [int] NULL,
	[EmployeeNameID] [int] NULL,
	[NotificationID] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
	[Priority] [int] NULL,
	[DocProcessed] [char](1) NULL,
	[Company] [varchar](10) NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
 CONSTRAINT [BusinessException_PK] PRIMARY KEY CLUSTERED 
(
	[BusinessExceptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OrderInformation]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OrderInformation](
	[OrderInformationID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[OrderNumber] [varchar](25) NOT NULL,
	[SourceCompany] [varchar](10) NOT NULL,
	[OrderType] [varchar](20) NULL,
	[OrderTypeDescription] [varchar](50) NULL,
	[OrderStatus] [varchar](50) NULL,
	[RecordDate] [datetime] NULL,
	[CreateDate] [datetime] NOT NULL,
	[SourceID] [varchar](40) NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
 CONSTRAINT [OrderInformation_PK] PRIMARY KEY CLUSTERED 
(
	[OrderInformationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[Item] [char](10) NOT NULL,
	[InCustody] [bit] NOT NULL,
	[Company] [varchar](120) NOT NULL,
	[CompanyID] [varchar](120) NOT NULL,
	[SourceID] [varchar](120) NULL,
	[DataSource] [varchar](10) NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
 CONSTRAINT [Customer_PK] PRIMARY KEY CLUSTERED 
(
	[Company] ASC,
	[CompanyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY],
 CONSTRAINT [CK_Customer] UNIQUE NONCLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CustomerAddress]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CustomerAddress](
	[CustomerAddressID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerNameID] [int] NOT NULL,
	[EmployeeAddressID] [int] NULL,
	[AddressType] [varchar](10) NULL,
	[AddressLine1] [varchar](50) NULL,
	[AddressLine2] [varchar](50) NULL,
	[AddressLine3] [varchar](50) NULL,
	[StreetType] [varchar](4) NULL,
	[HouseNumber] [varchar](5) NULL,
	[HalfAddress] [varchar](5) NULL,
	[PrefixDirection] [varchar](2) NULL,
	[StreetName] [varchar](60) NULL,
	[PostDirection] [varchar](2) NULL,
	[Apartment] [varchar](10) NULL,
	[City] [varchar](20) NULL,
	[State] [varchar](2) NULL,
	[PostalCode] [varchar](9) NULL,
	[CountryCode] [varchar](10) NULL,
	[SourceID] [varchar](40) NULL,
	[DataSource] [varchar](10) NULL,
	[DateModified] [smalldatetime] NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
	[ContributingCompany] [varchar](50) NULL,
	[IDSourceCode] [varchar](50) NULL,
 CONSTRAINT [CustomerAddress_PK] PRIMARY KEY CLUSTERED 
(
	[CustomerAddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CustomerAttribute]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CustomerAttribute](
	[CustomerAttributeID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerNameID] [int] NOT NULL,
	[EmployeeAttributeID] [int] NULL,
	[AssignedIDType] [varchar](10) NOT NULL,
	[AssignedIDNumber] [varchar](50) NOT NULL,
	[AssignedIDState] [varchar](10) NULL,
	[PrimaryID] [bit] NOT NULL CONSTRAINT [DF_CustomerAttribute_PrimaryID]  DEFAULT ((0)),
	[SourceID] [varchar](255) NULL,
	[DataSource] [varchar](10) NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
	[ContributingCompany] [varchar](50) NULL,
	[DateModified] [smalldatetime] NULL,
	[IDSourceCode] [varchar](50) NULL,
 CONSTRAINT [CustomerAttribute_PK] PRIMARY KEY CLUSTERED 
(
	[CustomerAttributeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CustomerDescription]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CustomerDescription](
	[CustomerDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerNameID] [int] NOT NULL,
	[EmployeeDescriptionID] [int] NULL,
	[Sex] [char](1) NULL,
	[Race] [varchar](10) NULL,
	[Height] [char](3) NULL,
	[Weight] [char](3) NULL,
	[EyeColor] [varchar](10) NULL,
	[BirthPlace] [varchar](40) NULL,
	[HairColor] [varchar](10) NULL,
	[Occupation] [varchar](50) NULL,
	[SourceID] [varchar](255) NULL,
	[DataSource] [varchar](10) NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
	[ContributingCompany] [varchar](50) NULL,
	[DateModified] [smalldatetime] NULL,
	[IDSourceCode] [varchar](50) NULL,
 CONSTRAINT [CustomerDescription_PK] PRIMARY KEY CLUSTERED 
(
	[CustomerDescriptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CustomerName]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CustomerName](
	[CustomerNameID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[EmployeeNameID] [int] NULL,
	[Item] [char](10) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[MiddleName] [varchar](50) NULL,
	[LastName] [varchar](50) NOT NULL,
	[SuffixName] [varchar](10) NULL,
	[NameTitle] [varchar](10) NULL,
	[BusinessName] [varchar](100) NULL,
	[DOB] [datetime] NULL,
	[DeathDate] [smalldatetime] NULL,
	[NameType] [varchar](10) NULL,
	[PhoneticLastName] [varchar](10) NULL,
	[SoundexFirstName] [varchar](5) NULL,
	[SoundexLastName] [varchar](5) NULL,
	[AliasIndicator] [char](1) NULL,
	[DataSource] [varchar](10) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUserName] [varchar](50) NOT NULL,
	[SourceID] [varchar](255) NULL,
	[ValidationIndicator] [char](1) NULL,
	[ValidationComments] [varchar](100) NULL,
	[DateModified] [smalldatetime] NULL,
	[Note] [varchar](100) NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
	[ContributingCompany] [varchar](50) NULL,
	[IDSourceCode] [varchar](50) NULL,
 CONSTRAINT [PK_CustomerName] PRIMARY KEY CLUSTERED 
(
	[CustomerNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CustomerParent]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CustomerParent](
	[CustomerParentID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerNameID] [int] NOT NULL,
	[EmployeeParentID] [int] NULL,
	[PartyType] [varchar](15) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[MiddleName] [varchar](50) NULL,
	[LastName] [varchar](50) NOT NULL,
	[NameTitle] [varchar](10) NULL,
	[AddressLine1] [varchar](50) NULL,
	[AddressLine2] [varchar](50) NULL,
	[AddressLine3] [varchar](50) NULL,
	[City] [varchar](20) NULL,
	[State] [varchar](2) NULL,
	[PostalCode] [varchar](9) NULL,
	[SSN] [varchar](9) NULL,
	[PhoneNumber] [varchar](20) NULL,
	[Notes] [varchar](70) NULL,
	[SourceID] [varchar](40) NULL,
	[DataSource] [varchar](50) NULL,
	[DateModified] [smalldatetime] NULL,
	[ContributingCompany] [varchar](50) NULL,
	[IDSourceCode] [varchar](50) NULL,
	[UpdateUserName] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
 CONSTRAINT [PK_CustomerParent] PRIMARY KEY CLUSTERED 
(
	[CustomerParentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CustomerPhone]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CustomerPhone](
	[CustomerPhoneID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerNameID] [int] NOT NULL,
	[EmployeePhoneID] [int] NULL,
	[PhoneType] [varchar](10) NULL,
	[PhoneNumber] [varchar](20) NULL,
	[PhoneSuffix] [varchar](10) NULL,
	[SourceID] [varchar](40) NULL,
	[DataSource] [varchar](10) NULL,
	[DateModified] [smalldatetime] NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
	[ContributingCompany] [varchar](50) NULL,
	[IDSourceCode] [varchar](50) NULL,
 CONSTRAINT [CustomerPhone_PK] PRIMARY KEY CLUSTERED 
(
	[CustomerPhoneID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OrderReferredOrder]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OrderReferredOrder](
	[ReferredOrderID] [int] IDENTITY(1,1) NOT NULL,
	[OrderInformationID] [int] NOT NULL,
	[CompanyOrderNumber] [varchar](255) NOT NULL,
	[Company] [char](50) NOT NULL,
	[CompanyDate] [datetime] NULL,
	[SourceID] [varchar](255) NOT NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LookupType]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LookupType](
	[LookupTypeCode] [varchar](50) NOT NULL,
	[ActiveIndicator] [varchar](1) NULL,
	[Description] [varchar](50) NOT NULL,
	[ExchangeUse] [bit] NULL CONSTRAINT [DF_LookupType_ExchangeUse]  DEFAULT ((0)),
	[Owner] [varchar](10) NOT NULL CONSTRAINT [DF_LookupType_Owner]  DEFAULT ('Common'),
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
 CONSTRAINT [LookupType_PK] PRIMARY KEY CLUSTERED 
(
	[LookupTypeCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LookupTypeValue]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LookupTypeValue](
	[LookupTypeCode] [varchar](50) NOT NULL,
	[LookupTypeValue] [varchar](30) NOT NULL,
	[ValueDescription] [varchar](1000) NULL,
	[ActiveIndicator] [char](1) NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
 CONSTRAINT [pk_LookupTypeValue] PRIMARY KEY NONCLUSTERED 
(
	[LookupTypeCode] ASC,
	[LookupTypeValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_CodeValue]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE CLUSTERED INDEX [IX_CodeValue] ON [dbo].[LookupTypeValue]
(
	[LookupTypeCode] ASC,
	[LookupTypeValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LookupTypeValueTranslation]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LookupTypeValueTranslation](
	[LookupTypeValueTranslationID] [int] IDENTITY(1,1) NOT NULL,
	[LookupTypeCode] [varchar](50) NOT NULL,
	[LookupTypeValue] [varchar](30) NOT NULL,
	[Company] [varchar](20) NOT NULL,
	[CodeValue] [varchar](200) NOT NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[Version] [int] NOT NULL,
 CONSTRAINT [pk_LookupTypeValueTranslation] PRIMARY KEY CLUSTERED 
(
	[LookupTypeValueTranslationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY],
 CONSTRAINT [uqc_LookupTypeValueTranslation] UNIQUE NONCLUSTERED 
(
	[LookupTypeCode] ASC,
	[LookupTypeValue] ASC,
	[Company] ASC,
	[CodeValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MatchMerge]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MatchMerge](
	[MatchMergeID] [int] IDENTITY(1,1) NOT NULL,
	[Action] [varchar](50) NOT NULL,
	[FromLastName] [varchar](100) NOT NULL,
	[FromFirstName] [varchar](100) NULL,
	[FromMiddleName] [varchar](100) NULL,
	[FromItem] [char](10) NULL,
	[FromLSID] [char](10) NOT NULL,
	[ToLastName] [varchar](100) NOT NULL,
	[ToFirstName] [varchar](100) NULL,
	[ToMiddleName] [varchar](100) NULL,
	[ToItem] [char](10) NOT NULL,
	[ToLSID] [char](10) NOT NULL,
	[Company] [varchar](50) NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
 CONSTRAINT [PK_MatchMerge] PRIMARY KEY CLUSTERED 
(
	[MatchMergeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Item_Merge_Log]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Item_Merge_Log](
	[Item_MergeID] [int] IDENTITY(1,1) NOT NULL,
	[FromItem] [char](10) NOT NULL,
	[ToItem] [char](10) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
 CONSTRAINT [PK_Item_Merge_Log] PRIMARY KEY CLUSTERED 
(
	[Item_MergeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ItemSource]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ItemSource](
	[Item] [char](10) NOT NULL,
	[DataSource] [char](10) NULL,
	[ExchangeType] [char](10) NULL,
	[LockExchangeID] [int] NOT NULL CONSTRAINT [DF_ItemSource_LockExchangeID]  DEFAULT ((0)),
	[LockExchangeName] [varchar](50) NULL,
	[LastMaintenanceTask] [varchar](50) NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
 CONSTRAINT [ItemSource_PK] PRIMARY KEY CLUSTERED 
(
	[Item] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NextKeyGenerator]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NextKeyGenerator](
	[NextKeyID] [int] IDENTITY(1,1) NOT NULL,
	[KeyName] [varchar](20) NOT NULL,
	[KeyValue] [int] NOT NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
 CONSTRAINT [NextKeyGenerator_PK] PRIMARY KEY CLUSTERED 
(
	[NextKeyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Company]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Company](
	[CompanyID] [int] IDENTITY(1,1) NOT NULL,
	[Company] [varchar](30) NOT NULL,
	[BodyID] [varchar](255) NOT NULL,
	[LastName] [varchar](255) NOT NULL,
	[FirstName] [varchar](255) NULL,
	[MiddleName] [varchar](255) NULL,
	[ADUserName] [varchar](255) NULL,
	[ADDomain] [varchar](255) NULL,
	[UpdateUserName] [varchar](255) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
 CONSTRAINT [pk_Company] PRIMARY KEY NONCLUSTERED 
(
	[CompanyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY],
 CONSTRAINT [uqc_Company_Company_BodyID_LastName] UNIQUE NONCLUSTERED 
(
	[Company] ASC,
	[BodyID] ASC,
	[LastName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EmployeeAddress]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmployeeAddress](
	[EmployeeAddressID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeNameID] [int] NOT NULL,
	[AddressType] [varchar](10) NULL,
	[AddressLine1] [varchar](50) NULL,
	[AddressLine2] [varchar](50) NULL,
	[AddressLine3] [varchar](50) NULL,
	[StreetType] [varchar](4) NULL,
	[HouseNumber] [varchar](5) NULL,
	[HalfAddress] [varchar](5) NULL,
	[PrefixDirection] [varchar](2) NULL,
	[StreetName] [varchar](60) NULL,
	[PostDirection] [varchar](2) NULL,
	[Apartment] [varchar](10) NULL,
	[City] [varchar](20) NULL,
	[State] [varchar](2) NULL,
	[PostalCode] [varchar](9) NULL,
	[CountryCode] [varchar](10) NULL,
	[SourceID] [varchar](40) NULL,
	[DataSource] [varchar](10) NULL,
	[DateModified] [smalldatetime] NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
	[ContributingCompany] [varchar](50) NULL,
	[IDSourceCode] [varchar](50) NULL,
 CONSTRAINT [EmployeeAddress_PK] PRIMARY KEY CLUSTERED 
(
	[EmployeeAddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EmployeeAttribute]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmployeeAttribute](
	[EmployeeAttributeID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeNameID] [int] NOT NULL,
	[AssignedIDType] [varchar](10) NOT NULL,
	[AssignedIDNumber] [varchar](50) NOT NULL,
	[AssignedIDState] [varchar](10) NULL,
	[SourceID] [varchar](255) NULL,
	[PrimaryID] [bit] NOT NULL CONSTRAINT [DF_EmployeeAttribute_PrimaryID]  DEFAULT ((0)),
	[DataSource] [varchar](10) NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
	[ContributingCompany] [varchar](50) NULL,
	[DateModified] [smalldatetime] NULL,
	[IDSourceCode] [varchar](50) NULL,
 CONSTRAINT [EmployeeAttribute_PK] PRIMARY KEY CLUSTERED 
(
	[EmployeeAttributeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EmployeeDescription]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmployeeDescription](
	[EmployeeDescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeNameID] [int] NOT NULL,
	[Sex] [char](1) NULL,
	[Race] [varchar](10) NULL,
	[Height] [char](3) NULL,
	[Weight] [char](3) NULL,
	[EyeColor] [varchar](10) NULL,
	[BirthPlace] [varchar](40) NULL,
	[HairColor] [varchar](10) NULL,
	[Occupation] [varchar](50) NULL,
	[SourceID] [varchar](255) NULL,
	[DataSource] [varchar](10) NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
	[ContributingCompany] [varchar](50) NULL,
	[DateModified] [smalldatetime] NULL,
	[IDSourceCode] [varchar](50) NULL,
 CONSTRAINT [EmployeeDescription_PK] PRIMARY KEY CLUSTERED 
(
	[EmployeeDescriptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EmployeeName]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmployeeName](
	[EmployeeNameID] [int] IDENTITY(1,1) NOT NULL,
	[Item] [char](10) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[MiddleName] [varchar](50) NULL,
	[LastName] [varchar](50) NOT NULL,
	[SuffixName] [varchar](10) NULL,
	[NameTitle] [varchar](10) NULL,
	[BusinessName] [varchar](100) NULL,
	[DOB] [datetime] NULL,
	[DeathDate] [smalldatetime] NULL,
	[NameType] [varchar](10) NULL,
	[PhoneticLastName] [varchar](10) NULL,
	[SoundexFirstName] [varchar](5) NULL,
	[SoundexLastName] [varchar](5) NULL,
	[AliasIndicator] [char](1) NULL,
	[DataSource] [varchar](10) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUserName] [varchar](50) NOT NULL,
	[SourceID] [varchar](255) NULL,
	[ValidationIndicator] [char](1) NULL,
	[ValidationComments] [varchar](100) NULL,
	[DateModified] [smalldatetime] NULL,
	[Note] [varchar](100) NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
	[ContributingCompany] [varchar](50) NULL,
	[IDSourceCode] [varchar](50) NULL,
 CONSTRAINT [EmployeeName_PK] PRIMARY KEY CLUSTERED 
(
	[EmployeeNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EmployeeParent]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmployeeParent](
	[EmployeeParentID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeNameID] [int] NOT NULL,
	[PartyType] [varchar](15) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[MiddleName] [varchar](50) NULL,
	[LastName] [varchar](50) NOT NULL,
	[NameTitle] [varchar](10) NULL,
	[AddressLine1] [varchar](50) NULL,
	[AddressLine2] [varchar](50) NULL,
	[AddressLine3] [varchar](50) NULL,
	[City] [varchar](20) NULL,
	[State] [varchar](2) NULL,
	[PostalCode] [varchar](9) NULL,
	[SSN] [varchar](9) NULL,
	[PhoneNumber] [varchar](20) NULL,
	[Notes] [varchar](70) NULL,
	[SourceID] [varchar](40) NULL,
	[DataSource] [varchar](50) NULL,
	[DateModified] [smalldatetime] NULL,
	[ContributingCompany] [varchar](50) NULL,
	[IDSourceCode] [varchar](50) NULL,
	[UpdateUserName] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
 CONSTRAINT [PK_EmployeeParent] PRIMARY KEY CLUSTERED 
(
	[EmployeeParentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EmployeePhone]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmployeePhone](
	[EmployeePhoneID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeNameID] [int] NOT NULL,
	[PhoneType] [varchar](10) NULL,
	[PhoneNumber] [varchar](20) NULL,
	[PhoneSuffix] [varchar](10) NULL,
	[SourceID] [varchar](40) NULL,
	[DataSource] [varchar](10) NULL,
	[DateModified] [smalldatetime] NULL,
	[UpdateUserName] [varchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[VersionCount] [int] NOT NULL,
	[ContributingCompany] [varchar](50) NULL,
	[IDSourceCode] [varchar](50) NULL,
 CONSTRAINT [EmployeePhone_PK] PRIMARY KEY CLUSTERED 
(
	[EmployeePhoneID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[vLookupTranslation]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vLookupTranslation]
AS
SELECT     dbo.LookupTypeValueTranslation.Company, dbo.LookupTypeValueTranslation.CodeValue
FROM         dbo.LookupType INNER JOIN
                      dbo.LookupTypeValue ON dbo.LookupType.LookupTypeCode = dbo.LookupTypeValue.LookupTypeCode INNER JOIN
                      dbo.LookupTypeValueTranslation ON dbo.LookupTypeValue.LookupTypeCode = dbo.LookupTypeValueTranslation.LookupTypeCode AND 
                      dbo.LookupTypeValue.LookupTypeValue = dbo.LookupTypeValueTranslation.LookupTypeValue
WHERE     (dbo.LookupType.ExchangeUse = 1) AND (dbo.LookupType.LookupTypeCode = 'HairColor') AND (dbo.LookupTypeValue.LookupTypeValue = 'BRN')

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_OrderInformation4]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_OrderInformation4] ON [dbo].[OrderInformation]
(
	[OrderInformationID] ASC,
	[CustomerID] ASC,
	[OrderNumber] ASC,
	[OrderType] ASC,
	[RecordDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_OrderInformation5]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_OrderInformation5] ON [dbo].[OrderInformation]
(
	[OrderInformationID] ASC,
	[CustomerID] ASC,
	[OrderNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_OrderInformation6]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_OrderInformation6] ON [dbo].[OrderInformation]
(
	[CustomerID] ASC,
	[RecordDate] ASC
)
INCLUDE ( 	[OrderInformationID],
	[OrderNumber],
	[SourceCompany],
	[OrderType],
	[OrderTypeDescription],
	[OrderStatus],
	[CreateDate],
	[SourceID],
	[UpdateUserName],
	[UpdateDate],
	[VersionCount]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_OrderInformation7]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_OrderInformation7] ON [dbo].[OrderInformation]
(
	[CustomerID] ASC,
	[OrderType] ASC,
	[OrderNumber] ASC
)
INCLUDE ( 	[OrderInformationID],
	[SourceCompany],
	[OrderTypeDescription],
	[OrderStatus],
	[RecordDate],
	[CreateDate],
	[SourceID],
	[UpdateUserName],
	[UpdateDate],
	[VersionCount]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_OrderInformation8]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_OrderInformation8] ON [dbo].[OrderInformation]
(
	[CustomerID] ASC,
	[OrderNumber] ASC,
	[OrderType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_OrderInfoTypeDate]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_OrderInfoTypeDate] ON [dbo].[OrderInformation]
(
	[OrderInformationID] ASC,
	[OrderType] ASC,
	[RecordDate] ASC
)
INCLUDE ( 	[OrderNumber],
	[SourceCompany]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_OrderNumber]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_OrderNumber] ON [dbo].[OrderInformation]
(
	[OrderNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_RecordDate]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_RecordDate] ON [dbo].[OrderInformation]
(
	[RecordDate] DESC
)
INCLUDE ( 	[OrderInformationID],
	[CustomerID],
	[OrderNumber],
	[SourceCompany],
	[OrderType],
	[OrderTypeDescription],
	[OrderStatus],
	[CreateDate],
	[SourceID],
	[UpdateUserName],
	[UpdateDate],
	[VersionCount]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Customer_IDX1]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [Customer_IDX1] ON [dbo].[Customer]
(
	[Item] ASC,
	[CompanyID] ASC,
	[Company] ASC
)
INCLUDE ( 	[CustomerID],
	[InCustody],
	[SourceID],
	[DataSource],
	[UpdateUserName],
	[UpdateDate],
	[VersionCount]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Customer_IDX3]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [Customer_IDX3] ON [dbo].[Customer]
(
	[Item] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IDX_CustomerNameID]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_CustomerNameID] ON [dbo].[CustomerAddress]
(
	[CustomerNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IDX_EmployeeAddressID]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_EmployeeAddressID] ON [dbo].[CustomerAddress]
(
	[EmployeeAddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_Customer1]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_Customer1] ON [dbo].[CustomerAttribute]
(
	[AssignedIDType] ASC,
	[DateModified] DESC
)
INCLUDE ( 	[CustomerAttributeID],
	[CustomerNameID],
	[EmployeeAttributeID],
	[AssignedIDNumber],
	[AssignedIDState],
	[PrimaryID],
	[SourceID],
	[DataSource],
	[VersionCount],
	[ContributingCompany],
	[IDSourceCode]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_Customer2]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_Customer2] ON [dbo].[CustomerAttribute]
(
	[SourceID] ASC,
	[CustomerAttributeID] ASC
)
INCLUDE ( 	[CustomerNameID],
	[EmployeeAttributeID],
	[AssignedIDType],
	[AssignedIDNumber],
	[AssignedIDState],
	[PrimaryID],
	[DataSource],
	[VersionCount],
	[ContributingCompany],
	[DateModified],
	[IDSourceCode]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IDX_CustomerNameID]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_CustomerNameID] ON [dbo].[CustomerAttribute]
(
	[CustomerNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IDX_EmployeeAttributeID]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_EmployeeAttributeID] ON [dbo].[CustomerAttribute]
(
	[EmployeeAttributeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IDX_CustomerNameID]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_CustomerNameID] ON [dbo].[CustomerDescription]
(
	[CustomerNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IDX_EmployeeDescriptionID]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_EmployeeDescriptionID] ON [dbo].[CustomerDescription]
(
	[EmployeeDescriptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IDX_CustomerID_CustomerNameID]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_CustomerID_CustomerNameID] ON [dbo].[CustomerName]
(
	[CustomerID] ASC,
	[CustomerNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_CustomerName_LastName]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_CustomerName_LastName] ON [dbo].[CustomerName]
(
	[LastName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_CustomerName3]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_CustomerName3] ON [dbo].[CustomerName]
(
	[AliasIndicator] ASC,
	[CustomerID] ASC,
	[CustomerNameID] ASC,
	[LastName] ASC,
	[FirstName] ASC,
	[MiddleName] ASC,
	[SuffixName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_CustomerName4]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_CustomerName4] ON [dbo].[CustomerName]
(
	[CustomerNameID] ASC
)
INCLUDE ( 	[CustomerID],
	[EmployeeNameID],
	[Item],
	[FirstName],
	[MiddleName],
	[LastName],
	[SuffixName],
	[NameTitle],
	[BusinessName],
	[DOB],
	[DeathDate],
	[NameType],
	[PhoneticLastName],
	[AliasIndicator],
	[DataSource],
	[CreateDate],
	[CreateUserName],
	[SourceID],
	[ValidationIndicator],
	[ValidationComments],
	[DateModified],
	[Note],
	[VersionCount],
	[ContributingCompany],
	[IDSourceCode]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IDX_CustomerName5]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_CustomerName5] ON [dbo].[CustomerName]
(
	[CustomerNameID] ASC
)
INCLUDE ( 	[EmployeeNameID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_CustomerName6]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_CustomerName6] ON [dbo].[CustomerName]
(
	[SourceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_CustomerName7]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_CustomerName7] ON [dbo].[CustomerName]
(
	[NameType] ASC,
	[CustomerID] ASC,
	[CustomerNameID] ASC,
	[Item] ASC
)
INCLUDE ( 	[EmployeeNameID],
	[FirstName],
	[MiddleName],
	[LastName],
	[SuffixName],
	[NameTitle],
	[BusinessName],
	[DOB],
	[DeathDate],
	[PhoneticLastName],
	[SoundexFirstName],
	[SoundexLastName],
	[AliasIndicator],
	[DataSource],
	[CreateDate],
	[CreateUserName],
	[SourceID],
	[ValidationIndicator],
	[ValidationComments],
	[DateModified],
	[Note],
	[UpdateUserName],
	[UpdateDate],
	[VersionCount],
	[ContributingCompany],
	[IDSourceCode]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_CustomerName8]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_CustomerName8] ON [dbo].[CustomerName]
(
	[CustomerID] ASC,
	[NameType] ASC,
	[Item] ASC,
	[CustomerNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IDX_EmployeeNameID]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_EmployeeNameID] ON [dbo].[CustomerName]
(
	[EmployeeNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IDX_CustomerNameID]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_CustomerNameID] ON [dbo].[CustomerParent]
(
	[CustomerNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IDX_EmployeeParentID]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_EmployeeParentID] ON [dbo].[CustomerParent]
(
	[EmployeeParentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IDX_CustomerNameID]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_CustomerNameID] ON [dbo].[CustomerPhone]
(
	[CustomerNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IDX_EmployeePhoneID]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_EmployeePhoneID] ON [dbo].[CustomerPhone]
(
	[EmployeePhoneID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_OrderReferredOrder1]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_OrderReferredOrder1] ON [dbo].[OrderReferredOrder]
(
	[OrderInformationID] ASC,
	[CompanyOrderNumber] ASC,
	[Company] ASC,
	[CompanyDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDXOrderReferredOrder2]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDXOrderReferredOrder2] ON [dbo].[OrderReferredOrder]
(
	[OrderInformationID] ASC,
	[CompanyOrderNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDXOrderReferredOrder3]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDXOrderReferredOrder3] ON [dbo].[OrderReferredOrder]
(
	[OrderInformationID] ASC,
	[CompanyOrderNumber] ASC
)
INCLUDE ( 	[ReferredOrderID],
	[Company],
	[CompanyDate],
	[SourceID],
	[UpdateUserName],
	[UpdateDate],
	[VersionCount]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_LookupTypeValue]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_LookupTypeValue] ON [dbo].[LookupTypeValue]
(
	[LookupTypeCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UniqueValues]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_UniqueValues] ON [dbo].[LookupTypeValue]
(
	[LookupTypeCode] ASC,
	[LookupTypeValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_MatchMerge]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_MatchMerge] ON [dbo].[MatchMerge]
(
	[Action] ASC,
	[FromLastName] ASC,
	[FromFirstName] ASC,
	[FromItem] ASC,
	[ToLastName] ASC,
	[ToFirstName] ASC,
	[ToItem] ASC,
	[UpdateDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [EmployeeAddress_IDX]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [EmployeeAddress_IDX] ON [dbo].[EmployeeAddress]
(
	[EmployeeNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [EmployeeAttribute_IDX]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [EmployeeAttribute_IDX] ON [dbo].[EmployeeAttribute]
(
	[AssignedIDType] ASC,
	[AssignedIDNumber] ASC,
	[AssignedIDState] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [EmployeeAttribute_IDX3]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [EmployeeAttribute_IDX3] ON [dbo].[EmployeeAttribute]
(
	[EmployeeNameID] ASC
)
INCLUDE ( 	[EmployeeAttributeID],
	[AssignedIDType],
	[AssignedIDNumber],
	[AssignedIDState],
	[SourceID],
	[DataSource],
	[UpdateUserName],
	[UpdateDate],
	[VersionCount]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [EmployeeAttribute_IDX4]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [EmployeeAttribute_IDX4] ON [dbo].[EmployeeAttribute]
(
	[AssignedIDType] ASC,
	[AssignedIDNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IDX_EmployeeNameID]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_EmployeeNameID] ON [dbo].[EmployeeDescription]
(
	[EmployeeNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [_dta_index_EmployeeName_12_1387151987__K2_K1_3_4_5_6_7_8_9_10_11_12_13_16_17_18_19_20_21_22_25]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [_dta_index_EmployeeName_12_1387151987__K2_K1_3_4_5_6_7_8_9_10_11_12_13_16_17_18_19_20_21_22_25] ON [dbo].[EmployeeName]
(
	[Item] ASC,
	[EmployeeNameID] ASC
)
INCLUDE ( 	[FirstName],
	[MiddleName],
	[LastName],
	[SuffixName],
	[NameTitle],
	[BusinessName],
	[DOB],
	[DeathDate],
	[NameType],
	[PhoneticLastName],
	[AliasIndicator],
	[DataSource],
	[CreateDate],
	[CreateUserName],
	[SourceID],
	[ValidationIndicator],
	[ValidationComments],
	[VersionCount]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_EmployeeName]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_EmployeeName] ON [dbo].[EmployeeName]
(
	[LastName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [EmployeeName_DOB]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [EmployeeName_DOB] ON [dbo].[EmployeeName]
(
	[DOB] ASC,
	[Item] ASC,
	[EmployeeNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [EmployeeName_IDX]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [EmployeeName_IDX] ON [dbo].[EmployeeName]
(
	[NameType] ASC,
	[LastName] ASC,
	[FirstName] ASC,
	[MiddleName] ASC,
	[Item] ASC,
	[EmployeeNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [EmployeeName_IDX_LastName]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [EmployeeName_IDX_LastName] ON [dbo].[EmployeeName]
(
	[LastName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [EmployeeName_IDX2]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [EmployeeName_IDX2] ON [dbo].[EmployeeName]
(
	[NameType] ASC,
	[Item] ASC,
	[FirstName] ASC,
	[MiddleName] ASC,
	[LastName] ASC,
	[DOB] ASC,
	[PhoneticLastName] ASC,
	[EmployeeNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [EmployeeName_IDX3]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [EmployeeName_IDX3] ON [dbo].[EmployeeName]
(
	[NameType] ASC,
	[Item] ASC,
	[FirstName] ASC,
	[MiddleName] ASC,
	[LastName] ASC,
	[AliasIndicator] ASC,
	[EmployeeNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [EmployeeName_IDX4]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [EmployeeName_IDX4] ON [dbo].[EmployeeName]
(
	[FirstName] ASC,
	[EmployeeNameID] ASC,
	[LastName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [EmployeeName_Item]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [EmployeeName_Item] ON [dbo].[EmployeeName]
(
	[Item] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/****** Object:  Index [IDX_EmployeeNameID]    Script Date: 12/30/2015 1:19:45 PM ******/
CREATE NONCLUSTERED INDEX [IDX_EmployeeNameID] ON [dbo].[EmployeePhone]
(
	[EmployeeNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
ALTER TABLE [dbo].[audit_LogParameter]  WITH CHECK ADD  CONSTRAINT [FK_audit_parameterLog_audit_queryLog] FOREIGN KEY([id])
REFERENCES [dbo].[audit_LogQuery] ([id])
GO
ALTER TABLE [dbo].[audit_LogParameter] CHECK CONSTRAINT [FK_audit_parameterLog_audit_queryLog]
GO
ALTER TABLE [dbo].[OrderInformation]  WITH CHECK ADD  CONSTRAINT [FK_OrderInformation_Customer] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO
ALTER TABLE [dbo].[OrderInformation] CHECK CONSTRAINT [FK_OrderInformation_Customer]
GO
ALTER TABLE [dbo].[CustomerAddress]  WITH CHECK ADD  CONSTRAINT [FK_CustomerAddress_CustomerName] FOREIGN KEY([CustomerNameID])
REFERENCES [dbo].[CustomerName] ([CustomerNameID])
GO
ALTER TABLE [dbo].[CustomerAddress] CHECK CONSTRAINT [FK_CustomerAddress_CustomerName]
GO
ALTER TABLE [dbo].[CustomerAddress]  WITH CHECK ADD  CONSTRAINT [FK_CustomerAddress_EmployeeAddress] FOREIGN KEY([EmployeeAddressID])
REFERENCES [dbo].[EmployeeAddress] ([EmployeeAddressID])
GO
ALTER TABLE [dbo].[CustomerAddress] CHECK CONSTRAINT [FK_CustomerAddress_EmployeeAddress]
GO
ALTER TABLE [dbo].[CustomerAttribute]  WITH CHECK ADD  CONSTRAINT [FK_CustomerAttribute_CustomerName] FOREIGN KEY([CustomerNameID])
REFERENCES [dbo].[CustomerName] ([CustomerNameID])
GO
ALTER TABLE [dbo].[CustomerAttribute] CHECK CONSTRAINT [FK_CustomerAttribute_CustomerName]
GO
ALTER TABLE [dbo].[CustomerAttribute]  WITH CHECK ADD  CONSTRAINT [FK_CustomerAttribute_EmployeeAttribute] FOREIGN KEY([EmployeeAttributeID])
REFERENCES [dbo].[EmployeeAttribute] ([EmployeeAttributeID])
GO
ALTER TABLE [dbo].[CustomerAttribute] CHECK CONSTRAINT [FK_CustomerAttribute_EmployeeAttribute]
GO
ALTER TABLE [dbo].[CustomerDescription]  WITH CHECK ADD  CONSTRAINT [FK_CustomerDescription_CustomerName] FOREIGN KEY([CustomerNameID])
REFERENCES [dbo].[CustomerName] ([CustomerNameID])
GO
ALTER TABLE [dbo].[CustomerDescription] CHECK CONSTRAINT [FK_CustomerDescription_CustomerName]
GO
ALTER TABLE [dbo].[CustomerDescription]  WITH CHECK ADD  CONSTRAINT [FK_CustomerDescription_EmployeeDescription] FOREIGN KEY([EmployeeDescriptionID])
REFERENCES [dbo].[EmployeeDescription] ([EmployeeDescriptionID])
GO
ALTER TABLE [dbo].[CustomerDescription] CHECK CONSTRAINT [FK_CustomerDescription_EmployeeDescription]
GO
ALTER TABLE [dbo].[CustomerName]  WITH CHECK ADD  CONSTRAINT [FK_CustomerName_Customer] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO
ALTER TABLE [dbo].[CustomerName] CHECK CONSTRAINT [FK_CustomerName_Customer]
GO
ALTER TABLE [dbo].[CustomerName]  WITH CHECK ADD  CONSTRAINT [FK_CustomerName_EmployeeName] FOREIGN KEY([EmployeeNameID])
REFERENCES [dbo].[EmployeeName] ([EmployeeNameID])
GO
ALTER TABLE [dbo].[CustomerName] CHECK CONSTRAINT [FK_CustomerName_EmployeeName]
GO
ALTER TABLE [dbo].[CustomerParent]  WITH CHECK ADD  CONSTRAINT [FK_CustomerParent_CustomerName] FOREIGN KEY([CustomerNameID])
REFERENCES [dbo].[CustomerName] ([CustomerNameID])
GO
ALTER TABLE [dbo].[CustomerParent] CHECK CONSTRAINT [FK_CustomerParent_CustomerName]
GO
ALTER TABLE [dbo].[CustomerParent]  WITH CHECK ADD  CONSTRAINT [FK_CustomerParent_EmployeeParent] FOREIGN KEY([EmployeeParentID])
REFERENCES [dbo].[EmployeeParent] ([EmployeeParentID])
GO
ALTER TABLE [dbo].[CustomerParent] CHECK CONSTRAINT [FK_CustomerParent_EmployeeParent]
GO
ALTER TABLE [dbo].[CustomerPhone]  WITH CHECK ADD  CONSTRAINT [FK_CustomerPhone_CustomerName] FOREIGN KEY([CustomerNameID])
REFERENCES [dbo].[CustomerName] ([CustomerNameID])
GO
ALTER TABLE [dbo].[CustomerPhone] CHECK CONSTRAINT [FK_CustomerPhone_CustomerName]
GO
ALTER TABLE [dbo].[CustomerPhone]  WITH CHECK ADD  CONSTRAINT [FK_CustomerPhone_EmployeePhone] FOREIGN KEY([EmployeePhoneID])
REFERENCES [dbo].[EmployeePhone] ([EmployeePhoneID])
GO
ALTER TABLE [dbo].[CustomerPhone] CHECK CONSTRAINT [FK_CustomerPhone_EmployeePhone]
GO
ALTER TABLE [dbo].[OrderReferredOrder]  WITH CHECK ADD  CONSTRAINT [FK_OrderReferredOrder_OrderInformation] FOREIGN KEY([OrderInformationID])
REFERENCES [dbo].[OrderInformation] ([OrderInformationID])
GO
ALTER TABLE [dbo].[OrderReferredOrder] CHECK CONSTRAINT [FK_OrderReferredOrder_OrderInformation]
GO
ALTER TABLE [dbo].[LookupTypeValue]  WITH CHECK ADD  CONSTRAINT [fk_LookupTypeValue] FOREIGN KEY([LookupTypeCode])
REFERENCES [dbo].[LookupType] ([LookupTypeCode])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LookupTypeValue] CHECK CONSTRAINT [fk_LookupTypeValue]
GO
ALTER TABLE [dbo].[LookupTypeValueTranslation]  WITH CHECK ADD  CONSTRAINT [fk_LookupTypeValueTranslation] FOREIGN KEY([LookupTypeCode], [LookupTypeValue])
REFERENCES [dbo].[LookupTypeValue] ([LookupTypeCode], [LookupTypeValue])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LookupTypeValueTranslation] CHECK CONSTRAINT [fk_LookupTypeValueTranslation]
GO
ALTER TABLE [dbo].[EmployeeAddress]  WITH CHECK ADD  CONSTRAINT [EmployeeName_EmployeeAddress_FK1] FOREIGN KEY([EmployeeNameID])
REFERENCES [dbo].[EmployeeName] ([EmployeeNameID])
GO
ALTER TABLE [dbo].[EmployeeAddress] CHECK CONSTRAINT [EmployeeName_EmployeeAddress_FK1]
GO
ALTER TABLE [dbo].[EmployeeAttribute]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeAttribute_EmployeeName] FOREIGN KEY([EmployeeNameID])
REFERENCES [dbo].[EmployeeName] ([EmployeeNameID])
GO
ALTER TABLE [dbo].[EmployeeAttribute] CHECK CONSTRAINT [FK_EmployeeAttribute_EmployeeName]
GO
ALTER TABLE [dbo].[EmployeeDescription]  WITH CHECK ADD  CONSTRAINT [EmployeeName_EmployeeDescription_FK1] FOREIGN KEY([EmployeeNameID])
REFERENCES [dbo].[EmployeeName] ([EmployeeNameID])
GO
ALTER TABLE [dbo].[EmployeeDescription] CHECK CONSTRAINT [EmployeeName_EmployeeDescription_FK1]
GO
ALTER TABLE [dbo].[EmployeeName]  WITH CHECK ADD  CONSTRAINT [ItemSource_EmployeeName_FK1] FOREIGN KEY([Item])
REFERENCES [dbo].[ItemSource] ([Item])
GO
ALTER TABLE [dbo].[EmployeeName] CHECK CONSTRAINT [ItemSource_EmployeeName_FK1]
GO
ALTER TABLE [dbo].[EmployeeParent]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeName_EmployeeParent] FOREIGN KEY([EmployeeNameID])
REFERENCES [dbo].[EmployeeName] ([EmployeeNameID])
GO
ALTER TABLE [dbo].[EmployeeParent] CHECK CONSTRAINT [FK_EmployeeName_EmployeeParent]
GO
ALTER TABLE [dbo].[EmployeePhone]  WITH CHECK ADD  CONSTRAINT [EmployeeName_EmployeePhone_FK1] FOREIGN KEY([EmployeeNameID])
REFERENCES [dbo].[EmployeeName] ([EmployeeNameID])
GO
ALTER TABLE [dbo].[EmployeePhone] CHECK CONSTRAINT [EmployeeName_EmployeePhone_FK1]
GO
/****** Object:  StoredProcedure [dbo].[_BookingTylerOrders]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[_BookingTylerOrders]
  @violationDate date = null
as
begin

	SET NOCOUNT ON;

if(@violationDate is null)
  begin
	select booking.*, court.OrderNumber from OrderInformation booking, OrderInformation court, Customer bookingEmployee, Customer courtEmployee
	where booking.CustomerID = bookingEmployee.CustomerID
		and bookingEmployee.Item = courtEmployee.Item
		and courtEmployee.CustomerID = court.CustomerID
		and booking.OrderType = 'booking'
		and court.SourceCompany = 'odyssey'
  end
else
  begin
	select booking.*, court.OrderNumber from OrderInformation booking, OrderInformation court, Customer bookingEmployee, Customer courtEmployee
	where booking.RecordDate >= @violationDate
		and (booking.OrderStatus between convert(varchar(10),@violationDate,112) and convert(varchar(10),getdate(),112) or booking.OrderStatus is null or booking.OrderStatus = '')
		and booking.CustomerID = bookingEmployee.CustomerID
		and bookingEmployee.Item = courtEmployee.Item
		and courtEmployee.CustomerID = court.CustomerID
		and booking.OrderType = 'booking'
		and court.SourceCompany = 'odyssey'
  end
end

GO
/****** Object:  StoredProcedure [dbo].[_completeDataBaseDelete]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[_completeDataBaseDelete]

AS
begin
set nocount on;
delete CustomerDescription
delete CustomerAttribute
delete CustomerAddress
delete CustomerPhone
delete CustomerParent
delete OrderReferredOrder
delete caseinformation
delete CustomerName
delete Customer

delete EmployeeDescription
delete EmployeeAttribute
delete EmployeeAddress
delete EmployeePhone
delete EmployeeParent
delete EmployeeName
delete ItemSource

delete cjishubexchangehelper.dbo.rulesfired
delete cjishubexchangehelper.dbo.exchangedocument
delete cjishubexchangehelper.dbo.notificationdata
end

GO
/****** Object:  StoredProcedure [dbo].[_completeEmployeeDelete]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[_completeEmployeeDelete]
	@Item char(10)
AS
begin
	set nocount on;
	delete from OrderReferredOrder where OrderInformationID in 
		(select OrderInformationID from OrderInformation where CustomerID in 
		(select CustomerID from Customer where Item = @Item))
		
    delete from OrderInformation where CustomerID in 
    	(select CustomerID from Customer where Item = @Item)	
    	
   delete from CustomerParent where CustomerNameID in 
   (select CustomerNameID from CustomerName where CustomerID in 
   (select CustomerID from Customer where Item = @Item)) 		
   
   delete from CustomerPhone where CustomerNameID in 
   (select CustomerNameID from CustomerName where CustomerID in 
   (select CustomerID from Customer where Item = @Item)) 		
   
   delete from CustomerAddress where CustomerNameID in 
   (select CustomerNameID from CustomerName where CustomerID in 
   (select CustomerID from Customer where Item = @Item)) 		

   delete from CustomerDescription where CustomerNameID in 
   (select CustomerNameID from CustomerName where CustomerID in 
   (select CustomerID from Customer where Item = @Item)) 		

   delete from CustomerAttribute where CustomerNameID in 
   (select CustomerNameID from CustomerName where CustomerID in 
   (select CustomerID from Customer where Item = @Item)) 		

   delete from CustomerName where CustomerID in 
   (select CustomerID from Customer where Item = @Item)

   delete from Customer where Item = @Item
      	
	delete from EmployeeParent where EmployeeNameID in 
		(select Employeenameid from EmployeeName where Item = @Item)        	
    	
	delete from EmployeeAddress where EmployeeNameID in 
		(select Employeenameid from EmployeeName where Item = @Item)        	
    	
	delete from EmployeePhone where EmployeeNameID in 
		(select Employeenameid from EmployeeName where Item = @Item)        	
    	
	delete from EmployeeDescription where EmployeeNameID in 
		(select Employeenameid from EmployeeName where Item = @Item)        	
    	
	delete from EmployeeAttribute where EmployeeNameID in 
		(select Employeenameid from EmployeeName where Item = @Item)        	
		
	delete from EmployeeName where Item = @Item					
	
	delete from ItemSource where Item = @Item	
end
GO
/****** Object:  StoredProcedure [dbo].[CompanyOrderDump]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[CompanyOrderDump]
	 @Company varchar(255) = null
	,@CompanyID varchar(255) = null 
	,@CompanyOrderNumber varchar(255) = null
AS
BEGIN
	SET NOCOUNT ON;

	IF ((@Company is null and @CompanyID is null) and (@CompanyOrderNumber is null))
		return;		

	declare @cpid int
	declare @count int

	if (@CompanyOrderNumber is null)
	    begin
			set @cpid = (select Customerid from Customer where Company = @Company and CompanyID = @CompanyID)
			set @count = 1
		end
	else
		begin
			set @count = (select count(Customerid) from OrderInformation where OrderNumber = @CompanyOrderNumber)
			if (@count > 1)
				Select Item,Company,CompanyID from Customer where CustomerID in (select Customerid from OrderInformation where OrderNumber = @CompanyOrderNumber)
			else
				set @cpid = (select Customerid from OrderInformation where OrderNumber = @CompanyOrderNumber)	
		end;
			
	if (@count < 2)
	    begin
			set @count = (select count(*) from Customer where Customerid = @cpid);
			if (@count > 0)
				select 'Employee' [Employee],sourceid, datasource,* from Customer where Customerid = @cpid;
	
			set @count = (select count(*) from Customername where Customerid = @cpid and AliasIndicator != 'Y');
			if (@count > 0)
				select 'Name' [Name],sourceid, datasource,* from Customername where Customerid = @cpid and AliasIndicator != 'Y';
	
			set @count = (select count(*) from Customerparent where Customernameid in (select Customernameid from Customername where Customerid = @cpid));
			if (@count > 0)
				select 'Parent' [Parent], sourceid, datasource,* from Customerparent where Customernameid in (select Customernameid from Customername where Customerid = @cpid);

			set @count = (select count(*) from Customername where Customerid = @cpid and AliasIndicator = 'Y');
			if (@count > 0)
				select 'Alias' [Alias],sourceid, datasource,* from Customername where Customerid = @cpid and AliasIndicator = 'Y';

			set @count = (select count(*) from Customerdescription where Customernameid in (select Customernameid from Customername where Customerid = @cpid));
			if (@count > 0)
				select 'Desc' [Desc],sourceid, datasource,* from Customerdescription  
					where Customernameid in (select Customernameid from Customername where Customerid = @cpid);

			set @count = (select count(*) from Customerattribute where Customernameid in (select Customernameid from Customername where Customerid = @cpid));
			if (@count > 0)
				select 'Att' [Att],sourceid, datasource,* from Customerattribute 
					where Customernameid in (select Customernameid from Customername where Customerid = @cpid);

			set @count = (select count(*) from Customeraddress where Customernameid in (select Customernameid from Customername where Customerid = @cpid));
			if (@count > 0)
				select 'Adr' [Adr],sourceid, datasource, Customeraddressid, Customernameid, Employeeaddressid, addresstype, addressline1, city, state, postalcode, addressline2, addressline3, * from Customeraddress   
					where Customernameid in (select Customernameid from Customername where Customerid = @cpid);

			set @count = (select count(*) from Customerphone where Customernameid in (select Customernameid from Customername where Customerid = @cpid));
			if (@count > 0)
				select 'Phn' [Phn],sourceid, datasource,* from Customerphone   
					where Customernameid in (select Customernameid from Customername where Customerid = @cpid);
		
			set @count = (select count(*) from caseinformation where Customerid = @cpid);
			if (@count > 0)
				select 'Order' [Order],sourceid, sourceCompany,* from caseinformation   where Customerid = @cpid;
		
			set @count = (select count(*) from caseinformation where Customerid = @cpid);
			if (@count > 0)
				select 'ReferredOrder' [ReferOrder],sourceid, * from OrderReferredOrder  where OrderInformationID in (select OrderInformationID from caseinformation where Customerid = @cpid);
		end;		
END

GO
/****** Object:  StoredProcedure [dbo].[AliasEmployeeName_ExcludeEmployeeNameID]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AliasEmployeeName_ExcludeEmployeeNameID]
	@EmployeeNameID int
	,@Item char(10)
	,@userName varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@Item is NULL)
		return;

   	if (@username is not null)
	begin
		exec audit_LogSelect_CREATE @storedProcedureName='AliasEmployeeName_ExcludeEmployeeNameID', @parameterName='EmployeeNameID', @parameterValue=@EmployeeNameID, @userName=@userName;
		exec audit_LogSelect_CREATE @storedProcedureName='AliasEmployeeName_ExcludeEmployeeNameID', @parameterName='Item', @parameterValue=@Item, @userName=@userName;
	end

	SELECT [EmployeeNameID]
      ,[Item]
      ,[FirstName]
      ,[MiddleName]
      ,[LastName]
      ,[SuffixName]
      ,[NameTitle]
      ,[BusinessName]
      ,[DOB]
      ,[DeathDate]
      ,[NameType]
      ,[PhoneticLastName]
      ,[AliasIndicator]
      ,[DataSource]
      ,[CreateDate]
      ,[CreateUserName]
	  ,[SourceID]
	  ,[ValidationIndicator]
	  ,[ValidationComments]
	  ,[DateModified]
	  ,[Note]
      ,[VersionCount]
	  ,[ContributingCompany]
	  ,[IDSourceCode]
	FROM [dbo].[EmployeeName]
	WHERE [Item] = @Item
		and [EmployeeNameID] <> @EmployeeNameID
	order by [DateModified] desc
END

GO
/****** Object:  StoredProcedure [dbo].[allDataDump]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[allDataDump]
	 @item varchar(10)
AS
BEGIN
	SET NOCOUNT ON;
	declare @count int
	
	set @count = (select count(*) from Customer where item = @item);
	if (@count > 0)
		select 'Customer' [Customer], sourceid, * from Customer where item = @item order by Customer.sourceid;

	set @count = (select count(*) from caseinformation where Customerid in (select Customerid from Customer where item = @item));
	if (@count > 0)
		select 'OrderInformation' [OrderInformation], sourceid, * from caseinformation
		where Customerid in (select Customerid from Customer where item = @item) order by caseinformation.SourceID;

	set @count = (select count(*) from casereferredcase where caseinformationid in (select caseinformationid from caseinformation where Customerid in (select Customerid from Customer where item = @item)));
	if (@count > 0)
		select 'OrderReferredOrder' [OrderReferredOrder], sourceid, * from casereferredcase
		where caseinformationid in (select caseinformationid from caseinformation where Customerid in (select Customerid from Customer where item = @item)) order by casereferredcase.sourceid;

	set @count = (select count(*) from Customername where item = @item);
	if (@count > 0)
		select 'CustomerName' [CustomerName], sourceid, * from Customername where item = @item order by Customername.sourceid;

	set @count = (select count(*) from Customerattribute where Customernameid in (select Customernameid from Customername where item = @item));
	if (@count > 0)
		select 'CustomerAttribute' [CustomerAttribute], sourceid, * from Customerattribute
		where Customernameid in (select Customernameid from Customername where item = @item) order by Customerattribute.sourceid;

	set @count = (select count(*) from Customerdescription where Customernameid in (select Customernameid from Customername where item = @item));
	if (@count > 0)
		select 'CustomerDescription' [CustomerDescription], sourceid, * from Customerdescription
		where Customernameid in (select Customernameid from Customername where item = @item) order by Customerdescription.sourceid;

	set @count = (select count(*) from Customeraddress where Customernameid in (select Customernameid from Customername where item = @item));
	if (@count > 0)
		select 'CustomerAddress' [CustomerAddress], sourceid, * from Customeraddress
		where Customernameid in (select Customernameid from Customername where item = @item) order by Customeraddress.sourceid;

	set @count = (select count(*) from Customerphone where Customernameid in (select Customernameid from Customername where item = @item));
	if (@count > 0)
		select 'CustomerPhone' [CustomerPhone], sourceid, * from Customerphone
		where Customernameid in (select Customernameid from Customername where item = @item) order by Customerphone.sourceid;

	set @count = (select count(*) from Customerparent where Customernameid in (select Customernameid from Customername where item = @item));
	if (@count > 0)
		select 'CustomerParent' [CustomerParent], sourceid, * from Customerparent
		where Customernameid in (select Customernameid from Customername where item = @item) order by Customerparent.sourceid;

	set @count = (select count(*) from Employeename where item = @item);
	if (@count > 0)
		select 'Name' [Name], sourceid, * from Employeename where item = @item order by Employeename.sourceid;
	
	set @count = (select count(*) from Employeeattribute where Employeenameid in (select Employeenameid from Employeename where item = @item));
	if (@count > 0)
		select 'Attribute' [Attribute], sourceid, * from Employeeattribute 
		where Employeenameid in (select Employeenameid from Employeename where item = @item) order by Employeeattribute.sourceid, assignedIDType, updatedate;
	
	set @count = (select count(*) from Employeedescription where Employeenameid in (select Employeenameid from Employeename where item = @item));
	if (@count > 0)
		select 'Description' [Description], sourceid, * from Employeedescription 
		where Employeenameid in (select Employeenameid from Employeename where item = @item) order by Employeedescription.SourceID;
	
	set @count = (select count(*) from Employeeaddress where Employeenameid in (select Employeenameid from Employeename where item = @item));
	if (@count > 0)
		select 'Address' [Address], sourceid, * from Employeeaddress 
		where Employeenameid in (select Employeenameid from Employeename where item = @item) order by Employeeaddress.SourceID;
	
	set @count = (select count(*) from Employeephone where Employeenameid in (select Employeenameid from Employeename where item = @item));
	if (@count > 0)
		select 'Phone' [Phone], sourceid, * from Employeephone 
		where Employeenameid in (select Employeenameid from Employeename where item = @item) order by Employeephone.SourceID;
	
	set @count = (select count(*) from Employeeparent where Employeenameid in (select Employeenameid from Employeename where item = @item));
	if (@count > 0)
		select 'Parent' [Parent], sourceid, * from Employeeparent 
		where Employeenameid in (select Employeenameid from Employeename where item = @item) order by Employeeparent.SourceID;

	set @count = (select count(*) from Item_merge_log where toitem = @item or fromitem = @item);
	if (@count > 0)
		select 'MergeLog' [MergeLog], * from Item_merge_log where  toitem = @item or fromitem = @item;
		
	select 'ItemSource' [ItemSource], * from Itemsource where item = @item;
END

GO
/****** Object:  StoredProcedure [dbo].[audit_LogParameter_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[audit_LogParameter_CREATE]
	@id int
	,@parameter varchar(100)
	,@value varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

	insert into [dbo].[audit_logParameter]
           ([id]
           ,[parameterName]
           ,[parameterValue])
     values
           (@id
           ,@parameter
           ,@value)
END

GO
/****** Object:  StoredProcedure [dbo].[audit_LogQuery_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[audit_LogQuery_CREATE]
	@storedProcedureName varchar(100)
	,@userName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	insert into [dbo].[audit_LogQuery]
		([procedureName]
		,[userName]
		,[date])
	values
		(@storedProcedureName
		,@userName
		,getDate())

	if @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	else 
		select 0
END

GO
/****** Object:  StoredProcedure [dbo].[audit_LogSelect_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[audit_LogSelect_CREATE]
	@storedProcedureName varchar(100)
	,@parameterName varchar(100) = null
	,@parameterValue varchar(100) = null
	,@userName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	insert into [dbo].[audit_logQuery]
		([procedureName]
		,[userName]
		,[date])
	values
		(@storedProcedureName
		,@userName
		,getDate())

	declare @id int
	set @id = (select scope_identity())

	if (@parameterValue is not null and @parameterValue <> '')
	begin
		insert into [dbo].[audit_logParameter]
			   ([id]
			   ,[parameterName]
			   ,[parameterValue])
		 values
			   (@id
			   ,@parameterName
			   ,@parameterValue)
	end
END

GO
/****** Object:  StoredProcedure [dbo].[audit_LogSelect_Read]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Michelle/Lyle		
-- Create date: 2/2009
-- Description:	Query the audit/Log table. This stored procedure is not used by CJIS portal. This proc is only used for testing or
--              for ad hoc reporting.  
-- =============================================

CREATE PROCEDURE [dbo].[audit_LogSelect_Read]
	 @name varchar(100) = null
	,@procedureName varchar(100) = null  
	,@parameterValue varchar(100) = null
	,@startDate datetime = null
	,@endDate datetime = null
AS
BEGIN
	SET NOCOUNT ON;

	declare @start dateTime;
	declare @end dateTime;

	-- set default dates
	set @start = (select min(date) from audit_LogQuery)
	set @end = (select max(date) from audit_LogQuery)

	if (@startDate is not null)
		set @start = @startDate;
--		set @start = dbo.DateTime(year(@startDate), month(@startDate), day(@startDate), 0, 0, 0);

	if (@endDate is not null)
		set @end = @endDate;
--		set @end = dbo.DateTime(year(@endDate), month(@endDate), day(@endDate), 24, 59, 0);

	select q.id, q.procedurename, p.parametername, p.parametervalue, q.username, q.date 
	from audit_LogQuery q 
		left outer join audit_logparameter p on q.id = p.id 
	where date between @start and @end and username = coalesce(@name, username)
		and (@procedureName is null or (procedureName like @procedureName + '%'))
		and (@parameterValue is null or (parameterValue = @parameterValue))
	order by q.id desc
END

GO
/****** Object:  StoredProcedure [dbo].[BusinessException_ByItem]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BusinessException_ByItem]
	  @Item char(10) 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [BusinessExceptionID]
		  ,[Item]
		  ,[Category]
		  ,[SubCategory]
		  ,[Reason]
		  ,[ValidationStatus]
		  ,[ValidationDate]
          ,[MatchedItem]
          ,[CustomerNameID]
		   ,[EmployeeNameID]
		   ,[NotificationID]
           ,[Priority]
          ,[DocProcessed]
          ,[Company]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[VersionCount]
	FROM [dbo].[BusinessException] (READUNCOMMITTED)
	WHERE [Item] = @Item
END












GO
/****** Object:  StoredProcedure [dbo].[BusinessException_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BusinessException_CREATE]
	  @Item char(10)
      ,@Category varchar(10) = null
      ,@SubCategory varchar(10) = null
      ,@Reason varchar(100) = null
      ,@ValidationStatus char(1) = null
      ,@ValidationDate datetime = null
      ,@UpdateUserName varchar(50)
	  ,@MatchedItem varchar(100)=null
	  ,@CustomerNameID int = null
	  ,@EmployeeNameID int = null
	  ,@NotificationID int 
	  ,@Priority int = 100
	  ,@DocProcessed char(1) = 'N'
	  ,@Company varchar(10) = null
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[BusinessException]
           ([Item]
           ,[Category]
           ,[SubCategory]
           ,[Reason]
           ,[ValidationStatus]
           ,[ValidationDate]
           ,[MatchedItem]
           ,[CustomerNameID]
           ,[EmployeeNameID]
           ,[NotificationID]
           ,[Priority]
           ,[DocProcessed]
		   ,[Company] 	
		   ,[CreateDate]
           ,[UpdateUserName]
           ,[UpdateDate]
           ,[VersionCount])
     VALUES
           (@Item
           ,@Category
           ,@SubCategory
           ,@Reason
           ,@ValidationStatus
           ,@ValidationDate
		   ,@MatchedItem
		   ,@CustomerNameID
		   ,@EmployeeNameID
		   ,@NotificationID
		   ,@Priority
		   ,@DocProcessed
		   ,@Company
           ,GETDATE()
           ,@UpdateUserName
           ,GETDATE()
           ,0)

	IF @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	ELSE 
		select 0

END















GO
/****** Object:  StoredProcedure [dbo].[BusinessException_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BusinessException_DELETE]
	  @BusinessExceptionID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[BusinessException]
	WHERE [BusinessExceptionID] = @BusinessExceptionID
END

GO
/****** Object:  StoredProcedure [dbo].[BusinessException_DuplicateCheckByItem]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BusinessException_DuplicateCheckByItem]
	  @Item char(10)            
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [BusinessExceptionID]
		  ,[Item]
		  ,[Category]
		  ,[SubCategory]
		  ,[Reason]
		  ,[ValidationStatus]
		  ,[ValidationDate]
          ,[MatchedItem]
          ,[CustomerNameID]
		   ,[EmployeeNameID]
		   ,[NotificationID]
           ,[Priority]
          ,[DocProcessed]
          ,[Company]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[VersionCount]
	FROM [dbo].[BusinessException] (READUNCOMMITTED)
	WHERE [Item] = @Item          
     
     AND DocProcessed = 'N'
END














GO
/****** Object:  StoredProcedure [dbo].[BusinessException_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BusinessException_READ]
	@BusinessExceptionID int
	,@userName varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;

   	if (@username is not null)
		exec audit_LogSelect_CREATE @storedProcedureName='BusinessException_READ', @parameterName='businessExceptionID', @parameterValue=@BusinessExceptionID, @userName=@userName;

	SELECT [BusinessExceptionID]
		  ,[Item]
		  ,[Category]
		  ,[SubCategory]
		  ,[Reason]
          ,[ValidationStatus]
		  ,[ValidationDate]
          ,[MatchedItem]
          ,[CustomerNameID]
		  ,[EmployeeNameID]
		  ,[NotificationID]
          ,[Priority]
          ,[DocProcessed]
          ,[Company]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[VersionCount]
	FROM [dbo].[BusinessException] (READUNCOMMITTED)
	WHERE [BusinessExceptionID] = @BusinessExceptionID
END














GO
/****** Object:  StoredProcedure [dbo].[BusinessException_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BusinessException_UPDATE]
	@BusinessExceptionID int
	,@Item char(10)
	,@Category varchar(10) = null
	,@SubCategory varchar(10) = null
	,@Reason varchar(100) = null
	,@ValidationStatus char(1) = null
	,@ValidationDate datetime = null
	,@MatchedItem varchar(100) = null
	,@CustomerNameID int = null
	,@EmployeeNameID int = null
	,@NotificationID int 
	,@Priority int
	,@DocProcessed char(1)
	,@Company varchar(10) = null
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[BusinessException]
	SET    [Item] = @Item 
           ,[Category] = @Category
           ,[SubCategory] = @SubCategory
           ,[Reason] = @Reason
           ,[ValidationStatus] = @ValidationStatus
           ,[ValidationDate] = @ValidationDate
		   ,[MatchedItem] = @MatchedItem
		   ,[CustomerNameID] = @CustomerNameID
		   ,[EmployeeNameID] = @EmployeeNameID
		   ,[NotificationID] = @NotificationID
		   ,[Priority] = @Priority
		   ,[DocProcessed] = @DocProcessed
           ,[Company] = @Company
           ,[UpdateUserName] = @UpdateUserName
           ,[UpdateDate] = GETDATE()
           ,[VersionCount] = [VersionCount] + 1
	WHERE  [BusinessExceptionID] = @BusinessExceptionID
END












GO
/****** Object:  StoredProcedure [dbo].[OrderDump]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Batch submitted through debugger: SQLQuery1.sql|7|0|C:\Users\rlittlefield\AppData\Local\Temp\~vsAE2D.sql
CREATE procedure [dbo].[OrderDump]
	 @OrderNumber varchar(25)
AS
BEGIN
	SET NOCOUNT ON;

	--declare @cpid int
	declare @count int
	
	-- new version - Employee based
	set @count = (select count(*) from Customer where Customerid in (select Customerid from caseinformation where casenumber = @OrderNumber));
	if (@count > 0)
		select 'Employee' [Employee],sourceid, datasource,* from Customer where Customerid in (select Customerid from caseinformation where casenumber = @OrderNumber);

	set @count = (select count(*) from caseinformation where Customerid in (select Customerid from caseinformation where casenumber = @OrderNumber));
	if (@count > 0)
		select 'Order' [Order],sourceid, sourceCompany,* from caseinformation where Customerid in (select Customerid from caseinformation where casenumber = @OrderNumber);
	
	set @count = (select count(*) from casereferredcase where caseinformationid in (select caseinformationid from caseinformation where casenumber = @OrderNumber));
	if (@count > 0)
		select 'Referred' [Referred],sourceid, * from casereferredcase where caseinformationid in (select caseinformationid from caseinformation where casenumber = @OrderNumber);
	
	set @count = (select count(*) from Customername where Customerid in (select Customerid from caseinformation where casenumber = @OrderNumber) and AliasIndicator != 'Y');
	if (@count > 0)
		select 'Name' [Name],sourceid, datasource,* from Customername where Customerid in (select Customerid from caseinformation where casenumber = @OrderNumber) and AliasIndicator != 'Y';
	
	set @count = (select count(*) from Customerparent where Customernameid in (select Customernameid from Customername where Customerid in (select Customerid from caseinformation where casenumber = @OrderNumber)));
	if (@count > 0)
		select 'Parent' [Parent], sourceid, datasource,* from Customerparent where Customernameid in (select Customernameid from Customername where Customerid in (select Customerid from caseinformation where casenumber = @OrderNumber));

	set @count = (select count(*) from Customername where Customerid in (select Customerid from caseinformation where casenumber = @OrderNumber) and AliasIndicator = 'Y');
	if (@count > 0)
		select 'Alias' [Alias],sourceid, datasource,* from Customername where Customerid in (select Customerid from caseinformation where casenumber = @OrderNumber) and AliasIndicator = 'Y';

	set @count = (select count(*) from Customeraddress where Customernameid in (select Customernameid from Customername where Customerid in (select Customerid from caseinformation where casenumber = @OrderNumber)));
	if (@count > 0)
		select 'Adr' [Adr],sourceid, datasource, Customeraddressid, Customernameid, Employeeaddressid, addresstype, addressline1, city, state, postalcode, addressline2, addressline3, * from Customeraddress   
		where Customernameid in (select Customernameid from Customername where Customerid in (select Customerid from caseinformation where casenumber = @OrderNumber));

	set @count = (select count(*) from Customerattribute where Customernameid in (select Customernameid from Customername where Customerid in (select Customerid from caseinformation where casenumber = @OrderNumber)));
	if (@count > 0)
		select 'Att' [Att],sourceid, datasource,* from Customerattribute 
		where Customernameid in (select Customernameid from Customername where Customerid in (select Customerid from caseinformation where casenumber = @OrderNumber));

	set @count = (select count(*) from Customerdescription where Customernameid in (select Customernameid from Customername where Customerid in (select Customerid from caseinformation where casenumber = @OrderNumber)));
	if (@count > 0)
		select 'Desc' [Desc],sourceid, datasource,* from Customerdescription  
		where Customernameid in (select Customernameid from Customername where Customerid in (select Customerid from caseinformation where casenumber = @OrderNumber));

	set @count = (select count(*) from Customerphone where Customernameid in (select Customernameid from Customername where Customerid in (select Customerid from caseinformation where casenumber = @OrderNumber)));
	if (@count > 0)
		select 'Phn' [Phn],sourceid, datasource,* from Customerphone   
		where Customernameid in (select Customernameid from Customername where Customerid in (select Customerid from caseinformation where casenumber = @OrderNumber));	

/* previous version - case based
	set @cpid = (select Customerid from caseinformation where casenumber = @OrderNumber)
	
	set @count = (select count(*) from Customer where Customerid = @cpid);
	if (@count > 0)
		select 'Employee' [Employee],sourceid, datasource,* from Customer where Customerid = @cpid;

	set @count = (select count(*) from caseinformation where Customerid = @cpid);
	if (@count > 0)
		select 'Order' [Order],sourceid, sourceCompany,* from caseinformation   where Customerid = @cpid;
	
	set @count = (select count(*) from Customername where Customerid = @cpid and AliasIndicator != 'Y');
	if (@count > 0)
		select 'Name' [Name],sourceid, datasource,* from Customername where Customerid = @cpid and AliasIndicator != 'Y';
	
	set @count = (select count(*) from Customerparent where Customernameid in (select Customernameid from Customername where Customerid = @cpid));
	if (@count > 0)
		select 'Parent' [Parent], sourceid, datasource,* from Customerparent where Customernameid in (select Customernameid from Customername where Customerid = @cpid);

	set @count = (select count(*) from Customername where Customerid = @cpid and AliasIndicator = 'Y');
	if (@count > 0)
		select 'Alias' [Alias],sourceid, datasource,* from Customername where Customerid = @cpid and AliasIndicator = 'Y';

	set @count = (select count(*) from Customeraddress where Customernameid in (select Customernameid from Customername where Customerid = @cpid));
	if (@count > 0)
		select 'Adr' [Adr],sourceid, datasource, Customeraddressid, Customernameid, Employeeaddressid, addresstype, addressline1, city, state, postalcode, addressline2, addressline3, * from Customeraddress   
		where Customernameid in (select Customernameid from Customername where Customerid = @cpid);

	set @count = (select count(*) from Customerattribute where Customernameid in (select Customernameid from Customername where Customerid = @cpid));
	if (@count > 0)
		select 'Att' [Att],sourceid, datasource,* from Customerattribute 
		where Customernameid in (select Customernameid from Customername where Customerid = @cpid);

	set @count = (select count(*) from Customerdescription where Customernameid in (select Customernameid from Customername where Customerid = @cpid));
	if (@count > 0)
		select 'Desc' [Desc],sourceid, datasource,* from Customerdescription  
		where Customernameid in (select Customernameid from Customername where Customerid = @cpid);

	set @count = (select count(*) from Customerphone where Customernameid in (select Customernameid from Customername where Customerid = @cpid));
	if (@count > 0)
		select 'Phn' [Phn],sourceid, datasource,* from Customerphone   
		where Customernameid in (select Customernameid from Customername where Customerid = @cpid);

	--set @count = (select count(*) from Customerevent where Customerid = @cpid);
	--if (@count > 0)
	--	select 'Evnt' [Evnt],sourceid, datasource,* from Customerevent 
	--	where Customerid = @cpid;
*/
END

GO
/****** Object:  StoredProcedure [dbo].[OrderInformation_Booking]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[OrderInformation_Booking]
	@caseNumber varchar(25)
	,@violationDate date
	,@userName varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;

	if ((@caseNumber is NULL or @caseNumber = '') and (@violationDate is NULL))
		return;

   	if (@username is not null)
   	begin
		exec audit_LogSelect_CREATE @storedProcedureName='OrderInformation_Booking', @parameterName='caseNumber', @parameterValue=@caseNumber, @userName=@userName;
		exec audit_LogSelect_CREATE @storedProcedureName='OrderInformation_Booking', @parameterName='startDate', @parameterValue=@violationDate, @userName=@userName;
		declare @endDate varchar(10)
		set @endDate = (select convert(varchar(10),getdate(),112) as date);
		exec audit_LogSelect_CREATE @storedProcedureName='OrderInformation_Booking', @parameterName='endDate', @parameterValue=@endDate, @userName=@userName;
	end


	SELECT [OrderInformationID]
	  ,[CustomerID]
	  ,[OrderNumber]
	  ,[SourceCompany]
	  ,[OrderType]
	  ,[OrderTypeDescription]
	  ,[OrderStatus]
	  ,[RecordDate]
	  ,[CreateDate]
	  ,[SourceID]
	  ,[UpdateUserName]
	  ,[UpdateDate]
	  ,[VersionCount]
	FROM [OrderInformation]
	WHERE [OrderType] = 'booking' and [RecordDate] >= @violationDate
		and ([OrderStatus] between convert(varchar(10),@violationDate,112) and convert(varchar(10),getdate(),112) or [OrderStatus] is null or [OrderStatus] = '')
		and [CustomerID] in (select CustomerID from Customer 
		  where Item in (select Item from Customer where CustomerID in (select CustomerID from OrderInformation where OrderNumber = @caseNumber)))
	order by RecordDate desc
END
GO
/****** Object:  StoredProcedure [dbo].[OrderInformation_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[OrderInformation_CREATE]
	@CustomerID int
	,@OrderNumber varchar(25)
	,@SourceCompany varchar(10)
	,@OrderType varchar(20) = null
	,@OrderTypeDescription varchar(50) = null
	,@OrderStatus varchar(50) = null
	,@RecordDate datetime = null
	,@SourceID varchar(40) = null
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[OrderInformation]
           ([OrderNumber]
           ,[CustomerID]
           ,[SourceCompany]
           ,[OrderType]
           ,[OrderTypeDescription]
           ,[OrderStatus]
           ,[RecordDate]
           ,[CreateDate]
		   ,[SourceID]
           ,[UpdateUserName]
           ,[UpdateDate]
           ,[VersionCount])
     VALUES
           (@OrderNumber
           ,@CustomerID
           ,@SourceCompany
           ,@OrderType
           ,@OrderTypeDescription
           ,@OrderStatus
		   ,@RecordDate
		   ,getdate()
		   ,@SourceID
           ,@UpdateUserName
           ,GETDATE()
           ,0)

	IF @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	ELSE 
		select 0
END

GO
/****** Object:  StoredProcedure [dbo].[OrderInformation_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[OrderInformation_DELETE]
	  @OrderInformationID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[OrderInformation]
	WHERE [OrderInformationID] = @OrderInformationID
END

GO
/****** Object:  StoredProcedure [dbo].[OrderInformation_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[OrderInformation_READ]
	@caseInformationID int = null
	,@CustomerID int = null
	,@item varchar(10) = null
	,@caseNumber varchar(25) = null
	,@sourceCompany varchar(10) = null
	,@sourceID varchar(40) = null
	,@caseType varchar(20) = null
	,@userName varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@caseInformationID is NULL and @CustomerID is null and @item is null and @caseNumber is null)
		return;

   	if (@username is not null)
   	begin
		exec audit_LogSelect_CREATE @storedProcedureName='OrderInformation_READ', @parameterName='caseInformationID', @parameterValue=@caseInformationID, @userName=@userName;
   		if (@CustomerID is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='OrderInformation_READ', @parameterName='CustomerID', @parameterValue=@CustomerID, @userName=@userName;
   		if (@item is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='OrderInformation_READ', @parameterName='Item', @parameterValue=@item, @userName=@userName;
		if (@caseNumber is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='OrderInformation_READ', @parameterName='caseNumber', @parameterValue=@caseNumber, @userName=@userName;
		if (@sourceCompany is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='OrderInformation_READ', @parameterName='sourceCompany', @parameterValue=@sourceCompany, @userName=@userName;
		if (@sourceID is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='OrderInformation_READ', @parameterName='sourceID', @parameterValue=@sourceID, @userName=@userName;
		if (@caseType is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='OrderInformation_READ', @parameterName='caseType', @parameterValue=@caseType, @userName=@userName;
	end

	if (@caseNumber is not null and @sourceCompany is not null and @sourceID is not null and @caseType is not null)
		SELECT [OrderInformationID]
		  ,[CustomerID]
		  ,[OrderNumber]
		  ,[SourceCompany]
		  ,[OrderType]
		  ,[OrderTypeDescription]
		  ,[OrderStatus]
		  ,[RecordDate]
		  ,[CreateDate]
		  ,[SourceID]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[VersionCount]
		FROM [OrderInformation]
		WHERE [OrderNumber] = @caseNumber
			and [SourceCompany] = @sourceCompany
			and [SourceID] = @sourceID
			and [OrderType] = @caseType
		order by RecordDate desc
	else if (@caseInformationID is not null)
		SELECT [OrderInformationID]
		  ,[CustomerID]
		  ,[OrderNumber]
		  ,[SourceCompany]
		  ,[OrderType]
		  ,[OrderTypeDescription]
		  ,[OrderStatus]
		  ,[RecordDate]
		  ,[CreateDate]
		  ,[SourceID]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[VersionCount]
		FROM [OrderInformation]
		WHERE [OrderInformationID] = @caseInformationID
		order by RecordDate desc
	else if (@CustomerID is not null)
		SELECT [OrderInformationID]
		  ,[CustomerID]
		  ,[OrderNumber]
		  ,[SourceCompany]
		  ,[OrderType]
		  ,[OrderTypeDescription]
		  ,[OrderStatus]
		  ,[RecordDate]
		  ,[CreateDate]
		  ,[SourceID]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[VersionCount]
		FROM [OrderInformation]
		WHERE [CustomerID] = @CustomerID
		order by RecordDate desc
	else if (@item is not null)
		SELECT [OrderInformationID]
		  ,[OrderInformation].[CustomerID]
		  ,[OrderNumber]
		  ,[SourceCompany]
		  ,[OrderType]
		  ,[OrderTypeDescription]
		  ,[OrderStatus]
		  ,[RecordDate]
		  ,[CreateDate]
		  ,[OrderInformation].[SourceID]
		  ,[OrderInformation].[UpdateUserName]
		  ,[OrderInformation].[UpdateDate]
		  ,[OrderInformation].[VersionCount]
		FROM [OrderInformation]
			inner join Customer on Customer.CustomerID = OrderInformation.CustomerID
		WHERE [Item] = @item
		order by RecordDate desc
	else 
		SELECT [OrderInformationID]
		  ,[CustomerID]
		  ,[OrderNumber]
		  ,[SourceCompany]
		  ,[OrderType]
		  ,[OrderTypeDescription]
		  ,[OrderStatus]
		  ,[RecordDate]
		  ,[CreateDate]
		  ,[SourceID]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[VersionCount]
		FROM [OrderInformation]
		WHERE [OrderNumber] = @caseNumber
			and [SourceCompany] = ISNULL(@sourceCompany, [SourceCompany])
			and [OrderType] = ISNULL(@caseType, [OrderType])
		order by RecordDate desc
END
GO
/****** Object:  StoredProcedure [dbo].[OrderInformation_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[OrderInformation_UPDATE]
	@OrderInformationID int
	,@CustomerID int
	,@OrderNumber varchar(25)
	,@SourceCompany varchar(10)
	,@OrderType varchar(20) = null
	,@OrderTypeDescription varchar(50) = null
	,@OrderStatus varchar(50) = null
	,@SourceID varchar(40) = null
-- NOTE: don't update the RecordDate field - probation documents do not contain 
-- a probation start date, so we try to get the date from the referred
-- case or activity nodes. Since additional referred cases and/or 
-- activities could have new dates this field should only be saved on
-- initial add of the case information record.
	--,@RecordDate datetime = null
	--,@CreateDate datetime
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[OrderInformation]
	SET    [OrderNumber] = @OrderNumber
           ,[CustomerID] = @CustomerID
           ,[SourceCompany] = @SourceCompany
           ,[OrderType] = @OrderType
           ,[OrderTypeDescription] = @OrderTypeDescription
           ,[OrderStatus] = @OrderStatus
           --,[RecordDate] = @RecordDate
           --,[CreateDate] = @CreateDate
		   ,[SourceID] = @SourceID
           ,[UpdateUserName] = @UpdateUserName
           ,[UpdateDate] = GETDATE()
           ,[VersionCount] = [VersionCount] + 1
	 WHERE [OrderInformationID] = @OrderInformationID
END

GO
/****** Object:  StoredProcedure [dbo].[Customer_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Customer_CREATE]
	@Item char(10)
	,@InCustody bit
	,@Company varchar(120) = null
	,@CompanyID varchar(120) = null
	,@SourceID varchar(120) = null
	,@DataSource varchar(10) = null
	,@UpdateUserName varchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Customer]
           ([Item]
           ,[InCustody]
           ,[Company]
           ,[CompanyID]
           ,[SourceID]
           ,[DataSource]
           ,[UpdateUserName]
           ,[UpdateDate]
           ,[VersionCount])
     VALUES
           (@Item
           ,@InCustody
           ,@Company
           ,@CompanyID
           ,@SourceID
           ,@DataSource
           ,@UpdateUserName
           ,GETDATE()
           ,0)
	
	IF @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	ELSE 
		select 0
 END
GO
/****** Object:  StoredProcedure [dbo].[Customer_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[Customer_DELETE]
	  @CustomerID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[Customer]
	WHERE [CustomerID] = @CustomerID
END
GO
/****** Object:  StoredProcedure [dbo].[Customer_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Batch submitted through debugger: SQLQuery13.sql|7|0|C:\Users\cfranklin\AppData\Local\Temp\~vs733A.sql

CREATE PROCEDURE [dbo].[Customer_READ]
	@CustomerID int = null
	,@Item char(10) = null
	,@Company varchar(120) = null
	,@localID varchar(120) = null
	,@userName varchar(50) = null
	,@InCustody Bit = null
AS
BEGIN
	SET NOCOUNT ON;
--	if (@CustomerID is NULL and @Item is NULL or (@Company is null and @localID is null))  
--   ** Changed "OR" to "AND" CF 1/4/2012 any attempt to pass Item or CustomerID failed to pass the test ** 
	if (@CustomerID is NULL and @Item is NULL and @InCustody is NULL and (@Company is null and @localID is null))
		return;

   	if (@username is not null)
	begin
		if (@CustomerID is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='Customer_READ', @parameterName='CustomerID', @parameterValue=@CustomerID, @userName=@userName;
		if (@Item is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='Customer_READ', @parameterName='Item', @parameterValue=@Item, @userName=@userName;
		if (@Company is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='Customer_READ', @parameterName='Company', @parameterValue=@Company, @userName=@userName;
		if (@localID is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='Customer_READ', @parameterName='CompanyID', @parameterValue=@localID, @userName=@userName;
	end
	IF (@InCustody is not NULL)
	BEGIN
		SELECT [CustomerID]
			,[Item]
			,[InCustody]
			,[Company]
			,[CompanyID]
			,[SourceID]
			,[DataSource]
			,[UpdateUserName]
			,[UpdateDate]
			,[VersionCount]
		FROM [Customer]
		WHERE [InCustody] = @InCustody
	END
	ELSE
	IF (@Company is not NULL and @localID is null)
	BEGIN
		SELECT [CustomerID]
			,[Item]
			,[InCustody]
			,[Company]
			,[CompanyID]
			,[SourceID]
			,[DataSource]
			,[UpdateUserName]
			,[UpdateDate]
			,[VersionCount]
		FROM [Customer]
		WHERE [Company] = @Company
	END
	ELSE

	IF (@CustomerID is not NULL)
	BEGIN
		SELECT [CustomerID]
			,[Item]
			,[InCustody]
			,[Company]
			,[CompanyID]
			,[SourceID]
			,[DataSource]
			,[UpdateUserName]
			,[UpdateDate]
			,[VersionCount]
		FROM [Customer]
		WHERE [CustomerID] = @CustomerID
	END
	ELSE
		BEGIN
		IF (@Item is not NULL)
			SELECT [CustomerID]
				,[Item]
				,[InCustody]
				,[Company]
				,[CompanyID]
				,[SourceID]
				,[DataSource]
				,[UpdateUserName]
				,[UpdateDate]
				,[VersionCount]
			FROM [Customer]
			WHERE [Item] = @Item
		ELSE
			SELECT [CustomerID]
				,[Item]
				,[InCustody]
				,[Company]
				,[CompanyID]
				,[SourceID]
				,[DataSource]
				,[UpdateUserName]
				,[UpdateDate]
				,[VersionCount]
			FROM [Customer]
			WHERE [Company] = @Company
				AND [CompanyID] = @localID
					
		END		
	
	/*
	SELECT [CustomerID]
		,[Item]
		,[InCustody]
		,[Company]
		,[CompanyID]
		,[SourceID]
		,[DataSource]
		,[UpdateUserName]
		,[UpdateDate]
		,[VersionCount]
	FROM [Customer]
	WHERE [CustomerID] = ISNULL(@CustomerID, CustomerID)
		AND [Item] = ISNULL(@Item, Item)
		AND [Company] = ISNULL(@Company, Company)
		AND [CompanyID] = ISNULL(@localID, CompanyID)*/
END
GO
/****** Object:  StoredProcedure [dbo].[Customer_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Customer_UPDATE]
	@CustomerID int
	,@Item char(10)
	,@InCustody bit
	,@Company varchar(120) = null
	,@CompanyID varchar(120) = null
	,@SourceID varchar(120) = null
	,@DataSource varchar(10) = null
	,@UpdateUserName varchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Customer]
	SET [Item] = @Item
		,[InCustody] = @InCustody
		,[Company] = @Company
		,[CompanyID] = @CompanyID
		,[SourceID] = @SourceID
		,[DataSource] = @DataSource
		,[UpdateUserName] = @UpdateUserName
		,[UpdateDate] = GETDATE()
		,[VersionCount] = [VersionCount] + 1
	WHERE [CustomerID] = @CustomerID
END
GO
/****** Object:  StoredProcedure [dbo].[CustomerAddress_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CustomerAddress_CREATE]
	@CustomerNameID char(10)
	,@AddressType varchar(10) = null
	,@AddressLine1 varchar(50) = null
	,@AddressLine2 varchar(50) = null
	,@AddressLine3 varchar(50) = null
	,@StreetType varchar(4) = null
	,@HouseNumber varchar(5) = null
	,@HalfAddress varchar(5) = null
	,@PrefixDirection varchar(2) = null
	,@StreetName varchar(60) = null
	,@PostDirection varchar(2) = null
	,@Apartment varchar(10) = null
	,@City varchar(20) = null
	,@State varchar(2) = null
	,@PostalCode varchar(9) = null
	,@CountryCode varchar(10) = null
	,@SourceID varchar(40) = null
	,@DataSource varchar(10) = null
	,@ContributingCompany varchar(50) = null
	,@DateModified smalldatetime = null
	,@IDSourceCode varchar(50) = null
	,@UpdateUserName varchar(50)
	,@EmployeeAddressID int = null

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[CustomerAddress]
		([CustomerNameID]
		,[AddressType]
		,[AddressLine1]
		,[AddressLine2]
		,[AddressLine3]
		,[StreetType]
		,[HouseNumber]
		,[HalfAddress]
		,[PrefixDirection]
		,[StreetName]
		,[PostDirection]
		,[Apartment]
		,[City]
		,[State]
		,[PostalCode]
		,[CountryCode]
		,[SourceID]
		,[DataSource]
		,[ContributingCompany]
		,[DateModified]
		,[IDSourceCode]
		,[UpdateUserName]
		,[UpdateDate]
		,[VersionCount]
		,EmployeeAddressID)
     VALUES
		(@CustomerNameID
		,@AddressType
		,@AddressLine1
		,@AddressLine2
		,@AddressLine3
		,@StreetType
		,@HouseNumber
		,@HalfAddress
		,@PrefixDirection
		,@StreetName
		,@PostDirection
		,@Apartment
		,@City
		,@State
		,@PostalCode
		,@CountryCode
	    ,@SourceID
		,@DataSource 
		,@ContributingCompany
		,@DateModified
		,@IDSourceCode
		,@UpdateUserName
		,GETDATE()
		,0
		,@EmployeeAddressID)
	
	IF @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	ELSE 
		select 0
 END










GO
/****** Object:  StoredProcedure [dbo].[CustomerAddress_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CustomerAddress_DELETE]
	  @CustomerAddressID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[CustomerAddress]
	WHERE [CustomerAddressID] = @CustomerAddressID
END




GO
/****** Object:  StoredProcedure [dbo].[CustomerAddress_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CustomerAddress_READ]
	  @CustomerAddressID int = null,
	  @CustomerNameID int = null,
	  @EmployeeAddressID int = null,
	  @SortOption char(1) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@CustomerAddressID is NULL and @CustomerNameID is NULL and @EmployeeAddressID is NULL and @SortOption is null)
		return;
		
	if (@SortOption is not null)
		SELECT [CustomerAddressID]
		  ,[CustomerNameID]
		  ,[AddressType]
		  ,[AddressLine1]
		  ,[AddressLine2]
		  ,[AddressLine3]
		  ,[StreetType]
		  ,[HouseNumber]
		  ,[HalfAddress]
		  ,[PrefixDirection]
		  ,[StreetName]
		  ,[PostDirection]
		  ,[Apartment]
		  ,[City]
		  ,[State]
		  ,[PostalCode]
		  ,[CountryCode]
		  ,[SourceID]
		  ,[DataSource]
		  ,[DateModified]
		  ,[VersionCount]
		  ,[EmployeeAddressID]
		  ,[ContributingCompany]
		  ,[IDSourceCode]
		FROM [dbo].[CustomerAddress]
		WHERE [CustomerNameID] = @CustomerNameID
		order by [SourceID]
	ELSE if (@EmployeeAddressID is not NULL and @CustomerAddressID is not NULL)
		SELECT [CustomerAddressID]
		  ,[CustomerNameID]
		  ,[AddressType]
		  ,[AddressLine1]
		  ,[AddressLine2]
		  ,[AddressLine3]
		  ,[StreetType]
		  ,[HouseNumber]
		  ,[HalfAddress]
		  ,[PrefixDirection]
		  ,[StreetName]
		  ,[PostDirection]
		  ,[Apartment]
		  ,[City]
		  ,[State]
		  ,[PostalCode]
		  ,[CountryCode]
		  ,[SourceID]
		  ,[DataSource]
		  ,[DateModified]
		  ,[VersionCount]
		  ,[EmployeeAddressID]
		  ,[ContributingCompany]
		  ,[IDSourceCode]
		FROM [dbo].[CustomerAddress]
		WHERE CustomerAddressID = (select min(CustomerAddressID) from [dbo].[CustomerAddress] 
									 where [EmployeeAddressID] = @EmployeeAddressID and [CustomerAddressID] != @CustomerAddressID)
	else
		SELECT [CustomerAddressID]
		  ,[CustomerNameID]
		  ,[AddressType]
		  ,[AddressLine1]
		  ,[AddressLine2]
		  ,[AddressLine3]
		  ,[StreetType]
		  ,[HouseNumber]
		  ,[HalfAddress]
		  ,[PrefixDirection]
		  ,[StreetName]
		  ,[PostDirection]
		  ,[Apartment]
		  ,[City]
		  ,[State]
		  ,[PostalCode]
		  ,[CountryCode]
		  ,[SourceID]
		  ,[DataSource]
		  ,[DateModified]
		  ,[VersionCount]
		  ,[EmployeeAddressID]
		  ,[ContributingCompany]
		  ,[IDSourceCode]
		FROM [dbo].[CustomerAddress]
		WHERE [CustomerAddressID] = ISNULL(@CustomerAddressID, CustomerAddressID)
			AND [CustomerNameID] = ISNULL(@CustomerNameID, CustomerNameID)
			AND [EmployeeAddressID] = ISNULL(@EmployeeAddressID, EmployeeAddressID)
		order by [AddressType], updateDate desc
END














GO
/****** Object:  StoredProcedure [dbo].[CustomerAddress_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CustomerAddress_UPDATE]
	@CustomerAddressID int
	,@CustomerNameID int
	,@AddressType varchar(10) = null
	,@AddressLine1 varchar(50) = null
	,@AddressLine2 varchar(50) = null
	,@AddressLine3 varchar(50) = null
	,@StreetType varchar(4) = null
	,@HouseNumber varchar(5) = null
	,@HalfAddress varchar(5) = null
	,@PrefixDirection varchar(2) = null
	,@StreetName varchar(60) = null
	,@PostDirection varchar(2) = null
	,@Apartment varchar(10) = null
	,@City varchar(20) = null
	,@State varchar(2) = null
	,@PostalCode varchar(9) = null
	,@CountryCode varchar(10) = null
	,@SourceID varchar(40) = null
	,@DataSource varchar(10) = null
	,@DateModified smalldatetime = null
	,@UpdateUserName varchar(50)
	,@EmployeeAddressID int = null
	,@ContributingCompany varchar(50) = null
	,@IDSourceCode varchar(50) = null

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE  [dbo].[CustomerAddress]
	SET     [CustomerNameID] = @CustomerNameID
			,[AddressType] = @AddressType
			,[AddressLine1] = @AddressLine1
			,[AddressLine2] = @AddressLine2
			,[AddressLine3] = @AddressLine3
			,[StreetType] = @StreetType
			,[HouseNumber] = @HouseNumber
			,[HalfAddress] = @HalfAddress
			,[PrefixDirection] = @PrefixDirection
			,[StreetName] = @StreetName
			,[PostDirection] = @PostDirection
			,[Apartment] = @Apartment
			,[City] = @City
			,[State] = @State
			,[PostalCode] = @PostalCode
			,[CountryCode] = @CountryCode
		    ,[SourceID] = @SourceID
			,[DataSource]= @DataSource
		    ,[ContributingCompany] = @ContributingCompany
		    ,[DateModified] = @DateModified
		    ,[IDSourceCode] = @IDSourceCode
			,[UpdateUserName] = @UpdateUserName
			,[UpdateDate] = GETDATE()
			,[VersionCount] = [VersionCount] + 1
			,[EmployeeAddressID] = @EmployeeAddressID
	WHERE   [CustomerAddressID] = @CustomerAddressID
END










GO
/****** Object:  StoredProcedure [dbo].[CustomerAttribute_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CustomerAttribute_CREATE]
	@CustomerNameID int
	,@AssignedIDType varchar(10)
	,@AssignedIDNumber varchar(50)
	,@AssignedIDState varchar(10) = null
	,@PrimaryID bit
	,@ContributingCompany varchar(50) = null
	,@DateModified smallDateTime = null
	,@IDSourceCode varchar(50) = null
	,@SourceID varchar(255) = null
	,@DataSource varchar(10) = null
	,@UpdateUserName varchar(50)
	,@EmployeeAttributeID int = null
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[CustomerAttribute]
		([CustomerNameID]
		,[AssignedIDType]
		,[AssignedIDNumber]
		,[AssignedIDState]
		,[PrimaryID]
		,[ContributingCompany]
		,[DateModified]
		,[IDSourceCode]
		,[SourceID]
		,[DataSource]
		,[UpdateUserName]
		,[UpdateDate]
		,[VersionCount]
		,[EmployeeAttributeID])
     VALUES
		(@CustomerNameID
		,@AssignedIDType
		,@AssignedIDNumber
		,@AssignedIDState
		,@PrimaryID
		,@ContributingCompany
		,@DateModified
		,@IDSourceCode
		,@SourceID
		,@DataSource
		,@UpdateUserName
		,GETDATE()
		,0
		,@EmployeeAttributeID)
	
	IF @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	ELSE 
		select 0
END











GO
/****** Object:  StoredProcedure [dbo].[CustomerAttribute_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CustomerAttribute_DELETE]
	  @CustomerAttributeID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[CustomerAttribute]
	WHERE [CustomerAttributeID] = @CustomerAttributeID
END





GO
/****** Object:  StoredProcedure [dbo].[CustomerAttribute_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CustomerAttribute_READ]
	  @CustomerAttributeID int = null,
	  @CustomerNameID int = null,
	  @EmployeeAttributeID int = null,
	  @SortOption char(1) = null
AS
BEGIN
	SET NOCOUNT ON;

   if (@CustomerAttributeID is NULL and @CustomerNameID is NULL and @EmployeeAttributeID is null and @SortOption is null)
		return;

   if (@SortOption is not null)   
		SELECT [CustomerAttributeID]
		  ,[CustomerNameID]
		  ,[AssignedIDType]
		  ,[AssignedIDNumber]
		  ,[AssignedIDState]
		  ,[PrimaryID]
		  ,[SourceID]
		  ,[DataSource]
		  ,[VersionCount]
		  ,[EmployeeAttributeID]
		  ,[ContributingCompany]
		  ,[DateModified]
		  ,[IDSourceCode]
		FROM [dbo].[CustomerAttribute]
		WHERE [CustomerAttributeID] = ISNULL(@CustomerAttributeID, CustomerAttributeID)
			and [CustomerNameID] = ISNULL(@CustomerNameID, CustomerNameID)
			and [EmployeeAttributeID] = ISNULL(@EmployeeAttributeID, EmployeeAttributeID)
		order by [SourceID]
	ELSE if (@EmployeeAttributeID is not NULL and @CustomerAttributeID is not NULL)
		SELECT [CustomerAttributeID]
			,[CustomerNameID]
			,[AssignedIDType]
			,[AssignedIDNumber]
			,[AssignedIDState]
			,[PrimaryID]
			,[SourceID]
			,[DataSource]
			,[VersionCount]
			,[EmployeeAttributeID]
			,[ContributingCompany]
			,[DateModified]
			,[IDSourceCode]
			FROM [dbo].[CustomerAttribute]
			WHERE CustomerAttributeID = (select min(CustomerAttributeID) from [dbo].[CustomerAttribute]
										   where [EmployeeAttributeID] = @EmployeeAttributeID and [CustomerAttributeID] != @CustomerAttributeID)
    else
		SELECT [CustomerAttributeID]
		  ,[CustomerNameID]
		  ,[AssignedIDType]
		  ,[AssignedIDNumber]
		  ,[AssignedIDState]
		  ,[PrimaryID]
		  ,[SourceID]
		  ,[DataSource]
		  ,[VersionCount]
		  ,[EmployeeAttributeID]
		  ,[ContributingCompany]
		  ,[DateModified]
		  ,[IDSourceCode]
		FROM [dbo].[CustomerAttribute]
		WHERE [CustomerAttributeID] = ISNULL(@CustomerAttributeID, CustomerAttributeID)
			and [CustomerNameID] = ISNULL(@CustomerNameID, CustomerNameID)
			and [EmployeeAttributeID] = ISNULL(@EmployeeAttributeID, EmployeeAttributeID)
		order by [AssignedIDType],[DateModified] desc
END
GO
/****** Object:  StoredProcedure [dbo].[CustomerAttribute_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CustomerAttribute_UPDATE]
	@CustomerAttributeID int
	,@CustomerNameID int
	,@AssignedIDType varchar(10)
	,@AssignedIDNumber varchar(50)
	,@AssignedIDState varchar(10) = null
	,@PrimaryID bit
	,@ContributingCompany varchar(50)
	,@DateModified smallDateTime = null
	,@IDSourceCode varchar(50) = null
	,@SourceID varchar(255) = null
	,@DataSource varchar(10) = null
	,@UpdateUserName varchar(50)
	,@EmployeeAttributeID int = null
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE  [dbo].[CustomerAttribute]
	SET     [CustomerNameID] = @CustomerNameID
			,[AssignedIDType] = @AssignedIDType
			,[AssignedIDNumber] = @AssignedIDNumber
			,[AssignedIDState] = @AssignedIDState
			,[PrimaryID] = @PrimaryID
		    ,[ContributingCompany] = @ContributingCompany
		    ,[DateModified] = @DateModified
		    ,[IDSourceCode] = @IDSourceCode
		    ,[SourceID] = @SourceID
			,[DataSource] = @DataSource
			,[UpdateUserName] = @UpdateUserName
			,[UpdateDate] = GETDATE()
			,[VersionCount] = [VersionCount] + 1
			,[EmployeeAttributeID] = @EmployeeAttributeID
	WHERE   [CustomerAttributeID] = @CustomerAttributeID
END










GO
/****** Object:  StoredProcedure [dbo].[CustomerDescription_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[CustomerDescription_CREATE]
	@CustomerNameID int
	,@Sex char(1) = null
	,@Race varchar(10) = null
	,@Height char(3) = null
	,@Weight char(3) = null
	,@EyeColor varchar(10) = null
	,@BirthPlace varchar(40) = null
	,@HairColor varchar(10) = null
	,@Occupation varchar(50) = null
	,@ContributingCompany varchar(50) = null
	,@DateModified smallDateTime = null
	,@IDSourceCode varchar(50) = null
	,@SourceID varchar(255) = null
	,@DataSource varchar(10) = null
	,@UpdateUserName varchar(50)
	,@EmployeeDescriptionID int = null
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[CustomerDescription]
		([CustomerNameID]
		,[Sex]
		,[Race]
		,[Height]
		,[Weight]
		,[EyeColor]
		,[BirthPlace]
		,[HairColor]
		,[Occupation]
		,[ContributingCompany]
		,[DateModified]
		,[IDSourceCode]
		,[SourceID]
		,[DataSource]
		,[UpdateUserName]
		,[UpdateDate]
		,[VersionCount]
		,[EmployeeDescriptionID])
     VALUES
		(@CustomerNameID
		,@Sex
		,@Race
		,@Height
		,@Weight
		,@EyeColor
		,@BirthPlace
		,@HairColor
		,@Occupation
		,@ContributingCompany
		,@DateModified
		,@IDSourceCode
		,@SourceID
		,@DataSource
		,@UpdateUserName
		,GETDATE()
		,0
		,@EmployeeDescriptionID)

	IF @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	ELSE 
		select 0
END











GO
/****** Object:  StoredProcedure [dbo].[CustomerDescription_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CustomerDescription_DELETE]
	  @CustomerDescriptionID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[CustomerDescription]
	WHERE [CustomerDescriptionID] = @CustomerDescriptionID
END





GO
/****** Object:  StoredProcedure [dbo].[CustomerDescription_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CustomerDescription_READ]
	  @CustomerDescriptionID int = null,
	  @CustomerNameID int = null,
	  @EmployeeDescriptionID int = null,
	  @SortOption char(1) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@CustomerDescriptionID is NULL and @CustomerNameID is NULL and @EmployeeDescriptionID is null and @SortOption is null)
		return;

	if (@SortOption is not null)
		SELECT [CustomerDescriptionID]
		  ,[CustomerNameID]
		  ,[Sex]
		  ,[Race]
		  ,[Height]
		  ,[Weight]
		  ,[EyeColor]
		  ,[BirthPlace]
		  ,[HairColor]
		  ,[Occupation]
		  ,[SourceID]
		  ,[DataSource]
		  ,[VersionCount]
		  ,[EmployeeDescriptionID]
		  ,[ContributingCompany]
		  ,[DateModified]
		  ,[IDSourceCode]
		FROM [dbo].[CustomerDescription]
		WHERE [CustomerNameID] = @CustomerNameID order by [SourceID]
	ELSE if (@EmployeeDescriptionID is not NULL and @CustomerDescriptionID is not NULL)
		SELECT [CustomerDescriptionID]
		  ,[CustomerNameID]
		  ,[Sex]
		  ,[Race]
		  ,[Height]
		  ,[Weight]
		  ,[EyeColor]
		  ,[BirthPlace]
		  ,[HairColor]
		  ,[Occupation]
		  ,[SourceID]
		  ,[DataSource]
		  ,[VersionCount]
		  ,[EmployeeDescriptionID]
		  ,[ContributingCompany]
		  ,[DateModified]
		  ,[IDSourceCode]
		FROM [dbo].[CustomerDescription]
		WHERE CustomerDescriptionID = (select min(CustomerDescriptionID) from [dbo].[CustomerDescription]
										 where [EmployeeDescriptionID] = @EmployeeDescriptionID and [CustomerDescriptionID] != @CustomerDescriptionID)
	else
		SELECT [CustomerDescriptionID]
			,[CustomerNameID]
			,[Sex]
			,[Race]
			,[Height]
			,[Weight]
			,[EyeColor]
			,[BirthPlace]
			,[HairColor]
			,[Occupation]
			,[SourceID]
			,[DataSource]
			,[VersionCount]
			,[EmployeeDescriptionID]
			,[ContributingCompany]
			,[DateModified]
			,[IDSourceCode]
		FROM [dbo].[CustomerDescription]
		WHERE [CustomerDescriptionID] = ISNULL(@CustomerDescriptionID, CustomerDescriptionID)
			and [CustomerNameID] = ISNULL(@CustomerNameID, CustomerNameID)
			and [EmployeeDescriptionID] = ISNULL(@EmployeeDescriptionID, EmployeeDescriptionID)
		order by CustomerDescriptionID
END
GO
/****** Object:  StoredProcedure [dbo].[CustomerDescription_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[CustomerDescription_UPDATE]
	@CustomerDescriptionID int,
	@CustomerNameID int,
	@Sex char(1) = null,
	@Race varchar(10) = null,
	@Height char(3) = null,
	@Weight char(3) = null,
	@EyeColor varchar(10) = null,
	@BirthPlace varchar(40) = null,
	@HairColor varchar(10) = null,
	@Occupation varchar(50) = null,
	@ContributingCompany varchar(50) = null,
	@DateModified smallDateTime = null,
	@IDSourceCode varchar(50) = null,
	@SourceID varchar(255) = null,
	@DataSource varchar(10)=null,
	@UpdateUserName varchar(50),
	@EmployeeDescriptionID int = null
AS
BEGIN
	SET NOCOUNT ON;

UPDATE [dbo].[CustomerDescription]
   SET [CustomerNameID] = @CustomerNameID
      ,[Sex] = @Sex
      ,[Race] = @Race
      ,[Height] = @Height
      ,[Weight] = @Weight
      ,[EyeColor] = @EyeColor
      ,[BirthPlace] = @BirthPlace
      ,[HairColor] = @HairColor
      ,[Occupation] = @Occupation
      ,[ContributingCompany] = @ContributingCompany
	  ,[DateModified] = @DateModified
      ,[IDSourceCode] = @IDSourceCode
	  ,[SourceID] = @SourceID
	  ,[DataSource] = @DataSource
	  ,[UpdateUserName] = @UpdateUserName
	  ,[EmployeeDescriptionID] = @EmployeeDescriptionID
 WHERE [CustomerDescriptionID] = @CustomerDescriptionID
END







GO
/****** Object:  StoredProcedure [dbo].[CustomerName_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CustomerName_CREATE]
	 @CustomerID int
	,@EmployeeNameID int = null
	,@Item char(10)
	,@FirstName varchar(50) = null
	,@MiddleName varchar(50) = null
	,@LastName varchar(50) = null
	,@SuffixName varchar(10) = null
	,@NameTitle varchar(10) = null
	,@BusinessName varchar(100) = null
	,@DOB datetime = null
	,@DeathDate smalldatetime = null
	,@NameType varchar(10) = null
	,@PhoneticLastName varchar(10) = null
	,@AliasIndicator char(1) = null
	,@DataSource varchar(10)
	,@CreateUserName varchar(50)
	,@SourceID varchar(40) = null
	,@ValidationIndicator char(1) = 'N'
	,@ValidationComments varchar(100) = null
	,@ContributingCompany varchar(50) = null
	,@DateModified smalldatetime = null
	,@Note varchar(100) = null	
	,@IDSourceCode varchar(50) = null
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[CustomerName]
		(
		 [CustomerID]
		,[EmployeeNameID]
		,[Item]
		,[FirstName]
		,[MiddleName]
		,[LastName]
		,[SuffixName]
		,[NameTitle]
		,[BusinessName]
		,[DOB]
		,[DeathDate]
		,[NameType]
		,[PhoneticLastName]
		,[SoundexFirstName]
		,[SoundexLastName]
		,[AliasIndicator]
		,[DataSource]
		,[CreateDate]
		,[CreateUserName]
		,[SourceID]
	    ,[ValidationIndicator]
	    ,[ValidationComments]
		,[ContributingCompany]
		,[DateModified]
		,[Note]
		,[IDSourceCode]
		,[UpdateUserName]
		,[UpdateDate]
		,[VersionCount])
     VALUES
		(@CustomerID
		,@EmployeeNameID
		,@Item
		,@FirstName
		,@MiddleName
		,@LastName
		,@SuffixName
		,@NameTitle
		,@BusinessName
		,@DOB
		,@DeathDate
		,@NameType
		,@PhoneticLastName
		,SOUNDEX(@FirstName)
		,SOUNDEX(@LastName)
		,@AliasIndicator
		,@DataSource
		,GETDATE()
		,@CreateUserName
	    ,@SourceID
	    ,@ValidationIndicator
	    ,@ValidationComments
		,@ContributingCompany
		,@DateModified
		,@Note
		,@IDSourceCode
		,@UpdateUserName
		,GETDATE()
		,0)

	IF @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	ELSE 
		select 0
END
















GO
/****** Object:  StoredProcedure [dbo].[CustomerName_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CustomerName_DELETE]
	  @CustomerNameID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[CustomerName]
	WHERE [CustomerNameID] = @CustomerNameID
END





GO
/****** Object:  StoredProcedure [dbo].[CustomerName_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CustomerName_READ]
	  @CustomerNameID int = null
	  ,@CustomerID int = null
	  ,@EmployeeNameID int = null
	  ,@Item char(10) = null
	  ,@SourceID varchar(255) = null
	  ,@DataSource varchar(10) = null
	  ,@SortOption char(1) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@CustomerNameID is NULL and @CustomerID is null and @EmployeeNameID is null and @Item is NULL and @SourceID is null and @SortOption is NULL)
		return;
	
	if (@SortOption is not null and @SortOption > '')
		SELECT [CustomerNameID]
		  ,[CustomerID]
		  ,[EmployeeNameID]
		  ,[Item]
		  ,[FirstName]
		  ,[MiddleName]
		  ,[LastName]
		  ,[SuffixName]
		  ,[NameTitle]
		  ,[BusinessName]
		  ,[DOB]
		  ,[DeathDate]
		  ,[NameType]
		  ,[PhoneticLastName]
		  ,[AliasIndicator]
		  ,[DataSource]
		  ,[CreateDate]
		  ,[CreateUserName]
		  ,[SourceID]
		  ,[ValidationIndicator]
		  ,[ValidationComments]
		  ,[DateModified]
		  ,[Note]
		  ,[VersionCount]
		  ,[ContributingCompany]
		  ,[IDSourceCode]
		FROM [dbo].[CustomerName]
		WHERE [CustomerNameID] = ISNULL(@CustomerNameID, CustomerNameID)
			AND [CustomerID] = ISNULL(@CustomerID, CustomerID)
			AND [Item] = ISNULL(@Item, Item)
		order by [CustomerNameID]
	else if (@EmployeeNameID is not NULL and @CustomerNameID is not NULL)
		SELECT [CustomerNameID]
		  ,[CustomerID]
		  ,[EmployeeNameID]
		  ,[Item]
		  ,[FirstName]
		  ,[MiddleName]
		  ,[LastName]
		  ,[SuffixName]
		  ,[NameTitle]
		  ,[BusinessName]
		  ,[DOB]
		  ,[DeathDate]
		  ,[NameType]
		  ,[PhoneticLastName]
		  ,[AliasIndicator]
		  ,[DataSource]
		  ,[CreateDate]
		  ,[CreateUserName]
		  ,[SourceID]
		  ,[ValidationIndicator]
		  ,[ValidationComments]
		  ,[DateModified]
		  ,[Note]
		  ,[VersionCount]
		  ,[ContributingCompany]
		  ,[IDSourceCode]char
		FROM [dbo].[CustomerName]
		WHERE CustomerNameID = (select min(CustomerNameID) from [dbo].[CustomerName]
								  where [EmployeeNameID] = @EmployeeNameID and [CustomerNameID] != @CustomerNameID)
    else if (@Item is not NULL and @DataSource is not NULL)
		SELECT [CustomerNameID]
		  ,[CustomerID]
		  ,[EmployeeNameID]
		  ,[Item]
		  ,[FirstName]
		  ,[MiddleName]
		  ,[LastName]
		  ,[SuffixName]
		  ,[NameTitle]
		  ,[BusinessName]
		  ,[DOB]
		  ,[DeathDate]
		  ,[NameType]
		  ,[PhoneticLastName]
		  ,[AliasIndicator]
		  ,[DataSource]
		  ,[CreateDate]
		  ,[CreateUserName]
		  ,[SourceID]
		  ,[ValidationIndicator]
		  ,[ValidationComments]
		  ,[DateModified]
		  ,[Note]
		  ,[VersionCount]
		  ,[ContributingCompany]
		  ,[IDSourceCode]
		FROM [dbo].[CustomerName]
		WHERE @Item = [Item] and @DataSource = [DataSource]
		order by UpdateDate
	else if (@SourceID is not NULL and @DataSource is not NULL)
		SELECT [CustomerNameID]
		  ,[CustomerID]
		  ,[EmployeeNameID]
		  ,[Item]
		  ,[FirstName]
		  ,[MiddleName]
		  ,[LastName]
		  ,[SuffixName]
		  ,[NameTitle]
		  ,[BusinessName]
		  ,[DOB]
		  ,[DeathDate]
		  ,[NameType]
		  ,[PhoneticLastName]
		  ,[AliasIndicator]
		  ,[DataSource]
		  ,[CreateDate]
		  ,[CreateUserName]
		  ,[SourceID]
		  ,[ValidationIndicator]
		  ,[ValidationComments]
		  ,[DateModified]
		  ,[Note]
		  ,[VersionCount]
		  ,[ContributingCompany]
		  ,[IDSourceCode]
		FROM [dbo].[CustomerName]
		WHERE @SourceID = [SourceID] and @DataSource = [DataSource]
		order by UpdateDate
	ELSE
		SELECT [CustomerNameID]
			  ,[CustomerID]
			  ,[EmployeeNameID]
			  ,[Item]
			  ,[FirstName]
			  ,[MiddleName]
			  ,[LastName]
			  ,[SuffixName]
			  ,[NameTitle]
			  ,[BusinessName]
			  ,[DOB]
			  ,[DeathDate]
			  ,[NameType]
			  ,[PhoneticLastName]
			  ,[AliasIndicator]
			  ,[DataSource]
			  ,[CreateDate]
			  ,[CreateUserName]
			  ,[SourceID]
			  ,[ValidationIndicator]
			  ,[ValidationComments]
			  ,[DateModified]
			  ,[Note]
			  ,[VersionCount]
			  ,[ContributingCompany]
			  ,[IDSourceCode]
			FROM [dbo].[CustomerName]
			WHERE [CustomerNameID] = ISNULL(@CustomerNameID, [CustomerNameID])
				and [Item] = ISNULL(@Item, [Item])
				and [CustomerID] = ISNULL(@CustomerID, [CustomerID])
				and [EmployeeNameID] = ISNULL(@EmployeeNameID, [EmployeeNameID])
END
GO
/****** Object:  StoredProcedure [dbo].[CustomerName_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[CustomerName_UPDATE]
	 @CustomerNameID int
	,@CustomerID int
	,@EmployeeNameID int = null
	,@Item char(10)
	,@FirstName varchar(50) = null
	,@MiddleName varchar(50) = null
	,@LastName varchar(50) = null
	,@SuffixName varchar(10) = null
	,@NameTitle varchar(10) = null
	,@BusinessName varchar(100) = null
	,@DOB datetime = null
	,@DeathDate smalldatetime = null
	,@NameType varchar(10) = null
	,@PhoneticLastName varchar(10) = null
	,@AliasIndicator char(1) = null
	,@DataSource varchar(10)
	,@CreateUserName varchar(50)
	,@SourceID varchar(255) = null
	,@ValidationIndicator char(1) = 'N'
	,@ValidationComments varchar(100) = null
	,@ContributingCompany varchar(50) = null
	,@DateModified smalldatetime = null
    ,@Note varchar(100) = 'null'
	,@IDSourceCode varchar(50) = null
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE  [dbo].[CustomerName]
	SET     
			[CustomerID] = @CustomerID
			,[EmployeeNameID] = @EmployeeNameID
			,[Item] = @Item
			,[FirstName] = @FirstName
			,[MiddleName] = @MiddleName
			,[LastName] = @LastName
			,[SuffixName] = @SuffixName
			,[NameTitle] = @NameTitle
			,[BusinessName] = @BusinessName
			,[DOB] = @DOB
			,[DeathDate] = @DeathDate
			,[NameType] = @NameType
			,[PhoneticLastName] = @PhoneticLastName
			,[SoundexFirstName] = SOUNDEX(@FirstName)
			,[SoundexLastName] = SOUNDEX(@LastName)
			,[AliasIndicator] = @AliasIndicator
			,[DataSource] = @DataSource
			,[CreateUserName] = @CreateUserName
			,[SourceID] = @SourceID
			,[ValidationIndicator] = @ValidationIndicator
			,[ValidationComments] = @ValidationComments
		    ,[ContributingCompany ] = @ContributingCompany
		    ,[DateModified] = @DateModified
            ,[Note] = @Note
		    ,[IDSourceCode] = @IDSourceCode
			,[UpdateUserName] = @UpdateUserName
			,[UpdateDate] = GETDATE()
			,[VersionCount] = [VersionCount] + 1
	WHERE   [CustomerNameID] = @CustomerNameID
END












GO
/****** Object:  StoredProcedure [dbo].[CustomerParent_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CustomerParent_CREATE]
	 @CustomerNameID int
	,@EmployeeParentID int
    ,@PartyType varchar(15)
	,@FirstName varchar(50) = null
	,@MiddleName varchar(50) = null
	,@LastName varchar(50)
	,@NameTitle varchar(10) = null
	,@AddressLine1 varchar(50) = null
	,@AddressLine2 varchar(50) = null
	,@AddressLine3 varchar(50) = null
	,@City varchar(20) = null
	,@State varchar(2) = null
	,@PostalCode varchar(9) = null
	,@SSN varchar(9) = null
	,@PhoneNumber varchar(20) = null
	,@Notes varchar(70) = null
	,@SourceID varchar(40)
	,@DataSource varchar(50)
	,@DateModified smalldatetime = null
	,@ContributingCompany varchar(50)
	,@IDSourceCode varchar(50) = null
	,@UpdateUserName nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[CustomerParent]
		([CustomerNameID]
		,[EmployeeParentID]
		,[PartyType]
		,[FirstName]
		,[MiddleName]
		,[LastName]
		,[NameTitle]
		,[AddressLine1]
		,[AddressLine2]
		,[AddressLine3]
		,[City]
		,[State]
		,[PostalCode]
		,[SSN]
		,[PhoneNumber]
		,[Notes]
		,[SourceID]
		,[DataSource]
		,[DateModified]
		,[ContributingCompany]
		,[IDSourceCode]
		,[UpdateUserName]
		,[UpdateDate]
		,[VersionCount])
     VALUES
		(@CustomerNameID
		,@EmployeeParentID
		,@PartyType
		,@FirstName
		,@MiddleName
		,@LastName
		,@NameTitle
		,@AddressLine1
		,@AddressLine2
		,@AddressLine3
		,@City
		,@State
		,@PostalCode
		,@SSN
		,@PhoneNumber
		,@Notes
	    ,@SourceID
	    ,@DataSource
	    ,@DateModified
		,@ContributingCompany
		,@IDSourceCode
		,@UpdateUserName
		,GETDATE()
		,0)

	IF @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	ELSE 
		select 0
END

GO
/****** Object:  StoredProcedure [dbo].[CustomerParent_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CustomerParent_DELETE]
	  @CustomerParentID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[CustomerParent]
	WHERE [CustomerParentID] = @CustomerParentID
END

GO
/****** Object:  StoredProcedure [dbo].[CustomerParent_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CustomerParent_READ]
	@CustomerParentID int = null
	,@CustomerNameID int = null
	,@EmployeeParentID int = null
	,@SortOption char(1) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@CustomerParentID is NULL and @CustomerNameID is null and @EmployeeParentID is null and @SortOption is null)
		return;
		
	if (@SortOption is not null)
			SELECT [CustomerParentID]
			  ,[CustomerNameID]
			  ,[EmployeeParentID]
			  ,[PartyType]
			  ,[FirstName]
			  ,[MiddleName]
			  ,[LastName]
			  ,[NameTitle]
			  ,[AddressLine1]
			  ,[AddressLine2]
			  ,[AddressLine3]
			  ,[City]
			  ,[State]
			  ,[PostalCode]
			  ,[SSN]
			  ,[PhoneNumber]
			  ,[Notes]
			  ,[SourceID]
			  ,[DataSource]
			  ,[DateModified]
			  ,[ContributingCompany]
			  ,[IDSourceCode]
			  ,[UpdateUserName]
			  ,[UpdateDate]
			  ,[VersionCount]
		FROM [dbo].[CustomerParent]
		WHERE [CustomerNameID] = @CustomerNameID order by [SourceID]
	else if (@EmployeeParentID is not NULL and @CustomerParentID is not NULL)
		SELECT [CustomerParentID]
			  ,[CustomerNameID]
			  ,[EmployeeParentID]
			  ,[PartyType]
			  ,[FirstName]
			  ,[MiddleName]
			  ,[LastName]
			  ,[NameTitle]
			  ,[AddressLine1]
			  ,[AddressLine2]
			  ,[AddressLine3]
			  ,[City]
			  ,[State]
			  ,[PostalCode]
			  ,[SSN]
			  ,[PhoneNumber]
			  ,[Notes]
			  ,[SourceID]
			  ,[DataSource]
			  ,[DateModified]
			  ,[ContributingCompany]
			  ,[IDSourceCode]
			  ,[UpdateUserName]
			  ,[UpdateDate]
			  ,[VersionCount]
		FROM [dbo].[CustomerParent]
		WHERE CustomerParentID = 
			(select min(CustomerParentID) from [dbo].[CustomerParent]
			 where [EmployeeParentID] = @EmployeeParentID and [CustomerParentID] != @CustomerParentID)
	ELSE	
	if (@CustomerParentID is not null)
		SELECT [CustomerParentID]
			  ,[CustomerNameID]
			  ,[EmployeeParentID]
			  ,[PartyType]
			  ,[FirstName]
			  ,[MiddleName]
			  ,[LastName]
			  ,[NameTitle]
			  ,[AddressLine1]
			  ,[AddressLine2]
			  ,[AddressLine3]
			  ,[City]
			  ,[State]
			  ,[PostalCode]
			  ,[SSN]
			  ,[PhoneNumber]
			  ,[Notes]
			  ,[SourceID]
			  ,[DataSource]
			  ,[DateModified]
			  ,[ContributingCompany]
			  ,[IDSourceCode]
			  ,[UpdateUserName]
			  ,[UpdateDate]
			  ,[VersionCount]
		FROM [dbo].[CustomerParent]
		WHERE [CustomerParentID] = @CustomerParentID
	else
		SELECT [CustomerParentID]
			  ,[CustomerNameID]
			  ,[EmployeeParentID]
			  ,[PartyType]
			  ,[FirstName]
			  ,[MiddleName]
			  ,[LastName]
			  ,[NameTitle]
			  ,[AddressLine1]
			  ,[AddressLine2]
			  ,[AddressLine3]
			  ,[City]
			  ,[State]
			  ,[PostalCode]
			  ,[SSN]
			  ,[PhoneNumber]
			  ,[Notes]
			  ,[SourceID]
			  ,[DataSource]
			  ,[DateModified]
			  ,[ContributingCompany]
			  ,[IDSourceCode]
			  ,[UpdateUserName]
			  ,[UpdateDate]
			  ,[VersionCount]
		FROM [dbo].[CustomerParent]
		WHERE [CustomerParentID] = ISNULL(@CustomerParentID, CustomerParentID)
			and [CustomerNameID] = ISNULL(@CustomerNameID, CustomerNameID)
			and [EmployeeParentID] = ISNULL(@EmployeeParentID, EmployeeParentID)
		order by [CustomerParentID]
END

GO
/****** Object:  StoredProcedure [dbo].[CustomerParent_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CustomerParent_UPDATE]
	@CustomerParentID int
	,@CustomerNameID int
	,@EmployeeParentID int = null
	,@PartyType varchar(15)
	,@FirstName varchar(50) = null
	,@MiddleName varchar(50) = null
	,@LastName varchar(50)
	,@NameTitle varchar(10) = null
	,@AddressLine1 varchar(50) = null
	,@AddressLine2 varchar(50) = null
	,@AddressLine3 varchar(50) = null
	,@City varchar(20) = null
	,@State varchar(2) = null
	,@PostalCode varchar(9) = null
	,@SSN varchar(9) = null
	,@PhoneNumber varchar(20) = null
	,@Notes varchar(70) = null
	,@SourceID varchar(40) = null
	,@DataSource varchar(50)
	,@DateModified smalldatetime = null
	,@ContributingCompany varchar(50) = null
	,@IDSourceCode varchar(50) = null
	,@UpdateUserName nvarchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[CustomerParent]
	SET [CustomerNameID] = @CustomerNameID
	  ,[EmployeeParentID] = @EmployeeParentID
	  ,[PartyType] = @PartyType
	  ,[FirstName] = @FirstName
	  ,[MiddleName] = @MiddleName
	  ,[LastName] = @LastName
	  ,[NameTitle] = @NameTitle
	  ,[AddressLine1] = @AddressLine1
	  ,[AddressLine2] = @AddressLine2
	  ,[AddressLine3] = @AddressLine3
	  ,[City] = @City
	  ,[State] = @State
	  ,[PostalCode] = @PostalCode
	  ,[SSN] = @SSN
	  ,[PhoneNumber] = @PhoneNumber
	  ,[Notes] = @Notes
	  ,[SourceID] = @SourceID
	  ,[DataSource] = @DataSource
	  ,[DateModified] = @DateModified
	  ,[ContributingCompany] = @ContributingCompany
	  ,[IDSourceCode] = @IDSourceCode
	  ,[UpdateUserName] = @UpdateUserName
	  ,[UpdateDate] = GETDATE()
	  ,[VersionCount] = [VersionCount] + 1
	WHERE   [CustomerParentID] = @CustomerParentID
END

GO
/****** Object:  StoredProcedure [dbo].[CustomerPhone_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[CustomerPhone_CREATE]
	@CustomerNameID int
	,@PhoneType varchar(10) = null
	,@PhoneNumber varchar(20) = null
	,@PhoneSuffix varchar(10) = null
	,@SourceID varchar(40) = null
	,@DataSource varchar(20) = null
	,@ContributingCompany varchar(50) = null
	,@DateModified smalldatetime = null
	,@IDSourceCode varchar(50) = null
	,@UpdateUserName varchar(50)
	,@EmployeePhoneID int = null
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[CustomerPhone]
		([CustomerNameID]
		,[PhoneType]
		,[PhoneNumber]
		,[PhoneSuffix]
		,[SourceID]
		,[DataSource]
		,[ContributingCompany]
		,[DateModified]
		,[IDSourceCode]
		,[UpdateUserName]
		,[UpdateDate]
		,[VersionCount]
		,[EmployeePhoneID])
     VALUES
		(@CustomerNameID
		,@PhoneType
		,@PhoneNumber
		,@PhoneSuffix
		,@SourceID
		,@DataSource
		,@ContributingCompany
		,@DateModified
		,@IDSourceCode
		,@UpdateUserName
		,GETDATE()
		,0
		,@EmployeePhoneID)

	IF @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	ELSE 
		select 0
END











GO
/****** Object:  StoredProcedure [dbo].[CustomerPhone_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CustomerPhone_DELETE]
	  @CustomerPhoneID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[CustomerPhone]
	WHERE [CustomerPhoneID] = @CustomerPhoneID
END





GO
/****** Object:  StoredProcedure [dbo].[CustomerPhone_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CustomerPhone_READ]
	@CustomerPhoneID int = null
	,@CustomerNameID int = null
	,@EmployeePhoneID int = null
	,@SortOption char(1) = null
	,@SourceID varchar(40) = null
AS
BEGIN
	SET NOCOUNT ON;
	if (@CustomerPhoneID is NULL and @CustomerNameID is NULL and @EmployeePhoneID is null and @SortOption = null and @SourceID = null)
		return;

	if (@SourceID is not null)
		SELECT [CustomerPhoneID]
			,[CustomerNameID]
			,[PhoneType]
			,[PhoneNumber]
			,[PhoneSuffix]
			,[SourceID]
			,[DataSource]
			,[DateModified]
			,[VersionCount]
			,[EmployeePhoneID]
			,[ContributingCompany]
			,[IDSourceCode]
		FROM [dbo].[CustomerPhone]
		WHERE [SourceID] = @SourceID
		order by [CustomerPhoneID]
	if (@SortOption is not null)
		SELECT [CustomerPhoneID]
			,[CustomerNameID]
			,[PhoneType]
			,[PhoneNumber]
			,[PhoneSuffix]
			,[SourceID]
			,[DataSource]
			,[DateModified]
			,[VersionCount]
			,[EmployeePhoneID]
			,[ContributingCompany]
			,[IDSourceCode]
		FROM [dbo].[CustomerPhone]
		WHERE [CustomerNameID] = @CustomerNameID
		order by [SourceID]
	ELSE if (@EmployeePhoneID is not NULL and @CustomerPhoneID is not NULL)
		SELECT [CustomerPhoneID]
			,[CustomerNameID]
			,[PhoneType]
			,[PhoneNumber]
			,[PhoneSuffix]
			,[SourceID]
			,[DataSource]
			,[DateModified]
			,[VersionCount]
			,[EmployeePhoneID]
			,[ContributingCompany]
			,[IDSourceCode]
		FROM [dbo].[CustomerPhone]
		WHERE CustomerPhoneID = (select min(CustomerPhoneID) from [dbo].[CustomerPhone]
								   where [EmployeePhoneID] = @EmployeePhoneID and [CustomerPhoneID] != @CustomerPhoneID)
	ELSE
		SELECT [CustomerPhoneID]
			,[CustomerNameID]
			,[PhoneType]
			,[PhoneNumber]
			,[PhoneSuffix]
			,[SourceID]
			,[DataSource]
			,[DateModified]
			,[VersionCount]
			,[EmployeePhoneID]
			,[ContributingCompany]
			,[IDSourceCode]
		FROM [dbo].[CustomerPhone]
		WHERE [CustomerPhoneID] = ISNULL(@CustomerPhoneID, CustomerPhoneID)
			and [CustomerNameID] = ISNULL(@CustomerNameID, CustomerNameID)
			and [EmployeePhoneID] = ISNULL(@EmployeePhoneID, EmployeePhoneID)
		order by [PhoneType],[DateModified] desc
END












GO
/****** Object:  StoredProcedure [dbo].[CustomerPhone_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[CustomerPhone_UPDATE]
	@CustomerPhoneID int
	,@CustomerNameID char(10)
	,@PhoneType varchar(10) = null
	,@PhoneNumber varchar(20) = null
	,@PhoneSuffix varchar(10) = null
	,@SourceID varchar(40) = null
	,@DataSource varchar(10) = null
	,@ContributingCompany varchar(50) = null
	,@DateModified smalldatetime = null
	,@IDSourceCode varchar(50) = null
	,@UpdateUserName varchar(50)
	,@EmployeePhoneID int = null
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE  [dbo].[CustomerPhone]
	SET     [CustomerNameID] = @CustomerNameID
			,[PhoneType] = @PhoneType
			,[PhoneNumber] = @PhoneNumber
			,[PhoneSuffix] = @PhoneSuffix
			,[UpdateUserName] = @UpdateUserName
			,[SourceID] = @SourceID
			,[DataSource] = @DataSource
			,[ContributingCompany] = @ContributingCompany
			,[DateModified] = @DateModified
			,[IDSourceCode] = @IDSourceCode
			,[UpdateDate] = GETDATE()
			,[VersionCount] = [VersionCount] + 1
			,[EmployeePhoneID] = @EmployeePhoneID
	WHERE   [CustomerPhoneID] = @CustomerPhoneID
END








GO
/****** Object:  StoredProcedure [dbo].[OrderReferredOrder_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[OrderReferredOrder_CREATE]
 @OrderInformationID int = null
,@CompanyOrderNumber varchar(255) = null
,@Company char(50) = null
,@CompanyDate datetime = null
,@SourceID varchar(255) = null
,@UpdateUserName varchar(50) = null
AS
BEGIN
SET NOCOUNT ON;


INSERT INTO [OrderReferredOrder]
           ([OrderInformationID]
           ,[CompanyOrderNumber]
           ,[Company]
           ,[CompanyDate]
           ,[SourceID]
           ,[UpdateUserName]
           ,[UpdateDate]
           ,[VersionCount])
     VALUES
           (
             @OrderInformationID
            ,@CompanyOrderNumber
            ,@Company
            ,@CompanyDate
            ,@SourceID
            ,@UpdateUserName
            ,GETDATE()
            ,0)

	IF @@ERROR = 0 AND @@ROWCOUNT = 1
   		select scope_identity()
	ELSE
   		select 0
END


GO
/****** Object:  StoredProcedure [dbo].[OrderReferredOrder_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[OrderReferredOrder_DELETE]
	  @ReferredOrderID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [OrderReferredOrder]
	WHERE [ReferredOrderID] = @ReferredOrderID
END


GO
/****** Object:  StoredProcedure [dbo].[OrderReferredOrder_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[OrderReferredOrder_READ]
	@ReferredOrderID int = null
	,@OrderInformationID int = null
	,@userName varchar(50) = null
AS
BEGIN
  SET NOCOUNT ON;

  if (@ReferredOrderID is NULL and @OrderInformationID is NULL)
     return;
     
	if (@username is not null)
   	begin
   		if (@ReferredOrderID is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='OrderReferredOrder_READ', @parameterName='ReferredOrderID', @parameterValue=@ReferredOrderID, @userName=@userName;
   		if (@OrderInformationID is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='OrderReferredOrder_READ', @parameterName='OrderInformationID', @parameterValue=@OrderInformationID, @userName=@userName;
	end     

  if (@ReferredOrderID is not NULL)
     SELECT 
       [ReferredOrderID]
      ,[OrderInformationID]
      ,[CompanyOrderNumber]
      ,[Company]
      ,[CompanyDate]
      ,[SourceID]
      ,[UpdateUserName]
      ,[UpdateDate]
      ,[VersionCount]

     FROM [dbo].[OrderReferredOrder]
     WHERE [ReferredOrderID] = @ReferredOrderID
  ELSE     SELECT
       [ReferredOrderID]
      ,[OrderInformationID]
      ,[CompanyOrderNumber]
      ,[Company]
      ,[CompanyDate]
      ,[SourceID]
      ,[UpdateUserName]
      ,[UpdateDate]
      ,[VersionCount]
    FROM [dbo].[OrderReferredOrder]
    WHERE [OrderInformationID] = @OrderInformationID
END

GO
/****** Object:  StoredProcedure [dbo].[OrderReferredOrder_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  StoredProcedure [dbo].[OrderReferredOrder_UPDATE] Script Date: 09/20/2007 09:56:23 ******/
CREATE PROCEDURE [dbo].[OrderReferredOrder_UPDATE]
   @ReferredOrderID int
  ,@OrderInformationID int
  ,@CompanyOrderNumber varchar(255) = null
  ,@Company char(50) = null
  ,@CompanyDate datetime = null
  ,@SourceID varchar(255) = null
  ,@UpdateUserName varchar(50) = null
AS
BEGIN
  SET NOCOUNT ON;

  UPDATE [dbo].[OrderReferredOrder]
    SET  [OrderInformationID] = @OrderInformationID
        ,[CompanyOrderNumber]  = @CompanyOrderNumber
        ,[Company]            = @Company
        ,[CompanyDate]        = @CompanyDate
        ,[SourceID]          = @SourceID
        ,[UpdateUserName]    = @UpdateUserName
        ,[UpdateDate] = GETDATE()
        ,[VersionCount] = [VersionCount] + 1
   WHERE [ReferredOrderID] = @ReferredOrderID
END









GO
/****** Object:  StoredProcedure [dbo].[OrderSourceIdDump]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[OrderSourceIdDump]
	 @sourceID varchar(255) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@sourceID is null)
		return;

	declare @cpid int
	declare @count int

	set @cpid = (select Customerid from Customer where SourceID = @sourceID)
	
	if (@cpid <= 0)
		return;
		
	select 'Employee' [Employee], sourceid, datasource, * from Customer where Customerid = @cpid;

	set @count = (select count(*) from caseinformation where Customerid = @cpid);
	if (@count > 0)
		select 'Order' [Order], sourceid, sourceCompany, * from caseinformation where Customerid = @cpid;
	
	set @count = (select count(*) from Customername where Customerid = @cpid and AliasIndicator != 'Y');
	if (@count > 0)
		select 'Name' [Name], sourceid, datasource, * from Customername where Customerid = @cpid and AliasIndicator != 'Y';
	
	set @count = (select count(*) from Customername where Customerid = @cpid and AliasIndicator = 'Y');
	if (@count > 0)
		select 'Alias' [Alias], sourceid, datasource,* from Customername where Customerid = @cpid and AliasIndicator = 'Y';

	set @count = (select count(*) from Customerattribute where Customernameid in (select Customernameid from Customername where Customerid = @cpid));
	if (@count > 0)
		select 'Att' [Att], sourceid, datasource, * from Customerattribute 
		where Customernameid in (select Customernameid from Customername where Customerid = @cpid);

	set @count = (select count(*) from Customeraddress where Customernameid in (select Customernameid from Customername where Customerid = @cpid));
	if (@count > 0)
		select 'Adr' [Adr], sourceid, datasource, Customeraddressid, Customernameid, Employeeaddressid, addresstype, addressline1, city, state, postalcode, addressline2, addressline3, * from Customeraddress   
		where Customernameid in (select Customernameid from Customername where Customerid = @cpid);

	set @count = (select count(*) from Customerphone where Customernameid in (select Customernameid from Customername where Customerid = @cpid));
	if (@count > 0)
		select 'Phn' [Phn], sourceid, datasource, * from Customerphone   
		where Customernameid in (select Customernameid from Customername where Customerid = @cpid);

	set @count = (select count(*) from Customerparent where Customernameid in (select Customernameid from Customername where Customerid = @cpid));
	if (@count > 0)
		select 'Parent' [Parent], sourceid, datasource, * from Customerparent where Customernameid in (select Customernameid from Customername where Customerid = @cpid);

	set @count = (select count(*) from Customerdescription where Customernameid in (select Customernameid from Customername where Customerid = @cpid));
	if (@count > 0)
		select 'Desc' [Desc],sourceid, datasource,* from Customerdescription  
		where Customernameid in (select Customernameid from Customername where Customerid = @cpid);
END
GO
/****** Object:  StoredProcedure [dbo].[CJISPortal_CrossReference]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CJISPortal_CrossReference]
	@caseNumber varchar(255), @accessAdultOnly bit = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@caseNumber is null)
		return;

  if(@accessAdultOnly = 1)
  begin
	SELECT distinct CompanyOrderNumber
      ,Company
      ,CompanyDate
      ,LastName
      ,FirstName
      ,MiddleName
      ,SuffixName
      ,OrderInformationID
	FROM
	(
	SELECT distinct [CompanyOrderNumber]
      ,r.[Company]
      ,[CompanyDate]
      ,[LastName]
      ,[FirstName]
      ,[MiddleName]
      ,[SuffixName]
      ,r.[OrderInformationID]
	FROM [dbo].[OrderReferredOrder] r
		inner join [dbo].[OrderInformation] c on c.OrderInformationID = r.OrderInformationID
		inner join [dbo].[Customername] n on n.CustomerID = c.CustomerID and AliasIndicator = 'n'
	where ([OrderNumber] = @caseNumber or CompanyOrderNumber = @caseNumber)
	    and n.NameType = 'adult'
	union
	SELECT distinct [OrderNumber]
      ,[OrderType]
      ,[RecordDate]
      ,[LastName]
      ,[FirstName]
      ,[MiddleName]
      ,[SuffixName]
      ,c.[OrderInformationID]
	FROM [dbo].[OrderReferredOrder] r
		inner join [dbo].[OrderInformation] c on c.OrderInformationID = r.OrderInformationID
		inner join [dbo].[Customername] n on n.CustomerID = c.CustomerID and AliasIndicator = 'n'
	where ([OrderNumber] = @caseNumber or CompanyOrderNumber = @caseNumber)
	    and n.NameType = 'adult'
	) as tbl
  end
  else
  begin
	SELECT distinct CompanyOrderNumber
      ,Company
      ,CompanyDate
      ,LastName
      ,FirstName
      ,MiddleName
      ,SuffixName
      ,OrderInformationID
	FROM
	(
	SELECT distinct [CompanyOrderNumber]
      ,r.[Company]
      ,[CompanyDate]
      ,[LastName]
      ,[FirstName]
      ,[MiddleName]
      ,[SuffixName]
      ,r.[OrderInformationID]
	FROM [dbo].[OrderReferredOrder] r
		inner join [dbo].[OrderInformation] c on c.OrderInformationID = r.OrderInformationID
		inner join [dbo].[Customername] n on n.CustomerID = c.CustomerID and AliasIndicator = 'n'
	where [OrderNumber] = @caseNumber or CompanyOrderNumber = @caseNumber
	union
	SELECT distinct [OrderNumber]
      ,[OrderType]
      ,[RecordDate]
      ,[LastName]
      ,[FirstName]
      ,[MiddleName]
      ,[SuffixName]
      ,c.[OrderInformationID]
	FROM [dbo].[OrderReferredOrder] r
		inner join [dbo].[OrderInformation] c on c.OrderInformationID = r.OrderInformationID
		inner join [dbo].[Customername] n on n.CustomerID = c.CustomerID and AliasIndicator = 'n'
	where [OrderNumber] = @caseNumber or CompanyOrderNumber = @caseNumber
	) as tbl
  end
END

GO
/****** Object:  StoredProcedure [dbo].[Defendant_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Defendant_READ]
	@OrderNumber varchar(25)
	,@userName varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;
	
	if @OrderNumber = null
		return;

   	if (@username is not null)
	begin
		exec audit_LogSelect_CREATE @storedProcedureName='Defendant_READ', @parameterName='OrderNumber', @parameterValue=@OrderNumber, @userName=@userName;
	end
	
	select distinct cpn.Item, ci.OrderNumber
	from CustomerName cpn 
	   inner join OrderInformation ci on (ci.CustomerID = cpn.CustomerID)
	where (ci.OrderNumber like @OrderNumber + '%') and (SourceCompany = 'odyssey') and (OrderType = 'court')
	order by cpn.Item
END
GO
/****** Object:  StoredProcedure [dbo].[GetValidationDocumentsList-PTYPE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetValidationDocumentsList-PTYPE]
	@NameType varchar(10)= null
	,@lastName varchar(50) = null
	,@firstName varchar(50) = null
	,@item char(10) = null
	,@userName varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@username is not null)
	begin
		if (@nameType is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='GetValidationDocumentsList-PTYPE', @parameterName='nameType', @parameterValue=@nameType, @userName=@userName;
		if (@lastName is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='GetValidationDocumentsList-PTYPE', @parameterName='lastName', @parameterValue=@lastName, @userName=@userName;
		if (@firstName is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='GetValidationDocumentsList-PTYPE', @parameterName='firstName', @parameterValue=@firstName, @userName=@userName;
		if (@item is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='GetValidationDocumentsList-PTYPE', @parameterName='Item', @parameterValue=@item, @userName=@userName;
	end

	if (@NameType = 'Adult')
		SELECT [BusinessExceptionID]
			   ,[BusinessException].[EmployeeNameID]
			   ,[BusinessException].[CustomerNameID]
			  ,[BusinessException].[Item]
			  ,[EmployeeName].[FirstName]
			  ,[EmployeeName].[LastName]
			  ,[BusinessException].[Reason]
			  ,[BusinessException].[Company]
	--		  ,[EmployeeName].[ContributingCompany]
 			  ,[BusinessException].[ValidationStatus]
			  ,[BusinessException].[UpdateUserName]
			  ,[BusinessException].[ValidationDate]
			,[EmployeeName].[NameType]
			,[EmployeeName].[SourceID]
		FROM 
			[dbo].[BusinessException] (READUNCOMMITTED),
			[dbo].[EmployeeName] (READUNCOMMITTED)
		WHERE 
				[DocProcessed] = 'N' 
			and [NameType] = 'Adult'
			and [EmployeeName].[EmployeeNameID] = [BusinessException].[EmployeeNameID]
			and (@lastName is null or ([EmployeeName].[LastName] like @lastName + '%'))
			and (@firstName is null or ([EmployeeName].[FirstName] like @firstName + '%'))
			and (@item is null or ([BusinessException].[Item] = @item))
			order by [BusinessException].[updatedate] desc
	else if (@NameType = 'Juvenile')
			SELECT [BusinessExceptionID]
			   ,[BusinessException].[EmployeeNameID]
			   ,[BusinessException].[CustomerNameID]
			  ,[BusinessException].[Item]
			  ,[EmployeeName].[FirstName]
			  ,[EmployeeName].[LastName]
			  ,[BusinessException].[Reason]
			  ,[BusinessException].[Company]
	--		  ,[EmployeeName].[ContributingCompany]
  			  ,[BusinessException].[ValidationStatus]
			  ,[BusinessException].[UpdateUserName]
			  ,[BusinessException].[ValidationDate]
			,[EmployeeName].[NameType]
			,[EmployeeName].[SourceID]
		FROM 
			[dbo].[BusinessException] (READUNCOMMITTED),
			[dbo].[EmployeeName] (READUNCOMMITTED)
		WHERE 
				[DocProcessed] = 'N' 
			and [NameType] = 'Juvenile'
			and [EmployeeName].[EmployeeNameID] = [BusinessException].[EmployeeNameID]
			and (@lastName is null or ([EmployeeName].[LastName] like @lastName + '%'))
			and (@firstName is null or ([EmployeeName].[FirstName] like @firstName + '%'))
			and (@item is null or ([BusinessException].[Item] = @item))
		order by [BusinessException].[updatedate] desc
	else
		SELECT [BusinessExceptionID]
			   ,[BusinessException].[EmployeeNameID]
			   ,[BusinessException].[CustomerNameID]
			  ,[BusinessException].[Item]
			  ,[EmployeeName].[FirstName]
			  ,[EmployeeName].[LastName]
			  ,[BusinessException].[Reason]
			  ,[BusinessException].[Company]
	--		  ,[EmployeeName].[ContributingCompany]
  			  ,[BusinessException].[ValidationStatus]
			  ,[BusinessException].[UpdateUserName]
			  ,[BusinessException].[ValidationDate]
			,[EmployeeName].[NameType]
			,[EmployeeName].[SourceID]
		FROM 
			[dbo].[BusinessException] (READUNCOMMITTED),
			[dbo].[EmployeeName] (READUNCOMMITTED)
		WHERE 
				[DocProcessed] = 'N' 
			and [EmployeeName].[EmployeeNameID] = [BusinessException].[EmployeeNameID]
			and (@lastName is null or ([EmployeeName].[LastName] like @lastName + '%'))
			and (@firstName is null or ([EmployeeName].[FirstName] like @firstName + '%'))
			and (@item is null or ([BusinessException].[Item] = @item))
			order by [BusinessException].[updatedate] desc
END

GO
/****** Object:  StoredProcedure [dbo].[localIDDump]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Batch submitted through debugger: SQLQuery1.sql|7|0|C:\Users\rlittlefield\AppData\Local\Temp\~vsAE2D.sql
CREATE procedure [dbo].[localIDDump]
	 @CompanyID varchar(25) = null
AS
BEGIN
	SET NOCOUNT ON;

	IF (@CompanyID is null)
		return;

	declare @cpid int
	declare @count int

	set @cpid = (select Customerid from Customer where CompanyID = @CompanyID)
	
	if (@cpid <= 0)
		set @cpid = (select Customerid from caseinformation where OrderNumber = @CompanyID)
	
	if (@cpid <= 0)
		return;
	
	set @count = (select count(*) from Customer where Customerid = @cpid);
	if (@count > 0)
		select 'Employee' [Employee],sourceid, datasource,* from Customer where Customerid = @cpid;

	set @count = (select count(*) from caseinformation where Customerid = @cpid);
	if (@count > 0)
		select 'Order' [Order],sourceid, sourceCompany,* from caseinformation   where Customerid = @cpid;
	
	set @count = (select count(*) from Customername where Customerid = @cpid and AliasIndicator != 'Y');
	if (@count > 0)
		select 'Name' [Name],sourceid, datasource,* from Customername where Customerid = @cpid and AliasIndicator != 'Y';
	
	set @count = (select count(*) from Customerparent where Customernameid in (select Customernameid from Customername where Customerid = @cpid));
	if (@count > 0)
		select 'Parent' [Parent], sourceid, datasource,* from Customerparent where Customernameid in (select Customernameid from Customername where Customerid = @cpid);

	set @count = (select count(*) from Customername where Customerid = @cpid and AliasIndicator = 'Y');
	if (@count > 0)
		select 'Alias' [Alias],sourceid, datasource,* from Customername where Customerid = @cpid and AliasIndicator = 'Y';

	set @count = (select count(*) from Customeraddress where Customernameid in (select Customernameid from Customername where Customerid = @cpid));
	if (@count > 0)
		select 'Adr' [Adr],sourceid, datasource, Customeraddressid, Customernameid, Employeeaddressid, addresstype, addressline1, city, state, postalcode, addressline2, addressline3, * from Customeraddress   
		where Customernameid in (select Customernameid from Customername where Customerid = @cpid);

	set @count = (select count(*) from Customerattribute where Customernameid in (select Customernameid from Customername where Customerid = @cpid));
	if (@count > 0)
		select 'Att' [Att],sourceid, datasource,* from Customerattribute 
		where Customernameid in (select Customernameid from Customername where Customerid = @cpid);

	set @count = (select count(*) from Customerdescription where Customernameid in (select Customernameid from Customername where Customerid = @cpid));
	if (@count > 0)
		select 'Desc' [Desc],sourceid, datasource,* from Customerdescription  
		where Customernameid in (select Customernameid from Customername where Customerid = @cpid);

	set @count = (select count(*) from Customerphone where Customernameid in (select Customernameid from Customername where Customerid = @cpid));
	if (@count > 0)
		select 'Phn' [Phn],sourceid, datasource,* from Customerphone   
		where Customernameid in (select Customernameid from Customername where Customerid = @cpid);

	--set @count = (select count(*) from Customerevent where Customerid = @cpid);
	--if (@count > 0)
	--	select 'Evnt' [Evnt],sourceid, datasource,* from Customerevent 
	--	where Customerid = @cpid;
END

GO
/****** Object:  StoredProcedure [dbo].[LookupType_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LookupType_CREATE]
	@LookupTypeCode varchar(50)
	,@ActiveIndicator varchar(1) = null
	,@Description varchar(50)
	,@ExchangeUse bit
	,@Owner varchar(10)
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[LookupType]
           ([LookupTypeCode]
           ,[ActiveIndicator]
           ,[Description]
           ,[ExchangeUse]
           ,[Owner]
           ,[UpdateUserName]
           ,[UpdateDate]
           ,[VersionCount])
     VALUES
           (@LookupTypeCode
           ,@ActiveIndicator
           ,@Description
           ,@ExchangeUse
           ,@Owner
           ,@UpdateUserName
           ,GETDATE()
           ,0)
END


GO
/****** Object:  StoredProcedure [dbo].[LookupType_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LookupType_DELETE]
	@LookupTypeCode varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[LookupType]
	WHERE [LookupTypeCode] = @LookupTypeCode
END

GO
/****** Object:  StoredProcedure [dbo].[LookupType_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LookupType_READ]
	@LookupTypeCode varchar(50) = NULL,
	@ActiveIndicator varchar(1) = NULL,
	@ExchangeUse bit = NULL
AS
BEGIN
	SET NOCOUNT ON;

--DECLARE @LookupTypeCode varchar(50), @ActiveIndicator varchar(1), @ExchangeUse bit
--SET @LookupTypeCode = 'HairColor'  SET  @ActiveIndicator = '1'  SET @ExchangeUse = 1

	if (@LookupTypeCode is NULL)
	  AND (@ActiveIndicator is NULL)
	  AND (@ExchangeUse is NULL)
		return;

IF (@LookupTypeCode is NULL)
  BEGIN
    IF (@ActiveIndicator = '1' and @ExchangeUse = 1)
      BEGIN
	    SELECT [LookupTypeCode]
		  ,[ActiveIndicator]
		  ,[Description]
		  ,[ExchangeUse]
		  ,[Owner]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[VersionCount]
	    FROM [dbo].[LookupType]
	    WHERE ActiveIndicator = @ActiveIndicator AND ExchangeUse = @ExchangeUse
      END
     ELSE
      BEGIN
        IF (@ActiveIndicator IS NOT NULL)
          BEGIN
	        SELECT [LookupTypeCode]
		      ,[ActiveIndicator]
		      ,[Description]
			  ,[ExchangeUse]
			  ,[Owner]
			  ,[UpdateUserName]
			  ,[UpdateDate]
			  ,[VersionCount]
			FROM [dbo].[LookupType]
			WHERE ActiveIndicator = @ActiveIndicator
          END
      END
  END
ELSE  -- (@LookupTypeCode is NOT NULL)
  BEGIN
	SELECT [LookupTypeCode]
		  ,[ActiveIndicator]
		  ,[Description]
		  ,[ExchangeUse]
		  ,[Owner]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[VersionCount]
	  FROM [dbo].[LookupType]
	WHERE LookupTypeCode = @LookupTypeCode
  END
END

GO
/****** Object:  StoredProcedure [dbo].[LookupType_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LookupType_UPDATE]
	@LookupTypeCode varchar(50)
	,@ActiveIndicator varchar(1) = null
	,@Description varchar(50)
	,@ExchangeUse bit
	,@Owner varchar(10)
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[LookupType]
	SET    [ActiveIndicator] = @ActiveIndicator
           ,[Description] = @Description
           ,[ExchangeUse] = @ExchangeUse
           ,[Owner] = @Owner
           ,[UpdateUserName] = @UpdateUserName
           ,[UpdateDate] = GETDATE()
           ,[VersionCount] = [VersionCount] + 1
	 WHERE [LookupTypeCode] = @LookupTypeCode
END

GO
/****** Object:  StoredProcedure [dbo].[LookupTypeValue_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LookupTypeValue_CREATE]
	@LookupTypeCode varchar(50)
	,@LookupTypeValue varchar(30)
	,@ValueDescription varchar(1000)
	,@ActiveIndicator char(1)
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[LookupTypeValue]
           ([LookupTypeCode]
		   ,[LookupTypeValue]
		   ,[ValueDescription]
           ,[ActiveIndicator]
           ,[UpdateUserName]
           ,[UpdateDate]
           ,[VersionCount])
     VALUES
           (@LookupTypeCode
		   ,@LookupTypeValue
		   ,@ValueDescription
           ,@ActiveIndicator
           ,@UpdateUserName
           ,GETDATE()
           ,0)
END

GO
/****** Object:  StoredProcedure [dbo].[LookupTypeValue_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LookupTypeValue_DELETE]
	@LookupTypeCode varchar(50)
	,@LookupTypeValue varchar(30)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[LookupTypeValue]
	WHERE [LookupTypeCode] = @LookupTypeCode
    AND [LookupTypeValue] = @LookupTypeValue
END

GO
/****** Object:  StoredProcedure [dbo].[LookupTypeValue_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LookupTypeValue_READ]
	 @LookupTypeCode varchar(50) = null
	,@LookupTypeValue varchar(30)= null
	,@orderBy int = 0  -- 0 is no order by, 1 == order by ValueDescription and 2 == order by LookupTypeValue
AS
BEGIN
	SET NOCOUNT ON;

	--if (@LookupTypeCode is NULL)
	--	return;

    -- Need to be able to select all records for LookupTypeCode and Value dropdowns
  if (@LookupTypeCode is NULL)
  begin
	SELECT [LookupTypeCode]
      ,[LookupTypeValue]
      ,[ValueDescription]
      ,[ActiveIndicator]
      ,[UpdateUserName]
      ,[UpdateDate]
      ,[VersionCount]
	FROM [dbo].[LookupTypeValue]
  end
  else if (@LookupTypeCode is not null and @LookupTypeValue is null and @orderBy is null)
  begin
	SELECT [LookupTypeCode]
      ,[LookupTypeValue]
      ,[ValueDescription]
      ,[ActiveIndicator]
      ,[UpdateUserName]
      ,[UpdateDate]
      ,[VersionCount]
	FROM [dbo].[LookupTypeValue]
	WHERE [LookupTypeCode] = @LookupTypeCode
	order by LookupTypeValue
  end
  else  -- if (@LookupTypeCode is NULL)
  begin
	if (@LookupTypeCode = 'ParentCompanyCompany')
	begin
	-- 'ParentCompanyCompany' has some special handling around it for the portal, so check for this code first
			/* 
		       This set of select statements have two lookups into the same table
		       a: gets a list of parent agencies
		       b: a list of agencies associated to a
		    */
			select a.[LookupTypeCode]
				,a.[LookupTypeValue] 
				,b.[ValueDescription]
				,b.[ActiveIndicator]
				,b.[UpdateUserName]
				,b.[UpdateDate]
				,b.[VersionCount]
			from dbo.lookuptypevalue a, lookuptypevalue b
			where a.lookuptypecode = 'ParentCompanyCompany' and b.lookuptypecode = 'Company'
			    and b.LookupTypeValue = a.ValueDescription
	end
	else if @orderBy = 0
	begin
	SELECT [LookupTypeCode]
      ,[LookupTypeValue]
      ,[ValueDescription]
      ,[ActiveIndicator]
      ,[UpdateUserName]
      ,[UpdateDate]
      ,[VersionCount]
	FROM [dbo].[LookupTypeValue]
	WHERE [LookupTypeCode] = @LookupTypeCode
	AND [LookupTypeValue] = ISNULL(@LookupTypeValue, LookupTypeValue)
	end
	else if @orderBy = 1
	begin
	SELECT [LookupTypeCode]
      ,[LookupTypeValue]
      ,[ValueDescription]
      ,[ActiveIndicator]
      ,[UpdateUserName]
      ,[UpdateDate]
      ,[VersionCount]
	FROM [dbo].[LookupTypeValue]
	WHERE [LookupTypeCode] = @LookupTypeCode
	AND [LookupTypeValue] = ISNULL(@LookupTypeValue, LookupTypeValue)
	order by ValueDescription
	end
	else  -- @orderBy = 2
	begin
	SELECT [LookupTypeCode]
      ,[LookupTypeValue]
      ,[ValueDescription]
      ,[ActiveIndicator]
      ,[UpdateUserName]
      ,[UpdateDate]
      ,[VersionCount]
	FROM [dbo].[LookupTypeValue]
	WHERE [LookupTypeCode] = @LookupTypeCode
	AND [LookupTypeValue] = ISNULL(@LookupTypeValue, LookupTypeValue)
	order by LookupTypeValue
	end
  end  --if (@LookupTypeCode is NULL)
END
GO
/****** Object:  StoredProcedure [dbo].[LookupTypeValue_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LookupTypeValue_UPDATE]
	@LookupTypeCode varchar(50)
	,@LookupTypeValue varchar(30)
	,@ValueDescription varchar(1000)
	,@ActiveIndicator char(1)
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[LookupTypeValue]
	SET    [LookupTypeValue] = @LookupTypeValue
           ,[ValueDescription] = @ValueDescription
		   ,[ActiveIndicator] = @ActiveIndicator
           ,[UpdateUserName] = @UpdateUserName
           ,[UpdateDate] = GETDATE()
           ,[VersionCount] = [VersionCount] + 1
	 WHERE [LookupTypeCode] = @LookupTypeCode
  --   AND [LookupTypeValue] = @LookupTypeValue
END

GO
/****** Object:  StoredProcedure [dbo].[LookupTypeValueTranslation_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LookupTypeValueTranslation_CREATE]
	@LookupTypeCode varchar(50)
	,@LookupTypeValue varchar(30)
	,@Company varchar(20)
	,@CodeValue char(200)
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[LookupTypeValueTranslation]
		([LookupTypeCode]
		,[LookupTypeValue]
		,[Company]
		,[CodeValue]
		,[UpdateUserName]
		,[UpdateDate]
		,[Version])
	VALUES
		(@LookupTypeCode
		,@LookupTypeValue
		,@Company
		,@CodeValue
		,@UpdateUserName
		,GETDATE()
		,0)
END
GO
/****** Object:  StoredProcedure [dbo].[LookupTypeValueTranslation_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LookupTypeValueTranslation_DELETE]
	@LookupTypeValueTranslationID int
AS
BEGIN
	SET NOCOUNT ON;

	if (@LookupTypeValueTranslationID is null)
		return;
		
	DELETE
	FROM [dbo].[LookupTypeValueTranslation]
	WHERE LookupTypeValueTranslationID = @LookupTypeValueTranslationID

END
GO
/****** Object:  StoredProcedure [dbo].[LookupTypeValueTranslation_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LookupTypeValueTranslation_READ]
	@LookupTypeValueTranslationID int = null
	,@LookupTypeCode varchar(50) = null
	,@LookupTypeValue varchar(30) = null
	,@Company varchar(20) = null
	,@CodeValue char(200) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@LookupTypeCode is NULL and @LookupTypeValue is null and @Company is null and @CodeValue is null and @LookupTypeValueTranslationID is null)
		return;

  if (@LookupTypeValueTranslationID is null)
  begin
	if (@LookupTypeCode is not null and @LookupTypeValue is null and @Company is null and @CodeValue is null)
	begin
		SELECT [LookupTypeValueTranslationID]
		  ,[LookupTypeCode]
		  ,[LookupTypeValue]
		  ,[Company]
		  ,[CodeValue]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[Version]
		FROM [dbo].[LookupTypeValueTranslation]
		WHERE [LookupTypeCode] = @LookupTypeCode
		order by LookupTypeValue, Company, CodeValue
	end
	else if (@LookupTypeCode is null and @LookupTypeValue is not null and @Company is null and @CodeValue is null)
	begin
		SELECT [LookupTypeValueTranslationID]
		  ,[LookupTypeCode]
		  ,[LookupTypeValue]
		  ,[Company]
		  ,[CodeValue]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[Version]
		FROM [dbo].[LookupTypeValueTranslation]
		WHERE [LookupTypeValue] = @LookupTypeValue
		order by LookupTypeCode, Company, CodeValue
	end
	else if (@LookupTypeCode is null and @LookupTypeValue is null and @Company is not null and @CodeValue is null)
	begin
		SELECT [LookupTypeValueTranslationID]
		  ,[LookupTypeCode]
		  ,[LookupTypeValue]
		  ,[Company]
		  ,[CodeValue]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[Version]
		FROM [dbo].[LookupTypeValueTranslation]
		WHERE [Company] = @Company
		order by LookupTypeCode, LookupTypeValue, CodeValue
	end
	else if (@LookupTypeCode is null and @LookupTypeValue is null and @Company is null and @CodeValue is not null)
	begin
		SELECT [LookupTypeValueTranslationID]
		  ,[LookupTypeCode]
		  ,[LookupTypeValue]
		  ,[Company]
		  ,[CodeValue]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[Version]
		FROM [dbo].[LookupTypeValueTranslation]
		WHERE [CodeValue] = @CodeValue
		order by LookupTypeCode, LookupTypeValue, Company
	end
	else if (@LookupTypeCode is not null and @LookupTypeValue is not null and @Company is not null and @CodeValue is null)
	begin  -- this should be a unique row, if it's not there is an issue
		SELECT [LookupTypeValueTranslationID]
		  ,[LookupTypeCode]
		  ,[LookupTypeValue]
		  ,[Company]
		  ,[CodeValue]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[Version]
		FROM [dbo].[LookupTypeValueTranslation]
		WHERE [LookupTypeCode] = @LookupTypeCode and [Company] = @Company and [LookupTypeValue] = @LookupTypeValue
		order by LookupTypeValue
	end
	else  -- gets unique row
	begin
		SELECT [LookupTypeValueTranslationID]
		  ,[LookupTypeCode]
		  ,[LookupTypeValue]
		  ,[Company]
		  ,[CodeValue]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[Version]
		FROM [dbo].[LookupTypeValueTranslation]
		WHERE [LookupTypeCode] = @LookupTypeCode and [LookupTypeValue] = @LookupTypeValue and [Company] = @Company and [CodeValue] = @CodeValue
	end
  end
  else  -- @LookupTypeValueTranslationID is not null
  begin
		SELECT [LookupTypeValueTranslationID]
		  ,[LookupTypeCode]
		  ,[LookupTypeValue]
		  ,[Company]
		  ,[CodeValue]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[Version]
		FROM [dbo].[LookupTypeValueTranslation]
		WHERE [LookupTypeValueTranslationID] = @LookupTypeValueTranslationID
  end
end
GO
/****** Object:  StoredProcedure [dbo].[LookupTypeValueTranslation_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LookupTypeValueTranslation_UPDATE]
	@LookupTypeValueTranslationID int
	,@LookupTypeCode varchar(50)
	,@LookupTypeValue varchar(30)
	,@Company varchar(20)
	,@CodeValue char(200)
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	
	if (@LookupTypeValueTranslationID is NULL or @LookupTypeCode is NULL or @LookupTypeValue is NULL or @Company is NULL or @CodeValue is NULL 
	    or @UpdateUserName is NULL)
		return;

	UPDATE [dbo].[LookupTypeValueTranslation]
		SET [LookupTypeCode] = @LookupTypeCode
			,[LookupTypeValue] = @LookupTypeValue
			,[Company] = @Company
			,[CodeValue] = @CodeValue
			,[UpdateUserName] = @UpdateUserName
			,[UpdateDate] = GETDATE()
			,[Version] = [Version] + 1
	WHERE LookupTypeValueTranslationID = @LookupTypeValueTranslationID
END

GO
/****** Object:  StoredProcedure [dbo].[LookupTypeValueTranslationsByLookupTypeCode]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[LookupTypeValueTranslationsByLookupTypeCode]
	@LookupTypeCode varchar(50)
	,@userName varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;

-- TEST INPUT VARIABLE SECTION - DELETE OR COMMENT BEFORE UPDATING STORED PROCEDURE
--DECLARE @LookupTypeCode varchar(50), @userName varchar(50)
--SET @LookupTypeCode = 'activityType'
-- END TEST SECTION

	if (@LookupTypeCode is NULL)
		return;

 --  	if (@userName is not null)
	--begin
	--	exec audit_LogSelect_CREATE @storedProcedureName='[GetLookupTypeValueTranslationsByLookupTypeCode]', @parameterName='LookupTypeCode', @parameterValue=@LookupTypeCode, @userName=@userName;
	--end

	SELECT dbo.LookupTypeValueTranslation.LookupTypeValueTranslationID,
	       dbo.LookupTypeValue.LookupTypeCode,
           dbo.LookupTypeValue.LookupTypeValue,
           dbo.LookupTypeValue.ValueDescription,
           dbo.LookupTypeValue.ActiveIndicator,
           dbo.LookupTypeValueTranslation.UpdateUserName,
           dbo.LookupTypeValueTranslation.UpdateDate,
           dbo.LookupTypeValueTranslation.[Version],
           dbo.LookupTypeValueTranslation.Company,
           dbo.LookupTypeValueTranslation.CodeValue
		FROM dbo.LookupTypeValue 
		  LEFT OUTER JOIN dbo.LookupTypeValueTranslation 
		    ON (
		      dbo.LookupTypeValue.LookupTypeCode = dbo.LookupTypeValueTranslation.LookupTypeCode
		      AND dbo.LookupTypeValue.LookupTypeValue = dbo.LookupTypeValueTranslation.LookupTypeValue
		      )
		WHERE dbo.LookupTypeValue.LookupTypeCode = @LookupTypeCode


END

GO
/****** Object:  StoredProcedure [dbo].[MatchMerge_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[MatchMerge_CREATE]
	@Action varchar(50)
	,@FromLastName varchar(100)
	,@FromFirstName varchar(100) = null
	,@FromMiddleName varchar(100) = null
	,@FromItem char(10) = null
	,@FromLSID char(10)
	,@ToLastName varchar(100)
	,@ToFirstName varchar(100) = null
	,@ToMiddleName varchar(100) = null
	,@ToItem char(10)
	,@ToLSID char(10)
	,@Company varchar(50) = null
	,@UpdateUserName varchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[MatchMerge]
		([Action]
		,[FromLastName]
		,[FromFirstName]
		,[FromMiddleName]
		,[FromItem]
		,[FromLSID]
		,[ToLastName]
		,[ToFirstName]
		,[ToMiddleName]
		,[ToItem]
		,[ToLSID]
		,[Company]
		,[UpdateUserName]
		,[UpdateDate]
		,[VersionCount])
	VALUES
		(@Action
		,@FromLastName
		,@FromFirstName
		,@FromMiddleName
		,@FromItem
		,@FromLSID
		,@ToLastName
		,@ToFirstName
		,@ToMiddleName
		,@ToItem
		,@ToLSID
		,@Company
		,@UpdateUserName
		,GETDATE()
		,0)

	IF @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	ELSE 
		select 0
end

GO
/****** Object:  StoredProcedure [dbo].[MatchMerge_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MatchMerge_DELETE]
	  @matchMergeID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[MatchMerge]
	WHERE [MatchMergeID] = @matchMergeID
END

GO
/****** Object:  StoredProcedure [dbo].[MatchMerge_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MatchMerge_READ]
	@matchMergeID int = null
	,@lastName varchar(100) = null
	,@firstName varchar(100) = null
	,@item char(10) = null
	,@action varchar(50) = null
	,@minDate datetime = null
	,@maxDate datetime = null
	,@allAgencies bit
	,@userName varchar(50) = null
	,@userRole varchar(50) = null

AS
BEGIN
	SET NOCOUNT ON;

print @lastName
--	declare @sql  Nvarchar(4000)
--	declare @agencies varchar(50)
--	if (@isAllFullMatch = 1)
--		set @agencies = '''court'', ''da'', ''probation'', ''sheriff'''
--	else
--		set @agencies = '''court'', ''da'''
--
--print @agencies

--set @sql = (select 'SELECT [MatchMergeID]
--		,[Action]
--		,[FromName]
--		,[FromItem]
--		,[FromLSID]
--		,[ToName]
--		,[ToItem]
--		,[ToLSID]
--		,[Company]
--		,[UpdateUserName]
--		,[UpdateDate]
--		,[VersionCount]
--	FROM [dbo].[MatchMerge]
--	where (' + @action + ' is null or ([Action] = ' + @action + '))
--		and (CHARINDEX(QUOTENAME([Company] , ''''), ' + @agencies + ') <> -1)' )
--
--print 'got here...'
--print @sql
--print 'and here...'
--exec sp_ExecuteSQL @SQL;

   	if (@username is not null)
	begin
		if (@matchMergeID is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='MatchMerge_READ', @parameterName='matchMergeID', @parameterValue=@matchMergeID, @userName=@userName;

		if (@lastName is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='MatchMerge_READ', @parameterName='lastName', @parameterValue=@lastName, @userName=@userName;
		
		if (@firstName is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='MatchMerge_READ', @parameterName='firstName', @parameterValue=@firstName, @userName=@userName;

		if (@item is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='MatchMerge_READ', @parameterName='Item', @parameterValue=@item, @userName=@userName;
		
		if (@action is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='MatchMerge_READ', @parameterName='action', @parameterValue=@action, @userName=@userName;

		if (@allAgencies = 1)
			exec audit_LogSelect_CREATE @storedProcedureName='MatchMerge_READ', @parameterName='Company', @parameterValue='all agencies', @userName=@userName;
		else
			exec audit_LogSelect_CREATE @storedProcedureName='MatchMerge_READ', @parameterName='Company', @parameterValue='not Sheriff', @userName=@userName;
		
		if (@minDate is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='MatchMerge_READ', @parameterName='minDate', @parameterValue=@minDate, @userName=@userName;

		if (@maxDate is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='MatchMerge_READ', @parameterName='maxDate', @parameterValue=@maxDate, @userName=@userName;
	end

	if ((@userRole is not null) and (@userRole = 'adult'))
		if (@allAgencies = 1)
			SELECT [MatchMergeID]
				,[Action]
				,[FromLastName]
				,[FromFirstName]
				,[FromMiddleName]
				,[FromItem]
				,[FromLSID]
				,[ToLastName]
				,[ToFirstName]
				,[ToMiddleName]
				,[ToItem]
				,[ToLSID]
				,[Company]
				,[UpdateUserName]
				,[UpdateDate]
				,[VersionCount]
			FROM [dbo].[MatchMerge]
			where (@action is null or ([Action] = @action))
				and ((@lastName is null or ([FromLastName] like @lastName + '%'))
					or (@lastName is null or ([ToLastName] like @lastName + '%')))
				and ((@firstName is null or ([FromFirstName] like @firstName + '%'))
					or (@firstName is null or ([ToFirstName] like @firstName + '%')))
				and ((@item is null or ([FromItem] = @item))
					or (@item is null or ([ToItem] = @item)))
				and (@matchMergeID is null or [MatchMergeID] = @matchMergeID)
				and ([UpdateDate] between coalesce(@minDate, '1/1/1900') and coalesce(@maxDate, '12/31/2999'))
				and (substring(tolsid, 1, 1) <> 'J')
	--			and (CHARINDEX(QUOTENAME([Company] , ''''), @agencies) <> -1)
	--			and [Company] in ('court', 'da', 'probation', 'sheriff')
			order by [UpdateDate] desc
		else
			SELECT [MatchMergeID]
				,[Action]
				,[FromLastName]
				,[FromFirstName]
				,[FromMiddleName]
				,[FromItem]
				,[FromLSID]
				,[ToLastName]
				,[ToFirstName]
				,[ToMiddleName]
				,[ToItem]
				,[ToLSID]
				,[Company]
				,[UpdateUserName]
				,[UpdateDate]
				,[VersionCount]
			FROM [dbo].[MatchMerge]
			where (@action is null or ([Action] = @action))
				and ((@lastName is null or ([FromLastName] like @lastName + '%'))
					or (@lastName is null or ([ToLastName] like @lastName + '%')))
				and ((@firstName is null or ([FromFirstName] like @firstName + '%'))
					or (@firstName is null or ([ToFirstName] like @firstName + '%')))
				and ((@item is null or ([FromItem] = @item))
					or (@item is null or ([ToItem] = @item)))
				and (@matchMergeID is null or [MatchMergeID] = @matchMergeID)
				and ([UpdateDate] between coalesce(@minDate, '1/1/1900') and coalesce(@maxDate, '12/31/2999'))
				and (substring(tolsid, 1, 1) <> 'J')
	--			and (CHARINDEX(QUOTENAME([Company] , ''''), @agencies) <> -1)
				and [Company] not in ('sheriff')
			order by [UpdateDate] desc
	else  -- when user has 'juvenile' role they can view all names
		if (@allAgencies = 1)
			SELECT [MatchMergeID]
				,[Action]
				,[FromLastName]
				,[FromFirstName]
				,[FromMiddleName]
				,[FromItem]
				,[FromLSID]
				,[ToLastName]
				,[ToFirstName]
				,[ToMiddleName]
				,[ToItem]
				,[ToLSID]
				,[Company]
				,[UpdateUserName]
				,[UpdateDate]
				,[VersionCount]
			FROM [dbo].[MatchMerge]
			where (@action is null or ([Action] = @action))
				and ((@lastName is null or ([FromLastName] like @lastName + '%'))
					or (@lastName is null or ([ToLastName] like @lastName + '%')))
				and ((@firstName is null or ([FromFirstName] like @firstName + '%'))
					or (@firstName is null or ([ToFirstName] like @firstName + '%')))
				and ((@item is null or ([FromItem] = @item))
					or (@item is null or ([ToItem] = @item)))
				and (@matchMergeID is null or [MatchMergeID] = @matchMergeID)
				and ([UpdateDate] between coalesce(@minDate, '1/1/1900') and coalesce(@maxDate, '12/31/2999'))
	--			and (CHARINDEX(QUOTENAME([Company] , ''''), @agencies) <> -1)
	--			and [Company] in ('court', 'da', 'probation', 'sheriff')
			order by [UpdateDate] desc
		else
			SELECT [MatchMergeID]
				,[Action]
				,[FromLastName]
				,[FromFirstName]
				,[FromMiddleName]
				,[FromItem]
				,[FromLSID]
				,[ToLastName]
				,[ToFirstName]
				,[ToMiddleName]
				,[ToItem]
				,[ToLSID]
				,[Company]
				,[UpdateUserName]
				,[UpdateDate]
				,[VersionCount]
			FROM [dbo].[MatchMerge]
			where (@action is null or ([Action] = @action))
				and ((@lastName is null or ([FromLastName] like @lastName + '%'))
					or (@lastName is null or ([ToLastName] like @lastName + '%')))
				and ((@firstName is null or ([FromFirstName] like @firstName + '%'))
					or (@firstName is null or ([ToFirstName] like @firstName + '%')))
				and ((@item is null or ([FromItem] = @item))
					or (@item is null or ([ToItem] = @item)))
				and (@matchMergeID is null or [MatchMergeID] = @matchMergeID)
				and ([UpdateDate] between coalesce(@minDate, '1/1/1900') and coalesce(@maxDate, '12/31/2999'))
	--			and (CHARINDEX(QUOTENAME([Company] , ''''), @agencies) <> -1)
				and [Company] not in ('sheriff')
			order by [UpdateDate] desc
END

GO
/****** Object:  StoredProcedure [dbo].[MatchMerge_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MatchMerge_UPDATE]
	@MatchMergeID int
	,@Action varchar(50)
	,@FromLastName varchar(100)
	,@FromFirstName varchar(100) = null
	,@FromMiddleName varchar(100) = null
	,@FromItem char(10) = null
	,@FromLSID char(10)
	,@ToLastName varchar(100)
	,@ToFirstName varchar(100) = null
	,@ToMiddleName varchar(100) = null
	,@ToItem char(10)
	,@ToLSID char(10)
	,@Company varchar(50) = null
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[MatchMerge]
	SET [Action] = @Action
		,[FromLastName] = @FromLastName
		,[FromFirstName] = @FromFirstName
		,[FromMiddleName] = @FromMiddleName
		,[FromItem] = @FromItem
		,[FromLSID] = @FromLSID
		,[ToLastName] = @ToLastName
		,[ToFirstName] = @ToFirstName
		,[ToMiddleName] = @ToMiddleName
		,[ToItem] = @ToItem
		,[ToLSID] = @ToLSID
		,[Company] = @Company
		,[UpdateUserName] = @UpdateUserName
		,[UpdateDate] = GETDATE()
		,[VersionCount] = [VersionCount] + 1
	WHERE [MatchMergeID] = @MatchMergeID
END

GO
/****** Object:  StoredProcedure [dbo].[ItemOrderDump]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[ItemOrderDump]
	 @caseNumber varchar(25) = null
AS
BEGIN
SET NOCOUNT ON;

	IF (@caseNumber is null)
		return;

	declare @cpid int
	declare @count int

		set @count = (select count(*) from Customer where Item = @caseNumber);
		if (@count > 0)
			select 'Employee' [Employee], sourceid, datasource, * from Customer where Item = @caseNumber order by updatedate desc;

		set @count = (select count(*) from caseinformation where Customerid in (select Customerid from Customer where Item = @caseNumber));
		if (@count > 0)
			select 'Order' [Order], sourceid, sourceCompany ,* from caseinformation where Customerid in (select Customerid from Customer where Item = @caseNumber) order by updatedate desc;
		
		set @count = (select count(*) from Customername where Customerid in (select Customerid from Customer where Item = @caseNumber) and AliasIndicator != 'Y');
		if (@count > 0)
			select 'Name' [Name],sourceid, datasource,* from Customername where Customerid in (select Customerid from Customer where Item = @caseNumber) and AliasIndicator != 'Y' order by updatedate desc;
		
		set @count = (select count(*) from Customerparent where Customernameid in (select Customernameid from Customername where Customerid in (select Customerid from Customer where Item = @caseNumber)));
		if (@count > 0)
			select 'Parent' [Parent], sourceid, datasource,* from Customerparent where Customernameid in (select Customernameid from Customername where Customerid in (select Customerid from Customer where Item = @caseNumber)) order by updatedate desc;

		set @count = (select count(*) from Customername where Customerid in (select Customerid from Customer where Item = @caseNumber) and AliasIndicator = 'Y');
		if (@count > 0)
			select 'Alias' [Alias],sourceid, datasource,* from Customername where Customerid in (select Customerid from Customer where Item = @caseNumber) and AliasIndicator = 'Y' order by updatedate desc;

		set @count = (select count(*) from Customeraddress where Customernameid in (select Customernameid from Customername where Customerid in (select Customerid from Customer where Item = @caseNumber)));
		if (@count > 0)
			select 'Adr' [Adr],sourceid, datasource, Customeraddressid, Customernameid, Employeeaddressid, addresstype, addressline1, city, state, postalcode, addressline2, addressline3, * from Customeraddress   
			where Customernameid in (select Customernameid from Customername where Customerid in (select Customerid from Customer where Item = @caseNumber)) order by updatedate desc;

		set @count = (select count(*) from Customerattribute where Customernameid in (select Customernameid from Customername where Customerid in (select Customerid from Customer where Item = @caseNumber)));
		if (@count > 0)
			select 'Att' [Att],sourceid, datasource,* from Customerattribute 
			where Customernameid in (select Customernameid from Customername where Customerid in (select Customerid from Customer where Item = @caseNumber));

		set @count = (select count(*) from Customerdescription where Customernameid in (select Customernameid from Customername where Customerid in (select Customerid from Customer where Item = @caseNumber)));
		if (@count > 0)
			select 'Desc' [Desc],sourceid, datasource,* from Customerdescription  
			where Customernameid in (select Customernameid from Customername where Customerid in (select Customerid from Customer where Item = @caseNumber)) order by updatedate desc;

		set @count = (select count(*) from Customerphone where Customernameid in (select Customernameid from Customername where Customerid in (select Customerid from Customer where Item = @caseNumber)));
		if (@count > 0)
			select 'Phn' [Phn],sourceid, datasource,* from Customerphone   
			where Customernameid in (select Customernameid from Customername where Customerid in (select Customerid from Customer where Item = @caseNumber)) order by updatedate desc;
END

GO
/****** Object:  StoredProcedure [dbo].[ItemMergeLog_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[ItemMergeLog_CREATE]
	@FromItem char(10)
	,@ToItem char(10)
	,@CustomerID int = null
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Item_Merge_Log]
           ([FromItem]
           ,[ToItem]
           ,[CustomerID]
           ,[UpdateUserName]
           ,[UpdateDate]
           ,[VersionCount])
     VALUES
           (@FromItem
           ,@ToItem
           ,@CustomerID
           ,@UpdateUserName
           ,GETDATE()
           ,0)
	
	IF @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	ELSE 
		select 0
END


GO
/****** Object:  StoredProcedure [dbo].[ItemMergeLog_GetByToItem]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[ItemMergeLog_GetByToItem]
	  @ToItem char(10)
AS
BEGIN
	SET NOCOUNT ON;

   if (@ToItem is NULL)
		return;

	SELECT 
	[Item_MergeID]
      ,[FromItem]
      ,[ToItem]
      ,[CustomerID]
      ,[UpdateUserName]
      ,[UpdateDate]
      ,[VersionCount]
  FROM [dbo].[Item_Merge_Log]
	WHERE [ToItem] = @ToItem
       ORDER BY FromItem,CustomerID
END


GO
/****** Object:  StoredProcedure [dbo].[ItemMergeLog_GetByToItemandCustomerID]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ItemMergeLog_GetByToItemandCustomerID]
	@toItem char(10) = null
    ,@cpID int = null
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Item_MergeID]
      ,[FromItem]
      ,[ToItem]
      ,[CustomerID]
      ,[UpdateUserName]
      ,[UpdateDate]
      ,[VersionCount]
	FROM [Item_Merge_Log]
	where (@toItem is null or ([ToItem] = @toItem))
		and (@cpID is null or ([CustomerID] = @cpID))
END


GO
/****** Object:  StoredProcedure [dbo].[ItemMergeLog_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- script for update to stored procedure:
CREATE PROCEDURE [dbo].[ItemMergeLog_READ]
	@itemMergeID int = null
	,@fromItem char(10) = null
	,@toItem char(10) = null
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Item_MergeID]
      ,[FromItem]
      ,[ToItem]
      ,[CustomerID]
      ,[UpdateUserName]
      ,[UpdateDate]
      ,[VersionCount]
	FROM [Item_Merge_Log]
	where (@itemMergeID is null or ([Item_MergeID] = @itemMergeID))
		and (@fromItem is null or ([FromItem] = @fromItem))
		and (@toItem is null or [ToItem] = @toItem)
	order by updatedate
END

GO
/****** Object:  StoredProcedure [dbo].[ItemMergeLog_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ItemMergeLog_UPDATE]
	@ItemMergeID int
	,@FromItem char(10)
	,@ToItem char(10)
	,@CustomerID int = null
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Item_Merge_Log]
	SET [FromItem] = @FromItem
		,[ToItem] = @ToItem
		,[CustomerID] = @CustomerID
		,[UpdateUserName] = @UpdateUserName
		,[UpdateDate] = GETDATE()
		,[VersionCount] = [VersionCount] + 1
	WHERE [Item_MergeID] = @ItemMergeID
END



GO
/****** Object:  StoredProcedure [dbo].[ItemSource_ALL]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ItemSource_ALL]
	  
AS
BEGIN
	SET NOCOUNT ON;

	
	SELECT [Item]
	  ,[DataSource]
	  ,[ExchangeType]
      ,[LockExchangeId]
      ,[LockExchangeName]  
      ,[LastMaintenanceTask]  
	  ,[UpdateUserName]
	  ,[UpdateDate]
	  ,[VersionCount]
	FROM [ItemSource]
	
END




GO
/****** Object:  StoredProcedure [dbo].[ItemSource_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ItemSource_CREATE]
	  @Item char(10)
      ,@DataSource char(10) = null
      ,@ExchangeType char(10) = null
      ,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	
	if (@Item is null) set @Item = '' else set @Item = ltrim(rtrim(@Item));
	
	if (@Item = '')
	begin
		declare @NextKeyID int
		EXEC [NextKeyGenerator_GetNextKey]
			@KeyName = 'Item',
			@NextKeyID = @NextKeyID OUTPUT
		set @Item = 'D' + (select right(replicate('0', 9) + cast(@NextKeyID as varchar(9)), 9))
--print @Item
	end

	INSERT INTO [ItemSource]
           ([Item]
           ,[DataSource]
           ,[ExchangeType]
		   ,[LockExchangeID]
           ,[LockExchangeName]	
           ,[LastMaintenanceTask]
           ,[UpdateUserName]
           ,[UpdateDate]
           ,[VersionCount])
     VALUES
           (@Item
           ,@DataSource
           ,@ExchangeType
		   ,0
           ,''
           ,''
           ,@UpdateUserName
           ,GETDATE()
           ,0)
           
SELECT @Item

END
GO
/****** Object:  StoredProcedure [dbo].[ItemSource_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ItemSource_DELETE]
	  @Item char(10)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [ItemSource]
	WHERE [Item] = @Item
END

GO
/****** Object:  StoredProcedure [dbo].[ItemSource_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ItemSource_READ]
	  @Item char(10)
AS
BEGIN
	SET NOCOUNT ON;

	if (@Item is null)
		return;

	SELECT [Item]
	  ,[DataSource]
	  ,[ExchangeType]
	  ,[LockExchangeID]
      ,[LockExchangeName]	
      ,[LastMaintenanceTask]
	  ,[UpdateUserName]
	  ,[UpdateDate]
	  ,[VersionCount]
	FROM [ItemSource]
	WHERE [Item] = @Item
END



GO
/****** Object:  StoredProcedure [dbo].[ItemSource_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ItemSource_UPDATE]
	  @Item char(10)
      ,@DataSource char(10) = null
      ,@ExchangeType char(10) = null
      ,@LockExchangeID int = 0
      ,@LockExchangeName varchar(50) = null
      ,@LastMaintenanceTask varchar(50) = null      
      ,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [ItemSource]
	SET    [DataSource] = @DataSource
           ,[ExchangeType] = @ExchangeType
		   ,[LockExchangeID] = @LockExchangeID
           ,[LockExchangeName]	= @LockExchangeName
           ,[LastMaintenanceTask] = @LastMaintenanceTask
           ,[UpdateUserName] = @UpdateUserName
           ,[UpdateDate] = GETDATE()
           ,[VersionCount] = [VersionCount] + 1
	WHERE  [Item] = @Item
END


GO
/****** Object:  StoredProcedure [dbo].[NextKeyGenerator_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[NextKeyGenerator_CREATE]
	@KeyName varchar(20)
	,@KeyValue int
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [NextKeyGenerator]
           ([KeyName]
           ,[KeyValue]
           ,[UpdateUserName]
           ,[UpdateDate]
           ,[VersionCount])
     VALUES
           (@KeyName
           ,@KeyValue
           ,@UpdateUserName
           ,GETDATE()
           ,0)
	
	IF @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	ELSE 
		select 0
END



GO
/****** Object:  StoredProcedure [dbo].[NextKeyGenerator_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[NextKeyGenerator_DELETE]
	  @NextKeyID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [NextKeyGenerator]
	WHERE [NextKeyID] = @NextKeyID
END

GO
/****** Object:  StoredProcedure [dbo].[NextKeyGenerator_GetNextKey]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[NextKeyGenerator_GetNextKey] (

   @KeyName varchar(30),
   @NextKeyID int output     

)
as
begin

 -- **************************************************/
  -- Increment and return next key id to
  -- use for the key value in @KEY_NM.  
  --
  -- This procedure should be used only
  -- from within other stored procedures.
  -- The next key id is returned in the
  -- @NextKeyID output parameter. 
  --
  -- Callers should either handle the
  -- exceptions raised by this s.p. or
  -- check @@ERROR after the call. 
  -- *************************************************/

  set nocount on
  declare @SQLERROR int,
     @SQLROWCOUNT int

  update dbo.NextKeyGenerator
  set @NextKeyID = KeyValue = KeyValue + 1
  where KeyName = @KeyName

  select @SQLERROR = @@ERROR, @SQLROWCOUNT = @@ROWCOUNT
  if (@SQLROWCOUNT = 0) or (@SQLERROR != 0)
  begin
    select @NextKeyID = 0
    raiserror ('Next key update failed. Error selecting row for key', 16, 1 ) 
    return
  end

end



GO
/****** Object:  StoredProcedure [dbo].[NextKeyGenerator_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[NextKeyGenerator_READ]
	  @NextKeyID int = null
	  ,@KeyName varchar(20) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@NextKeyID > 0)
	begin
		SELECT [NextKeyID]
		  ,[KeyName]
		  ,[KeyValue]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[VersionCount]
		FROM [NextKeyGenerator]
		WHERE [NextKeyID] = ISNULL(@NextKeyID, NextKeyID)
	end
	else  -- search by KeyName field
	begin
		SELECT [NextKeyID]
		  ,[KeyName]
		  ,[KeyValue]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[VersionCount]
		FROM [NextKeyGenerator]
		WHERE [KeyName] = ISNULL(@KeyName, KeyName)
	end
END
GO
/****** Object:  StoredProcedure [dbo].[NextKeyGenerator_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[NextKeyGenerator_UPDATE]
	@NextKeyID int
	,@KeyName varchar(20)
	,@KeyValue int
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [NextKeyGenerator]
	SET    [KeyName] = @KeyName
           ,[KeyValue] = @KeyValue
           ,[UpdateUserName] = @UpdateUserName
           ,[UpdateDate] = GETDATE()
           ,[VersionCount] = [VersionCount] + 1
	WHERE  [NextKeyID] = @NextKeyID
END


GO
/****** Object:  StoredProcedure [dbo].[Company_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Company_CREATE]
	@Company varchar(255) = null
	,@BodyID varchar(255) = null
	,@LastName varchar(255) = null
	,@FirstName varchar(255) = null
	,@MiddleName varchar(255) = null
	,@ADUserName varchar(255) = null
	,@ADDomain varchar(255) = null
	,@UpdateUserName varchar(255)
AS
BEGIN
	SET NOCOUNT ON;

	if (@UpdateUserName is NULL)
		return;
		
	if (@Company is NULL and @BodyID is NULL and @LastName is NULL and @FirstName is NULL and @ADUserName is NULL and @ADDomain is NULL)
		return;
		
	INSERT INTO [Company]
           ([Company]
           ,[BodyID]
           ,[LastName]
           ,[FirstName]
           ,[MiddleName]
           ,[ADUserName]
           ,[ADDomain]
           ,[UpdateUserName]
           ,[UpdateDate]
           ,[VersionCount])
     VALUES
           (@Company
           ,@BodyID
           ,@LastName
           ,@FirstName
           ,@MiddleName
           ,@ADUserName
           ,@ADDomain
           ,@UpdateUserName
           ,GETDATE()
           ,0)

	IF @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	ELSE 
		select 0
END

GO
/****** Object:  StoredProcedure [dbo].[Company_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Company_DELETE]
	@CompanyID int
AS
BEGIN
	SET NOCOUNT ON;

	if (@CompanyID is NULL)
		return;
		
	DELETE FROM [dbo].[Company]
	WHERE CompanyID = @CompanyID

END

GO
/****** Object:  StoredProcedure [dbo].[Company_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Company_READ]
	@CompanyID int = null
	,@Company varchar(255) = null
	,@BodyID varchar(255) = null
	,@ADUserName varchar(255) = null
	,@ADDomain varchar(255) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@CompanyID is NULL and @Company is NULL and @BodyID is NULL and @ADUserName is NULL and @ADDomain is NULL)
	begin
		SELECT [CompanyID]
		  ,[Company]
		  ,[BodyID]
		  ,[LastName]
		  ,[FirstName]
		  ,[MiddleName]
		  ,[ADUserName]
		  ,[ADDomain]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[VersionCount]
		FROM [dbo].[Company]
	end
	else if (@CompanyID is not null)
	begin
		SELECT [CompanyID]
		  ,[Company]
		  ,[BodyID]
		  ,[LastName]
		  ,[FirstName]
		  ,[MiddleName]
		  ,[ADUserName]
		  ,[ADDomain]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[VersionCount]
		FROM [dbo].[Company]
		WHERE [CompanyID] = @CompanyID
	end
	else if (@Company is NOT NULL and @BodyID is NULL)
	begin
		SELECT [CompanyID]
		  ,[Company]
		  ,[BodyID]
		  ,[LastName]
		  ,[FirstName]
		  ,[MiddleName]
		  ,[ADUserName]
		  ,[ADDomain]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[VersionCount]
		FROM [dbo].[Company]
		WHERE [Company] = @Company
	end
	else if (@Company is NOT NULL and @BodyID is NOT NULL)
	begin
		SELECT [CompanyID]
		  ,[Company]
		  ,[BodyID]
		  ,[LastName]
		  ,[FirstName]
		  ,[MiddleName]
		  ,[ADUserName]
		  ,[ADDomain]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[VersionCount]
		FROM [dbo].[Company]
		WHERE [Company] = @Company
		  AND [BodyID] = @BodyID
	end
	else if (@ADUserName is NOT NULL and @ADDomain is NOT NULL)
	begin
		SELECT [CompanyID]
		  ,[Company]
		  ,[BodyID]
		  ,[LastName]
		  ,[FirstName]
		  ,[MiddleName]
		  ,[ADUserName]
		  ,[ADDomain]
		  ,[UpdateUserName]
		  ,[UpdateDate]
		  ,[VersionCount]
		FROM [dbo].[Company]
		WHERE [ADUserName] = @ADUserName
		  AND [ADDomain] = @ADDomain
	end
end

GO
/****** Object:  StoredProcedure [dbo].[Company_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Company_UPDATE]
	@CompanyID int
	,@Company varchar(255) = null
	,@BodyID varchar(255) = null
	,@LastName varchar(255) = null
	,@FirstName varchar(255) = null
	,@MiddleName varchar(255) = null
	,@ADUserName varchar(255) = null
	,@ADDomain varchar(255) = null
	,@UpdateUserName varchar(255)
AS
BEGIN
	SET NOCOUNT ON;

	if (@CompanyID is NULL or @UpdateUserName is NULL)
		return;
		
	if (@Company is NULL and @BodyID is NULL and @LastName is NULL and @FirstName is NULL and @ADUserName is NULL and @ADDomain is NULL)
		return;
		
	UPDATE [dbo].[Company]
		SET [Company] = @Company
			,[BodyID] = @BodyID
			,[LastName] = @LastName
			,[FirstName] = @FirstName
			,[MiddleName] = @MiddleName
			,[ADUserName] = @ADUserName
			,[ADDomain] = @ADDomain
			,[UpdateUserName] = @UpdateUserName
			,[UpdateDate] = GETDATE()
			,[VersionCount] = [VersionCount] + 1
	WHERE CompanyID = @CompanyID

END

GO
/****** Object:  StoredProcedure [dbo].[EmployeeAddress_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[EmployeeAddress_CREATE]
	@EmployeeNameID char(10)
	,@AddressType varchar(10) = null
	,@AddressLine1 varchar(50) = null
	,@AddressLine2 varchar(50) = null
	,@AddressLine3 varchar(50) = null
	,@StreetType varchar(4) = null
	,@HouseNumber varchar(5) = null
	,@HalfAddress varchar(5) = null
	,@PrefixDirection varchar(2) = null
	,@StreetName varchar(60) = null
	,@PostDirection varchar(2) = null
	,@Apartment varchar(10) = null
	,@City varchar(20) = null
	,@State varchar(2) = null
	,@PostalCode varchar(9) = null
	,@CountryCode varchar(10) = null
	,@SourceID varchar(40) = null
	,@DataSource varchar(10) = null
	,@ContributingCompany varchar(50) = null
	,@DateModified smalldatetime = null
	,@IDSourceCode varchar(50) = null
	,@UpdateUserName varchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [EmployeeAddress]
		([EmployeeNameID]
		,[AddressType]
		,[AddressLine1]
		,[AddressLine2]
		,[AddressLine3]
		,[StreetType]
		,[HouseNumber]
		,[HalfAddress]
		,[PrefixDirection]
		,[StreetName]
		,[PostDirection]
		,[Apartment]
		,[City]
		,[State]
		,[PostalCode]
		,[CountryCode]
		,[SourceID]
		,[DataSource]
		,[ContributingCompany]
		,[DateModified]
		,[IDSourceCode]
		,[UpdateUserName]
		,[UpdateDate]
		,[VersionCount])
     VALUES
		(@EmployeeNameID
		,@AddressType
		,@AddressLine1
		,@AddressLine2
		,@AddressLine3
		,@StreetType
		,@HouseNumber
		,@HalfAddress
		,@PrefixDirection
		,@StreetName
		,@PostDirection
		,@Apartment
		,@City
		,@State
		,@PostalCode
		,@CountryCode
	    ,@SourceID
		,@DataSource 
		,@ContributingCompany
		,@DateModified
		,@IDSourceCode
		,@UpdateUserName
		,GETDATE()
		,0)
	
	IF @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	ELSE 
		select 0
 END






GO
/****** Object:  StoredProcedure [dbo].[EmployeeAddress_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EmployeeAddress_DELETE]
	  @EmployeeAddressID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [EmployeeAddress]
	WHERE [EmployeeAddressID] = @EmployeeAddressID
END

GO
/****** Object:  StoredProcedure [dbo].[EmployeeAddress_GETBYItem]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EmployeeAddress_GETBYItem]
	@Item char(10)
	,@userName varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@Item is NULL and @Item = '')
		return

	if (@username is not null)
		exec audit_LogSelect_CREATE @storedProcedureName='EmployeeAddress_GETBYItem', @parameterName='Item', @parameterValue=@Item, @userName=@userName;

	SELECT [EmployeeAddressID]
      ,[EmployeeNameID]
      ,[AddressType]
      ,[AddressLine1]
      ,[AddressLine2]
      ,[AddressLine3]
      ,[StreetType]
      ,[HouseNumber]
      ,[HalfAddress]
      ,[PrefixDirection]
      ,[StreetName]
      ,[PostDirection]
      ,[Apartment]
      ,[City]
      ,[State]
      ,[PostalCode]
      ,[CountryCode]
	  ,[SourceID]
	  ,[DataSource]
	  ,[DateModified]
      ,[VersionCount]
      ,[ContributingCompany]
      ,[IDSourceCode]
	FROM [EmployeeAddress]
	WHERE [EmployeeNameID] in
		( select [EmployeeNameID] from EmployeeNAME where Item=@Item	) 
    order by [AddressType], [DateModified] desc
END















GO
/****** Object:  StoredProcedure [dbo].[EmployeeAddress_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EmployeeAddress_READ]
	@EmployeeAddressID int
	,@EmployeeNameID int
	,@userName varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@EmployeeAddressID is NULL and @EmployeeNameID is NULL)
		return;

   	if (@username is not null)
	begin
		exec audit_LogSelect_CREATE @storedProcedureName='EmployeeAddress_READ', @parameterName='EmployeeAddressID', @parameterValue=@EmployeeAddressID, @userName=@userName;
		exec audit_LogSelect_CREATE @storedProcedureName='EmployeeAddress_READ', @parameterName='EmployeeNameID', @parameterValue=@EmployeeNameID, @userName=@userName;
	end

	if (@EmployeeAddressID is not NULL)
		SELECT [EmployeeAddressID]
		  ,[EmployeeNameID]
		  ,[AddressType]
		  ,[AddressLine1]
		  ,[AddressLine2]
		  ,[AddressLine3]
		  ,[StreetType]
		  ,[HouseNumber]
		  ,[HalfAddress]
		  ,[PrefixDirection]
		  ,[StreetName]
		  ,[PostDirection]
		  ,[Apartment]
		  ,[City]
		  ,[State]
		  ,[PostalCode]
		  ,[CountryCode]
		  ,[SourceID]
		  ,[DataSource]
		  ,[DateModified]
		  ,[VersionCount]
		  ,[ContributingCompany]
		  ,[IDSourceCode]
		FROM [EmployeeAddress]
		WHERE [EmployeeAddressID] = @EmployeeAddressID
		order by [AddressType], [DateModified] desc
	ELSE
		SELECT [EmployeeAddressID]
		  ,[EmployeeNameID]
		  ,[AddressType]
		  ,[AddressLine1]
		  ,[AddressLine2]
		  ,[AddressLine3]
		  ,[StreetType]
		  ,[HouseNumber]
		  ,[HalfAddress]
		  ,[PrefixDirection]
		  ,[StreetName]
		  ,[PostDirection]
		  ,[Apartment]
		  ,[City]
		  ,[State]
		  ,[PostalCode]
		  ,[CountryCode]
		  ,[SourceID]
		  ,[DataSource]
		  ,[DateModified]
		  ,[VersionCount]
		  ,[ContributingCompany]
		  ,[IDSourceCode]
		FROM [EmployeeAddress]
		WHERE [EmployeeNameID] = @EmployeeNameID
		order by [AddressType], [DateModified] desc
END
GO
/****** Object:  StoredProcedure [dbo].[EmployeeAddress_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EmployeeAddress_UPDATE]
	@EmployeeAddressID int
	,@EmployeeNameID int
	,@AddressType varchar(10) = null
	,@AddressLine1 varchar(50) = null
	,@AddressLine2 varchar(50) = null
	,@AddressLine3 varchar(50) = null
	,@StreetType varchar(4) = null
	,@HouseNumber varchar(5) = null
	,@HalfAddress varchar(5) = null
	,@PrefixDirection varchar(2) = null
	,@StreetName varchar(60) = null
	,@PostDirection varchar(2) = null
	,@Apartment varchar(10) = null
	,@City varchar(20) = null
	,@State varchar(2) = null
	,@PostalCode varchar(9) = null
	,@CountryCode varchar(10) = null
	,@SourceID varchar(40) = null
	,@DataSource varchar(10) = null
	,@DateModified smalldatetime = null
	,@UpdateUserName varchar(50)
	,@ContributingCompany varchar(50) = null
	,@IDSourceCode varchar(50) = null

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE  [EmployeeAddress]
	SET     [EmployeeNameID] = @EmployeeNameID
			,[AddressType] = @AddressType
			,[AddressLine1] = @AddressLine1
			,[AddressLine2] = @AddressLine2
			,[AddressLine3] = @AddressLine3
			,[StreetType] = @StreetType
			,[HouseNumber] = @HouseNumber
			,[HalfAddress] = @HalfAddress
			,[PrefixDirection] = @PrefixDirection
			,[StreetName] = @StreetName
			,[PostDirection] = @PostDirection
			,[Apartment] = @Apartment
			,[City] = @City
			,[State] = @State
			,[PostalCode] = @PostalCode
			,[CountryCode] = @CountryCode
		    ,[SourceID] = @SourceID
			,[DataSource]= @DataSource
		    ,[ContributingCompany] = @ContributingCompany
		    ,[DateModified] = @DateModified
		    ,[IDSourceCode] = @IDSourceCode
			,[UpdateUserName] = @UpdateUserName
			,[UpdateDate] = GETDATE()
			,[VersionCount] = [VersionCount] + 1
	WHERE   [EmployeeAddressID] = @EmployeeAddressID
END







GO
/****** Object:  StoredProcedure [dbo].[EmployeeAttribute_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EmployeeAttribute_CREATE]
	@EmployeeNameID int
	,@AssignedIDType varchar(10)
	,@AssignedIDNumber varchar(50)
	,@AssignedIDState varchar(10) = null
	,@PrimaryID bit
	,@ContributingCompany varchar(50) = null
	,@DateModified smallDateTime = null
	,@IDSourceCode varchar(50) = null
	,@SourceID varchar(255) = null
	,@DataSource varchar(10) = null
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [EmployeeAttribute]
		([EmployeeNameID]
		,[AssignedIDType]
		,[AssignedIDNumber]
		,[AssignedIDState]
		,[PrimaryID]
		,[ContributingCompany]
		,[DateModified]
		,[IDSourceCode]
		,[SourceID]
		,[DataSource]
		,[UpdateUserName]
		,[UpdateDate]
		,[VersionCount])
     VALUES
		(@EmployeeNameID
		,@AssignedIDType
		,@AssignedIDNumber
		,@AssignedIDState
		,@PrimaryID
		,@ContributingCompany
		,@DateModified
		,@IDSourceCode
		,@SourceID
		,@DataSource
		,@UpdateUserName
		,GETDATE()
		,0)
	
	IF @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	ELSE 
		select 0
END







GO
/****** Object:  StoredProcedure [dbo].[EmployeeAttribute_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EmployeeAttribute_DELETE]
	  @EmployeeAttributeID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [EmployeeAttribute]
	WHERE [EmployeeAttributeID] = @EmployeeAttributeID
END


GO
/****** Object:  StoredProcedure [dbo].[EmployeeAttribute_GETBYItem]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EmployeeAttribute_GETBYItem]
	@Item char(10)
	,@userName varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@Item is NULL and @Item = '')
		return;

   	if (@username is not null)
		exec audit_LogSelect_CREATE @storedProcedureName='EmployeeAttribute_GETBYItem', @parameterName='Item', @parameterValue=@Item, @userName=@userName;

	SELECT [EmployeeAttributeID]
      ,[EmployeeNameID]
      ,[AssignedIDType]
      ,[AssignedIDNumber]
      ,[AssignedIDState]
	  ,[SourceID]
      ,[PrimaryID]	  
	  ,[DataSource]
      ,[VersionCount]
	  ,[ContributingCompany]
	  ,[DateModified]
	  ,[IDSourceCode]
	FROM [EmployeeAttribute]
	WHERE [EmployeeNameID] in
		( select [EmployeeNameID] from EmployeeNAME where Item=@Item	)
--	order by [AssignedIDType],[DateModified] desc
	order by [AssignedIDType],EmployeeAttributeID
END








GO
/****** Object:  StoredProcedure [dbo].[EmployeeAttribute_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EmployeeAttribute_READ]
	@EmployeeAttributeID int
	,@EmployeeNameID int
	,@AssignedIDNumber varchar(50) = null
	,@AssignedIDType varchar(10) = null
	,@userName varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;

	if ((@EmployeeAttributeID is NULL and @EmployeeNameID is NULL) and (@AssignedIDNumber is NULL or @AssignedIDNumber = ''))
		return;

   	if (@username is not null)
	begin
		if (@EmployeeAttributeID is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeAttribute_READ', @parameterName='EmployeeAttributeID', @parameterValue=@EmployeeAttributeID, @userName=@userName;
		if (@EmployeeNameID is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeAttribute_READ', @parameterName='EmployeeNameID', @parameterValue=@EmployeeNameID, @userName=@userName;
		if (@AssignedIDNumber is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeAttribute_GETBYASSIGNEDID', @parameterName='assignedIDNumber', @parameterValue=@AssignedIDNumber, @userName=@userName;
		if (@AssignedIDType is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeAttribute_GETBYASSIGNEDID', @parameterName='assignedIDType', @parameterValue=@AssignedIDType, @userName=@userName;
	end

   if (@EmployeeAttributeID is not NULL)
		SELECT [EmployeeAttributeID]
		  ,[EmployeeNameID]
		  ,[AssignedIDType]
		  ,[AssignedIDNumber]
		  ,[AssignedIDState]
		  ,[SourceID]
		  ,[PrimaryID]		  
		  ,[DataSource]
		  ,[VersionCount]
		  ,[ContributingCompany]
		  ,[DateModified]
		  ,[IDSourceCode]
		FROM [EmployeeAttribute]
		WHERE [EmployeeAttributeID] = @EmployeeAttributeID
		order by [AssignedIDType],[DateModified] desc
   else if (@AssignedIDNumber is not null and @AssignedIDNumber <> '')
		SELECT [EmployeeAttributeID]
		  ,[EmployeeNameID]
		  ,[AssignedIDType]
		  ,[AssignedIDNumber]
		  ,[AssignedIDState]
		  ,[SourceID]
		  ,[PrimaryID]	  
		  ,[DataSource]
		  ,[VersionCount]
		  ,[ContributingCompany]
		  ,[DateModified]
		  ,[IDSourceCode]
		FROM [EmployeeAttribute]
		WHERE [AssignedIDNumber] = @AssignedIDNumber
		AND [AssignedIDType] = @AssignedIDType
	else
		SELECT [EmployeeAttributeID]
		  ,[EmployeeNameID]
		  ,[AssignedIDType]
		  ,[AssignedIDNumber]
		  ,[AssignedIDState]
		  ,[SourceID]
		  ,[PrimaryID]		  
		  ,[DataSource]
		  ,[VersionCount]
		  ,[ContributingCompany]
		  ,[DateModified]
		  ,[IDSourceCode]
		FROM [EmployeeAttribute]
		WHERE [EmployeeNameID] = @EmployeeNameID
		order by [AssignedIDType],[DateModified] desc
END
GO
/****** Object:  StoredProcedure [dbo].[EmployeeAttribute_ResetPrimaryFCNattributes]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EmployeeAttribute_ResetPrimaryFCNattributes]
	@Item char(10)
AS
BEGIN
	SET NOCOUNT ON;

	Update EmployeeAttribute set PrimaryID = 0 where EmployeeAttributeID in (Select [EmployeeAttributeID] FROM [EmployeeAttribute]
		WHERE ([EmployeeNameID] in (select [EmployeeNameID] from EmployeeNAME where Item=@Item)) and [AssignedIDType] = 'SRF-FCN')
END

GO
/****** Object:  StoredProcedure [dbo].[EmployeeAttribute_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[EmployeeAttribute_UPDATE]
	@EmployeeAttributeID int
	,@EmployeeNameID int
	,@AssignedIDType varchar(10)
	,@AssignedIDNumber varchar(50)
	,@AssignedIDState varchar(10) = null
	,@PrimaryID bit
	,@ContributingCompany varchar(50) = null
	,@DateModified smallDateTime = null
	,@IDSourceCode varchar(50) = null
	,@SourceID varchar(255) = null
	,@DataSource varchar(10) = null
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE  [EmployeeAttribute]
	SET     [EmployeeNameID] = @EmployeeNameID
			,[AssignedIDType] = @AssignedIDType
			,[AssignedIDNumber] = @AssignedIDNumber
			,[AssignedIDState] = @AssignedIDState
			,[PrimaryID] = @PrimaryID
		    ,[ContributingCompany] = @ContributingCompany
		    ,[DateModified] = @DateModified
		    ,[IDSourceCode] = @IDSourceCode
		    ,[SourceID] = @SourceID
			,[DataSource] = @DataSource
			,[UpdateUserName] = @UpdateUserName
			,[UpdateDate] = GETDATE()
			,[VersionCount] = [VersionCount] + 1
	WHERE   [EmployeeAttributeID] = @EmployeeAttributeID
END






GO
/****** Object:  StoredProcedure [dbo].[EmployeeDescription_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[EmployeeDescription_CREATE]
	@EmployeeNameID int
	,@Sex char(1) = null
	,@Race varchar(10) = null
	,@Height char(3) = null
	,@Weight char(3) = null
	,@EyeColor varchar(10) = null
	,@BirthPlace varchar(40) = null
	,@HairColor varchar(10) = null
	,@Occupation varchar(50) = null
	,@ContributingCompany varchar(50) = null
	,@DateModified smallDateTime = null
	,@IDSourceCode varchar(50) = null
	,@SourceID varchar(255) = null
	,@DataSource varchar(10) = null
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [EmployeeDescription]
		([EmployeeNameID]
		,[Sex]
		,[Race]
		,[Height]
		,[Weight]
		,[EyeColor]
		,[BirthPlace]
		,[HairColor]
		,[Occupation]
		,[ContributingCompany]
		,[DateModified]
		,[IDSourceCode]
		,[SourceID]
		,[DataSource]
		,[UpdateUserName]
		,[UpdateDate]
		,[VersionCount])
     VALUES
		(@EmployeeNameID
		,@Sex
		,@Race
		,@Height
		,@Weight
		,@EyeColor
		,@BirthPlace
		,@HairColor
		,@Occupation
		,@ContributingCompany
		,@DateModified
		,@IDSourceCode
		,@SourceID
		,@DataSource
		,@UpdateUserName
		,GETDATE()
		,0)

	IF @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	ELSE 
		select 0
END







GO
/****** Object:  StoredProcedure [dbo].[EmployeeDescription_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EmployeeDescription_DELETE]
	  @EmployeeDescriptionID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [EmployeeDescription]
	WHERE [EmployeeDescriptionID] = @EmployeeDescriptionID
END


GO
/****** Object:  StoredProcedure [dbo].[EmployeeDescription_GETBYItem]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EmployeeDescription_GETBYItem]
	@Item char(10)
	,@userName varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@Item is NULL and @Item = '')
		return;

   	if (@username is not null)
		exec audit_LogSelect_CREATE @storedProcedureName='EmployeeDescription_GETBYItem', @parameterName='Item', @parameterValue=@Item, @userName=@userName;

	SELECT [EmployeeDescriptionID]
      ,[EmployeeNameID]
      ,[Sex]
      ,[Race]
      ,[Height]
      ,[Weight]
      ,[EyeColor]
      ,[BirthPlace]
      ,[HairColor]
      ,[Occupation]
	  ,[SourceID]
	  ,[DataSource]
      ,[VersionCount]
	  ,[ContributingCompany]
	  ,[DateModified]
	  ,[IDSourceCode]
	FROM [EmployeeDescription]
	WHERE [EmployeeNameID] in
		( select [EmployeeNameID] from EmployeeNAME where Item=@Item	)
	order by [DateModified] desc
END









GO
/****** Object:  StoredProcedure [dbo].[EmployeeDescription_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EmployeeDescription_READ]
	@EmployeeDescriptionID int
	,@EmployeeNameID int
	,@userName varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@EmployeeDescriptionID is NULL and @EmployeeNameID is NULL)
		return;

   	if (@username is not null)
	begin
		exec audit_LogSelect_CREATE @storedProcedureName='EmployeeDescription_READ', @parameterName='EmployeeDescriptionID', @parameterValue=@EmployeeDescriptionID, @userName=@userName;
		exec audit_LogSelect_CREATE @storedProcedureName='EmployeeDescription_READ', @parameterName='EmployeeNameID', @parameterValue=@EmployeeNameID, @userName=@userName;
	end

   if (@EmployeeDescriptionID is NOT NULL)
		SELECT [EmployeeDescriptionID]
		  ,[EmployeeNameID]
		  ,[Sex]
		  ,[Race]
		  ,[Height]
		  ,[Weight]
		  ,[EyeColor]
		  ,[BirthPlace]
		  ,[HairColor]
		  ,[Occupation]
		  ,[SourceID]
		  ,[DataSource]
		  ,[VersionCount]
		  ,[ContributingCompany]
		  ,[DateModified]
		  ,[IDSourceCode]
		FROM [EmployeeDescription]
		WHERE [EmployeeDescriptionID] = @EmployeeDescriptionID
		order by [DateModified] desc
	ELSE
		SELECT [EmployeeDescriptionID]
		  ,[EmployeeNameID]
		  ,[Sex]
		  ,[Race]
		  ,[Height]
		  ,[Weight]
		  ,[EyeColor]
		  ,[BirthPlace]
		  ,[HairColor]
		  ,[Occupation]
		  ,[SourceID]
		  ,[DataSource]
		  ,[VersionCount]
		  ,[ContributingCompany]
		  ,[DateModified]
		  ,[IDSourceCode]
		FROM [EmployeeDescription]
		WHERE [EmployeeNameID] = @EmployeeNameID
		order by [DateModified] desc
END









GO
/****** Object:  StoredProcedure [dbo].[EmployeeDescription_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[EmployeeDescription_UPDATE]
	@EmployeeDescriptionID int,
	@EmployeeNameID int,
	@Sex char(1) = null,
	@Race varchar(10) = null,
	@Height char(3) = null,
	@Weight char(3) = null,
	@EyeColor varchar(10) = null,
	@BirthPlace varchar(40) = null,
	@HairColor varchar(10) = null,
	@Occupation varchar(50) = null,
	@ContributingCompany varchar(50) = null,
	@DateModified smallDateTime = null,
	@IDSourceCode varchar(50) = null,
	@SourceID varchar(255) = null,
	@DataSource varchar(10)=null,
	@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

UPDATE [EmployeeDescription]
   SET [EmployeeNameID] = @EmployeeNameID
      ,[Sex] = @Sex
      ,[Race] = @Race
      ,[Height] = @Height
      ,[Weight] = @Weight
      ,[EyeColor] = @EyeColor
      ,[BirthPlace] = @BirthPlace
      ,[HairColor] = @HairColor
      ,[Occupation] = @Occupation
	  ,[ContributingCompany] = @ContributingCompany
	  ,[DateModified] = @DateModified
	  ,[IDSourceCode] = @IDSourceCode
	  ,[SourceID] = @SourceID
	  ,[DataSource] = @DataSource
	  ,[UpdateUserName] = @UpdateUserName
 WHERE [EmployeeDescriptionID] = @EmployeeDescriptionID
END



GO
/****** Object:  StoredProcedure [dbo].[EmployeeDump]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[EmployeeDump]
	 @item varchar(10) = null
	 ,@caseNumber varchar(10) = null
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @pid int
	
	if (@item is not null)
		SET @pid = (select top 1 Employeenameid from Employeename where Item=@item)
	else
	if (@caseNumber is not null)
		SET @pid = (select top 1 pn.Employeenameid from Employeename pn
			inner join CustomerName cpn on cpn.EmployeeNameID = pn.EmployeeNameID
			inner join Customer cp on cp.CustomerID = cpn.CustomerID
			inner join OrderInformation ci on ci.CustomerID = cp.CustomerID and ci.OrderNumber = @caseNumber);

	if (@pid <= 0)
		return;
	
	declare @count int
	
	set @count = (select count(*) from Employeename where Employeenameid=@pid);
	if (@count > 0)
		select 'Name' [Name], sourceid,* from Employeename where Employeenameid=@pid;

	set @count = (select count(*) from Employeename where Item=@item and Employeenameid!=@pid);
	if (@count > 0)
		select 'Alias' [Alias], sourceid, * from Employeename where Item=@item and Employeenameid!=@pid;
	
	set @count = (select count(*) from Employeeparent where Employeenameid = @pid);
	if (@count > 0)
		select 'Parent' [Parent], sourceid, * from Employeeparent where Employeenameid = @pid;

	set @count = (select count(*) from Employeeaddress where Employeenameid in (select Employeenameid from Employeename where Item=@item));
	if (@count > 0)
		select 'Adr' [Adr], sourceid, * from Employeeaddress 
		where Employeenameid in (select Employeenameid from Employeename where Item=@item);

	set @count = (select count(*) from Employeeattribute where Employeenameid in (select Employeenameid from Employeename where Item=@item));
	if (@count > 0)
		select 'Att' [Att], sourceid, * from Employeeattribute 
		where Employeenameid in (select Employeenameid from Employeename where Item=@item) order by assignedIDType, updatedate;

	set @count = (select count(*) from Employeedescription where Employeenameid in (select Employeenameid from Employeename where Item=@item));
	if (@count > 0)
		select 'Desc' [Desc], sourceid, * from Employeedescription 
		where Employeenameid in (select Employeenameid from Employeename where Item=@item);

	set @count = (select count(*) from Employeephone where Employeenameid in (select Employeenameid from Employeename where Item=@item));
	if (@count > 0)
		select 'Phn' [Phone], sourceid, * from Employeephone 
		where Employeenameid in (select Employeenameid from Employeename where Item=@item);
	
	set @count = (select COUNT(*) from OrderInformation c
		inner join Customer cp on cp.CustomerID = c.CustomerID and cp.Item = @item);
	if (@count > 0)
		select 'Order' [OrderInformation], c.SourceID, c.* from OrderInformation c 
			inner join Customer cp on cp.CustomerID = c.CustomerID and cp.Item = @item
			order by OrderNumber;

	set @count = (select COUNT(*) from OrderReferredOrder c
		inner join OrderInformation i on i.OrderInformationID = c.OrderInformationID
		inner join Customer cp on cp.CustomerID = i.CustomerID and cp.Item = @item);
	if (@count > 0)
		select 'Referral' [OrderReferredOrder], c.SourceID, c.* from OrderReferredOrder c 
			inner join OrderInformation i on i.OrderInformationID = c.OrderInformationID
			inner join Customer cp on cp.CustomerID = i.CustomerID and cp.Item = @item
			order by OrderNumber;
END

GO
/****** Object:  StoredProcedure [dbo].[EmployeeName_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EmployeeName_CREATE]
	@Item char(10)
	,@FirstName varchar(50) = null
	,@MiddleName varchar(50) = null
	,@LastName varchar(50)
	,@SuffixName varchar(10) = null
	,@NameTitle varchar(10) = null
	,@BusinessName varchar(100) = null
	,@DOB datetime = null
	,@DeathDate smalldatetime = null
	,@NameType varchar(10) = null
	,@PhoneticLastName varchar(10) = null
	,@AliasIndicator char(1) = null
	,@DataSource varchar(10)
	,@CreateUserName varchar(50)
	,@SourceID varchar(255) = null
	,@ValidationIndicator char(1) = 'N'
	,@ValidationComments varchar(100) = null
	,@DateModified smalldatetime = null
    ,@Note varchar(100) = null
	,@ContributingCompany varchar(50) = null
	,@IDSourceCode varchar(50) = null
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [EmployeeName]
		([Item]
		,[FirstName]
		,[MiddleName]
		,[LastName]
		,[SuffixName]
		,[NameTitle]
		,[BusinessName]
		,[DOB]
		,[DeathDate]
		,[NameType]
		,[PhoneticLastName]
		,[SoundexFirstName]
		,[SoundexLastName]
		,[AliasIndicator]
		,[DataSource]
		,[CreateDate]
		,[CreateUserName]
		,[SourceID]
	    ,[ValidationIndicator]
	    ,[ValidationComments]
		,[DateModified]
		,[Note]
		,[ContributingCompany]
		,[IDSourceCode]
		,[UpdateUserName]
		,[UpdateDate]
		,[VersionCount])
     VALUES
		(@Item
		,@FirstName
		,@MiddleName
		,@LastName
		,@SuffixName
		,@NameTitle
		,@BusinessName
		,@DOB
		,@DeathDate
		,@NameType
		,@PhoneticLastName
		,SOUNDEX(@FirstName)
		,SOUNDEX(@LastName)
		,@AliasIndicator
		,@DataSource
		,GETDATE()
		,@CreateUserName
	    ,@SourceID
	    ,@ValidationIndicator
	    ,@ValidationComments
		,@DateModified
		,@Note
		,@ContributingCompany
		,@IDSourceCode
		,@UpdateUserName
		,GETDATE()
		,0)

	IF @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	ELSE 
		select 0
END

GO
/****** Object:  StoredProcedure [dbo].[EmployeeName_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EmployeeName_DELETE]
	  @EmployeeNameID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[EmployeeName]
	WHERE [EmployeeNameID] = @EmployeeNameID
END


GO
/****** Object:  StoredProcedure [dbo].[EmployeeName_Item]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EmployeeName_Item]
	@Item char(10)
	,@nameType varchar(10) = null
	,@userName varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;

	if ( @Item is NULL)
		return;

   	if (@username is not null)
		exec audit_LogSelect_CREATE @storedProcedureName='EmployeeName_Item', @parameterName='Item', @parameterValue=@Item, @userName=@userName;

	declare @sql varchar(max)
	declare @nameTypeCondition varchar(max)

	-- default for no name type defined
	set @nameTypeCondition = ' and nameType in (''adult'',''juvenile'') '
	if (@nameType is not null)
		set @nameTypeCondition = ' and nameType = ''' + @nameType + ''''
--print @nameTypeCondition
	
		set @sql = 'SELECT [EmployeeNameID]
		  ,[Item]
		  ,[FirstName]
		  ,[MiddleName]
		  ,[LastName]
		  ,[SuffixName]
		  ,[NameTitle]
		  ,[BusinessName]
		  ,[DOB]
		  ,[DeathDate]
		  ,[NameType]
		  ,[PhoneticLastName]
		  ,[AliasIndicator]
		  ,[DataSource]
		  ,[CreateDate]
		  ,[CreateUserName]
		  ,[SourceID]
		  ,[ValidationIndicator]
		  ,[ValidationComments]
		  ,[DateModified]
		  ,[Note]	
		  ,[VersionCount]
		  ,[ContributingCompany]
		  ,[IDSourceCode]
		FROM [EmployeeName]
		WHERE  [Item] = ''' + @Item + '''' + @nameTypeCondition
		
--print @sql
exec(@sql)
END

GO
/****** Object:  StoredProcedure [dbo].[EmployeeName_PHONETICSEARCH]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[EmployeeName_PHONETICSEARCH]
	@PhoneticLastName varchar(10)
	,@DOB datetime = null
	,@userName varchar(50) = null
	,@userRole varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@PhoneticLastName is null)
		return;
	
	if (ltrim(rtrim(@PhoneticLastName)) = '')
		return;
	
   	if (@username is not null)
	begin
		if (@PhoneticLastName is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeName_PHONETICSEARCH', @parameterName='phoneticLastName', @parameterValue=@PhoneticLastName, @userName=@userName;
		if (@DOB is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeName_PHONETICSEARCH', @parameterName='dob', @parameterValue=@DOB, @userName=@userName;
	end

	if ((@userRole is not null) and (@userRole = 'adult'))
		SELECT [EmployeeNameID]
		  ,[Item]
		  ,[FirstName]
		  ,[MiddleName]
		  ,[LastName]
		  ,[SuffixName]
		  ,[NameTitle]
		  ,[BusinessName]
		  ,[DOB]
		  ,[DeathDate]
		  ,[NameType]
		  ,[PhoneticLastName]
		  ,[AliasIndicator]
		  ,[DataSource]
		  ,[CreateDate]
		  ,[CreateUserName]
		  ,[SourceID]
		  ,[ValidationIndicator]
		  ,[ValidationComments]
		  ,[DateModified]
		  ,[Note]
		  ,[VersionCount]
		  ,[ContributingCompany]
		  ,[IDSourceCode]
		FROM [dbo].[EmployeeName]
		WHERE ([PhoneticLastName] like @PhoneticLastName + '%' or [PhoneticLastName] is null)
			-- always want the 'null' DOB's in the result set
--		and ([dob] = @DOB or ([dob] is null or [dob] = ''))
		and ([dob] = ISNULL(@DOB, dob) or [dob] is null or [dob] = '')
		and ([NameType] = 'adult')
		Order By DOB
	else  -- users with juvenile security can see all (juvenile and adult) records
		SELECT [EmployeeNameID]
		  ,[Item]
		  ,[FirstName]
		  ,[MiddleName]
		  ,[LastName]
		  ,[SuffixName]
		  ,[NameTitle]
		  ,[BusinessName]
		  ,[DOB]
		  ,[DeathDate]
		  ,[NameType]
		  ,[PhoneticLastName]
		  ,[AliasIndicator]
		  ,[DataSource]
		  ,[CreateDate]
		  ,[CreateUserName]
		  ,[SourceID]
		  ,[ValidationIndicator]
		  ,[ValidationComments]
		  ,[DateModified]
		  ,[Note]
		  ,[VersionCount]
		  ,[ContributingCompany]
		  ,[IDSourceCode]
		FROM [dbo].[EmployeeName]
		WHERE ([PhoneticLastName] like @PhoneticLastName + '%' or [PhoneticLastName] is null)
			-- always want the 'null' DOB's in the result set
--		and ([dob] = @DOB or ([dob] is null or [dob] = ''))
		and ([dob] = ISNULL(@DOB, dob) or [dob] is null or [dob] = '')
		Order By DOB
END

GO
/****** Object:  StoredProcedure [dbo].[EmployeeName_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EmployeeName_READ]
	@EmployeeNameID int = null
	,@Item char(10) = null
	,@FirstName varchar(50) = null
	,@LastName varchar(50) = null
	,@ExcludeItem varchar(500) = null
	,@nameType varchar(10) = null
	,@userName varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@EmployeeNameID is NULL and @Item is NULL and @LastName is null and @FirstName is null and @nameType is null)
		return;
		
--print @FirstName
--print @LastName

	DECLARE @TempItems table ( Item char(10))
	DECLARE @singleItem char(10), @Pos int
	
	SET @ExcludeItem = LTRIM(RTRIM(@ExcludeItem))+ ','
	SET @Pos = CHARINDEX(',', @ExcludeItem, 1)

	IF REPLACE(@ExcludeItem, ',', '') <> ''
	BEGIN
		WHILE @Pos > 0
		BEGIN
			SET @singleItem = LTRIM(RTRIM(LEFT(@ExcludeItem, @Pos - 1)))
				IF @singleItem <> ''
				BEGIN
					INSERT INTO @TempItems (Item) VALUES (cast(@singleItem as char(10))) --Use Appropriate conversion
				END
			SET @ExcludeItem = RIGHT(@ExcludeItem, LEN(@ExcludeItem) - @Pos)
			SET @Pos = CHARINDEX(',', @ExcludeItem, 1)
		END
	END
	
   	if (@username is not null)
	begin
		if (@EmployeeNameID is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeName_READ', @parameterName='EmployeeNameID', @parameterValue=@EmployeeNameID, @userName=@userName;
		if (@Item is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeName_READ', @parameterName='Item', @parameterValue=@Item, @userName=@userName;
		if (@FirstName is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeName_READ', @parameterName='firstName', @parameterValue=@FirstName, @userName=@userName;
		if (@LastName is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeName_READ', @parameterName='lastName', @parameterValue=@LastName, @userName=@userName;
		if (@ExcludeItem is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeName_READ', @parameterName='excludedItems', @parameterValue=@ExcludeItem, @userName=@userName;
		if (@nameType is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeName_READ', @parameterName='nameType', @parameterValue=@nameType, @userName=@userName;
	end

	if (@EmployeeNameID is NOT NULL and @EmployeeNameID > 0)
		SELECT [EmployeeNameID]
		  ,[Item]
		  ,[FirstName]
		  ,[MiddleName]
		  ,[LastName]
		  ,[SuffixName]
		  ,[NameTitle]
		  ,[BusinessName]
		  ,[DOB]
		  ,[DeathDate]
		  ,[NameType]
		  ,[PhoneticLastName]
		  ,[AliasIndicator]
		  ,[DataSource]
		  ,[CreateDate]
		  ,[CreateUserName]
		  ,[SourceID]
		  ,[ValidationIndicator]
		  ,[ValidationComments]
		  ,[DateModified]
          ,[Note]
		  ,[VersionCount]
		  ,[ContributingCompany]
		  ,[IDSourceCode]
		FROM [EmployeeName]
		WHERE [EmployeeNameID] = @EmployeeNameID
	ELSE if (@Item is not null)
		SELECT [EmployeeNameID]
		  ,[Item]
		  ,[FirstName]
		  ,[MiddleName]
		  ,[LastName]
		  ,[SuffixName]
		  ,[NameTitle]
		  ,[BusinessName]
		  ,[DOB]
		  ,[DeathDate]
		  ,[NameType]
		  ,[PhoneticLastName]
		  ,[AliasIndicator]
		  ,[DataSource]
		  ,[CreateDate]
		  ,[CreateUserName]
		  ,[SourceID]
		  ,[ValidationIndicator]
		  ,[ValidationComments]
		  ,[DateModified]
		  ,[Note]
		  ,[VersionCount]
		  ,[ContributingCompany]
		  ,[IDSourceCode]
		FROM [EmployeeName]
		WHERE [Item] = @Item
	else if @nameType = 'adult'
		SELECT [EmployeeNameID]
		  ,[Item]
		  ,[FirstName]
		  ,[MiddleName]
		  ,[LastName]
		  ,[SuffixName]
		  ,[NameTitle]
		  ,[BusinessName]
		  ,[DOB]
		  ,[DeathDate]
		  ,[NameType]
		  ,[PhoneticLastName]
		  ,[AliasIndicator]
		  ,[DataSource]
		  ,[CreateDate]
		  ,[CreateUserName]
		  ,[SourceID]
		  ,[ValidationIndicator]
		  ,[ValidationComments]
		  ,[DateModified]
		  ,[Note]
		  ,[VersionCount]
		  ,[ContributingCompany]
		  ,[IDSourceCode]
		FROM [EmployeeName]
		WHERE 
			((FirstName = @FirstName or @FirstName is null)
			and (LastName = @LastName or @LastName is null)
			and Item not in (select Item from @TempItems)) and nametype = 'adult'
	else if @nameType = 'juvenile'
		SELECT [EmployeeNameID]
		  ,[Item]
		  ,[FirstName]
		  ,[MiddleName]
		  ,[LastName]
		  ,[SuffixName]
		  ,[NameTitle]
		  ,[BusinessName]
		  ,[DOB]
		  ,[DeathDate]
		  ,[NameType]
		  ,[PhoneticLastName]
		  ,[AliasIndicator]
		  ,[DataSource]
		  ,[CreateDate]
		  ,[CreateUserName]
		  ,[SourceID]
		  ,[ValidationIndicator]
		  ,[ValidationComments]
		  ,[DateModified]
		  ,[Note]
		  ,[VersionCount]
		  ,[ContributingCompany]
		  ,[IDSourceCode]
		FROM [EmployeeName]
		WHERE 
			((FirstName = @FirstName or @FirstName is null)
			and (LastName = @LastName or @LastName is null)
			and Item not in (select Item from @TempItems)) and nametype = 'juvenile'
	else  -- @nameType is null
	SELECT [EmployeeNameID]
      ,[Item]
      ,[FirstName]
      ,[MiddleName]
      ,[LastName]
      ,[SuffixName]
      ,[NameTitle]
      ,[BusinessName]
      ,[DOB]
      ,[DeathDate]
      ,[NameType]
	  ,[PhoneticLastName]
	  ,[AliasIndicator]
      ,[DataSource]
      ,[CreateDate]
      ,[CreateUserName]
	  ,[SourceID]
	  ,[ValidationIndicator]
	  ,[ValidationComments]
	  ,[DateModified]
      ,[Note]
      ,[VersionCount]
	  ,[ContributingCompany]
	  ,[IDSourceCode]
	FROM [EmployeeName]
	WHERE 
		((FirstName = @FirstName or @FirstName is null)
		and (LastName = @LastName or @LastName is null)
		and Item not in (select Item from @TempItems))
END	
GO
/****** Object:  StoredProcedure [dbo].[EmployeeName_SEARCHBYATTRIBUTES]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EmployeeName_SEARCHBYATTRIBUTES]
	@AssignedIDType varchar(100) = null
	,@AssignedIDNumber varchar(200) = null
	,@ExcludeItem varchar(500) = null
	,@nameType varchar(10) = null
	,@userName varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@AssignedIDType is null and @AssignedIDNumber is null)
		return;
	
	if (ltrim(rtrim(@AssignedIDType)) = '' and ltrim(rtrim(@AssignedIDNumber)) = '')
		return;
	
	if (@username is not null)
	begin
		if (@AssignedIDType > '')
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeName_SEARCHBYATTRIBUTES', @parameterName='assignedIDType', @parameterValue=@AssignedIDType, @userName=@userName;
		if (@AssignedIDNumber > '')
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeName_SEARCHBYATTRIBUTES', @parameterName='assignedIDNumber', @parameterValue=@AssignedIDNumber, @userName=@userName;
		if (@ExcludeItem > '')
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeName_SEARCHBYATTRIBUTES', @parameterName='excludedItem', @parameterValue=@ExcludeItem, @userName=@userName;
	end

	DECLARE @TempIDTypes table ( IDType varchar(10))
	DECLARE @TempIDNumbers table ( IDNumber varchar(50)	)
	DECLARE @TempItems table ( Item char(10))

	DECLARE @IDType varchar(10), @Pos int,  @IDNumber varchar(50), @Pos2 int, @Item char(10), @Pos3 int	
	
	SET @AssignedIDType = LTRIM(RTRIM(@AssignedIDType))+ ','
	SET @Pos = CHARINDEX(',', @AssignedIDType, 1)

	SET @AssignedIDNumber = LTRIM(RTRIM(@AssignedIDNumber))+ ','
	SET @Pos2 = CHARINDEX(',', @AssignedIDNumber, 1)

	SET @ExcludeItem = LTRIM(RTRIM(@ExcludeItem))+ ','
	SET @Pos3 = CHARINDEX(',', @ExcludeItem, 1)

	IF REPLACE(@AssignedIDType, ',', '') <> ''
	BEGIN
		WHILE @Pos > 0
		BEGIN
			SET @IDType = LTRIM(RTRIM(LEFT(@AssignedIDType, @Pos - 1)))
				IF @IDType <> ''
				BEGIN
					INSERT INTO @TempIDTypes (IDType) VALUES (@IDType) --Use Appropriate conversion
				END
			SET @AssignedIDType = RIGHT(@AssignedIDType, LEN(@AssignedIDType) - @Pos)
			SET @Pos = CHARINDEX(',', @AssignedIDType, 1)
		END
	END          

	IF REPLACE(@AssignedIDNumber, ',', '') <> ''
	BEGIN
		WHILE @Pos2 > 0
		BEGIN
			SET @IDNumber = LTRIM(RTRIM(LEFT(@AssignedIDNumber, @Pos2 - 1)))
			IF @IDNumber <> ''
			BEGIN
				INSERT INTO @TempIDNumbers (IDNumber) VALUES (@IDNumber)
			END
			SET @AssignedIDNumber = RIGHT(@AssignedIDNumber, LEN(@AssignedIDNumber) - @Pos2)
			SET @Pos2 = CHARINDEX(',', @AssignedIDNumber, 1)
		END
	END	

	IF REPLACE(@ExcludeItem, ',', '') <> ''
	BEGIN
		WHILE @Pos3 > 0
		BEGIN
			SET @Item = LTRIM(RTRIM(LEFT(@ExcludeItem, @Pos3 - 1)))
				IF @Item <> ''
				BEGIN
					INSERT INTO @TempItems (Item) VALUES (cast(@Item as char(10))) --Use Appropriate conversion
				END
			SET @ExcludeItem = RIGHT(@ExcludeItem, LEN(@ExcludeItem) - @Pos3)
			SET @Pos3 = CHARINDEX(',', @ExcludeItem, 1)
		END
	END   

	if @nameType is null
	SELECT [EmployeeName].[EmployeeNameID]
      ,[EmployeeName].[Item]
      ,[EmployeeName].[FirstName]
      ,[EmployeeName].[MiddleName]
      ,[EmployeeName].[LastName]
      ,[EmployeeName].[SuffixName]
      ,[EmployeeName].[NameTitle]
      ,[EmployeeName].[BusinessName]
      ,[EmployeeName].[DOB]
      ,[EmployeeName].[DeathDate]
      ,[EmployeeName].[NameType]
	  ,[EmployeeName].[PhoneticLastName]
	  ,[EmployeeName].[AliasIndicator]
      ,[EmployeeName].[DataSource]
      ,[EmployeeName].[CreateDate]
      ,[EmployeeName].[CreateUserName]
	  ,[EmployeeName].[SourceID]
	  ,[EmployeeName].[ValidationIndicator]
	  ,[EmployeeName].[ValidationComments]
	  ,[EmployeeName].[DateModified]
	  ,[EmployeeName].[Note]
      ,[EmployeeName].[VersionCount]
	  ,[EmployeeName].[ContributingCompany]
	  ,[EmployeeName].[IDSourceCode]
	FROM 
		[dbo].[EmployeeName],
		[dbo].[EmployeeAttribute]
	WHERE ([EmployeeName].[EmployeeNameID] = [EmployeeAttribute].[EmployeeNameID])
	    and ([EmployeeAttribute].[AssignedIDType] in (select IDType from @TempIDTypes))
		and ([EmployeeAttribute].[AssignedIDNumber] in (select IDNumber from @TempIDNumbers))
		and (Item not in (select Item from @TempItems))

	else if @nameType = 'adult'
	SELECT [EmployeeName].[EmployeeNameID]
      ,[EmployeeName].[Item]
      ,[EmployeeName].[FirstName]
      ,[EmployeeName].[MiddleName]
      ,[EmployeeName].[LastName]
      ,[EmployeeName].[SuffixName]
      ,[EmployeeName].[NameTitle]
      ,[EmployeeName].[BusinessName]
      ,[EmployeeName].[DOB]
      ,[EmployeeName].[DeathDate]
      ,[EmployeeName].[NameType]
	  ,[EmployeeName].[PhoneticLastName]
	  ,[EmployeeName].[AliasIndicator]
      ,[EmployeeName].[DataSource]
      ,[EmployeeName].[CreateDate]
      ,[EmployeeName].[CreateUserName]
	  ,[EmployeeName].[SourceID]
	  ,[EmployeeName].[ValidationIndicator]
	  ,[EmployeeName].[ValidationComments]
	  ,[EmployeeName].[DateModified]
	  ,[EmployeeName].[Note]
      ,[EmployeeName].[VersionCount]
	  ,[EmployeeName].[ContributingCompany]
	  ,[EmployeeName].[IDSourceCode]
	FROM 
		[dbo].[EmployeeName],
		[dbo].[EmployeeAttribute]
	WHERE ([EmployeeName].[EmployeeNameID] = [EmployeeAttribute].[EmployeeNameID])
	    and ([EmployeeAttribute].[AssignedIDType] in (select IDType from @TempIDTypes))
		and ([EmployeeAttribute].[AssignedIDNumber] in (select IDNumber from @TempIDNumbers))
		and (Item not in (select Item from @TempItems)) and nametype = 'adult'

	else if @nameType = 'juvenile'
	SELECT [EmployeeName].[EmployeeNameID]
      ,[EmployeeName].[Item]
      ,[EmployeeName].[FirstName]
      ,[EmployeeName].[MiddleName]
      ,[EmployeeName].[LastName]
      ,[EmployeeName].[SuffixName]
      ,[EmployeeName].[NameTitle]
      ,[EmployeeName].[BusinessName]
      ,[EmployeeName].[DOB]
      ,[EmployeeName].[DeathDate]
      ,[EmployeeName].[NameType]
	  ,[EmployeeName].[PhoneticLastName]
	  ,[EmployeeName].[AliasIndicator]
      ,[EmployeeName].[DataSource]
      ,[EmployeeName].[CreateDate]
      ,[EmployeeName].[CreateUserName]
	  ,[EmployeeName].[SourceID]
	  ,[EmployeeName].[ValidationIndicator]
	  ,[EmployeeName].[ValidationComments]
	  ,[EmployeeName].[DateModified]
	  ,[EmployeeName].[Note]
      ,[EmployeeName].[VersionCount]
	  ,[EmployeeName].[ContributingCompany]
	  ,[EmployeeName].[IDSourceCode]
	FROM 
		[dbo].[EmployeeName],
		[dbo].[EmployeeAttribute]
	WHERE ([EmployeeName].[EmployeeNameID] = [EmployeeAttribute].[EmployeeNameID])
	    and ([EmployeeAttribute].[AssignedIDType] in (select IDType from @TempIDTypes))
		and ([EmployeeAttribute].[AssignedIDNumber] in (select IDNumber from @TempIDNumbers))
		and (Item not in (select Item from @TempItems)) and nametype = 'juvenile'

	--drop table #IDTypes 
	--drop table #IDNumbers
END

GO
/****** Object:  StoredProcedure [dbo].[EmployeeName_SEARCHDOB]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[EmployeeName_SEARCHDOB]
	@FirstName varchar(50)
	,@LastName varchar(50)
	,@DOB datetime
	,@userName varchar(50) = null
	,@userRole varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@FirstName is null and @LastName is null and @DOB is null)
		return;
	
	if (ltrim(rtrim(@FirstName)) = '' and ltrim(rtrim(@LastName)) = '' and ltrim(rtrim(@DOB)) = '')
		return;

   	if (@username is not null)
	begin
		if (@LastName is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeName_SEARCHDOB', @parameterName='lastName', @parameterValue=@LastName, @userName=@userName;
		if (@FirstName is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeName_SEARCHDOB', @parameterName='firstName', @parameterValue=@FirstName, @userName=@userName;
		if (@DOB is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeName_SEARCHDOB', @parameterName='dob', @parameterValue=@DOB, @userName=@userName;
	end

	if ((@userRole is not null) and (@userRole = 'adult'))
		SELECT [EmployeeNameID]
		  ,[Item]
		  ,[FirstName]
		  ,[MiddleName]
		  ,[LastName]
		  ,[SuffixName]
		  ,[NameTitle]
		  ,[BusinessName]
		  ,[DOB]
		  ,[DeathDate]
		  ,[NameType]
		  ,[PhoneticLastName]
		  ,[AliasIndicator]
		  ,[DataSource]
		  ,[CreateDate]
		  ,[CreateUserName]
		  ,[SourceID]
		  ,[ValidationIndicator]
		  ,[ValidationComments]
		  ,[DateModified]
		  ,[Note]
		  ,[VersionCount]
		  ,[ContributingCompany]
		  ,[IDSourceCode]
		FROM [dbo].[EmployeeName]
		WHERE (FirstName like @FirstName or @FirstName is null or FirstName is null)
			and (LastName Like @LastName or @LastName is null)
			and (DOB = @DOB or @DOB is null or DOB is null)
			and ([NameType] = 'adult')
	else  -- when user has 'juvenile' role they can view all names
		SELECT [EmployeeNameID]
		  ,[Item]
		  ,[FirstName]
		  ,[MiddleName]
		  ,[LastName]
		  ,[SuffixName]
		  ,[NameTitle]
		  ,[BusinessName]
		  ,[DOB]
		  ,[DeathDate]
		  ,[NameType]
		  ,[PhoneticLastName]
		  ,[AliasIndicator]
		  ,[DataSource]
		  ,[CreateDate]
		  ,[CreateUserName]
		  ,[SourceID]
		  ,[ValidationIndicator]
		  ,[ValidationComments]
		  ,[DateModified]
		  ,[Note]
		  ,[VersionCount]
		  ,[ContributingCompany]
		  ,[IDSourceCode]
		FROM [dbo].[EmployeeName]
		WHERE (FirstName like @FirstName or @FirstName is null or FirstName is null)
			and (LastName Like @LastName or @LastName is null)
			and (DOB = @DOB or @DOB is null or DOB is null)
END

GO
/****** Object:  StoredProcedure [dbo].[EmployeeName_SEARCHID]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EmployeeName_SEARCHID]
	 @Item char(10) = null
	,@ExcludeItem varchar(500) = null
	,@nameType varchar(10) = null
	,@userName varchar(50) = null
AS
BEGIN

	SET NOCOUNT ON;
--print 'LS ID: '+@LSIDNumber
--print 'Item: '+@Item
--print 'excluded Item: '+@ExcludeItem
--print 'username: '+@userName

	if (@Item is null )
		return;

	DECLARE @TempItems table ( Item char(10))
	DECLARE @TempItem char(10), @Pos int
	
	SET @ExcludeItem = LTRIM(RTRIM(@ExcludeItem))+ ','
	SET @Pos = CHARINDEX(',', @ExcludeItem, 1)

	IF REPLACE(@ExcludeItem, ',', '') <> ''
	BEGIN
		WHILE @Pos > 0
		BEGIN
			SET @TempItem = LTRIM(RTRIM(LEFT(@ExcludeItem, @Pos - 1)))
				IF @TempItem <> ''
				BEGIN
					INSERT INTO @TempItems (Item) VALUES (cast(@TempItem as char(10))) --Use Appropriate conversion
				END
			SET @ExcludeItem = RIGHT(@ExcludeItem, LEN(@ExcludeItem) - @Pos)
			SET @Pos = CHARINDEX(',', @ExcludeItem, 1)
		END
	END   

   	if (@username is not null)
	begin
		if (@Item is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeName_SEARCHID', @parameterName='Item', @parameterValue=@Item, @userName=@userName;
	 	if (@ExcludeItem is not null and @ExcludeItem <>'')
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeName_SEARCHID', @parameterName='excludedItem', @parameterValue=@ExcludeItem, @userName=@userName;
	end

	SELECT [EmployeeNameID]
		,[Item]
		,[FirstName]
		,[MiddleName]
		,[LastName]
		,[SuffixName]
		,[NameTitle]
		,[BusinessName]
		,[DOB]
		,[DeathDate]
		,[NameType]
        ,[PhoneticLastName]
		,[AliasIndicator]
		,[DataSource]
		,[CreateDate]
		,[CreateUserName]
		,[SourceID]
		,[ValidationIndicator]
		,[ValidationComments]
		,[DateModified]
		,[Note]
		,[VersionCount]
		,[ContributingCompany]
		,[IDSourceCode]
	FROM [dbo].[EmployeeName]
	WHERE (Item = @Item) 
		and Item not in (select Item from @TempItems) 
		AND [NameType] = ISNULL(@nameType, NameType)
END

GO
/****** Object:  StoredProcedure [dbo].[EmployeeName_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[EmployeeName_UPDATE]
	@EmployeeNameID int
	,@Item char(10)
	,@FirstName varchar(50) = null
	,@MiddleName varchar(50) = null
	,@LastName varchar(50) = null
	,@SuffixName varchar(10) = null
	,@NameTitle varchar(10) = null
	,@BusinessName varchar(100) = null
	,@DOB datetime = null
	,@DeathDate smalldatetime = null
	,@NameType varchar(10) = null
	,@PhoneticLastName varchar(10) = null
	,@AliasIndicator char(1) = null
	,@DataSource varchar(10)
	,@CreateUserName varchar(50)
	,@SourceID varchar(255) = null
	,@ValidationIndicator char(1) = 'N'
	,@ValidationComments varchar(100) = null
	,@ContributingCompany varchar(50) = null
	,@DateModified smalldatetime = null
    ,@Note varchar(100) = null
	,@IDSourceCode varchar(50) = null
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE  [dbo].[EmployeeName]
	SET     [Item] = @Item
			,[FirstName] = @FirstName
			,[MiddleName] = @MiddleName
			,[LastName] = @LastName
			,[SuffixName] = @SuffixName
			,[NameTitle] = @NameTitle
			,[BusinessName] = @BusinessName
			,[DOB] = @DOB
			,[DeathDate] = @DeathDate
			,[NameType] = @NameType
			,[PhoneticLastName] = @PhoneticLastName
			,[SoundexFirstName] = SOUNDEX(@FirstName)
			,[SoundexLastName] = SOUNDEX(@LastName)
			,[AliasIndicator] = @AliasIndicator
			,[DataSource] = @DataSource
			,[CreateUserName] = @CreateUserName
			,[SourceID] = @SourceID
			,[ValidationIndicator] = @ValidationIndicator
			,[ValidationComments] = @ValidationComments
		    ,[ContributingCompany] = @ContributingCompany
		    ,[DateModified] = @DateModified
		    ,[Note] = @Note
		    ,[IDSourceCode] = @IDSourceCode
			,[UpdateUserName] = @UpdateUserName
			,[UpdateDate] = GETDATE()
			,[VersionCount] = [VersionCount] + 1
	WHERE   [EmployeeNameID] = @EmployeeNameID
END









GO
/****** Object:  StoredProcedure [dbo].[EmployeeParent_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[EmployeeParent_CREATE]
	@EmployeeNameID int
    ,@PartyType varchar(15)
	,@FirstName varchar(50) = null
	,@MiddleName varchar(50) = null
	,@LastName varchar(50) 
	,@NameTitle varchar(10) = null
	,@AddressLine1 varchar(50) = null
	,@AddressLine2 varchar(50) = null
	,@AddressLine3 varchar(50) = null
	,@City varchar(20) = null
	,@State varchar(2) = null
	,@PostalCode varchar(9) = null
	,@SSN varchar(9) = null
	,@PhoneNumber varchar(20) = null
	,@Notes varchar(70) = null
	,@SourceID varchar(40) = null
	,@DataSource varchar(50) = null
	,@DateModified smalldatetime = null
	,@ContributingCompany varchar(50) = null
	,@IDSourceCode varchar(50)= null
	,@UpdateUserName nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[EmployeeParent]
		([EmployeeNameID]
		,[PartyType]
		,[FirstName]
		,[MiddleName]
		,[LastName]
		,[NameTitle]
		,[AddressLine1]
		,[AddressLine2]
		,[AddressLine3]
		,[City]
		,[State]
		,[PostalCode]
		,[SSN]
		,[PhoneNumber]
		,[Notes]
		,[SourceID]
		,[DataSource]
		,[DateModified]
		,[ContributingCompany]
		,[IDSourceCode]
		,[UpdateUserName]
		,[UpdateDate]
		,[VersionCount])
     VALUES
		(@EmployeeNameID
		,@PartyType
		,@FirstName
		,@MiddleName
		,@LastName
		,@NameTitle
		,@AddressLine1
		,@AddressLine2
		,@AddressLine3
		,@City
		,@State
		,@PostalCode
		,@SSN
		,@PhoneNumber
		,@Notes
	    ,@SourceID
	    ,@DataSource
	    ,@DateModified
		,@ContributingCompany
		,@IDSourceCode
		,@UpdateUserName
		,GETDATE()
		,0)

	IF @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	ELSE 
		select 0
END






GO
/****** Object:  StoredProcedure [dbo].[EmployeeParent_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EmployeeParent_DELETE]
	  @EmployeeParentID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[EmployeeParent]
	WHERE [EmployeeParentID] = @EmployeeParentID
END

GO
/****** Object:  StoredProcedure [dbo].[EmployeeParent_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EmployeeParent_READ]
  	@EmployeeParentID int = null
	,@EmployeeNameID int = null  	
	,@item char(10) = null
	,@userName varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@EmployeeParentID is NULL and @item is null and @EmployeeNameID is null and @item is null)
		return;

   	if (@username is not null)
   	begin
   		if (@EmployeeParentID is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeParent_READ', @parameterName='EmployeeParentID', @parameterValue=@EmployeeParentID, @userName=@userName;
   		if (@item is not null)
			exec audit_LogSelect_CREATE @storedProcedureName='EmployeeParent_READ', @parameterName='Item', @parameterValue=@item, @userName=@userName;
	end

	if (@EmployeeParentID is not null)
		SELECT [EmployeeParentID]
			  ,[EmployeeNameID]
			  ,[PartyType]
			  ,[FirstName]
			  ,[MiddleName]
			  ,[LastName]
			  ,[NameTitle]
			  ,[AddressLine1]
			  ,[AddressLine2]
			  ,[AddressLine3]
			  ,[City]
			  ,[State]
			  ,[PostalCode]
			  ,[SSN]
			  ,[PhoneNumber]
			  ,[Notes]
			  ,[SourceID]
			  ,[DataSource]
			  ,[DateModified]
			  ,[ContributingCompany]
			  ,[IDSourceCode]
			  ,[UpdateUserName]
			  ,[UpdateDate]
			  ,[VersionCount]
		FROM [EmployeeParent]
		WHERE [EmployeeParentID] = @EmployeeParentID
	else
		if (@EmployeeNameID is not null)
				SELECT [EmployeeParentID]
			  ,[EmployeeNameID]
			  ,[PartyType]
			  ,[FirstName]
			  ,[MiddleName]
			  ,[LastName]
			  ,[NameTitle]
			  ,[AddressLine1]
			  ,[AddressLine2]
			  ,[AddressLine3]
			  ,[City]
			  ,[State]
			  ,[PostalCode]
			  ,[SSN]
			  ,[PhoneNumber]
			  ,[Notes]
			  ,[SourceID]
			  ,[DataSource]
			  ,[DateModified]
			  ,[ContributingCompany]
			  ,[IDSourceCode]
			  ,[UpdateUserName]
			  ,[UpdateDate]
			  ,[VersionCount]
		FROM [EmployeeParent]
		WHERE [EmployeeNameID] = @EmployeeNameID
		else
		SELECT [EmployeeParentID]
			  ,[EmployeeNameID]
			  ,[PartyType]
			  ,[FirstName]
			  ,[MiddleName]
			  ,[LastName]
			  ,[NameTitle]
			  ,[AddressLine1]
			  ,[AddressLine2]
			  ,[AddressLine3]
			  ,[City]
			  ,[State]
			  ,[PostalCode]
			  ,[SSN]
			  ,[PhoneNumber]
			  ,[Notes]
			  ,[SourceID]
			  ,[DataSource]
			  ,[DateModified]
			  ,[ContributingCompany]
			  ,[IDSourceCode]
			  ,[UpdateUserName]
			  ,[UpdateDate]
			  ,[VersionCount]
		FROM [EmployeeParent]
		WHERE [EmployeeNameID] in
			(select [EmployeeNameID] from Employeename where item = @item)
		order by [PartyType], [DateModified] desc	
END
GO
/****** Object:  StoredProcedure [dbo].[EmployeeParent_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[EmployeeParent_UPDATE]
	@EmployeeParentID int
	,@EmployeeNameID int
	,@PartyType varchar(15)
	,@FirstName varchar(50) = null
	,@MiddleName varchar(50) = null
	,@LastName varchar(50)
	,@NameTitle varchar(10) = null
	,@AddressLine1 varchar(50) = null
	,@AddressLine2 varchar(50) = null
	,@AddressLine3 varchar(50) = null
	,@City varchar(20) = null
	,@State varchar(2) = null
	,@PostalCode varchar(9) = null
	,@SSN varchar(9) = null
	,@PhoneNumber varchar(20) = null
	,@Notes varchar(70) = null
	,@SourceID varchar(40) = null
	,@DataSource varchar(50) = null
	,@DateModified smalldatetime = null
	,@ContributingCompany varchar(50) = null
	,@IDSourceCode varchar(50) = null
	,@UpdateUserName nvarchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE  [dbo].EmployeeParent
	SET [EmployeeNameID] = @EmployeeNameID
	  ,[PartyType] = @PartyType
	  ,[FirstName] = @FirstName
	  ,[MiddleName] = @MiddleName
	  ,[LastName] = @LastName
	  ,[NameTitle] = @NameTitle
	  ,[AddressLine1] = @AddressLine1
	  ,[AddressLine2] = @AddressLine2
	  ,[AddressLine3] = @AddressLine3
	  ,[City] = @City
	  ,[State] = @State
	  ,[PostalCode] = @PostalCode
	  ,[SSN] = @SSN
	  ,[PhoneNumber] = @PhoneNumber
	  ,[Notes] = @Notes
	  ,[SourceID] = @SourceID
	  ,[DataSource] = @DataSource
	  ,[DateModified] = @DateModified
	  ,[ContributingCompany] = @ContributingCompany
	  ,[IDSourceCode] = @IDSourceCode
	  ,[UpdateUserName] = @UpdateUserName
	  ,[UpdateDate] = GETDATE()
	  ,[VersionCount] = [VersionCount] + 1
	WHERE   [EmployeeParentID] = @EmployeeParentID
END



GO
/****** Object:  StoredProcedure [dbo].[EmployeePhone_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[EmployeePhone_CREATE]
	@EmployeeNameID int
	,@PhoneType varchar(10) = null
	,@PhoneNumber varchar(20) = null
	,@PhoneSuffix varchar(10) = null
	,@SourceID varchar(40) = null
	,@DataSource varchar(20) = null
	,@ContributingCompany varchar(50) = null
	,@DateModified smalldatetime = null
	,@IDSourceCode varchar(50) = null
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[EmployeePhone]
		([EmployeeNameID]
		,[PhoneType]
		,[PhoneNumber]
		,[PhoneSuffix]
		,[SourceID]
		,[DataSource]
		,[ContributingCompany]
		,[DateModified]
		,[IDSourceCode]
		,[UpdateUserName]
		,[UpdateDate]
		,[VersionCount])
     VALUES
		(@EmployeeNameID
		,@PhoneType
		,@PhoneNumber
		,@PhoneSuffix
		,@SourceID
		,@DataSource
		,@ContributingCompany
		,@DateModified
		,@IDSourceCode
		,@UpdateUserName
		,GETDATE()
		,0)

	IF @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	ELSE 
		select 0
END








GO
/****** Object:  StoredProcedure [dbo].[EmployeePhone_DELETE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EmployeePhone_DELETE]
	  @EmployeePhoneID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[EmployeePhone]
	WHERE [EmployeePhoneID] = @EmployeePhoneID
END


GO
/****** Object:  StoredProcedure [dbo].[EmployeePhone_GETBYItem]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EmployeePhone_GETBYItem]
	@Item char(10)
	,@userName varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;

	if (@Item is NULL or @Item = '')
		return;

   	if (@username is not null)
		exec audit_LogSelect_CREATE @storedProcedureName='EmployeePhone_GETBYItem', @parameterName='Item', @parameterValue=@Item, @userName=@userName;

	SELECT [EmployeePhoneID]
		,[EmployeeNameID]
		,[PhoneType]
		,[PhoneNumber]
		,[PhoneSuffix]
		,[SourceID]
		,[DataSource]
		,[DateModified]
		,[VersionCount]
		,[ContributingCompany]
		,[IDSourceCode]
	FROM [dbo].[EmployeePhone]
WHERE [EmployeeNameID] in
		(select [EmployeeNameID] from EmployeeNAME where Item=@Item)
	order by [PhoneType],[DateModified] desc
END












GO
/****** Object:  StoredProcedure [dbo].[EmployeePhone_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EmployeePhone_READ]
	@EmployeePhoneID int
	,@EmployeeNameID int
	,@sourceID varchar(40) = null
	,@userName varchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;
	if (@EmployeePhoneID is NULL and @EmployeeNameID is NULL and @sourceID is NULL)
		return;

   	if (@username is not null)
	begin
		exec audit_LogSelect_CREATE @storedProcedureName='EmployeePhone_READ', @parameterName='EmployeePhoneID', @parameterValue=@EmployeePhoneID, @userName=@userName;
		exec audit_LogSelect_CREATE @storedProcedureName='EmployeePhone_READ', @parameterName='EmployeeNameID', @parameterValue=@EmployeeNameID, @userName=@userName;
		exec audit_LogSelect_CREATE @storedProcedureName='EmployeePhone_READ', @parameterName='sourceID', @parameterValue=@sourceID, @userName=@userName;
	end

	if (@sourceID is NOT NULL)
		SELECT [EmployeePhoneID]
			,[EmployeeNameID]
			,[PhoneType]
			,[PhoneNumber]
			,[PhoneSuffix]
			,[SourceID]
			,[DataSource]
			,[DateModified]
			,[VersionCount]
			,[ContributingCompany]
			,[IDSourceCode]
		FROM [dbo].[EmployeePhone]
		WHERE [SourceID] = @sourceID
		order by [EmployeePhoneID]
	if (@EmployeePhoneID is NOT NULL)
		SELECT [EmployeePhoneID]
			,[EmployeeNameID]
			,[PhoneType]
			,[PhoneNumber]
			,[PhoneSuffix]
			,[SourceID]
			,[DataSource]
			,[DateModified]
			,[VersionCount]
			,[ContributingCompany]
			,[IDSourceCode]
		FROM [dbo].[EmployeePhone]
		WHERE [EmployeePhoneID] = @EmployeePhoneID
		order by [PhoneType], [DateModified]
	ELSE
		SELECT [EmployeePhoneID]
			,[EmployeeNameID]
			,[PhoneType]
			,[PhoneNumber]
			,[PhoneSuffix]
			,[SourceID]
			,[DataSource]
			,[DateModified]
			,[VersionCount]
			,[ContributingCompany]
			,[IDSourceCode]
		FROM [dbo].[EmployeePhone]
		WHERE [EmployeeNameID] = @EmployeeNameID
		order by [PhoneType], [DateModified]
END











GO
/****** Object:  StoredProcedure [dbo].[EmployeePhone_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[EmployeePhone_UPDATE]
	@EmployeePhoneID int
	,@EmployeeNameID char(10)
	,@PhoneType varchar(10) = null
	,@PhoneNumber varchar(20) = null
	,@PhoneSuffix varchar(10) = null
	,@SourceID varchar(40) = null
	,@DataSource varchar(10) = null
	,@ContributingCompany varchar(50) = null
	,@DateModified smalldatetime = null
	,@IDSourceCode varchar(50) = null
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE  [dbo].[EmployeePhone]
	SET     [EmployeeNameID] = @EmployeeNameID
			,[PhoneType] = @PhoneType
			,[PhoneNumber] = @PhoneNumber
			,[PhoneSuffix] = @PhoneSuffix
			,[UpdateUserName] = @UpdateUserName
			,[SourceID] = @SourceID
			,[DataSource] = @DataSource
			,[ContributingCompany] = @ContributingCompany
			,[DateModified] = @DateModified
			,[IDSourceCode] = @IDSourceCode
			,[UpdateDate] = GETDATE()
			,[VersionCount] = [VersionCount] + 1
	WHERE   [EmployeePhoneID] = @EmployeePhoneID
END






GO
/****** Object:  StoredProcedure [dbo].[SourceCompanyIdDump]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SourceCompanyIdDump]
	 @sourceCompanyID varchar(25) = null
AS
BEGIN
	SET NOCOUNT ON;

	IF (@sourceCompanyID is null)
		return;

	declare @count int

	set @count = (select count(*) from Customer where sourceid = @sourceCompanyID);
	if (@count > 0)
		select 'Employee' [Employee],sourceid, datasource, * from Customer where sourceid = @sourceCompanyID;

	set @count = (select count(*) from caseinformation where sourceid = @sourceCompanyID);
	if (@count > 0)
		select 'Order' [Order],sourceid, sourceCompany, * from caseinformation where sourceid = @sourceCompanyID;
	
	set @count = (select count(*) from Customername where sourceid = @sourceCompanyID and AliasIndicator != 'Y');
	if (@count > 0)
		select 'Name' [Name],sourceid, datasource,* from Customername where sourceid = @sourceCompanyID and AliasIndicator != 'Y';
	
	set @count = (select count(*) from Customerparent where sourceid like @sourceCompanyID + '%');
	if (@count > 0)
		select 'Parent' [Parent], sourceid, datasource,* from Customerparent where sourceid like @sourceCompanyID + '%';

	set @count = (select count(*) from Customername where sourceid like @sourceCompanyID + '%' and AliasIndicator = 'Y');
	if (@count > 0)
		select 'Alias' [Alias],sourceid, datasource,* from Customername where sourceid like @sourceCompanyID + '%' and AliasIndicator = 'Y';

	set @count = (select count(*) from Customeraddress where substring(sourceid, LEN(addresstype) + 1, LEN(sourceid) + 1) = @sourceCompanyID);
	if (@count > 0)
		select 'Adr' [Adr],sourceid, datasource, Customeraddressid, Customernameid, Employeeaddressid, addresstype, addressline1, city, state, postalcode, addressline2, addressline3, * from Customeraddress   
		where substring(sourceid, LEN(addresstype) + 1, LEN(sourceid) + 1) = @sourceCompanyID;

	set @count = (select count(*) from Customerattribute where substring(sourceid, LEN(assignedidtype) + 1, LEN(sourceid) + 1) = @sourceCompanyID);
	if (@count > 0)
		select 'Att' [Att],sourceid, datasource,* from Customerattribute where substring(sourceid, LEN(assignedidtype) + 1, LEN(sourceid) + 1) = @sourceCompanyID;

	set @count = (select count(*) from Customerdescription where sourceid = @sourceCompanyID);
	if (@count > 0)
		select 'Desc' [Desc],sourceid, datasource,* from Customerdescription where sourceid = @sourceCompanyID;

	set @count = (select count(*) from Customerphone where substring(sourceid, LEN(phonetype) + 1, LEN(sourceid) + 1) = @sourceCompanyID);
	if (@count > 0)
		select 'Phn' [Phn],sourceid, datasource,* from Customerphone where substring(sourceid, LEN(phonetype) + 1, LEN(sourceid) + 1) = @sourceCompanyID;
END

GO
/****** Object:  StoredProcedure [dbo].[SystemException_CREATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SystemException_CREATE]
	@Item char(10)
	,@ExceptionSource varchar(20) = null
	,@ErrorType varchar(20) = null
	,@ErrorCode varchar(10) = null
	,@ErrorDescription varchar(100) = null
	,@ErrorStatus char(1) = null
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[SystemException]
		([Item]
		,[ExceptionSource]
		,[ErrorType]
		,[ErrorCode]
		,[ErrorDescription]
		,[ErrorStatus]
		,[UpdateUserName]
		,[UpdateDate]
		,[VersionCount])
     VALUES
		(@Item
		,@ExceptionSource
		,@ErrorType
		,@ErrorCode
		,@ErrorDescription
		,@ErrorStatus
		,@UpdateUserName
		,GETDATE()
		,0)

	IF @@ERROR = 0 AND @@ROWCOUNT = 1 
		select scope_identity()
	ELSE 
		select 0
END


GO
/****** Object:  StoredProcedure [dbo].[SystemException_READ]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SystemException_READ]
	  @ExceptionID int
AS
BEGIN
	SET NOCOUNT ON;

	if (@ExceptionID is null)
		return;

	SELECT [ExceptionID]
      ,[Item]
      ,[ExceptionSource]
      ,[ErrorType]
      ,[ErrorCode]
      ,[ErrorDescription]
      ,[ErrorStatus]
      ,[UpdateUserName]
      ,[UpdateDate]
      ,[VersionCount]
	FROM [dbo].[SystemException]
	WHERE [ExceptionID] = @ExceptionID
END


GO
/****** Object:  StoredProcedure [dbo].[SystemException_UPDATE]    Script Date: 12/30/2015 1:19:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SystemException_UPDATE]
	@ExceptionID int
	,@Item char(10)
	,@ExceptionSource varchar(20) = null
	,@ErrorType varchar(20) = null
	,@ErrorCode varchar(10) = null
	,@ErrorDescription varchar(100) = null
	,@ErrorStatus char(1) = null
	,@UpdateUserName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE	[dbo].[SystemException]
	SET     [Item] = @Item
			,[ExceptionSource] = @ExceptionSource
			,[ErrorType] = @ErrorType
			,[ErrorCode] = @ErrorCode
			,[ErrorDescription] = @ErrorDescription
			,[ErrorStatus] = @ErrorStatus
			,[UpdateUserName] = @UpdateUserName
			,[UpdateDate] = GETDATE()
			,[VersionCount] = [VersionCount] + 1
	WHERE   [ExceptionID] = @ExceptionID
END

GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:39:41.1820588' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'AliasEmployeeName_ExcludeEmployeeNameID'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:39:42.9774963' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'BusinessException_ByItem'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:39:43.7112838' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'BusinessException_CREATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:39:46.2248963' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'BusinessException_DELETE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:39:42.9774963' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'BusinessException_DuplicateCheckByItem'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:39:46.9118463' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'BusinessException_READ'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:39:48.2232963' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'BusinessException_UPDATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:39:57.7625338' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'OrderInformation_CREATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:39:59.3081713' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'OrderInformation_DELETE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:00.0263463' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'OrderInformation_READ'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:00.7601338' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'OrderInformation_UPDATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:07.3017713' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'Customer_READ'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:10.8770338' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'CustomerAddress_CREATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:14.1712713' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'CustomerAddress_DELETE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:14.8738338' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'CustomerAddress_READ'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:16.4038588' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'CustomerAddress_UPDATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:19.4951338' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'CustomerAttribute_CREATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:21.2593463' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'CustomerAttribute_DELETE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:22.0243588' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'CustomerAttribute_READ'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:23.5231588' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'CustomerAttribute_UPDATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:25.4122713' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'CustomerDescription_CREATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:27.6136338' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'CustomerDescription_DELETE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:28.3318088' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'CustomerDescription_READ'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:29.7525463' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'CustomerDescription_UPDATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:41.7741713' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'CustomerName_UPDATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:45.2713713' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'CustomerPhone_CREATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:47.0668088' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'CustomerPhone_DELETE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:47.7693713' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'CustomerPhone_READ'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:49.1901088' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'CustomerPhone_UPDATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:55.9503213' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'LookupType_CREATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:56.9026838' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'LookupType_DELETE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:57.5740213' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'LookupType_READ'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:58.1985213' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'LookupType_UPDATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:40:59.1821088' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'LookupTypeValue_CREATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:00.2905963' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'LookupTypeValue_DELETE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:01.0712213' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'LookupTypeValue_READ'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:01.7425588' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'LookupTypeValue_UPDATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:02.8510463' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'ItemSource_ALL'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:03.4443213' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'ItemSource_CREATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:05.4895588' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'ItemSource_DELETE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:06.2545713' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'ItemSource_READ'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:06.9102963' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'ItemSource_UPDATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:07.8782713' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'NextKeyGenerator_CREATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:08.7525713' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'NextKeyGenerator_DELETE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:09.4239088' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'NextKeyGenerator_GetNextKey'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:10.1576963' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'NextKeyGenerator_READ'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:10.8914838' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'NextKeyGenerator_UPDATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:11.9219088' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeAddress_CREATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:14.8570588' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeAddress_DELETE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:15.5127838' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeAddress_GETBYItem'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:16.1528963' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeAddress_READ'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:16.9803588' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeAddress_UPDATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:20.0872463' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeAttribute_CREATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:21.7109463' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeAttribute_DELETE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:23.1785213' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeAttribute_GETBYItem'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:23.8498588' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeAttribute_READ'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:24.7709963' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeAttribute_UPDATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:26.4883713' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeDescription_CREATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:28.5960588' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeDescription_DELETE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:29.2517838' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeDescription_GETBYItem'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:29.9231213' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeDescription_READ'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:30.7037463' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeDescription_UPDATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:34.7473838' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeName_CREATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:37.7293713' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeName_DELETE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:38.4163213' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeName_Item'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:39.9463463' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeName_READ'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:41.4607588' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeName_SEARCHDOB'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:42.4755713' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeName_SEARCHID'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:46.7690088' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeeName_UPDATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:50.0632463' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeePhone_CREATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:51.7962338' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeePhone_DELETE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:52.4675713' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeePhone_GETBYItem'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:53.1232963' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeePhone_READ'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:53.8883088' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'EmployeePhone_UPDATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:57.8382713' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'SystemException_CREATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:59.1809463' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'SystemException_READ'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:41:59.8522838' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'SystemException_UPDATE'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:46:55.3812963' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'audit_ItemMergeLog'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:46:45.4829713' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'audit_EmployeeAddress'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:46:49.4641588' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'audit_EmployeeAttribute'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:46:52.1963463' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'audit_EmployeeDescription'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:46:55.3812963' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'audit_EmployeeName'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:46:59.6591213' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'audit_EmployeePhone'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:47:16.6455213' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerAddress'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:47:20.7203838' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerAttribute'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:47:23.7804338' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerDescription'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:47:30.3845213' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerName'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:47:36.4733963' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerPhone'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:47:50.6339338' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmployeeAddress'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:47:54.6775713' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmployeeAttribute'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:47:57.7376213' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmployeeDescription'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:48:01.0943088' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmployeeName'
GO
EXEC sys.sp_addextendedproperty @name=N'Last Scripted', @value=N'Tuesday, October 30, 2007 at 13:48:06.1683713' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmployeePhone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[16] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "LookupType"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 205
               Right = 211
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "LookupTypeValue"
            Begin Extent = 
               Top = 96
               Left = 280
               Bottom = 215
               Right = 453
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "LookupTypeValueTranslation"
            Begin Extent = 
               Top = 64
               Left = 660
               Bottom = 260
               Right = 833
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vLookupTranslation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vLookupTranslation'
GO
