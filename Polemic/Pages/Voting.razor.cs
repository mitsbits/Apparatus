using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polemic.Pages
{
    public partial class Voting : IDisposable
    {
        private long forScore;
        private long againstScore;
        private string forMessages;
        private string againstMessages;
        private MarkupString forMarkup;
        private MarkupString againstMarkup;
        [Inject]
        protected Topic topic { get; set; }
        public Voting()
        {


        }
        private async Task SendMessageFor()
        {
            await topic.VoteFor(value);
            value = string.Empty;
        }
        private async Task SendMessageAgainst()
        {
            await topic.VoteAgainst(value);
            value = string.Empty;
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.topic.BallotCast += BallotCast;
        }



        private void BallotCast(object sender, BallotCastEventArgs eventArgs)
        {
            forScore = topic.Score(Stance.For);
            againstScore = topic.Score(Stance.Against);
            forMessages = ProduceHtml(eventArgs.Ballot) + forMessages;
            againstMessages = ProduceHtml(eventArgs.Ballot) + againstMessages;
            forMarkup = new MarkupString(forMessages);
            againstMarkup = new MarkupString(againstMessages);
        }

        private string ProduceHtml(Ballot ballot)
        {
            var builder = new StringBuilder();
            var background = ballot.Stance == Stance.For ? "black" : "white";
            var foreground = ballot.Stance == Stance.For ? "white" : "black";

            builder.Append($"<p style='background-color:{background}; color:{foreground}>");
            builder.Append($"<span><b>{ballot.Timestamp.ToString("d/M HH:mm")}</b></span>");
            builder.Append($"{ballot.Message}</p>");
            return builder.ToString();
        }

        public void Dispose()
        {
            this.topic.BallotCast -= BallotCast;
        }
    }
}
