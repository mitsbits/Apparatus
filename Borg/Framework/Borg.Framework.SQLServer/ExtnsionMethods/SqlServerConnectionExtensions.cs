using Borg.Infrastructure.Core;
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
    {/// <summary>
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
            foreach (var replacement in relacements)
            {
                sqltext = sqltext.Replace(replacement.Key, replacement.Value);
            }
            if (sqlConnection.State == System.Data.ConnectionState.Closed) await sqlConnection.OpenAsync();
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
            var reader = new StreamReader(sqltext);
            payload.Append(await reader.ReadToEndAsync());
            var sql = payload.ToString();
            foreach (var replacement in relacements)
            {
                sql = sql.Replace(replacement.Key, replacement.Value);
            }
            if (sqlConnection.State == System.Data.ConnectionState.Closed) await sqlConnection.OpenAsync();
            var server = new Server(new ServerConnection(sqlConnection));
            server.ConnectionContext.ExecuteNonQuery(sql);
            server.ConnectionContext.Disconnect();
            if (disposedepedencies)
            {
                sqltext.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }
    }
}