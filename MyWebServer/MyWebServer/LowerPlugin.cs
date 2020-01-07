using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebServer
{
    public class LowerPlugin : IPlugin
    {
        public Single CanHandle(IRequest req)
        {
            string body = req.GetContentString();
            if (body != null && body.StartsWith("text="))
            {
                return 1.0f;
            }
            return 0.0f;
        }

        public IResponse Handle(IRequest req)
        {
            string body = req.GetContentString();
            body = body.Remove(0, body.IndexOf('=') + 1);

            Response response = new Response();
            response.StatusCode = 200;
            response.ContentType = http.ContentTypeEncoding(http.CONTENT_TYPE_TEXT_PLAIN, "UTF-8");
            response.AddHeader(http.CONNECTION, http.CONNECTION_CLOSED);
            response.AddHeader(http.CONTENT_LANGUAGE, http.CONTENT_LANGUAGE_EN);

            // Send empty handling protocol
            if (string.IsNullOrEmpty(body.Trim()))
            {
                response.SetContent("Bitte geben Sie einen Text ein");
                return response;
            }

            // Send correctly executed protocol
            response.SetContent(body.ToLower());
            return response;
        }
    }
}
