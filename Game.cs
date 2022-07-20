using System;

namespace Battleship
{
    public class Game
    {
        private readonly long GameId;
        public string PlayerA { get; private set; } = "";
        public string PlayerB { get; private set; } = "";
        private bool Turn = true;
        
        private readonly Board BoardA;
        private readonly Board BoardB;
        
        public Game(long GameId)
        {
            this.GameId = GameId;
            BoardA = new();
            BoardB = new();
            Load();
        }
        
        public Game(string PlayerA, string PlayerB)
        {
            GameId = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            
            this.PlayerA = PlayerA;
            this.PlayerB = PlayerB;
            
            BoardA = new();
            BoardB = new();
            
            Database.Add("Battleship_Games", "GameId, PlayerA, PlayerB, Turn, BoardA, BoardB", $"'{GameId}', '{PlayerA}', '{PlayerB}', '{(Turn ? 1 : 0)}', '{BoardA}', '{BoardB}'");
        }
        
        ~Game() { Save(); }
        
        public long Id() { return GameId; } 
        
        public Board Board(string Player)
        {
            if(Player == PlayerA) { return BoardA; }
            if(Player == PlayerB) { return BoardB; }
            return new Board();
        }
        
        public override string ToString()
        {
            return "{" + $" \"GameId\": {GameId}, \"PlayerA\": \"{PlayerA}\", \"PlayerB\": \"{PlayerB}\", \"Turn\": {Turn} " + "}";
        }
        
        private void Load()
        {
            string[] Data = Database.Get("Battleship_Games", "PlayerA, PlayerB, Turn, BoardA, BoardB", $"GameId = '{GameId}'")[0];
            
            if(Data.Length >= 4)
            {
                Data[2] = Data[2].Replace("1", "true").Replace("0", "false");
                
                PlayerA = Data[0];
                PlayerB = Data[1];
                Turn = bool.Parse(Data[2]);
                BoardA.FromString(Data[3]);
                BoardB.FromString(Data[4]);
            }
            else
            {
                Console.Log(Console.LogType.Warning, "Game " + GameId + " is missing or corrupted!");
            }
        }
        
        private void Save()
        {
            Database.Set("Battleship_Games", $"PlayerA = '{PlayerA}', PlayerB = '{PlayerB}', Turn = '{Turn}', BoardA = '{BoardA}', BoardB = '{BoardB}'", $"GameId = '{GameId}'");
        }
    }
}