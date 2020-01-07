using System;
using System.Collections.Generic;
using System.Text;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    public class TestPlugin : IPlugin
    {
        public Single CanHandle(IRequest req)
        {
            if (CheckParameter(req))
            {
                return 1.0f;
            }
            if (CheckUrlPath(req))
            {
                return 0.8f;
            }

            return 0.0f;
        }

        private bool CheckParameter(IRequest req)
        {
            if (!req.Url().Parameter.ContainsKey("pluginTest"))
            {
                return false;
            }

            string param = req.Url().Parameter["pluginTest"];
            if (string.IsNullOrEmpty(param))
            {
                return false;
            }

            bool erg = false;
            try
            {
                erg = bool.Parse(param);
            }
            catch (FormatException fex)
            {
                Console.WriteLine(fex);
            }

            return erg;
        }

        private bool CheckUrlPath(IRequest req)
        {
            return req.Url().Segments[0] == "test" || req.Url().RawUrl == "/";
        }

        public IResponse Handle(IRequest req)
        {
            Response response = new Response();
            response.AddHeader(http.CONNECTION, http.CONNECTION_CLOSED);
            response.AddHeader(http.CONTENT_LANGUAGE, http.CONTENT_LANGUAGE_EN);
            response.AddHeader(http.CONTENT_ENCODING, http.CONTENT_ENCODING_UTF8);
            response.StatusCode = 200;
            response.ContentType = http.ContentTypeEncoding(http.CONTENT_TYPE_TEXT_HTML, "UTF-8");
            response.SetContent("<!DOCTYPE html><html><body><h1>Test Plugin</h1><h3>#+ßöäüabc123</h3></body></html>");
            return response;
        }
    }
}
