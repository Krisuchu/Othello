namespace Othello
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Othello");
            Console.WriteLine();

            GameGrid Game = new GameGrid();
            bool PlayerTurn = true;
            int Turn = 1;

            try
            {
                while(Turn < 60)
                {
                    Game.PrintGameBoard();
                    Console.Write("Enter X-Coordinate: ");
                    int InputColumn = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Enter Y-Coordinate: ");
                    int InputRow = Convert.ToInt32(Console.ReadLine());
                    Game.UpdateGameBoard(InputColumn, InputRow, PlayerTurn);
                    PlayerTurn = !PlayerTurn;
                    Turn++;
                }
            }
            catch(FormatException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}