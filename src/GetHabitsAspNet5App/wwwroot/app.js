(function() {
    "use strict";
    angular.module("getHabitsApp", [ "ngResource" ]);
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