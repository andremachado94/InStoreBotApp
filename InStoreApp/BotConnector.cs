using Microsoft.Bot.Connector.DirectLine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStoreApp
{
    public class BotConnector
    {
        private static string directLineSecret = "3_2aT1ijBtM.cwA.DDw.SqBs2RHLim-Wo0ASWbpChRJ_wRvKcg9NcPZ2xAZ6I6c";
        private static string botId = "SKB-LGP";
        private static string fromUser = "DirectLineSampleClientUser";

        /*
         private static string directLineSecret = "MMfKoIJvtwE.cwA.qo0.rrFm3sLzYD3QgdSR6W_LC9ofqscazQqB2Xvg_sZd2ks";
        private static string botId = "InStoreBotTest";
        private static string fromUser = "DirectLineSampleClientUser";     
        */

        DirectLineClient client2;
        Conversation conversation2;

        public async Task Test()
        {
            await startConversation();

            while (true)
            {
                string input = Console.ReadLine().Trim();

                if (input.ToLower() == "exit")
                {
                    break;
                }
                else
                {
                    string resp = await sendBotMessage(input);
                    Console.WriteLine("Response: " + resp);
                }
            }
        }

        public async Task startConversation()
        {
            try
            {
                client2 = new DirectLineClient(directLineSecret);
                conversation2 = await client2.Conversations.StartConversationAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public async Task<string> sendBotMessage(string msg)
        {
            Activity userMessage = new Activity
            {
                From = new ChannelAccount(fromUser),
                Text = msg,
                Type = ActivityTypes.Message
            };
            await client2.Conversations.PostActivityAsync(conversation2.ConversationId, userMessage);
            string response = await ReadBotMessageAsync(client2, conversation2.ConversationId);

            return response;
        }

        private async Task<string> ReadBotMessageAsync(DirectLineClient client, string conversationId)
        {
            string watermark = null;
            var activitySet = await client2.Conversations.GetActivitiesAsync(conversationId, watermark);
            watermark = activitySet?.Watermark;

            Console.WriteLine("Watermark: " + watermark + "\n");

            var activities = from x in activitySet.Activities
                             where x.From.Id == botId
                             select x;

            string resp = "Hey - ";

            foreach (Activity activity in activities)
            {
                Console.WriteLine("Text: " + activity.Text);
                Console.WriteLine("Format: " + activity.TextFormat);

                Console.WriteLine("Locale: " + activity.Locale);
                Console.WriteLine();

                resp = activity.Text;


                if (activity.Attachments != null)
                {
                    foreach (Attachment attachment in activity.Attachments)
                    {
                        switch (attachment.ContentType)
                        {
                            case "application/vnd.microsoft.card.hero":
                                RenderHeroCard(attachment);
                                break;

                                /*case "image/png":
                                    Console.WriteLine($"Opening the requested image '{attachment.ContentUrl}'");

                                    Process.Start(attachment.ContentUrl);
                                    break;
                                    */
                        }
                    }
                }

            }
            return resp;
        }

        /*

        public static async Task StartBotConversation()
        {
            DirectLineClient client;


            try
            {
                Console.WriteLine("Creating Client");
                Console.Clear();

                client = new DirectLineClient(directLineSecret);

                Console.WriteLine("Client Created");
                Console.Clear();

                Console.WriteLine("Starting Conversation");
                var conversation = await client.Conversations.StartConversationAsync();
                Console.WriteLine("Conversation is over");

                Console.WriteLine("Reading Messages Async");
                new System.Threading.Thread(async () => await ReadBotMessagesAsync(client, conversation.ConversationId)).Start();
                Console.WriteLine("Thread Created");

                Console.Write("Command> ");

                while (true)
                {
                    string input = Console.ReadLine().Trim();

                    if (input.ToLower() == "exit")
                    {
                        break;
                    }
                    else
                    {
                        if (input.Length > 0)
                        {
                            Activity userMessage = new Activity
                            {
                                From = new ChannelAccount(fromUser),
                                Text = input,
                                Type = ActivityTypes.Message
                            };

                            await client.Conversations.PostActivityAsync(conversation.ConversationId, userMessage);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        */
        private static async Task ReadBotMessagesAsync(DirectLineClient client, string conversationId)
        {
            string watermark = null;
            int j = 0;
            while (true)
            {
                //Console.WriteLine("Activity Set number " + j);
                j++;
                var activitySet = await client.Conversations.GetActivitiesAsync(conversationId, watermark);
                watermark = activitySet?.Watermark;

                var activities = from x in activitySet.Activities
                                 where x.From.Id == botId
                                 select x;

                foreach (Activity activity in activities)
                {
                    Console.WriteLine(activity.Text);
                    /*
                                        if (activity.Attachments != null)
                                        {
                                            foreach (Attachment attachment in activity.Attachments)
                                            {
                                                switch (attachment.ContentType)
                                                {
                                                    case "application/vnd.microsoft.card.hero":
                                                        RenderHeroCard(attachment);
                                                        break;

                                                    case "image/png":
                                                        Console.WriteLine($"Opening the requested image '{attachment.ContentUrl}'");

                                                        Process.Start(attachment.ContentUrl);
                                                        break;
                                                }
                                            }
                                        }
                                        */

                    Console.Write("Command> ");
                }

                await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
            }
        }

        private static void RenderHeroCard(Attachment attachment)
        {
            const int Width = 70;
            Func<string, string> contentLine = (content) => string.Format($"{{0, -{Width}}}", string.Format("{0," + ((Width + content.Length) / 2).ToString() + "}", content));


            var heroCard = JsonConvert.DeserializeObject<HeroCard>(attachment.Content.ToString());

            if (heroCard != null)
            {
                Console.WriteLine("/{0}", new string('*', Width + 1));
                Console.WriteLine("{0}", contentLine(heroCard.Title));
                Console.WriteLine("{0}", new string(' ', Width));
                Console.WriteLine("{0}", contentLine(heroCard.Text));
                Console.WriteLine("{0}/", new string('*', Width + 1));
            }
        }
    }
}
