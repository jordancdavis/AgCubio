/*

Ag Cubio Server
Jordan Davis & Jacob Osterloh
December 9, 2015
Ps9 - cs3500

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Timers;
using Newtonsoft.Json;
using System.IO;

namespace AgCubio
{
    /// <summary>
    /// The server maintains the state of the world, computes all game mechanics, and communicates
    /// to the clients what is going on. The clients only job is to display the state of the world
    /// to the player and send movement/split requests to the server.
    /// </summary>
    public static class Server
    {
        /// <summary>
        /// A list of all the connections to the server
        /// </summary>
        public static LinkedList<StateObject> connections = new LinkedList<StateObject>();

        /// <summary>
        /// world object created for the server
        /// </summary>
        public static World world;

        /// <summary>
        /// Time connected to the server
        /// </summary>
        public static DateTime startTime;

        /// <summary>
        /// database object
        /// </summary>
        private static Database dbase;


        /// <summary>
        /// The main function which will build a new world and start the server
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            //get filepath and create a world
            string filename = "";
            if (args.Length > 0 && File.Exists(args[0]))
                filename = args[0];
            else if (File.Exists("C:\\Users\\jordand\\cs3500\\Aviato\\AgCubio\\Resources\\world_parameters.xml"))
            {
                //try to creat w/ filename first
                filename = "C:\\Users\\jordand\\cs3500\\Aviato\\AgCubio\\Resources\\world_parameters.xml";
                world = new World(filename);
            }
            else
            {
                //just create a world already
                world = new World();
            }

            //start the server
            start();
            WebServer.startWebServer();
            dbase = new Database();
            Console.Read();
        }

        /// <summary>
        /// populate the initial world (with food), set up the heartbeat of the program, and await
        /// network client connections (NOTE: Jim suggests we use a Timer object for the heartbeat).
        /// </summary>
        public static void start()
        {
            //Timer object for the heartbeat
            Timer timer = new Timer();
            timer.Interval = 1.0 / (double)world.heartbeats_per_second * 1000.00;
            timer.Elapsed += new ElapsedEventHandler(update);
            timer.Start();

            //create food and begin populating the world
            Cube food;
            do
                continue;
            while (Server.world.growFood(1.0, out food));

            //begin listening for a connection with Game Client
            Network.Server_Awaiting_Client_Loop(new Action<StateObject>(handle_new_client_connection));

            //begin listening for a connection with Web Server
            Network.Server_Awaiting_Web_Loop(new Action<StateObject>(WebServer.handle_new_web_connection));
            startTime = DateTime.Now;
        }

        /// <summary>
        /// NOTE: this is most likely used as the callback function required by the networking code parameters.
        /// This code should set up a callback to receive a players name and then request more data from the connection.
        /// </summary>
        /// <param name="state"></param>
        private static void handle_new_client_connection(StateObject state)
        {
            state.callBackFunction = new Action<StateObject>(receive_player_name);
            Network.i_want_more_data(state);
        }

        /// <summary>
        /// this function should create the new player cube (and of corse update the world about it) and store
        /// away all the necessary data for the connection to be use for further communication. 
        /// It should also set up the callback for handling move/split requests and request new data from the socket.
        /// Finally it should send the current state of the world to the player.
        /// 
        /// NOTE: the current server exemplar adds the player to the world before the initial state is sent.
        /// the initial state can take seconds to arrive, thus the player's cube can be eaten, or even move,
        /// while htis is happening. 
        /// An excellent solution will avoid this (i.i., the player's game only really starts after all the data
        /// has been sent.
        /// 
        /// </summary>
        /// <param name="state"></param>
        private static void receive_player_name(StateObject state)
        {
            //setup the callback for move/split
            Socket socket = state.workSocket;
            string name = state.sb.ToString().Trim();
            state.callBackFunction = new Action<StateObject>(handle_data_from_client);

            //create new player cube
            Cube newPlayer = world.addPlayer(name);
            state.uid = newPlayer.uid;

            lock (world)
            {
                try
                {
                    //send cube across the server to the client
                    Network.Send(socket, JsonConvert.SerializeObject((object)newPlayer) + "\n");
                    StringBuilder sb = new StringBuilder();
                    foreach (Cube cube in Server.world.food.Values)
                        sb.Append(JsonConvert.SerializeObject((object)cube) + "\n");

                    //send state of world to client
                    Network.Send(socket, sb.ToString());
                }
                catch
                { }

            }

            //make connection and call more data
            lock (connections)
                connections.AddLast(state);
            Network.i_want_more_data(state);

        }

        /// <summary>
        /// this data should be either (move,x,y) or (split,x,y).
        /// in either case you should handle the request
        /// 
        /// 
        /// NOTE: when move,x,y does not mean move the cube to x,y, but move the cube towards x,y.
        /// NOTE: this code should be the callback used by the networking code when it receives data.
        /// </summary>
        /// <param name="state"></param>
        private static void handle_data_from_client(StateObject state)
        {
            try
            {
                //create array for string builder
                char[] gap = new char[1] { '\n' };
                string[] myArr = state.sb.ToString().Split(gap, StringSplitOptions.RemoveEmptyEntries);
                bool isSplit = false;
                int x = 0;  //x location
                int y = 0;  //y location

                //itterate through each string/data
                for (int index = 0; index < myArr.Length; ++index)
                {
                    //get first part of string
                    string cube = myArr[index];
                    if (cube.Length > 1 && (int)cube[0] == 40 && (int)cube[cube.Length - 1] == 41)
                    {
                        //get move or split request. 
                        string[] myArr2 = cube.Substring(1, cube.Length - 2).Split(',');
                        if (myArr2[0] == "move")
                        {
                            int.TryParse(myArr2[1], out x);
                            int.TryParse(myArr2[2], out y);
                        }
                        else if (myArr2[0] == "split")
                        {
                            //mark the isSplit flag
                            int.TryParse(myArr2[1], out x);
                            int.TryParse(myArr2[2], out y);
                            isSplit = true;
                            break;
                        }
                    }
                }
                lock (world)
                {
                    //perform split/move command.
                    if (isSplit)
                        world.splitPlayer(state.uid, x, y);
                    else
                        world.movePlayer(state.uid, x, y);
                }
                //clear string builder
                state.sb.Clear();

                //call for more data
                Network.i_want_more_data(state);

            }
            catch (Exception e)
            {
                state.workSocket.Shutdown(SocketShutdown.Both);
                state.workSocket.Close();
                //print any error that occured
                Console.WriteLine(e.ToString());
            }

        }

        /// <summary>
        /// this is the main "heart" of the game. Durring each update event the server should:
        /// 1.Grow new food
        /// 2.Handle Players eating food or other players
        /// 3.Handle Players cube attrition (and other things, like food growth)
        /// 4.Handle sending the current state of the world to EVERY client (new food, player changes, etc.).
        /// 
        /// If a client has disconnected since the last update, this should be cleaned up here.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void update(Object sender, ElapsedEventArgs e)
        {
            //controll heartbeats.
            ((Timer)sender).Stop();
            ++world.heartbeats;
            if (world.heartbeats_per_second % 25 == 0)
            {
                double totalSeconds = DateTime.Now.Subtract(startTime).TotalSeconds;
            }

            //grow food
            Cube food;
            world.growFood(0.50, out food);
            if (food != null)
            {
                //get connection
                LinkedListNode<StateObject> states = connections.First;
                while (states != null)
                {
                    try
                    {
                        //send food
                        Network.Send(states.Value.workSocket, JsonConvert.SerializeObject(food) + "\n");
                        states = states.Next;
                    }
                    catch (Exception)
                    {
                        LinkedListNode<StateObject> tempState = states.Next;
                        Server.connections.Remove(states);
                        states = tempState;
                    }
                }
            }

            //eat food
            LinkedList<Cube> cubes = world.eatFood();
            lock (connections)
            {
                foreach (Cube item in cubes)
                {
                    foreach (StateObject state in connections)
                    {
                        try
                        {
                            //send eaten food
                            Network.Send(state.workSocket, JsonConvert.SerializeObject((object)item) + "\n");
                        }
                        catch
                        { }
                    }
                }
            }

            //handle attrition
            world.attrition();
            LinkedList<Cube> eat = world.eatPlayers();
            lock (Server.connections)
            {
                lock (Server.world)
                {
                    try
                    {
                        //send eaten players
                        foreach (Cube cube in eat)
                        {
                            foreach(Cube c in world.namedEats[cube.team_id])
                            {
                                cube.playersEaten.Add(c.Name);
                            }
                            if(cube.team_id == cube.uid)
                                //send cube info to database
                                dbase.populateDB(cube);
                            foreach (StateObject item_4 in Server.connections)
                                Network.Send(item_4.workSocket, JsonConvert.SerializeObject((object)cube) + "\n");
                        }
                        //send players
                        foreach (Cube player in Server.world.players.Values)
                        {                         
                            foreach (StateObject item_5 in Server.connections)
                                Network.Send(item_5.workSocket, JsonConvert.SerializeObject((object)player) + "\n");
                        }
                    }
                    catch
                    {
                    }
                }
            }
        //continue heartbeats
        ((Timer)sender).Start();
        }
    }
}
