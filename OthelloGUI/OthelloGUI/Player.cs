namespace OthelloGUI
{
    public enum PlayerType
    {
        None, Black, White
    }

    public class Player
    {
        public PlayerType Type { get; set; }
        public int Points { get; set; }

        public Player(PlayerType type)
        {
            Type = type;
            Points = 2;
        }
    }
}