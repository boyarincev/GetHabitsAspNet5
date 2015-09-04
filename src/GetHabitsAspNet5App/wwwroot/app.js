(function() {
    "use strict";
    angular.module("getHabitsApp", [ "ngRoute", "getHabitsApp.habitsService" ]).config(config);
    config.$inject = [ "$routeProvider", "$locationProvider" ];
    function config($routeProvider, $locationProvider) {
        $routeProvider.when("/app", {
            templateUrl: "/Views/list.html",
            controller: "habitsController"
        }).when("/app/add", {
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
        $scope.habits = habitsService.query();
        $scope.Name = "Приложуха";
        function activate() {}
    }
})();

(function() {
    "use strict";
    angular.module("getHabitsApp.habitsService", [ "ngResource" ]).factory("habitsService", habitsService);
    habitsService.$inject = [ "$resource" ];
    function habitsService($resource) {
        return $resource("api/habits/:habitId", {
            habitId: "@id"
        }, {});
    }
})();