/// <binding ProjectOpened='default, karmaServerStart' />
/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {
    //загружаем плагины Grunt
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-karma');

    //настраиваем плагины
    grunt.initConfig({
        uglify: {
            scripts: {
                options: { 
                    compress: false,
                    beautify: true,
                    mangle: false
                },
                files: { 'wwwroot/app.js': ['Scripts/app.js', 'Scripts/**/*.js', '!Scripts/**/tests/*.js'] }
            }
        },
        watch: {
            scripts: {
                files: ['Scripts/**/*.js', '!Scripts/**/tests/*.js'],
                tasks: ['uglify']
            },
            karma: {
                files: ['Scripts/js/**/*.js', 'Scripts/**/tests/*.js'],
                tasks: ['karma:unit:run'] //NOTE the :run flag 
            }
        },
        karma: {
            unit: {
                configFile: 'karma.conf.js',
                //background: true,
                singleRun: false
            }
        }
    });

    //настроим запуск задач
    grunt.registerTask('karmaServerStart', ['karma:unit:start']);
    grunt.registerTask('default', ['uglify', 'watch']);
};