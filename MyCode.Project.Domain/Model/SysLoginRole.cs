using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace MyCode.Project.Domain.Model
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("SysLoginRole")]
    public partial class SysLoginRole
    {
           public SysLoginRole(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public Guid LoginRoleID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public Guid MerchantID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public Guid LoginID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public Guid RoleID {get;set;}

    }
}
