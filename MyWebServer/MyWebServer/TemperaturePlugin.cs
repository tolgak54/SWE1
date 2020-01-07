using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebServer
{
    class TemperaturePlugin : IPlugin
    {
        public Single CanHandle(IRequest req)
        {
            if (req == null || req.Url() == null || req.Url().Segments.Length < 1)
            {
                return 0.0f;
            }
            if (req.Url().Segments[0] == "temp")
            {
                return 1.0f;
            }
            return 0.0f;
        }

        public IResponse Handle(IRequest req)
        {
            bool restXML = false;
            if (req.Url().Parameter.ContainsKey("type"))
            {
                string type = req.Url().Parameter["type"].ToLower();
                restXML = type == "rest";
            }

            // Create response object
            Response result = new Response();
            result.StatusCode = 400;

            // Check if URL contains all needed information
            if (!(req.Url().Parameter.ContainsKey("from")
                && req.Url().Parameter.ContainsKey("until")))
            {
                return result;
            }

            // Parse date time values from GET parameters
            DateTime from, until;
            try
            {
                from = DateTime.Parse(req.Url().Parameter["from"]);
                until = DateTime.Parse(req.Url().Parameter["until"]);
            }
            catch (FormatException)
            {
                return result;
            }

            // Check if parsing was successful
            if (from == null || until == null)
            {
                return result;
            }

            return CreateHTML(result, req, data);
        }

        private IResponse CreateHTML(IResponse result, IRequest req, List<Temperature> data)
        {
            // 200 Success Code
            result.StatusCode = 200;
            result.ContentType = http.CONTENT_TYPE_TEXT_HTML;

            // Build Response
            StringBuilder content = new StringBuilder();
            content.Append("<html><body>");
            content.Append("<h1>Temperature</h1>");
            content.Append("<table>");
            content.Append("<tr><th>ID</th><th>Date</th><th>Kelvin</th><th>Celsius</th><th>Fahrenheit</th></tr>");
            foreach (Temperature current in data)
            {
                content.Append("<tr>");
                content.Append("<td>").Append(current.ID).Append("</td>");
                content.Append("<td>").Append(current.Date).Append("</td>");
                content.Append("<td>").Append(current.Kelvin).Append("</td>");
                content.Append("<td>").Append(current.Celsius).Append("</td>");
                content.Append("<td>").Append(current.Fahrenheit).Append("</td>");
                content.Append("</tr>");
            }
            content.Append("</table>");
            content.Append("</body></html>");

            // Finish Response
            result.SetContent(content.ToString());
            return result;
        }
    }
}
