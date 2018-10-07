# 描述

1. 部署环境 ：Window Server
2. 开发平台 ：VS2015或VS2017
3. ORM ：SqlSugar
4. Ioc框架 ：Unity
5. Aop框架 ：Unity
6. Json框架 ：Newtonsoft.Json
7. 日志框架 : log4net
8. 缓存 ：内存或Redis
9. Api接口帮助：/swagger/，已按Area区域划分
10、授权：JWT token

## 通用操作
1. 明显能预知的异常，使用BaseException抛出

```
if (workprocess.Status != (int)WorkProcessStatus.STOP) {throw new BaseException("当前进程状态不是停止");}
```
2. 写日志

```
LogHelper.Error("错误测试str");
LogHelper.Info("错误测试Exception");
```
错误日志存放在目录App_Log，当异常发生的时候，每一层包括仓储和服务层都会拦截到错误信息，可以看到每一层具体的传参情况。看日志可以通过http请求txt文件查看到。

3.AOP事务 

方法头添加[TransactionCallHandler]，需要注意只拦截一个方法最外层的事务,一个方法只拦截一次。比如

```
[TransactionCallHandler]
public void RestartStopProcess(Guid workprocessId) {}
```

4、关于批量操作 

在Controller层循环，在Service层只写单个处理的方法。比如例子，批量审核：
Controller层：

```
[HttpPost]
public void Audit(List<Guid> ids)
{
    foreach(var id in ids) { _workProcessService.Audit(id);}
}
```
Service层：

```
[TransactionCallHandler]
public void Audit(Guid id) {}
```
此种方式能保证Service运用到事务，且不会出现大事务回滚的复杂情况。

5、关于事务补充 

第三方不可逆操作在方法里需要写到最后一步，比如：

```
[TransactionCallHandler]
public void TransferMoney()
{
    1.执行保存数据库操作，更新支付状态
    2.执行微信打款操作
}
```

6、SQL注入 

若对SQL注入不了解，没有把握自己写的方法会不会导致SQL注入，重点看下面的SQL操作demo，所有的SQL请求参数都采用参数化查询的方式提交。

7、AOP缓存

一些内容相对固定，不会变化，可以在方法头上加上缓存标识，比如10分钟缓存：

```
[CachingCallHandler("00:10:00")]
```
8、Token授权 

头部增加：Authorization token，比如： 
header 'Authorization: CbkmWDbwqf5BGvRUA416zYRBlAM09Py_mYGXDwoFflEqsHworu'

9、不需要授权的接口，在Controller层方法添加头：

```
[AllowAnonymous]
```
比如：

```
[HttpGet]
[AllowAnonymous]
public void ExportTest()
{
}
```
10、接口统一返回参数，格式：

```
{
"Data": null,//返回如果有数据，统一放这里，不管是List还是单实体
"ResultCode": 1,//如果接口没出错，则返回1;返回-1则接口出错
"ErrorMessage": null//如果接口出错，这里显示出错信息
}
```
11、GenerateCode 

代码自动生成，如果已存在相应的文件，除了实体外，仓储是直接跳过。

### 代码目录结构
```
├── 00-Lib              -- 依赖库文件夹
├── 02-Document         -- 文档文件夹
├── 03-FrameWork        -- 存放第三方开源源代码
├── 04-MyCode.Project.Domain --域
│   ├── Config             --配置实体   
│   ├── Message            --消息实体
│   │   ├── Act              --操作请求实体
│   │   ├── Request          --查询实体
│   │   ├── Response         --响应实体
│   │   ├── Model            --数据库实体
│   │   ├── Repositories     --仓储接口
├── 05-MyCode.Project.GenerateCode   --代码生成
├── 06-MyCode.Project.Infrastructure --基础设施层
├── 07-MyCode.Project.OutSideService --外部引用层
├── 08-MyCode.Project.Repositories   --仓储实现层
├── 09-MyCode.Project.ScheduleTask   --调度层
├── 10-MyCode.Project.Services       --服务层
├── 11-MyCode.Project.WebApi         --WEBAPI层                       -- 
```


## SQL操作

1. Insert插入成功会自动将自递增id新的值更新到实体

```
_workProcessRepository.Add(new WorkProcess()
{
          FuncType = 1,
          MethodName = "",
          ParameterInfo = "",
          WorkProcessId = Guid.NewGuid(),
          UpdateTime = DateTime.Now
});
```
2. 取单表的前n条数据，单表情况多参考这里写法，只返回需要的字段，不要返回表中所有字段；

```
return _workProcessRepository.Queryable()
.Where(p => p.Status == (int)WorkProcessStatus.RUUNING && p.SystemType == 200)
 .Take(top)
 .OrderBy(p => p.UpdateTime).Select(p => new { p.WorkProcessId }).ToList();

//生成Sql
exec sp_executesql N'SELECT * FROM (SELECT  [WorkProcessId] AS [WorkProcessId] ,ROW_NUMBER() OVER(ORDER BY [UpdateTime] ASC) AS RowIndex  FROM [WorkProcess] WITH(NOLOCK)   WHERE (( [Status] = @Status0 ) AND ( [SystemType] = @Const1 ))) T WHERE RowIndex BETWEEN 1 AND 10',N'@Status0 int,@Const1 int',@Status0=1,@Const1=200
```
3. Update 更新

```
 var workProcess = _workProcessRepository.SelectFirst(p => p.WorkProcessId == Guid.Parse("7BDDBBD3-B1CD-4C25-93BA-D7BF22032108"));
 workProcess.Remark = "修改测试";
_workProcessRepository.Update(workProcess);
```

4. Update 更新部分字段，如果能明确更新字段，用该方法

