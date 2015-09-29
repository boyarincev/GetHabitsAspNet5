(function () {
    'use strict';

    angular
        .module('getHabitsApp.HabitsControllers', [])
        .controller('HabitsListController', HabitsListController);

    HabitsListController.$inject = ['$scope', 'habitService', 'checkinService'];

    function HabitsListController($scope, habitService, checkinsService) {

        //Properties
        $scope.creatingHabit = false;
        $scope.editable = false;
        $scope.amountCheckins = 12;
        $scope.arrayHead = new Array($scope.amountCheckins);
        $scope.habits = habitService.list();


        //Methods
        $scope.submitHabit = submitHabit;
        $scope.addNewHabit = addNewHabit;
        $scope.editHabit = editHabit;
        $scope.delHabit = delHabit;
        $scope.cancelEdit = cancelEdit;
        $scope.getDayForHead = getDayForHead;
        $scope.setFollowState = setFollowState;
        $scope.setUpViewState = setUpViewState;

        activate();

        function activate() {

        }

        function addNewHabit() {
            var newHabit = habitService.createHabitButNotSave();
            newHabit.newName = newHabit.Name;
            newHabit.editable = true;
            $scope.creatingHabit = true;
            $scope.habits.push(newHabit);
        }

        function submitHabit(habit) {
            habit.Name = habit.newName;
            habit.editable = false;
            habit.saveSuccessEvent = saveSuccess;
            habitService.saveHabit(habit);
        }

        function saveSuccess() {
            $scope.creatingHabit = false;
        }

        function editHabit(habit) {
            habit.newName = habit.Name;
            habit.editable = true;
        }

        function delHabit(habit, habitIndex) {
            habitService.remove(habit);
            $scope.habits.splice(habitIndex, 1);
        }

        function cancelEdit(habit, habitIndex) {
            habit.editable = false;
            $scope.creatingHabit = false;

            habit.newName = habit.Name;

            var id = habit.Id;

            if (id === undefined) {
                $scope.habits.splice(habitIndex, 1);
            }
        }

        function getDayForHead(headIndex) {
            var currentDate = new Date();
            currentDate.setDate(currentDate.getDate() - headIndex);
            return currentDate.getDate();
        }

        function setFollowState(checkin) {
            switch (checkin.State) {
                case 0:
                    checkin.State = 1;
                    break;
                case 1:
                    checkin.State = 2;
                    break;
                case 2:
                    checkin.State = 0;
                    break;

            }

            $scope.setUpViewState(checkin);
            setupServerState(checkin);
        }

        function setUpViewState(checkin) {
            switch (checkin.State) {
                case 0:
                    checkin.viewState = 'glyphicon-ban-circle checkin-missed';
                    break;
                case 1:
                    checkin.viewState = 'glyphicon-ok-circle checkin-done';
                    break;
                case 2:
                    checkin.viewState = 'glyphicon-remove-circle checkin-not-done';
                    break;
            }

        }

        function setupServerState(checkin) {
            checkinsService.setState(checkin.HabitId, checkin.Date, checkin.State);
        }
    }
})();
