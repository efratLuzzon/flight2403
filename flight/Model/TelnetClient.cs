﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace flight.Model
{
    class TelnetClient : ITelnetClient
    {
        private Socket sender;
        List<string> adressDashboard;
        public void connect(string ip, int port)
        {

            byte[] bytes = new byte[1024];

            try
            {
                // Connect to a Remote server  
                // Get Host IP Address that is used to establish a connection  
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
                // If a host has multiple addresses, you will get a list of addresses  
                IPHostEntry host = Dns.GetHostEntry(ip);
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP  socket.    
                sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.    
                try
                {
                    // Connect to Remote EndPoint  
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public void disconnect()
        {
            // Release the socket.    
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }

        public string read()
        {
            byte[] msgInBytes;
            string messege = "";
            string lineFromSim = "";
            initializeAdressAdressDashboardr();
            for(int i = 0; i < adressDashboard.Count; i++)
            {
                messege += "get " + adressDashboard[i] + "\n";
            }
            msgInBytes = Encoding.ASCII.GetBytes(messege);

            // Send the data through the socket.    
            int bytesSent = sender.Send(msgInBytes);
            for (int i = 0; i < adressDashboard.Count; i++)
            {
                byte[] bytes = new byte[1024];
                // Receive the response from the remote device.    
                try
                {
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine(i.ToString() +" :" + Encoding.ASCII.GetString(bytes, 0, bytesRec));
                    lineFromSim += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                }
                catch (Exception e)
                {
                    Console.WriteLine("fail in read from simulator" + e.ToString());
                }
            }
            return lineFromSim;

        }

        private void initializeAdressAdressDashboardr()
        {
            //two last adress are for map
            adressDashboard = new List<string> {
                "/instrumentation/heading-indicator/indicated-heading-deg",
                "/instrumentation/gps/indicated-vertical-speed",
                "/instrumentation/gps/indicated-ground-speed-kt",
                "/instrumentation/airspeed-indicator/indicated-speed-kt",
                "/instrumentation/gps/indicated-altitude-ft",
                "/instrumentation/attitude-indicator/internal-roll-deg",
                "/instrumentation/attitude-indicator/internal-pitch-deg",
                "/instrumentation/altimeter/indicated-altitude-ft", 
                "/position/latitude-deg",
                "/position/longitude-deg"
            }; 
        }

        public void write(List<string> command)
        {
           
            string allCommand = "";
            try
            {
                for(int i = 0; i < command.Count; i++)
                {
                    allCommand += "set " + command[i] + "\n";
                }
                // Encode the data string into a byte array.    
                byte[] msg = Encoding.ASCII.GetBytes(allCommand);

                // Send the data through the socket.    
                int bytesSent = sender.Send(msg);
                for(int i = 0; i < command.Count; i++)
                {
                    byte[] bytes = new byte[1024];
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, bytesRec) + " "+i +
                        " get from simulator");

                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("fail in write to Simulator" + e.ToString());
            }
        }
     }
}