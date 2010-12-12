using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Owin;
using Machine.Specifications;

namespace Requestoring.Specs {

    // TODO we need Owin.Session.Cookie to get sessions working as expected
    //
    // This is a port of the example Rack application that is currently used for testing 
    // HttpRequestor, which can be found at ./Specs/Requestor/web-app-for-specs.ru
    public class OwinExampleApplication : Application, IApplication {
        public override Owin.IResponse Call(IRequest rawRequest) {
            Owin.Request  request  = new Owin.Request(rawRequest);
            Owin.Response response = new Owin.Response();

	    switch (request.PathInfo) {
		case "":
		case "/":
		    response.Write("Hello World");
		    break;
		case "/info":
		    response.ContentType = "text/plain";
		    response.Write("You did: {0} {1}\n\n", request.Method, request.PathInfo);
		    response.Write("Times requested: 0 [SESSIONS NOT IMPLEMENTED YET]\n\n");
		    foreach (KeyValuePair<string,string> item in request.GET)
			response.Write("QueryString: {0} = {1}\n", item.Key, item.Value);
		    foreach (KeyValuePair<string,string> item in request.POST)
			response.Write("POST Variable: {0} = {1}\n", item.Key, item.Value);
		    foreach (KeyValuePair<string,IEnumerable<string>> header in request.Headers)
			foreach (string value in header.Value)
			    response.Write("Header: {0} = {1}\n", header.Key, value);
		    break;
		case "/boom":
		    response.SetStatus(500);
		    response.Write("Boom!");
		    break;
		case "/headers":
		    response.Write("This has custom headers FOO and BAR");
		    response.SetHeader("FOO", "This is the value of foo");
		    response.SetHeader("BAR", "Bar is different");
		    break;
		case "/redirect":
		    response.Write("Redirecting");
		    response.Redirect("/info?redirected=true");
		    break;
		default:
		    response.SetStatus(404);
		    response.Write("Not Found: {0} {1}", request.Method, request.PathInfo);
		    break;
	    }

            return response;
        }
    }

    public class OwinSpec : Requestor.Static {
        Establish context = () => Instance = new Requestor(new OwinRequestor(new OwinExampleApplication()));
    }
}

/*
use Rack::Session::Cookie

run lambda { |env|
  request  = Rack::Request.new(env)
  response = Rack::Response.new

  path = request.path_info.empty? ? '/' : request.path_info
  verb = request.request_method

  env['rack.session']['timesRequested'] ||= 0
  env['rack.session']['timesRequested'] +=  1

  case path
  when '', '/'
    response.write "Hello World"
  when '/info'
    response.headers['Content-Type'] = 'text/plain'
    response.write "You did: #{verb} #{path}"
    response.write "\n\n"
    response.write "Times requested: #{env['rack.session']['timesRequested']}"
    response.write "\n\n"
    request.GET.each do |query_string|
      response.write "QueryString: #{query_string.first} = #{query_string.last}\n"
    end
    request.POST.each do |post_variable|
      response.write "POST Variable: #{post_variable.first} = #{post_variable.last}\n"
    end
    response.write "\n"
    puts env.to_yaml
    env.each {|key, value| response.write "Header: #{key} = #{value}\n" }
  else
    response.write "Not Found: #{verb} #{path}"
    response.status = 404
  end

  response.finish
}

*/
