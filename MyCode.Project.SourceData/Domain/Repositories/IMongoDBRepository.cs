using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wolf.Project.SourceData.Domain.Model;

namespace Wolf.Project.Domain.Repositories
{
    public interface IMongoDBRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Add(T entity);


        /// <summary>
        /// 修改某字段
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        void Update(string id, string field, string value);

        /// <summary>
        /// 实体更新
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);

        /// <summary>
        /// 根据id查询一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T SelectFirst(string id);


        /// <summary>
        /// 根据条件查询一条数据
        /// </summary>
        /// <param name="express"></param>
        /// <returns></returns>
        T SelectFirst(Expression<Func<T, bool>> express);

        /// <summary>
        /// 找到多条数据
        /// </summary>
        /// <param name="express"></param>
        /// <returns></returns>
        List<T> SelectList(Expression<Func<T, bool>> express);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list"></param>
        void Add(List<T> list);

        /// <summary>
        /// 根据表达式返回一个灵活的对象
        /// </summary>
        /// <param name="express"></param>
        /// <returns></returns>
        IFindFluent<T, T> Queryable(Expression<Func<T, bool>> express);

        /// <summary>
        /// 根据Id批量删除
        /// </summary>
        /// <param name="list"></param>
        void Delete(List<ObjectId> list);

        /// <summary>
        /// 得到符合条件的数量
        /// </summary>
        /// <param name="express"></param>
        /// <returns></returns>
        long Count(Expression<Func<T, bool>> express);
    }

}
