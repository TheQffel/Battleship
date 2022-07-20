using System.Collections.Generic;

namespace Battleship
{
    public static class Games
    {
        private static Dictionary<long, Game> AllGames = new();
        
        public static Game ById(long Id)
        {
            if(!AllGames.ContainsKey(Id))
            {
                Game ResumedGame = new(Id);
                AllGames[ResumedGame.Id()] = ResumedGame;
            }
            return AllGames[Id];
        }
        
        public static Game New(string PlayerA, string PLayerB)
        {
            Game NewGame = new(PlayerA, PLayerB);
            AllGames[NewGame.Id()] = NewGame;
            return AllGames[NewGame.Id()];
        }
    }
}