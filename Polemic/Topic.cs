using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Polemic
{
    public delegate void BallotCastEventHandler(object sender, BallotCastEventArgs e);
    public class Topic
    {

        public event BallotCastEventHandler BallotCast;
        public Topic(string title)
        {
            Title = title;
        }
        private Lazy<ConcurrentBag<Ballot>> _ballotBox = new Lazy<ConcurrentBag<Ballot>>(() => new ConcurrentBag<Ballot>());
        ConcurrentBag<Ballot> Ballots => _ballotBox.Value;
        public string Title { get;  }
        public Task VoteFor(string message)
        {
            var ballot = new Ballot(Stance.For, message);
            Ballots.Add(ballot);
            OnBallotCast(new BallotCastEventArgs(ballot));
            return Task.CompletedTask;
        }
        public Task VoteAgainst(string message)
        {
            var ballot = new Ballot(Stance.Against, message);
            Ballots.Add(ballot);
            OnBallotCast(new BallotCastEventArgs(ballot));
            return Task.CompletedTask;
        }

        public long Score(Stance stance)
        {
            return Ballots.Where(x => x.Stance == stance).Sum(x => x.Message.Length);
        }

        private  void OnBallotCast(BallotCastEventArgs e)
        {
            BallotCastEventHandler handler = BallotCast;
            handler?.Invoke(this, e);
        }
    }

    public class Ballot
    {
        public Ballot(Stance stance, string message)
        {
            Stance = stance;
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

    public class BallotCastEventArgs :EventArgs
    {
        public BallotCastEventArgs(Ballot ballot)
        {
            Ballot = ballot;
        }
        public Ballot Ballot { get; }
    }
}
