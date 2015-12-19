/*

Ag Cubio Database
Jordan Davis & Jacob Osterloh
December 9, 2015
Ps9 - cs3500

*/

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgCubio
{
    /// <summary>
    /// Conroller used to communicate back and forth with a database and server.
    /// </summary>
    public class Database
    {

        private int index;
        private double maxMass;
        private string name;
        private int rank;
        private List<string> playersEaten;
        private int foodEaten;
        private int cubesEaten;
        private DateTime timeOfDeath;
        private TimeSpan span;
        private string timeAlive;
        private List<string> top5;
        public const string connectionString = "server=atr.eng.utah.edu;database=cs3500_osterloh;uid=cs3500_osterloh;password=trescommas";
        //Password = trescommas
        //make sure password matches above

        /// <summary>
        /// Dictionary of data from the database
        /// </summary>
        public Dictionary<int, WebData> webdata;

        
        /// <summary>
        /// create a database
        /// </summary>
        public Database()
        {
            index = 0;
            maxMass = 0;
            rank = 0;
            name = "";
            playersEaten = new List<string>();
            foodEaten = 0;
            cubesEaten = foodEaten + playersEaten.Count;
            timeOfDeath = DateTime.Now;
            span = DateTime.Now - DateTime.Now;
            timeAlive = new TimeSpan(span.Hours, span.Minutes, span.Seconds).ToString();
            top5 = new List<string>();
            webdata = new Dictionary<int, WebData>();
            webdata.Add(0, new WebData());  //this index used for top 5 info.
        }

        /// <summary>
        /// Sets the data that is being put into the database
        /// </summary>
        /// <param name="cube"></param>
        private void setStats(Cube cube)
        {
            maxMass = cube.highestScore;
            name = cube.Name;
            playersEaten = cube.playersEaten;
            foodEaten = cube.foodEaten;
            cubesEaten = foodEaten + playersEaten.Count;
            timeOfDeath = DateTime.Now;
            span = timeOfDeath - cube.birth;
            timeAlive = new TimeSpan(span.Hours, span.Minutes, span.Seconds).ToString();
            top5 = new List<string>();
            if (cube.rank < 1)
                cube.rank = 1;
            else
                cube.rank = cube.rank + 1; // becasue index starts at zero
            rank = cube.rank;
        }

        
        /// <summary>
        /// Gets all information fromt he Database and stores it into the webdata Dictionary
        /// </summary>
        public void getScoresDB()
        {
            using (MySqlConnection sql = new MySqlConnection(connectionString))
            {
                try
                {
                    //open and create command
                    sql.Open();
                    MySqlCommand command = sql.CreateCommand();

                    //get all data from 'Player' table
                    command.CommandText = "SELECT * from Player";
                    using(MySqlDataReader reader = command.ExecuteReader())
                    {
                        //get all information from database
                        while (reader.Read())
                        {
                            WebData w = new WebData();
                            w.index = (int)reader["id"];
                            w.name = (string)reader["name"];
                            w.rank = (int)reader["rank"];
                            w.maxMass = (int)reader["maxMass"];
                            w.timeAlive = (string)reader["timeAlive"];
                            w.timeOfDeath = (DateTime)reader["timeOfDeath"];
                            w.foodEaten = (int)reader["foodEaten"];
                            w.cubesEaten = (int)reader["cubesEaten"];
                            if(w.index != 0)
                                webdata[w.index] = w;   //add to dictioanry
                        }
                    }

                    //get all data from 'NamesOfPlayersEaten' table
                    command.CommandText = "SELECT * from NamesOfPlayersEaten";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        int x = 0;
                        string name = "";
                        while (reader.Read())
                        {                         
                            x = (int)reader["id"];
                            name = ((string)reader["namesOfPlayersEaten"]);
                            webdata[x].playersEaten.Add(name);
                        }
                    }

                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }


        /// <summary>
        /// sends cube data to database
        /// </summary>
        /// <param name="cube"></param>
        public void populateDB(Cube cube)
        {
            using (MySqlConnection sql = new MySqlConnection(connectionString))
            {
                try
                {
                    // set stats before sending to db
                    setStats(cube);
                    MySqlCommand command = sql.CreateCommand();

                    //send to 'Player' table
                    command.CommandText = "INSERT INTO Player(name, rank, maxMass, foodEaten, cubesEaten, timeAlive, timeOfDeath) VALUES(@name, @rank, @maxMass, @foodEaten, @cubesEaten, @timeAlive, @timeOfDeath)";
                    sql.Open();
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@rank", rank);
                    command.Parameters.AddWithValue("@maxMass", maxMass);
                    command.Parameters.AddWithValue("@foodEaten", foodEaten);
                    command.Parameters.AddWithValue("@cubesEaten", cubesEaten);
                    command.Parameters.AddWithValue("@timeAlive", timeAlive);
                    command.Parameters.AddWithValue("@timeOfDeath", timeOfDeath);
                    command.ExecuteNonQuery();

                    //if cube ate other players, send to 'NamesOfPlayersEaten' table
                    foreach (String AteName in playersEaten)
                    {
                        command.CommandText = "INSERT INTO NamesOfPlayersEaten(id,namesOfPlayersEaten) VALUES((SELECT id from Player WHERE name='" + cube.Name + "'),@namesOfPlayersEaten)";
                        try
                        {
                            command.Parameters.Clear(); //must clear parameters each time
                            command.Parameters.AddWithValue("@namesOfPlayersEaten", AteName);
                            command.ExecuteNonQuery();
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }



        /// <summary>
        /// updates alltime top5 rank
        /// stores data in webdata[0].top5
        /// </summary>
        /// <param name="cube"></param>
        public void getTop5()
        {
            using (MySqlConnection sql = new MySqlConnection(connectionString))
            {
                try
                {
                    MySqlCommand command = sql.CreateCommand();
                    sql.Open();

                    //gets the All time top5 games.
                    command.CommandText = "SELECT * FROM Player ORDER BY maxMass DESC LIMIT 25";
                    command.ExecuteNonQuery();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        string rankName;
                        int rankId;
                        webdata[0].top5 = new List<string>();
                        while (reader.Read())
                        {
                            rankName = (string)reader["name"];
                            rankId = (int)reader["id"];
                            //index zero is reserved for highscores info
                            webdata[0].top5.Add(rankName);
                            webdata[0].playersEaten.Add("" + rankId);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

    }
}

