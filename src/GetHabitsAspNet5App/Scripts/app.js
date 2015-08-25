(function () {
    'use strict';

    angular.module('getHabitsApp', [
        // Angular modules
        'ngResource',
        'ngRoute'
        // Custom modules


        // 3rd Party Modules

    ]).config(config);

    config.$inject = ['$routeProvider', '$locationProvider'];

    function config($routeProvider, $locationProvider) {
        $routeProvider
            .when('/app', {
                templateUrl: '/Views/list.html',
                controller: 'habitsController'
            })
            .when('/app/add', {
                templateUrl: '/Views/add.html',
                controller: 'habitsController'
            });

        $locationProvider.html5Mode(true);
    }
})();