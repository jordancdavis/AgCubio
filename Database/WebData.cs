/*

Ag Cubio WebData
Jordan Davis & Jacob Osterloh
December 9, 2015
Ps9 - cs3500

*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgCubio
{
    /// <summary>
    /// An object to hold data from the agCubio database
    /// </summary>
    public class WebData
    {
        /// All valuse used for WebData Object

        public int index { get; set; }
        public int rank { get; set; }
        public double maxMass { get; set; }
        public string name { get; set; }
        public List<string> playersEaten;
        public int foodEaten { get; set; }
        public int cubesEaten { get; set; }
        public DateTime timeOfDeath { get; set; }
        public TimeSpan span { get; set; }
        public string timeAlive { get; set; }
        public List<string> top5;


        /// <summary>
        /// creates a WebData object.
        /// index, rank, maxMass, name, playersEaten, foodEaten, cubesEaten, timeAlive, top5
        /// </summary>
        public WebData()
        {
            index = 0;
            rank = 0;
            maxMass = 0;
            name = "";
            playersEaten = new List<string>();
            foodEaten = 0;
            cubesEaten = 0;
            timeAlive = "00:00:00";
            top5 = new List<string>();
        }
    }
}
