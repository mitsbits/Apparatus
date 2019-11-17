


using Microsoft.AspNetCore.Components;

using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polemic.Pages
{
    public partial class Voting : IDisposable
    {
        private string value;
        private long forScore;
        private long againstScore;
        private string forMessages;
        private string againstMessages;
        private MarkupString forMarkup;
        private MarkupString againstMarkup;

        [Parameter]
        public string Key { get; set; }

        [Inject]
        protected TopicStore store { get; set; }

        [Inject]
        protected IJSRuntime JsRuntime { get; set; }
        [Inject]
        protected NavigationManager navManager { get; set; }
        protected Topic topic { get; set; } 

        private Mode DisplayMode => string.IsNullOrWhiteSpace(Key) ? Voting.Mode.Grid : Voting.Mode.Item;
        public Voting()
        {


        }
        private async Task SendMessageFor()
        {
            if (string.IsNullOrEmpty(value)) return;
            await topic.VoteFor(value);
            value = string.Empty;
        }
        private async Task SendMessageAgainst()
        {
            if (string.IsNullOrEmpty(value)) return;
            await topic.VoteAgainst(value);
            value = string.Empty;
        }






        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (DisplayMode == Mode.Item)
            {
                this.topic = this.store.Get(Key);
                this.topic.BallotCast += BallotCast;
                forMessages = string.Empty;
                againstMessages = string.Empty;
                forMarkup = new MarkupString(forMessages);
                againstMarkup = new MarkupString(againstMessages);
                if (topic.Ballots.Any())
                {
                    foreach (var ballot in topic.Ballots.OrderBy(x => x.Timestamp))
                    {
                        BallotCast(this, new BallotCastEventArgs(ballot));
                    }
                }
            }

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
        void Navigate(string title)
        {
            navManager.NavigateTo($"voting/{title}");
        }

        private void AddTopic()
        {
            if (string.IsNullOrEmpty(value)) return;
            store.Add(value);
            value = string.Empty;
        }

        enum Mode { Grid, Item }

        public void Dispose()
        {
            if (DisplayMode == Mode.Item && topic != null)
            {
                this.topic.BallotCast -= BallotCast;
            }
        }

        public Task Handle(BallotCastEventArgs notification, CancellationToken cancellationToken = default)
        {
            BallotCast(this, notification);
            return Task.CompletedTask;
        }
    }
}

