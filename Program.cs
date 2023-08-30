using System.Text;
using System.Diagnostics;
using Discord;
using Discord.WebSocket;
using Types;
using Newtonsoft.Json;

#nullable enable

class Bot
{
    public static string? BOT_TOKEN = getToken();
    static async Task Main()
    {
        var config = new DiscordSocketConfig
        {
            AlwaysDownloadUsers = true,
            LogLevel = LogSeverity.Info,
            GatewayIntents = GatewayIntents.All
        };

        var client = new DiscordSocketClient(config);

        client.Log += Log;
    
        client.MessageReceived += async Task (Message) =>
        {
            if (Message.Content.ToLower() == "ping")
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                EmbedBuilder embed = new EmbedBuilder();
                embed.WithTitle("Pong");

                var reply = await Message.Channel.SendMessageAsync(text: "Pong", embed: embed.Build());
                stopwatch.Stop();
                TimeSpan responseTime = stopwatch.Elapsed;

                ButtonBuilder button1 = new ButtonBuilder()
                .WithCustomId("click-me")
                .WithLabel($"Response time: {Convert.ToInt32(responseTime.TotalMilliseconds)} ms")
                .WithStyle(ButtonStyle.Success);

                button1.IsDisabled = true;
                ComponentBuilder Row = new ComponentBuilder().WithButton(button1);
                await reply.ModifyAsync((m) =>
                {
                    m.Components = Row.Build();
                    m.Embeds = null;
                });

                stopwatch.Restart();
            }
        };

        await client.LoginAsync(TokenType.Bot, BOT_TOKEN);
        await client.StartAsync();

        await Task.Delay(-1);
    }
    static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
    static string? getToken()
    {
        var Text_Json = File.ReadAllText("config.json", Encoding.UTF8);
        JSON? json = JsonConvert.DeserializeObject<JSON>(Text_Json);
        return json!.token;
    }

}
