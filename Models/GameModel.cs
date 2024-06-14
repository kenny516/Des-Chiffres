namespace Dechiffre.Models
{
    public class GameModel
    {
        public int TargetNumber { get; set; }
        public List<int> Numbers { get; set; }

        public List<Player> Players { get; set; }

        // id du winner pas encore verifier
        public int Winner_not_verify { get; set; }

        //valeur a verifier dans le formulaire de verification
        public int Value_verify { get; set; }
        
        
        // verification

        public Player VerifyWinner()
        {
            int Idwinner_not_verify = Winner_not_verify;
            Console.WriteLine("verify nbr " + Value_verify);

            if (Players[Idwinner_not_verify].nbrChoice == Value_verify)
            {
                Players[Idwinner_not_verify].point++;
                return Players[Idwinner_not_verify];
            }
            int otherPlayerId = (Idwinner_not_verify + 1) % Players.Count;
            Players[otherPlayerId].point++;
            return Players[otherPlayerId];
        }

// Fonction pour déterminer le gagnant temporaire
        public void DetermineWinner(GameModel game)
        {
            Player j1 = game.Players[0];
            Player j2 = game.Players[1];
            // si un joueur n a pas donne de reponse
            if (game.IsOneChoicePositive(j1, j2))
            {
                game.Winner_not_verify = j1.nbrChoice > 0 ? 0 : 1;
            }
            // sinon  il on tous donne
            else if (game.AreBothChoicesPositive(j1, j2))
            {
                int diffj1 = Math.Abs(j1.nbrChoice - game.TargetNumber);
                int diffj2 = Math.Abs(j2.nbrChoice - game.TargetNumber);
                game.Winner_not_verify = game.DetermineClosestPlayer(diffj1, diffj2, j1, j2);
            }
            // sinon aucun a donne
            else
            {
                game.Winner_not_verify = -1;
            }
        }


// Vérifier si l'un des joueurs a un choix positif et l'autre négatif
        private bool IsOneChoicePositive(Player j1, Player j2)
        {
            return (j1.nbrChoice > 0 && j2.nbrChoice < 0) || (j1.nbrChoice < 0 && j2.nbrChoice > 0);
        }

        // Vérifier si les deux joueurs ont des choix positifs (on tous les deux commiter)
        private  bool AreBothChoicesPositive(Player j1, Player j2)
        {
            return j1.nbrChoice > 0 && j2.nbrChoice > 0;
        }

// Déterminer le joueur le plus proche du nombre cible
        private  int DetermineClosestPlayer(int diff1, int diff2, Player j1, Player j2)
        {
            if (diff1 < diff2)
            {
                return 0; // j1 plus proche
            }
            if (diff1 > diff2)
            {
                return 1; // j2 plus proche
            }
            // Les deux joueurs sont à égale distance du nombre cible
            return j1.temps < j2.temps ? 0 : 1;
                
        }
    }
}