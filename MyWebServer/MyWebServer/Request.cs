using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyWebServer
{
    class Request : IRequest
    {
        public bool IsValid
        {
            get;
            private set;
        }

        public string Method
        {
            get;
            private set;
        }
        public IUrl Url
        {
            get;
            private set;
        }

        public IDictionary<string, string> Headers
        {
            get;
            private set;
        }
        public string UserAgent
        {
            get
            {
                if (!Headers.ContainsKey(""))
                {
                    throw new InvalidOperationException("No UserAgent in Header");
                }
                return Headers[""];
            }
        }

        public int HeaderCount
        {
            get
            {
                return Headers.Count;
            }
        }

        public int ContentLength
        {
            get
            {
                if (!Headers.ContainsKey(""))
                {
                    return 0;
                }
                return int.Parse(Headers[""]);
            }
        }

        public string ContentType
        {
            get
            {
                if (!Headers.ContainsKey(""))
                {
                    throw new InvalidOperationException("No Content Type in Header");
                }
                return Headers[""];
            }
        }

        public Stream ContentStream
        {
            get;
            private set;
        }

        public string ContentString
        {
            get;
            private set;
        }
        public byte[] ContentBytes
        {
            get;
            private set;
        }

        public byte[] GetContentBytes()
        {
            return ContentBytes;
        }

        public int GetContentLength()
        {
            return ContentLength;
        }

        public Stream GetContentStream()
        {
            return ContentStream;
        }

        public string GetContentString()
        {
            return ContentString;
        }

        public string GetContentType()
        {
            return ContentType;
        }

        public int GetHeaderCount()
        {
            return HeaderCount;
        }

        public IDictionary<string, string> GetHeaders()
        {
            return Headers;
        }

        public bool GetIsValid()
        {
            return IsValid;
        }

        public string GetMethod()
        {
            return Method;
        }

        public string GetUserAgent()
        {
            return UserAgent;
        }

        IUrl IRequest.Url()
        {
            return Url;
        }
        public Request(Stream request)
        {
            StreamReader sr = new StreamReader(request, Encoding.UTF8);
            if (Headers == null)
            {
                Headers = new Dictionary<string, string>();
            }

            int cnt = 0;
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                Console.WriteLine(line);
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                if (line.Contains(':') && cnt > 0)
                {
                    string[] dataFromHeader = line.Split(':');
                    string dictKey = dataFromHeader[0].Trim().ToLower();
                    string dictValue = dataFromHeader[1].Trim();
                    Headers.Add(dictKey, dictValue);
                }
                else
                {
                    string[] paramArray = line.Split(' ');
                    if (paramArray.Length >= 1)
                    {
                        Method = paramArray[0].ToUpper();
                    }
                    if (paramArray.Length >= 2)
                    {
                        string myUrl = line.Remove(0, line.IndexOf(' '));
                        myUrl = myUrl.Remove(myUrl.LastIndexOf(' ')).Trim();
                        Url = new Url(myUrl);
                    }
                }
            }
        }
    }
}

