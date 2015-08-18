(function () {
    'use strict';

    angular
        .module('getHabitsApp')
        .controller('habitsController', habitsController);

    habitsController.$inject = ['$scope', 'habitsService'];

    function habitsController($scope, habitsService) {
        $scope.title = 'habitsController';

        activate();

        $scope.habits = habitsService.getData();

        function activate() { }
    }
})();
