using System;
using Npgsql;
using System.Collections;


namespace Ticket.DataAccess.Common
{
    public class TicketException : Exception
    {
        public virtual IValidationCodeResult ValidationCodeResult { get; set; }

        public TicketException(string message) : base(message)
        {

        }
    }
    public class EntityUpdateException : TicketException
    {

        //
        public override IValidationCodeResult ValidationCodeResult { get; set; }

        public new string Message { get; set; }
        public string MessageText { get; set; }


        public EntityUpdateException(Exception ex) : base(ex.Message)
        {

            Type type = ex.GetType();
            ValidationCodeResult = new EntityUpdateErrorResult();
            var entityUpdateErrorResult = (EntityUpdateErrorResult)ValidationCodeResult;
            if (type == typeof(PostgresException))
            {
                PostgresException dbEx = ex as PostgresException;
                entityUpdateErrorResult.Data = dbEx.Data;
                entityUpdateErrorResult.Code = dbEx.Code;
                entityUpdateErrorResult.ColumnName = dbEx.ColumnName;
                entityUpdateErrorResult.ConstraintName = dbEx.ConstraintName;
                entityUpdateErrorResult.DataTypeName = dbEx.DataTypeName;
                entityUpdateErrorResult.Detail = dbEx.Detail;
                entityUpdateErrorResult.File = dbEx.File;
                entityUpdateErrorResult.InternalQuery = dbEx.InternalQuery;
                this.Message = dbEx.Message;
                this.MessageText = dbEx.MessageText;
                entityUpdateErrorResult.Position = dbEx.Position;
                entityUpdateErrorResult.Routine = dbEx.Routine;
                entityUpdateErrorResult.SchemaName = dbEx.SchemaName;
                entityUpdateErrorResult.Severity = dbEx.Severity;
                entityUpdateErrorResult.SqlState = dbEx.SqlState;
                entityUpdateErrorResult.TableName = dbEx.TableName;
                entityUpdateErrorResult.Where = dbEx.Where;
            }
        }



    }

    public class EntityUpdateErrorResult : IValidationCodeResult
    {
        public string Code { get; set; }

        public string ColumnName { get; set; }

        public string ConstraintName { get; set; }
        public new IDictionary Data { get; set; }

        public string DataTypeName { get; set; }

        public string Detail { get; set; }

        public string File { get; set; }

        public string Hint { get; set; }

        public int InternalPosition { get; set; }

        public string InternalQuery { get; set; }

        public string Line { get; }

        public int Position { get; set; }

        public string Routine { get; set; }
        public string SchemaName { get; set; }

        public string Severity { get; set; }

        public string SqlState { get; set; }

        public NpgsqlStatement Statement { get; set; }

        public string TableName { get; set; }

        public string Where { get; set; }

        public EntityUpdateErrorResult()
        {

        }

    }
}
