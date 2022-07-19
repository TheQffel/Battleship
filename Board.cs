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
                    GameBoard[i, j] = 0;
                }
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
                    JsonBoard += GameBoard[i, j] + ", ";
                }
                JsonBoard += "},\n";
            }
            
            return "{" + JsonBoard +  "}";
        }
    }
}