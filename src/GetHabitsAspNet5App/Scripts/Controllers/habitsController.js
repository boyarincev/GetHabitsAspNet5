(function () {
    'use strict';

    angular
        .module('getHabitsApp')
        .controller('habitsController', habitsController);

    habitsController.$inject = ['$scope']; 

    function habitsController($scope) {
        $scope.title = 'habitsController';

        activate();

        function activate() { }
    }
})();
