using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;

public class Program {
    private DiscordSocketClient _client;
    protected Configuration config;

	public static Task Main(string[] args) => new Program().MainAsync();

	public async Task MainAsync()
	{
        _client = new DiscordSocketClient();

        _client.Log += Log;

        //TODO Perform better error handling.
        try {
            config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("./config.json"))!;
            Console.WriteLine(config.getToken());
            Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
        }
        catch (AggregateException ae)
        {
            Console.WriteLine("Caught aggregate exception-Task.Wait behavior");
            ae.Handle((x) =>
            {
                if (x is UnauthorizedAccessException) // This we know how to handle.
                {
                    Console.WriteLine("You do not have permission to access all folders in this path.");
                    Console.WriteLine("See your network administrator or try another path.");
                    return true;
                }
                return false; // Let anything else stop the application.
            });
        }

        

        await _client.LoginAsync(TokenType.Bot, config.getToken());
        await _client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
	}

    //TODO Create more robust logging (Probably a custom class with things such as terminal highlighting).
    private Task Log(LogMessage msg)
    {
	    Console.WriteLine(msg.ToString());
	    return Task.CompletedTask;
    }

}
