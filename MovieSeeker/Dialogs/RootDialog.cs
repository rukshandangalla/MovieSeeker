using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using MovieSeeker.Models;
using MovieSeeker.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MovieSeeker.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        List<Movie> movieList;

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

        private async Task ResumeAfterProfileDialog(IDialogContext context, IAwaitable<UserProfile> result)
        {
            var profile = await result as UserProfile;
            context.UserData.SetValue("profile", profile);

            await context.PostAsync($"Hi {profile.Name}, How can I help you today?");

            context.Wait(ProcessQuerry);
        }

        private async Task ProcessQuerry(IDialogContext context, IAwaitable<object> result)
        {
            var response = await result as Activity;

            if (response.Text.ToLower().Contains("find"))
            {
                await context.PostAsync("On it. Let me check");

                //get movie results
                movieList = await MovieInfoService.Instance.GetAllMovies();

                var reply = context.MakeMessage();
                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                reply.Attachments = GetCardsAttachments(movieList);

                await context.PostAsync(reply);

                context.Wait(ProcessSelectedMovie);
            }
            else
            {
                await context.PostAsync($"sorry I don't understand!");
                context.Wait(ProcessQuerry);
            }
        }

        private async Task ProcessSelectedMovie(IDialogContext context, IAwaitable<object> result)
        {
            var response = await result as Activity;

            var selectedMovie = movieList.FirstOrDefault(i => i.Id.ToString() == response.Text);

            await context.PostAsync("Let me show you the trailer");

            var message = context.MakeMessage();
            var attachment = GetSelectedTrailer(selectedMovie);
            message.Attachments.Add(attachment);

            await context.PostAsync(message);

            context.Wait(ProcessSelectedMovie);
        }

        private Attachment GetSelectedTrailer(Movie selectedMovie)
        {
            var videoCard = new VideoCard
            {
                Title = selectedMovie.Name,
                Subtitle = selectedMovie.Genre,
                Text = $"Cast: {selectedMovie.Cast}",
                Image = new ThumbnailUrl
                {
                    Url = selectedMovie.Poster
                },
                Media = new List<MediaUrl>
                {
                    new MediaUrl()
                    {
                        Url = selectedMovie.Trailer
                    }
                }
            };

            return videoCard.ToAttachment();
        }

        private IList<Attachment> GetCardsAttachments(List<Movie> movieList)
        {
            List<Attachment> retVal = new List<Attachment>();

            foreach (var movie in movieList)
            {
                retVal.Add(GetHeroCard(movie.Name, movie.Genre, $"Cast: {movie.Cast}", new CardImage(url: movie.Poster), new CardAction(ActionTypes.PostBack, "Watch Trailer", value: movie.Id.ToString())));
            }

            return retVal;
        }

        private static Attachment GetHeroCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction }
            };

            return heroCard.ToAttachment();
        }
    }
}