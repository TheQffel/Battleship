using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Battleship.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class User : ControllerBase
    {
        [HttpGet("")]
        public string Check()
        {
            string? Username = Request.Cookies["Username"];
            string? Password = Request.Cookies["Password"];
            
            if(Username != null && Password != null)
            {
                if(Username.Length is < 4 or > 24 || !Username.All(char.IsLetterOrDigit)) { return "{ \"Status\": false }"; }
                if(Password.Length is < 8 or > 32) { return "{ \"Status\": false }"; }
                
                string Hash = Database.Get("Battleship_Users", "Password", $"Username = '{Username}'")[0][0];
                return "{ \"Status\": " + (HashText(Password) == Hash).ToString().ToLower() + " }";
            }
            return "{ \"Status\": false }";
        }
        
        [HttpGet("logout")]
        public string Logout()
        {
            Response.Cookies.Delete("Username");
            Response.Cookies.Delete("Password");
            return "{ \"Status\": true }";
        }
        
        [HttpPost("login")]
        public string Login([FromForm]string Username, [FromForm]string Password)
        {
            if(Username.Length is < 4 or > 24 || !Username.All(char.IsLetterOrDigit)) { return "{ \"Status\": false }"; }
            if(Password.Length is < 8 or > 32) { return "{ \"Status\": false }"; }
            
            string Hash = Database.Get("Battleship_Users", "Password", $"Username = '{Username}'")[0][0];
            if(HashText(Password) == Hash)
            {
                Response.Cookies.Append("Username", Username);
                Response.Cookies.Append("Password", Password);
                return "{ \"Status\": true }";
            }
            return "{ \"Status\": false }";
        }
        
        [HttpPost("register")]
        public string Register([FromForm]string Username, [FromForm]string Password)
        {
            if(Username.Length is < 4 or > 24 || !Username.All(char.IsLetterOrDigit)) { return "{ \"Status\": false }"; }
            if(Password.Length is < 8 or > 32) { return "{ \"Status\": false }"; }
            
            string User = Database.Get("Battleship_Users", "Password", $"Username = '{Username}'")[0][0];
            if(User.Length > 1) { return "{ \"Status\": false }"; }
            Database.Add("Battleship_Users", "Username, Password", $"'{Username}', '{HashText(Password)}'");
            return Login(Username, Password);
        }
        
        private string HashText(string Text)
        {
            byte[] Bytes = Encoding.UTF8.GetBytes(Text);
            SHA256 HashString = SHA256.Create();
            byte[] Hash = HashString.ComputeHash(Bytes);
            string HashText = "";
            foreach (byte X in Hash)
            { HashText += $"{X:x2}"; }
            return HashText;
        }
    }
}