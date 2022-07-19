using System;

namespace Battleship
{
    public class Game
    {
        private long GameId;
        public string PlayerA { get; }
        public string PlayerB { get; }
        private bool Turn = true;
        
        private Board GameBoard;
        
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
    }
}