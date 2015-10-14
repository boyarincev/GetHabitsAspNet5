(function () {
    'use strict';

    angular
        .module('getHabitsApp.habitService', ['ngResource', 'getHabitsApp.checkinService'])
        .constant('apiUrl', 'api/habits')
        .factory('habitService', habitService);

    habitService.$inject = ['$resource', 'apiUrl', 'checkinService'];

    function habitService($resource, apiUrl, checkinService) {
        var Resource = $resource(apiUrl + '/:habitId', { habitId: '@Id' }, {
            //query: { method: 'GET', params: {}, isArray: true}
        });

        return {
            list: list,
            remove: remove,
            saveHabit: saveHabit,
            createHabitButNotSave: createHabitButNotSave
        };

        function list(amountCheckins) {
            return Resource.query({ checkinLastDaysAmount: amountCheckins });
        }

        function remove(habit) {
            habit.$remove();
        }

        function createHabitButNotSave(amountCheckins) {
            var newHabit = new Resource({});
            newHabit.Name = '';
            newHabit.Checkins = checkinService.getEmptyArrayCheckins(amountCheckins);
            return newHabit;
        }

        function saveHabit(habit, amountCheckins) {
            habit.saving = true;
            habit.$save({ checkinLastDaysAmount: amountCheckins }, saveHabitSuccess, saveHabitError);
        }

        function saveHabitSuccess(habit) {
            habit.saving = false;
        }

        function saveHabitError(headers) {
            //TODO нужно что-то делать при ошибке
            //Думаю, что нужно выдавать сообщение о ошибке
            var hdrs = headers;
        }
    }
})();