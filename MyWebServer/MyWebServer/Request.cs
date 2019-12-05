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
        string req;
        string line;
        StreamReader sr;
        string method;
        string userAgent;
        int contentLength;
        string contentType;
        IUrl myUrl;
        string contentString;
        Stream contentStream;
        byte[] contentBytes;
        List<string> listOfHeaders;
        Dictionary<string, string> headers;

        public Request(Stream request)
        {
            listOfHeaders = new List<string>();
            sr = new StreamReader(request, Encoding.UTF8);
            while ((line = sr.ReadLine()) != null)
            {
                req += line;
                listOfHeaders.Add(line);
            }
            listOfHeaders.RemoveAt(0);
            listOfHeaders.Remove("");
        }
        public bool GetIsValid()
        {
            string[] tmp = req.Split('/');
            if (req.Length <= 0 | req == "")
            {
                isValid = false;
            }
            else if (tmp.Length <= 1)
            {
                isValid = false;
            }
            //Vereinfachen
            else if (tmp[0].ToUpper() == "GET ")
            {
                isValid = true;
            }
            else if (tmp[0].ToUpper() == "POST ")
            {
                isValid = true;
            }
            else if (tmp[0].ToUpper() == "HEAD ")
            {
                isValid = true;
            }
            else if (tmp[0].ToUpper() == "PUT ")
            {
                isValid = true;
            }
            else if (tmp[0].ToUpper() == "DELETE ")
            {
                isValid = true;
            }
            else if (tmp[0].ToUpper() == "TRACE ")
            {
                isValid = true;
            }
            else if (tmp[0].ToUpper() == "OPTIONS ")
            {
                isValid = true;
            }
            else if (tmp[0].ToUpper() == "CONNECT ")
            {
                isValid = true;
            }
            return isValid;
        }

        public string GetMethod()
        {
            string[] tmp = req.Split('/');
            // spliten im Konstruktor
            if (tmp.Length <= 1)
            {
                return "";
            }
            else
            {
                method = tmp[0].ToUpper();
                method = method.Trim();
            }
            return method;
        }
        public IUrl GetUrl()
        {
            string[] tmp = req.Split(' ');
            if (tmp.Length > 1)
            {
                myUrl = new Url(tmp[1]);
            }
            return myUrl;
        }

        public IDictionary<string, string> GetHeaders()
        {
            if (headers == null)
            {
                headers = new Dictionary<string, string>();
                foreach (var item in listOfHeaders)
                {
                    if (item.Contains(":"))
                    {
                        string[] a = item.Split(':');
                        string key = a[0].ToLower();
                        string value = a[1].Trim();
                        headers.Add(key, value);
                    }
                }
            }
            return headers;
        }
        public string GetUserAgent()
        {
            if (headers == null)
            {
                headers = new Dictionary<string, string>();
                foreach (var item in listOfHeaders)
                {
                    if (item.Contains(":"))
                    {
                        string[] a = item.Split(':');
                        string key = a[0];
                        string value = a[1].Trim();
                        headers.Add(key, value);
                    }
                }
                if (headers.ContainsKey("User-Agent"))
                {
                    userAgent = headers["User-Agent"];
                }
            }
            return userAgent;
        }

        public int GetHeaderCount()
        {
            return headers.Count;
        }

        public int GetContentLength()
        {
            if (headers == null)
            {
                headers = new Dictionary<string, string>();
                foreach (var item in listOfHeaders)
                {
                    if (item.Contains(":"))
                    {
                        string[] a = item.Split(':');
                        string key = a[0];
                        string value = a[1];
                        headers.Add(key, value);
                    }
                }
                if (headers.ContainsKey("Content-Length"))
                {
                    contentLength = Convert.ToInt32(headers["Content-Length"]);
                }
            }
            return contentLength;
        }

        public string GetContentType()
        {
            if (headers == null)
            {
                headers = new Dictionary<string, string>();
                foreach (var item in listOfHeaders)
                {
                    if (item.Contains(":"))
                    {
                        string[] a = item.Split(':');
                        string key = a[0];
                        string value = a[1].Trim();
                        headers.Add(key, value);
                    }
                }
                if (headers.ContainsKey("Content-Type"))
                {
                    contentType = headers["Content-Type"];
                }
            }
            return contentType;
        }

        public Stream GetContentStream()
        {
            contentBytes = GetContentBytes();
            contentBytes[0] = 0;
            contentStream = new MemoryStream(GetContentBytes());
            return contentStream;
        }

        public string GetContentString()
        {
            foreach (var item in listOfHeaders)
            {
                if (!item.Contains(":"))
                {
                    contentString += item;
                }
            }
            return contentString;
        }
        public byte[] GetContentBytes()
        {
            contentString = "";
            contentBytes = Encoding.UTF8.GetBytes(contentString);
            return Encoding.UTF8.GetBytes(GetContentString());
        }
    }
}

