IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_Orgin]'))
DROP VIEW [dbo].[v_Orgin]
GO
/*�ⲿ������ͼ������ⲿ����ָ������ⵥ�ķ���������
���������������ֱ��ǣ��ֿ⡢���̡���Ӧ��
���вֿ�����������ʹ�ã�
�����ǵ����˻���ʹ�ã�
��Ӧ�������ɹ����ʹ��*/
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

