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
        
        public void SyncWithDb() { Save(); }

        public Board PlayerBoard(string Player)
        {
            if(Player == PlayerA) { return BoardA; }
            if(Player == PlayerB) { return BoardB; }
            return new Board();
        }
        public Board OpponentBoard(string Player)
        {
            if(Player == PlayerA) { return BoardB; }
            if(Player == PlayerB) { return BoardA; }
            return new Board();
        }
        
        private bool IsPLayerMove(string Player)
        {
            if(Player == PlayerA && Turn) { return true; }
            if(Player == PlayerB && !Turn) { return true; }
            return false;
        }
        
        private bool AreShipsPlaced(string Player)
        {
            Board BoardToCheck = PlayerBoard(Player);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if(BoardToCheck.Get(i, j) > 0) { return true; }
                }
            }
            return false;
        }

        public bool PlaceShips(string Player, string[] Ships, out string? Error)
        {
            Board Board = PlayerBoard(Player);
            
            Error = null;
            
            if(!AreShipsPlaced(Player))
            {
                // 0 = amount correct, 1 = ship with length 1 exists, 2 = ship with length 2 exists,
                // 3 = ship with length 3 exists, 4 = ship with length 4 exists, 5 = ship ... etc.
                bool[] ShipsAmountCorrect = { false, false, false, false, false, false, false, false };
                ShipsAmountCorrect[0] = Ships.Length == 5;
                
                if(ShipsAmountCorrect[0])
                {
                    for (int i = 0; i < Ships.Length; i++)
                    {
                        if(Ships[i].Length is 4 or 6 or 8 or 10 or 12) // 2,3,4,5,6 length ship
                        {
                            int Length = Ships[i].Length/2;
                            int[] X = new int[Length];
                            int[] Y = new int[Length];
                            
                            for (int j = 0; j < Length; j++)
                            {
                                X[j] = Board.LetterToInt(Ships[i][j*2]);
                                Y[j] = Board.LetterToInt(Ships[i][j*2+1]);
                            }
                            
                            if(!Board.PlaceShip(Length, X, Y)) { break; }
                            ShipsAmountCorrect[Length] = true;
                        }
                        else
                        {
                            Error = "Ship " + (i+1) + " have invalid length!";
                            return false;
                        }
                    }
                    
                    if(ShipsAmountCorrect[0] && ShipsAmountCorrect[2] && ShipsAmountCorrect[3] && ShipsAmountCorrect[4] && ShipsAmountCorrect[5] && ShipsAmountCorrect[6])
                    {
                        return true;
                    }
                    Board.Clear();
                    
                    Error = "Missing one ore more ship!";
                    return false;
                }
                Error = "Invalid amount of ships!";
                return false;
            }
            Error = "Ships already placed!";
            return false;
        }
        
        public bool Move(string Player, char PosX, char PosY, out string? Error)
        {
            Board Board = OpponentBoard(Player);
            
            int X = Board.LetterToInt(PosX);
            int Y = Board.LetterToInt(PosY);
            
            Error = null;
            
            if(Winner() == null)
            {
                if(IsPLayerMove(Player))
                {
                    if(AreShipsPlaced(PlayerA) && AreShipsPlaced(PlayerB))
                    {
                        if(Board.Get(X, Y) <= 1)
                        {
                            if(Board.Get(X, Y) == 0)
                            {
                                Board.Set(X, Y, 2);
                                Turn = !Turn;
                            }
                            else
                            {
                                Board.Set(X, Y, 3);
                                if(Winner() != null)
                                {
                                    Turn = !Turn;
                                }
                            }
                            return true;
                        }
                        Error = "Already shot!";
                        return false;
                    }
                    Error = "Ships not placed!";
                    return false;
                }
                Error = "Not your move!";
                return false;
            }
            Error = "Game is finished!";
            return false;
        }
        
        private string? Winner()
        {
            string? Winner = null;
            bool PlacedShipsA = false;
            bool PlacedShipsB = false;
            bool NoShipsLeftA = true;
            bool NoShipsLeftB = true;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if(BoardA.Get(i, j) != 0) { PlacedShipsA = true; }
                    if(BoardB.Get(i, j) != 0) { PlacedShipsB = true; }
                    if(BoardA.Get(i, j) == 1) { NoShipsLeftA = false; }
                    if(BoardB.Get(i, j) == 1) { NoShipsLeftB = false; }
                }
            }
            
            if(PlacedShipsA && PlacedShipsB)
            {
                if(NoShipsLeftA != NoShipsLeftB)
                {
                    if(NoShipsLeftA) { Winner = PlayerB; }
                    if(NoShipsLeftB) { Winner = PlayerA; }
                }
            }
            
            return Winner;
        }
        
        public override string ToString()
        {
            string WinnerName = Winner() ?? "";
            if(WinnerName.Length > 0)
            {WinnerName = $", \"Winner\": \"{WinnerName}\""; }
            return "{" + $" \"GameId\": {GameId}, \"PlayerA\": \"{PlayerA}\", \"PlayerB\": \"{PlayerB}\", \"Turn\": {Turn.ToString().ToLower()}" + WinnerName + " }";
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
                Console.Log(Console.LogType.Error, "Game " + GameId + " is missing or corrupted!");
            }
        }
        
        private void Save()
        {
            Database.Set("Battleship_Games", $"PlayerA = '{PlayerA}', PlayerB = '{PlayerB}', Turn = '{(Turn ? 1 : 0)}', BoardA = '{BoardA}', BoardB = '{BoardB}'", $"GameId = '{GameId}'");
        }
    }
}