(function () {
    'use strict';

    angular
        .module('getHabitsApp.habitsService', ['ngResource'])
        .constant('apiUrl', 'api/habits')
        .factory('habitsService', habitsService);

    habitsService.$inject = ['$resource', 'apiUrl'];

    function habitsService($resource, apiUrl) {
        var Resource = $resource(apiUrl + '/:habitId', { habitId: '@Id' }, {
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

        function saveHabitError() {
            //TODO нужно что-то делать при ошибке
        }
    }
})();