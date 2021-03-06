// Karma configuration
// Generated on Thu Sep 03 2015 17:59:17 GMT+0300 (RTZ 2 (зима))

module.exports = function(config) {
  config.set({

    // base path that will be used to resolve all patterns (eg. files, exclude)
    basePath: '',


    // frameworks to use
    // available frameworks: https://npmjs.org/browse/keyword/karma-adapter
    frameworks: ['jasmine', 'sinon'],


    // list of files / patterns to load in the browser
    files: [
      'wwwroot/lib/angular/angular.js',
      'wwwroot/lib/angular-resource/angular-resource.js',
      'wwwroot/lib/angular-route/angular-route.js',
      'wwwroot/lib/angular-mocks/angular-mocks.js',
      'wwwroot/app.js',
      'Scripts/**/*Tests.js'
    ],


    // list of files to exclude
    exclude: [
        'Scripts/**/*E2ETests.js'
    ],


    // preprocess matching files before serving them to the browser
    // available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
    preprocessors: {
    },


    // test results reporter to use
    // possible values: 'dots', 'progress'
    // available reporters: https://npmjs.org/browse/keyword/karma-reporter
    reporters: ['progress', 'junit'],


    // web server port
    port: 9876,


    // enable / disable colors in the output (reporters and logs)
    colors: true,


    // level of logging
    // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
    logLevel: config.LOG_DEBUG,


    // enable / disable watching file and executing tests whenever any file changes
    autoWatch: false,


    // start these browsers
    // available browser launchers: https://npmjs.org/browse/keyword/karma-launcher
    browsers: ['Chrome'],

    plugins: [
        'karma-chrome-launcher',
        'karma-jasmine',
        'karma-junit-reporter',
        'karma-sinon'
    ],

    // Continuous Integration mode
    // if true, Karma captures browsers, runs the tests and exits
    singleRun: false,

    junitReporter: {
        outputFile: 'unit.xml',
        suite: 'unit3'
    }
  })
}
