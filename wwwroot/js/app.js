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
        });
});

// Déclaration du contrôleur MainController
app.controller('MainController', function ($scope) {
    $scope.message = '';
});

// Déclaration du contrôleur HomeController
app.controller('HomeController', function ($scope) {
    $scope.message = 'Welcome to the Home page!';
});

app.controller('PrivacyController', function ($scope) {
    $scope.message = 'Your privacy is important to us.';
});
