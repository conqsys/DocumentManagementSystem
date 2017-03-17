using Dapper;
using Microsoft.Extensions.Configuration;
using SimpleStack.Orm;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Ticket.DataAccess.Common
{
    public class DatabaseContext : IDisposable
    {
        public DatabaseContext(IConfiguration configuration)
        {
            this._connectionString = configuration.GetValue<string>("DBInfo:ConnectionString");
        }

        private string _connectionString;
        private OrmConnection _connection;
        public OrmConnection Connection
        {
            get
            {
                if (_connection == null)
                {

                    var factory = new OrmConnectionFactory(new PostgresCustomDialectProvider(), this._connectionString);
                    this._connection = factory.OpenConnection();
                }

                return _connection;
            }
        }
        public IDbTransaction _currentTransaction;
        public IDbTransaction CurrentTransaction
        {
            get
            {
                return this.Connection.Transaction;
            }
            private set
            {
                this._currentTransaction = value;
            }
        }

        /// <summary>
        /// this function will execute Insert Statement with id sequence
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="defination"></param>
        /// <returns></returns>
        public T Insert<T, TEntity>(TEntity entity)
        {
            var commandDefination = this.Connection.DialectProvider.ToInsertRowStatement(entity);
            var t = this.Connection.ExecuteScalar<T>(commandDefination);
            return t;
        }

        /// <summary>
        /// this function will execute Insert Statement with id sequence and async operation type
        /// </summary>
        /// <typeparam name="Task<T>"></typeparam>
        /// <param name="defination"></param>
        /// <returns></returns>
        public async Task<T> InsertAsync<T, TEntity>(TEntity entity)
        {
            var commandDefination = this.Connection.DialectProvider.ToInsertRowStatement(entity);
            var t = await this.Connection.ExecuteScalarAsync<T>(commandDefination);
            return t;
        }

        public IDbTransaction BeginTransaction()
        {
            this.CurrentTransaction = this.Connection.BeginTransaction();
            return this.CurrentTransaction;
        }

        public void CommitTransaction()
        {
            if (this.CurrentTransaction != null)
            {
                this.CurrentTransaction.Commit();
                this.CurrentTransaction = null;
            }
        }

        public void Dispose()
        {
            this.Connection.Close();
            this._connection = null;
        }
    }
}
