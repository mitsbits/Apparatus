namespace Borg.Framework.SQLServer.Broadcast
{
    public class SqlBroadcastBusConfig
    {
        public static readonly string Key = typeof(SqlBroadcastBus).FullName.Replace(".", ":");
        public string SqlConnectionString { get; set; }
        public string SubcriberName { get; set; }
        public string[] QueuesToListen { get; set; }
        public int PollingIntervalInSeconds { get; set; } = 5;
    }
}