using Dapper;
using Ticket.Base;
using SimpleStack.Orm.PostgreSQL;
using System.Collections.Generic;
using System.Reflection;

namespace Ticket.DataAccess.Common
{
    public class PostgresCustomDialectProvider : PostgreSQLDialectProvider
    {
        public override CommandDefinition ToInsertRowStatement<T>(T objWithProperties, ICollection<string> insertFields = null)
        // where T : new()
        {
            var tableAttribute = this.GetTableAttribute<T>(objWithProperties);

            string seqQuery = "";
            if (tableAttribute != null && !string.IsNullOrEmpty(tableAttribute.SequenceName))
            {
                seqQuery = "; select currval('" + tableAttribute.SequenceName + "');";
            }

            var baseDefination = base.ToInsertRowStatement(objWithProperties, insertFields);

            string baseSql = baseDefination.CommandText;
            baseSql = baseSql + seqQuery;

            return new CommandDefinition(baseSql, baseDefination.Parameters);
        }


        protected TableWithSequenceAttribute GetTableAttribute<T>(T objWithProperties)
        {
            var type = objWithProperties.GetType().GetTypeInfo();
            var tableAttribute = type.GetCustomAttribute<TableWithSequenceAttribute>();
            return tableAttribute;
        }
    }

}
