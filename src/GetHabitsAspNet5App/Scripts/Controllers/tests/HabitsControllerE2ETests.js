(function () {
    'use strict';

    //insulate manipulations with DOM elements
    function getHabitsTable() {
        return element(by.id('habits'));
    }

    function getAddNewHabitButton() {
        return element(by.id('add-new-habit-button'));
    }

    function getAllTrInTable(table) {
        return table.all(by.tagName('tr'));
    }

    function getInputInHabitRow(habitTr) {
        return habitTr.element(by.css('input'));
    }

    function getSpanWithDisplayedHabitName(habitTr) {
        return habitTr.element(by.css('span.habit-name'));
    }

    function getSpanWhichDisplayHabitName(displayedName) {
        return getHabitsTable().element(by.cssContainingText('span.habit-name', displayedName));
    }

    function getSendHabitButton(habitTr) {
        return habitTr.element(by.css('button.send-habit'));
    }

    function getEditButton(habitTr) {
        return habitTr.element(by.css('button.edit-button'));
    }

    function getDeleteButton(habitTr) {
        return habitTr.element(by.css('button.delete-button'));
    }

    function getCancelEdit(habitTr) {
        return habitTr.element(by.css('button.cancel-edit'));
    }

    //Tests start
    describe('Testing list habit:', function () {
        var habitsTable;

        beforeEach(function () {
            browser.get('');
            habitsTable = getHabitsTable();
        });

        describe('Work with new habit:', function () {
            var newHabitButton;

            beforeEach(function () {
                newHabitButton = getAddNewHabitButton();
            });

            it('After click NewHabit button have to appears new row in habit table', function () {
                //Arrange
                var countRowsBefore = getAllTrInTable(habitsTable).count();

                //Act
                newHabitButton.click();

                //Assert
                var countRowsAfter = getAllTrInTable(habitsTable).count().then(function (countAfter) {
                    expect(countRowsBefore).toBe(countAfter - 1);
                });
            });

            it('After click NewHabit button, it have to be disabled', function () {
                //Arrange
                expect(newHabitButton.isEnabled()).toBe(true);

                //Act
                newHabitButton.click();

                //Assert
                expect(newHabitButton.isEnabled()).toBe(false);
            });

            describe('Clicked New Habit button:', function () {
                var lastTr;
                var newInput;
                var textNameForInput;
                var lastHabitNameInTable;

                beforeEach(function () {
                    newHabitButton.click();
                    lastTr = getAllTrInTable(habitsTable).last();
                    newInput = getInputInHabitRow(lastTr);
                    textNameForInput = 'Thinking about life';
                    lastHabitNameInTable = getSpanWithDisplayedHabitName(lastTr);
                });

                it('Submit new habit into end of habits table', function () {
                    //Arrange

                    //Act
                    newInput.sendKeys(textNameForInput);
                    newInput.submit();

                    //Assert
                    var factName = lastHabitNameInTable.getText();
                    expect(factName).toBe(textNameForInput);
                });

                it('Sending new habit by click button', function () {
                    //Arrange
                    var sendHabitButton = getSendHabitButton(lastTr);

                    //Act
                    newInput.sendKeys(textNameForInput);
                    sendHabitButton.click();

                    //Assert
                    var factName = lastHabitNameInTable.getText();
                    expect(factName).toBe(textNameForInput);
                });

                describe('New habit submited:', function () {
                    var editButton;
                    var deleteButton;
                    var sendHabitButton;

                    beforeEach(function () {
                        newInput.sendKeys(textNameForInput);
                        sendHabitButton = getSendHabitButton(lastTr);
                        sendHabitButton.click();
                        editButton = getEditButton(lastTr);
                        deleteButton = getDeleteButton(lastTr);
                    });

                    it("edit new habit's name", function () {
                        editButton.click();
                        newInput.sendKeys('Addon');
                        sendHabitButton.click();
                        expect(textNameForInput + 'Addon').toBe(lastHabitNameInTable.getText());
                    });

                    it("delete new habit", function () {
                        var countRowsBefore = getAllTrInTable(habitsTable).count();
                        deleteButton.click();
                        getAllTrInTable(habitsTable).count().then(function (countRowsAfter) {
                            expect(countRowsBefore).toBe(countRowsAfter + 1);
                        });

                        expect(getSpanWhichDisplayHabitName(textNameForInput).isPresent()).toBe(false);
                    });
                });

                describe('Cancel creating new habit:', function () {
                    var countRowsAtFirst;
                    var cancelEditButton;

                    beforeEach(function () {
                        countRowsAtFirst = getAllTrInTable(habitsTable).count();
                        cancelEditButton = getCancelEdit(lastTr);
                    });

                    it('without text input', function () {
                        //Arrange

                        //Act

                        //Assert

                    });

                    it('with text input', function () {
                        //Act
                        newInput.sendKeys(textNameForInput);
                    });

                    it('with text input and clear after', function () {
                        newInput.sendKeys(textNameForInput);
                        newInput.clear();
                    });

                    afterEach(function () {
                        //Arrange

                        //Act
                        cancelEditButton.click();

                        //Assert
                        var countRowsAtEnd = getAllTrInTable(habitsTable).count().then(function (countAtEnd) {
                            expect(countAtEnd + 1).toEqual(countRowsAtFirst);
                        });
                    });
                });
            });
        });

        describe('Work with exist habit:', function () {
            var firstHabitTrInTable;
            var editButton;
            var deleteButton;
            var sendHabitButton;
            var inputControl;
            var firstHabitName;
            var cancelEditButton;

            beforeEach(function () {
                firstHabitTrInTable = getAllTrInTable(habitsTable).last();
                firstHabitName = getSpanWithDisplayedHabitName(firstHabitTrInTable);
                inputControl = getInputInHabitRow(firstHabitTrInTable);
                sendHabitButton = getSendHabitButton(firstHabitTrInTable);
                cancelEditButton = getCancelEdit(firstHabitTrInTable);
                editButton = getEditButton(firstHabitTrInTable);
                deleteButton = getDeleteButton(firstHabitTrInTable);
            });

            it('edit name exist habit', function () {
                //Arrange
                var initialHabitName;
                firstHabitName.getText().then(function(habitName) {
                    initialHabitName = habitName;
                });
                //Act
                firstHabitName.click();
                editButton.click();
                inputControl.sendKeys('Addon');
                sendHabitButton.click();
                //Assert
                firstHabitName.getText().then(function(finalHabitName) {
                    expect(initialHabitName + 'Addon').toBe(finalHabitName);
                });
            });

            it('cancel edit after enter new name', function () {
                //Arrange
                var initialHabitName;
                firstHabitName.getText().then(function (habitName) {
                    initialHabitName = habitName;
                });
                //Act
                firstHabitName.click();
                editButton.click();
                inputControl.sendKeys('Addon');
                cancelEditButton.click();
                //Assert
                firstHabitName.getText().then(function (finalHabitName) {
                    expect(initialHabitName).toBe(finalHabitName);
                });
            });

            it("delete exist habit", function () {
                //Arrange
                var countRowsBefore = getAllTrInTable(habitsTable).count();
                var name = firstHabitName.getText();
                expect(getSpanWhichDisplayHabitName(name).isPresent()).toBe(true);
                //Act
                firstHabitName.click();
                deleteButton.click();
                //Assert
                getAllTrInTable(habitsTable).count().then(function (countRowsAfter) {
                    expect(countRowsBefore).toBe(countRowsAfter + 1);
                });

                expect(getSpanWhichDisplayHabitName(name).isPresent()).toBe(false);
            });
        });

        describe('testing Checkins', function () {
            it('click checkin link', function () {
                //Arrange
                var lastTr = getAllTrInTable(habitsTable).last();
                var firstSpanCheckin = lastTr.element(by.css('td.checkin span'));
                var firstSpanCheckinClass = firstSpanCheckin.getAttribute('class');

                firstSpanCheckin.click();

                expect(firstSpanCheckinClass).not.toBe(firstSpanCheckin.getAttribute('class'));
            });
        });
    });
})();