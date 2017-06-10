using Microsoft.Bot.Builder.Dialogs;
using MovieSeeker.Models;
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
            EnsureProfileName(context);
            return Task.CompletedTask;
        }

        private void EnsureProfileName(IDialogContext context)
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
        }

        private void EnsureLocation(IDialogContext context)
        {
            if (string.IsNullOrEmpty(_profile.Location))
            {
                PromptDialog.Text(context, NameEntered, @"Where are you from?");
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
    }
}