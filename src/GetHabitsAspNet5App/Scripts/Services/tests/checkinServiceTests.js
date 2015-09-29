(function() {
    'use strict';

    describe('Testing checkinService:', function () {
        var $httpBackend, checkinApiUrl, checkinService;

        beforeEach(module('getHabitsApp.checkinService'));

        beforeEach(inject(function(_$httpBackend_, _checkinApiUrl_, _checkinService_) {
            $httpBackend = _$httpBackend_;
            checkinApiUrl = _checkinApiUrl_;
            checkinService = _checkinService_;
        }));

        it('checkinApiUrl have to be "api/checkins"', function() {
            expect(checkinApiUrl).toBe('api/checkins');
        });

        describe('testing setState() method', function () {
            var habitId = 1, state = 2, date = new Date();

            beforeEach(function () {
                checkinService.setState(habitId, date, state);
            });

            it('have to send correct query', function() {
                //Arrange
                $httpBackend.expectPOST(checkinApiUrl, { HabitId: habitId, Date: date, State: state }).respond({});

                //Act
                $httpBackend.flush();

                //Assert
            });
        });
    });
})();