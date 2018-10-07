using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Services
{
    /// <summary>
    /// automapper的管理类
    /// </summary>
    public class AutoMapperManager
    {
        /// <summary>
        /// 初始化类型的map
        /// </summary>
        public static void InitMap(){
            Mapper.Initialize(cfg => cfg.AddProfile<MapProfile>());
		
        }
    }
}
