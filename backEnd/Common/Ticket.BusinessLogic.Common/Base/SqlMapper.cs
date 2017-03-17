using SimpleStack.Orm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using SimpleStack.Orm.Attributes;
using SimpleStack.Orm.Expressions;
using System.Linq.Expressions;
using Ticket.Base;

namespace Dapper
{
    public static class SqlMapper
    {

        public static async Task<PaginationQuery<T>> QueryPagination<T>(this OrmConnection cnn,
            string countExpression,
            string whereExpression,
            int? startPageNo = null,
            int? endPageNo = null,
            int? pageSize = null,
            object parameters = null,
            bool? runCount = null)
        {

            ModelDefinition modelDef = ModelDefinition<T>.Definition;
            string tableName = cnn.DialectProvider.GetQuotedTableName(modelDef);

            string countQuery = countExpression;
            if (string.IsNullOrEmpty(countQuery))
            {
                countQuery = "SELECT COUNT(*) FROM " + tableName;
            }

            var data = (await cnn.SelectAsync<T>());
            long countValue = 0;
            countValue = await cnn.QueryFirstAsync<long>(countQuery);

            return new PaginationQuery<T>()
            {
                Data = data,
                TotalRecords = countValue
            };
        }

       



    


        public static async Task<PaginationQuery<T>> QueryPagination<T>(this OrmConnection cnn,
            SqlExpressionVisitor<T> expressionVisitor,
            string selectExpression = "",
            string whereExpression = "",
            string countExpression = "",
            string groupByExpression = "",
            string havingExpression = "",
            string orderByExpression = "",
            int? startPageNo = null,
            int? endPageNo = null,
            int? pageSize = null,
            object parameters = null,
            bool runCount = false)
        {
            if (expressionVisitor == null)
                expressionVisitor = cnn.DialectProvider.ExpressionVisitor<T>();

            if (string.IsNullOrEmpty(selectExpression))
                selectExpression = expressionVisitor.SelectExpression;

            if (string.IsNullOrEmpty(whereExpression))
                whereExpression = expressionVisitor.WhereExpression;

            if (string.IsNullOrEmpty(havingExpression))
                havingExpression = expressionVisitor.HavingExpression;

            if (string.IsNullOrEmpty(groupByExpression))
                groupByExpression = expressionVisitor.GroupByExpression;


            if (string.IsNullOrEmpty(orderByExpression))
                orderByExpression = expressionVisitor.OrderByExpression;



            ModelDefinition modelDef = ModelDefinition<T>.Definition;
            string tableName = cnn.DialectProvider.GetQuotedTableName(modelDef);

            string countQuery = countExpression;
            if (string.IsNullOrEmpty(countQuery))
            {
                countQuery = "SELECT COUNT(*) FROM " + tableName;
            }

            var sql = selectExpression + " " + whereExpression + " " + " order by id desc";
            var data = await cnn.QueryAsync<T>(sql, parameters);

           // var data = (await cnn.SelectAsync<T>());
            long countValue = 0;
            countValue = await cnn.QueryFirstAsync<long>(countQuery);

            return new PaginationQuery<T>()
            {
                Data = data
                ,
                TotalRecords = countValue
            };
        }


     

    }

    public class PaginationQuery<T> : IPaginationQuery<T>
    {
        public IEnumerable<T> Data { get; set; }
        public long TotalRecords { get; set; }
    }
}
