using System;
using System.ComponentModel;

namespace OthelloGUI
{
    public class GameState
    {
        public Player[,] GameGrid { get; private set; }
        public Player CurrentPlayer { get; private set; }
        public int BlackPoints { get; private set; }
        public int WhitePoints { get; private set; }
        public int Turns { get; private set; }
        public bool GameOver { get; private set; }

        // Action on valmis delegaatti ja yksinkertaisempi käyttää
        // Action määrittää toiminnon, jota voidaan käyttää kaikkialla ohjelmassa 
        // "tilaamalla" se, jonka jälkeen tilaavassa osassa ohjelmaa voidaan suorittaa toimintoja.
        public event Action<int, int>? MoveMade;
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
            // Siirto on laillinen, mikäli peli ei ole loppunut ja valittu ruutu on tyhjä
            return !GameOver && GameGrid[r, c] == Player.None;
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
            if(!IsLegalMove(r,c))
            {
                return;
            }

            // Suoritetaan siirto ja lisätään tehtyjen siirtojen määrää
            GameGrid[r, c] = CurrentPlayer;
            Turns++;

            // Jos peli päättyy, suoritetaan vain viimeinen siirto
            if(IsGameOver())
            {
                MoveMade?.Invoke(r, c);
                GameEnded?.Invoke(CurrentPlayer);
            }

            // Jos peli ei pääty, suoritetaan siirto ja vaihdetaan aktiivinen pelaaja
            else
            {
                SwitchPlayer();
                MoveMade?.Invoke(r, c);
            }
        }

        // Funktio palauttaa pelilaudan alkuperäiseen tilaansa
        public void Reset()
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
            GameRestarted?.Invoke();
        }
    }
}
