using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace MyWebServer
{
    public class NavigationPlugin : IPlugin
    {
        private static object refreshLock = new object();
        private static bool refreshing = false;
        private static Dictionary<string, List<string>> streetCityMap;
        
        public Single CanHandle(IRequest req)
        {
            if (req == null || req.Url() == null || req.Url().Segments.Length < 1)
            {
                return 0.0f;
            }
            if (req.Url().Segments[0] == "navi")
            {
                if (http.METHOD_GET.Equals(req.GetMethod(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return 0.8f;
                }
                if (http.METHOD_POST.Equals(req.GetMethod(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return 1.0f;
                }
                return 0.5f;
            }
            return 0.0f;
        }

        public IResponse Handle(IRequest req)
        {
            IResponse result = new Response();
            result.StatusCode = 200;

            string street = null;
            if (req.Url().Parameter.ContainsKey("street"))
            {
                street = req.Url().Parameter["street"];
            }
            else if (req.GetContentString() != null && req.GetContentString().Contains("street="))
            {
                string[] splits = req.GetContentString().Split('=');
                if (splits.Length > 1)
                {
                    street = splits[1];
                }
            }
            if (req.Url().Parameter.ContainsKey("refresh"))
            {
                bool refresh = Boolean.Parse(req.Url().Parameter["refresh"]);
                if (refresh)
                {
                    if (!LoadCityStreetCorrelation())
                    {
                        result.SetContent("Warnung: Karte wird gerade aufbereitet");
                        return result;
                    }
                }
            }

            lock (refreshLock)
            {
                if (refreshing)
                {
                    result.SetContent("Warnung: Karte wird gerade aufbereitet");
                    return result;
                }
            }
            if (streetCityMap == null)
            {
                if (!LoadCityStreetCorrelation())
                {
                    result.SetContent("Warnung: Karte wird gerade aufbereitet");
                    return result;
                }
            }
            if (string.IsNullOrEmpty(street))
            {
                result.SetContent("Bitte geben Sie eine Anfrage ein");
                return result;
            }
            else
            {
                StringBuilder content = new StringBuilder();

                bool containsKey = false;
                lock (streetCityMap)
                {
                    containsKey = streetCityMap.ContainsKey(street);
                }

                if (containsKey)
                {
                    content.Append("Orte gefunden,");
                    lock (streetCityMap)
                    {
                        foreach (string city in streetCityMap[street])
                        {
                            content
                                .Append(city)
                                .Append(',');
                        }
                    }
                    content.Length--;
                }
                else
                {
                    content.Append("Keine Orte gefunden");
                }
                
                result.SetContent(content.ToString());
            }
            return result;
        }

        private bool LoadCityStreetCorrelation()
        {
            lock (refreshLock)
            {
                if (refreshing)
                {
                    return false;
                }
                refreshing = true;
            }

            streetCityMap = new Dictionary<string, List<string>>(6533);

            string street = null;
            if (File.Exists("./streetmap/map.osm.xml"))
            {
                using (XmlReader reader = XmlReader.Create("./streetmap/map.osm.xml"))
                {
                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        if (reader.NodeType != XmlNodeType.Element)
                        {
                            continue;
                        }

                        if (reader.Name == null || reader.Name != "tag")
                        {
                            continue;
                        }

                        string key = reader.GetAttribute("k");

                        if (key != null && (key == "name" || key == "addr:street"))
                        {
                            street = reader.GetAttribute("v");
                        }

                        if (street != null && key != null && (key == "city" || key == "addr:city"))
                        {
                            lock (streetCityMap)
                            {
                                if (!streetCityMap.ContainsKey(street))
                                {
                                    streetCityMap.Add(street, new List<string>());
                                }
                                streetCityMap[street].Add(reader.GetAttribute("v"));
                            }
                            street = null;
                        }
                    }
                }
            }

            lock (refreshLock)
            {
                refreshing = false;
            }
            return true;
        }
    }
}
