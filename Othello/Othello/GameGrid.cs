using System;
using System.Security.Cryptography.X509Certificates;

class GameGrid
{
    public void PrintGameBoard()
    {
        Console.WriteLine("  0 1 2 3 4 5 6 7");
        Console.WriteLine();

        for (int i = 0; i < 8; i++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (x < 1)
                {
                    Console.Write(i + " ");
                }
                Console.Write(Board[i, x]);
                if (x < 7)
                {
                    Console.Write("|");
                }
                else
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine("  ----------------");
        }
        Console.WriteLine();
    }

    public void UpdateGameBoard(int Column, int Row, bool isBlack)
    {
        if (isBlack)
        {
            Board[Row, Column] = 1;
            Console.WriteLine("On musta: " + isBlack);
            TurnDisks(Row, Column, isBlack);
        }
        else
        {
            Board[Row, Column] = 2;
            Console.WriteLine("On musta: " + isBlack);
            TurnDisks(Row, Column, isBlack);
        }
    }

    public void TurnDisks(int Row, int Column, bool isBlack)
    {
        int DiscColor;
        int OpponentColor;

        if (isBlack)
        {
            DiscColor = 1;
            OpponentColor = 2;
        }
        else
        {
            DiscColor = 2;
            OpponentColor = 1;
        }

        for (int i = 0; i < 8; i++)
        {
            int z = 1;
            while (Board[Row, Column - z] == OpponentColor && Column - z > 0)
            {
                z++;
                if (Board[Row, Column - z] == OpponentColor)
                {
                    continue;
                }
                else if (Board[Row, Column - z] == DiscColor)
                {
                    for (int TurnedDiscs = 0; TurnedDiscs < z; z--)
                    {
                        Board[Row, Column - z + 1] = DiscColor;
                        z--;
                    }
                }
                else
                {
                    break;
                }
            }
                
            while (Board[Row + z, Column] == OpponentColor && Row + z < 7)
            {
                z++;
                if (Board[Row + z, Column] == OpponentColor)
                {
                    continue;
                }
                else if (Board[Row + z, Column] == DiscColor)
                {
                    for (int d = 0; d < z; z--)
                    {
                        Board[Row + z - 1, Column] = DiscColor;
                        z--;
                    }
                }
                else
                {
                    break;
                }
            }

            while (Board[Row - z, Column] == OpponentColor && Row - z > 0)
            {
                z++;
                if (Board[Row - z, Column] == OpponentColor)
                {
                    continue;
                }
                else if (Board[Row - z, Column] == DiscColor)
                {
                    for (int d = 0; d < z; z--)
                    {
                        Board[Row - z + 1, Column] = DiscColor;
                        z--;
                    }
                }
                else
                {
                    break;
                }
            }
            while (Board[Row - z, Column + z] == OpponentColor && Column + z < 7 && Row - z > 0)
            {
                z++;
                if (Board[Row - z, Column + z] == OpponentColor)
                {
                    continue;
                }
                else if (Board[Row - z, Column + z] == DiscColor)
                {
                    for (int d = 0; d < z; z--)
                    {
                        Board[Row - z + 1, Column + z - 1] = DiscColor;
                        z--;
                    }
                }
                else
                {
                    break;
                }
            }
            while (Board[Row - z, Column - z] == OpponentColor && Column - z > 0 && Row - z > 0)
            {
                z++;
                if (Board[Row - z, Column - z] == OpponentColor)
                {
                    continue;
                }
                else if (Board[Row - z, Column - z] == DiscColor)
                {
                    for (int d = 0; d < z; z--)
                    {
                        Board[Row - z + 1, Column - z + 1] = DiscColor;
                        z--;
                    }
                }
                else
                {
                    break;
                }
            }
            while (Board[Row + z, Column + z] == OpponentColor && Row + z < 7 && Column + z < 7)
            {
                z++;
                if (Board[Row + z, Column + z] == OpponentColor)
                {
                    continue;
                }
                else if (Board[Row + z, Column + z] == DiscColor)
                {
                    for (int d = 0; d < z; z--)
                    {
                        Board[Row + z - 1, Column + z - 1] = DiscColor;
                        z--;
                    }
                }
                else
                {
                    break;
                }
            }

            while (Board[Row, Column + z] == OpponentColor && Column + z < 7)
            {
                z++;
                if (Board[Row, Column + z] == OpponentColor)
                {
                    continue;
                }
                else if (Board[Row, Column + z] == DiscColor)
                {
                    for (int d = 0; d < z; z--)
                    {
                        Board[Row, Column + z - 1] = DiscColor;
                        z--;
                    }
                }
                else
                {
                    break;
                }
            }
            while (Board[Row + z, Column - z] == OpponentColor && Row + z < 7 && Column - z > 0)
            {
                z++;
                if (Board[Row + z, Column - z] == OpponentColor)
                {
                    continue;
                }
                else if (Board[Row + z, Column - z] == DiscColor)
                {
                    for (int d = 0; d < z; z--)
                    {
                        Board[Row + z - 1, Column - z + 1] = DiscColor;
                        z--;
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }

    private int[,] Board = {
                {0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 2, 1, 0, 0, 0},
                {0, 0, 0, 1, 2, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0}};
}
