using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyWebServer
{
    class Request : IRequest
    {
        bool isValid;
        string line;
        StreamReader sr;
        string method;
        string userAgent;
        int count = 0;
        int contentLength;
        string contentType;
        IUrl myUrl;
        string contentString;
        Stream contentStream;
        byte[] contentBytes;
        string[] parameters;
        Dictionary<string, string> headers;

        public Request(Stream request)
        {
            sr = new StreamReader(request, Encoding.UTF8);
            while ((line = sr.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }
                parameters = line.Split(' ');
                count++;
            }
        }
        public bool GetIsValid()
        {
            isValid = true;
            if (count <= 0)
            {
                isValid = false;
            }

            if (myUrl == null)
            {
                isValid = false;
            }
            return isValid;
        }

        public string GetMethod()
        {
            if (parameters.Length >= 1)
            {
                method = parameters[0].ToUpper();
            }
            return method;
        }
        public IUrl GetUrl()
        {
            if (parameters.Length >= 2)
            {
                string url = line.Remove(0, line.IndexOf(' '));
                url = url.Remove(url.LastIndexOf(' ')).Trim();
                myUrl = new Url(url);
            }
            return myUrl;
        }

        public IDictionary<string, string> GetHeaders()
        {
            if (headers == null)
            {
                headers = new Dictionary<string, string>();
            }
            if (line.Contains(":") && count > 0)
            {
                string[] headerData = line.Split(':');
                string key = headerData[0].Trim().ToLower();
                string value = headerData[1].Trim();
                headers.Add(key, value);
            }
            return headers;
        }
        public string GetUserAgent()
        {
            if (!headers.ContainsKey(userAgent))
            {
                userAgent = "";
            }
            return headers[userAgent];
        }

        public int GetHeaderCount()
        {
            return headers.Count;
        }

        public int GetContentLength()
        {
            if (contentLength <= 0)
            {
                contentLength = 0;
            }
            return contentString.Length;
        }

        public string GetContentType()
        {
            if (!headers.ContainsKey(contentType))
            {
                contentType = "";
            }
            return headers[contentType];
        }

        public Stream GetContentStream()
        {
            if (contentLength > 0)
            {
                char[] result = new char[contentLength];
                sr.Read(result, 0, contentLength);
                contentString = new string(result);
                contentBytes = Encoding.UTF8.GetBytes(contentString);
                contentStream = new MemoryStream(contentLength);
            }
            return contentStream;
        }

        public string GetContentString()
        {
            if (contentLength > 0)
            {
                char[] result = new char[contentLength];
                sr.Read(result, 0, contentLength);
                contentString = new string(result);
            }
            return contentString;
        }
        public byte[] GetContentBytes()
        {
            if (contentLength > 0)
            {
                char[] result = new char[contentLength];
                sr.Read(result, 0, contentLength);
                contentString = new string(result);
                contentBytes = Encoding.UTF8.GetBytes(contentString);
            }
            return contentBytes;
        }
    }
}

