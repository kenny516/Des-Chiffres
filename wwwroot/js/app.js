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
    $scope.timeLeft = 60; // Set the timer to 60 seconds
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

    var timer = $interval(function () {
        if ($scope.timeLeft > 0) {
            $scope.timeLeft--;
            $scope.timeFormat = formatTime($scope.timeLeft);
        } else {
            $scope.isTimeUp = true;
            $interval.cancel(timer);
            $scope.timeIsUpFunction();
        }
    }, 1000);
    
    // fontion valider reponse d un joueur
    $scope.submitResults = function (playerIndex) {
        $scope.game.players[playerIndex].Temps = 60 - $scope.timeLeft; // Calculer le temps de soumission pour le joueur
    };
    
    //fonction rejouer avec nouveau nombre 
    $scope.newGame = function (){
        $http.post("api/game/newGame",$scope.game).then(function (response){
            console.log("atoo zaaa");
            $scope.timeLeft = 60;
            $scope.game = response.data;
        })
    }
    
    

    // Fonction à appeler lorsque le temps est écoulé
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
        console.log("ajout de" + valueOp)
        $scope.verifChoice += valueOp.toString(); // Ajouter l'élément à la chaîne verifChoice
    }

    ////////
    $scope.verify = function () {
        $scope.verifvalue = eval($scope.verifChoice);
        console.log("value int final " + $scope.verifvalue);
        $scope.game.value_verify = $scope.verifvalue;

        $http.post('/api/game/verify', $scope.game).then(function (response) {
            $scope.playerWin = response.data;
            $scope.finishGame = true;
            console.log("gagnant "+response.data);
        });
    };
    
});
function formatTime(seconds) {
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = seconds % 60;
    return `${minutes}:${String(remainingSeconds).padStart(2, '0')}`;
}



