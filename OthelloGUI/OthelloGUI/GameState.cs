using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace OthelloGUI
{
    public class GameState
    {
        public PlayerType[,] GameGrid { get; private set; } = new PlayerType[8, 8];
        public PlayerType CurrentPlayer { get; private set; }

        // Tietorakenne sisältää siirron koordinaatit ja niihin liittyvät käännettävät nappulat.
        // Tietorakenteeseen tallennetaan tehtävän siirron koordinaatti ja tähän siirtoon sidonnaisten käännettävien nappien koordinaatit.
        public Dictionary<Tuple<int, int>, List<Tuple<int, int>>> PossibleMoves = new Dictionary<Tuple<int, int>, List<Tuple<int, int>>>();

        // Tietorakenteeseen lisätään ne napit, jotka tullaan kääntämään.
        public List<Tuple<int, int>> Discs = new List<Tuple<int, int>>();

        // Tietorakennetta käytetään peräkkäisten vastustajan nappien tarkasteluun, 
        // kun halutaan selvittää, käännetäänkö vastustajan nappeja tiettyyn suuntaan edetessä.
        // Tietorakenteeseen tallennetut napit lisätään tietorakenteeseen Discs,
        // mikäli nämä napit tullaan kääntämään.
        public List<Tuple<int, int>> Opponents = new List<Tuple<int, int>>();

        public Player Black = new Player(PlayerType.Black);
        public Player White = new Player(PlayerType.White);
        public int Turns { get; private set; }
        public bool GameOver { get; private set; }
        
        // Action on valmis delegaatti ja yksinkertaisempi käyttää.
        // Action määrittää toiminnon, jota voidaan käyttää kaikkialla ohjelmassa 
        // "tilaamalla" se, jonka jälkeen tilaavassa osassa ohjelmaa voidaan suorittaa toimintoja.
        public event Action<int, int>? MoveMade;
        public event Action? GameRestarted;
        public event Action<PlayerType>? GameEnded;
        public event Action<int, int>? UpdateGUI;
        public event Action<int, int>? Coordinates;
        public event Action? TieGame;
        public event Action? ChangePlayerImage;

        // Funktio luo Othello-pelilaudan alkutilanteen
        public GameState()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Luodaan uusi pelilauta, jotta vanhalle pelilaudalle tehdyt siirrot
            // eivät ole edelleen voimassa. Uuden pelilaudan luonti hankkiutuu automaattisesti
            // eroon vanhasta, mikäli sitä ei olla tallennettu mihinkään.
            GameGrid = new PlayerType[8, 8];

            GameGrid[3, 3] = PlayerType.White;
            GameGrid[4, 4] = PlayerType.White;
            GameGrid[3, 4] = PlayerType.Black;
            GameGrid[4, 3] = PlayerType.Black;
            CurrentPlayer = PlayerType.Black;
            Black.Points = 2;
            White.Points = 2;
            Turns = 0;
            GameOver = false;
            FindPossibleMoves();
            UpdateGUI?.Invoke(Black.Points, White.Points);
        }

        // Funktio palauttaa pelilaudan alkuperäiseen tilaansa
        public void Reset()
        {
            PossibleMoves.Clear();
            Opponents.Clear();
            Discs.Clear();
            InitializeGame();
            GameRestarted?.Invoke();
        }

        // Funktio tarkastaa, löytyykö sallittujen siirtojen joukosta klikattu koordinaatti
        private bool IsLegalMove(int r, int c)
        {
            Coordinates?.Invoke(r, c);
            var coordinates = new Tuple<int, int>(r, c);
            if (PossibleMoves.ContainsKey(coordinates))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static private bool IsInsideBoard(int row, int col)
        {
            return row >= 0 && row < 8 && col >= 0 && col < 8;
        }

        // Funktio vaihtaa vuorossa olevan pelaajan
        private void SwitchPlayer()
        {
            PossibleMoves.Clear();
            if(CurrentPlayer == PlayerType.Black)
            {
                CurrentPlayer = PlayerType.White;
                FindPossibleMoves();
            }
            else
            {
                CurrentPlayer = PlayerType.Black;
                FindPossibleMoves();
            }
            if(PossibleMoves.Count == 0)
            {
                ChangePlayerImage?.Invoke();
                SwitchPlayer();
            }
        }

        private void FindPossibleMoves()
        {
            for(int y = 0; y < 8; y++)
            {
                for(int x = 0; x < 8; x++)
                {
                    if (GameGrid[x, y] == PlayerType.None)
                    {
                        ScanDirections(x, y);
                        if (Discs.Count > 0)
                        {
                            var coordinates = new Tuple<int, int>(x, y);
                            // Tärkeä määritellä "new", koska ilman tätä sanakirjaan tallennetaan kopio
                            // listasta. Kun lista tyyhjennetään seuraavalla rivillä, lähtevät sen tiedot myös sanakirjasta.
                            // Ei siis voi tehdä näin: PossibleMoves[coordinates] = Discs!!!
                            PossibleMoves[coordinates] = new List<Tuple<int, int>>(Discs);
                            Discs.Clear();
                        }
                    }
                }
            }
        }

        // Funktio käy läpi kaikki mahdolliset suunnat klikatusta ruudusta katsottuna,
        // jotta DiscNumbers-funktio voi tallentaa tiedon käännettävistä pelinappuloista.
        private void ScanDirections(int row, int col)
        {
            for (int r = -1; r <= 1; r++)
            {
                for (int c = -1; c <= 1; c++)
                {
                    // Poisluetaan tarkasteltavasta 3x3 ruudukosta se ruutu, jossa asettamamme nappi on
                    if (r == 0 && c == 0)
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

            while (IsInsideBoard(r, c) && GameGrid[r, c] != PlayerType.None)
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

        // Funktio tarkastaa, onko peli syytä lopettaa eri tilanteissa
        private bool IsGameOver()
        {
            if(Black.Points == 0 || White.Points == 0 || Turns == 60)
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
            Turns++;

            // Kasvatetaan nykyisen pelaajan pistesaldoa yhdellä tämän asettaessa uuden nappulan laudalle.
            if (CurrentPlayer == PlayerType.Black)
            {
                Black.Points++;
            }
            else
            {
                White.Points++;
            }

            // Etsitään sanakirjasta avainta (koordinaattia) vastaava lista kyseisen
            // koordinaatin käännettävistä napuloista ja käännetään nappulat.
            var key = new Tuple<int, int>(r, c);
            var DiscList = PossibleMoves[key];
            foreach(var disc in DiscList)
            {
                GameGrid[disc.Item1, disc.Item2] = CurrentPlayer;
                if(CurrentPlayer == PlayerType.Black)
                {
                    Black.Points++;
                    White.Points--;
                }
                else
                {
                    Black.Points--;
                    White.Points++;
                }
            }

            // Tehdään siirto graafisen käyttöliittymän puolella sekä käännetään
            // käännettävät pelinappulat. Lisäksi päivitetään pelin pistetilanne.
            MoveMade?.Invoke(r, c);
            foreach (var disc in DiscList)
            {
                MoveMade?.Invoke(disc.Item1, disc.Item2);
            }
            UpdateGUI?.Invoke(Black.Points, White.Points);

            // Jos peli päättyy, siirrytään pelin lopetusruutuun. Toisessa tilanteessa
            // vaihdetaan aktiivista pelaajaa.
            if (IsGameOver())
            {
                if(Black.Points == White.Points)
                {
                    TieGame?.Invoke();
                }
                else if(Black.Points > White.Points)
                {
                    GameEnded?.Invoke(PlayerType.Black);
                }
                else
                {
                    GameEnded?.Invoke(PlayerType.White);
                }
            }
            else
            {
                SwitchPlayer();
            }
        }
    }
}
