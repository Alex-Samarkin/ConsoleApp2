using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create a listener.
            HttpListener listener = new HttpListener();

            listener.Prefixes.Add("http://localhost:8080/");
            listener.Prefixes.Add("http://localhost:8080/api/");
            listener.Prefixes.Add("http://localhost:8080/time/");

            listener.Start();
            Console.WriteLine("Listening...");

            // Note: The GetContext method blocks while waiting for a request.
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            // Construct a response.
            var url = request.Url.ToString();
            Console.WriteLine(url);

            string responseString = "";
            if(url.StartsWith("http://localhost:8080/"))
            { 
                responseString = "<HTML><BODY><H2>Hello world!</H2><hr></BODY></HTML>";
            }
            if (url.StartsWith("http://localhost:8080/api"))
            {
                responseString = "<HTML><BODY>API version 1</BODY></HTML>";
            }
            if (url.StartsWith("http://localhost:8080/time"))
            {
                responseString = $"<HTML><BODY>Time is {DateTime.Now}</BODY></HTML>";
            }

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);

            // You must close the output stream.
            output.Close();

            listener.Stop();
            Console.WriteLine("Stop");
            Console.ReadLine();
        }
    }
}
