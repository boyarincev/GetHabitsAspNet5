(function() {
    "use strict";
    angular.module("getHabitsApp", [ "ngRoute", "getHabitsApp.habitService", "getHabitsApp.checkinService", "getHabitsApp.HabitsControllers" ]).config(config).constant("AppName", "getHabitsApp");
    config.$inject = [ "$routeProvider", "$locationProvider" ];
    function config($routeProvider, $locationProvider) {
        $routeProvider.when("/app", {
            templateUrl: "/Views/list.html",
            controller: "HabitsListController"
        }).when("/app/add", {
            templateUrl: "/Views/add.html",
            controller: "HabitAddController"
        });
        $locationProvider.html5Mode(true);
    }
})();

(function() {
    "use strict";
    angular.module("getHabitsApp.HabitsControllers", []).controller("HabitsListController", HabitsListController);
    HabitsListController.$inject = [ "$scope", "habitService", "checkinService" ];
    function HabitsListController($scope, habitService, checkinsService) {
        $scope.creatingHabit = false;
        $scope.editable = false;
        $scope.amountCheckins = 12;
        $scope.arrayHead = new Array($scope.amountCheckins);
        $scope.habits = habitService.list();
        $scope.submitHabit = submitHabit;
        $scope.addNewHabit = addNewHabit;
        $scope.editHabit = editHabit;
        $scope.delHabit = delHabit;
        $scope.cancelEdit = cancelEdit;
        $scope.getDayForHead = getDayForHead;
        $scope.setFollowState = setFollowState;
        $scope.setUpViewState = setUpViewState;
        activate();
        function activate() {}
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
                checkin.viewState = "glyphicon-ban-circle checkin-missed";
                break;

              case 1:
                checkin.viewState = "glyphicon-ok-circle checkin-done";
                break;

              case 2:
                checkin.viewState = "glyphicon-remove-circle checkin-not-done";
                break;
            }
        }
        function setupServerState(checkin) {
            checkinsService.setState(checkin.HabitId, checkin.Date, checkin.State);
        }
    }
})();

(function() {
    "use strict";
    angular.module("getHabitsApp.checkinService", [ "ngResource" ]).constant("checkinApiUrl", "api/checkins").factory("checkinService", checkinService);
    checkinService.$inject = [ "$resource", "checkinApiUrl" ];
    function checkinService($resource, checkinApiUrl) {
        var Checkin = $resource(checkinApiUrl, {}, {});
        return {
            setState: setState
        };
        function setState(habitId, date, state) {
            var checkin = Checkin.save([], {
                HabitId: habitId,
                Date: date,
                State: state
            });
        }
    }
})();

(function() {
    "use strict";
    angular.module("getHabitsApp.habitService", [ "ngResource" ]).constant("apiUrl", "api/habits").factory("habitService", habitService);
    habitService.$inject = [ "$resource", "apiUrl" ];
    function habitService($resource, apiUrl) {
        var Resource = $resource(apiUrl + "/:habitId", {
            habitId: "@Id"
        }, {});
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
            newHabit.Name = "";
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
        function saveHabitError() {}
    }
})();

(function() {
    "use strict";
    angular.module("getHabitsApp.habitsService", [ "ngResource" ]).constant("apiUrl", "api/habits").factory("habitsService", habitsService);
    habitsService.$inject = [ "$resource", "apiUrl" ];
    function habitsService($resource, apiUrl) {
        var Resource = $resource(apiUrl + "/:habitId", {
            habitId: "@Id"
        }, {});
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
            newHabit.Name = "";
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
        function saveHabitError() {}
    }
})();