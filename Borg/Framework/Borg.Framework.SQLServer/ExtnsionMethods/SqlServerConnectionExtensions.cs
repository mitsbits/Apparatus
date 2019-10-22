using Borg.Infrastructure.Core;
using MediatR;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Borg
{
    public static class SqlServerConnectionExtensions

    {



        const string sqlFindDBQuery = "SELECT * FROM master.dbo.sysdatabases where name = @dbName";
        /// <summary>
        /// Run a batch script against sql server
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="sqltext"></param>
        /// <param name="relacements"></param>
        /// <param name="disposedepedencies"></param>
        /// <returns ><see cref="Task"/></returns>
        /// <exception cref="ArgumenNullException"></exception>  
        /// <exception cref="TimeoutException" >the commands run later than the timeout </exception>
        /// <exception cref="SqlException "></exception>
        public static async Task RunBatch(this SqlConnection sqlConnection, string sqltext, IDictionary<string, string> relacements, bool disposedepedencies = false)
        {
            sqlConnection = Preconditions.NotNull(sqlConnection, nameof(sqlConnection));
            if (sqlConnection.State == System.Data.ConnectionState.Closed) await sqlConnection.OpenAsync();

            foreach (var replacement in relacements)
            {
                sqltext = sqltext.Replace(replacement.Key, replacement.Value);
            }
 
            var server = new Server(new ServerConnection(sqlConnection));
            server.ConnectionContext.ExecuteNonQuery(sqltext);
            server.ConnectionContext.Disconnect();
            if (disposedepedencies)
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }
        /// <summary>
        /// Run a batch script against sql server
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="sqltext"></param>
        /// <param name="relacements"></param>
        /// <param name="disposedepedencies"></param>
        /// <returns><see cref="Task"/></returns>
        /// <exception cref="ArgumenNullException"></exception>  
        /// <exception cref="TimeoutException" >the commands run later than the timeout </exception>
        /// <exception cref="SqlException "></exception>
        public static async Task RunBatch(this SqlConnection sqlConnection, Stream sqltext, IDictionary<string, string> relacements, bool disposedepedencies = false)
        {
            sqlConnection = Preconditions.NotNull(sqlConnection, nameof(sqlConnection));
            var payload = new StringBuilder();
            sqltext.Position = 0;
            using (var reader = new StreamReader(sqltext))
            {
                payload.Append(await reader.ReadToEndAsync());
            }
            sqltext.Dispose();
            var sql = payload.ToString();

            await sqlConnection.RunBatch(sql, relacements, disposedepedencies);

        }

        private static Task CreateDbIfNotExists(SqlConnection sqlConnection)
        {
            var dbName = sqlConnection.Database;

            sqlConnection.ChangeDatabase("master");
            using (SqlCommand sqlCmd = new SqlCommand(sqlFindDBQuery, sqlConnection))
            {
                sqlCmd.Parameters.AddWithValue("@dbName", dbName);
                int exists = sqlCmd.ExecuteNonQuery();

                if (exists <= 0)
                {
                    string script = $"CREATE DATABASE [{dbName}] CONTAINMENT = NONE";
                    sqlCmd.CommandText = script;
                    sqlCmd.ExecuteNonQuery();

                }
                return Unit.Task;
            }
        }
    }
}