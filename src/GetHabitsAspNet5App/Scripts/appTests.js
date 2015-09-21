(function () {
    'use strict';
    describe('Tests getHabitsApp module', function () {
        describe('Check loading module and its dependencies', function () {
            var name;

            beforeEach(module('getHabitsApp'));

            beforeEach(inject(function (_AppName_) {
                name = _AppName_;
            }));

            it('application name is getHabitsApp', function () {
                expect(name).toEqual('getHabitsApp');
            });
        });

        describe('Check routing', function() {

        });
    });
})();