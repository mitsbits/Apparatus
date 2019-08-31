using Borg.Framework.Cms.Contracts;
using Borg.Framework.DAL;

namespace Borg.Framework.Cms
{
    public class CmsOperationResult : ICmsOperationResult
    {
        private readonly TransactionOutcome _outcome;
        private readonly CmsError[] _usererrors;

        public CmsOperationResult(TransactionOutcome outcome, params CmsError[] usererrors)
        {
            _outcome = outcome;
            _usererrors = usererrors;
        }

        public bool Succeded => _outcome == TransactionOutcome.Success;

        public ICmsError[] Errors => _usererrors;
    }

    public class CmsOperationResult<TData> : CmsOperationResult, ICmsOperationResult<TData>
    {
        public CmsOperationResult(TransactionOutcome outcome, TData payload, params CmsError[] usererrors) : base(outcome, usererrors)
        {
            Payload = payload;
        }

        public TData Payload
        {
            get;
        }
    }
}