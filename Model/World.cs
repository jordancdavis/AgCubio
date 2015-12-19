/*

Ag Cubio World
Jordan Davis & Jacob Osterloh
December 9, 2015
Ps9 - cs3500

*/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AgCubio
{
    /// <summary>
    /// Responsible for storing the current state of the game, including the status and location of
    /// every object in the game
    /// </summary>
    public class World
    {
        //World parameters from XML document

        /// <summary>
        /// world width
        /// </summary>
        public readonly int width = 2500;   //2000 or 5000

        /// <summary>
        /// world height
        /// </summary>
        public readonly int height = 2500;  //2000 or 5000

        /// <summary>
        /// max distance a split can go
        /// </summary>
        public readonly int max_split_distance = 100;

        /// <summary>
        /// top speed of small cubes
        /// </summary>
        public readonly double top_speed = 2.5;

        /// <summary>
        /// min speed of large cubes
        /// </summary>
        public readonly double low_speed = 0.2;

        /// <summary>
        /// speed that cube dissolves / loses mass
        /// </summary>
        public readonly double attrition_rate = 200.0;

        /// <summary>
        /// value of the food
        /// </summary>
        public readonly int food_value = 1;


        /// <summary>
        /// virus value
        /// </summary>
        public readonly int virus_value = 250;

        /// <summary>
        /// ammount of viruses
        /// </summary>
        public readonly int virus_ammount = 25;

        /// <summary>
        /// counts the amount of viruses in world
        /// </summary>
        public int virusCount;

        /// <summary>
        /// player starting mass
        /// </summary>
        public readonly int player_start_mass = 50;

        /// <summary>
        /// amount of food in the world.
        /// server should update one food per heartbeat if below this threshold
        /// </summary>
        public readonly int max_food = 5000;

        /// <summary>
        /// min size in order to split
        /// player not allowed to split if below this mass
        /// </summary>
        public readonly int min_split_mass = 100;

        /// <summary>
        /// total amout of splits a player can have at any given moment
        /// test server doesnt do this. try 10 - 20
        /// </summary>
        public readonly int max_splits = 5;

        /// <summary>
        /// how much a cube must cover another cube to consume it
        /// </summary>
        public readonly double absorb_constant = 1.25;

        /// <summary>
        /// largest possible view of the world
        /// </summary>
        public readonly double max_view_range = 2500.0;

        /// <summary>
        /// how many updates the server should attempt to execute per second
        /// NOTE: adequate work will simply update the world as fast as possible.
        /// </summary>
        public readonly int heartbeats_per_second = 25;

        /// <summary>
        /// random number used for random things.
        /// </summary>
        private Random random;

        /// <summary>
        /// count to determine a new color.
        /// </summary>
        private int colorCount;

        /// <summary>
        /// heartbeats
        /// </summary>
        public int heartbeats;

        /// <summary>
        /// dictionary for player cubes
        /// </summary>
        public Dictionary<int, Cube> players;

        /// <summary>
        /// dictionary for food cubes
        /// </summary>
        public Dictionary<int, Cube> food;

        /// <summary>
        /// dictionary for split player cubes
        /// </summary>
        public Dictionary<int, LinkedList<Cube>> splitPlayers;

        /// <summary>
        /// dictionary for list of names that each player has eaten
        /// </summary>
        public Dictionary<int, List<Cube>> namedEats;

        /// <summary>
        /// used to give players their highest rankings
        /// </summary>
        public List<Cube> rankings;


        /// <summary>
        /// Creates a world for AgCubio through an XML file
        /// </summary>
        /// <param name="filename"></param>
        public World(string filename)
        {
            //check for filename
            if (string.IsNullOrWhiteSpace(filename))
                return;
            //load XML doc
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filename);

            //initialize variables
            this.width = int.Parse(xmlDocument.DocumentElement.SelectSingleNode("/parameters/width").InnerText);
            this.height = int.Parse(xmlDocument.DocumentElement.SelectSingleNode("/parameters/height").InnerText);
            this.max_split_distance = int.Parse(xmlDocument.DocumentElement.SelectSingleNode("/parameters/max_split_distance").InnerText);
            this.top_speed = float.Parse(xmlDocument.DocumentElement.SelectSingleNode("/parameters/top_speed").InnerText);
            this.low_speed = float.Parse(xmlDocument.DocumentElement.SelectSingleNode("/parameters/low_speed").InnerText);
            this.attrition_rate = (double)float.Parse(xmlDocument.DocumentElement.SelectSingleNode("/parameters/attrition_rate").InnerText);
            this.food_value = int.Parse(xmlDocument.DocumentElement.SelectSingleNode("/parameters/food_value").InnerText);
            this.player_start_mass = int.Parse(xmlDocument.DocumentElement.SelectSingleNode("/parameters/player_start_mass").InnerText);
            this.min_split_mass = int.Parse(xmlDocument.DocumentElement.SelectSingleNode("/parameters/min_split_mass").InnerText);
            this.absorb_constant = (double)float.Parse(xmlDocument.DocumentElement.SelectSingleNode("/parameters/absorb_constant").InnerText);
            this.max_view_range = (double)float.Parse(xmlDocument.DocumentElement.SelectSingleNode("/parameters/max_view_range").InnerText);
            this.heartbeats_per_second = int.Parse(xmlDocument.DocumentElement.SelectSingleNode("/parameters/heartbeats_per_second").InnerText);

            //create Dictionaries
            this.players = new Dictionary<int, Cube>();
            this.food = new Dictionary<int, Cube>();
            this.splitPlayers = new Dictionary<int, LinkedList<Cube>>();
            this.namedEats = new Dictionary<int, List<Cube>>();
            this.rankings = new List<Cube>();

            //set random numbers
            random = new Random();
            colorCount = random.Next(0, 31);
            virusCount = 0;
        }


        /// <summary>
        /// Creates a new World for AgCubio
        /// Note: this is the backup plan. try using XML filepath first.
        /// </summary>
        public World()
        {
            this.players = new Dictionary<int, Cube>();
            this.food = new Dictionary<int, Cube>();
            this.splitPlayers = new Dictionary<int, LinkedList<Cube>>();
            this.namedEats = new Dictionary<int, List<Cube>>();
            this.rankings = new List<Cube>();
            random = new Random();
            colorCount = random.Next(0, 31);
            virusCount = 0;
        }


        /// <summary>
        /// This method is used for the server to add a player into the world.
        /// initializes all the data.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Cube addPlayer(string name)
        {
            lock (this)
            {
                //create a cube and all of its data
                Cube cube = new Cube();
                cube.Mass = (double)player_start_mass;
                cube.Name = name;
                cube.food = false;
                cube.loc_x = (float)random.Next(width);
                cube.loc_y = (float)random.Next(height);
                cube.argb_color = getColor();
                cube.rank = rankings.Count + 1;
                cube.setEdges(cube.Mass, cube.loc_x, cube.loc_y);
                namedEats.Add(cube.uid, new List<Cube>());

                //add cube to players
                players[cube.uid] = cube;
                rankings.Add(cube);
                return cube;
            }
        }

        /// <summary>
        /// Grows food in the world
        /// 
        /// One food should be randomly generated and placed on the world per heartbeat. Excellent work will 
        /// make this amount easily variable (via a constant) so that game play tweaking can occur.
        ///
        /// Food Eating Functionality Requirement: Any time a player is on top of a food, the player's mass 
        /// should be increased by the size of the food and the food destroyed. Note: anytime any cube 
        /// (food/player) is destroyed, a "final message" needs to be sent to every client with the 
        /// cube mass set to 0.
        ///
        /// Note: how will implement the above is up to you but we expect a easily understandable 
        /// algorithm that is reasonably efficient. For example, 
        /// you could check every food cube vs. every player cube every heartbeat of the game. 
        /// Do you think this is a reasonable algorithm? Your README should discuss your choice of implementation.
        ///
        /// Another possible way to show excellence is to allow food to randomly grow larger (mass++). 
        /// This should not happen often and should be tweakable by a parameter to improve the game experience.
        /// 
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="foodPiece"></param>
        /// <returns></returns>
        public bool growFood(double rate, out Cube foodPiece)
        {
            lock (this)
            {
                //check food count , and random rare for growth
                if (this.food.Count < this.max_food && this.random.NextDouble() < rate)
                {
                    //create a food cube
                    foodPiece = new Cube();
                    foodPiece.loc_x = random.Next() % width;
                    foodPiece.loc_y = random.Next() % height;
                    foodPiece.argb_color = getColor();
                    foodPiece.food = true;
                    foodPiece.Name = "";
                    foodPiece.Mass = food_value;
                    foodPiece.isVirus = false;

                    //check if a virus is needed in the world
                    if (virusCount < virus_ammount)
                    {
                        //set the food to look like a virus
                        foodPiece.Mass = virus_value;
                        foodPiece.isVirus = true;
                        foodPiece.argb_color = Color.Lime.ToArgb();
                        virusCount++;
                    }

                    //set edges of cube
                    foodPiece.setEdges(foodPiece.Mass, foodPiece.loc_x, foodPiece.loc_y);

                    //add cube to food
                    this.food[foodPiece.uid] = foodPiece;
                    return true;
                }
                foodPiece = (Cube)null;
                return false;
            }
        }

        /// <summary>
        /// In the absence of a move command, cubes for a player should not be moved. 
        /// If a move command comes from the client, all cubes associated with the player 
        /// should move toward that spot.
        ///
        /// Overlapping: Split cubes are not allowed to overlap(by more than a little bit) 
        /// until their split time is elapsed.How you choose to implement this is up to you 
        /// but will have a big impact on the "look and feel" of the game.For example: if you 
        /// simply don't move cubes that are overlapped, you will see cubes "trapped/stopped" when 
        /// they accidentally get set to overlap while splitting. If you simply don't move cubes that 
        /// would overlap because of the move, you will find your cubes "freeze up" as they all try 
        /// to go to the same spot.
        ///
        /// The Edge of the World You should not allow cubes to leave the world.Some overlap 
        /// (up to say 30% of the width of the cube is allowable)
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void movePlayer(int uid, int x, int y)
        {
            lock (this)
            {
                //check if the player has split cubes
                LinkedList<Cube> team;
                if (this.splitPlayers.TryGetValue(uid, out team))
                {
                    foreach (Cube player in team)
                    {
                        //move each cube in team
                        player.applyMomentum();
                        moveCube(player, x, y);
                    }
                    handleTeamCollisions(team);
                }
                else
                {
                    //player is not split so move the cube if player exists
                    Cube cube;
                    if (!this.players.TryGetValue(uid, out cube))
                    {
                        return;
                    }
                    moveCube(players[uid], x, y);
                }
            }

        }

        /// <summary>
        /// This is a helper method for moving a group of cubes.
        /// Prevents them from overlaping with eachother/eating eachother.
        /// Also makes it look very nice.
        /// </summary>
        /// <param name="team"></param>
        private void handleTeamCollisions(LinkedList<Cube> team)
        {
            //create a time to check for when its okay to group back up
            DateTime now = DateTime.Now;
            foreach (Cube cube in team)
            {
                //once split time is up allow a merge
                if (DateTime.Compare(now, cube.splitTime) > 0)
                    cube.canMerge = true;

                //call overlap from cube class. 
                float min_x_overlap;
                float min_y_overlap;
                float com_x;
                float com_y;
                //if the cube is overlaping another cube. move it off the cube.
                if (cube.overlap(team, out min_x_overlap, out min_y_overlap, out com_x, out com_y))
                {
                    //math that works out the kinks. MAGIC DONT TOUCH.
                    min_x_overlap = Math.Min(min_x_overlap, 10f);
                    min_y_overlap = Math.Min(min_y_overlap, 10f);
                    if ((double)com_x > (double)cube.loc_x)
                        cube.loc_x -= min_x_overlap;
                    else
                        cube.loc_x += min_x_overlap;
                    if ((double)com_y > (double)cube.loc_y)
                        cube.loc_y -= min_y_overlap;
                    else
                        cube.loc_y += min_y_overlap;
                }
            }
        }


        /// <summary>
        /// In the absence of a move command, cubes for a player should not be moved. 
        /// If a move command comes from the client, all cubes associated with the player 
        /// should move toward that spot.
        ///
        /// Overlapping: Split cubes are not allowed to overlap(by more than a little bit) 
        /// until their split time is elapsed.How you choose to implement this is up to you 
        /// but will have a big impact on the "look and feel" of the game.For example: if you 
        /// simply don't move cubes that are overlapped, you will see cubes "trapped/stopped" when 
        /// they accidentally get set to overlap while splitting. If you simply don't move cubes that 
        /// would overlap because of the move, you will find your cubes "freeze up" as they all try 
        /// to go to the same spot.
        ///
        /// The Edge of the World You should not allow cubes to leave the world.Some overlap 
        /// (up to say 30% of the width of the cube is allowable)
        /// </summary>
        /// <param name="player"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void moveCube(Cube player, int x, int y)
        {

            //allows cube to remain in place
            if ((double)Math.Abs(((float)x - player.loc_x)) < 10.0 && (double)Math.Abs(((float)y - player.loc_y)) < 10.0)
                return;

            //set the speed of the movement
            double speed = (top_speed - ((float)(player.Mass / 2500)));

            //limit low speed
            if ((double)speed < (double)low_speed)
                speed = low_speed;

            //apply speed as how much cube is allowed to move.
            player.loc_x += (((float)x - player.loc_x) / (float)Math.Sqrt((double)((float)x - player.loc_x) *
                (double)((float)x - player.loc_x) + (double)((float)y - player.loc_y) * (double)((float)y - player.loc_y)) * (float)speed);
            player.loc_y += (((float)y - player.loc_y) / (float)Math.Sqrt((double)((float)x - player.loc_x) *
                (double)((float)x - player.loc_x) + (double)((float)y - player.loc_y) * (double)((float)y - player.loc_y)) * (float)speed);

            //prevent cube from going off the edge
            handleEdge(player);

        }



        /// <summary>
        /// Any time a larger cube significantly overlaps a smaller cube, the smaller cube should 
        /// be absorbed (all mass transferred) into the bigger cube and the smaller cube's demise 
        /// should be broadcast to all clients and the world updated.
        ///
        /// Note: if the eaten cube is part of a "split", then you must update any data structures 
        /// tracking this information.
        ///
        /// IMPORTANT Note: if the eaten cube is part of a "split" AND is the original cube of the player, 
        /// PRESERVE the unique id of the cube! The easiest way to do this is to swap the unique id of the 
        /// original(now eaten) cube with one of its team cubes. (If there are no team cubes, then it is 
        /// game over for the player.)
        /// </summary>
        /// <returns></returns>
        public LinkedList<Cube> eatPlayers()
        {
            LinkedList<Cube> erased = new LinkedList<Cube>();
            lock (this)
            {
                //create two loops to check for overlap (from both sides)
                foreach (Cube cubePlayer1 in this.players.Values)
                {
                    foreach (Cube cubePlayer2 in this.players.Values)
                    {
                        //when we want to ourself
                        if (cubePlayer1 != cubePlayer2)
                        {
                            //handles team re-grouping.
                            if (cubePlayer1.team_id == cubePlayer2.team_id)
                            {
                                //check if its time to merge and if overlap has occured.
                                if (cubePlayer1.canMerge && cubePlayer2.canMerge && cubePlayer2.inside(cubePlayer1))
                                {
                                    //add cubes back together on the same team
                                    if (cubePlayer2.uid == cubePlayer2.team_id)
                                    {
                                        erased.AddFirst(cubePlayer1);
                                        cubePlayer2.Mass += cubePlayer1.Mass;
                                        if (cubePlayer2.Mass > cubePlayer2.highestScore)
                                            cubePlayer2.highestScore = (int)cubePlayer2.Mass;
                                        cubePlayer1.Mass = 0.0;
                                    }
                                    else
                                    {
                                        erased.AddFirst(cubePlayer2);
                                        cubePlayer1.Mass += cubePlayer2.Mass;
                                        if (cubePlayer1.Mass > cubePlayer1.highestScore)
                                            cubePlayer1.highestScore = (int)cubePlayer1.Mass;
                                        cubePlayer2.Mass = 0.0;
                                    }
                                }
                            }
                            //check which player is bigger and apply merge.
                            else if (cubePlayer1.Mass > cubePlayer2.Mass * this.absorb_constant && cubePlayer2.inside(cubePlayer1))
                            {
                                erased.AddFirst(cubePlayer2);
                                cubePlayer1.Mass += cubePlayer2.Mass;
                                if (cubePlayer1.Mass > cubePlayer1.highestScore)
                                    cubePlayer1.highestScore = (int) cubePlayer1.Mass;

                                //add the dead to the list of the player who at it.
                                List<Cube> names;
                                if (this.namedEats.TryGetValue(cubePlayer1.team_id, out names))
                                    this.namedEats[cubePlayer1.team_id].Add(cubePlayer2);
                                else
                                {
                                    names = new List<Cube>();
                                    names.Add(cubePlayer2);
                                    this.namedEats.Add(cubePlayer1.team_id,names);
                                }
                                cubePlayer2.Mass = 0.0;
                                //take player out of rankings to avoid impossible advancements
                                rankings.Remove(cubePlayer2);
                            }
                        }
                    }
                    //rank the players
                    rankPlayer(cubePlayer1);
                }
                this.erasePlayer(erased);
            }
            //return the list of erased player to update world
            return erased;
        }

        /// <summary>
        /// helper method that removes player from the world
        /// </summary>
        /// <param name="erased"></param>
        private void erasePlayer(LinkedList<Cube> erased)
        {
            //remove all the cubes given
            foreach (Cube cube1 in erased)
            {
                //remove cube from players
                this.players.Remove(cube1.uid);

                //remove cube from split players
                LinkedList<Cube> linkedList;
                if (this.splitPlayers.TryGetValue(cube1.team_id, out linkedList))
                {
                    if (linkedList.Count < 2)
                        throw new Exception();
                    linkedList.Remove(cube1);

                    //This fixes a bug that was weird and cause some random cube switching
                    if (cube1.uid == cube1.team_id)
                    {
                        Cube cube2 = Enumerable.First<Cube>((IEnumerable<Cube>)linkedList);
                        this.players.Remove(cube2.uid);
                        this.players.Add(cube1.team_id, cube2);
                        cube1.uid = cube2.uid;
                        cube2.uid = cube1.team_id;
                    }
                    if (linkedList.Count == 1)
                    {
                        this.splitPlayers.Remove(cube1.team_id);
                    }
                    linkedList = (LinkedList<Cube>)null;
                }
            }
        }

        /// <summary>
        /// Any time a larger cube significantly overlaps a smaller cube, the smaller cube should 
        /// be absorbed (all mass transferred) into the bigger cube and the smaller cube's demise 
        /// should be broadcast to all clients and the world updated.
        ///
        /// Note: if the eaten cube is part of a "split", then you must update any data structures 
        /// tracking this information.
        ///
        /// IMPORTANT Note: if the eaten cube is part of a "split" AND is the original cube of the player, 
        /// PRESERVE the unique id of the cube! The easiest way to do this is to swap the unique id of the 
        /// original(now eaten) cube with one of its team cubes. (If there are no team cubes, then it is 
        /// game over for the player.)
        /// </summary>
        /// <returns></returns>
        public LinkedList<Cube> eatFood()
        {
            lock (this)
            {
                LinkedList<Cube> cubes = new LinkedList<Cube>();
                try
                {
                    foreach (Cube cubePlayer in this.players.Values)
                    {
                        foreach (Cube cubeFood in this.food.Values)
                        {
                            //if player is over food we can eat it
                            if ((double)cubePlayer.loc_x - (double)(cubePlayer.width / 2) < (double)cubeFood.loc_x &&
                                (double)cubePlayer.loc_x + (double)(cubePlayer.width / 2) > (double)cubeFood.loc_x &&
                                (double)cubePlayer.loc_y - (double)(cubePlayer.width / 2) < (double)cubeFood.loc_y &&
                                (double)cubePlayer.loc_y + (double)(cubePlayer.width / 2) > (double)cubeFood.loc_y)
                            {
                                //check if food is a virus
                                if (cubeFood.isVirus)
                                {
                                    //if its a virus and we are bigger then blow up the cube (and virus)
                                    if (cubePlayer.Mass > (cubeFood.Mass * absorb_constant))
                                    {
                                        //remove virus.
                                        virusCount--;
                                        cubes.AddFirst(cubeFood);
                                        cubeFood.Mass = 0.0;
                                        //subtract mass from player
                                        cubePlayer.Mass = cubePlayer.Mass - (cubePlayer.Mass / 10);
                                        //split the cube a bunch of times
                                        for (int i = 0; i < max_splits; i++)
                                            splitPlayer(cubePlayer.uid, (int)cubePlayer.loc_x, (int)cubePlayer.loc_y);

                                        //Dont remove the following 3 lines. they are needed to fix a random exception.
                                        Cube empty = new Cube();
                                        empty.Mass = 0;
                                        players[empty.uid] = empty;
                                    }
                                }
                                else
                                {
                                    //not a virus. Remove food and add 1 to player mass.
                                    cubes.AddFirst(cubeFood);
                                    cubePlayer.Mass += cubeFood.Mass;

                                    //set highest scores
                                    if (cubePlayer.Mass > cubePlayer.highestScore)
                                        cubePlayer.highestScore = (int)cubePlayer.Mass;

                                    //increment food eaten
                                    cubePlayer.foodEaten++;
                                    cubeFood.Mass = 0.0;
                                }
                            }
                        }
                        foreach (Cube cube in cubes)
                        {
                            //this actually removes the food/virus
                            this.food.Remove(cube.uid);
                        }
                    }
                    //return the list of removed items.
                    return cubes;
                }
                catch
                {
                    //return the list of removed items.
                    return cubes;
                }

            }
        }

        /// <summary>
        /// The server must handle split nodes. I suggest a separate data structure from the main 
        /// cubes list which stores those cubes which are part of a split. Alternatively, you could
        /// augment your cube data structure to store this info.
        /// When a split request comes from a client, all cubes that belong to a given player and have 
        /// sufficient mass, are split(respecting the maximum splits constant). The split happens along 
        /// the line from each node to the requested position and distance should have some ratio to the 
        /// size of the cube.Excellent work will make this a "tweakable" value in order to optimize game 
        /// play(i.e., splitting too far makes the game less fun (for others) and splitting too short makes 
        /// the game less fun(for the player).
        ///
        /// Timer: When a cube splits it should have a time set.The cube should be marked as not being 
        /// allowed to merge until the time elapses.
        ///
        /// Momentum: When a cube splits, it should not immediately jump to the final "split point", 
        /// but should instead have a momentum that moves it smoothly toward that spot for a short period of time.
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void splitPlayer(int uid, int x, int y)
        {
            lock (this)
            {
                //if player is not split then this will create the team list
                LinkedList<Cube> cubes;
                if (!this.splitPlayers.TryGetValue(uid, out cubes))
                {
                    Cube cube = this.players[uid];
                    cubes = new LinkedList<Cube>();
                    cubes.AddFirst(cube);
                    cube.team_id = cube.uid;
                }

                LinkedList<Cube> team = new LinkedList<Cube>();
                foreach (Cube cube in cubes)
                {
                    //add the cube to the team
                    team.AddFirst(cube);
                    if (cube.Mass < (double)this.min_split_mass)
                        return;

                    //math (DONT TOUCH)
                    float num1 = (float)x - cube.loc_x;
                    float num2 = (float)y - cube.loc_y;
                    float num3 = (float)Math.Sqrt((double)num1 * (double)num1 + (double)num2 * (double)num2);
                    float num4 = (float)((double)cube.width * 5.0 / 120);
                    float x_mom = (num1 / num3) * num4;
                    float y_mom = (num2 / num3) * num4;

                    //modify original cube
                    cube.Mass /= 2;
                    cube.canMerge = false;
                    cube.splitTime = DateTime.Now.AddSeconds(10.0 + (cube.Mass / 100.0));

                    //create a new cube
                    Cube tempCube = new Cube();
                    tempCube.loc_x = cube.loc_x;
                    tempCube.loc_y = cube.loc_y;
                    tempCube.Mass = cube.Mass;
                    tempCube.highestScore = (int)cube.highestScore;
                    tempCube.argb_color = cube.argb_color;
                    tempCube.food = false;
                    tempCube.Name = cube.Name;
                    tempCube.canMerge = false;
                    tempCube.splitTime = DateTime.Now.AddSeconds(10.00 + (cube.Mass / 100.0));
                    tempCube.team_id = cube.team_id;
                    tempCube.setEdges(tempCube.Mass, tempCube.loc_x, tempCube.loc_y);

                    //move the cube away from the original cube
                    tempCube.setMomentum(x_mom, y_mom, this.heartbeats_per_second);
                    this.players[tempCube.uid] = tempCube;
                    team.AddFirst(tempCube);
                }

                //if no splits then do nothing
                if (team.Count <= 1)
                    return;

                //add split team to split players list
                this.splitPlayers[uid] = team;

            }

        }

        /// <summary>
        /// Rate player shrinks
        /// 
        /// At each heartbeat of the game every player cube should lose some portion of its mass. 
        /// Larger cubes should lose mass faster than smaller cubes. Cubes less than some mass 
        /// (say 200) should not lose mass. Cubes less than some mass (say 800) should only lose 
        /// mass very slowly. Cubes above 800 should rapidly start losing mass. 
        /// (Again this should be tweakable).
        /// 
        /// </summary>
        public void attrition()
        {
            lock (this)
            {
                foreach (Cube cube in players.Values)
                    cube.attrition(attrition_rate, 200.00);
            }
        }

        /// <summary>
        /// Prevents the cube from moving off of the edge of the world.
        /// </summary>
        public void handleEdge(Cube player)
        {
            //Math that prevents the cube from going out of bounds
            if ((double)player.loc_x + (double)(player.width / 2) > (double)this.width)
                player.loc_x = (float)(-(player.width / 2) + this.width) + (float)(this.random.NextDouble() * 4.0 - 2.0);
            if ((double)player.loc_x - (double)(player.width / 2) < 0.0)
                player.loc_x = (float)(player.width / 2) + (float)(this.random.NextDouble() * 2.0 - 1.0);
            if ((double)player.loc_y + (double)(player.width / 2) > (double)this.height)
                player.loc_y = (float)(-(player.width / 2) + this.height) + (float)(this.random.NextDouble() * 4.0 - 2.0);
            if ((double)player.loc_y - (double)(player.width / 2) >= 0.0)
                return;
            player.loc_y = (float)(player.width / 2) + (float)(this.random.NextDouble() * 2.0 - 1.0);
        }

        /// <summary>
        /// Method to determine a cubes color.
        /// This is a large list of colors so it was easiest to use switch case.
        /// 
        /// NOTE: all viruses are lime with a dark green border.
        /// 
        /// </summary>
        /// <returns></returns>
        public int getColor()
        {
            //increment color
            colorCount++;

            //keep color in range of our colors
            if (colorCount > 31)
                colorCount = 0;

            //get new color
            switch (colorCount)
            {
                case 0:
                    return Color.Aqua.ToArgb();
                case 1:
                    return Color.Blue.ToArgb();
                case 2:
                    return Color.BlueViolet.ToArgb();
                case 3:
                    return Color.Coral.ToArgb();
                case 4:
                    return Color.Crimson.ToArgb();
                case 5:
                    return Color.Cyan.ToArgb();
                case 6:
                    return Color.DeepPink.ToArgb();
                case 7:
                    return Color.DodgerBlue.ToArgb();
                case 8:
                    return Color.Green.ToArgb();
                case 9:
                    return Color.Gold.ToArgb();
                case 10:
                    return Color.HotPink.ToArgb();
                case 11:
                    return Color.Indigo.ToArgb();
                case 12:
                    return Color.Magenta.ToArgb();
                case 13:
                    return Color.Maroon.ToArgb();
                case 14:
                    return Color.Navy.ToArgb();
                case 15:
                    return Color.Orange.ToArgb();
                case 16:
                    return Color.OrangeRed.ToArgb();
                case 17:
                    return Color.Purple.ToArgb();
                case 18:
                    return Color.Red.ToArgb();
                case 19:
                    return Color.RoyalBlue.ToArgb();
                case 20:
                    return Color.Tomato.ToArgb();
                case 21:
                    return Color.Turquoise.ToArgb();
                case 22:
                    return Color.Violet.ToArgb();
                case 23:
                    return Color.Yellow.ToArgb();
                case 24:
                    return Color.Fuchsia.ToArgb();
                case 25:
                    return Color.Yellow.ToArgb();
                case 26:
                    return Color.Red.ToArgb();
                case 27:
                    return Color.Orange.ToArgb();
                case 28:
                    return Color.Green.ToArgb();
                case 29:
                    return Color.Blue.ToArgb();
                case 30:
                    return Color.Purple.ToArgb();
                case 31:
                    return Color.Lime.ToArgb();
                default: //lucky
                    return Color.Transparent.ToArgb();
            }
        }
        

        /// <summary>
        /// helper method to determine the players rank.
        /// loops through untill the player is in the correct index of dataset
        /// never decrements
        /// only keeps highest rank information
        /// </summary>
        /// <param name="c"></param>
        private void rankPlayer(Cube c)
        {
            if(rankings.Count > 1)
            {
                int index = rankings.IndexOf(c);

                if (index-1 >= 0)
                {
                    while (rankings[index].highestScore > rankings[index - 1].highestScore)
                    {
                        Cube cube = rankings[index];
                        rankings[index] = rankings[index- 1];
                        rankings[index - 1] = cube;
                        rankings[index-1].rank = rankings[index].rank - 1;
                    }
                }
                else
                {
                    if(rankings[1].highestScore > rankings[0].highestScore)
                    {
                        Cube b = rankings[1];
                        rankings[1] = rankings[0];
                        rankings[0] = b;
                    }
                }
            }

        }

    }

}

