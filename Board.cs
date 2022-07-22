namespace Battleship
{
    public class Board
    {
        private int[,] GameBoard = new int[10, 10];

        public Board()
        {
            Clear();
        }
        
        public void Clear()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Set(i, j, 0);
                }
            }
        }
        
        public bool PlaceShip(int Length, int[] X, int[] Y)
        {
            if(Length != Y.Length) { return false; }
            if(Length != X.Length) { return false; }
            
            if(ShipPlacementCorrect(Length, X, Y))
            {
                for (int i = 0; i < Length; i++)
                {
                    Set(X[i], Y[i], 1);
                }
                return true;
            }
            return false;
        }
        
        private bool ShipPlacementCorrect(int Length, int[] X, int[] Y)
        {
            for (int i = 0; i < Length; i++)
            {
                if(Get(X[i], Y[i]) == 1) { return false; }
                
                if(X[i] > 0) { if(Get(X[i] - 1, Y[i]) == 1) { return false; } }
                if(X[i] < 9) { if(Get(X[i] + 1, Y[i]) == 1) { return false; } }
                if(Y[i] > 0) { if(Get(X[i], Y[i] - 1) == 1) { return false; } }
                if(Y[i] < 9) { if(Get(X[i], Y[i] + 1) == 1) { return false; } }
                
                if(X[i] > 0 && Y[i] > 0) { if(Get(X[i] - 1, Y[i] - 1) == 1) { return false; } }
                if(X[i] < 9 && Y[i] > 0) { if(Get(X[i] + 1, Y[i] - 1) == 1) { return false; } }
                if(X[i] > 0 && Y[i] < 9) { if(Get(X[i] - 1, Y[i] + 1) == 1) { return false; } }
                if(X[i] < 9 && Y[i] < 9) { if(Get(X[i] + 1, Y[i] + 1) == 1) { return false; } }
            }
            return true;
        }
        
        public int LetterToInt(string Letter)
        {
            return LetterToInt(Letter[0]);
        }
        
        public int LetterToInt(char Letter)
        {
            switch (Letter)
            {
                case 'A': case 'a': case '0': return 0;
                case 'B': case 'b': case '1': return 1;
                case 'C': case 'c': case '2': return 2;
                case 'D': case 'd': case '3': return 3;
                case 'E': case 'e': case '4': return 4;
                case 'F': case 'f': case '5': return 5;
                case 'G': case 'g': case '6': return 6;
                case 'H': case 'h': case '7': return 7;
                case 'I': case 'i': case '8': return 8;
                case 'J': case 'j': case '9': return 9;
                default: return -1;
            }
        }
        
        public int Get(int X, int Y)
        {
            return GameBoard[X, Y];
        }
        
        public void Set(int X, int Y, int State)
        {
            GameBoard[X, Y] = State;
        }
        
        public override string ToString()
        {
            string JsonBoard = "\n";
            
            for (int i = 0; i < 10; i++)
            {
                JsonBoard += "\t[ ";
                for (int j = 0; j < 10; j++)
                {
                    JsonBoard += Get(i, j) + ", ";
                }
                JsonBoard = JsonBoard.Remove(JsonBoard.LastIndexOf(',')) + " ";
                JsonBoard += "],\n";
            }
            JsonBoard = JsonBoard.Remove(JsonBoard.LastIndexOf(',')) + "\n";
            
            return "[" + JsonBoard +  "]";
        }
        
        public void FromString(string JsonData)
        {
            JsonData = JsonData.Replace("[", "").Replace("]", "");
            
            string[] DataSet = JsonData.Split("\t ");

            for (int i = 0; i < 10; i++)
            {
                string[] Data = DataSet[i+1].Split(", ");

                for (int j = 0; j < 10; j++)
                {
                    Set(i, j, int.Parse(Data[j]));
                }
            }
        }
    }
}