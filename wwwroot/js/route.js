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