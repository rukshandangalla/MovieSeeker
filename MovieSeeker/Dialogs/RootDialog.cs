using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using MovieSeeker.Data.Models;
using MovieSeeker.Data.Service;
using MovieSeeker.Util;
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
        Movie selectedMovie;

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

            context.Call(new MovieSeekDialog(), ResumeAfterMovieSeekDialog);
        }

        private async Task ResumeAfterMovieSeekDialog(IDialogContext context, IAwaitable<object> result)
        {
            await ProcessQuerry(context, result);
        }

        private async Task ProcessQuerry(IDialogContext context, IAwaitable<object> result)
        {
            var response = await result as Activity;

            await context.PostAsync("On it. Let me check");

            //get movie results
            movieList = await MovieInfoService.Instance.GetAllMovies();

            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetMovieCardsAttachments(movieList);

            await context.PostAsync(reply);

            await context.PostAsync("You can select one movie to watch trailer or locate available theaters.");

            context.Wait(ProcessSelectedMovie);
        }

        private async Task ProcessSelectedMovie(IDialogContext context, IAwaitable<object> result)
        {
            var response = await result as Activity;

            selectedMovie = movieList.FirstOrDefault(i => i.Id.ToString() == response.Text);

            if (selectedMovie != null)
            {
                PromptDialog.Choice(context, ResumeAfterChoice, new List<string> { "Watch trailer", "Available Theaters" }, "What you want to do?");
            }
            else
            {
                await context.PostAsync("I don't understand");
                context.Wait(ProcessSelectedMovie);
            }
        }

        private async Task ResumeAfterChoice(IDialogContext context, IAwaitable<string> result)
        {
            var response = await result as string;

            if (response.Equals("Watch trailer"))
            {
                await context.PostAsync("let me find the trailer for you");
                await context.PostAsync(selectedMovie.Trailer);
            }
            else if (response.Equals("Available Theaters"))
            {
                await context.PostAsync("let me find available theaters");

                var reply = context.MakeMessage();
                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                reply.Attachments = GetTheaterCardsAttachments(selectedMovie.Theaters);

                await context.PostAsync(reply);
            }
            else
            {
                await context.PostAsync("sorry I don't understand!");
            }

            context.Wait(MessageReceivedAsync);
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

        private IList<Attachment> GetMovieCardsAttachments(List<Movie> theaters)
        {
            List<Attachment> retVal = new List<Attachment>();

            foreach (var movie in movieList)
            {
                retVal.Add(MovieSeekerUtilities.GetHeroCard(movie.Name, movie.Genre, $"Cast: {movie.Cast}", new CardImage(url: movie.Poster), new CardAction(ActionTypes.PostBack, movie.Name, value: movie.Id.ToString())));
            }

            return retVal;
        }

        private IList<Attachment> GetTheaterCardsAttachments(List<Theater> theaters)
        {
            List<Attachment> retVal = new List<Attachment>();

            foreach (var theater in theaters)
            {
                retVal.Add(MovieSeekerUtilities.GetHeroCard(theater.Name, theater.Location, selectedMovie.Name, new CardImage(url: theater.Cordinates), new CardAction(ActionTypes.OpenUrl, "Location", value: theater.Map)));
            }

            return retVal;
        }
    }
}