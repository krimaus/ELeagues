using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ELeagues
{
	class ServerComm
	{

        // Use ServerComm.CurrentUser as you would any other variable name,
        // null value means no user is logged in so one should set null value when user logs out
        static string? _globalString;
        public static string? CurrentUser 
        {
            get
            {
                return _globalString;
            }
            set
            {
                _globalString = value;
            }
        }

        static bool _adminStatus = false;
        public static bool AdminStatus
        {
            get
            {
                return _adminStatus;
            }
            set
            {
                _adminStatus = value;
            }
        }

        public static bool ServerCall(string messageToServer)
        {

            try
            {

                // Establish the remote endpoint
                // for the socket. This example
                // uses port 23177 on the local
                // computer.
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 23177);

                // Creation TCP/IP Socket using
                // Socket Class Constructor
                Socket sender = new Socket(ipAddr.AddressFamily,
                           SocketType.Stream, ProtocolType.Tcp);

                try
                {

                    // Connect Socket to the remote
                    // endpoint using method Connect()
                    sender.Connect(localEndPoint);

                    // We print EndPoint information
                    // that we are connected
                    Console.WriteLine("Socket connected to -> {0} ",
                                  sender.RemoteEndPoint.ToString());

                    // Creation of message that
                    // we will send to Server
                    // message template ca:username:password:false
                    // ca - create account
                    // cl - create league
                    // ct - create tourney
                    // ap - add player
                    // cm - create match
                    // em - edit match
                    // sq - server query
                    byte[] messageSent = Encoding.ASCII.GetBytes(messageToServer + "<EOF>");
                    int byteSent = sender.Send(messageSent);

                    // Data buffer
                    byte[] messageReceived = new byte[1024];

                    // We receive the message using
                    // the method Receive(). This
                    // method returns number of bytes
                    // received, that we'll use to
                    // convert them to string
                    int byteRecv = sender.Receive(messageReceived);
                    Console.WriteLine("Message from Server -> {0}",
                          Encoding.ASCII.GetString(messageReceived,
                                                     0, byteRecv));

                    // Close Socket using
                    // the Close() method
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                    //sr - server reply
                    if (Encoding.ASCII.GetString(messageReceived, 0, byteRecv) == "sr:approved")
                        return true;
                    else
                        return false;
                }

                // Manage Socket's Exceptions
                catch (ArgumentNullException ane)
                {

                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                    return false;
                }

                catch (SocketException se)
                {

                    Console.WriteLine("SocketException : {0}", se.ToString());
                    return false;
                }

                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                    return false;
                }
            }

            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
                return false;
            }
        }
    }
}
