using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExamPracticeProblem1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ClearAll();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
            //Gathers the JSON data
            List<Movie> MoviesNumberUsersVoted = new List<Movie>();
            List<Movie> movies = GettingDataFromWebservice();

            // ANSWER THE QUESTIONS

            //1. List all of the different genres for the movies
            GetAllGenresForMovies(movies);

            //2. Which movie has the highest IMDB score ?
            GetHighestIMDBScoreMovies(movies);

            //3. List all of the different movies that have a number of voted users with 350000 or more
            GetAllMoviesWithVotedUsersGreaterThan(movies, 350000);

            //4. How many movies where Anthony Russo is the director ?
            HowManyMoviesAnthonyRussoDirected(movies);

            //5. How many movies where Robert Downey Jr. is the actor 1 ?
            HowManyMoviesWhereRobertDowneyActor1(movies);

        }

        //problem5 how many movies with Robery Downey Jr
        private void HowManyMoviesWhereRobertDowneyActor1(List<Movie> movies)
        {
            int count = 0;
            foreach (var movie in movies)
            {
                if (movie.actor_1_name == "Robert Downey Jr.")
                {
                    count++;
                }
            }
            txtDowney.Text = count.ToString("N0");
        }

        //problem4 how many movies with anthony russo
        private void HowManyMoviesAnthonyRussoDirected(List<Movie> movies)
        {
            int count = 0;
            foreach (var movie in movies)
            {
                if (movie.director_name == "Anthony Russo")
                {
                    count++;
                }
            }
            txtRusso.Text = count.ToString("N0");
            
        }

        //Problem3 get all movies from data set >= value
        private void GetAllMoviesWithVotedUsersGreaterThan(List<Movie> movies, int v)
        {
            foreach (var movie in movies)
            {
                if(movie.num_voted_users >= v)
                {
                    Hyperlink h = new Hyperlink();
                    h.NavigateUri = new Uri(movie.movie_imdb_link);
                    h.Inlines.Add(movie.movie_title);
                    h.RequestNavigate += LinkOnRequestNavigate;
                    lstBoxVotedUsers.Items.Add(h);
                }
            }
        }

        //Problem2 get the highest IMDB score from data set
        private void GetHighestIMDBScoreMovies(List<Movie> movies)
        {
            List<Movie> highestIMDBScores = new List<Movie>();
            foreach (var movie in movies)
            {
                if (highestIMDBScores.Count < 1)
                {
                    highestIMDBScores.Add(movie);
                    continue;
                }
                else
                {
                    if (highestIMDBScores[0].imdb_score < movie.imdb_score) //we have new highest imdb score
                    {
                        highestIMDBScores.Clear();
                        highestIMDBScores.Add(movie);
                    }
                    else if (highestIMDBScores[0].imdb_score == movie.imdb_score) //have same score so add movie
                    {
                        highestIMDBScores.Add(movie);
                    }
                    else //current instance of (movie)
                    {
                        //dont need to add or delete
                    }
                }
            }
            if(highestIMDBScores.Count >1)
            {
                string content = "";
                foreach (var m in highestIMDBScores)
                {
                    content += m.movie_title + '\n';
                }
                txtIMBDscore.Text = content;
            }
            else
            {
                Hyperlink h = new Hyperlink();
                h.NavigateUri = new Uri(highestIMDBScores[0].movie_imdb_link);
                h.Inlines.Add(highestIMDBScores[0].movie_title);
                h.RequestNavigate += LinkOnRequestNavigate;
                txtIMBDscore.Inlines.Add(h);
            }
            
        }

        private void LinkOnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        //Problem1 grabs genres for movies
        private void GetAllGenresForMovies(List<Movie> movies)
        {
            Dictionary<string, int> genres = new Dictionary<string, int>();
            foreach (var movie in movies)
            {
                if(movie.genres.Contains("|"))
                {
                    var gs = movie.genres.Split('|');
                    foreach (var g in gs)
                    {
                        if(genres.ContainsKey(g))
                        {
                            genres[g] = genres[g]+1;
                        }
                        else
                        {
                            genres.Add(g, 1);
                        }
                        //lstBoxGenres.Items.Add(g);
                    }
                }
                else
                {
                    if (genres.ContainsKey(movie.genres))
                    {
                        genres[movie.genres] = genres[movie.genres] + 1;
                    }
                    else
                    {
                        genres.Add(movie.genres, 1);
                    }
                }

            }
            foreach (var key in genres.Keys)
            {
                lstBoxGenres.Items.Add($"{key}({genres[key].ToString("N0")})");
            }
        }

        //gets list of movies from provided web-service   /returns movie objects
        private static List<Movie> GettingDataFromWebservice()
        {
            List<Movie> movies;
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(@"http://pcbstuou.w27.wh-2.com/webservices/3033/api/Movies?number=100").Result;
                var content = response.Content.ReadAsStringAsync().Result;
                movies = JsonConvert.DeserializeObject<List<Movie>>(content); 
            }

            return movies; //might need to manually add
        }

        //clears all data afterwards
        private void ClearAll()
        {
            txtDowney.Inlines.Clear();
            txtRusso.Inlines.Clear();
            txtIMBDscore.Inlines.Clear();
            lstBoxGenres.Items.Clear();
            lstBoxVotedUsers.Items.Clear();
        }
    }
}
