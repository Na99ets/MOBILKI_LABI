using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace wb
{
    class Program
    {
        // private static readonly Dictionary<string, List<Dictionary<long, string>>> Sessions = new();
        
        private static List<Session> Sessions = new();


        static async Task Main(string[] args)
        {
            var client = new TelegramBotClient("7944110435:AAEpwzSiY8OmY-Cqg9PCTHmzktlEljuIRwE");
            
            var me = await client.GetMe();

            Console.WriteLine(me.Username);

            client.StartReceiving(Update, Error);
            Console.ReadLine();

            static Task Error(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
            {
                return null;
            }

            async static Task Update(ITelegramBotClient bot, Update update, CancellationToken token)
            {
                var message = update.Message;

                if(message!= null && message.Text != null){
                    var player = message.Chat.Id;
                    if(message.Text.Contains("/start")){
                        var args = message.Text.Split(' ');
                        if (args.Length > 1)
                        {
                            string sessionId = args[1];
                            if(SessionExist(sessionId)){
                                GetSessionById(sessionId).SetUser(message.Chat.Id);
                                Session session = GetSessionById(sessionId);
                                foreach (var user in session.GetUsers())
                                    {
                                        Console.WriteLine($"{user} in game {sessionId}, {session.UsersCount()}");
                                    }
                                if(session.UsersCount() == 2){
                                    foreach (var playerId in session.GetUsers())
                                    {
                                        await bot.SendMessage(playerId, "Game is started!");
                                        var replyMarkup = new ReplyKeyboardMarkup(true)
                                            .AddNewRow("Rock🪨", "Paper📄", "Scissors✂️");

                                        var sent = await bot.SendMessage(playerId, "☠️☠️☠️Choose Rock, Paper or Scissors!☠️☠️☠️", replyMarkup: replyMarkup);

                                    }
                                }
                                return;
                            }                                
                            else
                            {
                                await bot.SendMessage(message.Chat, "Invalid or outdated session ID");
                                return;
                            }
                        }
                    }
                    switch(message.Text) {
                        case "/start":
                            await bot.SendMessage(message.Chat, "Hi lets go start a new game, just send /newgame");
                            break;
                        case "/newgame":
                            var link = CreateLink(bot, message);
                            await bot.SendMessage(message.Chat, $"The game is started, send this link `{link}` to your opponent", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                            break;
                        case "Rock🪨":
                        case "Scissors✂️":
                        case "Paper📄":
                            if(PlayerInSession(player)){
                                var session = GetSessionByPlayer(player);
                                session.SetAnswer(player, message.Text);
                                if(session.GetAnswer(player)!="" && session.GetSecondUserAnswer(player)==""){
                                    await bot.SendMessage(message.Chat.Id, "Wait for your opponent", replyMarkup: new ReplyKeyboardRemove());
                                    return;
                                }
                                else{
                                    if(session.GetAnswer(player)!="" && session.GetSecondUserAnswer(player)!=""){
                                        Console.WriteLine(session.UsersCount());
                                        foreach(var playerId in session.GetUsers()){
                                            Console.WriteLine(playerId);
                                            await bot.SendMessage(playerId, $"{session.GetSecondUserAnswer(playerId)}", replyMarkup: new ReplyKeyboardRemove());
                                            await bot.SendMessage(playerId, session.GetWiner(playerId));
                                            session.SaveSession(playerId);
                                            var replyMarkup = new ReplyKeyboardMarkup(true)
                                                .AddNewRow("Rematch♻️", "Leave🚫");

                                            var sent = await bot.SendMessage(playerId, "Play again with this player?", replyMarkup: replyMarkup);
                                        }                                        
                                        // if(GetSessionByPlayer(player)!=null){
                                        //     Sessions.Remove(GetSessionByPlayer(player));
                                        //     Console.WriteLine($"session: '{session.id}' removed");
                                        // }
                                        return;
                                    }
                                }
                            }
                            break;
                        case "Rematch♻️":
                            if(GetSessionByPlayer(player)!=null){
                                var currentSession = GetSessionByPlayer(player);
                                string sessionId = Guid.NewGuid().ToString();
                                Session session = new();
                                foreach (var playerId in currentSession.GetUsers())
                                {
                                    session.id = sessionId;
                                    session.SetUser(playerId);
                                    Console.WriteLine(session.id, playerId);
                                }
                                Sessions.Remove(GetSessionByPlayer(player));
                                Sessions.Add(session);
                                foreach (var playerId in session.GetUsers())
                                {
                                    await bot.SendMessage(playerId, "Game is started!", replyMarkup: new ReplyKeyboardRemove());
                                    var replyMarkup = new ReplyKeyboardMarkup(true)
                                        .AddNewRow("Rock🪨", "Paper📄", "Scissors✂️");

                                    var sent = await bot.SendMessage(playerId, "☠️☠️☠️Choose Rock, Paper or Scissors!☠️☠️☠️", replyMarkup: replyMarkup);

                                }
                            }
                            break;
                        case "Leave🚫":
                            if(GetSessionByPlayer(player)!=null){
                                var session = GetSessionByPlayer(player);
                                foreach (var playerId in session.GetUsers())
                                {
                                    await bot.SendMessage(playerId, "Game is ended!", replyMarkup: new ReplyKeyboardRemove());
                                }
                                Sessions.Remove(GetSessionByPlayer(player));
                                Console.WriteLine($"session: '{session.id}' removed");
                            }
                            break;
                    }
                    return;
                        
                }
            }

            static string CreateLink(ITelegramBotClient bot, Message message)
            {
                string sessionId = Guid.NewGuid().ToString();
                Session session = new();
                session.id = sessionId;
                session.SetUser(message.Chat.Id);
                Sessions.Add(session);
                foreach (var user in session.GetUsers())
                {
                    Console.WriteLine($"{message.Chat.Id} create session {sessionId}, {session.UsersCount()}");
                }


                

                string link = $"https://t.me/pogodnikjopka_bot?start={sessionId}";
                return link;
            }

            static bool SessionExist(string sessionId){
                bool result = false;
                foreach(var session in Sessions){
                    if(session.id == sessionId){
                        result = true;
                    }
                }
                return result;
            }

            static Session GetSessionById(string sessionId){
                foreach(var session in Sessions){
                    if(session.id == sessionId){
                        return session;
                    }
                }
                return null;
            }

            static Session GetSessionByPlayer(long player){
                foreach(var session in Sessions){
                    foreach(var playerId in session.GetUsers())
                    {
                        if(playerId == player){
                            return session;
                        }
                    }
                }
                return null;
            }

            static bool PlayerInSession(long player){
                bool result = false;
                foreach(var session in Sessions){
                    foreach(var playerId in session.GetUsers())
                    {
                        if(playerId == player){
                            result = true;
                        }
                    }
                }
                return result;
            }
        
        }
    }
}
