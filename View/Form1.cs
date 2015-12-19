/*

Ag Cubio Gui
Jordan Davis & Jacob Osterloh
December 9, 2015
Ps9 - cs3500

*/

using AgCubio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    /// <summary>
    /// AgCubio GUI view for the client
    /// This client view draws a world filled with cubes provided from a server.
    /// The window will be zoomed in on the players cube and you can watch it grow
    /// the more food you eat.
    /// Stats are shown durring gameplay as well as after death.
    /// Connection Error Messages are displayed when server is not available for connection.
    /// </summary>
    public partial class Form1 : Form
    {
        //player name
        private string name;
        //hostname
        private string hostname;
        //brush for drawing names
        private System.Drawing.SolidBrush nameBrush;
        //bursh for drawing cubes
        private System.Drawing.SolidBrush playerBrush;
        //pen for painting viruses
        private System.Drawing.Pen Vpen;
        //game world
        private World world;
        //current cube uID
        private int myUid;
        //determines if cube is dead or not
        private bool isDead;
        //current socket
        private Socket mySocket;
        //players score
        private int myScore;
        //players high score
        private int myHighScore;
        //number of players eaten
        private int teamPlayersEat;
        //team food eat
        private int teamFoodEat;
        //last mass helpef for determining players/food eaten
        private double lastMass;
        //if player is alive or dead
        private bool isAlive;
        //timer for fps
        private Stopwatch fpsWatch = new Stopwatch();
        //counter for timer stats
        private DateTime startTime;
        //counter for fps
        private int frameCount;
        //team mass for split cubes
        private double teamMass;
        //boolean to determine if paintworld has already been called
        private bool initialConnect;
        //scale factor for resized view
        private int scaleFactor;
        //magic key for toggleing scale on/off
        private bool magicScaleKey;
        //xml file to set world parameters
        private string filename;


        /// <summary>
        /// Constructor for GUI
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            //this is the filpath for world parameters
            filename = "";
            if (File.Exists("C:\\Users\\jordand\\cs3500\\Aviato\\AgCubio\\Resources\\world_parameters.xml"))
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

            //initialize variables
            myScore = 0;
            isAlive = true;
            teamMass = world.player_start_mass;
            lastMass = teamMass;
            initialConnect = true;
            magicScaleKey = false;
            teamFoodEat = 0;
            teamPlayersEat = 0;  
        }

        /// <summary>
        /// Method called when play button has been clicked
        /// --Connects to server
        /// --Draws the world
        /// --Starts the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPlay_Click(object sender, EventArgs e)
        {

            //start game timers
            this.fpsWatch.Restart();
            this.startTime = DateTime.Now;
            this.myScore = 0;
            this.teamPlayersEat = 0;
            this.teamFoodEat = 0;

            teamMass = world.player_start_mass;

            //get name and server
            this.name = TextBoxName.Text;
            this.hostname = TextBoxServer.Text;

            //attemt to connect to server
            this.mySocket = null;
            this.mySocket = Network.Connect_to_Server(new Action<StateObject>(callbackFirst), hostname);
            if (mySocket == null)
                return;

            //connection made
            this.isAlive = true;
            this.isDead = false;

            //dark theme box controlls
            if (this.checkBoxColor.Checked)
            {
                this.BackColor = Color.Black;
                this.checkBoxColor.BackColor = Color.Black;
                this.checkBoxColor.ForeColor = Color.White;
            }
            else
            {
                this.BackColor = Color.FromArgb(192, 255, 255);
                this.checkBoxColor.BackColor = this.BackColor;
                this.checkBoxColor.ForeColor = Color.Black;
            }

            //hide setup
            hideSetup();

        }

        /// <summary>
        /// Allows user to push enter to begin game
        /// calls buttonPlay_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonPlay_Click(sender, e);
        }

        /// <summary>
        /// Button that allows user to Restart game
        /// calls buttonPlay_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonReset_Click(object sender, EventArgs e)
        {
            buttonPlay_Click(sender, e);
            hideSetup();
            teamPlayersEat = 0;
            teamFoodEat = 0;
            myScore = world.player_start_mass;
            lastMass = world.player_start_mass;
        }

        /// <summary>
        /// intial callback for the name
        /// sends name to server
        /// displays message if error occured in connection
        /// </summary>
        /// <param name="state"></param>
        private void callbackFirst(StateObject state)
        {
            //checks if an error occured while attempting to connect to server
            if (state.error_occured)
            {
                //if error occured then show message and showSetup
                Invoke(new Action(() =>
                {
                    this.labelConnectionLost.Show();
                    this.showSetup();
                }));
            }
            else
            {
                //conection was successful, send socket and playername.
                state.callBackFunction = new Action<StateObject>(callbackPlayer);
                Network.Send(this.mySocket, name + "\n");

                //draw cubes, only need to call this once or else trippy world is shown
                if (initialConnect)
                    this.Paint += new System.Windows.Forms.PaintEventHandler(this.paintWorld);
                //set tis bool to false because we dont want to redraw ever again.
                initialConnect = false;
            }
        }

        /// <summary>
        /// gets player info from server
        /// </summary>
        /// <param name="state"></param>
        private void callbackPlayer(StateObject state)
        {
            //use string builder to receive info from server
            StringBuilder sb = state.sb;
            try
            {
                //put info into char array separated by newline for Json
                char[] myArray = new char[1] { '\n' };
                string info = sb.ToString().Split(myArray, StringSplitOptions.RemoveEmptyEntries)[0];
                sb.Remove(0, info.Length);

                //convert Json info into Cube object
                Cube cube = JsonConvert.DeserializeObject<Cube>(info);

                if (!cube.food)
                {
                    //if cube is not food add it to player Dictionary
                    lock (this.world)
                        this.world.players[cube.uid] = cube;
                    //save myUID
                    myUid = cube.uid;
                }
                else
                {
                    //cube is food so add it to food dictionary
                    lock (this.world)
                        this.world.food[cube.uid] = cube;
                }
            }
            catch (Exception)
            {//ignore exception for improved gamplay
            }

            //set callback to calbackData to continue to get data from server
            state.callBackFunction = new Action<StateObject>(this.callbackData);
            callbackData(state);
        }

        /// <summary>
        /// Receives Data from the Server once a connection has been made
        /// </summary>
        /// <param name="state"></param>
        private void callbackData(StateObject state)
        {
            //create stringbuilder from stateobject
            StringBuilder sb = state.sb;
            lock (this.world)
            {
                //put data from server into char array separated by newline for Json
                char[] charArr = new char[1] { '\n' };
                string[] serverData = sb.ToString().Split(charArr, StringSplitOptions.RemoveEmptyEntries);
                int count = 0;
                try
                {
                    //go through all strings that the server provided
                    foreach (string cubeInfo in serverData)
                    {
                        //find cubes infro separated by { and }
                        if (cubeInfo.StartsWith("{") && cubeInfo.EndsWith("}"))
                        {
                            //convert incoming cube info into a cube object
                            Cube incomingCube = JsonConvert.DeserializeObject<Cube>(cubeInfo);
                            //make sure that cube is not null
                            if (incomingCube != null)
                            {
                                //increment count
                                ++count;
                                //check if cube is dead or alive (determined by Mass > 0)
                                if (incomingCube.Mass == 0.0)
                                {
                                    //Cube is dead so remove it from proper dictionary
                                    if (incomingCube.food)
                                    {
                                        this.world.food.Remove(incomingCube.uid);
                                    }
                                    else
                                    {
                                        this.world.players.Remove(incomingCube.uid);
                                        this.isDead = true;
                                        //check if cube was our players cube
                                        if (incomingCube.uid == myUid)
                                        {
                                            //if cube was our player, this will allow death
                                            //screen to be visible.
                                            isAlive = false;
                                            this.showStats(isAlive);
                                        }
                                    }
                                }
                                else //cube is still active
                                {
                                    //add cube to world
                                    if (incomingCube.food)
                                    {
                                        this.world.food[incomingCube.uid] = incomingCube;
                                    }
                                    else
                                    {
                                        this.world.players[incomingCube.uid] = incomingCube;
                                        this.isDead = false;
                                    }
                                }
                            }
                            else //cube info was null, coninue
                                break;
                        }//cube info didnt start with { and end with }, coninue
                        else
                            break;
                    }//End Foreach
                } //End Try
                catch (Exception)
                {//ignore exceptions for better gameplay
                }

                //clear the string builder
                sb.Clear();
                //if cubes were added then invalidate
                if (count > 0)
                    this.Invalidate();
                //if extra data remains, append it so we dont lose info
                if (count != serverData.Length)
                    sb.Append(serverData[serverData.Length - 1]);
            }//End Lock

            //ask server for fore data
            Network.i_want_more_data(state);
        }

        /// <summary>
        /// Paints all cubes in the world
        /// Updated statistic lables
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void paintWorld(object sender, PaintEventArgs e)
        {
            try
            {
                //only scale if magic key is not pressed
                if (magicScaleKey)
                {
                    scaleFactor = 1;
                }
                else
                {
                    //scale the screen to zoom in on player cube.
                    if (teamMass < 5000)
                        scaleFactor = 1;
                    if (teamMass < 3000)
                    {
                        //handles jumpy scale problem
                        if ((scaleFactor == 1 && teamMass > 2850))
                        {
                            scaleFactor = 1;
                        }
                        else
                            scaleFactor = 2;
                    }
                    if (teamMass < 1200)
                    {
                        //handles jumpy scale problem
                        if ((scaleFactor == 2 && teamMass > 1190))
                        {
                            scaleFactor = 2;
                        }
                        else
                            scaleFactor = 3;
                    }
                    if (teamMass < 500)
                    {
                        //handles jumpy scale problem
                        if ((scaleFactor == 3 && teamMass > 490))
                        {
                            scaleFactor = 3;
                        }
                        else
                            scaleFactor = 4;
                    }

                    if (teamMass < 100)
                    {
                        //handles jumpy scale problem
                        if ((scaleFactor == 4 && teamMass > 95))
                        {
                            scaleFactor = 4;
                        }
                        else
                            scaleFactor = 5;
                    }
                }

                //send player positon to server
                this.sendPosition(scaleFactor);
                //if player is alive, show stats (otherwise restart menu)

////is stats arn't working then change this back            
////this.showStats(isAlive);

                this.Update();

                //begin frameCounter
                ++this.frameCount;
                TimeSpan elapsed = this.fpsWatch.Elapsed;
                if (elapsed.Seconds > 0)
                {
                    //update framCounter lables
                    this.labelFPSValue.Text = string.Concat((object)(this.frameCount / elapsed.Seconds));
                    this.labelFPSValue.Invalidate();
                    this.labelFPSValue.Update();
                    this.labelFPSValue.Refresh();
                }
                if (elapsed.Seconds > 60)
                {
                    //resart counter to keep it accurate
                    this.fpsWatch.Restart();
                    this.frameCount = 0;
                }

                //Here is where we draw cubes only if game has begun
                if (this.myUid > 0 && (this.world.food.Count >= 0) && !this.isDead)
                {
                    lock (world)
                    {
                        //update stats lables
                        updateStats();

                        //Scale View

                        e.Graphics.ScaleTransform(scaleFactor, scaleFactor);
                        e.Graphics.TranslateTransform(-world.players[myUid].loc_x, -world.players[myUid].loc_y);
                        e.Graphics.TranslateTransform(this.Width / 2f / scaleFactor, this.Height / 2f / scaleFactor);

                        //draw food cubes
                        foreach (Cube item in world.food.Values)
                        {
                            //draw food cube
                            float width = Math.Max(item.width, 5); // or Max (width, 5)
                            this.playerBrush = new SolidBrush(Color.FromArgb(item.argb_color));
                            this.Vpen = new Pen(Color.DarkGreen, 3);

                            if (item.isVirus)
                            {
                                //width = item.width;
                                e.Graphics.FillRectangle(playerBrush, item.loc_x - width, item.loc_y - width, item.width, item.width);
                                e.Graphics.DrawRectangle(Vpen, item.loc_x - width, item.loc_y - width, item.width, item.width);

                            }
                            else
                                e.Graphics.FillRectangle(playerBrush, item.loc_x, item.loc_y, width * 0.75f, width * 0.75f);
                        }

                        //draw player cubes w/ name
                        foreach (Cube item in world.players.Values)
                        {
                            //draw player cube
                            float width = (item.width / 2);
                            this.playerBrush = new SolidBrush(Color.FromArgb(item.argb_color));
                            e.Graphics.FillRectangle(playerBrush, item.loc_x - width, item.loc_y - width, item.width, item.width);

                            //set font style, size and color
                            Font myFont = new Font("Arial", (float)(3f));
                            if (item.Mass > 5000)
                                myFont = new Font("Arial", (float)(70f));
                            if (item.Mass > 4000)
                                myFont = new Font("Arial", (float)(60f));
                            if (item.Mass > 3000)
                                myFont = new Font("Arial", (float)(50f));
                            if (item.Mass > 2000)
                                myFont = new Font("Arial", (float)(40f));
                            if (item.Mass > 1000)
                                myFont = new Font("Arial", (float)(30f));
                            if (item.Mass > 500)
                                myFont = new Font("Arial", (float)(20f));
                            if (item.Mass > 250)
                                myFont = new Font("Arial", (float)(10f));
                            if (item.Mass > 100)
                                myFont = new Font("Arial", (float)(5f));

                            this.nameBrush = new SolidBrush(Color.White);
                            SizeF fontsize = e.Graphics.MeasureString(item.Name, myFont);
                            //center the string
                            StringFormat format = new StringFormat();
                            format.LineAlignment = StringAlignment.Center;
                            format.Alignment = StringAlignment.Center;
                            //draw player string name
                            e.Graphics.DrawString(item.Name, myFont, (Brush)nameBrush, ((float)item.loc_x - item.width / 50), ((float)item.loc_y - item.width / 50), format);
                        }
                    }
                }
                //hideSetup();
                this.Invalidate();   //needed to repaint the world
                this.Focus();        //needed to focus on the split or mouse event
            }
            catch
            {
                //Connection to Server lost, close and print Error on screen
                Invoke(new Action(() =>
                {
                    if (this.world.players.ContainsKey(myUid))
                        this.labelConnectionLost.Show();
                    this.showSetup();
                }));
            }
        }

        /// <summary>
        /// Helper method called in paintWorld that Handles all the stats
        ///  that need to be updated on the screen
        /// during gameplay
        /// </summary>
        private void updateStats()
        {
            //update food label
            this.labelFoodValue.Text = string.Concat((object)this.world.food.Count);
            this.labelFoodValue.Invalidate();
            this.labelFoodValue.Update();

            //update players label
            this.labelPlayersValue.Text = string.Concat((object)this.world.players.Count);
            this.labelFoodValue.Invalidate();
            this.labelFoodValue.Update();

            //calculate team Mass/score
            double teamscore = 0;
            //go through each cube and find team cubes
            foreach (Cube player in this.world.players.Values)
            {
                //increment teamscore for each team cube
                if (player.team_id == this.myUid)
                {
                    teamscore += player.Mass;
                }
            }
            //if not team cubes exist/ cube is not split, just get the mass
            if (teamscore == 0)
                teamscore = world.players[myUid].Mass;

            //set the team mass
            if (this.world.players.ContainsKey(myUid))
                this.world.players[myUid].teamMass = teamscore;

            this.teamMass = teamscore;

            if (teamMass > (lastMass + world.player_start_mass) && teamMass < 2000)
            {
                teamPlayersEat++;
                teamFoodEat += ((int)teamMass - (int)lastMass);
                
            }
            else if(teamMass > (lastMass + 100))
            {
                teamPlayersEat++;
                teamFoodEat +=((int)teamMass - (int)lastMass);

            }
            lastMass = teamMass;

            //set score
            if (teamscore > myScore)
            {
                this.labelScoreValue.Text = (int)teamscore + "";
                this.labelScoreDeadValue.Text = (int)teamscore + "";
                this.myScore = (int)teamscore;
            }

            //set score
            if (myHighScore < myScore)
            {
                this.labelHighScoreValueDead.Text = myScore + "";
                myHighScore = myScore;
            }

            //set time alive for end of game
            TimeSpan durration = DateTime.Now - this.startTime;
            String durrationString = new TimeSpan(durration.Hours, durration.Minutes, durration.Seconds).ToString();
            this.labelTimeAliveValue.Text = durrationString;


            this.labelPlayersEatenValue.Text = teamPlayersEat + "";
            this.labelFoodEatenValue.Text = (myScore - teamFoodEat - world.player_start_mass) + "";

            //set team score for end of game
            this.labelMassValue.Text = (int)teamscore + "";

            this.world.players[myUid].highestScore = myScore;
        }

        /// <summary>
        /// Sends cube position to server
        /// </summary>
        private void sendPosition(int scale)
        {
            //will only send position if player cube exists.
            Cube cube = new Cube();
            if (!this.world.players.TryGetValue(myUid, out cube))
                return;
            //once player cube exists, send location to server
            //send as '(move,x,y)\n'
            try
            {
                int px = this.PointToClient(Control.MousePosition).X + (int)cube.loc_x - (int)(Width / 2f);
                int py = this.PointToClient(Control.MousePosition).Y + (int)cube.loc_y - (int)(Height / 2f);
                if (teamMass > 2500)
                {
                    px = this.PointToClient(Control.MousePosition).X + (int)cube.loc_x - (int)(Width / 2f / scale);
                    py = this.PointToClient(Control.MousePosition).Y + (int)cube.loc_y - (int)(Height / 2f / scale);
                }

                Network.Send(this.mySocket, "(move, " + px + ", " + py + ")\n");
            }
            catch (Exception)
            {
                //Connection to Server lost, close and print Error on screen
                try
                {
                    mySocket.Shutdown(SocketShutdown.Both);
                    mySocket.Close();
                }
                catch { }
                //if error occured then show message and showSetup
                Invoke(new Action(() =>
                {
                    this.labelConnectionLost.Show();
                    this.showSetup();
                }));
            }

        }

        /// <summary>
        /// Sends cube split info to server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendSplit(object sender, KeyEventArgs e)
        {
            //will only send split if player cube exists.
            Cube cube = new Cube();
            if (!this.world.players.TryGetValue(myUid, out cube))
                return;
            try
            {
                //if space key is pressed, send message to server
                if (e.KeyCode == Keys.Space)
                {
                    //send message as '(split,x,y)\n'
                    int px = this.PointToClient(Control.MousePosition).X + (int)cube.loc_x - (int)(Width / 2f / this.scaleFactor);
                    int py = this.PointToClient(Control.MousePosition).Y + (int)cube.loc_y - (int)(Height / 2f / this.scaleFactor);
                    Network.Send(this.mySocket, "(split," + px / 2 + "," + py / 2 + ")\n");
                }

                //magic key controller
                else if (e.KeyCode == Keys.Tab)
                {
                    if (magicScaleKey)
                        magicScaleKey = false;
                    else
                        magicScaleKey = true;
                }
            }
            catch //do nothing
            {
            }

        }



        /// <summary>
        /// helper method to hide the setup screen.
        /// -name label/textbox, server label/textbox, title,  and play button
        /// -stats
        /// </summary>
        private void hideSetup()
        {
            //hide setup labels
            this.TextBoxName.Hide();
            this.TextBoxServer.Hide();
            this.LabelName.Hide();
            this.LabelServer.Hide();
            this.buttonPlay.Hide();
            this.checkBoxColor.Hide();
            this.labelTitle.Hide();
            this.labelConnectionLost.Hide();

            if (teamMass == world.player_start_mass)
            {
                //show statistic lables
                this.labelMass.Show();
                this.labelMassValue.Show();
                this.labelFPS.Show();
                this.labelFPSValue.Show();
                this.labelFood.Show();
                this.labelFoodValue.Show();
                this.labelScore.Show();
                this.labelScoreValue.Show();
                this.labelPlayersValue.Show();
                this.labelPlayer.Show();
            }

            //Hide restart
            this.panelDead.Hide();
            this.Invalidate();
            this.Update();
        }


        /// <summary>
        /// helper method to show the setup screen.
        /// -name label/textbox, server label/textbox, title,  and play button
        /// -stats
        /// </summary>
        private void showSetup()
        {
            //show setuplables
            this.TextBoxName.Show();
            this.TextBoxServer.Show();
            this.LabelName.Show();
            this.LabelServer.Show();
            this.buttonPlay.Show();
            this.checkBoxColor.Show();
            this.labelTitle.Show();

            //hide stats
            this.labelMass.Hide();
            this.labelMassValue.Hide();
            this.labelFPS.Hide();
            this.labelFPSValue.Hide();
            this.labelFood.Hide();
            this.labelFoodValue.Hide();
            this.labelScore.Hide();
            this.labelScoreValue.Hide();
            this.labelPlayer.Hide();
            this.labelPlayersValue.Hide();

            isAlive = true;
        }


        /// <summary>
        /// helper method to show the setup screen.
        /// -name label/textbox, server label/textbox, title and play button
        /// -stats
        /// </summary>
        private void showStats(bool alive)
        {
            //if player has died
            if (!alive)
            {
                //Show Retart
                this.panelDead.Show();
                this.panelDead.Update();
                this.Update();
                this.Invalidate();
                this.Refresh();

                //hide stats
                this.labelMass.Hide();
                this.labelMassValue.Hide();
                this.labelFPS.Hide();
                this.labelFPSValue.Hide();
                this.labelFood.Hide();
                this.labelFoodValue.Hide();
                this.labelScore.Hide();
                this.labelScoreValue.Hide();
            }
        }
    }
}
