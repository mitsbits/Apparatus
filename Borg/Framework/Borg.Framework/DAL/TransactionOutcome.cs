namespace Borg.Framework.DAL
{
    public enum TransactionOutcome
    {
        Success = 1,
        Failure = -1
    }

    public enum DmlOperation
    {
        Query,
        Create,
        Update,
        Delete
    }
}