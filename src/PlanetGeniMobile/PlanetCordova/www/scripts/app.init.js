//Copyright ©2013-2014 Memba® Sarl. All rights reserved.
/*jslint browser:true */
/*jshint browser:true */

/*******************************************************************************************
 * Application loader
 *******************************************************************************************/
(function () {

    "use strict";

    var fn = Function,
        global = fn('return this')(),
        Modernizr = global.Modernizr,
        DEBUG = true,
        MODULE = 'app.init.js: ';

    //SEE: https://github.com/SlexAxton/yepnope.js/issues/82
    function domReady() {
        jQuery(document).ready(function () {

            if (window.device !== undefined && window.device.cordova !== undefined) {
                //wait for the deviceready event in Phonegap
                // Common events are: `load`, `deviceready`, `offline`, and `online`.
                document.addEventListener('deviceready', global.app.onDeviceReady, false);
            } else {
                //no need to wait when debugging on a PC
                global.app.onDeviceReady();
            }
        });
    }

    Modernizr.load([

        {
            load: './Scripts/jquery-2.1.4.min.js',
            callback: function (url) { //called both in case of success and failure
                if (DEBUG && global.console) {
                    global.console.log(MODULE + url + ' loading attempt');
                }
            },
            complete: function () {
                if (!global.jQuery) {
                }
            }
        },
        //Bootstrap and font awesome
        {
            load: [
            'http://netdna.bootstrapcdn.com/bootstrap/3.0.3/css/bootstrap.min.css',
            'http://netdna.bootstrapcdn.com/bootstrap/3.0.3/js/bootstrap.min.js',
            'http://netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.min.css'
            ],
            callback: function (url) {
                if (DEBUG && global.console) {
                    global.console.log(MODULE + url + ' loading attempt');
                }
            }
        },
        //App libraries
        {
            test: (window.device !== undefined && window.device.cordova !== undefined), //test whether we run in phonegap
            //yep: [ //load minified uglyfied runtime scripts
            //],
            //nope: [ //load debug (non minified) scripts
            //],
            load: [
                './css/console.css',
                './Scripts/console.js',
                './Scripts/api.js',
                './Scripts/app.js'
            ],
            callback: function (url) {
                if (DEBUG && global.console) {
                    global.console.log(MODULE + url + ' loading attempt');
                }
            },
            complete: function () {
                domReady();
            }
        }
    ]);
}());