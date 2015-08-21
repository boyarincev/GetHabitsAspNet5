(function () {
    'use strict';

    angular
        .module('getHabitsApp')
        .factory('habitsService', habitsService);

    habitsService.$inject = ['$resource'];

    function habitsService($resource) {
        var service = {
            getData: getData()
        };

        return service;

        function getData() {
            return $resource('api/habits', {}, {
                query: { method: 'GET', params: {}, isArray: true}
            });
        }
    }
})();