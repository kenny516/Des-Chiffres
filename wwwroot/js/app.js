var app = angular.module('dechiffreApp', ['ngRoute']);

app.config(function ($routeProvider) {
    $routeProvider
        .when('/homepage', {
            templateUrl: '/Home/Index'
        })
        .when('/privacypage', {
            templateUrl: '/Home/Privacy'
        })
        .when('/jouer', {
            templateUrl: '/Home/Home'
        })
        .when('/start', {
            templateUrl: '/Game/Game'
        });
});


app.controller('GameController', function ($scope, $interval, $http) {
    const chronoInit = 20;
    $scope.timeLeft = 20; // Set the timer to 60 seconds
    $scope.timeFormat = formatTime($scope.timeLeft);
    $scope.isTimeUp = false;
    $scope.game = {};

    $scope.choice = null;
    $scope.verifChoice = "";

    $scope.finishGame = false;
    $scope.playerWin = null;

    $http.get('/api/game/index').then(function (response) {
        $scope.game = response.data;
        $scope.choice = angular.copy($scope.game.numbers);
    });
    
    $scope.target = 0;
    $scope.numbers = new Array(7);

    $scope.submitForm = function() {
        $scope.game.targetNumber = $scope.target;
        $scope.game.numbers[0] = $scope.numbers[0];
        $scope.game.numbers[1] = $scope.numbers[1];
        $scope.game.numbers[2] = $scope.numbers[2];
        $scope.game.numbers[3] = $scope.numbers[3];
        $scope.game.numbers[4] = $scope.numbers[4];
        $scope.game.numbers[5] = $scope.numbers[5];
        $scope.game.numbers[6] = $scope.numbers[6];
        
        $scope.game.players[0].nbrChoice = -1;
        $scope.game.players[1].nbrChoice = -1;
        $scope.game.players[0].temps = 0;
        $scope.game.players[1].temps = 0;
        $scope.game.winner_not_verify = 10;
        $scope.game.value_verify = 0;
        
        
        $scope.verifChoice = "";
        $scope.finishGame = false;
        $scope.playerWin = null;
        $scope.timeLeft = chronoInit;
        $scope.choice = angular.copy($scope.numbers);
        $scope.isTimeUp = false;
        
        
        console.log("changer"+$scope.choice);
    };

    var timer = $interval(function () {
        if ($scope.timeLeft > 0) {
            $scope.timeFormat = formatTime($scope.timeLeft);
            $scope.timeLeft--;
            $scope.timeFormat = formatTime($scope.timeLeft);
        }else if ($scope.timeLeft === 0){
            $scope.timeFormat = formatTime($scope.timeLeft);
            $scope.isTimeUp = true;
/*            $interval.cancel(timer);*/
            $scope.timeIsUpFunction();
        }
    }, 1000);

    // fontion valider reponse d un joueur
    $scope.submitResults = function (playerIndex) {
        $scope.game.players[playerIndex].Temps = chronoInit - $scope.timeLeft; // Calculer le temps de soumission pour le joueur
        if ($scope.game.players[0].Temps > 0 && $scope.game.players[1].Temps > 0){
            $scope.timeLeft = 0;
            console.log("end time")
        }
    };

    //fonction rejouer avec nouveau nombre 
    $scope.newGame = function () {
        $http.post("api/game/newGame", $scope.game).then(function (response) {
            console.log("New Game");
            $scope.game = response.data;
            $scope.verifChoice = "";
            $scope.finishGame = false;
            $scope.playerWin = null;
            $scope.timeLeft = chronoInit;
            $scope.choice = angular.copy($scope.game.numbers);
        })
    }


    // Fonction à appeler lorsque le temps est écoulé determine le winner temp
    $scope.timeIsUpFunction = function () {
        $http.post('/api/game/submitresults', $scope.game).then(function (response) {
            $scope.game = response.data;
        });
    };


    //////////

    $scope.addOperator = function (valueOp) {
        var index = $scope.choice.indexOf(valueOp); // Trouver l'index de l'élément à retirer
        if (index !== -1) { // Vérifier si l'élément existe dans le tableau
            $scope.choice.splice(index, 1); // Retirer l'élément du tableau
        }
        console.log("verifChoice ajout de " + valueOp)
        $scope.verifChoice += valueOp.toString(); // Ajouter l'élément à la chaîne verifChoice
    }

    ////////
    $scope.verify = function () {
        $scope.verifvalue = eval($scope.verifChoice);
        console.log("value float final " + $scope.verifvalue);
        $scope.game.value_verify = $scope.verifvalue;

        $http.post('/api/game/verify', $scope.game).then(function (response) {
            $scope.playerWin = response.data;
            $scope.finishGame = true;
            console.log("gagnant " + response.data);
        });
    }

/*    $scope.callEval = function () {
        const encodeString = encodeURIComponent($scope.verifChoice)
        return $http.get('api/game/eval/?eval=' + encodeString)
            .then(function (response) {
                console.log(response.data);
                return response.data;
            })
            .catch(function (error) {
                console.error("Error occurred while fetching data: ", error);
                throw error;
            });
    };*/

});


function formatTime(seconds) {
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = seconds % 60;
    return `${minutes}:${String(remainingSeconds).padStart(2, '0')}`;
}






