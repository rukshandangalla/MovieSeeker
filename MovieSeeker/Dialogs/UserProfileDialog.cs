using Microsoft.Bot.Builder.Dialogs;
using MovieSeeker.Data.Models;
using System;
using System.Threading.Tasks;

namespace MovieSeeker.Dialogs
{
    [Serializable]
    public class UserProfileDialog : IDialog<UserProfile>
    {
        UserProfile _profile;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(EnsureProfileName);
            return Task.CompletedTask;
        }

        private Task EnsureProfileName(IDialogContext context, IAwaitable<object> result)
        {
            if (!context.UserData.TryGetValue("profile", out _profile))
            {
                _profile = new UserProfile();
            }

            if (string.IsNullOrEmpty(_profile.Name))
            {
                PromptDialog.Text(context, NameEntered, @"What is your name?");
            }
            else
            {
                EnsureLocation(context);
            }

            return Task.CompletedTask;
        }

        private void EnsureLocation(IDialogContext context)
        {
            if (string.IsNullOrEmpty(_profile.Location))
            {
                PromptDialog.Text(context, LocationEntered, @"Where are you from?");
            }
            else
            {
                context.Done(_profile);
            }
        }

        private async Task NameEntered(IDialogContext context, IAwaitable<string> result)
        {
            _profile.Name = await result;
            EnsureLocation(context);
        }

        private async Task LocationEntered(IDialogContext context, IAwaitable<string> result)
        {
            _profile.Location = await result;
            context.Done(_profile);
        }
    }
}