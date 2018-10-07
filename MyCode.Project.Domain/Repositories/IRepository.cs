using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using MyCode.Project.Infrastructure.Search;
using MyCode.Project.Infrastructure.Common;
using System.Data.SqlClient;
using SqlSugar;

namespace MyCode.Project.Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region 事务处理
        void BeginTran();

        void CommitTran();

        void RollbackTran();

        #endregion

        /// <summary>
        /// 根据表达式得到列表
        /// </summary>
        /// <param name="whereExpression">条件</param>
        /// <returns></returns>
        List<TEntity> SelectList(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 查询分页
        /// 例子 "select * from table where id=@id and name=@name",new {id=1,name="a"}
        /// </summary>

        PageResult<T> SelectListPage<T>(string sql, int pageIndex, int pageSize, string order,object parameters = null) where T : class, new();

        /// <summary>
        /// 查询分页
        /// </summary>
        PageResult<T> SelectListPage<T>(string sql, SearchCondition condition, int pageIndex, int pageSize, string order) where T:class,new();

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="sql">SQL条件</param>
        /// <param name="parameters">参数，可以直接用new{id=1}</param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sql, object parameters=null);

        /// <summary>
        /// 返回单条记录
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        TEntity SelectFirst(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 用SQL返回单条记录
        /// 例子 "select * from table where id=@id and name=@name",new {id=1,name="a"}
        /// </summary>
        T SelectFirst<T>(string sql, object parameters = null);

        /// <summary>
        /// 用SQL返回多条记录
        /// 例子 "select * from table where id=@id and name=@name",new {id=1,name="a"}
        /// </summary>
        List<T> SelectList<T>(string sql, object parameters=null);

        /// <summary>
        /// 根据主键返回的列表
        /// </summary>
        /// <param name="ids">一组主键ID</param>
        /// <returns></returns>
        List<TEntity> SelectList(List<Guid> ids);

        /// <summary>
        /// 得到数量
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        int Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="instance">实体</param>
        /// <param name="tablename">null使用默认表名。不为NULL，适用于分表情况</param>
        void Add(TEntity instance,string tablename=null);

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="tablename">null使用默认表名。不为NULL，适用于分表情况</param>
        void Add(List<TEntity> entities,string tablename=null);

        /// <summary>
        /// 单个的修改
        /// </summary>
        /// <param name="instance"></param>
        void Update(TEntity instance);

        /// <summary>
        /// 修改一组对象
        /// </summary>
        /// <param name="updateObjs"></param>
        void Update(List<TEntity> updateObjs);

        /// <summary>
        /// 批量修改，只修改某些字段
        /// </summary>
        /// <param name="updateObjs"></param>
        /// <param name="columns"></param>
        void Update(List<TEntity> updateObjs, Expression<Func<TEntity, object>> columns);

        /// <summary>
        /// 按字段批量修改一组对象
        /// </summary>
        /// <param name="updateObjs">一组对象</param>
        /// <param name="columns">要修改的列</param>
        void Update(IEnumerable<TEntity> updateObjs, Expression<Func<TEntity, object>> columns);

        /// <summary>
        /// 根据表达式是否存在
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        bool IsExist(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 批量修改的方法
        /// </summary>
        /// <param name="updateDynamicObject">修改的动态对象</param>
        /// <param name="columns">要修改的列</param>
        //void Update(dynamic updateDynamicObject, Expression<Func<TEntity, TEntity>> columns);
        //void Update(dynamic updateDynamicObject);

        /// <summary>
        /// 按字段修改，满足条件的数据，批量修改的补充。
        /// 例子：Update(it => new WorkProcess { Remark = "测试批量修改",SystemType = 0 },p => p.WorkProcessId ==Guid.Parse("7BDDBBD3-B1CD-4C25-93BA-D7BF22032108"));
        /// </summary>
        /// <param name="columns">要修改的列</param>
        /// <param name="whereExpression">要修改的条件</param>
        void Update(Expression<Func<TEntity, TEntity>> columns, Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 根据表达式删除
        /// </summary>
        /// <param name="whereExpression"></param>
        void Delete(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="ids"></param>
        void DeleteByIds(dynamic[] ids);

        /// <summary>
        /// 得到一个更加灵活的查询对象
        /// </summary>
        /// <returns></returns>
        ISugarQueryable<TEntity> Queryable();




    }
}