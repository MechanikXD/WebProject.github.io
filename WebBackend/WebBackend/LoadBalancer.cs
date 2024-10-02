namespace WebBackend.Balancer;

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

public class LoadBalancer {
    private static readonly HttpClient HttpClient = new HttpClient();

    private static readonly string[] Servers =
        { "http://localhost:5001", "http://localhost:5002", "http://localhost:5003" }; // SERVER LIST

    private static int _currentServerIndex = 0;
    private static readonly HttpListener Listener = new HttpListener();

    public LoadBalancer() {
        // Create an HttpListener to handle incoming requests
        Listener.Prefixes.Add("http://localhost:8080/"); // PORT LISTENING
        Listener.Start();
        Console.WriteLine("Load Balancer is listening on http://localhost:8080/");
    }

    public async void Run() {
        while (true) {
            // Wait for an incoming request
            HttpListenerContext context = await Listener.GetContextAsync();
            Console.WriteLine("Received request for: " + context.Request.Url);

            // Distribute the request using the Round Robin algorithm
            var serverUrl = GetNextServer();
            Console.WriteLine($"Forwarding request to: {serverUrl}");

            // Forward the request to the selected server
            var responseText = await ForwardRequestToServer(serverUrl, context.Request);

            // Send the server response back to the client
            var buffer = System.Text.Encoding.UTF8.GetBytes(responseText);
            context.Response.ContentLength64 = buffer.Length;
            await context.Response.OutputStream.WriteAsync(buffer);
            context.Response.OutputStream.Close();
        }
    }

    // Round Robin algorithm to get the next server
    private static string GetNextServer() {
        var server = Servers[_currentServerIndex];
        _currentServerIndex = (_currentServerIndex + 1) % Servers.Length;
        return server;
    }

    // Forward the request to the selected backend server
    private static async Task<string> ForwardRequestToServer(string serverUrl, HttpListenerRequest request) {
        // Create a new HttpRequestMessage based on the incoming request
        var forwardRequest = new HttpRequestMessage(new HttpMethod(request.HttpMethod), serverUrl);

        // Forward the request and get the response from the backend server
        var serverResponse = await HttpClient.SendAsync(forwardRequest);
        return await serverResponse.Content.ReadAsStringAsync();
    }
}