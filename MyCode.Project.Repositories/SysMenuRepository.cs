﻿using MyCode.Project.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Domain.Message;
using MyCode.Project.Domain.Model;
using MyCode.Project.Domain.Repositories;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Infrastructure.Search;

namespace MyCode.Project.Repositories
{
    public class SysMenuRepository: Repository<SysMenu>, ISysMenuRepository
    {
        public SysMenuRepository(MyCodeSqlSugarClient context) : base(context)
        { }

      


	

	}
}