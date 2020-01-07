using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyWebServer
{
    public class StaticFilePlugin : IPlugin
    {
        public Single CanHandle(IRequest req)
        {
            if (req == null || req.Url() == null || req.Url().RawUrl == null)
            {
                return 0.0f;
            }
            return 0.5f;
        }

        public IResponse Handle(IRequest req)
        {
            Response response = new Response();
            response.AddHeader(http.CONNECTION, http.CONNECTION_CLOSED);
            response.AddHeader(http.CONTENT_LANGUAGE, http.CONTENT_LANGUAGE_EN);
            response.AddHeader(http.CONTENT_ENCODING, http.CONTENT_ENCODING_UTF8);

            string file = null;
            string reqUrl = req.Url().RawUrl.Replace("/", @"\");
            string dir = AppContext.Current.WorkingDirectory + reqUrl;

            if (File.Exists(reqUrl))
            {
                return CreateResponse(response, reqUrl);
            }

            if (Directory.Exists(dir))
            {
                if (!dir.EndsWith(@"\"))
                {
                    dir += @"\";
                }
            }

            foreach (string doc in http.DEFAULT_DOCUMENTS)
            {
                string osFile = dir + doc.Replace("/", @"\");
                if (File.Exists(osFile))
                {
                    file = osFile;
                    break;
                }
            }

            if (file == null)
            {
                response.StatusCode = 404;
                return response;
            }
            return CreateResponse(response, file);
        }

        private IResponse CreateResponse(IResponse res, string file)
        {
            string ext = Path.GetExtension(file);

            res.StatusCode = 200;
            res.ContentType = http.ContentTypeEncoding(http.MimeTypeFromExtension(ext), "UTF-8");
            res.SetContent(File.ReadAllBytes(file));
            return res;
        }
    }
}
