(function () {
    'use strict';

    angular.module('getHabitsApp', [
        // Angular modules
        'ngRoute',
        // Custom modules
        'getHabitsApp.habitsService',
        'getHabitsApp.checkinService',
        'getHabitsApp.HabitsControllers'

        // 3rd Party Modules

    ]).config(config).constant('AppName', 'getHabitsApp');

    config.$inject = ['$routeProvider', '$locationProvider'];

    /**
     * @description Determine
     * @param {string} $routeProvider The $routeProvider for given function
     * @param {number} $locationProvider $locationProvider determine which location should be used
     */
    function config($routeProvider, $locationProvider) {
        $routeProvider
            .when('/app', {
                templateUrl: '/Views/list.html',
                controller: 'HabitsListController'
            })
            .when('/app/add', {
                templateUrl: '/Views/add.html',
                controller: 'HabitAddController'
            });

        $locationProvider.html5Mode(true);
    }

})();