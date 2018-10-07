using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace MyCode.Project.Domain.Model
{
    ///<summary>
    ///图片分组
    ///</summary>
    [SugarTable("BasPictureGroup")]
    public partial class BasPictureGroup
    {
           public BasPictureGroup(){


           }
           /// <summary>
           /// Desc:ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public Guid PictureStockGroupID {get;set;}

           /// <summary>
           /// Desc:对应的商家。
           /// Default:
           /// Nullable:False
           /// </summary>           
           public Guid MerchantID {get;set;}

           /// <summary>
           /// Desc:名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Name {get;set;}

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
           /// Desc:编辑人
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Editor {get;set;}

           /// <summary>
           /// Desc:编辑时间
           /// Default:
           /// Nullable:False
           /// </summary>           
           public DateTime EditTime {get;set;}

           /// <summary>
           /// Desc:备注
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Note {get;set;}

    }
}
