using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using Owin;

namespace Requestoring {

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

	    Owin.Response owinResponse = Application.GetResponse(TheApplication, request);

	    // generate the Requestoring.Response
	    Response response  = new Response();
	    response.Status = owinResponse.StatusCode;
	    response.Body   = owinResponse.BodyText;

	    response.Headers = new Dictionary<string,string>();
	    foreach (KeyValuePair<string,IEnumerable<string>> header in owinResponse.Headers)
		foreach (string value in header.Value) {
		    string key = header.Key;
		    if (key == "location")     key = "Location";
		    if (key == "content-type") key = "Content-Type";
		    response.Headers[key] = value;
		}

	    // headers ...
	    return response;
        }
    }
}
