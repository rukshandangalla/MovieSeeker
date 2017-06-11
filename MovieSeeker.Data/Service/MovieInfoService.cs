using MovieSeeker.Data.DTOs;
using MovieSeeker.Data.Mappers;
using MovieSeeker.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MovieSeeker.Data.Service
{
    public class MovieInfoService
    {
        private HttpClient client = new HttpClient();

        private static MovieInfoService instance;

        public static MovieInfoService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MovieInfoService();
                }

                return instance;
            }
        }

        private MovieInfoService()
        {
            client.BaseAddress = new Uri("http://movieinfoapi.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Movie>> GetAllMovies()
        {
            List<MovieDto> movieList = new List<MovieDto>();
            List<Movie> retList = new List<Movie>();

            HttpResponseMessage response = await client.GetAsync("api/movies");
            if (response.IsSuccessStatusCode)
            {
                movieList = await response.Content.ReadAsAsync<List<MovieDto>>();
            }

            foreach (var movie in movieList)
            {
                retList.Add(MovieMapper.Convert(movie));
            }

            return retList;
        }
    }
}