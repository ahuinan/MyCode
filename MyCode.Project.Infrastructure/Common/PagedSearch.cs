using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Common
{
	public class PagedSearch
	{
		/// <summary>
		/// 每页显示行数
		/// </summary>
		public int PageSize { get; set; }

		/// <summary>
		/// 页索引，即从第几页，从1开始
		/// </summary>
		public int Page { get; set; }

		/// <summary>
		/// 初始化一个<see cref="PagedSearch"/>类型的实例
		/// </summary>
		public PagedSearch() : this(15, 1)
		{
		}

		/// <summary>
		/// 初始化一个<see cref="PagedSearch"/>类型的实例
		/// </summary>
		/// <param name="pageSize">每页显示行数</param>
		/// <param name="page">页索引</param>
		public PagedSearch(int pageSize, int page)
		{
			this.PageSize = pageSize;
			this.Page = page;
		}

	}
	/// <summary>
	/// 分页查询类，带分页信息
	/// </summary>
	/// <typeparam name="TEntity">查询条件</typeparam>
	public class PagedSearch<TEntity> : PagedSearch where TEntity : new()
	{
		/// <summary>
		/// 查询条件
		/// </summary>
		public TEntity Condition { get; set; }

		/// <summary>
		/// 初始化一个<see cref="PagedSearch{TEntity}"/>类型的实例
		/// </summary>
		public PagedSearch() : this(15, 1, new TEntity())
		{
		}

		/// <summary>
		/// 初始化一个<see cref="PagedSearch{TEntity}"/>类型的实例
		/// </summary>
		/// <param name="pageSize">每页显示行数</param>
		/// <param name="page">页索引</param>
		/// <param name="condition">查询条件</param>
		public PagedSearch(int pageSize, int page, TEntity condition) : base(pageSize, page)
		{
			this.Condition = condition;
		}
	}
}