```
 _workProcessRepository.Update(
it => new WorkProcess { Remark = "测试批量修改",SystemType = 0 },
p => p.WorkProcessId ==Guid.Parse("7BDDBBD3-B1CD-4C25-93BA-D7BF22032108"));
//生成Sql
exec sp_executesql N'UPDATE [WorkProcess]  SET
[Remark] = @Const0 , [SystemType] = @Const1   WHERE ( [WorkProcessId] =@constant2)',N'@Const0 nvarchar(4000),@Const1 int,@constant2 uniqueidentifier',@Const0=N'测试批量修改',@Const1=0,@constant2='7BDDBBD3-B1CD-4C25-93BA-D7BF22032108'
```

5.Sql语句查询返回列表

```
_workProcessRepository.SelectList<WorkProcess>("Select top 10 * from workprocess where systemtype=@systemtype", new { SystemType = 200 });
//生成的Sql
exec sp_executesql N'Select top 10 * from workprocess where systemtype=@systemtype',N'@SystemType int',@SystemType=200
```

6.根据一组主键Guid返回列表

```
var ids = new List<Guid>();
ids.Add(Guid.Parse("6B2E752C-CBD3-4C56-80C0-0000339F982A"));
ids.Add(Guid.Parse("E1CD8853-993C-4EFE-8863-0000FDF68054"));
 _workProcessRepository.SelectList(ids);
 
//生成的Sql语句
SELECT [WorkProcessId],[TypePath],[MethodName],[ParameterInfo],[UpdateTime],[Remark],[Status],[ExceptionInfo],[SystemType],[FuncType] FROM [WorkProcess] WITH(NOLOCK)   WHERE [WorkProcessId] IN ('6b2e752c-cbd3-4c56-80c0-0000339f982a','e1cd8853-993c-4efe-8863-0000fdf68054')  
```


7.大数据插入  

当需要插入非常多的数据时，先将数据处理好，调用Add(List)的方法，速度会很快；特别注意不要一个个去Add(Model)，这种会非常慢；

```
var workProcessList = new List<WorkProcess>();

for (int i = 0; i < 1000; i++)
{
     workProcessList.Add(new WorkProcess(){
                        FuncType = 1,
                        MethodName = "",
                        ParameterInfo = "",
                        WorkProcessId = Guid.NewGuid(),
                        UpdateTime = DateTime.Now
      });
}
_workProcessRepository.Add(workProcessList);
```

8.大数据修改

当需要修改非常多的数据时，先将数据处理好，调用Update(List)的方法，速度非常快，特别注意不要一个个去Update(Model)，这种会非常慢；另提供一个优化的方法，只批量更新需要的字段：
```
//lines为一组Guid集合，这里某些情况并不需要从数据库拿数据
 var workprocesses = lines.Select(line => new WorkProcess { WorkProcessId = line.WorkProcessId, TypePath = "MyCode" });

_workProcessRepository.Update(workprocesses,it => new { it.TypePath});
//生成的Sql
UPDATE S SET S.[TypePath]=T.[TypePath] FROM [WorkProcess] S    INNER JOIN             (
              
 SELECT N'6b2e752c-cbd3-4c56-80c0-0000339f982a' AS [WorkProcessId],N'test201898' AS [TypePath]		
UNION ALL 
 SELECT N'e1cd8853-993c-4efe-8863-0000fdf68054' AS [WorkProcessId],N'test201898' AS [TypePath]
            ) T ON S.[WorkProcessId]=T.[WorkProcessId]
```
9、使用Sql语句查询单条数据

```
_workProcessRepository.SelectFirst<WorkProcess>("Select top 1 * from workprocess")
```

带参数：

```
_sysLoginRepository.SelectList<SysLogin>("select top 10 * from SysLogin where login like '%' + @key + '%'", new { key = "k" });
```


10、使用Sql语句查询列表数据

```
 _workProcessRepository.SelectList<WorkProcess>("Select top 10 * from workprocess");
```
11、IN查询

```
_workProcessRepository.Queryable().In<WorkProcess>("workprocessid", "6B2E752C-CBD3-4C56-80C0-0000339F982A", "E1CD8853-993C-4EFE-8863-0000FDF68054")
```
12、Count查询

```
_workProcessRepository.Count(p => p.SystemType == 200);
```
13、分页

```
 var strSql = $@"SELECT	
	L.LoginId,
	Login,
	L.Name,
	Tele,
	L.Editor,
	L.EditTime,
	L.Status,
	SR.Name AS RoleName
FROM 
	SysLogin L WITH (nolock)
LEFT JOIN
	dbo.SysLoginRole LR WITH (NOLOCK) ON L.LoginID = LR.LoginID
LEFT JOIN
	dbo.SysRole SR WITH (nolock) ON LR.RoleID = SR.RoleID";

var where = new SearchCondition();
where.AddCondition("L.MerchantID",merchantId, SqlOperator.Equal,true);
where.AddSqlCondition("L.Login like '%' + @key + '%'",
true,
new SugarParameter("key", "a")); 

where.AddCondition("L.status",1,SqlOperator.Equal,true);

var whereResult = where.BuildConditionSql();

return this.SelectListPage<LoginListResp>(strSql, where, request.Page, request.PageSize, "EditTime desc");
```
14、按需读取
```
//这里只需要一个字段
 var loginRole = _sysLoginRoleRepository.Queryable().Where(p => p.LoginID == loginId).Select(p => new { p.RoleID }).First();
```




# 参考或引用

- https://github.com/sunkaixuan/SqlSugar
- http://jwt.io
- https://github.com/jianxuanbing/SwashbuckleEx
