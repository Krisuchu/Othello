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
        private readonly Dictionary<Player, ImageSource> imageSources = new()
        {
            { Player.Black, new BitmapImage(new Uri("pack://application:,,,/Assets/Othello chip black.png")) },
            { Player.White, new BitmapImage(new Uri("pack://application:,,,/Assets/Othello chip white.png")) }
        };

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
            gameState.GameEnded += OnGameEnded;
        }

        private void OnMoveMade(int row, int col)
        {
            Player player = gameState.GameGrid[row, col];
            imageControls[row, col].Source = imageSources[player];
            PlayerImage.Source = imageSources[gameState.CurrentPlayer];
        }

        private void OnGameEnded(Player player)
        {

        }

        private void OnGameRestarted()
        {

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
            imageControls[3,3].Source = imageSources[Player.White];
            imageControls[4, 4].Source = imageSources[Player.White];
            imageControls[3, 4].Source = imageSources[Player.Black];
            imageControls[4, 3].Source = imageSources[Player.Black];
        }

        private void GameGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double squareSize = GameGrid.Width / 8;
            Point clickPosition = e.GetPosition(GameGrid);
            int row = (int)(clickPosition.Y / squareSize);
            int col = (int)(clickPosition.X / squareSize);
            gameState.MakeMove(row, col);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
