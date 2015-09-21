describe('Check main app page:', function () {
    it('have to be "habits" id on page', function () {
        browser.get('');

        expect(element(by.id('habits')).isPresent()).toBe(true);
    });
});