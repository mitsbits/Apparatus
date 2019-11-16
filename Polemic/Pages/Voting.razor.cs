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
            forMessages = ProduceHtml(eventArgs.Ballot, Stance.For) + forMessages;
            againstMessages = ProduceHtml(eventArgs.Ballot, Stance.Against) + againstMessages;
            forMarkup = new MarkupString(forMessages);
            againstMarkup = new MarkupString(againstMessages);
        }

        private string ProduceHtml(Ballot ballot, Stance stance)
        {
            string background;
            string foreground;

            if (ballot.Stance == Stance.Against)
            {
                if (stance == Stance.Against)
                {
                    background = "white";
                    foreground = "black";
                }
                else
                {
                    background = "black";
                    foreground = "black";
                }
            }
            else
            {
                if (stance == Stance.Against)
                {
                    background = "white";
                    foreground = "white";
                }
                else
                {
                    background = "black";
                    foreground = "white";
                }
            }
            var builder = new StringBuilder();

            builder.Append($"<div style='background-color:{background}; color:{foreground};' >");
            builder.Append($"<b>{ballot.Timestamp.ToString("d/M HH:mm")}</b> ");
            builder.Append($"{ballot.Message}</div>");
            return builder.ToString();
        }

        public void Dispose()
        {
            this.topic.BallotCast -= BallotCast;
        }
    }
}
