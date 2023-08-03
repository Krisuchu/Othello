namespace Othello
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Othello");
            Console.WriteLine();

            int InputRow = 4;
            int InputCol = 4;

            GameGrid Game = new GameGrid();
            bool PlayerTurn = true;
            int Turn = 1;

            while(Game.PossibleMoves(InputCol, InputRow, PlayerTurn))
            {
                Game.PrintGameBoard();
                
                while(!Game.IsEmpty(InputCol, InputRow) || !Game.IsValidSpot(InputCol, InputRow, PlayerTurn))
                {
                    Console.Write("Enter X-Coordinate: ");
                    InputCol = Convert.ToInt32(Console.ReadLine());
                    while (InputCol < 1 || InputCol > 8)
                    {
                        Console.Write("Enter X-Coordinate: ");
                        InputCol = Convert.ToInt32(Console.ReadLine());
                    }

                    Console.Write("Enter Y-Coordinate: ");
                    InputRow = Convert.ToInt32(Console.ReadLine());
                    while (InputRow < 1 || InputRow > 8)
                    {
                        Console.Write("Enter Y-Coordinate: ");
                        InputRow = Convert.ToInt32(Console.ReadLine());
                    }

                }

                Console.Write('\n');
                Game.UpdateGameBoard(InputCol, InputRow, PlayerTurn);
                PlayerTurn = !PlayerTurn;
                Turn++;
            }
            Game.PrintGameBoard();
            Console.WriteLine("Game Over");
            Console.ReadLine();
        }
    }
}