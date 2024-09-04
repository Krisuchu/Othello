using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace OthelloGUI
{
    public class GameState
    {
        public Player[,] GameGrid { get; private set; }
        public Player CurrentPlayer { get; private set; }
        public List<Tuple<int, int>> Discs = new List<Tuple<int, int>>();
        public List<Tuple<int, int>> Opponents = new List<Tuple<int, int>>();
        public int BlackPoints { get; private set; }
        public int WhitePoints { get; private set; }
        public int Turns { get; private set; }
        public bool GameOver { get; private set; }

        // Action on valmis delegaatti ja yksinkertaisempi käyttää
        // Action määrittää toiminnon, jota voidaan käyttää kaikkialla ohjelmassa 
        // "tilaamalla" se, jonka jälkeen tilaavassa osassa ohjelmaa voidaan suorittaa toimintoja.
        public event Action<int, int>? MoveMade;
        public event Action<int, int>? UpdatePoints;
        public event Action<int, int>? Coordinates;
        public event Action? GameRestarted;
        public event Action<Player>? GameEnded;

        // Funktio luo Othello-pelilaudan alkutilanteen
        public GameState()
        {
            GameGrid = new Player[8, 8];
            GameGrid[3, 3] = Player.White;
            GameGrid[4, 4] = Player.White;
            GameGrid[3, 4] = Player.Black;
            GameGrid[4, 3] = Player.Black;
            BlackPoints = 2;
            WhitePoints = 2;
            CurrentPlayer = Player.Black;
            Turns = 0;
            GameOver = false;
        }

        // Funktio tarkastaa, onko tehtävä siirto laillinen
        private bool IsLegalMove(int r, int c)
        {
            if (GameOver && GameGrid[r, c] != Player.None)
            {
                return false;
            }
            else
            {
                TurnedDiscs(r, c);

                // Siirto on laillinen, jos tämä siirto aiheuttaa vastustajan nappien kääntymisen.
                // Disc.Count laskee käännettävien nappuloiden määrän.
                return Discs.Count > 0;
            }
        }

        // Funktio käy läpi kaikki mahdolliset suunnat klikatusta ruudusta katsottuna,
        // jotta DiscNumbers-funktio voi tallentaa tiedon käännettävistä pelinappuloista.
        private void TurnedDiscs(int row, int col)
        {
            for(int r = -1; r <= 1; r++)
            {
                for(int c = -1; c <= 1; c++)
                {
                    // Poisluetaan tarkasteltavasta 3x3 ruudukosta se ruutu, jossa asettamamme nappi on
                    if(r == 0 && c == 0)
                    {
                        continue;
                    }
                    // Tarkastellaan pelilaudan tilannetta lähtien pisteestä [row, col] suuntaan (r, c),
                    // jossa r ja c ilmoittavat etenemissuunnan klikatusta ruudusta katsottuna.
                    DiscNumbers(row, col, r, c);
                }
            }
        }

        // Funktio käy läpi klikatun ruudun viereisiä ruutuja edettäessä suuntaan (x, y)
        // ja tallentaa ruutujen koordinaatit talteen myöhempää kääntämistä varten.
        private void DiscNumbers(int row, int col, int x, int y)
        {
            // Muodostetaan koordinaatti seuraavalle tarkastelupisteelle, kun edetään suuntaan (x, y).
            int r = row + x;
            int c = col + y;
            bool areFlippedTokens = false;
            
            while (IsInsideBoard(r, c) && GameGrid[r, c] != Player.None)
            {
                // Onko seuraava nappula edelleen vastustajan värinen liikuttaessa kyseiseen suuntaan
                if (GameGrid[r, c] != CurrentPlayer)
                {
                    Opponents.Add(new Tuple<int, int>(r, c));
                    r += x;
                    c += y;
                }
                // Mikäli seuraava nappula on oman värinen, lopetetaan tarkastelu tähän suuntaan
                else
                {
                    if (Opponents.Count > 0)
                    {
                        areFlippedTokens = true;
                    }
                    break;
                }
            }

            if (areFlippedTokens)
            {
                // Käydään läpi kaikki käännettävät nappulat ja lisätään ne käännettävien
                // nappuloiden listaan "Discs"
                foreach (var opponent in Opponents)
                {
                    Discs.Add(new Tuple<int, int>(opponent.Item1, opponent.Item2));
                }
            }
            Opponents.Clear();
        }

        static private bool IsInsideBoard(int row, int col)
        {
            return row >= 0 && row < 8 && col >= 0 && col < 8;
        }


        // Funktio vaihtaa vuorossa olevan pelaajan
        private void SwitchPlayer()
        {
            if(CurrentPlayer == Player.Black)
            {
                CurrentPlayer = Player.White;
            }
            else
            {
                CurrentPlayer = Player.Black;
            }
        }

        // Funktio tarkastaa, onko peli syytä lopettaa eri tilanteissa
        private bool IsGameOver()
        {
            if(BlackPoints == 0 || WhitePoints == 0 || Turns == 60)
            {
                return true;
            }
            return false;
        }

        // Funktio tekee siirron pelilaudalla, jos siirto on laillinen
        public void MakeMove(int r, int c)
        {
            if(!IsLegalMove(r, c))
            {
                return;
            }

            // Suoritetaan siirto pelin logiikan puolella ja lisätään tehtyjen siirtojen määrää
            GameGrid[r, c] = CurrentPlayer;
            Coordinates?.Invoke(r, c);
            foreach(var disc in Discs)
            {
                GameGrid[disc.Item1, disc.Item2] = CurrentPlayer;
                if(CurrentPlayer == Player.Black)
                {
                    BlackPoints++;
                    WhitePoints--;
                }
                else
                {
                    BlackPoints--;
                    WhitePoints++;
                }
            }
            Turns++;

            // Jos peli päättyy, suoritetaan vain viimeinen siirto GUI:n puolella
            if (IsGameOver())
            {
                MoveMade?.Invoke(r, c);
                foreach (var disc in Discs)
                {
                    MoveMade?.Invoke(disc.Item1, disc.Item2);
                }
                GameEnded?.Invoke(CurrentPlayer);
            }

            // Jos peli ei pääty, suoritetaan siirto ja vaihdetaan aktiivinen pelaaja GUI:n puolella
            else
            {
                SwitchPlayer();
                MoveMade?.Invoke(r, c);
                
                foreach (var disc in Discs)
                {
                    MoveMade?.Invoke(disc.Item1, disc.Item2);
                }
            }
            UpdatePoints?.Invoke(BlackPoints, WhitePoints);
            Discs.Clear();
        }

        // Funktio palauttaa pelilaudan alkuperäiseen tilaansa
        public void Reset()
        {
            Opponents.Clear();
            Discs.Clear();
            GameGrid = new Player[8, 8];
            GameGrid[3, 3] = Player.White;
            GameGrid[4, 4] = Player.White;
            GameGrid[3, 4] = Player.Black;
            GameGrid[4, 3] = Player.Black;
            BlackPoints = 2;
            WhitePoints = 2;
            CurrentPlayer = Player.Black;
            Turns = 0;
            GameOver = false;
            GameRestarted?.Invoke();
            UpdatePoints?.Invoke(BlackPoints, WhitePoints);
            

        }
    }
}
