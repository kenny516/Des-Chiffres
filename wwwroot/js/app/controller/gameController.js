app.controller('GameController', ['$scope', '$interval', 'GameService', function ($scope, $interval, GameService) {
    const chronoInit = 20;
    $scope.timeLeft = chronoInit;
    $scope.timeFormat = formatTime($scope.timeLeft);
    $scope.isTimeUp = false;
    $scope.game = {};
    $scope.choice = null;
    $scope.verifChoice = "";
    $scope.finishGame = false;
    $scope.playerWin = null;

    // Load game
    GameService.loadGame().then(function (response) {
        $scope.game = response.data;
        $scope.choice = angular.copy($scope.game.numbers);
        console.log("Game loaded")
        // Load suggestion
        GameService.suggestInput($scope.game).then(function (response) {
            console.log("Suggestion received");
            $scope.suggestion = response.data;
            $scope.suggestedNumber = response.data.numberSugges;
            $scope.suggestedOperation = response.data.operation;
            console.log("Suggest loaded")
        }, function (error) {
            console.error("Error fetching suggestion", error);
        });
    });
    
    var timer = $interval(function () {
        if ($scope.timeLeft > 0) {
            $scope.timeLeft--;
            $scope.timeFormat = formatTime($scope.timeLeft);
        } else if ($scope.timeLeft === 0) {
            $scope.isTimeUp = true;
            $scope.timeIsUpFunction();
        }
    }, 1000);
    

    $scope.suggestInputPlayer = function(playerIndex) {
        $scope.game.players[playerIndex].nbrChoice = $scope.suggestedNumber;
        console.log("charger dans le input du joueur")
    }
    $scope.suggestVerif = function(playerIndex) {
        $scope.verifChoice = $scope.suggestedOperation;
        console.log("charger dans le verifiaction du joueur")
    }
    
    $scope.Formtarget = 0;
    $scope.Formnumbers = new Array(7);

    $scope.submitForm = function () {
        $scope.game.targetNumber = $scope.Formtarget;
        for (let i = 0; i < $scope.game.numbers.length; i++) {
            $scope.game.numbers[i] = angular.copy($scope.Formnumbers[i]);
        }
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

        console.log("change game data" + $scope.choice);
    };



    $scope.submitResults = function (playerIndex) {
        $scope.game.players[playerIndex].Temps = chronoInit - $scope.timeLeft;
        if ($scope.game.players[0].Temps > 0 && $scope.game.players[1].Temps > 0) {
            $scope.timeLeft = 0;
            console.log("end time");
        }
    };

    $scope.newGame = function () {
        GameService.newGame($scope.game).then(function (response) {
            console.log("New Game");
            $scope.game = response.data;
            $scope.verifChoice = "";
            $scope.finishGame = false;
            $scope.playerWin = null;
            $scope.timeLeft = chronoInit;
            $scope.choice = angular.copy($scope.game.numbers);
        });
    };

    $scope.timeIsUpFunction = function () {
        GameService.submitResults($scope.game).then(function (response) {
            $scope.game = response.data;
        });
    };

    $scope.addOperator = function (valueOp) {
        var index = $scope.choice.indexOf(valueOp);
        if (index !== -1) {
            $scope.choice.splice(index, 1);
        }
        console.log("verifChoice ajout de " + valueOp);
        $scope.verifChoice += valueOp.toString();
    };

    $scope.verify = function () {
        $scope.verifvalue = eval($scope.verifChoice);
        console.log("value float final " + $scope.verifvalue);
        $scope.game.value_verify = $scope.verifvalue;

        GameService.verify($scope.game).then(function (response) {
            $scope.playerWin = response.data;
            $scope.finishGame = true;
            console.log("gagnant " + response.data);
        });
    };

    function formatTime(seconds) {
        const minutes = Math.floor(seconds / 60);
        const remainingSeconds = seconds % 60;
        return `${minutes}:${String(remainingSeconds).padStart(2, '0')}`;
    }
}]);
