using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wolf.Project.Domain.Config;
using Wolf.Project.Domain.Repositories;
using Wolf.Project.SourceData.Domain.Model;

namespace Wolf.Project.SourceData.Repositories.Common
{
    public class MongoRepository<T> : IMongoDBRepository<T> where T: BaseEntity
    {
      

        private IMongoCollection<T> _collection = null;

        public MongoRepository(MyMongoClient client)
        {
          
            this._collection = client.GetDatabase(SystemConfig.MongoDBName).GetCollection<T>(typeof(T).Name);
        }

        #region Add(插入)
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Add(T entity)
        {
            
            this._collection.InsertOne(entity);
            return entity;
        }
        #endregion

        #region Update(修改某字段)
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public void Update(string id, string field, string value)
        {
            var filter = Builders<T>.Filter.Eq("Id", ObjectId.Parse(id));
            var updated = Builders<T>.Update.Set(field, value);
            UpdateResult result = this._collection.UpdateOneAsync(filter, updated).Result;
        }
        #endregion


        #region Update(实体更新)
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {

            //修改条件
            FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", entity.id);

            //要修改的字段
            var list = new List<UpdateDefinition<T>>();

            foreach (var item in entity.GetType().GetProperties())
            {
                if (item.Name.ToLower() == "id") continue;

                list.Add(Builders<T>.Update.Set(item.Name, item.GetValue(entity)));
            }
            var updatefilter = Builders<T>.Update.Combine(list);

            var result = _collection.UpdateOne(filter, updatefilter);

        }
        #endregion

        #region Delete(删除)
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            var filter = Builders<T>.Filter.Eq("Id", entity.id);
            this._collection.DeleteOne(filter);
        }
        #endregion

        #region SelectFirst(按ID查询一条数据)
        /// <summary>
        /// 根据id查询一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T SelectFirst(string id)
        {
            return this._collection.Find(a => a.id == ObjectId.Parse(id)).FirstOrDefault();
        }
        #endregion

        #region SelectFirst(根据条件查询一条数据)
        /// <summary>
        /// 根据条件查询一条数据
        /// </summary>
        /// <param name="express"></param>
        /// <returns></returns>
        public T SelectFirst(Expression<Func<T, bool>> express)
        {
            return this._collection.Find(express).FirstOrDefault();
        }
        #endregion

        #region SelectList(找到多条数据)
        public List<T> SelectList(Expression<Func<T, bool>> express)
        {
            return this._collection.Find(express).ToList();
        }
        #endregion

        #region Queryable(根据表达式返回一个灵活的对象)
        public IFindFluent<T, T> Queryable(Expression<Func<T, bool>> express)
        {
            return this._collection.Find(express);
        }
        #endregion

        #region Count(得到符合条件的数量)
        public long Count(Expression<Func<T, bool>> express)
        {
            return this._collection.CountDocuments(express);
        }
        #endregion

        #region Add(批量添加)
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list"></param>
        public void Add(List<T> list)
        {
            this._collection.InsertMany(list);
        }
        #endregion

        
        #region Delete(根据id删除数据)
        /// <summary>
        /// 根据Id批量删除
        /// </summary>
        public void Delete(List<ObjectId> list)
        {
            var filter = Builders<T>.Filter.In("Id", list);
            this._collection.DeleteManyAsync(filter);
        }
        #endregion

    }
}
