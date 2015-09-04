(function () {
    'use strict';

    angular
        .module('getHabitsApp.habitsService', ['ngResource'])
        .factory('habitsService', habitsService);

    habitsService.$inject = ['$resource'];

    
    function habitsService($resource) {
        return $resource('api/habits/:habitId', {habitId: '@id'}, {
            //query: { method: 'GET', params: {}, isArray: true}
        });
    }
})();