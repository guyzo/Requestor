using System;
using System.Collections.Generic;
using System.Text;
using Owin;

namespace Requestoring {

    public class OwinRequestor : IRequestor {

        IApplication Application { get; set; }

        public OwinRequestor(IApplication application) {
            Application = application;
        }

        public IResponse GetResponse(string verb, string url, IDictionary<string, string> postVariables, IDictionary<string, string> requestHeaders) {
            throw new NotImplementedException();
        }
    }
}
