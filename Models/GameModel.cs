namespace Dechiffre.Models
{
    public class GameModel
    {
        public int TargetNumber { get; set; }
        public List<int> Numbers { get; set; }
        public List<Player> players { get; set; }
        public int winner_not_verify { get; set; }
        public int value_verify {  get; set; }
        public string Winner { get; set; }
    }
    public class Player
    {
        public string name { get; set; }
        public double temps { get; set; }
        public int nbrChoice { get; set; }
    }

}
