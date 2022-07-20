using System;

namespace Battleship
{
    public class Game
    {
        private readonly long GameId;
        public string PlayerA { get; private set; } = "";
        public string PlayerB { get; private set; } = "";
        private bool Turn = true;
        
        private readonly Board GameBoard;
        
        public Game(long GameId)
        {
            this.GameId = GameId;
            GameBoard = new();
            Load();
        }
        
        public Game(string PlayerA, string PlayerB)
        {
            GameId = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            
            this.PlayerA = PlayerA;
            this.PlayerB = PlayerB;
            
            GameBoard = new();
        }
        
        public override string ToString()
        {
            return GameId + ": " + PlayerA + " vs " + PlayerB;
        }
        
        private void Load()
        {
            string[] Data = Database.Get("Battleship_Games", "PlayerA, PlayerB, Turn, Board", $"GameId = '{GameId}'")[0];
            
            if(Data.Length >= 4)
            {
                PlayerA = Data[0];
                PlayerB = Data[1];
                Turn = bool.Parse(Data[2]);
                
                GameBoard.FromString(Data[3]);
                
            }
            else
            {
                Console.Log(Console.LogType.Warning, "Game " + GameId + " is corrupted!");
            }
        }
        
        private void Save()
        {
            Database.Set("Battleship_Games", $"PlayerA = '{PlayerA}', PlayerB = '{PlayerB}', Turn = '{Turn}', Board = '{GameBoard}', ", $"GameId = '{GameId}'");
        }
    }
}