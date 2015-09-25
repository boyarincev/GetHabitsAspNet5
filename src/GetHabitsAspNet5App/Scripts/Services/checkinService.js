(function () {
    'use strict';

    angular.module('getHabitsApp.checkinService', ['ngResource'])
        .constant('apiUrl', 'api/checkins')
        .factory('checkinService', checkinService);

    checkinService.$inject = ['$resource', 'apiUrl'];

    function checkinService($resource, apiUrl){
        var Checkin = $resource(apiUrl, { habitId: '@habitId', date: '@date' }, {
            //query: { method: 'GET', params: {}, isArray: true}
        });

        return {
            setState: setState
        };

        function setState(habitId, date, state) {
            var checkin = Checkin.save({ habitId: habitId, date: date, state: state });
        }
    }
})();