//Copyright ©2013-2014 Memba® Sarl. All rights reserved.
/*jslint browser:true*/
/*jshint browser:true*/

(function ($, undefined) {

    "use strict";

    var fn = Function,
        global = fn('return this')(),
        api = global.api,
        app = global.app = global.app || {};

    //See http://jqfundamentals.com/chapter/ajax-deferreds
    //Se also http://joseoncode.com/2011/09/26/a-walkthrough-jquery-deferred-and-promise/
    app.asyncLog = function (data) {
        var dfd = $.Deferred();
        debug.log('Result: ' + JSON.stringify(data));
        dfd.resolve(data);
        return dfd.promise();
    };

    app.asyncErr = function (data) {
        var dfd = $.Deferred();
        debug.log('Error: ' + JSON.stringify(data));
        dfd.resolve(data);
        return dfd.promise();
    };

    app.viewModel = ({
        userName: ko.observable('Alice'),
        password: ko.observable('password123'),
        hasRegistered: ko.observable(false),
        loginProvider: ko.observable(null),
        session: ko.observable(null),
        externalLogins: ko.observableArray([]),
        isLogged: function () {
            return (this.get('session') !== null);
        },
        hasExternalLogins: function () {
            return (app.viewModel.externalLogins().length !== 0)
        },
        register: function (e) {
            if (app.viewModel.get('loginProvider') === null) {
                debug.log('Register standard user ' + app.viewModel.get('userName'));
                $.when(api.register(app.viewModel.get('userName'), app.viewModel.get('password')))
                    .then(app.asyncLog, app.asyncErr);
            } else {
                debug.log('Register ' + app.viewModel.get('loginProvider') + ' user ' + app.viewModel.get('userName'));
                $.when(api.registerExternal(app.viewModel.get('userName')))
                    .then(app.asyncLog, app.asyncErr);
            }
        },
        signIn: function (e) {
            debug.log('Sign in');
            $.when(api.openSession(this.userName, this.password, true))
                .then(app.asyncLog, app.asyncErr)
                .then(function (data) {
                    if (data && data.access_token) {
                        app.viewModel.session(data);
                    }
                    if (app.viewModel.isLogged()) {
                        debug.log('New session started');
                    }
                    return data;
                });
        },
        signOut: function (e) {
            debug.log('Sign out');
            $.when(api.logOut())
               .then(app.asyncLog, app.asyncErr)
               .then(function (data) {
                   app.viewModel.session(null);
                   debug.log('Session ended');
                   return data;
               });
        },
        getUserInfo: function (e) {
            debug.log('Get user information');
            $.when(api.getUserInfo())
                .then(app.asyncLog, app.asyncErr)
                .then(function (data) {
                    app.viewModel.userName(data.UserName);
                    app.viewModel.hasRegistered(data.HasRegistered);
                    app.viewModel.loginProvider(data.LoginProvider);
                    if (data.LoginProvider !== null) {
                        app.viewModel.password(null)
                    }
                });
        },
        getValues: function (e) {
            debug.log('Get values');
            $.when(api.getValues())
                .then(app.asyncLog, app.asyncErr);
        },
        getExternalLogins: function (e) {
            debug.log('Get external logins with returnUrl = ' + window.location.protocol + '//' + window.location.host + window.location.pathname);
            //$.when(api.getExternalLogins('/', true))
            //Get rid of hash and querystring in returnUrl
            $.when(api.getExternalLogins(window.location.protocol + '//' + window.location.host + window.location.pathname, true))
                .then(app.asyncLog, app.asyncErr)
                .then(function (result) {
                    for (var i = 0; i < result.length; i++) {
                        app.viewModel.externalLogins.push(result[i]);
                    }
                });
        },
        gotoExternalLogin: function (data, event) {
            debug.log('External login');
            debug.log(data.Url);
            sessionStorage["state"] = data.State;
            sessionStorage["loginUrl"] = data.Url;
            api._util.archiveSessionStorageToLocalStorage();
            cordova.InAppBrowser.open(data.Url, '_blank', 'location=no, toolbar=no');
            //window.location.assign(e.url);
        },
        openInAppBrowser: function (e) {
            See: http://phonegap-tips.com/articles/google-api-oauth-with-phonegaps-inappbrowser.html
                var ref = window.open('http://apache.org', '_blank', 'location=no, toolbar=no'),
                    loadStartHandler = function () {
                        debug.log(event.url);
                    },
                    exitHandler = function () {
                        ref.removeEventListener('loadstart');
                        ref.removeEventListener('exit');
                    };
            ref.addEventListener('loadstart', loadStartHandler);
            ref.addEventListener('exit', exitHandler);
        }
    });

    app.onDeviceReady = function () {

        //TODO: If you’re sending text to the Visual Studio output window, you can format the text so that
        //you can open the source file just by double-clicking the line in the output window. Needs to look like this:
        //filename(linenumber): message
        window.onerror = function (errMsg, fileName, ln) {
            debug.log("Error : " + errMsg +
                    ", In file : " + fileName +
                    ", at line number:" + ln);
        };

        debug.log('document ready');

        if (window.location.hash.indexOf('#access_token=') === 0) {
            //TODO: verify sessionStorage["state"]
            debug.log('Found an access_token in the url');
            var session = api.parseExternalToken(window.location.hash);
            app.viewModel.session(session);
            app.viewModel.userName('?');
            app.viewModel.hasRegistered( true); //do not display the register button
            app.viewModel.loginProvider( null); //do not display the register button
        }

        if (window.device) {
            debug.log('This is a phonegap device: ' + window.device.model);
        } else {
            debug.log('This is a browser: ' + window.navigator.userAgent);
        }
        debug.log("{0} ===== {1}", $("form")[0], app.viewModel);
        ko.applyBindings(app.viewModel, $("form")[0]);
        debug.log('document bound to view model');

        debug.log('api located at ' + api.endPoints.root);

        //debugger;
        //following instructions at http://www.asp.net/web-api/overview/security/individual-accounts-in-web-api
        //and http://www.asp.net/web-api/overview/security/enabling-cross-origin-requests-in-web-api
        //$.when(api.getSession('Alice', 'password123'))
        //    .then(app.asyncLog, app.asyncErr)
        //    .then(api.getValues)
        //    .then(app.asyncLog, app.asyncErr);

    };

}(jQuery));


