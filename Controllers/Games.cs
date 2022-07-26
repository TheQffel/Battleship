using Microsoft.AspNetCore.Mvc;

namespace Battleship.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Games : ControllerBase
    {
        [HttpGet("{Player}")]
        public string Print(string Player)
        {
            string[][] PlayerGames = Database.Get("Battleship_Games", "GameId", $"PlayerA = '{Player}' OR PlayerB = '{Player}'");
            
            string JsonGames = " ";
            
            if(PlayerGames[0][0].Length > 0)
            {
                JsonGames = "\n";
                
                for (int i = 0; i < PlayerGames.Length; i++)
                {
                    JsonGames += "\t" + Battleship.Games.ById(long.Parse(PlayerGames[i][0])) + ",\n";
                }
                
                JsonGames = JsonGames.Remove(JsonGames.LastIndexOf(',')) + "\n";
            }
            return "[" + JsonGames +  "]";
        }
        
        [HttpGet("{PlayerA}/{PlayerB}")]
        public string Print(string PlayerA, string PlayerB)
        {
            return Battleship.Games.New(PlayerA, PlayerB).ToString();
        }
    }
}