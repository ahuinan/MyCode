using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace MyCode.Project.Domain.Model
{
    ///<summary>
    ///图片库的使用
    ///</summary>
    [SugarTable("BasPictureUse")]
    public partial class BasPictureUse
    {
           public BasPictureUse(){


           }
           /// <summary>
           /// Desc:ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public Guid PictureUseID {get;set;}

           /// <summary>
           /// Desc:对应的商家。
           /// Default:
           /// Nullable:False
           /// </summary>           
           public Guid MerchantID {get;set;}

           /// <summary>
           /// Desc:对应的图片库
           /// Default:
           /// Nullable:False
           /// </summary>           
           public Guid PictureStockID {get;set;}

           /// <summary>
           /// Desc:文件类型11商品图片
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int PictureType {get;set;}

           /// <summary>
           /// Desc:对应的数据ID，例如商品图片，对应商品ID11  GoodsID;
           /// Default:
           /// Nullable:False
           /// </summary>           
           public Guid UrlData {get;set;}

           /// <summary>
           /// Desc:文件顺序。例如图片，0为主图，其它为按顺序的图。
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int OrderNo {get;set;}

           /// <summary>
           /// Desc:状态1在用，0停用
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Status {get;set;}

           /// <summary>
           /// Desc:创建人
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Creater {get;set;}

           /// <summary>
           /// Desc:创建时间
           /// Default:
           /// Nullable:False
           /// </summary>           
           public DateTime CreateTime {get;set;}

           /// <summary>
           /// Desc:备注
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Note {get;set;}

    }
}
