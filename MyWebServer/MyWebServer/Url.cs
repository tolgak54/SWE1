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
                if (rawUrl == null)
                {
                    rawUrl = "";
                }
                if (rawUrl.Contains("?")==true)
                {
                    return PathTwo;
                }
                string[] temp2 = rawUrl.Split('#');
                return temp2[0];
            }
        }

        public string PathTwo
        {
            get
            {
                string[] tmp = rawUrl.Split('?');
                return tmp[0];
            }
        }
        public IDictionary<string, string> Parameter
        {
            get
            {
                if (parameterDict == null)
                {
                    parameterDict = new Dictionary<string, string>();
                    if (rawUrl == null)
                    {
                        rawUrl = "";
                    }
                    string[] tmp = rawUrl.Split('?');
                    if (tmp.Length == 2)
                    {
                        string[] tmp2 = tmp[1].Split('&');
                        for (int i = 0; i < tmp2.Length; i++)
                        {
                            string[] tmp3 = tmp2[i].Split('=');
                            parameterDict.Add(tmp3[0], tmp3[1]);
                        }
                    }
                }
                    return parameterDict;
            }
        }

        public int ParameterCount
        {
            get
            {
                return Parameter.Count;
            }
        }

        public string[] Segments
        {
            get
            {
                if (rawUrl==null)
                {
                    rawUrl = "";
                }
                List<string> resultList = new List<string>();
                string temp = rawUrl.Remove(0, 1);
                segments = temp.Split('/');
                //if (segments.Length <= 0)
                //{
                //    segments[0] = "0";
                //}
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
                if (rawUrl == null)
                {
                    rawUrl = "";
                }
                //if (segments[segments.Length - 1].Contains(".") == false)
                //{
                //    return "";
                //}
                //string result = segments[segments.Length - 1];
                string[] tmp = rawUrl.Split('#');
                string fragment = tmp[1];
                return fragment;
            }
        }
    }
}
