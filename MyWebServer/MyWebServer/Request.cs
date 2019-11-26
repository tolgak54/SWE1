using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyWebServer
{
    class Request : IRequest
    {
        Stream sRequest;
        bool isValid;
        string method;
        string userAgent;
        int contentLength;
        int headerCount;
        string contentType;
        Url myUrl;
        string contentString;
        Stream contentStream;
        byte[] contentBytes;
        Dictionary<string, string> headers;

        public Request (Stream request)
        {
            this.sRequest = request;
        }
        public bool GetIsValid()
        {
            if (sRequest==null)
            {
                isValid = false;
                return isValid;
            }
            isValid = true;
            return isValid;
        }

        public string GetMethod()
        {
            throw new NotImplementedException();
        }

        public IUrl GetUrl()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, string> GetHeaders()
        {
            throw new NotImplementedException();
        }

        public string GetUserAgent()
        {
            throw new NotImplementedException();
        }

        public int GetHeaderCount()
        {
            throw new NotImplementedException();
        }

        public int GetContentLength()
        {
            throw new NotImplementedException();
        }

        public string GetContentType()
        {
            throw new NotImplementedException();
        }

        public Stream GetContentStream()
        {
            throw new NotImplementedException();
        }

        public string GetContentString()
        {
            throw new NotImplementedException();
        }

        public byte[] GetContentBytes()
        {
            throw new NotImplementedException();
        }
    }
}
