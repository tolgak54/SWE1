using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MyWebServer
{
    class WebServer
    {
        TcpListener sSocket;
        bool online = false;

        public void WebServerStart()
        {
            online = true;
            sSocket = new TcpListener(2212);
            sSocket.Start();
            while(online)
            {
                Console.WriteLine("waiting for incoming connections...");
                Socket cSocket = sSocket.AcceptSocket();
                Console.WriteLine("connection established with: " + cSocket.LocalEndPoint);
                ThreadPool.QueueUserWorkItem("", cSocket);
            }
        }
        public void WebServerStop()
        {
            online = false;
            sSocket.Stop();
        }

        void HandleRequest(object cSocket)
        {
            Socket tmpSocket = (Socket)cSocket;
            NetworkStream ns = new NetworkStream(tmpSocket);
            IRequest request = new Request(ns);
            if(!request.)
            {

            }
        }
    }
}
