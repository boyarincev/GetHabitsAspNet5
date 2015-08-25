(function() {
    "use strict";
    angular.module("getHabitsApp", [ "ngResource", "ngRoute" ]).config(config);
    config.$inject("$routeProvider", "$locationProvider");
    function config($routeProvider, $locationProvider) {
        $routeProvider.when("/", {
            templateUrl: "/Views/list.html",
            controller: "habitsController"
        }).when("/add", {
            templateUrl: "/Views/add.html",
            controller: "habitsController"
        });
        $locationProvider.html5Mode(true);
    }
})();

(function() {
    "use strict";
    angular.module("getHabitsApp").controller("habitsController", habitsController);
    habitsController.$inject = [ "$scope", "habitsService" ];
    function habitsController($scope, habitsService) {
        $scope.title = "habitsController";
        activate();
        $scope.habits = habitsService.getData.query();
        $scope.Name = "Приложуха";
        function activate() {}
    }
})();

(function() {
    "use strict";
    angular.module("getHabitsApp").factory("habitsService", habitsService);
    habitsService.$inject = [ "$resource" ];
    function habitsService($resource) {
        var service = {
            getData: getData()
        };
        return service;
        function getData() {
            return $resource("api/habits", {}, {
                query: {
                    method: "GET",
                    params: {},
                    isArray: true
                }
            });
        }
    }
})();