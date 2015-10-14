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
            setState: setState,
            getEmptyArrayCheckins: getEmptyArrayCheckins
        };

        function setState(habitId, date, state) {
            var checkin = Checkin.save([], { HabitId: habitId, Date: date, State: state }, setStateSuccess, setStateError);
        }

        function setStateSuccess(checkin) {
            
        }

        function setStateError(headers) {
            //TODO нужно что-то делать при ошибке
            //Думаю, что нужно выдавать сообщение о ошибке
            var hdrs = headers;
        }

        function getEmptyArrayCheckins(habitId, amountCheckins) {
            var checkinArray = [];

            for (var i = 0; i < amountCheckins; i++) {
                var checkin = new Checkin();
                var currentDate = new Date();
                checkin.Date = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate(), 0, 0, 0, 0);
                checkin.State = 0;
                checkin.HabitId = habitId;
                checkinArray[i] = checkin;
            }
        }
    }
})();