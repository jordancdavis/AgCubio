/*

Ag Cubio Testing

Jordan Davis & Jacob Osterloh

November 17, 2015

Ps7 - cs3500

*/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace AgCubio
{
    [TestClass]
    public class UnitTest1
    {
        public Cube setCube()
        {
            Cube cube = new Cube();
            cube.argb_color = -1232134;
            cube.food = false;
            cube.highestScore = 1234;
            cube.loc_x = 65;
            cube.loc_y = 68;
            cube.Mass = 1024;
            cube.Name = "butterscotch";
            cube.setEdges(cube.Mass, cube.loc_x, cube.loc_y);
            cube.teamMass = cube.Mass;
            cube.team_id = 5000;
            cube.uid = 5000;
            return cube;
        }

        public Cube setFoodCube()
        {
            Cube cube = new Cube();
            cube.argb_color = -1232134;
            cube.food = true;
            cube.highestScore = 1234;
            cube.loc_x = 65;
            cube.loc_y = 68;
            cube.Mass = 1024;
            cube.Name = "butterscotch";
            cube.setEdges(cube.Mass, cube.loc_x, cube.loc_y);
            cube.teamMass = cube.Mass;
            cube.team_id = 23;
            cube.uid = 23;
            return cube;
        }

        public Cube setVirusCube()
        {
            Cube cube = new Cube();
            cube.argb_color = -1232134;
            cube.food = true;
            cube.isVirus = true;
            cube.highestScore = 1234;
            cube.loc_x = 65;
            cube.loc_y = 68;
            cube.Mass = 500;
            cube.Name = "butterscotch";
            cube.setEdges(cube.Mass, cube.loc_x, cube.loc_y);
            cube.teamMass = cube.Mass;
            cube.team_id = 23;
            cube.uid = 25;
            return cube;
        }

        public Cube setSameSpotCube()
        {
            Cube cube = new Cube();
            cube.argb_color = -1232134;
            cube.food = false;
            cube.highestScore = 1234;
            cube.loc_x = 65;
            cube.loc_y = 68;
            cube.Mass = 1024;
            cube.Name = "butterscotch";
            cube.setEdges(cube.Mass, cube.loc_x, cube.loc_y);
            cube.teamMass = cube.Mass;
            cube.team_id = 5000;
            cube.uid = 40;
            return cube;
        }
        public Cube setOtherCube()
        {
            Cube cube = new Cube();
            cube.argb_color = -1232134;
            cube.food = false;
            cube.highestScore = 1234;
            cube.loc_x = 10;
            cube.loc_y = 11;
            cube.Mass = 1024;
            cube.Name = "butterscotch";
            cube.setEdges(cube.Mass, cube.loc_x, cube.loc_y);
            cube.teamMass = cube.Mass;
            cube.team_id = 0;
            cube.uid = 1;
            return cube;
        }

        public Cube setOneMoreCube()
        {
            Cube cube = new Cube();
            cube.argb_color = -1232134;
            cube.food = false;
            cube.highestScore = 1234;
            cube.loc_x = 5;
            cube.loc_y = 30;
            cube.Mass = 1024;
            cube.Name = "butterscotch";
            cube.setEdges(cube.Mass, cube.loc_x, cube.loc_y);
            cube.teamMass = cube.Mass;
            cube.team_id = 0;
            cube.uid = 1;
            return cube;
        }

        [TestMethod]
        public void TestCube01()
        {
            Cube cube = setCube();

            Assert.AreEqual(cube.width, (int)Math.Pow(cube.Mass, 0.65));
            Assert.AreEqual(cube.top, (float)cube.loc_y + (cube.width / 2));
            Assert.AreEqual(cube.bottom, (float)cube.loc_y - (cube.width / 2));
            Assert.AreEqual(cube.left, (float)cube.loc_x - (cube.width / 2));
            Assert.AreEqual(cube.right, (float)cube.loc_x + (cube.width / 2));

            int theMass = (int)cube.Mass;
            int theHighestScore = (int)cube.highestScore;
            int theID = (int)cube.uid;
            string theName = cube.Name;
            int theColor = cube.argb_color;
            int theTID = (int)cube.team_id;
            bool foodBool = cube.food;
            int theTeamAss = (int)cube.teamMass;
            bool virus = cube.isVirus;
            int foodEaten = cube.foodEaten;
            int playersEaten = cube.playersEaten.Count;
        }

        [TestMethod]
        public void TestWorld01()
        {
            Cube cube = setCube();
            World world = new World();
            world.players.Add(cube.uid, cube);
            Assert.IsTrue(world.players.ContainsKey(cube.uid));
        }


        [TestMethod]
        public void TestWorldXML()
        {
            bool testPasssed = false;
            if (File.Exists("C:\\Users\\osterloh\\CS 3500 Assignments\\Aviato\\AgCubio\\Resources\\world_parameters.xml"))
            {
                //try to creat w/ filename first
                World world = new World("C:\\Users\\osterloh\\CS 3500 Assignments\\Aviato\\AgCubio\\Resources\\world_parameters.xml");
                testPasssed = true;
            }
            Assert.IsTrue(testPasssed);
        }
        [TestMethod]
        public void TestAttrition01()
        {
            Cube cube = setCube();
            cube.attrition(.5, 200);
            World world = new World();
            world.players.Add(cube.uid, cube);
            Assert.IsTrue(world.players.ContainsKey(cube.uid));
        }

        [TestMethod]
        public void TestAttrition02()
        {
            Cube cube = setCube();
            cube.Mass = 100;
            cube.attrition(.5, 150);
            World world = new World();
            world.players.Add(cube.uid, cube);
            Assert.IsTrue(world.players.ContainsKey(cube.uid));
        }

        [TestMethod]
        public void TestApplyMomentum01()
        {
            Cube cube = setCube();
            cube.applyMomentum();
            World world = new World();
            world.players.Add(cube.uid, cube);
            Assert.IsTrue(world.players.ContainsKey(cube.uid));
        }

        [TestMethod]
        public void TestApplyMomentum02()
        {
            Cube cube = setCube();
            cube.setMomentum(2, 3, -1);
            cube.applyMomentum();
            World world = new World();
            world.players.Add(cube.uid, cube);
            Assert.IsTrue(world.players.ContainsKey(cube.uid));
        }

        [TestMethod]
        public void TestSetMomentum01()
        {
            Cube cube = setCube();
            cube.setMomentum(2, 4, 12);
            World world = new World();
            world.players.Add(cube.uid, cube);
            Assert.IsTrue(world.players.ContainsKey(cube.uid));
        }

        [TestMethod]
        public void TestInside01()
        {
            Cube cube = setCube();
            Cube other = setCube();
            Assert.IsTrue(cube.inside(other)); 
            World world = new World();
            world.players.Add(cube.uid, cube);
            Assert.IsTrue(world.players.ContainsKey(cube.uid));
        }

        [TestMethod]
        public void TestInside02()
        {
            Cube cube = setCube();
            Cube other = setOtherCube();
            Assert.IsFalse(cube.inside(other));
            World world = new World();
            world.players.Add(cube.uid, cube);
            Assert.IsTrue(world.players.ContainsKey(cube.uid));
        }

        [TestMethod]
        public void TestOverlap01()
        {
            Cube cube = setCube();
            Cube other = setCube();
            LinkedList<Cube> others = new LinkedList<Cube>();
            others.AddFirst(other);
            float min_x_overlap;
            float min_y_overlap;
            float com_x;
            float com_y;
            Assert.IsFalse(cube.overlap(others, out min_x_overlap, out min_y_overlap, out com_x, out com_y));
            World world = new World();
            world.players.Add(cube.uid, cube);
            Assert.IsTrue(world.players.ContainsKey(cube.uid));
        }

        [TestMethod]
        public void TestOverlap02()
        {
            Cube cube = setCube();
            Cube other = setCube();
            other.uid = 2;
            cube.uid = 1;
            other.canMerge = false;
            LinkedList<Cube> others = new LinkedList<Cube>();
            others.AddFirst(other);
            float min_x_overlap;
            float min_y_overlap;
            float com_x;
            float com_y;
            Assert.IsTrue(cube.overlap(others, out min_x_overlap, out min_y_overlap, out com_x, out com_y));
            World world = new World();
            world.players.Add(cube.uid, cube);
            Assert.IsTrue(world.players.ContainsKey(cube.uid));
        }

        [TestMethod]
        public void TestOverlap03()
        {
            Cube cube = setCube();
            Cube other = setCube();
            other.uid = 2;
            cube.uid = 1;
            other.canMerge = false;
            LinkedList<Cube> others = new LinkedList<Cube>();
            others.AddFirst(cube);
            float min_x_overlap;
            float min_y_overlap;
            float com_x;
            float com_y;
            Assert.IsTrue(other.overlap(others, out min_x_overlap, out min_y_overlap, out com_x, out com_y));
            World world = new World();
            world.players.Add(cube.uid, cube);
            Assert.IsTrue(world.players.ContainsKey(cube.uid));
        }

        [TestMethod]
        public void TestOverlap04()
        {
            Cube cube = setOneMoreCube();
            Cube other = setOneMoreCube();
            other.uid = 2;
            cube.uid = 1;
            other.canMerge = false;
            LinkedList<Cube> others = new LinkedList<Cube>();
            others.AddFirst(cube);
            float min_x_overlap;
            float min_y_overlap;
            float com_x;
            float com_y;
            Assert.IsTrue(other.overlap(others, out min_x_overlap, out min_y_overlap, out com_x, out com_y));
            World world = new World();
            world.players.Add(cube.uid, cube);
            Assert.IsTrue(world.players.ContainsKey(cube.uid));
        }

        [TestMethod]
        public void TestObjectEquals01()
        {
            Cube cube = setCube();
            Object other = setOtherCube();
            Assert.IsFalse(other.Equals(cube));
            World world = new World();
            world.players.Add(cube.uid, cube);
            Assert.IsTrue(world.players.ContainsKey(cube.uid));
        }

        [TestMethod]
        public void TestObjectEquals02()
        {
            Cube cube = setCube();
            Object other = setOtherCube();
            Assert.IsFalse(other.Equals(null));
            World world = new World();
            world.players.Add(cube.uid, cube);
            Assert.IsTrue(world.players.ContainsKey(cube.uid));
        }

        [TestMethod]
        public void TestCubeEquals02()
        {
            Cube cube = setCube();
            Cube other = setOtherCube();
            Assert.IsFalse(cube.Equals(other));
            World world = new World();
            world.players.Add(cube.uid, cube);
            Assert.IsTrue(world.players.ContainsKey(cube.uid));
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            Cube cube = setCube();
            Cube other = setOtherCube();
            cube.GetHashCode();
            World world = new World();
            world.players.Add(cube.uid, cube);
            Assert.IsTrue(world.players.ContainsKey(cube.uid));
        }

        [TestMethod]
        public void TestAddPlayer01()
        {
            Cube cube = setCube();
            Cube other = setOtherCube();
            World world = new World();
            world.addPlayer("john");
            world.players.Add(cube.uid, cube);
            Assert.IsTrue(world.players.ContainsKey(cube.uid));
        }

        [TestMethod]
        public void TestGrowFood01()
        {
            Cube cube = setCube();
            Cube other = setOtherCube();
            cube.GetHashCode();
            World world = new World();
            Cube food = setCube();
            world.growFood(20, out food);
            world.players.Add(cube.uid, cube);
            Assert.IsTrue(world.players.ContainsKey(cube.uid));
        }

        [TestMethod]
        public void TestGrowFood02()
        {
            Cube cube = setCube();
            Cube other = setOtherCube();
            cube.GetHashCode();
            World world = new World();
            Cube food = setCube();
            world.growFood(0.1, out food);
            world.players.Add(cube.uid, cube);
            Assert.IsTrue(world.players.ContainsKey(cube.uid));
        }

        [TestMethod]
        public void TestMovePlayer01()
        {
            Cube cube = setCube();
            Cube other = setOtherCube();
            cube.GetHashCode();
            World world = new World();
            world.movePlayer(5000, 23, 24);
            Cube food = setCube();
            world.growFood(0.1, out food);
            world.players.Add(cube.uid, cube);
            world.movePlayer(5000, 23, 24);
            Assert.IsTrue(world.players.ContainsKey(cube.uid));
        }
        [TestMethod]
        public void TestMovePlayer02()
        {
            Cube cube = setCube();
            Cube other = setOtherCube();
            cube.GetHashCode();
            World world = new World();
            world.movePlayer(5000, 23, 24);
            Cube food = setCube();
            world.growFood(0.1, out food);
            LinkedList<Cube> cubes = new LinkedList<Cube>();
            world.splitPlayers.Add(cube.uid, cubes);
            world.splitPlayers.Add(other.uid, cubes);
            world.movePlayer(5000, 23, 24);
        }

        [TestMethod]
        public void TestTeamCollisions()
        {
            Cube cube = setCube();
            Cube sameSpot = setSameSpotCube();
            Cube other = setOtherCube();
            cube.GetHashCode();
            World world = new World();
            world.movePlayer(5000, 23, 24);
            Cube food = setCube();
            world.growFood(0.1, out food);
            LinkedList<Cube> cubes = new LinkedList<Cube>();
            cubes.AddFirst(cube);
            cubes.AddFirst(sameSpot);
            world.splitPlayers.Add(cube.uid, cubes);
            world.splitPlayers.Add(other.uid, cubes);
            LinkedList<Cube> playercubes = new LinkedList<Cube>();
            world.players.Add(cube.uid, cube);
            world.players.Add(other.uid, other);
            world.players.Add(sameSpot.uid, sameSpot);
            world.movePlayer(5000, 23, 24);
            world.moveCube(cube, 23, 24);
            world.moveCube(cube, 23, 24);
            world.eatPlayers();
            world.attrition();
            world.splitPlayer(5000, 24, 25);
            world.eatFood();
        }

        [TestMethod]
        public void TestColor()
        {
            World world = new World();
            Cube cube = new Cube();
            for(int i = 0; i < 34; i++)
            {
                cube.argb_color = world.getColor();
            }
        }

        [TestMethod]
        public void TestEatFood()
        {
            Cube cube = setCube();
            Cube sameSpot = setSameSpotCube();
            Cube other = setOtherCube();
            cube.GetHashCode();
            World world = new World();
            world.movePlayer(5000, 23, 24);
            Cube food = setFoodCube();
            world.food.Add(food.uid, food);
            LinkedList<Cube> cubes = new LinkedList<Cube>();
            cubes.AddFirst(cube);
            cubes.AddFirst(sameSpot);
            world.splitPlayers.Add(cube.uid, cubes);
            world.splitPlayers.Add(other.uid, cubes);
            LinkedList<Cube> playercubes = new LinkedList<Cube>();
            world.players.Add(cube.uid, cube);
            world.players.Add(other.uid, other);
            world.players.Add(sameSpot.uid, sameSpot);
            world.movePlayer(5000, 23, 24);
            world.moveCube(cube, 23, 24);
            world.moveCube(cube, 23, 24);
            world.eatPlayers();
            world.attrition();
            world.splitPlayer(5000, 24, 25);
            world.eatFood();
        }

        [TestMethod]
        public void TestVirusCube()
        {
            Cube cube = setCube();
            Cube sameSpot = setSameSpotCube();
            Cube other = setOtherCube();
            cube.GetHashCode();
            World world = new World();
            world.movePlayer(5000, 23, 24);
            Cube virus = setVirusCube();
            world.food.Add(virus.uid, virus);
            LinkedList<Cube> cubes = new LinkedList<Cube>();
            cubes.AddFirst(cube);
            cubes.AddFirst(sameSpot);
            world.splitPlayers.Add(cube.uid, cubes);
            world.splitPlayers.Add(other.uid, cubes);
            LinkedList<Cube> playercubes = new LinkedList<Cube>();
            world.players.Add(cube.uid, cube);
            world.players.Add(other.uid, other);
            world.players.Add(sameSpot.uid, sameSpot);
            world.movePlayer(5000, 23, 24);
            world.moveCube(cube, 23, 24);
            world.moveCube(cube, 23, 24);
            world.eatPlayers();
            world.attrition();
            world.splitPlayer(5000, 24, 25);
            world.eatFood();
        }


        [TestMethod]
        public void TestEdge()
        {
            Cube cube = setCube();
            Cube sameSpot = setSameSpotCube();
            Cube other = setOtherCube();
            cube.GetHashCode();
            World world = new World();
            world.movePlayer(5000, 23, 24);
            Cube virus = setVirusCube();
            world.food.Add(virus.uid, virus);
            LinkedList<Cube> cubes = new LinkedList<Cube>();
            cubes.AddFirst(cube);
            cubes.AddFirst(sameSpot);
            world.splitPlayers.Add(cube.uid, cubes);
            world.splitPlayers.Add(other.uid, cubes);
            LinkedList<Cube> playercubes = new LinkedList<Cube>();
            world.players.Add(cube.uid, cube);
            world.players.Add(other.uid, other);
            world.players.Add(sameSpot.uid, sameSpot);
            world.movePlayer(5000, 23, 24);
            world.moveCube(cube, 23, 24);
            world.moveCube(cube, 23, 24);
            world.eatPlayers();
            world.attrition();
            world.splitPlayer(5000, 24, 25);
            world.eatFood();
            cube.loc_x = -5;
            cube.loc_y = -5;
            cube.loc_x = 5000;
            cube.loc_y = 5000;
        }

    }
}
