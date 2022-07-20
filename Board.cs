namespace Battleship
{
    public class Board
    {
        private int[,] GameBoard = new int[10, 10];

        public Board()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Set(i, j, 0);
                }
            }
        }
        
        public int LetterToInt(string Letter)
        {
            return LetterToInt(Letter[0]);
        }
        
        public int LetterToInt(char Letter)
        {
            switch (Letter)
            {
                case 'A': case 'a': return 0;
                case 'B': case 'b': return 1;
                case 'C': case 'c': return 2;
                case 'D': case 'd': return 3;
                case 'E': case 'e': return 4;
                case 'F': case 'f': return 5;
                case 'G': case 'g': return 6;
                case 'H': case 'h': return 7;
                case 'I': case 'i': return 8;
                case 'J': case 'j': return 9;
                case 'K': case 'k': return 10;
                case 'L': case 'l': return 11;
                case 'M': case 'm': return 12;
                case 'N': case 'n': return 13;
                case 'O': case 'o': return 14;
                case 'P': case 'p': return 15;
                case 'Q': case 'q': return 16;
                case 'R': case 'r': return 17;
                case 'S': case 's': return 18;
                case 'T': case 't': return 19;
                case 'U': case 'u': return 20;
                case 'V': case 'v': return 21;
                case 'W': case 'w': return 22;
                case 'X': case 'x': return 23;
                case 'Y': case 'y': return 24;
                case 'Z': case 'z': return 25;
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
                JsonBoard += "\t{ ";
                for (int j = 0; j < 10; j++)
                {
                    JsonBoard += Get(i, j) + ", ";
                }
                JsonBoard += "},\n";
            }
            
            return "{" + JsonBoard +  "}";
        }
        
        public void FromString(string JsonData)
        {
            JsonData = JsonData.Replace("{", "").Replace("}", "");
            
            string[] DataSet = JsonData.Split("\t ");

            for (int i = 1; i <= 10; i++)
            {
                string[] Data = DataSet[i].Split(", ");

                for (int j = 0; j < 10; j++)
                {
                    Set(i, j, int.Parse(Data[j]));
                }
            }
        }
    }
}