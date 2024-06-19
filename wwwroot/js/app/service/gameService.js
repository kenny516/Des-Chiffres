app.service('GameService', ['$http', function($http) {
    
    this.loadGame = function() {
        return $http.get('/api/game/index');
    };

    this.suggestInput = function(game) {
        return $http.post('api/game/suggest', game);
    };

    this.newGame = function(game) {
        return $http.post('api/game/newGame', game);
    };

    this.submitResults = function(game) {
        return $http.post('/api/game/submitresults', game);
    };

    this.verify = function(game) {
        return $http.post('/api/game/verify', game);
    };
    
}]);
