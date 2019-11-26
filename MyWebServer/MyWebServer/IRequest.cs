using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BIF.SWE1.Interfaces
{
    public interface IRequest
    {
        /// <summary>
        /// Returns true if the request is valid. A request is valid, if method and url could be parsed. A header is not necessary.
        /// </summary>
        bool GetIsValid();

        /// <summary>
        /// Returns the request method in UPPERCASE. get -> GET.
        /// </summary>
        string GetMethod();

        /// <summary>
        /// Returns a URL object of the request. Never returns null.
        /// </summary>
        IUrl GetUrl();

        /// <summary>
        /// Returns the request header. Never returns null. All keys must be lower case.
        /// </summary>
        IDictionary<string, string> GetHeaders();

        /// <summary>
        /// Returns the user agent from the request header
        /// </summary>
        string GetUserAgent();

        /// <summary>
        /// Returns the number of header or 0, if no header where found.
        /// </summary>
        int GetHeaderCount();

        /// <summary>
        /// Returns the parsed content length request header.
        /// </summary>
        int GetContentLength();

        /// <summary>
        /// Returns the parsed content type request header. Never returns null.
        /// </summary>
        string GetContentType();

        /// <summary>
        /// Returns the request content (body) stream or null if there is no content stream.
        /// </summary>
        Stream GetContentStream();

        /// <summary>
        /// Returns the request content (body) as string or null if there is no content.
        /// </summary>
        string GetContentString();

        /// <summary>
        /// Returns the request content (body) as byte[] or null if there is no content.
        /// </summary>
        byte[] GetContentBytes();
    }
}
