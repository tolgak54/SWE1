using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebServer
{
    class Url: IUrl
    {
        private string rawUrl;

        public Url(string path)
        {
            this.rawUrl = path;
        }

        public string RawUrl 
        {
            get
            {
                return rawUrl;
            }
        }

        public string Path => throw new NotImplementedException();

        public IDictionary<string, string> Parameter => throw new NotImplementedException();

        public int ParameterCount => throw new NotImplementedException();

        public string[] Segments => throw new NotImplementedException();

        public string FileName => throw new NotImplementedException();

        public string Extension => throw new NotImplementedException();

        public string Fragment => throw new NotImplementedException();
    }
}
