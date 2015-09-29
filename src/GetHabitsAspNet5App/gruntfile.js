/// <binding ProjectOpened='default, start-services, karma-server-start, start-weblistener, protractor-server-start' />
/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {
    //загружаем плагины Grunt
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-karma');
    grunt.loadNpmTasks('grunt-exec');
    grunt.loadNpmTasks('grunt-contrib-jshint');
    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-bower-task');
    grunt.loadNpmTasks('grunt-protractor-webdriver');

    //настраиваем плагины
    grunt.initConfig({
        uglify: {
            scripts: {
                options: {
                    compress: false,
                    beautify: true,
                    mangle: false
                    //compress: true,
                    //beautify: false,
                    //mangle: true
                },
                files: { 'wwwroot/app.js': ['Scripts/app.js', 'Scripts/**/*.js', '!Scripts/**/*Tests.js'] }
            }
        },
        watch: {
            scripts: {
                files: ['Scripts/**/*.js', '!Scripts/**/*Tests.js'],
                tasks: ['uglify', 'jshint']
            },
            karma: {
                files: ['wwwroot/app.js', 'Scripts/**/*Tests.js', '!Scripts/**/*E2ETests.js'],
                tasks: ['karma:unit:run'] //NOTE the :run flag
            }
        },
        karma: {
            unit: {
                configFile: 'karma.conf.js',
                //background: true,
                singleRun: false
            }
        },
        exec: {
            webdriverStart: {
                cmd: 'webdriver-manager start'
            },
            protractorStart: {
                cmd: 'protractor protractor.conf.js'
            },
            webdriverUpdate: {
                cmd: 'webdriver-manager update'
            },
            webdriverUpdateStandalone: {
                cmd: 'webdriver-manager update --standalone'
            },
            startWebListener: {
                cmd: 'dnx . web'
            }
        },
        jshint: {
            files: ['wwwroot/app.js', 'Scripts/**/*.js', '!Scripts/**/*Tests.js'],
            options: {
                '-W069': false,
            }
        },
        bower: {
            install: {
                options: {
                    targetDir: "wwwroot/lib",
                    layout: "byComponent",
                    cleanTargetDir: false
                }
            }
        },
        protractor_webdriver: {
            tests: {
                files: ['Scripts/**/*E2ETests.js']
            }
        }
    });

    //настроим запуск задач
    grunt.registerTask('start-weblistener', ['exec:startWebListener']);
    grunt.registerTask('webdriver-update', ['exec:webdriverUpdate']);
    grunt.registerTask('webdriver-update-standalone', ['exec:webdriverUpdateStandalone']);
    grunt.registerTask('protractor-server-start', ['exec:webdriverStart']);
    grunt.registerTask('protractor-tests-start', ['protractor_webdriver:tests']);
    grunt.registerTask('protractor-console-tests-start', ['exec:protractorStart']);
    grunt.registerTask('karma-server-start', ['karma:unit:start']);
    grunt.registerTask('karma-tests-start', ['karma:unit:run']);
    grunt.registerTask('default', ['uglify', 'watch']);
    //grunt.registerTask('start-services', ['start-weblistener', 'protractor-server-start', 'karma-server-start']); // нужно сделать через конкаррент таскс
};