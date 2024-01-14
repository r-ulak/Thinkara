﻿//Copyright ©2013-2014 Memba® Sarl. All rights reserved.
/*jslint browser:true*/
/*jshint browser:true*/


; (function ($, undefined) {

    "use strict";

    var fn = Function,
        global = fn('return this')(),
        api = global.api = global.api || {},
        MODULE = 'api.js: ',
        DEBUG = true;

    api.endPoints = {
        root: 'https://thinkara.com:4430/',
        externalLogins: '/api/Account/ExternalLogins',
        token: '/Token',
        register: '/api/Account/Register',
        registerExternal: '/api/Account/RegisterExternal',
        logOut: '/api/Account/Logout',
        userInfo: '/api/Account/UserInfo',
        values: '/api/values'
    };

    if (DEBUG) {
        api.endPoints.root = 'https://thinkara.com:4430/';
    }

    api._util = {

        getSecurityHeaders: function () {
            var accessToken = sessionStorage["accessToken"] || localStorage["accessToken"];
            if (accessToken) {
                return { "Authorization": "Bearer " + accessToken };
            }
            return {};
        },

        clearAccessToken: function () {
            localStorage.removeItem("accessToken");
            sessionStorage.removeItem("accessToken");
        },

        setAccessToken: function (accessToken, persistent) {
            if (persistent) {
                localStorage["accessToken"] = accessToken;
            } else {
                sessionStorage["accessToken"] = accessToken;
            }
        },

        archiveSessionStorageToLocalStorage: function () {
            var backup = {};

            for (var i = 0; i < sessionStorage.length; i++) {
                backup[sessionStorage.key(i)] = sessionStorage[sessionStorage.key(i)];
            }

            localStorage["sessionStorageBackup"] = JSON.stringify(backup);
            sessionStorage.clear();
        },

        restoreSessionStorageFromLocalStorage: function () {
            var backupText = localStorage["sessionStorageBackup"],
                backup;

            if (backupText) {
                backup = JSON.parse(backupText);

                for (var key in backup) {
                    sessionStorage[key] = backup[key];
                }

                localStorage.removeItem("sessionStorageBackup");
            }
        }
    };

    //Use $.deferred as in http://jsfiddle.net/L96cD/
    //See http://jqfundamentals.com/chapter/ajax-deferreds
    //Se also http://joseoncode.com/2011/09/26/a-walkthrough-jquery-deferred-and-promise/

    api.getExternalLogins = function (returnUrl, generateState) {
        try {
            return $.ajax({
                url: api.endPoints.root + api.endPoints.externalLogins,
                type: 'GET',
                cache: false, //Adds a parameter _ with a timestamp to the query string
                data: {
                    returnUrl: returnUrl, //encodeURIComponent(returnUrl),
                    generateState: generateState ? 'true' : 'false'
                }
            });
        } catch (err) {
            var dfd = new $.Deferred();
            dfd.reject(err);
            return dfd.promise();
        }
    };

    api.register = function (userName, password) {
        try {
            return $.ajax({
                url: api.endPoints.root + api.endPoints.register,
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify({
                    UserName: userName,
                    Password: password,
                    ConfirmPassword: password
                }),
                type: 'POST'
            });
        } catch (err) {
            var dfd = new $.Deferred();
            dfd.reject(err);
            return dfd.promise();
        }
    };

    api.registerExternal = function (userName) {
        try {
            return $.ajax({
                url: api.endPoints.root + api.endPoints.registerExternal,
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify({
                    UserName: userName
                }),
                type: 'POST',
                xhrFields: { withCredentials: true },
                headers: api._util.getSecurityHeaders()
            });
        } catch (err) {
            var dfd = new $.Deferred();
            dfd.reject(err);
            return dfd.promise();
        }
    };


    api.openSession = function (userName, password, rememberMe) {
        //debugger;
        //following instructions at http://www.asp.net/web-api/overview/security/individual-accounts-in-web-api
        //and http://www.asp.net/web-api/overview/security/enabling-cross-origin-requests-in-web-api
        try {
            return $.ajax({
                url: api.endPoints.root + api.endPoints.token,
                contentType: 'application/x-www-form-urlencoded',
                data: {
                    grant_type: 'password',
                    username: userName,
                    password: password
                },
                type: 'POST'
            }).then(function (data) {
                api._util.setAccessToken(data.access_token, rememberMe);
                return data;
            });
        } catch (err) {
            var dfd = new $.Deferred();
            dfd.reject(err);
            return dfd.promise();
        }
    };

    api.parseExternalToken = function (hash) {
        //#access_token=-QikJ_IFEkQ54bamU0HsYdlx6Nrn8itee83yPtXZNapdik6dm4fZCVrKcwdLT5f4QCUPNVaCvPrK90bs-sC22yQIoBEkjYciFplQc1kYjxFWlg1oQFtDqnUmso8xIpNAaRnO7MAdO2AKV3PoBL-E8B51yxGNAGDJAX0oScE0OzU8dIcELbgf1bsCPomLch5WwY_92Z1s5v3Iybdyh7WxIkYdylU5lyVptfF3r86aH5edphrrLyTOyWyxNtmSkkwwfCQX3Enx6uQbi5siiabOGf7NfsHiGAWqR8prUcxot9YMnGjJnAZFBvwLhb-qjAhGuruOmIiB4zRiJ9j4qwH9KMWXikoQ7DL8FAuxUecQ2Jna5FsDeS6WaNTqPhEKnv0mgk-MzIr7rGubENuwRakkijaw1Lk8n7p4hQwXdqEzXjA&token_type=bearer&expires_in=1209600&state=MCHBpm1G-Et-rwB9appTxjrQ8oaIttV3kYUUmEyrr401
        var keyValues = hash.substr(1).split('&'),
            data = {};
        $.each(keyValues, function (index, keyValue) {
            var pos = keyValue.indexOf('=');
            if (pos > 0 && pos < keyValue.length - 1) {
                data[keyValue.substr(0, pos)] = keyValue.substr(pos + 1);
            }
        });
        //TODO: check state
        api._util.setAccessToken(data.access_token, true);
        return data;
    };

    /**
    * Api's that require being logged in (security headers)
    */

    api.getUserInfo = function () {
        try {
            return $.ajax({
                url: api.endPoints.root + api.endPoints.userInfo,
                type: 'GET',
                cache: false,
                xhrFields: { withCredentials: true },
                headers: api._util.getSecurityHeaders()
                /*
                beforeSend: function (xhr, settings) {
                    xhr.withCredentials = true;
                    xhr.setRequestHeader('Authorization', 'Bearer ' + session.access_token);
                }
                */
            });
        } catch (err) {
            var dfd = new $.Deferred();
            dfd.reject(err);
            return dfd.promise();
        }
    };

    api.getValues = function () {
        try {
            return $.ajax({
                url: api.endPoints.root + api.endPoints.values,
                type: 'GET',
                xhrFields: { withCredentials: true },
                headers: api._util.getSecurityHeaders()
                /*
                beforeSend: function (xhr, settings) {
                    xhr.withCredentials = true;
                    xhr.setRequestHeader('Authorization', 'Bearer ' + session.access_token);
                }
                */
            });
        } catch (err) {
            var dfd = new $.Deferred();
            dfd.reject(err);
            return dfd.promise();
        }
    };

    api.logOut = function () {
        try {
            return $.ajax({
                url: api.endPoints.root + api.endPoints.logOut,
                contentType: 'application/x-www-form-urlencoded',
                type: 'POST',
                xhrFields: { withCredentials: true },
                headers: api._util.getSecurityHeaders()
                /*
                beforeSend: function (xhr, settings) {
                    xhr.withCredentials = true;
                    xhr.setRequestHeader('Authorization', 'Bearer ' + session.access_token);
                }
                */
            }).then(function (data) {
                api._util.clearAccessToken();
                return data;
            });
        } catch (err) {
            var dfd = new $.Deferred();
            dfd.reject(err);
            return dfd.promise();
        }
    };

}(jQuery));
