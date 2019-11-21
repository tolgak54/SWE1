using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebServer
{
    class Plugin : IPlugin
    {
        public float CanHandle(IRequest req)
        {
            throw new NotImplementedException();
        }

        public IResponse Handle(IRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
