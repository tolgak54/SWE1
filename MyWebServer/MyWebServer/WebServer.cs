using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MyWebServer
{
    class WebServer
    {
        TcpListener sSocket;
        bool online = false;
        IPAddress ipAddr;
        int port;
        IPluginManager pluginManager; 

        public IPAddress IpAddr { get => ipAddr; set => ipAddr = IPAddress.Loopback; }
        public int Port { get => port; set => port = 80; }
        public IPluginManager PluginManager { get => pluginManager; set => pluginManager = new PluginManager(); }

        public void WebServerStart()
        {
            online = true;
            sSocket = new TcpListener(IpAddr, Port);
            sSocket.Start();
            while(online)
            {
                Console.WriteLine("waiting for incoming connections...");
                Socket cSocket = sSocket.AcceptSocket();
                Console.WriteLine("connection established with: " + cSocket.LocalEndPoint);
                ThreadPool.QueueUserWorkItem(HandleRequest, cSocket);
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
            if(!request.GetIsValid())
            {
                Response badResponse = new Response();
                badResponse.StatusCode = 400;
                badResponse.Send(ns);
            }
            float tmp = 0.0f;
            IPlugin akt = null;
            foreach (IPlugin plugin in PluginManager.Plugins)
            {
                float tmp2 = plugin.CanHandle(request);
                if (tmp2 > tmp)
                {
                    tmp = tmp2;
                    akt = plugin;
                }
            }
            if(akt == null)
            {
                Response badResponse = new Response();
                badResponse.StatusCode = 400;
                badResponse.Send(ns);
            }
            IResponse response = akt.Handle(request);
            if (response != null)
            {
                response.Send(ns);
            }
            tmpSocket.Close();
        }
    }
}
