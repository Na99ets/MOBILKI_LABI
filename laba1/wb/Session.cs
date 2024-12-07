using System.Text.Json;


namespace wb
{
    class Player{

        public long Id { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
        public int Rocks { get; set; }
        public int Papers { get; set; }
        public int Scissors { get; set; }

    }
    class Data{
        public List<Player> Players { get; set; }
    }

    class Session{
        private static Data data = new Data{
            Players = new()
        };
        public string id = "";
        private Dictionary<long, string> Users = new();

        private bool gameIsStarted = false;
        private long winner;


        public int UsersCount(){
            return this.Users.Count;
        }

        public List<long> GetUsers(){
            return Users.Keys.ToList();
        }

        public void SetUser(long playerId){
            Users.Add(playerId, "");
        }

        public long GetSecondUserId(long playerId){
            foreach(var user in Users){
                if(user.Key != playerId){
                    return user.Key;
                }
            }
            return 0;
        }

        public bool GameStatus(){
            return gameIsStarted;
        }

        public void StartGame(){
            gameIsStarted = true;
        }

        public void SetAnswer(long playerId, string answer){
                Users[playerId] = answer;
        }

        public string GetAnswer(long playerId){
                return Users[playerId];
        }

        public string GetSecondUserAnswer(long playerId){
            foreach(var user in Users){
                if(user.Key != playerId){
                    return Users[user.Key];
                }
            }
            return "";
        }

        public string GetWiner(long playerId){
            if (this.GetAnswer(playerId) == this.GetSecondUserAnswer(playerId)){
                return "draw";
            }
            if(this.GetSecondUserAnswer(playerId) == "Rock🪨")
                if(this.GetSecondUserAnswer(playerId) == "Paper📄")
                    winner = this.GetSecondUserId(playerId);
                if(this.GetSecondUserAnswer(playerId) == "Scissors✂️")
                    winner = playerId;
            if (this.GetAnswer(playerId) == "Paper📄"){
                if(this.GetSecondUserAnswer(playerId) == "Rock🪨")
                    winner = playerId;
                if(this.GetSecondUserAnswer(playerId) == "Scissors✂️")
                    winner = this.GetSecondUserId(playerId);
            }
            if (this.GetAnswer(playerId) == "Scissors✂️"){
                if(this.GetSecondUserAnswer(playerId) == "Rock🪨")
                    winner = this.GetSecondUserId(playerId);
                if(this.GetSecondUserAnswer(playerId) == "Paper📄")
                    winner = playerId;
            }
            if(winner == playerId){
                return "you win";
            }
            return "you lose";

        }

        public void SaveSession(long playerId){
            
            bool _playerInPlayers = false;

            int r = 0;
            int p = 0;
            int s = 0;

            switch(GetAnswer(playerId)){
                case "Rock🪨":
                    r ++;
                    break;
                case "Paper📄":
                    p ++;
                    break;
                case "Scissors✂️":
                    s ++;
                    break;
                    
            }

            foreach(var player in data.Players){
                if(player.Id == playerId){
                    _playerInPlayers = true;
                    player.Id = playerId;
                    player.Rocks += r;
                    player.Papers += p;
                    player.Scissors += s;
                }
            }
            if(!_playerInPlayers){
                var player = new Player{
                Id = playerId,
                Rocks = r,
                Papers = p,
                Scissors = s
                };
                data.Players.Add(player);
            }

            string json = JsonSerializer.Serialize(data);
            File.WriteAllText("date.json", json);

        }
    }
}