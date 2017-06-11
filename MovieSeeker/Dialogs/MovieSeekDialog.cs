using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Threading.Tasks;

namespace MovieSeeker.Dialogs
{
    [LuisModel("c4540e48-3e7d-4a42-93c0-165686473103", "6c5075ae11ac40ddbfbc98fd4671bb7a")]
    [Serializable]
    public class MovieSeekDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sorry, I don't understand");
            context.Wait(MessageReceived);
        }

        [LuisIntent("findmovie")]
        public Task FindMovies(IDialogContext context, LuisResult result)
        {
            context.Done(result);
            return Task.CompletedTask;
        }
    }
}