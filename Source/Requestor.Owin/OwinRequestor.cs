using System;
using System.IO;
using System.Net;
using System.Web;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Owin;

namespace Requestoring {

    // TODO We're going to wait until Owin.Test is done *and* working with Cookie-based sessions
    //      and then we'll integrate that.
    public class OwinRequestor : IRequestor {

        IApplication TheApplication { get; set; }

        public OwinRequestor(IApplication application) {
            TheApplication = application;
        }

        public IResponse GetResponse(string verb, string url, IDictionary<string, string> postVariables, IDictionary<string, string> requestHeaders) {
	    // Console.WriteLine("GetResponse({0}, {1})", verb, url);
	    Uri uri                    = new Uri("http://localhost:3000" + url);

	    // todo ... do something with post vars and headers ... do the query string work? ...
	    RequestWriter request = new RequestWriter(verb, uri.PathAndQuery);
	    foreach (KeyValuePair<string,string> header in requestHeaders)
		request.Headers[header.Key] = new string[] { header.Value };

	    /* COPY/PASTED from HttpRequestor */
            string postString = null;
            if (postVariables != null && postVariables.Count == 1) {
                string firstKey = null;
                foreach (string key in postVariables.Keys) { firstKey = key; break; }
                if (postVariables[firstKey] == null)
                    postString = firstKey; // <--- the key has the actual PostData
            }
            if (postVariables != null && postVariables.Count > 0) {
                if (postString == null) {
                    postString = "";
                    foreach (var variable in postVariables)
                        postString += variable.Key + "=" + HttpUtility.UrlEncode(variable.Value) + "&";
		    if (postString.EndsWith("&"))
			postString = postString.Substring(0, postString.Length - 1);
                }
		//Console.WriteLine("POST STRING: {0}", postString);
		request.SetBody(postString);

		// TODO make this writable!!!!
                //if (request.ContentType == null)
                //    request.ContentType = "application/x-www-form-urlencoded";
		string contentType = null;
		foreach (KeyValuePair<string,IEnumerable<string>> header in request.Headers)
		    if (header.Key.ToLower().Replace("_","").Replace("-","") == "contenttype")
			contentType = header.Value.ToString();
		if (contentType == null)
		    request.SetHeader("content-type", "application/x-www-form-urlencoded");

                // request.ContentLength = bytes.Length; // TODO add this to RequestWriter! also, does setBody do this for us?
                //using (var stream = request.GetRequestStream())
                //    stream.Write(bytes, 0, bytes.Length);
                //var bytes = Encoding.ASCII.GetBytes(postString);
		//request.SetBody(bytes);
            }
	    /* COPY/PASTED from HttpRequestor ... then tweaked ... */

	    Owin.Response owinResponse = Application.GetResponse(TheApplication, request);

	    // generate the Requestoring.Response
	    Response response  = new Response();
	    response.Status = owinResponse.StatusCode;
	    response.Body   = owinResponse.BodyText;

	    response.Headers = new Dictionary<string,string>();
	    foreach (KeyValuePair<string,IEnumerable<string>> header in owinResponse.Headers)
		foreach (string value in header.Value) {
		    string key = header.Key;
		    if (key == "location")     key = "Location"; // TODO move this behavior into the app/specs?  shouldn't be here
		    if (key == "content-type") key = "Content-Type";
		    response.Headers[key] = value;
		}

	    // headers ...
	    return response;
        }
    }
}
