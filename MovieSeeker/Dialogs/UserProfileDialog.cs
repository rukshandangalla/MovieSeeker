using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace MovieSeeker.Dialogs
{
    [Serializable]
    public class UserProfileDialog : IDialog<object>
    {
        string userName;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var response = await result as Activity;

            context.UserData.TryGetValue("Name", out userName);

            if (string.IsNullOrEmpty(userName))
            {
                await context.PostAsync("What is your name?");
                context.Wait(NameEntered);
            }
            else
            {
                await context.PostAsync($"Hello, {userName} welcomeback!");
                context.Done(userName);
            }
        }

        private async Task NameEntered(IDialogContext context, IAwaitable<object> result)
        {
            var response = await result as Activity;

            await context.PostAsync($"Hello, {response.Text}");
            context.UserData.SetValue("Name", response.Text);

            context.Done(response.Text);
        }
    }
}