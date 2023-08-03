using System;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Data.Common;

class GameGrid
{
    public void PrintGameBoard()
    {
        Console.WriteLine("  1 2 3 4 5 6 7 8");
        Console.WriteLine();

        for (int i = 1; i < 9; i++)
        {
            for (int x = 1; x < 9; x++)
            {
                if (x < 2)
                {
                    Console.Write(i + " ");
                }
                Console.Write(Board[i, x]);
                if (x < 8)
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

    public bool IsEmpty(int InputColumn, int InputRow)
    {
        if(Board[InputRow, InputColumn] == 0)
        {
            return true;
        }
        else
        { 
            return false;
        }
    }

    public bool IsValidSpot(int Column, int Row, bool isBlack)
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

        Dictionary<string, int> Neighbours = new Dictionary<string, int>();

        Neighbours.Add("North", Board[Row - 1, Column]);
        Neighbours.Add("NorthEast", Board[Row - 1, Column + 1]);
        Neighbours.Add("East", Board[Row, Column + 1]);
        Neighbours.Add("SouthEast", Board[Row + 1, Column + 1]);
        Neighbours.Add("South", Board[Row + 1, Column]);
        Neighbours.Add("SouthWest", Board[Row + 1, Column - 1]);
        Neighbours.Add("West", Board[Row, Column - 1]);
        Neighbours.Add("NorthWest", Board[Row - 1, Column - 1]);

        foreach (var color in Neighbours)
        {
            if (color.Value == OpponentColor)
            {
                if (color.Key.Contains("North"))
                {
                    if (color.Key.Contains("East"))
                    {
                        int i = 1;
                        while (Board[Row - 1 - i, Column + 1 + i] == OpponentColor)
                        {
                            i++;
                        }
                        if (Board[Row - 1 - i, Column + 1 + i] == DiscColor)
                        {
                            return true;
                        }
                    }
                    else if (color.Key.Contains("West"))
                    {
                        int i = 1;
                        while (Board[Row - 1 - i, Column - 1 - i] == OpponentColor)
                        {
                            i++;
                        }
                        if (Board[Row - 1 - i, Column - 1 - i] == DiscColor)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        int i = 1;
                        while (Board[Row - 1 - i, Column] == OpponentColor)
                        {
                            i++;
                        }
                        if (Board[Row - 1 - i, Column] == DiscColor)
                        {
                            return true;
                        }
                    }
                }
                if (color.Key.Contains("South"))
                {
                    if (color.Key.Contains("East"))
                    {
                        int i = 1;
                        while (Board[Row + 1 + i, Column + 1 + i] == OpponentColor)
                        {
                            i++;
                        }
                        if (Board[Row + 1 + i, Column + 1 + i] == DiscColor)
                        {
                            return true;
                        }
                    }
                    else if (color.Key.Contains("West"))
                    {
                        int i = 1;
                        while (Board[Row + 1 + i, Column - 1 - i] == OpponentColor)
                        {
                            i++;
                        }
                        if (Board[Row + 1 + i, Column - 1 - i] == DiscColor)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        int i = 1;
                        while (Board[Row + 1 + i, Column] == OpponentColor)
                        {
                            i++;
                        }
                        if (Board[Row + 1 + i, Column] == DiscColor)
                        {
                            return true;
                        }
                    }
                }
                if (color.Key == "East")
                {
                    int i = 1;
                    while (Board[Row, Column + 1 + i] == OpponentColor)
                    {
                        i++;
                    }
                    if (Board[Row, Column + 1 + i] == DiscColor)
                    {
                        return true;
                    }
                }
                if (color.Key == "West")
                {
                    int i = 1;
                    while (Board[Row, Column - 1 - i] == OpponentColor)
                    {
                        i++;
                    }
                    if (Board[Row, Column - 1 - i] == DiscColor)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void UpdateGameBoard(int Column, int Row, bool isBlack)
    {
        if (isBlack)
        {
            Board[Row, Column] = 1;
            TurnDisks(Row, Column, isBlack);
        }
        else
        {
            Board[Row, Column] = 2;
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

        Dictionary<string, int> Neighbours = new Dictionary<string, int>();

            Neighbours.Add("North", Board[Row - 1, Column]);
            Neighbours.Add("NorthEast", Board[Row - 1, Column + 1]);
            Neighbours.Add("East", Board[Row, Column + 1]);
            Neighbours.Add("SouthEast", Board[Row + 1, Column + 1]);
            Neighbours.Add("South", Board[Row + 1, Column]);
            Neighbours.Add("SouthWest", Board[Row + 1, Column - 1]);
            Neighbours.Add("West", Board[Row, Column - 1]);
            Neighbours.Add("NorthWest", Board[Row - 1, Column - 1]);

        foreach (var color in Neighbours)
        {
            if (color.Value == OpponentColor)
            {
                if (color.Key.Contains("North"))
                {
                    if (color.Key.Contains("East"))
                    {
                        int i = 1;
                        while (Board[Row - 1 - i, Column + 1 + i] == OpponentColor)
                        {
                            i++;
                        }
                        if (Board[Row - 1 - i, Column + 1 + i] == DiscColor)
                        {
                            for (int x = i; x > 0; x--)
                            {
                                Board[Row - x, Column + x] = DiscColor;
                            }
                        }
                    }
                    else if (color.Key.Contains("West"))
                    {
                        int i = 1;
                        while (Board[Row - 1 - i, Column - 1 - i] == OpponentColor)
                        {
                            i++;
                        }
                        if (Board[Row - 1 - i, Column - 1 - i] == DiscColor)
                        {
                            for (int x = i; x > 0; x--)
                            {
                                Board[Row - x, Column - x] = DiscColor;
                            }
                        }
                    }
                    else
                    {
                        int i = 1;
                        while (Board[Row - 1 - i, Column] == OpponentColor)
                        {
                            i++;
                        }
                        if (Board[Row - 1 - i, Column] == DiscColor)
                        {
                            for (int x = i; x > 0; x--)
                            {
                                Board[Row - x, Column] = DiscColor;
                            }
                        }
                    }
                }
                if (color.Key.Contains("South"))
                {
                    if (color.Key.Contains("East"))
                    {
                        int i = 1;
                        while (Board[Row + 1 + i, Column + 1 + i] == OpponentColor)
                        {
                            i++;
                        }
                        if (Board[Row + 1 + i, Column + 1 + i] == DiscColor)
                        {
                            for (int x = i; x > 0; x--)
                            {
                                Board[Row + x, Column + x] = DiscColor;
                            }
                        }
                    }
                    else if (color.Key.Contains("West"))
                    {
                        int i = 1;
                        while (Board[Row + 1 + i, Column - 1 - i] == OpponentColor)
                        {
                            i++;
                        }
                        if (Board[Row + 1 + i, Column - 1 - i] == DiscColor)
                        {
                            for (int x = i; x > 0; x--)
                            {
                                Board[Row + x, Column - x] = DiscColor;
                            }
                        }
                    }
                    else
                    {
                        int i = 1;
                        while (Board[Row + 1 + i, Column] == OpponentColor)
                        {
                            i++;
                        }
                        if (Board[Row + 1 + i, Column] == DiscColor)
                        {
                            for (int x = i; x > 0; x--)
                            {
                                Board[Row + x, Column] = DiscColor;
                            }
                        }
                    }
                }
                if (color.Key == "East")
                {
                    int i = 1;
                    while (Board[Row, Column + 1 + i] == OpponentColor)
                    {
                        i++;
                    }
                    if (Board[Row, Column + 1 + i] == DiscColor)
                    {
                        for (int x = i; x > 0; x--)
                        {
                            Board[Row, Column + x] = DiscColor;
                        }
                    }
                }
                if (color.Key == "West")
                {
                    int i = 1;
                    while (Board[Row, Column - 1 - i] == OpponentColor)
                    {
                        i++;
                    }
                    if (Board[Row, Column - 1 - i] == DiscColor)
                    {
                        for (int x = i; x > 0; x--)
                        {
                            Board[Row, Column - x] = DiscColor;
                        }
                    }
                }
            }
        }
    }

 

    private int[,] Board = {
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 2, 1, 0, 0, 0, 0},
                {0, 0, 0, 0, 1, 2, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},};
}
