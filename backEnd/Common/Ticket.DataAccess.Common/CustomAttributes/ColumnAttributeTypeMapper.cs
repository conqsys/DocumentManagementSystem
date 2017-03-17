using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ticket.DataAccess.Common
{

    public class ColumnAttributeTypeMapper<T> : FallBackTypeMapper
    {
        public ColumnAttributeTypeMapper()
            : base(new SqlMapper.ITypeMap[]
                    {
                        new CustomPropertyTypeMap(typeof(T),
                            (type, columnName) =>
                                type.GetProperties().FirstOrDefault(prop =>
                                    prop.GetCustomAttributes(false)
                                        .OfType<ColumnAttribute>()
                                        .Any(attribute => attribute.Name == columnName)
                            )
                        ),
                        new DefaultTypeMap(typeof(T))
                    })
        {
        }
    }
}
