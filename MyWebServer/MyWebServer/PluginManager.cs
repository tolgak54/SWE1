using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebServer
{
    class PluginManager : IPluginManager
    {
        public IEnumerable<IPlugin> Plugins => throw new NotImplementedException();

        public void Add(IPlugin plugin)
        {
            throw new NotImplementedException();
        }

        public void Add(string plugin)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
