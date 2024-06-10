using Dechiffre.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DesChiffres.Controllers
{
    public class GameController : Controller
    {
        private static Random _random = new Random();

        [HttpGet("api/game/index")]
        public JsonResult Index(int minNbr = 1, int maxNbr = 1001, int nbrChoice = 7)
        {
            List<Player> players = new List<Player>
            {
                new Player { name = "player 1", temps = 0, nbrChoice = -1 },
                new Player { name = "player 2", temps = 0, nbrChoice = -1 }
            };

            GameModel? game = new GameModel
            {
                TargetNumber = _random.Next(minNbr, maxNbr),
                Numbers = GenerateNumbers(nbrChoice, 1, 101),
                players = players,
                winner_not_verify = 10,
                Winner = ""
            };

            return Json(game);
        }

        [HttpPost("api/game/submitresults")]
        public JsonResult SubmitResults([FromBody] GameModel game)
        {
            // Afficher les données du jeu dans le terminal pour débogage
            LogGameData(game);

            // Déterminer le gagnant en fonction des choix des joueurs
            DetermineWinner(game);
            Console.WriteLine("winer not verifyyy "+game.winner_not_verify);

            return Json(game);
        }

        // Fonction pour afficher les données du jeu
        private void LogGameData(GameModel game)
        {
            Console.WriteLine("Données du jeu reçues :");
            Console.WriteLine("TargetNumber : " + game.TargetNumber);
            Console.WriteLine("Numbers : ");
            foreach (var number in game.Numbers)
            {
                Console.WriteLine(number);
            }
            Console.WriteLine("Players : ");
            foreach (var player in game.players)
            {
                Console.WriteLine("Player Name : " + player.name);
                Console.WriteLine("Player Temps : " + player.temps);
                Console.WriteLine("Player NbrChoice : " + player.nbrChoice);
            }
        }

        // Fonction pour déterminer le gagnant
        private void DetermineWinner(GameModel game)
        {
            Player j1 = game.players[0];
            Player j2 = game.players[1];
            // si un joueur n a pas donne de reponse
            if (IsOneChoicePositive(j1, j2))
            {
                game.winner_not_verify = j1.nbrChoice > 0 ? 0 : 1;
            }
            // sinon 
            else if (AreBothChoicesPositive(j1, j2))
            {
                int diffj1 = Math.Abs(j1.nbrChoice - game.TargetNumber);
                int diffj2 = Math.Abs(j2.nbrChoice - game.TargetNumber);
                game.winner_not_verify = DetermineClosestPlayer(diffj1, diffj2, j1, j2);
            }
            else
            {
                game.winner_not_verify = -1;
            }
        }

        // Vérifier si l'un des joueurs a un choix positif et l'autre négatif
        private bool IsOneChoicePositive(Player j1, Player j2)
        {
            return (j1.nbrChoice > 0 && j2.nbrChoice < 0) || (j1.nbrChoice < 0 && j2.nbrChoice > 0);
        }

        // Vérifier si les deux joueurs ont des choix positifs
        private bool AreBothChoicesPositive(Player j1, Player j2)
        {
            return j1.nbrChoice > 0 && j2.nbrChoice > 0;
        }

        // Déterminer le joueur le plus proche du nombre cible
        private int DetermineClosestPlayer(int diffj1, int diffj2, Player j1, Player j2)
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



        // Action pour retourner la vue Game.cshtml
        public IActionResult Game()
        {
            return View();
        }

        // Logique pour déterminer le gagnant
        [HttpPost("api/game/verify")]
        public JsonResult Verify([FromBody] GameModel game)
        {
            int IDwinner_not_verify = game.winner_not_verify;
            Console.WriteLine("verify nbr " + game.value_verify);

            if (game.players[IDwinner_not_verify].nbrChoice == game.value_verify)
            {
                return Json(game.players[IDwinner_not_verify]);
            }
            else
            {
                // Find the ID of the other player
                int otherPlayerID = (IDwinner_not_verify + 1) % game.players.Count;
                // Return the other player
                return Json(game.players[otherPlayerID]);
            }
        }



        // Méthode pour générer des nombres aléatoires
        private List<int> GenerateNumbers(int count, int min, int max)
        {
            var numbers = new List<int>();
            for (int i = 0; i < count; i++)
            {
                numbers.Add(_random.Next(min, max));
            }
            return numbers;
        }
    }
    public class VerifyRequestModel
    {
        public GameModel Game { get; set; }
        public int NbrVerify { get; set; }
    }

}
