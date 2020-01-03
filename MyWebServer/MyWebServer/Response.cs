using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyWebServer
{
    class Response : IResponse
    {
        byte[] contentBytes;
        int statusCode;

        public int ContentLength
        {
            get
            {
                if(contentBytes != null)
                {
                    return contentBytes.Length;
                }
                return 0;
            }
        }
        public string Status
        {
            get
            {
                if (Settings.STATUS_CODES.ContainsKey(StatusCode))
                {
                    StringBuilder res = new StringBuilder(64);
                    res.Append(StatusCode);
                    res.Append(" ");
                    res.Append(Settings.STATUS_CODES[StatusCode]);
                    return res.ToString();
                }
                return null;
            }
        }
        public string ContentType
        {
            get
            {
                if(!Headers.ContainsKey(""))
                {
                    return null;
                }
                return Headers[""];
            }
            set
            {
                Headers[""] = value;
            }
        }
        public IDictionary<string, string> Headers
        {
            get; private set;
        }
        public string ServerHeader
        {
            get; set;
        }
        public int StatusCode
        {
            get
            {
                if (statusCode <= 0)
                {
                    throw new InvalidOperationException("No Status Code");
                }
                return statusCode;
            }
            set
            {
                statusCode = value;
            }
        }

        public Response()
        {
            Headers = new Dictionary<string, string>();
            ServerHeader = "BIF-SWE1-Server";
        }
        public IDictionary<string, string> GetHeaders()
        {
            return Headers;
        }

        public int GetContentLength()
        {
            return ContentLength;
        }

        public string GetContentType()
        {
            return ContentType;
        }

        public void SetContentType(string value)
        {
            ContentType = value;
        }

        public int GetStatusCode()
        {
            if (statusCode <= 0)
            {
                throw new InvalidOperationException("No Status Code");
            }
            return statusCode;
        }

        public void SetStatusCode(int value)
        {
            statusCode = value;
        }

        public string GetStatus()
        {
            StringBuilder res = new StringBuilder(64);
            if (Settings.STATUS_CODES.ContainsKey(StatusCode))
            {
                res.Append(StatusCode);
                res.Append(" ");
                res.Append(Settings.STATUS_CODES[StatusCode]);
            }
            return res.ToString();
        }

        public string GetServerHeader()
        {
            return ServerHeader;
        }

        public void SetServerHeader(string value)
        {
            ServerHeader = value;
        }

        public void AddHeader(string header, string value)
        {
            Headers[header] = value;
        }

        public void Send(Stream network)
        {
            if (!string.IsNullOrEmpty(ContentType) && ContentLength <= 0)
            {
                throw new InvalidOperationException("No Content Type");
            }

            StreamWriter sw = new StreamWriter(network, Encoding.ASCII);
            sw.NewLine = "\r\n";
            sw.WriteLine("HTTP/1.1 {0}", Status);
            sw.WriteLine("Server: {0}", ServerHeader);
            foreach (var item in Headers)
            {
                sw.WriteLine("{0}: {1}", item.Key, item.Value);
            }
            sw.WriteLine();
            sw.Flush();

            // Write Content Data
            if (contentBytes != null)
            {
                try
                {
                    BinaryWriter bw = new BinaryWriter(network);
                    bw.Write(contentBytes);
                    bw.Flush();
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void SetContent(string content)
        {
            SetContent(Encoding.UTF8.GetBytes(content));
        }

        public void SetContent(byte[] content)
        {
            contentBytes = content;
            Headers[HTTP.CONTENT_LENGTH_LC] = content.Length.ToString();
        }

        public void SetContent(Stream stream)
        {
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            SetContent(ms.ToArray());
        }
    }
}
