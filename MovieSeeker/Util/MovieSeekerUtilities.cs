using Microsoft.Bot.Connector;
using System.Collections.Generic;

namespace MovieSeeker.Util
{
    public static class MovieSeekerUtilities
    {
        public static Attachment GetHeroCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = cardImage != null ? new List<CardImage>() { cardImage } : null,
                Buttons = new List<CardAction>() { cardAction }
            };

            return heroCard.ToAttachment();
        }
    }
}