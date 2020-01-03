using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWebServer
{
    class Url: IUrl
    {
        string fragment;
        string fileName;
        string extension;
        string rawUrl;
        string path;
        Dictionary<string, string> parameterDict;
        string[] segments;
        public string RawUrl
        {
            get
            {
                return rawUrl;
            }
        }
        public string Path
        {
            get
            {
                return path;
            }
        }
        public IDictionary<string, string> Parameter
        {
            get
            {
                return parameterDict;
            }
        }

        public int ParameterCount
        {
            get
            {
                return parameterDict.Count;
            }
        }

        public string[] Segments
        {
            get
            {
                return segments;
            }
        }

        public string FileName
        {
            get
            {
                return fileName;
            }
        }

        public string Extension
        {
            get
            {
                return extension;
            }
        }

        public string Fragment
        {
            get
            {
                return fragment;
            }
        }
        public Url(string rawUrl)
        {
            this.rawUrl = rawUrl;
            parameterDict = new Dictionary<string, string>();
            if (rawUrl == null)
            {
                return;
            }
            this.path = rawUrl;

            string[] tmp = rawUrl.Split('?');
            if (tmp.Length > 0)
            {
                path = tmp[0];
            }

            segments = path?.Split(new char[] { '/', '\\' }).Skip(1).ToArray() ?? new string[] { };
            string[] tmpfrgmnt = path.Split('#');
            if (tmpfrgmnt.Length > 1)
            {
                fragment = tmpfrgmnt[1];
                path = tmpfrgmnt[0];
            }

            if (tmp.Length > 1)
            {
                string[] listOfParameters = tmp[1].Split('&');
                string[] resArray;
                foreach (string prmt in listOfParameters)
                {
                    resArray = prmt.Split('=');
                    if (resArray.Length > 1)
                    {
                        parameterDict.Add(resArray[0], resArray[1]);
                    }
                    else if (resArray.Length > 0)
                    {
                        parameterDict.Add(resArray[0], "");
                    }
                }
            }

            string end = segments.Last();
            if (end != null && end.Contains('.'))
            {
                fileName = end;
            }

            string[] tmpExtns = fileName.Split('.');
            if (tmpExtns.Length >= 1)
            {
                extension = "." + tmpExtns.Last();
            }
        }
    }
}
