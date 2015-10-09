(function () {
    'use strict';

    angular
        .module('getHabitsApp.habitService', ['ngResource'])
        .constant('apiUrl', 'api/habits')
        .factory('habitService', habitService);

    habitService.$inject = ['$resource', 'apiUrl'];

    function habitService($resource, apiUrl) {
        var Resource = $resource(apiUrl + '/:habitId', { habitId: '@Id', checkinLastDaysAmount: 12 }, {
            //query: { method: 'GET', params: {}, isArray: true}
        });

        return {
            list: list,
            remove: remove,
            saveHabit: saveHabit,
            createHabitButNotSave: createHabitButNotSave
        };

        function list() {
            return Resource.query();
        }

        function remove(habit) {
            habit.$remove();
        }

        function createHabitButNotSave() {
            var newHabit = new Resource({});
            newHabit.Name = '';
            return newHabit;
        }

        function saveHabit(habit) {
            habit.saving = true;
            habit.$save({}, saveHabitSuccess, saveHabitError);
        }

        function saveHabitSuccess(habit) {
            habit.saving = false;
            habit.saveSuccessEvent();
        }

        function saveHabitError(headers) {
            //TODO нужно что-то делать при ошибке
            //Думаю, что нужно выдавать сообщение о ошибке
            var hdrs = headers;
        }
    }
})();