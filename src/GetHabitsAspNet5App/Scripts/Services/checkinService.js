(function () {
    'use strict';

    angular.module('getHabitsApp.checkinService', ['ngResource'])
        .constant('checkinApiUrl', 'api/checkins')
        .factory('checkinService', checkinService);

    checkinService.$inject = ['$resource', 'checkinApiUrl'];

    function checkinService($resource, checkinApiUrl) {
        var Checkin = $resource(checkinApiUrl, { }, {
            //query: { method: 'GET', params: {}, isArray: true}
        });

        return {
            setState: setState
        };

        function setState(habitId, date, state) {
            var checkin = Checkin.save([], { HabitId: habitId, Date: date, State: state });
        }
    }
})();