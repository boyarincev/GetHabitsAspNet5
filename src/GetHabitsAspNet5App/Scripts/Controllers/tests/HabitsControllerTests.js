﻿(function () {
    'use strict';
    describe('Testing HabitsControllers module:', function () {
        var $controller;
        var scope;
        var habit;

        beforeEach(module('getHabitsApp'));

        beforeEach(inject(function (_$controller_, _$rootScope_) {
            $controller = _$controller_;
            scope = _$rootScope_.$new();
            })
        );

        beforeEach(function () {
            habit = { Name: 'Thinking about life' };
        });

        describe('Test HabitsListController:', function () {
            var habitService;
            var habitsController;

            beforeEach(inject(function (_habitService_) {
                habitService = _habitService_;

                sinon.stub(habitService, "list", function() {
                    return [{ Name: 'Give up smoking' }, { Name: 'Give up drinking' }]
                });
                sinon.stub(habitService, "createHabitButNotSave", function() {
                    return { Name: '' };
                });
                sinon.stub(habitService, 'saveHabit', function(habit) {
                    habit.saveSuccessEvent();
                });
                sinon.stub(habitService, 'remove');
            }));

            beforeEach(function () {
                habitsController = $controller('HabitsListController', { $scope: scope });
            });

            it('have to call list method of habitservice by creating', function () {
                //Arrahge

                //Act

                //Assert

                expect(habitService.list.calledOnce).toBe(true);
            });

            describe('addNewHabit() method testing:', function () {
                var initialCountHabits;

                beforeEach(function () {
                    //Arrange
                    initialCountHabits = scope.habits.length;

                    //Act
                    scope.addNewHabit();
                });

                it('added new habit into habits array', function () {
                    //Arrange

                    //Act

                    //Assert
                    expect(scope.habits.length).toBe(initialCountHabits + 1);
                });

                it('the new habit have property editable in true', function () {
                    var lastHabit = scope.habits.pop();
                    expect(lastHabit.editable).toBe(true);
                });

                it('scope property creatingHabit have to be true', function () {
                    expect(scope.creatingHabit).toBe(true);
                });

                it('newName property of habit have to copy Name property', function () {
                    var lastHabit = scope.habits.pop();

                    expect(lastHabit.newName).toBe(lastHabit.Name);
                });
            });

            describe('submitHabit() method testing:', function () {

                beforeEach(function () {
                    //Arrange
                    habit.newName = 'Thinking about life 2';
                    habit.editable = true;
                    scope.creatingHabit = true;

                    //Act
                    scope.submitHabit(habit);
                });

                it('newName property have to copying into Name property', function () {
                    //Assert
                    expect(habit.Name).toBe(habit.newName);
                });

                it('editable property have to be false', function () {
                    //Assert
                    expect(habit.editable).toBe(false);
                });

                it('creatingHabit of scope property have to be false', function () {
                    //Assert
                    expect(scope.creatingHabit).toBe(false);
                });
            });

            describe('editHabit() method testing:', function () {
                beforeEach(function () {
                    //Arrange

                    //Act
                    scope.editHabit(habit);
                });

                it('Name property have to copying to newName property', function () {
                    expect(habit.Name).toBe(habit.newName);
                });

                it('editable property have to be true', function () {
                    expect(habit.editable).toBe(true);
                });
            });

            describe('delHabit() method testing:', function () {
                it('need delete habit from habits array', function () {
                    //Arrange
                    var countHabits = scope.habits.length;
                    var indexRemovedHabit = 0;
                    var removedHabit = scope.habits[indexRemovedHabit];
                    var nameRemovedHabit = removedHabit.Name;

                    //Act
                    scope.delHabit(removedHabit, indexRemovedHabit)

                    //Assert
                    expect(countHabits - 1).toBe(scope.habits.length);
                    expect(scope.habits[indexRemovedHabit].Name).not.toBe(nameRemovedHabit);
                });
            });

            describe('cancelEdit() method testing:', function () {
                var currentIndex = 0;
                var currentHabit;
                var initialAmountHabits;

                beforeEach(function () {
                    //Arrange
                    currentHabit = scope.habits[currentIndex];
                    currentHabit.editable = true;
                    scope.creatingHabit = true;
                    initialAmountHabits = scope.habits.length;

                    //Act
                    scope.cancelEdit(currentHabit, currentIndex);
                });

                it('editable property have to be false', function () {
                    //Assert
                    expect(currentHabit.editable).toBe(false);
                });

                it('creatingHabit property of scope have to be false', function () {
                    //Assert
                    expect(scope.creatingHabit).toBe(false);
                });

                it('newName property of habit have to equal Name property', function () {
                    //Assert
                    expect(currentHabit.newName).toBe(currentHabit.Name);
                });

                it('if habit not saved yet, it have to be remove from habits', function () {
                    expect(currentHabit.Name).not.toBe(scope.habits[currentIndex]);
                    expect(initialAmountHabits - 1).toBe(scope.habits.length);
                });
            });

            describe('cancelEdit() method testing for habit with id:', function () {
                it('if habit saved already, it don\'t have to be removed from habits', function () {
                    //Arrange
                    var currentIndex = 0;
                    var currentHabit = scope.habits[currentIndex];
                    currentHabit.Id = 1;
                    var initialAmountHabits = scope.habits.length;

                    //Act
                    scope.cancelEdit(currentHabit, currentIndex);

                    //Assert
                    expect(currentHabit.Name).toBe(scope.habits[currentIndex].Name);
                    expect(initialAmountHabits).toBe(scope.habits.length);
                });
            });

            describe('setUpViewState() method testing:', function () {
                it('By 0 State checkin property have to sets up "glyphicon-ban-circle checkin-missed" viewState property', function () {
                    //Arrange
                    var checkin = { State: 0 };

                    //Act
                    scope.setUpViewState(checkin);

                    //Assert
                    expect(checkin.viewState).toBe('glyphicon-ban-circle checkin-missed');
                });

                it('By 1 State checkin property have to sets up "glyphicon-ok-circle checkin-done" viewState property', function () {
                    //Arrange
                    var checkin = { State: 1 };

                    //Act
                    scope.setUpViewState(checkin);

                    //Assert
                    expect(checkin.viewState).toBe('glyphicon-ok-circle checkin-done');
                });

                it('By 2 State checkin property have to sets up "glyphicon-remove-circle checkin-not-done" viewState property', function () {
                    //Arrange
                    var checkin = { State: 2 };

                    //Act
                    scope.setUpViewState(checkin);

                    //Assert
                    expect(checkin.viewState).toBe('glyphicon-remove-circle checkin-not-done');
                });
            });

            describe('setFollowState() method testing:', function () {

            });
        });
    });
})();