/*

Ag Cubio WebServer
Jordan Davis & Jacob Osterloh
December 9, 2015
Ps9 - cs3500

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AgCubio
{
    /// <summary>
    /// Allows the game server to connect to the web and send info from the database.
    /// 
    /// A user can request information from the game server by entering the following URL's into
    /// a web browser while the server is open.
    /// 
    /// -http://localhost:11100/scores
    /// -http://localhost:11100/highscores
    /// -http://localhost:11100/games?player=Joe
    /// -http://localhost:11100/eaten?id=35
    /// 
    /// Of course, if your server is running on a remote machine, you can substitute its IP address/name for "localhost"
    /// 
    /// An error page will be displayed if you request a player or id not contained in the database
    ///  
    /// </summary>
    public class WebServer
    {
        public static IPHostEntry host;
        public static string localIP;
        public static Database dbase;
        private static String errorMessage;

        /// <summary>
        /// begin the web server
        /// </summary>
        public static void startWebServer()
        {
            dbase = new Database();

            //this gets the current ip adddress of the server
            localIP = "localhost";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            string errorLink = "http://" + localIP + ":11100/scores";

            //this is the generic error message for the webpage
            errorMessage = "<!DOCTYPE html><html><head><title> Page Title </title>" +
            "</head><body bgcolor = \"#C0FFFF\">< h1><center><u><b><a href=" + errorLink + "> AgCubio </a></b></u></center></h1><p><h2><font color=\"red\"> HTTP ERROR 404: Invalid Webpage Request</font></h2></p><p>Please enter one of the following valid web addresses:</p>" +
            "<p>For all player data, click this link: <a href =" + errorLink + " > Scores </a> </p>" +
            "<p>For a list of high scores, click this link: <a href = http://" + localIP + ":11100/highscores> Highscores </a> </p>" +
            "<p>For all of the stats of a single player, enter the web address with the player's name at the end: <b>http://[Server's IP Adress]:11100/games?player=[Player name] </b></p>" +
            "<p>For a list of all of the players eaten by a player, enter the web address with the player's number at the end: <b>http://[Server's IP Adress]:11100/eaten?id=[Player number]</b> </p></body></html>";


        }

        /// <summary>
        /// handles a connection request calls method below as callback then asks for more data.
        /// </summary>
        /// <param name="state"></param>
        public static void handle_new_web_connection(StateObject state)
        {
            state.callBackFunction = new Action<StateObject>(receive_web_info);
            Network.i_want_more_data(state);
        }

        /// <summary>
        /// callback that gets request and produces the data to the web.
        /// </summary>
        /// <param name="state"></param>
        private static void receive_web_info(StateObject state)
        {
            //bytes from browser
            Socket socket = state.workSocket;
            string info = state.sb.ToString().Trim();

            //send initial info.
            string startInfo = "HTTP/1.1 200 OK\r\nConnection: close\r\nContent-Type: text/html; charset=UTF-8\r\n";
            Network.Send(socket, startInfo);
            Network.Send(socket, "\r\n");

            //get info from database
            dbase.getScoresDB();
            dbase.getTop5();

            //helpers
            string html = "";
            string data = "";

            //perform requests

            //this request is for the main scores table
            if (info.Contains("GET /scores HTTP/1.1"))
            {
                //if db is empty then tell user that no games have been played
                if (dbase.webdata[0].top5.Count < 1)
                {
                    html = "<!DOCTYPE html><html><head><title> Page Title </title></head><body bgcolor = \"#C0FFFF\"><h1><center><u><b><a href=" + "http://" + localIP + ":11100/scores" + "> AgCubio " +
                    "</a></b></u></center></h1><p><center>*No Game History Exist*</center></p><p></p></body></html>";
                }
                else
                {
                    //iterate through all the data and append it all to a string.
                    foreach (WebData w in dbase.webdata.Values)
                    {
                        //if the name has a space in it, replace it with "%20" for hyperlink
                        string space = w.name;
                        if (space.Contains(" "))
                        {
                            space = space.Replace(" ", "%20");
                        }
                        //ignore index 0 (its reserved)
                        if (w.index != 0)
                            data += ("<tr><td>" + "<a href= http://" + localIP + ":11100/eaten?id=" + w.index + ">" + w.index + "</a></td><td><a href= http://" + localIP + ":11100/games?player=" + space + ">" + w.name + "</a></td><td>" + w.rank +
                                "</td><td>" + w.maxMass + "</td><td>" + w.foodEaten + "</td><td>" + w.cubesEaten +
                                "</td><td>" + w.timeAlive + "</td><td>" + w.timeOfDeath + "</td></tr>");
                    }

                    //add data string to html page
                    html = "<!DOCTYPE html><html><head><title> Page Title </title></head><body bgcolor = \"#C0FFFF\"><h1><center><u><b><a href=" + "http://" + localIP + ":11100/scores" + "> AgCubio </a></b></u></center></h1><p><center><table style=\"width:80%\"><tr><td><b><u>id</u></b></td>" +
                           "<td><b><u>Name</u></b><td><b><u><a href = http://" + localIP + ":11100/highscores> Top Rank </a> </u></b></td><td><b><u>Maximum Mass</u></b></td>" +
                           "<td><b><u>Food Eaten</u></b></td><td><b><u>Cubes Eaten</u></b><td><b><u>Time Alive</u></b></td>" +
                           "<td><b><u>Time Of Death</u></b></td></tr>" + data + "</table></center></p><p></p></body></html>";
                }

                //send page and close socket
                Network.SendWeb(socket, html);
            }
            else if (info.Contains("GET /games?")) //this is when the player requests for all games of a player
            {                                     //including list of players eaten by the player
                //get the name
                bool flag = false; //used to determine if requested name existed
                
                //replace spaces in name for hyperlink
                string sub = info.Substring(18, 60);
                string playerName = "";
                string[] parts = sub.Split(' ');
                if (parts != null)
                {
                    playerName = parts[0];
                }
                playerName = playerName.Replace("%20", " ");

                string ateNames = "";

                //go through all data untill we find requested name.
                foreach (WebData w in dbase.webdata.Values)
                {
                    //only do something if the names match
                    if (w.name == playerName)
                    {
                        if (w.playersEaten.Count > 0)
                        {
                            //add a list of all the players we ate
                            foreach (string ate in w.playersEaten)
                            {
                                ateNames += "<tr><td>" + w.index + "</td><td>" + ate + "</td><tr>";
                            }
                        }
                        else
                        {
                            //no players were eaten by req player
                            ateNames = "<tr><td> * No Players Eaten * </td><tr>";
                        }

                        //create line for data
                        data += ("<tr><td><a href = http://" + localIP + ":11100/eaten?id=" + w.index + ">" + w.index + "</a></td><td>" + w.name + "</td><td>" + w.rank +
                            "</td><td>" + w.maxMass + "</td><td>" + w.foodEaten + "</td><td>" + w.cubesEaten +
                            "</td><td>" + w.timeAlive + "</td><td>" + w.timeOfDeath + "</td></tr>");

                        //a player exists
                        flag = true;
                    }
                }

                if (flag) //if a player exists
                {
                    //webpage layout
                    html = "<!DOCTYPE html><html><head><title> Page Title </title></head><body bgcolor = \"#C0FFFF\"><h1><center><u><b> <a href=" + "http://" + localIP + ":11100/scores" + "> AgCubio </a></b></u></center></h1><p><center><table style=\"width:80%\"><tr><td><b><u>id</u></b></td>" +
                            "<td><b><u>Name</u></b><td><b><u><a href = http://" + localIP + ":11100/highscores> Top Rank </a> </u></b></td><td><b><u>Maximum Mass</u></b></td>" +
                            "<td><b><u>Food Eaten</u></b></td><td><b><u>Cubes Eaten</u></b><td><b><u>Time Alive</u></b></td>" +
                            "<td><b><u>Time Of Death</u></b></td></tr>" + data + "</table></center></p><p></p><p></p>" +
                            "<center><table style=\"width:25%\"><tr><td><b><u>Players Eaten</u></b></td></tr>" + ateNames + "</table></center></p><p></p></body></html>";
                }
                else
                {
                    //player didnt exist, produce error
                    html = errorMessage;
                }

                //send data and close sockets
                Network.SendWeb(socket, html);
            }
            else if (info.Contains("GET /eaten?"))  //request for specific game by a plery
            {                                       //returns players eaten by player durring game session
                //get the req id number
                string sub = info.Substring(14, 24);
                string digits = new String(sub.TakeWhile(Char.IsDigit).ToArray());
                string ateNames = "";
                int id = 0;
                //make sure id exists, print error page if not
                if (!int.TryParse(digits, out id) || !dbase.webdata.ContainsKey(id))
                    Network.SendWeb(socket, errorMessage);
                else
                {
                    //get info from specific game
                    if (dbase.webdata[id].playersEaten.Count > 0)
                    {
                        //get list of players eaten
                        foreach (string ate in dbase.webdata[id].playersEaten)
                        {
                            ateNames += "<tr><td>" + ate + "</td><tr>";
                        }
                    }
                    else
                    {
                        //no players were eaten
                        ateNames = "<tr><td> No Players Eaten </td><tr>";
                    }
                    //if name has space, put %20 for hyperlink
                    string space = dbase.webdata[id].name;
                    if (space.Contains(" "))
                    {
                        space = space.Replace(" ", "%20");
                    }

                    //get table data
                    data += ("<tr><td>" + dbase.webdata[id].index + "</td><td><a href= http://" + localIP + ":11100/games?player=" + space + ">" + dbase.webdata[id].name + "</a></td><td>" + dbase.webdata[id].rank +
                        "</td><td>" + dbase.webdata[id].maxMass + "</td><td>" + dbase.webdata[id].foodEaten + "</td><td>" + dbase.webdata[id].cubesEaten +
                        "</td><td>" + dbase.webdata[id].timeAlive + "</td><td>" + dbase.webdata[id].timeOfDeath + "</td></tr>");

                    //add data to webpage layout
                    html = "<!DOCTYPE html><html><head><title> Page Title </title></head><body bgcolor = \"#C0FFFF\"><h1><center><u><b><a href=" + "http://" + localIP + ":11100/scores" + "> AgCubio </a></b></u></center></h1><p><center><table style=\"width:80%\"><tr><td><b><u>id</u></b></td>" +
                      "<td><b><u>Name</u></b><td><b><u><a href = http://" + localIP + ":11100/highscores> Top Rank </a> </u></b></td><td><b><u>Maximum Mass</u></b></td>" +
                      "<td><b><u>Food Eaten</u></b></td><td><b><u>Cubes Eaten</u></b><td><b><u>Time Alive</u></b></td>" +
                      "<td><b><u>Time Of Death</u></b></td></tr>" + data + "</table></center></p><p></p><p></p>" +
                      "<center><table style=\"width:10%\"><tr><td><b><u>Players Eaten</u></b></td></tr>" + ateNames + " </table></center></p><p></p></body></html>";

                    //send data and close socket
                    Network.SendWeb(socket, html);
                }
            }
            else if (info.Contains("highscores"))       //request for high scores made
            {
                //make sure data exists
                if (dbase.webdata[0].top5.Count > 0)
                {
                    //itterate through top 25 of sorted dataset. (there are only 25)
                    for (int i = 0; i < 25; i++)
                    {
                        int rank = i;
                        //fix name for hyperlink
                        string space = dbase.webdata[0].top5[i];
                        if (space.Contains(" "))
                        {
                            space = space.Replace(" ", "%20");
                        }
                        //append data in table
                        data += ("<tr><td><a href=http://" + localIP + ":11100/eaten?id=" + dbase.webdata[0].playersEaten[i] + ">" + dbase.webdata[0].playersEaten[i] + "</a></ td>" +
                                    "<td><a href=http://" + localIP + ":11100/games?player=" + space + ">" + dbase.webdata[0].top5[i] + "</a></td><td>" + (rank + 1) + "</td></tr>");
                    }
                    //insert data in web layout
                    html = "<!DOCTYPE html><html><head><title> Page Title </title></head><body bgcolor = \"#C0FFFF\"><h1><center><u><b><a href=" + "http://" + localIP + ":11100/scores" + "> AgCubio </a></b></u></center></h1><p><center><table style=\"width:25%\"><tr><td><b><u>id</u></b></td>" +
                            "<td><b><u>Name</u></b></td><td><b><u>All Time Rank</u></b></td></tr>" + data + "</table></center></p><p></p></body></html>";
                }
                else
                {
                    //no player data existed, print error
                    html = "<!DOCTYPE html><html><head><title> Page Title </title></head><body bgcolor = \"#C0FFFF\"><h1><center><u><b><a href=" + "http://" + localIP + ":11100/scores" + "> AgCubio " +
                        "</a></b></u></center></h1><p><center>*No Game History Exist*</center></p><p></p></body></html>";
                }

                //send web data and close socket
                Network.SendWeb(socket, html);
            }
            else
            {
                //an invalid request has been made. print error webpage   
                Network.SendWeb(socket, errorMessage);
            }
        }
    }
}
