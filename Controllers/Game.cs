using Microsoft.AspNetCore.Mvc;

namespace Battleship.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Game : ControllerBase
    {
        [HttpGet("{GameId}")]
        public string Print(long GameId)
        {
            return Battleship.Games.ById(GameId).ToString();
        }
        
        [HttpGet("{GameId}/{Player}")]
        public string Print(long GameId, string Player, string? Action = null)
        {
            if(Action != null)
            {
                if(Action.Contains('-')) // Ships
                {
                    string[] Ships = Action.Split('-');
                    if(!Battleship.Games.ById(GameId).PlaceShips(Player, Ships, out string? Error))
                    {
                        if(Error == null) { Error = "Unknown"; }
                        return "{ \"Status\": \"Error\", \"Error\": \"" + Error + "\" }";
                    }
                    Battleship.Games.ById(GameId).SyncWithDb();
                    return "{ \"Status\": \"Ok\" }";
                }
                else // Move
                {
                    if(Action.Length < 2) { return "{ \"Status\": \"Error\", \"Error\": \"Wrong field!\" }"; }
                    if(!Battleship.Games.ById(GameId).Move(Player, Action[0], Action[1], out string? Error))
                    {
                        if(Error == null) { Error = "Unknown"; }
                        return "{ \"Status\": \"Error\", \"Error\": \"" + Error + "\" }";
                    }
                    Battleship.Games.ById(GameId).SyncWithDb();
                    return "{ \"Status\": \"Ok\" }";
                }
            }
            string PlayerBoard = "\n\"Player\":\n" + Battleship.Games.ById(GameId).PlayerBoard(Player);
            string OpponentBoard = "\n\"Opponent\":\n" + Battleship.Games.ById(GameId).OpponentBoard(Player);
            OpponentBoard = OpponentBoard.Replace('1', '0') + "\n"; // Hide opponent's ships.
            return "{" + PlayerBoard + ", " + OpponentBoard + "}";
        }
    }
}