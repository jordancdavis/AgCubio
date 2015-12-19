Jordan Davis & Jacob Osterloh
December 9, 2015
cs3500 - ps9

AgCubio Game v3.0

Notes for Grader:
	DATABASE: cs3500_osterloh		
	Password: trescommas
		Some additions that we added to our webserver are links on the data to navigate to various pages.
			-clicking the 'AgCubio' title takes you to the score page (listing all players and games)
			-clicking the 'Top Rank' Column label will take you to the highscores page (all time top 25)
			-clicking a name will take you to all games played by that named player. 
			-clicking an id will take you to that specific game.
		We added a highscores page showing alltime top 25 stored in the database
			-http://localhost:11100/highscores

		All gameplay is synced to the database and then displayed to the webserver upon request.
		All requirments specified in the assignment details have been completed. 

DATABASE: 

	The database we created contains two tables. 
	One table contains most of the information necessary for the webpage. 
	It includes auto-incremented ids, names, top rank achieved by the players, maximum mass, 
	food eaten, total cubes eaten, time alive, and time of death.
	The other table is linked to the main table by foreign keys with the auto incremented id’s 
	generated in the first table and contains the names of cubes eaten by a specific player. 
	So simply this table contains the name of the eaten player with the id of the player who ate them. 
	If a player did not eat any other players, then their id will not be found in this table.

	-STORING
	In order to store items into the database we simply used “INSERT INTO” to add the cube info into the 
	appropriate table. We did have to make some changes to our cube and world class to gather the data that 
	we wanted to store (such as a linked list of strings that hold the names of players a cube has eaten).
	
		To populate Player Table
			(INSERT INTO Player(name, rank, maxMass, foodEaten, cubesEaten, timeAlive, timeOfDeath) 
			VALUES(@name, @rank, @maxMass, @foodEaten, @cubesEaten, @timeAlive, @timeOfDeath)

		To populate NamesOFPlayersEaten Table (using foreign keys)
			INSERT INTO NamesOfPlayersEaten(id,namesOfPlayersEaten) 
			VALUES((SELECT id from Player WHERE name='" + cube.Name + "'),@namesOfPlayersEaten)

	-RETRIEVING
	First we created a database class which can create a database object which holds our database information.
	This way the database information is accessible throughout the AgCubio solution. In order to retrieve our 
	information from the database we used the “SELECT * FROM” command on the appropriate table and then used 
	the reader to iterate through each row in the table and stored each rows’ information in a “WebData” object. 
	We then stored all of the WebData objects into a list for use later.

		To retreive player information from Player Table
			SELECT * from Player
	
		To retreive list of player eaten from NamesOfPlayersEaten Table
			SELECT * from NamesOfPlayersEaten

		To retreive top 25 player information from Player Table
			SELECT * FROM Player ORDER BY maxMass DESC LIMIT 25


WEBSERVER:

	We started by creating a Webserver class that takes the database class information and sends it to the web 
	depending on the request. This class is similar to the server class. It handles requests from the user and 
	accesses the database to provide that information over a socket.

	-WEBPAGES
	Depending on the request received through port 11100, the server will direct the web user to 1 of 5 pages. 
	These pages are Scores, High Scores, Player, Game Session, and Error. Scores displays all of the players 
	scores that are stored in the database. High Scores displays the 25 highest scores stored in the database. 
	Player displays all of the game session statistics for a specific player name. Game Session shows the statistics 
	of an individual game for a specific player. Error displays a 404 error and contains a summary of valid options.

	-NAVIGATING USING LINKS
	All of the different pages are also linked together using hyper links. On any page, by clicking the header 
	“AgCubio” this will take the user to the Scores page. Clicking on any player name will take the user to that 
	player’s player page. Clicking on any user id will take the user to the Game Session page for that player id. 
	Clicking on the column header “Top Rank” will take the user to the High Scores page.


/////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////            PS8 AgCubio  v2.0          //////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////


Jordan Davis & Jacob Osterloh
December 4, 2015
cs3500 - ps8

AgCubio Game v2.0

Graders:

	Everything Recomended/Required from the assignment description works. (to the best of our knowledge)
		game engine easily tweakable
		readonly and xml file for parameters
		splitting
		absorbing / eating cubes
		moving
		growing food
		viruses
		attrition
		random colors
		cube momentum / acceleration
		server (wtih multiple clients)
			-alllows other clients

	Having said that, our server works best with our own personal client. We attempted running Jim's client on our 
	server and it conencted but didnt create the world. We were able to allow other students to run their 
	client on our server and it worked the same but the performance wasn't as fast and obviously the view/graphics 
	were different. (The reason Jim's client wont work is because the IPv4 issue)

	-Be sure to run our server with our client for optimal performance-

	Features:
		Tab button: 
		Use the tab button to toggle the scale feature on or off in order to see a larger view of the world.

		Game Engine Easy to Tweak:
				World parameters are in the XML file named "world_parameters.xml" located in the Resources folder.
			We created two constructors for our world, one if you have an xml file on your compters, and another
			backup constructor if your file is missing. Feel free to change any of the parameters located in this file
			or in the world class to better test our code. The way it is currently set is what we feel is best for
			multiplayer gameplay. By best we mean most entertaining and enjoyable.

		Dark Theme:
			- On the startup screen you have the option to switch into "Dark Theme"
				Dark theme changes the background of the world to black.
				I highly reccoment playing on "Dark Theme" because its easier on the eyes.
		
		Split:
				Our split function works. It had a bug that we turned into a feature. When a player has two split cubes,
			and one can split but the other is not big enough to split and the player hits the split key, the cube
			big cube will split and the other cube will be left behind disjoint from the team.
			We decided to keep this feature because it a player splits often they have a random chance of being 
			penalized and loosing mass for it. Also sometimes if a player splits too much then they wont be able 
			to ever split again. This adds a little twist to the game so choose your splits wisely. 

		Viruses:
				Viruses work and are initialized to mass 200. only when your playercube is big enough will it split when
			too close to a virus. Player splits the max ammount of times and the virus dissapears. Also the player
			will lose mass every time they hit a virus.

		In Game Statistics:
			- Requirements from PS7
			- Players on the servers

		After Game Statistics:
			- Statitsics added to this client 
				- Total food eaten
				- Total player cubes eaten (each split cube counts)
				- Time Alive
				- Current score
				- you High score for your session


Design Decisions:
		Growing food:
				To grow food we just create food cubes untill the limit has been reached adding them to our
			food dictionary.nThis is called by the server and generates food at a specified rate. 
			We also added a rate parameter to our growFood method so that we can modify the speed/chance 
			a new food is created in a random location.

		Eating/Absorbing:
				In order to eat a player cube or food cube, we decided it was best just to check every food cube
			vs. every player cube every heartbeat of the game as well as every player cube with every player cube.
			Doing this may be a little slower then another solution, if a better one even exists, but we couldnt 
			think of a better solution so we decided to use this algorithm. It turns out that it worked pretty well
			and doesnt limit the quality of the program (to the best of our knowledge). We beleive that this is the
			only way to solve this probelm, please notify one of us if you have a better solution.

		Virus Strategy:
				Our strategy for creating viruses is to utilize the food cubes that we have already created.
			Food already swan random locations, generate when needed, and can be eaten/absorbed by a player.
			We created a parameter in our cube class that holds a boolean value determining if the cube is a
			virus. In the world class we then added parameters that limit the amount of viruses inside a world.
			Then if the number of viruses is ever lower then the limit, the next food sent through the server
			will be flagged as a virus. This change is done inside our grow food method. Each time we grow a food
			we check if another virus needs to be added. If it does then we set the mass, color, and width for the
			virus here too. 
				After we got our viruses working, we modified our client view to draw a nice box around the virus
			to make it more noticable. Lastly, in our eatFood method, we add a check if the food cube is also a 
			virus cube. If it is, we call our splitPlayer method for the maximum amount of splits allowed. 

		Testing strategy:
			Most testing was done by hand... on paper, then implemented into our code.
				Our testing strategy was done to test our math logic and algorithm functionality. We managed to
			reach most of our code but couldnt figure out how to test some of the private and void methods.
			Code coverage was above 90% and we feel comfortable with this.
				The best way we tested the game was by actually running the code and seeing if it behaved how we
			wanted. We also spend alot of time in the debugger making sure that our math was correct. We ran our server
			with other students clients and vice versa to make sure that they functioned and worked properly.


To Be Continued?

/////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////            PS7 AgCubio  v1.0          //////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////


Jordan Davis & Jacob Osterloh
November 17, 2015
cs3500 - ps7

AgCubio Game v1.0

Our AgCubio Game is 'fully functional' and ready for public release.
Everything Works. (to the best of our knowledge)

Features to be aware of:
	Make sure you have something in the server textbox or it wont allowe you to start.
	(not sure if entering an IPAddress works or if thats a server problem, doesnt throw error, but doesnt paint anything)
	Playernames cannot be longer than 30 characters. (Name can be empty)
	If game is unable to connect to server an error message is displayed.
	If error in server connection happens durring gameplay user is taken to start screen and error message is displayed.
	Move using the mouse
	Split using the space bar
	Stats on the top right update druing gameplay.
	Score stat displayed is your highes mass reach durring this game.
	Mass stat is the total mass of the team
	Post game stats: 
		Highscore after death is your highest score reached durring your session on the server.
		Time alive durring game is displayed after you die.
		Reset button after you die.
	Names are centered on the cube
	
		

Design Decisions:

	Model: 
	The Model class was our starting point. We created a cube class with all the specifications that we
	thought we would need and then checked back after we started receiving actual data from the server.
	We eventually came back to the Model class and modified all its contents so it would match with the
	JSON properties provided.

		Cube class:
		Cube class was created as a simple class that contained getters and setters for 
		each of the JSON properties that were being sent from the server. 

		World class: 
		We decided to use two Dictionaries in our world class, One for player cubes and 
		a second for food cubes. We decided to do it this way because it allowed us to 
		draw each type of cube differently (ie. drawing names on cubes).
		The world class also contains some extra member vairables other than the specifications
		provided from VC Jim. We added these because we figured we could use them when we 
		create our own server.


	Network_Controller:
	We initialy thought that we could do the Network_Controller after we created the GUI but that became
	very difficult because we couldnt receive anything from the serever and couldnt tell if it was doing
	what we expected. Inside the Network_Controller we referenced MSDN's implementation of Asynchronous
	Client servers. This helped guide us to develop our own implementation.

		Network:
		Stores all the socket code. Network is a generalized Class that uses callback functions to connect to a server,
		Send data to a server, and receive more data from the server. We allow our network to connect to either a 
		server name or IPAddress. We decided it was smart to handle any server exceptions in this Class and then send 
		a message and let the client handler deal with the error messages however they please.	

		StateObject:
		We created a state object to maintain the state of the world and communicate to the clients what is going on
		Our state object has a buffer which is used to get/hold the information from the server as well as a stringbuiler
		so that it can group the needed data in a single string. Anotehr important thing we added to our state object
		is a error check to help with server connection lost. This is a boolean value that is set to true whenever
		the server throws and exception or connection has been terminated. This allowed us to manage gameplay and prevent
		anything from happening to our Gui when no connection is made. It also will display a message if for some reason
		the client loses connection to the server.


	View:
	This View project creates the players view (GUI). This class uses the Model project, draws the GUI controls and
	game scene, handles user input, utilizes the Network_Controller library to send and receive data over a server.
	We began with handeling the simple functionalities (ie. getting a name, server address, drawing the scene, 
	showings status values). This was challanging at first because we tried to do it all with the sample data
	provided from VC Jim and without using the Server. Once we got our Network_Controller working we managed to 
	get this initial setup complete and move on to the following.  Using locks, we do all updating of the world
	in this View Controller:

		Form1: 
			Name/Server: 
			Allowing the user to enter a name and server was easy. We decided to limit the length
			of a name entered by the user to 30 characters and initialize the server name to "localhost". This
			just makes it easier to connect. We also only begin playing if the user has entered somehting in
			the server textbox. obviously if it is an invalid server we display a message.

			Connection Errors / Retry Connecting:
			For this we decided to pring a label that says "Server Unavailable" to give a hint to the user
			why the game is not working. We then decided to send the player to the start screen if a connection
			is not present. Even in the middle of the game, if the server disconnects, the user is sent to the
			start screen and error message is displayed. This makes it easy for them to reconnect as long as 
			a server is running.

			Server Socket Connect:
			For this our decision was to use 3 callback functions; one for connecting, another for sending/getting 
			player info, and lastly to get server data) this socked was saved in a member variable so that we could
			use it throughout the program checking if it was still connected. the socket and the state object both
			helped with the server. We were able to handle any partial data sent from the server by appening it to 
			the next information that comes in. 

			Allow user to press button or click enter to start game:
			We decided it would be best to allow the user to either click the 'play' button or simply press the 
			enter button when on the name text box. 

			Show/Hide helper methods:
			A decision was made to create helper methods to hide and show any labels/stats/buttons in helper methods.
			This was helpful so that the code was kept clean and that alot of redundant calls were prevented. It also
			save time and avert errors.

			Moving request:
			At first we simply just sent the location of the mouse over the server untill it was working. After that we 
			decided it would be best to allign the mouse cursor with the center of the cube. This was done by using
			the scaling factor for resize window. This also improved gameplay and gave a smoother control to the user.

			Split request:
			For this method we coppied our Move method only changing "move" to "split." We then decided to improve this
			by using this.Focus() in our paint world so that it would behave properly. (see prblems/solutions below)

			Drawing:
			Alot of decisions were made for drawing our world. We draw the scene on the panel and stats in the top right
			corner. Having two different Dictionaries in our World class allowed us to draw food differenly than palyers
			(mainly adding names). For names we chose a simple font and managed to calculate the center of the cube. 
			We wanted to scale the fontsize to handle splits as well as large cubes but were unable to finish due to 
			time constraints. We utilized PaintEventArgs, SolidBursh, FillRectangle, and DrawString in order to paint
			the world.

			FPS:
			To keep track of FPS we used a counter and timer in our 'paintworld' method. This clocks and counts
			the frames and provides the info on the gui. We weren't sure what threshold was best for our gui, but
			we are happy where it is currently set. To be honest, we didnt see much of a difference when changing it.

			After Death:
			When the palyer dies we wanted to give some good information about the users performance. The server
			didnt really send us much information, but were were able to calculate the total time player, score,
			as well as the highest score durring the current session. We decided to also provide a restart button
			so the player doesnt have to re-enter their name/server and can quickly get back into a new game.

			Polishing:
			We are perfectionists so we decided to make our GUI look as good as possible(as much as time allowed).
			To make our GUI clean we carefully placed our labels, chose our colors and placed the names of player
			cubes in the middle of their cube. We tried to scale the name based on the cube size but we were not
			able to finish due to lack of time.

			Zooming Window:
			This didn’t turn out to be as hard as we thought it would be once we figured out the correct method 
			calls to use. Basically we had to create a scaling factor based on the total mass of the player’s 
			cube(s). Then we used the e.graphics.ScaleTransform, and e.graphics.TranslateTransform with the 
			scale factor to resize the players screen to fit according to their mass.

			Extra Stuff:		
				-adding a high score for the current session. 
				-centering player names. 
				-time alive
				-zooming window.
				-play buttons
				-end of game reset panel (reset button & stats)
				-connection error (before & durring gameplay)
				-display/colors.
				-team Mass (total mass)



Problems/Solutions:
Listed below are some of the significant problems that we occured while developing this assignment allong with
the decisions necesary in order to fix the problem.

	Connecting to server:
	It was difficult to figure out the coding for establishing a connection. We were able to eventually get 
	this figured out by reviewing the MSDN code for networking and learning the uses for callback functions in
	 order to pass information back up the connection chain.

	TeamUID problem (team mass):
	This turned out to be a minor mistake. We were having a lot of trouble trying to calculate this. 
	We thought that by just checking if the team uid’s matched the player cube uid we could just add all of their 
	masses together. This was the right approach but we just had a syntactical error.  We thought that the server 
	was sending us back the teamUID as team_uid, but it turns out its JSON property should just be team_id. this 
	solved the problem

	Moving:
	At first we couldn’t figure out how to make the cube move. We realized that the sendPosition method we made 
	had no event caller. So we needed to update the position as often as possible by putting it in our paintWorld 
	method which has an invalidate() call in order to keep refreshing.

	Split:
	Our split was responding very strangely. It would create an entirely new cube each time we hit the space bar. 
	In order to fix this we simply needed to call this.Focus() after we called Invalidate() in the paintWorld(),
	 which fixed the problem.

	Flickering:
	Our cubes and even our text boxes would flicker as we moved. So we moved our paint event args in the form1.designer 
	into our callback function that occurs after the connection with the server had been made. this eliminated all 
	flickering.

	Reset Button: 
	At one point our reset button was painint an aditional work on top of the pervious world. This caused some crazy
	3-Demensional graphics. It was very trippy. To fix this we needed to crate a boolean that determins that the world
	has been drawn, once thw world has been drawn once it will never invode the 'paintworld' method again. It will just
	use the previous 'paintworld' method. This is done by also checking before painting if the player is dead or alive.



Testing:
Most of our testing was done through trial and error. The only things that we were really able to test in a 
“unit testing” environment were our constructors for cube and world, as well as the getters and setters 
associated with them. We were told a coded UI test was unnecessary and the only thing that need to be 
tested associated with the networking code was for JSON. We checked our JSON conversion by checking them 
in the debugger and witnessing the correct creation and removal of cubes in our GUI. Also Jim mentioned in 
class that we didn’t need to provide unit testing for Network Controller.
We did a simple Unit test for Model project that acheived 100% code coverage. Honestly not much to test in 
these two classes (world and cube) because they only contain getters and setters. All updating is handled
in the View project.


To Be Continued?