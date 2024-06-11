using Dechiffre.Models;
using Microsoft.AspNetCore.Mvc;

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
                new Player { name = "player 1", temps = 0, nbrChoice = -1 ,point = 0},
                new Player { name = "player 2", temps = 0, nbrChoice = -1 ,point = 0}
            };

            GameModel? game = new GameModel
            {
                TargetNumber = _random.Next(minNbr, maxNbr),
                Numbers = GenerateNumbers(nbrChoice, 1, 101),
                players = players,
                winner_not_verify = 10,
            };

            return Json(game);
        }

        [HttpPost("api/game/newGame")]
        public JsonResult NewGame([FromBody] GameModel game)
        {
            game.TargetNumber = _random.Next(100, 1000);
            game.players[0].nbrChoice = -1;
            game.players[1].nbrChoice = -1;

            game.winner_not_verify = 10;
            game.value_verify = 0;
            game.Numbers = GenerateNumbers(7, 1, 101);
            return Json(game);
        }
        

        [HttpPost("api/game/submitresults")]
        public JsonResult SubmitResults([FromBody] GameModel game)
        {
            // Afficher les données du jeu dans le terminal pour débogage
            LogGameData(game);

            // Déterminer le gagnant en fonction des choix des joueurs
            game.DetermineWinner(game);
            Console.WriteLine("winer not verifyyy "+game.winner_not_verify);

            return Json(game);
        }

////    FONCTION 

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

        // Action pour retourner la vue Game.cshtml
        public IActionResult Game()
        {
            return View();
        }

        // Logique pour déterminer le gagnant
        [HttpPost("api/game/verify")]
        public JsonResult Verify([FromBody] GameModel game)
        {
            return Json(game.VerifyWinner());
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

}
