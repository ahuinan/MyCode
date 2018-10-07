IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_Orgin]'))
DROP VIEW [dbo].[v_Orgin]
GO
/*外部机构视图。这个外部机构指的是入库单的发货机构，
包含三个档案，分别是；仓库、店铺、供应商
其中仓库是做调拨单使用，
店铺是店铺退货单使用，
供应商是做采购入库使用*/
CREATE VIEW dbo.v_Orgin
AS 
	SELECT ShopID AS OrginID,MerchantID,Code,Name FROM BasShop	
	WHERE [Status] = 1
	UNION ALL 
	SELECT StockID,MerchantID,Code,Name FROM BasStock
	WHERE [Status] = 1
	UNION ALL 
	SELECT VenderID,MerchantID,Code,Name FROM BasVender
	WHERE [Status] = 1
GO

