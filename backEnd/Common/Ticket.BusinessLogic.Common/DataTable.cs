using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.BusinessLogic.TicketService
{

    public class DataTable
    {
        public DataTable() : this("")
        {


        }
        public DataTable(string name)
        {
            _rows = new List<DataRow>();
            _columns = new List<DataColumn>();
        }
        private List<DataRow> _rows;

        public List<DataRow> Rows
        {
            get { return _rows; }
            set { _rows = value; }
        }

        private List<DataColumn> _columns;

        public List<DataColumn> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        public static DataTable FromDataReader(DbDataReader reader)
        {
            DataTable table = new DataTable();
            var schema = reader.GetColumnSchema();
            foreach (var column in schema)
            {
                DataColumn dataColumn = new DataColumn(column);
                table.Columns.Add(dataColumn);
            }
            while (reader.Read())
            {
                DataRow dr = table.AddNewRow();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dr[i] = reader[i];
                }
            }
            return table;
        }
        public DataRow AddNewRow()
        {
            DataRow newRow = new DataRow(this);
            this.Rows.Add(newRow);
            return newRow;
        }
        public int GetColumnIndex(string columnName)
        {
            DataColumn column = this.Columns.FirstOrDefault(i => i.ColumnName == columnName);
            int index = this.Columns.IndexOf(column);
            return index;
        }

    }

    public sealed class DataRow
    {
        public DataTable _dataTable;
        public DataTable Table { get; }
        public object[] ItemArray { get; set; }

        public DataRow(DataTable dataTable)
        {
            _dataTable = dataTable;
            _data = new object[_dataTable.Columns.Count];

        }
        public object[] _data;
        public object this[string columnName]
        {
            get
            {
                int index = _dataTable.GetColumnIndex(columnName);
                return this[index];
            }
            set
            {
                int index = _dataTable.GetColumnIndex(columnName);
                this[index] = value;
            }
        }

        public object this[int columnIndex]
        {
            get
            {
                return _data[columnIndex];
            }
            set
            {
                _data[columnIndex] = value;
            }
        }

        public int FieldCount
        {
            get
            {
                return _data.Length;
            }
        }
    }

    public class DataColumn : DbColumn
    {
        public DataColumn()
        {

        }

        public DataColumn(string name, Type type)
        {
        }

        public DataColumn(DbColumn dbColumn)
        {
            this.AllowDBNull = dbColumn.AllowDBNull;
            this.BaseCatalogName = dbColumn.BaseCatalogName;
            this.BaseColumnName = dbColumn.BaseColumnName;
            this.BaseSchemaName = dbColumn.BaseSchemaName;
            this.BaseServerName = dbColumn.BaseServerName;
            this.BaseTableName = dbColumn.BaseTableName;
            this.ColumnName = dbColumn.ColumnName;
            this.ColumnOrdinal = dbColumn.ColumnOrdinal;
            this.ColumnSize = dbColumn.ColumnSize;
            this.DataTypeName = dbColumn.DataTypeName;
            this.IsAliased = this.IsAliased;
            this.IsAutoIncrement = this.IsAutoIncrement;
            this.IsExpression = this.IsExpression;
            this.IsHidden = this.IsHidden;
            this.IsIdentity = this.IsIdentity;
            this.IsKey = this.IsKey;
            this.IsLong = this.IsLong;
            this.IsReadOnly = this.IsReadOnly;
            this.IsUnique = this.IsUnique;
            this.NumericPrecision = this.NumericPrecision;
            this.NumericScale = this.NumericScale;
            this.UdtAssemblyQualifiedName = this.UdtAssemblyQualifiedName;


        }
    }
    public class SqlDataAdapter
    {

    }
}

