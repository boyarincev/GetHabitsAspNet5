(function () {
    'use strict';
    describe("Testing habitService:", function () {
        var $httpBackend, apiUrl, habitService;

        beforeEach(module('getHabitsApp.habitService'));

        beforeEach(inject(function (_$httpBackend_, _apiUrl_, _habitService_) {
            $httpBackend = _$httpBackend_;
            apiUrl = _apiUrl_;
            habitService = _habitService_;
        }));

        it('apiUrl have to be "api/habits"', function () {
            expect(apiUrl).toBe("api/habits");
        });

        describe('list() method:', function () {
            it('have to be sent get request on apiUrl', function () {
                //Arrange
                var amountCheckins = 12;

                //Act
                habitService.list(amountCheckins);

                //Asserts
                $httpBackend.expectGET(apiUrl + "?checkinLastDaysAmount=" + amountCheckins).respond([]);
                $httpBackend.flush();
            });
        });

        describe('remove() method:', function () {
            var habit;

            beforeEach(function () {
                //Arrange
                habit = habitService.createHabitButNotSave();
                habit.Id = 1;

                //Act
                habitService.remove(habit);
            });

            it('have to be sent delete request', function () {

                //Assert
                $httpBackend.expectDELETE(apiUrl + '/' + habit.Id).respond();
                $httpBackend.flush();
            });
        });

        describe('createHabitButNotSave() method:', function () {
            var emptyHabit;

            beforeEach(function () {
                //Act
                emptyHabit = habitService.createHabitButNotSave();
            });

            it('have to create object with empty Name property', function () {
                expect(emptyHabit.Name).toBe('');
            });
        });

        describe('saveHabit() method:', function () {
            var habit;

            beforeEach(function () {
                //Arrange
                habit = habitService.createHabitButNotSave();
                habit.Id = 3;
                habit.saveSuccessEvent = function () {

                };

                //Act
                habitService.saveHabit(habit);
            });

            it('saving property until get respond have to be true', function() {
                //Act

                //Assert
                expect(habit.saving).toBe(true);
            });

            it('saving property after get success responce have to be false', function () {
                //Arrange
                $httpBackend.whenPOST(apiUrl + '/' + habit.Id, habit).respond(201, '');

                //Act
                $httpBackend.flush();

                //Assert
                expect(habit.saving).toBe(false);
            });

            it('if responce failure then saving property until have to be true', function () {
                //Arrange
                $httpBackend.whenPOST(apiUrl + '/' + habit.Id, habit).respond(400, '');

                //Act
                $httpBackend.flush();

                //Assert
                expect(habit.saving).toBe(true);
            });

            it('have to be sent post request', function () {
                //Assert
                $httpBackend.expectPOST(apiUrl + '/' + habit.Id, habit).respond(400, '');
                $httpBackend.flush();
            });
        });
    });



})();