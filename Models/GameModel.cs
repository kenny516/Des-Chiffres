using Microsoft.AspNetCore.Mvc;

namespace Dechiffre.Models
{

    public class GameModel
    {
        public int TargetNumber { get; set; }
        public List<int> Numbers { get; set; }
        public List<Player> players { get; set; }
        // id du winner pas encore verifier
        public int winner_not_verify { get; set; }
        //valeur a verifier 
        public int value_verify {  get; set; }


        // verification

        public Player VerifyWinner()
        {
            int IDwinner_not_verify = this.winner_not_verify;
            Console.WriteLine("verify nbr " + this.value_verify);

            if (this.players[IDwinner_not_verify].nbrChoice == this.value_verify)
            {
                return this.players[IDwinner_not_verify];
            }
            else
            {
                int otherPlayerID = (IDwinner_not_verify + 1) % this.players.Count;
                return this.players[otherPlayerID];
            }
        }

        // Fonction pour déterminer le gagnant
        public void DetermineWinner(GameModel game)
        {
            Player j1 = game.players[0];
            Player j2 = game.players[1];
            // si un joueur n a pas donne de reponse
            if (game.IsOneChoicePositive(j1, j2))
            {
                game.winner_not_verify = j1.nbrChoice > 0 ? 0 : 1;
            }
            // sinon 
            else if (game.AreBothChoicesPositive(j1, j2))
            {
                int diffj1 = Math.Abs(j1.nbrChoice - game.TargetNumber);
                int diffj2 = Math.Abs(j2.nbrChoice - game.TargetNumber);
                game.winner_not_verify = game.DetermineClosestPlayer(diffj1, diffj2, j1, j2);
            }
            else
            {
                game.winner_not_verify = -1;
            }
        }


        // Vérifier si l'un des joueurs a un choix positif et l'autre négatif
        public bool IsOneChoicePositive(Player j1, Player j2)
        {
            return (j1.nbrChoice > 0 && j2.nbrChoice < 0) || (j1.nbrChoice < 0 && j2.nbrChoice > 0);
        }

        // Vérifier si les deux joueurs ont des choix positifs
        public bool AreBothChoicesPositive(Player j1, Player j2)
        {
            return j1.nbrChoice > 0 && j2.nbrChoice > 0;
        }

        // Déterminer le joueur le plus proche du nombre cible
        public int DetermineClosestPlayer(int diffj1, int diffj2, Player j1, Player j2)
        {
            if (diffj1 < diffj2)
            {
                return 0; // j1 plus proche
            }
            else if (diffj1 > diffj2)
            {
                return 1; // j2 plus proche
            }
            else
            {
                // Les deux joueurs sont à égale distance du nombre cible
                return j1.temps < j2.temps ? 0 : 1;
            }
        }



    }



}
