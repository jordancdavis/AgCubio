/*

Ag Cubio Cubes
Jordan Davis & Jacob Osterloh
December 9, 2015
Ps9 - cs3500

*/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgCubio
{
    /// <summary>
    /// Creates a Cube object for the game AgCubio
    /// Each cube stores information needed to play the game
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Cube
    {
        /// <summary>
        /// unique ID for a cube
        /// </summary>
        [JsonProperty]
        public int uid { get; set; }

        /// <summary>
        /// team ID for a group of split cubes
        /// </summary>        
        [JsonProperty]
        public int team_id { get; set; }

        /// <summary>
        /// color of the cube
        /// </summary>
        [JsonProperty]
        public int argb_color { get; set; }

        /// <summary>
        /// players name
        /// food cubes are set to ""
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// If cube is food = true;
        /// if cube is player = false;
        /// </summary>
        [JsonProperty]
        public Boolean food { get; set; }

        /// <summary>
        /// Cube's mass.
        /// </summary>
        [JsonProperty]
        public double Mass
        {
            get
            { return this.mass; }
            set
            {
                this.mass = value;
                this.width = (int)Math.Pow(this.mass, 0.65);
                setEdges(value, this.loc_x, this.loc_y);
            }
        }


        /// <summary>
        /// the players highest score reached
        /// the Highest mass of a player
        /// </summary>
        public int highestScore { get; set; }


        /// <summary>
        /// mass of the cube
        /// </summary>
        private double mass;

        //
        //Position Properties
        //

        /// <summary>
        /// ocation on X axis
        /// </summary>
        [JsonProperty]
        public float loc_x { get; set; }

        /// <summary>
        /// location on Y axis
        /// </summary>
        [JsonProperty]
        public float loc_y { get; set; }


        /// <summary>
        /// Determines if food is a virus or regularFood
        /// </summary>
        [JsonProperty]
        public Boolean isVirus { get; set; }

        /// <summary>
        /// width of the cube
        /// sqrt of mass
        /// </summary>
        public int width { get; private set; }

        /// <summary>
        /// top position of cube
        /// </summary>
        public float top { get; set; }

        /// <summary>
        /// bottom position of cube
        /// </summary>
        public float bottom { get; set; }

        /// <summary>
        /// left position of cube
        /// </summary>
        public float left { get; set; }

        /// <summary>
        /// right position of cube
        /// </summary>
        public float right { get; set; }

        /// <summary>
        /// total mass of a split cube
        /// </summary>
        public double teamMass { get; set; }

        /// <summary>
        /// used to increment uid
        /// </summary>
        public static int idCount;

        /// <summary>
        /// Allows cubes to stay split and then merge again
        /// </summary>
        public bool canMerge;

        /// <summary>
        /// momentum remaining
        /// </summary>
        private float remainingMomentum;

        /// <summary>
        /// momentum on the x axis
        /// </summary>
        private float x_mom;

        /// <summary>
        /// momentum on the y axis
        /// </summary>
        private float y_mom;

        /// <summary>
        /// time cubes are split
        /// </summary>
        public DateTime splitTime;

        /// <summary>
        /// Amount of food the cube has eaten
        /// </summary>
        public int foodEaten { get; set; }

        /// <summary>
        /// rank of the player
        /// </summary>
        public int rank { get; set; }

        /// <summary>
        /// list of all the players eaten
        /// </summary>
        public List<string> playersEaten;

        /// <summary>
        /// life span of cube
        /// </summary>
        public DateTime birth { get; set; }


        /// <summary>
        /// Cube constructor for player cubes
        /// </summary>    
        public Cube()
        {
            Name = "";
            uid = Cube.idCount++;
            team_id = uid;
            argb_color = 0;
            Mass = 10;
            food = true;
            highestScore = 0;
            loc_x = 100;
            loc_y = 100;
            teamMass = mass;
            foodEaten = 0;
            isVirus = false;
            canMerge = true;
            rank = 0;
            playersEaten = new List<string>();
            birth = DateTime.Now;

            //set: top, bottom, left and right endges
            setEdges(Mass, loc_x, loc_y);
        }

        /// <summary>
        /// Helper method to set the Edges of a cube.
        /// </summary>
        /// <param name="mass"></param>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        public void setEdges(double mass, float _x, float _y)
        {
            //MATH
            top = (float)_y + (width / 2);
            bottom = (float)_y - (width / 2);
            left = (float)_x - (width / 2);
            right = (float)_x + (width / 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="minimumMass"></param>
        public void attrition(double rate, double minimumMass = 200.00)
        {
            //cant become smaller than minimumMass
            if (this.Mass <= minimumMass)
                return;
            //MATH
            this.Mass -= Math.Sqrt(this.Mass) / rate;
        }

        /// <summary>
        /// applies momentum to a cube when it is split in x and y directions
        /// </summary>
        public void applyMomentum()
        {
            //once momentum is emtpy stop extending
            if ((double)this.remainingMomentum < 0.00)
                return;
            --this.remainingMomentum;

            //extend cubes from original location.
            this.loc_x += this.x_mom;
            this.loc_y += this.x_mom;
        }

        /// <summary>
        /// initializes the momentum variables and remaining momentum
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="distance"></param>
        public void setMomentum(float x, float y, int distance)
        {
            this.x_mom = x;
            this.y_mom = y;
            this.remainingMomentum = (float) distance;
        }

        /// <summary>
        /// Determines whether a cube is "inside" of another cube
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool inside(Cube other)
        {
            if((double) this.loc_x < (double) other.right && (double) this.loc_x > (double) other.left 
                && (double) this.loc_y < (double)other.top)
            {
                return (double)this.loc_y > (double)other.bottom;
            }
            return false;
        }

        /// <summary>
        /// Determines if cubes are overlapping so that they can look clean and organized
        /// </summary>
        /// <param name="others"></param>
        /// <param name="min_x_overlap"></param>
        /// <param name="min_y_overlap"></param>
        /// <param name="com_x"></param>
        /// <param name="com_y"></param>
        /// <returns></returns>
        public bool overlap(LinkedList<Cube> others, out float min_x_overlap, out float min_y_overlap, out float com_x, out float com_y)
        {
            //MATH
            min_x_overlap = 0.0f;
            min_y_overlap = 0.0f;
            int num1 = 1;
            com_x = this.loc_x;
            com_y = this.loc_y;
            bool flag = false;
            foreach (Cube cube in others)
            {
                //check if the cubes are different and split
                if (cube.uid != this.uid && (!cube.canMerge || !this.canMerge))
                {
                    float num2 = (float)(this.width + cube.width + 2) / 
                        2f - Math.Abs(this.loc_x - cube.loc_x);
                    float num3 = (float)(this.width + cube.width + 2) / 
                        2f - Math.Abs(this.loc_y - cube.loc_y);

                    //check if they are overlaping.
                    if ((double)num2 > 0.0 && (double)num3 > 0.0)
                    {
                        //cubes are overlapping. 
                        flag = true;
                        com_x += cube.loc_x;
                        com_y += cube.loc_y;
                        ++num1;
                        if ((double)num2 > (double)num3)
                            min_y_overlap = num3;
                        else
                            min_x_overlap = num2;
                    }
                }
            }
            com_x /= (float)num1;
            com_y /= (float)num1;
            return flag;
        }

        /// <summary>
        /// Equals method for Cube and object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if ((Cube)obj == null)
                return false;
            return this.uid == ((Cube)obj).uid;
        }

        /// <summary>
        /// Equals method for cubes
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals (Cube obj)
        {
            return this.uid == obj.uid;
        }

        /// <summary>
        /// Get hash function (uid)
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.uid;
        }


    }


}
