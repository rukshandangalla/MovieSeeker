using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MovieSeeker.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            await context.PostAsync("MovieSeeker here. I can help you to find movies that are screening in local theaters.");

            await context.Forward(new UserProfileDialog(), ResumeAfterProfileDialog, activity, CancellationToken.None);
        }

        private async Task ResumeAfterProfileDialog(IDialogContext context, IAwaitable<object> result)
        {
            var response = await result as Activity;
            await context.PostAsync("How can I help you today?");

            context.Wait(ResumeAfterProfileDialog);
        }
    }
}