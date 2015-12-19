/*

Network_Controller
(used for Ag Cubio & Web Server)
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
    /// A general networking code that will open the socket between the client and the server 
    /// and provide helper functions for sending and receiving data. 
    /// </summary>
    public static class Network
    {
        // The port number for the remote device.
        private const int port = 11000;
        private const int webPort = 11100;

        /// <summary>
        /// This function should attempt to connect to the server via a provided hostname.
        /// It should save a callback function (in a state object) for use when data arrives.
        /// It will need to open a socket and then use the BeginConnect method. 
        /// Note: this method takes the "state" object and "regurgitates" it back to you when a
        /// connection is made, thus allowing "communication" between this function and the 
        /// Connected_to_Server function.
        /// Hostname = the name fo the server in which to connect
        /// Callback function = a function inside the View to be called when a connection is made.
        /// </summary>
        /// <reference> MSDN Asynchronous Client server </reference>
        public static Socket Connect_to_Server(Action<StateObject> callback, string hostname)
        {
            //set the hostname
            try
            {
                IPAddress ipAddress;
                try
                {
                    //grab ipAddress/hostname
                    ipAddress = Dns.GetHostEntry(hostname).AddressList[0];
                }
                catch (Exception)
                {
                    //if dns couldn't parse, set as ipaddress
                    ipAddress = IPAddress.Parse(hostname);
                }
                IPEndPoint endPoint = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.
                Socket socket = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);
                socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

                // Connect to the remote endpoint.
                socket.BeginConnect((EndPoint)endPoint, new AsyncCallback(Connected_to_Server), new StateObject()
                {
                    //save socket and callback inside state object
                    workSocket = socket,
                    callBackFunction = callback
                });
                return socket;
            }
            //if an exception occurs, write error to console and return null
            catch (Exception except)
            {
                //allows view_controller to determine if connection error occured.
                callback(new StateObject()
                {
                    error_occured = true
                });
                Console.WriteLine("Error occured: " + except.ToString());
                return (Socket)null;
            }
        }


        /// <summary>
        /// Refference by the Begin Connect method above and is "called" by the OS when the 
        /// socket connects to the server. The "state_in_an_ar_object" object contains a
        /// field "AsyncState" which contains the "state" object saved away in the above funciton.
        /// Once a connection is established the "saved away" callback funtion needs to be called.
        /// The network connection should then "BeginReceive" expecting more data to arrive (and provide the
        /// ReceiveCallback function for this purpose)
        /// </summary>
        /// <reference> MSDN Asynchronous Client server </reference>
        public static void Connected_to_Server(IAsyncResult state_in_an_ar_object)
        {
            // Retrieve the socket from the state object.
            StateObject state = (StateObject)state_in_an_ar_object.AsyncState;
            try
            {
                // Complete the connection.
                state.workSocket.EndConnect(state_in_an_ar_object);
                state.callBackFunction(state);
                i_want_more_data(state);
            }
            catch (Exception e)
            {
                //lets viewController know that error occured connecting
                state.error_occured = true;
                //error occured, call callbackfunction
                state.callBackFunction.DynamicInvoke(state);
                Console.WriteLine(e.ToString());
            }
        }


        /// <summary>
        /// Called by the OS when new data arrives. This method checks to see how much
        /// data arrived. If 0, the connection has been closed. On greater than zero data, this
        /// method should call the callback function provided above. 
        /// For our purposes, this function does not request more data. It is up to the code in the
        /// callback function to request more data.
        /// </summary>
        /// <reference> MSDN Asynchronous Client server </reference>
        public static void ReceiveCallback(IAsyncResult state_in_an_ar_object)
        {
            //try to callback
            try
            {
                // Retrieve the state object and the client socket 
                // from the asynchronous state object.
                StateObject state = (StateObject)state_in_an_ar_object.AsyncState;

                // Read data from the remote device.
                int bytesRead = state.workSocket.EndReceive(state_in_an_ar_object);

                //if bytes are not greater than zero, do noghing.
                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.
                    state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));
                    state.callBackFunction(state);
                }
                else
                {
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Function Used to get more data
        /// </summary>
        /// <param name="state"></param>
        /// <reference> MSDN Asynchronous Client server </reference>
        public static void i_want_more_data(StateObject state)
        {
            try
            {
                //literally get more data
                state.workSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None,
                new AsyncCallback(ReceiveCallback), state);
            }
            catch
            { state.error_occured = true; }

        }


        /// <summary>
        /// Allows the program to send data over a socket. This function converts the data
        /// into bytes and then sends them using socket.BeginSend.
        /// Client View must handle SocketException.
        /// </summary>
        /// <reference> MSDN Asynchronous Client server </reference>
        public static void Send(Socket socket, String data)
        {
            //try to send socket and data
            try
            {
                // Convert the string data to byte data using UTF8 encoding.
                byte[] byteData = Encoding.UTF8.GetBytes(data);

                // Begin sending the data to the remote device.
                socket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None,
                    new AsyncCallback(SendCallback), socket);
            }
            catch (Exception)
            {
                //close the socket if error happens
               socket.Shutdown(SocketShutdown.Both);
               socket.Close();
                //thorw error which is handled by clientView
               throw new SocketException();
            }
        }

        /// <summary>
        /// Helper method that assists the Send Function. If all the data has been sent,
        /// then life is good and nothing needs to be done. If there is more data to send, 
        /// the SendCallBack needs to arrange to send this data
        /// </summary>
        /// <reference> MSDN Asynchronous Client server </reference>
        public static void SendCallback(IAsyncResult state_in_an_ar_object)
        {
            try
            {
                // Retrieve the socket from the state object.
                // Complete sending the data to the remote device.
                ((Socket)state_in_an_ar_object.AsyncState).EndSend(state_in_an_ar_object);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// This is the heart of the server code. It asks the OS to listen for a connection
        /// and saves the callback function with that request. Upon a connection request coming in,
        /// the OS invokes the Accept_a_New_Client method
        /// 
        /// NOTE: while this method is called "loop", it is not a traditional loop, but an "event loop"
        /// (i.e., this method sets up the connection listener, which, when a connection occurs, sets up a 
        /// new connection listener, for anotehr connection).
        /// </summary>
        /// <param name="callback"></param>
        public static void Server_Awaiting_Client_Loop(Action<StateObject> callback)
        {
            //create a socket
            Socket socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            try
            {
                //begin listening
                socket.Bind((EndPoint)new IPEndPoint(IPAddress.IPv6Any, port));
                socket.Listen(100);

                //save callback function
                StateObject state = new StateObject();
                state.workSocket = socket;
                state.callBackFunction = callback;
                Console.WriteLine("Waiting for first connection...");

                //invoke the Accept_a_New_Client method
                socket.BeginAccept(new AsyncCallback(Accept_a_New_Client), state);

            }
            catch(Exception e)
            {
                //print an error to server
                Console.WriteLine(e.ToString());
            }

        }

        /// <summary>
        /// This code is invoked by the OS when a connection request comes in. it should:
        /// 1. Create a new socket
        /// 2. call the callback provided by the above method
        /// 3. await a new connection request
        /// 
        /// NOTE: the callback method referenced in the above function should have been transferred
        /// to this function via the AsyncResult parameter and should be invoked at this point.
        /// 
        /// WARNING!!!
        /// After accepting a new client, the Networking code should NOT start listening for data!
        /// It is the job of the game server (presumably via the callback method) to request data!
        /// 
        /// </summary>
        /// <param name="ar"></param>
        public static void Accept_a_New_Client(IAsyncResult ar)
        {
            //create a new socket
            Console.WriteLine("A new Client has contacted the Server.");
            StateObject state1 = (StateObject) ar.AsyncState;
            Socket socket = state1.workSocket.EndAccept(ar);
            StateObject state2 = new StateObject();
            state2.workSocket = socket;

            //call callback provided by method above
            state1.callBackFunction(state2);
            if (state2.callBackFunction == null)
                throw new Exception("caller needs to provide a callback function to the state object");

            //await a new connection request.
            state1.workSocket.BeginAccept(new AsyncCallback(Accept_a_New_Client), (object)state1);
        }



        /// <summary>
        /// This is the heart of the web server code. It asks the OS to listen for a connection
        /// and saves the callback function with that request. Upon a connection request coming in,
        /// the OS invokes the Accept_a_New_Web method
        /// 
        /// NOTE: while this method is called "loop", it is not a traditional loop, but an "event loop"
        /// (i.e., this method sets up the connection listener, which, when a connection occurs, sets up a 
        /// new connection listener, for anotehr connection).
        /// </summary>
        /// <param name="callback"></param>
        public static void Server_Awaiting_Web_Loop(Action<StateObject> callback)
        {
            //create a socket
            Socket socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            try
            {
                //begin listening
                socket.Bind((EndPoint)new IPEndPoint(IPAddress.IPv6Any, webPort));
                socket.Listen(100);

                //save callback function
                StateObject state = new StateObject();
                state.workSocket = socket;
                state.callBackFunction = callback;
                Console.WriteLine("Waiting for web connection...");

                //invoke the Accept_a_New_Client method
                socket.BeginAccept(new AsyncCallback(Accept_a_New_Web), state);

            }
            catch (Exception e)
            {
                //print an error to server
                Console.WriteLine(e.ToString());
            }

        }

        /// <summary>
        /// This code is invoked by the OS when a connection request comes in. it should:
        /// 1. Create a new socket
        /// 2. call the callback provided by the above method
        /// 3. await a new connection request
        /// 
        /// NOTE: the callback method referenced in the above function should have been transferred
        /// to this function via the AsyncResult parameter and should be invoked at this point.
        /// 
        /// WARNING!!!
        /// After accepting a new client, the Networking code should NOT start listening for data!
        /// It is the job of the game server (presumably via the callback method) to request data!
        /// 
        /// </summary>
        /// <param name="ar"></param>
        public static void Accept_a_New_Web(IAsyncResult ar)
        {
            //create a new socket
            Console.WriteLine("A new web has contacted the Server.");
            StateObject state1 = (StateObject)ar.AsyncState;
            Socket socket = state1.workSocket.EndAccept(ar);
            StateObject state2 = new StateObject();
            state2.workSocket = socket;

            //call callback provided by method above
            state1.callBackFunction(state2);
            if (state2.callBackFunction == null)
                throw new Exception("caller needs to provide a callback function to the state object");

            //await a new connection request.
            state1.workSocket.BeginAccept(new AsyncCallback(Accept_a_New_Web), (object)state1);
        }

        /// <summary>
        /// Allows the program to send data over a socket. This function converts the data
        /// into bytes and then sends them using socket.BeginSend.
        /// 
        /// NOTE: this method closes the given socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        public static void SendWeb(Socket socket, String data)
        {
            //try to send socket and data
            try
            {
                // Convert the string data to byte data using UTF8 encoding.
                byte[] byteData = Encoding.UTF8.GetBytes(data);

                // Begin sending the data to the remote device.
                socket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None,
                    new AsyncCallback(SendCallback), socket);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception)
            {
                //close the socket if error happens
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                //thorw error which is handled by clientView
                throw new SocketException();
            }
        }
    }


    /// <summary>
    /// State object for reading client data asynchronously
    /// </summary>
    /// <reference> MSDN Asynchronous Client server </reference>
    public class StateObject
    {
        /// <summary>
        /// State object for reading client data asynchronously
        /// </summary>
        public Socket workSocket = null;

        /// <summary>
        ///Size of receive buffer.
        /// </summary>
        public const int BufferSize = 1024;

        /// <summary>
        /// Receive buffer.
        /// </summary>
        public byte[] buffer = new byte[BufferSize];

        /// <summary>
        /// Received data String
        /// </summary>
        public StringBuilder sb = new StringBuilder();

        /// <summary>
        /// Delegate for callback functions
        /// </summary>
        public Action<StateObject> callBackFunction;

        /// <summary>
        /// determines if an error occured while connecting to server
        /// </summary>
        public bool error_occured;

        /// <summary>
        /// holds the uid of the current state cube
        /// </summary>
        public int uid;
    }
}
