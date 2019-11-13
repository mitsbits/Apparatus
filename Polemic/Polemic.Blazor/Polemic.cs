using Borg.Infrastructure.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Polemic.Blazor
{
    public interface IPolemicService
    {
        Task<IEnumerable<string>> Topics();
        Task<IPolemic> Topic(string topic);
    }
    public interface IPolemic
    {
        Task Vote(IVote vote, CancellationToken cancellationToken = default);
        string Title { get; }
        long Score(Stance stance);
    }

    public interface IVote
    {
        Stance Stance { get; }
        string User { get; }
        string Meesage { get; }
        DateTimeOffset Timestamp { get; }
    }

    public enum Stance
    {
        For,
        Against
    }

    public class Vote : IVote
    {
        public Vote(Stance stance, string user, string message)
        {
            Stance = Stance;
            User = user;
            Meesage = message;
        }
        public Stance Stance { get; }
        public string User { get; }
        public string Meesage { get; }
        public DateTimeOffset Timestamp { get; protected set; } = DateTimeOffset.UtcNow;
    }

    public abstract class PolemicBase : IPolemic
    {
        private Lazy<ConcurrentBag<IVote>> ballotΒox = new Lazy<ConcurrentBag<IVote>>(() => new ConcurrentBag<IVote>());
        ConcurrentBag<IVote> BallotΒox => ballotΒox.Value;
        public abstract string Title { get; }



        public long Score(Stance stance)
        {
            return BallotΒox.Where(x => x.Stance == stance).Sum(x => x.Meesage.Length);
        }

        public Task Vote(IVote vote, CancellationToken cancellationToken = default)
        {

            BallotΒox.Add(Preconditions.NotNull(vote, nameof(vote)));
            return Task.CompletedTask;
        }
    }

    public class Topic : PolemicBase
    {
        public Topic(string topic)
        {
            Title = Preconditions.NotEmpty(topic, nameof(topic));
        }
        public override string Title { get; }
    }

}

