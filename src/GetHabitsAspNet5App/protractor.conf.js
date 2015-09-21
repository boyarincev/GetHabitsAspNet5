exports.config = {
    framework: 'jasmine2',
    seleniumAddress: 'http://localhost:4444/wd/hub',
    baseUrl: 'http://localhost:5000/app/',
    specs: [
      'Scripts/**/*E2ETests.js'
    ],
    jasmineNodeOpts: {
        showColors: true, // Use colors in the command line report.
    }

};