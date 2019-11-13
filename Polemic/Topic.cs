using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Polemic
{
    public class Topic
    {

        public Topic(string title)
        {
            Title = title;
        }
        private Lazy<ConcurrentBag<Ballot>> _ballotBox = new Lazy<ConcurrentBag<Ballot>>(() => new ConcurrentBag<Ballot>());
        ConcurrentBag<Ballot> Ballots => _ballotBox.Value;
        public string Title { get;  }
        public Task VoteFor(string message)
        {
            Ballots.Add(new Ballot(Stance.For, message));
            return Task.CompletedTask;
        }
        public Task VoteAginst(string message)
        {
            Ballots.Add(new Ballot(Stance.Against, message));
            return Task.CompletedTask;
        }

        public long Score(Stance stance)
        {
            return Ballots.Where(x => x.Stance == stance).Sum(x => x.Message.Length);
        }
    }

    public class Ballot
    {
        public Ballot(Stance stance, string message)
        {
            Stance = Stance;
            Message = message;
        }
        public Stance Stance { get; }
        public string Message { get; }
        public DateTimeOffset Timestamp { get; } = DateTimeOffset.Now;
    }

    public enum Stance
    {
        For,
        Against
    }
}
