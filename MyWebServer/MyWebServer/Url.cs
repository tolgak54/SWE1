using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebServer
{
    class Url: IUrl
    {
        string rawUrl;
        string path;
        Dictionary<string, string> parameterDict;
        string[] segments;
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
                if (path==null)
                {
                    path = "";
                }
                string[] tmp = path.Split('?');
                string[] tmp2 = tmp[1].Split('&');
                parameterDict.Add("tmp2", tmp2.ToString());
                return parameterDict;
            }
        }

        public int ParameterCount
        {
            get
            {
                if (parameterDict.Count!=0)
                {
                    int paraCount = parameterDict.Count;
                    return paraCount;
                }
                return 0;
            }
        }

        public string[] Segments
        {
            get
            {
                if (path==null)
                {
                    path = "";
                }
                List<string> resultList = new List<string>();
                segments = path.Split('/');
                if (segments.Length <= 0)
                {
                    segments[0] = "0";
                }
                foreach (var item in segments)
                {
                    resultList.Add(item);
                }
                return resultList.ToArray();
            }
        }

        public string FileName
        {
            get
            {
                if (path==null)
                {
                    path = "";
                }
                if (segments[segments.Length - 1].Contains(".")==false)
                {
                    return "";
                }
                string result = segments[segments.Length - 1];
                string[] tmp = result.Split('#');
                string fileNameWithExt = tmp[0];
                return fileNameWithExt;
            }
        }

        public string Extension
        {
            get
            {
                if (path == null)
                {
                    path = "";
                }
                if (segments[segments.Length - 1].Contains(".") == false)
                {
                    return "";
                }
                string result = segments[segments.Length - 1];
                string[] tmp = result.Split('#');
                string fileNameWithExt = tmp[0];
                string[] tmp2 = fileNameWithExt.Split('.');
                string ext = tmp2[1];
                return "." + ext;
            }
        }

        public string Fragment
        {
            get
            {
                if (path == null)
                {
                    path = "";
                }
                if (segments[segments.Length - 1].Contains(".") == false)
                {
                    return "";
                }
                string result = segments[segments.Length - 1];
                string[] tmp = result.Split('#');
                string fragment = tmp[1];
                return fragment;
            }
        }
    }
}
