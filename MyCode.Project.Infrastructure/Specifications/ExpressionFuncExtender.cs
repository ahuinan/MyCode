using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace MyCode.Project.Infrastructure
{
    /// <summary>
    /// Represents the extender for Expression[Func[T, bool]] type.
    /// This is part of the solution which solves
    /// the expression parameter problem when going to Entity Framework by using
    /// Apworks specifications. For more information about this solution please
    /// </summary>
    public static class ExpressionFuncExtender
    {
        #region Private Methods
        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Combines two given expressions by using the AND semantics.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="first">The first part of the expression.</param>
        /// <param name="second">The second part of the expression.</param>
        /// <returns>The combined expression.</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }
        /// <summary>
        /// Combines two given expressions by using the OR semantics.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="first">The first part of the expression.</param>
        /// <param name="second">The second part of the expression.</param>
        /// <returns>The combined expression.</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }
        /// <summary>
        /// 创建一个Lambda表达式
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="prop">实体属性</param>
        /// <param name="value">属性值</param>
        /// <param name="valueType">属性值的数据类型</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateLambda<T>(string prop, object value, Type valueType = null)
        {
            var parameter = Expression.Parameter(typeof(T), "t");
            Expression property = null;
            string[] props = prop.Split('.');
            for (int i = 0; i < props.Length; i++)
            {
                if (property == null)
                {
                    property = Expression.Property(parameter, props[i]);
                }
                else
                {
                    property = Expression.Property(property, props[i]);
                }
            }
            var body = Expression.Equal(property, valueType != null ? Expression.Constant(value, valueType) : Expression.Constant(value));
            var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
            return lambda;
        }

        /// <summary>
        /// 创建一个Lambda表达式
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="prop">实体属性</param>
        /// <param name="value">属性值</param>
        /// <param name="valueType">属性值的数据类型</param>
        /// <returns></returns>
        public static Expression<Func<object, bool>> CreateLambda(Type t, string prop, object value, Type valueType = null)
        {
            var parameter = Expression.Parameter(t, "t");
            Expression property = null;
            string[] props = prop.Split('.');
            for (int i = 0; i < props.Length; i++)
			{
                if (property == null)
                {
                    property = Expression.Property(parameter, props[i]);
                }
                else
                {
                    property = Expression.Property(property, props[i]);
                }
			}
            var body = Expression.Equal(property, valueType != null ? Expression.Constant(value, valueType) : Expression.Constant(value));
            var lambda = Expression.Lambda<Func<object, bool>>(body, Expression.Parameter(typeof(object), "t"));
            return lambda;
        }
        #endregion
    }
}
