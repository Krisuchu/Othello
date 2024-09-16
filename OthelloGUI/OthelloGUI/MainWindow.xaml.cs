using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OthelloGUI
{
    public partial class MainWindow : Window
    {
        private readonly Dictionary<PlayerType, ImageSource> imageSources = new()
        {
            { PlayerType.Black, new BitmapImage(new Uri("pack://application:,,,/Assets/Othello chip black.png")) },
            { PlayerType.White, new BitmapImage(new Uri("pack://application:,,,/Assets/Othello chip white.png")) }
        };

        readonly ImageSource Tie = new BitmapImage(new Uri("pack://application:,,,/Assets/Othello chip tie.png"));

        // Luodaan 8x8 ruudukko kuvia, jotta pelilaudan tilaa voidaan päivittää visuaalisesti joko
        // tyhjäksi, valkoiseksi tai mustaksi.
        private readonly Image[,] imageControls = new Image[8, 8];
        private readonly GameState gameState = new GameState();

        public MainWindow()
        {
            InitializeComponent();
            SetupGameGrid();

            gameState.GameRestarted += OnGameRestarted;
            gameState.MoveMade += OnMoveMade;
            gameState.UpdateGUI += UpdateGUI;
            gameState.GameEnded += OnGameEnded;
            gameState.Coordinates += UpdateCoordinates;
            gameState.TieGame += OnTieGame;
            gameState.ChangePlayerImage += ChangeImage;
        }

        // Päivittää pelilaudan tilanteen tehdyn liikkeen perusteella.
        private void OnMoveMade(int row, int col)
        {
            PlayerType player = gameState.GameGrid[row, col];
            imageControls[row, col].Source = imageSources[player];
            RestartButton.IsEnabled = true;
        }

        private void UpdateGUI(int bPoints, int wPoints)
        {
            BlackPoints.Content = bPoints;
            WhitePoints.Content = wPoints;
            if(gameState.CurrentPlayer == PlayerType.Black)
            {
                PlayerImage.Source = imageSources[PlayerType.White];
            }
            else
            {
                PlayerImage.Source = imageSources[PlayerType.Black];
            }

        }

        // Päivittää käyttöliittymän pisteet ja nykyisen pelaajan vastaamaan pelin nykyistä tilaa
        private void UpdateCoordinates(int bPoints, int wPoints)
        {
            xCoordinate.Content = wPoints + 1;
            yCoordinate.Content = bPoints + 1;
        }

        // Näyttää pelin loppuruudun ja sen sisältämät tiedot pelin päättymisestä.
        private void OnGameEnded(PlayerType player)
        {
            WinnerScreen.Visibility = Visibility.Visible;
            WinnerImage.Visibility = Visibility.Visible;
            WinnerText.Visibility = Visibility.Visible;
            WinnerImage.Source = imageSources[player];
        }

        private void OnTieGame()
        {
            WinnerScreen.Visibility = Visibility.Visible;
            WinnerImage.Visibility = Visibility.Visible;
            TieText.Visibility = Visibility.Visible;
            WinnerImage.Source = Tie;
        }

        private void ChangeImage()
        {
            if (gameState.CurrentPlayer == PlayerType.Black)
            {
                PlayerImage.Source = imageSources[PlayerType.White];
            }
            else
            {
                PlayerImage.Source = imageSources[PlayerType.Black];
            }
        }

        // Palauttaa pelilaudan visuaaliset elementit pelin alkutilaan.
        private void OnGameRestarted()
        {
            WinnerScreen.Visibility = Visibility.Hidden;
            WinnerImage.Visibility = Visibility.Hidden;
            WinnerText.Visibility = Visibility.Hidden;
            TieText.Visibility = Visibility.Hidden;
            RestartButton.IsEnabled = false;

            for (int row = 0; row < 8; row++)
            {
                for(int col = 0; col < 8; col++)
                {
                    imageControls[row, col].Source = null;
                }
            }
            PlayerImage.Source = imageSources[gameState.CurrentPlayer];
            SetupStartPosition();
        }

        // Asettaa pelilaudan jokaiseen ruutuun kuvan, joko valkoinen, musta tai tyhjä kuva.
        private void SetupGameGrid()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Image imageControl = new Image();
                    GameGrid.Children.Add(imageControl);
                    imageControls[row, col] = imageControl;
                }
            }
            SetupStartPosition();
            RestartButton.IsEnabled = false;
        }

        private void SetupStartPosition()
        {
            imageControls[3, 3].Source = imageSources[PlayerType.White];
            imageControls[4, 4].Source = imageSources[PlayerType.White];
            imageControls[3, 4].Source = imageSources[PlayerType.Black];
            imageControls[4, 3].Source = imageSources[PlayerType.Black];
        }

        // Etsii pelilaudalla klikatun kohdan ja määrittää tämän perusteella x- ja y-koordinaatit
        private void GameGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double squareSize = GameGrid.Width / 8;
            Point clickPosition = e.GetPosition(GameGrid);
            int row = (int)(clickPosition.Y / squareSize);
            int col = (int)(clickPosition.X / squareSize);
            gameState.MakeMove(row, col);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            gameState.Reset();
        }
    }
}
