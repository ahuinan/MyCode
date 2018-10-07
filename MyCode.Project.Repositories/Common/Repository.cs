using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Domain.Repositories;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Infrastructure.Search;
using System.Data.SqlClient;
using MyCode.Project.Infrastructure;
using MyCode.Project.Infrastructure.Cache;
using System.Runtime.Remoting.Messaging;
using MyCode.Project.Repositories.Common;
using SqlSugar;
using MyCode.Project.Infrastructure.Exceptions;
using Newtonsoft.Json;
using MyCode.Project.Domain.Config;

namespace MyCode.Project.Repositories.Common
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class,new()
    {
        private MyCodeSqlSugarClient _context;

        public Repository(MyCodeSqlSugarClient context)
        {
            this._context = context;

            if (SystemConfig.IfOutputSql)
            {
                //输出执行的语句
                this._context.Aop.OnLogExecuting = (sql, pars) =>
                {
                    LogHelper.Info($"Sql:{sql}{Environment.NewLine}参数：{JsonConvert.SerializeObject(pars)}");

                    Console.WriteLine("底层仓储:" + context.GetHashCode());
                };
            }
        }

        #region 事务处理
        public void BeginTran()
        {
            this._context.Ado.BeginTran();
        }
        public void CommitTran()
        {
            this._context.Ado.CommitTran();
        }
        public void RollbackTran()
        {
            this._context.Ado.RollbackTran();
        }
        #endregion

        #region SelectListPage(查询分页)
        /// <summary>
        /// 例子 "select * from table where id=@id and name=@name",new {id=1,name="a"}
        /// </summary>

        public PageResult<T> SelectListPage<T>(string sql, int pageIndex, int pageSize, string order, object parameters = null) where T : class, new()
        {
            if (string.IsNullOrEmpty(order)) { throw new BaseException("排序字段不可空"); }

            var result = new PageResult<T>();

            int totalNum = 0;

            result.DataList = this._context.SqlQueryable<T>(sql).AddParameters(parameters).OrderBy(order).ToPageList(pageIndex, pageSize, ref totalNum);

            result.Total = totalNum;

            return result;
        }
        #endregion

        #region SelectListPage(查询分页)
        public PageResult<T> SelectListPage<T>(string sql, SearchCondition condition,int pageIndex,int pageSize, string order) where T : class, new()
        {
            var conditonModel = condition.BuildConditionSql();

            return SelectListPage<T>(sql + " where " + conditonModel.Sql, pageIndex, pageSize, order, conditonModel.ListParameter);
        }
        #endregion

        #region ExecuteSqlCommand(执行命令)
        public int ExecuteSqlCommand(string sql, object parameters=null) {
			
            return this._context.Ado.ExecuteCommand(sql, parameters);

        }
        #endregion

        #region SelectFirst(返回单条记录)

        public TEntity SelectFirst(Expression<Func<TEntity, bool>> whereExpression)
        {
            return this._context.Queryable<TEntity>().With(SqlWith.NoLock).Single(whereExpression);
        }
        #endregion

        #region SelectFirst(用SQL返回单条记录)
        /// <summary>
        /// 例子 "select * from table where id=@id and name=@name",new {id=1,name="a"}
        /// </summary>
        public T SelectFirst<T>(string sql, object parameters=null)
        {
            return this._context.Ado.SqlQuerySingle<T>(sql, parameters);
        }
        #endregion

        #region SelectList(用SQL返回多条记录)
        /// <summary>
        /// 例子 "select * from table where id=@id and name=@name",new {id=1,name="a"}
        /// </summary>
        public List<T> SelectList<T>(string sql,object parameters = null) 
        {
            return this._context.Ado.SqlQuery<T>(sql, parameters);
        }
        #endregion

        #region SelectList(列表)
        public List<TEntity> SelectList(Expression<Func<TEntity, bool>> whereExpression)
        {
            return _context.Queryable<TEntity>().Where(whereExpression).With(SqlWith.NoLock).ToList();
        }
        #endregion

        #region SelectList(根据主键返回的列表)
        public List<TEntity> SelectList(List<Guid> ids)
        {
            return _context.Queryable<TEntity>().In(ids).With(SqlWith.NoLock).ToList();
        }
        #endregion 

        #region Count(得到数量)
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return this._context.Queryable<TEntity>().Where(predicate).With(SqlWith.NoLock).Count();
        }
        #endregion

        #region IsExist(根据表达式是否存在)
        public bool IsExist(Expression<Func<TEntity, bool>> whereExpression)
        {
            return _context.Queryable<TEntity>().Where(whereExpression).Any();
        }
        #endregion

        #region Add(添加)
        public void Add(TEntity instance,string tablename = null)
        {
            if (tablename == null)
            {
                this._context.Insertable(instance).ExecuteCommand();

                return;
            }

            this._context.Insertable(instance).AS(tablename).ExecuteCommand();

        }
        #endregion

        #region Add(批量添加实体)
        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entities">实体集合</param>
        public void Add(List<TEntity> entities,string tableName = null)
        {
            if(tableName == null) {

                this._context.Insertable(entities).ExecuteCommand();

                return;
            }

            this._context.Insertable(entities).AS(tableName).ExecuteCommand();
            
        }
        #endregion

        #region Update(单个的修改)
        public void Update(TEntity instance)
        {
            this._context.Updateable(instance).ExecuteCommand();
        }
        #endregion

        #region Update(修改一组对象)
        /// <summary>
        /// 修改一组对象
        /// </summary>
        /// <param name="updateObjs"></param>
        public void Update(List<TEntity> updateObjs)
        {
            this._context.Updateable(updateObjs).ExecuteCommand();
        }
        #endregion

        #region Update(按字段批量修改一组对象)
        public void Update(List<TEntity> updateObjs, Expression<Func<TEntity, object>> columns)
        {
            this._context.Updateable(updateObjs).UpdateColumns(columns).ExecuteCommand();
        }
        #endregion

        #region Update(按字段批量修改一组对象)
        public void Update(IEnumerable<TEntity> updateObjs, Expression<Func<TEntity, object>> columns)
        {
            this._context.Updateable(updateObjs.ToList()).UpdateColumns(columns).ExecuteCommand();
        }
        #endregion 

        #region Update(按字段修改，满足条件的数据，批量修改的补充)
        /// <summary>
        /// 按字段修改，满足条件的数据，批量修改的补充。
        /// 例子：Update(it => new WorkProcess { Remark = "测试批量修改",SystemType = 0 },p => p.WorkProcessId ==Guid.Parse("7BDDBBD3-B1CD-4C25-93BA-D7BF22032108"));
        /// </summary>
        /// <param name="columns">要修改的列</param>
        /// <param name="whereExpression">要修改的条件</param>
        public void Update(Expression<Func<TEntity, TEntity>> columns, Expression<Func<TEntity, bool>> whereExpression)
        {
            this._context.Updateable<TEntity>().UpdateColumns(columns).Where(whereExpression).ExecuteCommand();
        }
        #endregion

        #region Delete(根据表达式删除)
        /// <summary>
        /// 根据表达式删除
        /// </summary>
        /// <param name="whereExpression"></param>
        public void Delete(Expression<Func<TEntity, bool>> whereExpression)
        {
            this._context.Deleteable<TEntity>().Where(whereExpression).ExecuteCommand();
        }
        #endregion

        #region DeleteByIds(根据一组ID删除)
        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteByIds(dynamic[] ids)
        {
            this._context.Deleteable<TEntity>().In(ids).ExecuteCommand();
        }
        #endregion

        #region Queryable(得到一个更加灵活的查询对象)
        /// <summary>
        /// 得到一个更加灵活的查询对象
        /// </summary>
        /// <returns></returns>
        public ISugarQueryable<TEntity> Queryable()
        {
            return this._context.Queryable<TEntity>().With(SqlWith.NoLock);
        }
        #endregion




    }
}
